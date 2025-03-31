using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DataAccess.Interface
{
    public interface ISubscriptionCategory
    {
        public Subscription? AddSubscription(Subscription sub);
        public Subscription? GetSubscription(int id);
        public ICollection<Subscription> GetSubscriptions(int userId);
        public bool DeleteSubscription(int id);
    }
}
