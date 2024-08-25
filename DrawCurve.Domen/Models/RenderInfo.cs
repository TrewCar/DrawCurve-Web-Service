using DrawCurve.Domen.Core.Menedger.Models;
using DrawCurve.Domen.Models.Core;
using DrawCurve.Domen.Models.Core.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DrawCurve.Domen.Models
{
    [Table("RenderInfo")]
    public class RenderInfo
    {
        [Key]
        public string KEY { get; set; }
        public string Name { get; set; }

        [ForeignKey(nameof(User))]
        public int AuthorId { get; set; }

        public RenderType Type { get; set; }
        public TypeStatus Status { get; set; }

        [JsonIgnore]
        [Column("Objects")]
        public string ObjectsJSON
        {
            get
            {

                return JsonSerializer.Serialize(Objects);
            }
            set
            {
                Objects = JsonSerializer.Deserialize<List<ObjectRender>>(value);
            }
        }

        [JsonIgnore]
        [Column("RenderConfig")]
        public string RenderConfigJSON
        {
            get => JsonSerializer.Serialize(RenderConfig);
            set => RenderConfig = JsonSerializer.Deserialize<RenderConfig>(value);
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
