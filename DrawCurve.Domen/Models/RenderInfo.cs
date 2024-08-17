using DrawCurve.Domen.Core.Menedger.Models;
using DrawCurve.Domen.Models.Core;
using DrawCurve.Domen.Models.Core.Objects;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrawCurve.Domen.Models
{
    [Table("RenderInfo")]
    public class RenderInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int AuthorId { get; set; }
        public TypeStatus Status { get; set; }

        [Column("Objects")]
        public string ObjectsJSON
        {
            get => JsonConvert.SerializeObject(Objects);
            set => Objects = JsonConvert.DeserializeObject<List<ObjectRender>>(value);
        }

        [Column("RenderConfig")]
        public string RenderConfigJSON
        {
            get => JsonConvert.SerializeObject(RenderConfig);
            set => RenderConfig = JsonConvert.DeserializeObject<RenderConfig>(value);
        }

        public DateTime DateCreate { get; set; }

        [NotMapped]
        public List<ObjectRender> Objects { get; set; }

        [NotMapped]
        public RenderConfig RenderConfig { get; set; }

        // Навигационное свойство
        public User User { get; set; }
    }
}
