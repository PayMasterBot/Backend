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
    public class Subscribe
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [ForeignKey("Id"), JsonIgnore]
        public virtual User User { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int Price { get; set; }
    }
}
