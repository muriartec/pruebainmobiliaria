using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsDotnetCsharp.Models.Response
{
    public class ListInmueblesResponse
    {
        public ListInmueblesResponse()
        {
            data = new List<InmuebleBE>();
        }
        public int codigo { get; set; }
        public string mensaje { get; set; }
        public List<InmuebleBE> data { get; set; }
    }
    public class InmuebleBE
    {
        public int cinmueble { get; set; }
        public int ctipo_inmueble { get; set; }
        public string vendedor { get; set; }
        public int cant_habitaciones { get; set; }
        public bool flag_disponible { get; set; }
        public bool flag_mascota { get; set; }
        public bool eliminado { get; set; }
    }
}
