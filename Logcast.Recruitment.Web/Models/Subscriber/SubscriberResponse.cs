using System;

namespace Logcast.Recruitment.Web.Models.Subscriber
{
    public class SubscriberResponse
    {
        public SubscriberResponse(Shared.Models.SubscriberModel subscriberModel)
        {
            Name = subscriberModel.Name;
            UserId = subscriberModel.Id;
        }

        public string Name { get; set; }
        public Guid UserId { get; set; }
    }
}