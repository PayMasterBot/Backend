using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Model
{
    public class ExhangeRateSubscription
    {
        [Key]
        public int UserId { get; set; }
        [ForeignKey("Id"), JsonIgnore]
        public User User { get; set; }
        [Key]
        public string Currency1 { get; set; }
        [Key]
        public string Currency2 { get; set; }
    }
}
