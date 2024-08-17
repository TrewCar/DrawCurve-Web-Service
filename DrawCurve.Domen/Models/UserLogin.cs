using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DrawCurve.Domen.Models
{
    [Table("UserLogin")]
    public class UserLogin
    {
        [Key]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        // Навигационное свойство
        public User User { get; set; }
    }
}
