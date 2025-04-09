using DataAccess.Interface;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class PgCategoryRepository : ICategoryRepository
    {
        private PgPayContext _ctx;

        public PgCategoryRepository(PgPayContext ctx)
        {
            _ctx = ctx;
        }

        public ExpenceCategory? AddCategory(ExpenceCategory category, User user)
        {
            ExpenceCategory? res = null;
            try
            {
                res = _ctx.ExpenceCategories.Find(category.Id);
                if (res is null)
                {
                    res = category;
                    _ctx.ExpenceCategories.Add(res);
                }
                if (res.Users.FirstOrDefault(u => u.Id == user.Id) is null)
                {
                    res.Users.Add(user);
                }
                _ctx.SaveChanges();
            }
            catch
            {
                return null;
            }
            return res;
        }

        public Expence? AddExpense(Expence expence)
        {
            try
            {
                _ctx.Expences.Add(expence);
                _ctx.SaveChanges();
            }
            catch
            {
                return null;
            }
            return expence;
        }

        public bool DeleteCategory(int id, int userId)
        {
            try
            {
                var cat = _ctx.ExpenceCategories.Find(id);
                var user = _ctx.Users.Find(userId);
                if (cat != null && user != null)
                {
                    user.ExpenceCategories.Remove(cat);
                    _ctx.SaveChanges();
                    return true;
                }
            }
            catch { }
            return false;
        }

        public ICollection<ExpenceCategory> GetCategories(int userId)
        {
            try
            {
                var user = _ctx.Users.Find(userId);
                if (user != null)
                {
                    return user.ExpenceCategories.AsEnumerable().ToList();
                }
            }
            catch { }
            return new List<ExpenceCategory>();
        }

        public ExpenceCategory? GetCategory(int id, int userId)
        {
            try
            {
                var user = _ctx.Users.Find(userId);
                if (user != null)
                {
                    return user.ExpenceCategories.FirstOrDefault(c => c.Id == id, null);
                }
            }
            catch { }
            return null;
        }

        public JsonObject? GetReport(int userId)
        {
            try
            {
                var user = _ctx.Users.Find(userId);
                if (user != null)
                {
                    var report = new JsonObject();
                    foreach (var cat in user.ExpenceCategories.AsEnumerable())
                    {
                        var categoryData = new JsonObject();
                        int s_cur = GetMonthSum(cat.Id, userId, true);
                        int s_prev = GetMonthSum(cat.Id, userId, false);
                        categoryData.Add("cur_month", s_cur);
                        categoryData.Add("prev_month", s_prev);
                        report.Add(cat.Title, categoryData);
                    }
                    return report.AsObject();
                }
            }
            catch { }
            return null;
        }

        private int GetMonthSum(int catId, int userId, bool current)
        {
            var dt = DateTime.UtcNow;
            if (!current)
            {
                dt = dt.AddMonths(-1);
            }
            var exp = _ctx.Expences.Where(e => e.CategoryId == catId && e.UserId == userId && e.Date.Month == dt.Month)
                .AsEnumerable();
            return exp.Sum(v => v.Price);
        }
    }
}
