using AwsDotnetCsharp.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsDotnetCsharp.Domain.Services
{
    public class InmuebleDomain : IInmuebleDomain
    {
        /// <summary>
        /// Obtener key de otro servicio externo
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetSecretKeyToken()
        {
            return "S3cr3t_K3y!.123_S3cr3t_K3y!.1234";
        }
    }
}
