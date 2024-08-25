using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DrawCurve.Domen.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id {  get; set; }
        [Required]
        public string Name { get; set; }
        public Role Role { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
