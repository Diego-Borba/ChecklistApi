using System.Text.Json.Serialization;

namespace ChecklistApi.Models
{
    public class ChecklistItem
    {
        public int Id { get; set; }
        public int ChecklistId { get; set; }
        public string Descricao { get; set; }
        public bool Concluido { get; set; }
        public string Tipo { get; set; } // "Implantacao" ou "PreVirada"

        [JsonIgnore]        
        public Checklist Checklist { get; set; }
    }
}