using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Model
{
    public class Expence
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [JsonIgnore]
        public virtual ExpenceCategory Category { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
