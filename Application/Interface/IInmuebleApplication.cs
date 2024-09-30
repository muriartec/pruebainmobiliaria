using Amazon.Lambda.Core;
using AwsDotnetCsharp.Models.Request;
using AwsDotnetCsharp.Models.Response;
using AwsDotnetCsharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsDotnetCsharp.Application.Interface
{
    public interface IInmuebleApplication
    {
        Task<GenerateTokenResponse> GenerateToken(ILambdaContext contextLambda);
        Task<ListInmueblesResponse> ListarInmuebles(ILambdaContext contextLambda, string vendedor);
        Task<SaveResult> MantInmuebles(ILambdaContext contextLambda, MantInmuebleRequest request);
        Task<ListTipoInmuebleResponse> ListTipoInmueble(ILambdaContext contextLambda);
        Task<bool> ValidateToken(ILambdaContext contextLambda, string token);
    }
}
