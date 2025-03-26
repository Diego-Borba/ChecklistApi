using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChecklistApi.Models
{
    public class Checklist
    {
        public int Id { get; set; }
        public string Cnpj { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataAtualizacao { get; set; }
        
        // Módulos adquiridos
        public bool Web { get; set; }
        public bool Datacash { get; set; }
        public bool Bmpdv { get; set; }
        public bool Nfce { get; set; }
        public bool Dashboard { get; set; }
        public bool Bmlink { get; set; }
        public bool Corefood { get; set; }
        public bool BmpdvStone { get; set; }
        public bool BmpdvRede { get; set; }
        
        // Itens do checklist
        [JsonIgnore]
        public List<ChecklistItem> Itens { get; set; } = new List<ChecklistItem>();
    }
}