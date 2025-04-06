using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class ExpenceDto
    {
        public int UserId { get; set; }
        public int CatId { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public string Title { get; set; }

        public static implicit operator Expence(ExpenceDto dto)
        {
            return new Expence
            {
                UserId = dto.UserId,
                CategoryId = dto.CatId,
                Date = dto.Date,
                Price = dto.Price,
                Title = dto.Title
            };
        }


        public static implicit operator ExpenceDto(Expence orig)
        {
            return new ExpenceDto
            {
                UserId = orig.UserId,
                CatId = orig.CategoryId,
                Date = orig.Date,
                Price = orig.Price,
                Title = orig.Title
            };
        }
    }
}
