using System;
using System.Collections.Generic;
using TriagemCaminhaoAPI.Data;
using TriagemCaminhaoAPI.Dto;

namespace TriagemCaminhao.Data;

public partial class Triagem
{
    public int Id { get; set; }

    public int CaminhaoId { get; set; }

    public int? DocaId { get; set; }

    public int StatusId { get; set; }
    public int PrioridadeID { get; set; }

    public DateTime DataChegada { get; set; }

    public DateTime? DataAtendimento { get; set; }

    public virtual Caminhoes Caminhao { get; set; } = null!;

    public virtual Doca? Doca { get; set; }

    public virtual StatusTriagem StatusTriagem { get; set; } = null!;
    public virtual PrioridadeTriagem PrioridadeTriagem { get; set; } = null!;
}
