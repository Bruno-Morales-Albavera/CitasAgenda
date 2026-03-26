using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Cita
    {
        public int IdCita { get; set; }

        public string NombreCliente { get; set; } = null!;

        public DateTime Fecha { get; set; }  
        public TimeSpan Hora { get; set; }   

        public int DuracionMinutos { get; set; }

        public string Estatus { get; set; } = null!;
        public List<Cita>? Citas { get; set; }
    }
}
