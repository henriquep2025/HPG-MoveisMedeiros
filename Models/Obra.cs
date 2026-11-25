namespace ProjetoMoveisMedeiros.Api.Models
{
    public class Obra
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public string Endereco { get; set; }
        public string NomeProjeto { get; set; }
        public string Status { get; set; }
        public DateTime? DataPrevistaInicio { get; set; }
        public DateTime? DataEstimadaEntrega { get; set; }
        public string AmbientesContratados { get; set; }
        public string MateriaisPrevistos { get; set; }
        public decimal Preco { get; set; }

        // Relacionamento com os itens do checklist
        public List<ItemChecklistObra> Checklist { get; set; } = new();
    }

    public class ItemChecklistObra
    {
        public int Id { get; set; }
        public int ObraId { get; set; }

        public int Ordem { get; set; } // 1, 2, 3, 4, 5
        public string Titulo { get; set; } // Ex: "Chapa e Substrato"
        public string Descricao { get; set; } // Ex: "Inspeção de empenamento, espessura..."

        // Status: "PENDENTE" (inicial), "OK", "NOK", "NA"
        public string Status { get; set; } = "PENDENTE";

        // Obrigatórios apenas quando Status == "NOK"
        public string? Observacao { get; set; }
        public string? FotoUrl { get; set; } // Caminho ou Base64 da foto anexada
    }
}
