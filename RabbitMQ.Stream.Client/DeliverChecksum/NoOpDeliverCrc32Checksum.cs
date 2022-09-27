// This source code is dual-licensed under the Apache License, version
// 2.0, and the Mozilla Public License, version 2.0.
// Copyright (c) 2007-2020 VMware, Inc.

namespace RabbitMQ.Stream.Client.DeliverChecksum
{
    internal sealed class NoOpDeliverCrc32Checksum : IDeliverCrc32Checksum
    {
        private static readonly DeliverCrc32ChecksumResult s_result = new(true, 0, 0);
        
        DeliverCrc32ChecksumResult IDeliverCrc32Checksum.Check(Deliver deliver)
        {
            return s_result;
        }
    }
}
