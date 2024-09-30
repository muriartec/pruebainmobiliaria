using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsDotnetCsharp.Models.Request
{
    public class MantInmuebleRequest : UserBase
    {
        public int cinmueble { get; set; }
        public int tipo_inmueble { get; set; }
        public string vendedor { get; set; }
        public int cant_habitaciones { get; set; }
        public bool flag_disponible { get; set; }
        public bool flag_mascota { get; set; }
    }
}
