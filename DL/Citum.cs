using System;
using System.Collections.Generic;

namespace DL;

public partial class Citum
{
    public int IdCita { get; set; }

    public string NombreCliente { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public TimeSpan Hora { get; set; }

    public int DuracionMinutos { get; set; }

    public string Estatus { get; set; } = null!;
}
