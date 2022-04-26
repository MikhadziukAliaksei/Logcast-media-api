using System;

namespace Logcast.Recruitment.Web.Models.Subscriber
{
    public class NewSubscriberResponse
    {
        public NewSubscriberResponse(Guid subscriberId)
        {
            SubscriberId = subscriberId;
        }

        public Guid SubscriberId { get; set; }
    }
}