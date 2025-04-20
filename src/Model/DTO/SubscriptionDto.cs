using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class SubscriptionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Price { get; set; }
        public string Title { get; set; }

        public static implicit operator Subscription(SubscriptionDto dto)
        {
            return new Subscription
            {
                Id = dto.Id,
                UserId = dto.UserId,
                Price = dto.Price,
                Title = dto.Title
            };
        }


        public static implicit operator SubscriptionDto(Subscription orig)
        {
            return new SubscriptionDto
            {
                Id = orig.Id,
                UserId = orig.UserId,
                Price = orig.Price,
                Title = orig.Title
            };
        }
    }
}
