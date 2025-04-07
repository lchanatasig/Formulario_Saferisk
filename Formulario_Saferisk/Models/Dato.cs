using System;
using System.Collections.Generic;

namespace Formulario_Saferisk.Models;

public partial class Dato
{
    public int Id { get; set; }

    public string NombresCompletos { get; set; } = null!;

    public string Perfil { get; set; } = null!;

    public string Broker { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public DateTime? FechaRegistro { get; set; }

    public string? Ciudad { get; set; }
}
