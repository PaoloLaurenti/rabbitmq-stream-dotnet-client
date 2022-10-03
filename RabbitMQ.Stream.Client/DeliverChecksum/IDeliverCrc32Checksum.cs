// This source code is dual-licensed under the Apache License, version
// 2.0, and the Mozilla Public License, version 2.0.
// Copyright (c) 2007-2020 VMware, Inc.

using System.Buffers;

namespace RabbitMQ.Stream.Client.DeliverChecksum
{
    public interface IDeliverCrc32Checksum
    {
        internal bool Check(ReadOnlySequence<byte> data, uint dataLen, uint expectedCrc);
    }
}
