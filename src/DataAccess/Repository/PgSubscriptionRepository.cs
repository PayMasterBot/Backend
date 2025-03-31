using DataAccess.Interface;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class PgSubscriptionRepository : ISubscriptionCategory
    {
        private PgPayContext _ctx;

        public PgSubscriptionRepository(PgPayContext ctx)
        {
            _ctx = ctx;
        }

        public Subscription? AddSubscription(Subscription sub)
        {
            try
            {
                _ctx.Subscriptions.Add(sub);
                _ctx.SaveChanges();
                return sub;
            }
            catch { }
            return null;
        }

        public bool DeleteSubscription(int id)
        {
            try
            {
                var sub = _ctx.Subscriptions.Find(id);
                if (sub is null)
                {
                    return false;
                }
                _ctx.Subscriptions.Remove(sub);
                _ctx.SaveChanges();
                return true;
            }
            catch { }
            return false;
        }

        public Subscription? GetSubscription(int id)
        {
            try
            {
                return _ctx.Subscriptions.Find(id);
            }
            catch { }
            return null;
        }

        public ICollection<Subscription> GetSubscriptions(int userId)
        {
            try
            {
                return _ctx.Subscriptions.Where(s => s.UserId == userId).AsEnumerable().ToList();
            }
            catch { }
            return new List<Subscription>();
        }
    }
}
