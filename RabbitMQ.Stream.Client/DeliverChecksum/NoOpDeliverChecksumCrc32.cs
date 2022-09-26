// This source code is dual-licensed under the Apache License, version
// 2.0, and the Mozilla Public License, version 2.0.
// Copyright (c) 2007-2020 VMware, Inc.

namespace RabbitMQ.Stream.Client.DeliverChecksum
{
    internal sealed class NoOpDeliverChecksumCrc32 : IDeliverChecksumCrc32
    {
        DeliverChecksumCrc32Result IDeliverChecksumCrc32.Check(Deliver deliver)
        {
            return new DeliverChecksumCrc32Result(true, deliver.Chunk.Crc, deliver.Chunk.Crc);
        }
    }
}
