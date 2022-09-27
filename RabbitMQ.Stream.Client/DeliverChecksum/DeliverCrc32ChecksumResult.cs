// This source code is dual-licensed under the Apache License, version
// 2.0, and the Mozilla Public License, version 2.0.
// Copyright (c) 2007-2020 VMware, Inc.

namespace RabbitMQ.Stream.Client.DeliverChecksum
{
    public readonly struct DeliverCrc32ChecksumResult
    {
        public DeliverCrc32ChecksumResult(bool isOK, int computedChecksum, int expectedChecksum)
        {
            IsOK = isOK;
            ComputedChecksum = computedChecksum;
            ExpectedChecksum = expectedChecksum;
        }

        internal bool IsOK { get; }
        internal int ComputedChecksum { get; }
        internal int ExpectedChecksum { get; }
    }
}
