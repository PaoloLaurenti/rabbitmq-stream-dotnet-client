using System;
using System.Buffers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RabbitMQ.Stream.Client
{
    public struct MessageContext
    {
        public ulong Offset { get; }

        public TimeSpan Timestamp { get; }

        public MessageContext(ulong offset, TimeSpan timestamp)
        {
            this.Offset = offset;
            this.Timestamp = timestamp;
        }
    }

    public record ConsumerConfig
    {
        public string Stream { get; set; }
        public string Reference { get; set; }
        public Func<Consumer, MessageContext, Message, Task> MessageHandler { get; set; }
        public IOffsetType OffsetSpec { get; set; } = new OffsetTypeNext();
    }

    public class Consumer : IDisposable
    {
        private readonly Client client;
        private readonly ConsumerConfig config;
        private byte subscriberId;
        private bool _disposed;

        private Consumer(Client client, ConsumerConfig config)
        {
            this.client = client;
            this.config = config;
        }

        public void Commit(ulong offset)
        {
        }

        public static async Task<Consumer> Create(ClientParameters clientParameters, ConsumerConfig config)
        {
            var client = await Client.Create(clientParameters);
            var consumer = new Consumer(client, config);
            await consumer.Init();
            return consumer;
        }

        private async Task Init()
        {
            ushort initialCredit = 2;
            var (consumerId, response) = await client.Subscribe(
                config.Stream,
                config.OffsetSpec, initialCredit,
                new Dictionary<string, string>(),
                async deliver =>
                {
                    foreach (var messageEntry in deliver.Messages)
                    {
                        var message = Message.From(messageEntry.Data); //data should be AMQP 1.0 encoded
                        await config.MessageHandler(this,
                            new MessageContext(messageEntry.Offset,
                                TimeSpan.FromMilliseconds(deliver.Chunk.Timestamp)),
                            message);
                    }

                    // give one credit after each chunk
                    await client.Credit(deliver.SubscriptionId, 1);
                });
            if (response.ResponseCode == ResponseCode.Ok)
            {
                this.subscriberId = consumerId;
                return;
            }

            throw new CreateConsumerException($"consumer could not be created code: {response.ResponseCode}");
        }

        public async Task<ResponseCode> Close()
        {
            if (_disposed)
                return ResponseCode.Ok;

            var deleteConsumerResponse = await this.client.Unsubscribe(this.subscriberId);
            var result = deleteConsumerResponse.ResponseCode;
            var closed = this.client.MaybeClose($"client-close-subscriber: {this.subscriberId}");
            if (closed.ResponseCode != ResponseCode.Ok)
            {
                // TODO replace with new logger
                Console.WriteLine($"Error during close tcp connection. Subscriber: {this.subscriberId}");
            }

            _disposed = true;
            return result;
        }

        //
        private void Dispose(bool disposing)
        {
            if (!disposing) return;

            var closeProducer = this.Close();
            if (closeProducer.Result != ResponseCode.Ok)
            {
                // TODO replace with new logger
                Console.WriteLine($"Error during remove producer. Subscriber: {this.subscriberId}");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}