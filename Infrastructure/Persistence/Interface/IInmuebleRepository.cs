using Amazon.Lambda.Core;
using AwsDotnetCsharp.Models.Request;
using AwsDotnetCsharp.Models.Response;
using AwsDotnetCsharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsDotnetCsharp.Infrastructure.Persistence.Interface
{
    public interface IInmuebleRepository
    {
        Task<ListInmueblesResponse> ListInmueble(ILambdaContext contextLambda, string vendedor);
        Task<ListTipoInmuebleResponse> ListTipoInmueble(ILambdaContext contextLambda);
        Task<SaveResult> MantInmuebles(ILambdaContext contextLambda, MantInmuebleRequest request);
    }
}
