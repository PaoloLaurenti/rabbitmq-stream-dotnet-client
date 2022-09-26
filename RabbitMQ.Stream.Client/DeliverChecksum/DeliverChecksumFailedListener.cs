// This source code is dual-licensed under the Apache License, version
// 2.0, and the Mozilla Public License, version 2.0.
// Copyright (c) 2007-2020 VMware, Inc.

using System.Threading.Tasks;

namespace RabbitMQ.Stream.Client.DeliverChecksum
{
    internal abstract class DeliverChecksumFailedListener
    {
        protected DeliverChecksumFailedListener()
        {
        }

        internal abstract Task Notify<T>(Deliver deliver, T computedChecksum, T expectedChecksum);
    }
}
