using System;
using System.Collections.Generic;

namespace TriagemCaminhao.Data;

public partial class Caminhoes
{
    public int Id { get; set; }

    public string NomeTransportadora { get; set; } = null!;

    public string Whatsapp { get; set; } = null!;

    public virtual ICollection<Triagem> Triagems { get; set; } = new List<Triagem>();

}
