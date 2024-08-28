using System;
using System.Collections.Generic;

namespace TriagemCaminhao.Data;

public partial class StatusTriagem
{
    public int Id { get; set; }

    public string Status { get; set; } = null!;

    public virtual Triagem Triagem { get; set; } = null!;

}
