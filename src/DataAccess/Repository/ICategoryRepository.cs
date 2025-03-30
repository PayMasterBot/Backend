using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface ICategoryRepository
    {
        public ExpenceCategory? AddCategory(ExpenceCategory category, User user);
        public ExpenceCategory? GetCategory(int id, int userId);
        public ICollection<ExpenceCategory> GetCategories(int userId);
        public bool DeleteCategory(int id, int userId);
        public Expence? AddExpense(Expence expence);
        public JsonObject? GetReport(int userId);
    }
}
