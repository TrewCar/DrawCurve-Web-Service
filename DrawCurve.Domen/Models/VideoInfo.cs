using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DrawCurve.Domen.Models
{
    public class VideoInfo
    {
        [Key]
        [ForeignKey(nameof(RenderInfo))]
        public string RenderCnfId { get; set; }

        [ForeignKey(nameof(User))]
        public int AuthorId {  get; set; }

        public string Name {  get; set; }
        public string Description { get; set; }
        public int Time {  get; set; }
        public DateTime DatePublish { get; set; } = DateTime.Now;

        // Навигационное свойство
        public User User { get; set; }
        [JsonIgnore]
        public RenderInfo RenderInfo { get; set; }
    }
}
