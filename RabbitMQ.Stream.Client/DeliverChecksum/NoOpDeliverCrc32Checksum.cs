// This source code is dual-licensed under the Apache License, version
// 2.0, and the Mozilla Public License, version 2.0.
// Copyright (c) 2007-2020 VMware, Inc.

using System.Buffers;

namespace RabbitMQ.Stream.Client.DeliverChecksum
{
    internal sealed class NoOpDeliverCrc32Checksum : IDeliverCrc32Checksum
    {
        bool IDeliverCrc32Checksum.Check(ReadOnlySequence<byte> data, uint dataLen, uint expectedCrc)
        {
            return true;
        }
    }
}
