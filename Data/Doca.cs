using System;
using System.Collections.Generic;

namespace TriagemCaminhao.Data;

public partial class Doca
{
    public int Id { get; set; }

    public string NomeDoca { get; set; } = null!;

    public string StatusDoca { get; set; } = null!;

    public virtual ICollection<Triagem> Triagems { get; set; } = new List<Triagem>();
}
