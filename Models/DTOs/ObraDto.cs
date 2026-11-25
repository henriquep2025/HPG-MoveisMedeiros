using System.ComponentModel.DataAnnotations;

namespace ProjetoMoveisMedeiros.Api.Models.DTOs
{
    public class ObraDto
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNome { get; set; }
        public string NomeProjeto { get; set; }
        public string Endereco { get; set; }
        public string Status { get; set; }
        public DateTime? DataPrevistaInicio { get; set; }
        public DateTime? DataEstimadaEntrega { get; set; }
        public string AmbientesContratados { get; set; }
        public string MateriaisPrevistos { get; set; }
        public decimal Preco { get; set; }

        // Lista dos itens do checklist retornada junto com a obra
        public List<ItemChecklistObraDto> Checklist { get; set; } = new();
    }

    public class CreateObraDto
    {
        public int ClienteId { get; set; }
        public string NomeProjeto { get; set; } // Ex: Residência Alphaville - Suíte Master & Living
        public string Endereco { get; set; }
        public string Status { get; set; } // "EM_ANDAMENTO" ou "CONCLUIDA"
        public DateTime? DataPrevistaInicio { get; set; }
        public DateTime? DataEstimadaEntrega { get; set; }
        public string AmbientesContratados { get; set; }
        public string MateriaisPrevistos { get; set; }
        public decimal Preco { get; set; }
    }

    public class ItemChecklistObraDto
    {
        public int Id { get; set; }
        public int ObraId { get; set; }

        public int Ordem { get; set; } // 1, 2, 3, 4, 5
        public string Titulo { get; set; } // Ex: "Chapa e Substrato"
        public string Descricao { get; set; } // Ex: "Inspeção de empenamento, espessura..."

        // Status: "PENDENTE" (inicial), "OK", "NOK", "NA"
        public string Status { get; set; } = "OK";

        // Obrigatórios apenas quando Status == "NOK"
        public string? Observacao { get; set; }
        public string? FotoUrl { get; set; } // Caminho ou Base64 da foto anexada
    }

    public class CreateItemChecklistObraDto
    {
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