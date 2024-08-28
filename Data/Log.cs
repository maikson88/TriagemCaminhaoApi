using System;
using System.Collections.Generic;

namespace TriagemCaminhao.Data;

public partial class Log
{
    public int Id { get; set; }

    public int? CaminhaoId { get; set; }

    public string Acao { get; set; } = null!;

    public DateTime DataHora { get; set; }

    public string? Usuario { get; set; }

    public string? Detalhes { get; set; }

    public virtual Caminhoes? Caminhao { get; set; }
}
