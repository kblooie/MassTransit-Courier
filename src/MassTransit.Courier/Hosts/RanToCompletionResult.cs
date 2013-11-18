// Copyright 2007-2013 Chris Patterson
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.Courier.Hosts
{
    using System;
    using System.Collections.Generic;
    using Contracts;
    using InternalMessages;


    class RanToCompletionResult :
        ExecutionResult
    {
        readonly string _activityName;
        readonly Guid _activityTrackingNumber;
        readonly IMessagingAdaptor _messagingAdaptor;
        readonly IDictionary<string, object> _results;
        readonly RoutingSlip _routingSlip;
        readonly DateTime _timestamp;

        public RanToCompletionResult(IMessagingAdaptor messagingAdaptor, RoutingSlip routingSlip, string activityName,
            Guid activityTrackingNumber, IDictionary<string, object> results)
        {
            _timestamp = DateTime.UtcNow;
            _routingSlip = routingSlip;
            _activityName = activityName;
            _activityTrackingNumber = activityTrackingNumber;
            _results = results;
            _messagingAdaptor = messagingAdaptor;
        }

        public DateTime Timestamp
        {
            get { return _timestamp; }
        }

        public void Evaluate()
        {
            _messagingAdaptor.Publish<RoutingSlipActivityCompleted>(
                new RoutingSlipActivityCompletedMessage(_routingSlip.TrackingNumber, _activityName,
                    _activityTrackingNumber, _timestamp, _results, _routingSlip.Variables));

            _messagingAdaptor.Publish<RoutingSlipCompleted>(new RoutingSlipCompletedMessage(_routingSlip.TrackingNumber, _timestamp,
                _routingSlip.Variables));
        }
    }
}