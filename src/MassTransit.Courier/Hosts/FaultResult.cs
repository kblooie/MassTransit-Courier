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
    using Contracts;
    using InternalMessages;


    class FaultResult :
        ExecutionResult
    {
        readonly Activity _activity;
        readonly Guid _activityTrackingNumber;
        readonly IMessagingAdaptor _messagingAdaptor;
        readonly Exception _exception;
        readonly DateTime _timestamp;
        readonly Guid _trackingNumber;

        public FaultResult(IMessagingAdaptor messagingAdaptor, Guid trackingNumber, Activity activity, Guid activityTrackingNumber,
            Exception exception)
        {
            _timestamp = DateTime.UtcNow;
            _messagingAdaptor = messagingAdaptor;
            _trackingNumber = trackingNumber;
            _activity = activity;
            _activityTrackingNumber = activityTrackingNumber;
            _exception = exception;
        }

        public DateTime Timestamp
        {
            get { return _timestamp; }
        }

        public void Evaluate()
        {
            var activityFaulted = new RoutingSlipActivityFaultedMessage(_trackingNumber, _timestamp, _activity.Name,
                _activityTrackingNumber, _exception);
            
            _messagingAdaptor.Publish<RoutingSlipActivityFaulted>(activityFaulted);

            Uri hostAddress = _messagingAdaptor.GetCurrentHostAddress();

            var activityExceptionInfo = new ActivityExceptionImpl(_activity.Name, hostAddress,
                _activityTrackingNumber, _timestamp, _exception);

            var routingSlipFaulted = new RoutingSlipFaultedMessage(_trackingNumber, _timestamp, activityExceptionInfo);
            _messagingAdaptor.Publish<RoutingSlipFaulted>(routingSlipFaulted);
        }
    }
}