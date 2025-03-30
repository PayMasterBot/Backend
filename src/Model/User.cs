using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [AllowNull]
        public string? Token { get; set; }
        [JsonIgnore]
        public virtual ICollection<ExpenceCategory> ExpenceCategories { get; set; }
    }
}
