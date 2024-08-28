using TriagemCaminhao.Data;

namespace TriagemCaminhaoAPI.Dto
{
    public class TriagemDto
    {
        public int Id { get; set; }

        public virtual CaminhoesDto Caminhao { get; set; } = null!;

        public virtual DocaDto? Doca { get; set; }

        public virtual StatusTriagemDto StatusTriagem { get; set; } = null!;
        public virtual PrioridadeTriagemDto PrioridadeTriagem { get; set; } = null!;
        public DateTime DataChegada { get; set; }

        public DateTime? DataAtendimento { get; set; }
    }
}
