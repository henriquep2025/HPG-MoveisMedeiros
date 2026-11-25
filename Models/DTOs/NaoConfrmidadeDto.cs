namespace ProjetoMoveisMedeiros.Api.Models.DTOs
{
    public class NaoConformidadeDto
    {
        public int Id { get; set; }
        public string PontoIdentificado { get; set; } // Ex: "Cozinha Gourmet • Torre de Embutir Nicho Forno"
        public string DescricaoOcorrencia { get; set; } // Descrição Detalhada da Ocorrência
        public string AcaoCorretiva { get; set; } // Ação Corretiva Imediata Recomendada
        public string FotosUrls { get; set; } // Caminhos das fotos obrigatórias (separados por ";" ou JSON)
        public string Status { get; set; } = "ABERTA"; // "ABERTA", "EM_ANALISE", "RESOLVIDA"
        public string GrauSeveridade { get; set; } = "LEVE"; // "LEVE", "MÉDIA", "CRÍTICA"
        public int ObraId { get; set; }
        public string ObraNome { get; set; } // Apenas o nome do projeto da obra
    }

    public class CreateNaoConformidadeDto
    {
        public string PontoIdentificado { get; set; }
        public string DescricaoOcorrencia { get; set; }
        public string AcaoCorretiva { get; set; }
        public string FotosUrls { get; set; }
        public string Status { get; set; } = "ABERTA";
        public string GrauSeveridade { get; set; } = "LEVE";
        public int ObraId { get; set; }
    }
}