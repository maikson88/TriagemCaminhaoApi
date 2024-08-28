using TriagemCaminhao.Data;

namespace TriagemCaminhaoAPI.Data
{
    public partial class PrioridadeTriagem
    {
        public int Id { get; set; }
        public bool IsPrioridade { get; set; }
        public string? Volume { get; set; }
        public string? Peso { get; set; }

        public virtual Triagem Triagem { get; set; } = null!;
    }
}
