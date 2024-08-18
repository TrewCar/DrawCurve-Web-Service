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
    public class VideoInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID {  get; set; }
        [ForeignKey(nameof(User))]
        public int AuthorId {  get; set; }
        [ForeignKey(nameof(RenderInfo))]
        public string RenderCnfId { get; set; }
        public string Name {  get; set; }
        public string Description { get; set; }
        public int Time {  get; set; }

        // Навигационное свойство
        public User User { get; set; }
        public RenderInfo RenderInfo { get; set; }
    }
}
