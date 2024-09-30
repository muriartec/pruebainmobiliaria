using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsDotnetCsharp.Models.Response
{
    public class ListTipoInmuebleResponse
    {
        public ListTipoInmuebleResponse()
        {
            data = new List<TipoInmuebleBE>();
        }
        public int codigo { get; set; }
        public string mensaje { get; set; }
        public List<TipoInmuebleBE> data { get; set; }
    }
    public class TipoInmuebleBE
    {
        public int ctipo_inmueble { get; set; }
        public string descripcion { get; set; }
    }
}
