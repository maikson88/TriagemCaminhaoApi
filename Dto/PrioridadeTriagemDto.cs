namespace TriagemCaminhaoAPI.Dto
{
    public class PrioridadeTriagemDto
    {
        public int Id { get; set; }
        public bool IsPrioridade { get; set; } = false;
        public string? Volume { get; set; }
        public string? Peso { get; set; }
    }
}
