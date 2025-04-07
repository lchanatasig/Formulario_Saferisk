using System;
using System.Collections.Generic;

namespace Formulario_Saferisk.Models;

public partial class Broker
{
    public int BrokerId { get; set; }

    public string CodAuto { get; set; } = null!;

    public string Categoria { get; set; } = null!;

    public string Descripcion { get; set; } = null!;
}
