using System;
using System.Collections.Generic;
using System.Net;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using AwsDotnetCsharp.Application.Interface;
using AwsDotnetCsharp.Models.Request;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(
typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AwsDotnetCsharp
{
    public class Handler
    {
        private readonly IInmuebleApplication _inmuebleApplication;
        public Handler() : this(null)
        {

        }
        internal Handler(IInmuebleApplication inmuebleApplication = null)
        {
            Startup.ConfigureServices();
            _inmuebleApplication = inmuebleApplication ?? Startup.Services.GetRequiredService<IInmuebleApplication>();

        }

        /// <summary>
        /// Function get token sin usuario
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> GenerateToken(APIGatewayProxyRequest request, ILambdaContext context)
        {
            PrintLogMessage(context, JsonConvert.SerializeObject(request));
            try
            {
                var tokenRespone = await _inmuebleApplication.GenerateToken(context);
                return ResponseEntity(tokenRespone, 200);
            }
            catch (Exception ex)
            {
                PrintLogMessage(context, "Error al generar token " + ex.Message);
                PrintLogMessage(context, JsonConvert.SerializeObject(ex));
                return ResponseEntity(new
                {
                    codigo = 500,
                    mensaje = "Error al generar token " + ex.Message
                }, 500);
            }
        }

        /// <summary>
        /// Listar Inmueble de vendedor
        /// </summary>
        /// <param name="request">nombre vendedor</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> ListarInmuebles(APIGatewayProxyRequest request, ILambdaContext context)
        {
            PrintLogMessage(context, JsonConvert.SerializeObject(request));
            bool validateToken = await ValidateAccessToken(request, context);
            if(validateToken)
            {
                string vendedor = request.QueryStringParameters["vendedor"] ?? "";
                var listInmuebles = await _inmuebleApplication.ListarInmuebles(context, vendedor);
                return ResponseEntity(new
                {
                    codigo = listInmuebles.codigo,
                    mensaje = listInmuebles.mensaje,
                    sucess = listInmuebles.codigo == (int)HttpStatusCode.OK,
                    listInmuebles.data
                }, listInmuebles.codigo);
            }
            else
            {
                return ResponseEntity(new { Message = "No cuenta con acceso." }, (int)HttpStatusCode.Unauthorized);
            }
        }
        /// <summary>
        /// Insert, Update table Innmueble
        /// </summary>
        /// <param name="request">
        /// cinmueble: identificador unico de tabla,
        /// tipo_inmueble: indetificador foraneo: (1) o (2)
        /// vendedor : nombre del vendedor
        /// cant_habitaciones : numero de habitaciones
        /// disponible: si esta disponible o no el inmueble
        /// mascota: si acepta o no mascota
        /// cusuario: (1) simulando un rastreo
        /// </param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> MantInmuebles(APIGatewayProxyRequest request, ILambdaContext context)
        {
            PrintLogMessage(context, JsonConvert.SerializeObject(request));
            bool validateToken = await ValidateAccessToken(request, context);
            if(validateToken)
            {
                string vendedor = request.QueryStringParameters["vendedor"] ?? "";
                MantInmuebleRequest requestMant = new MantInmuebleRequest();
                requestMant.cinmueble = Convert.ToInt32(request.QueryStringParameters["cinmueble"] ?? "0");
                requestMant.tipo_inmueble = Convert.ToInt32(request.QueryStringParameters["tipo_inmueble"] ?? "0");
                requestMant.vendedor = request.QueryStringParameters["vendedor"] ?? "";
                requestMant.cant_habitaciones = Convert.ToInt32(request.QueryStringParameters["cant_habitaciones"] ?? "0");
                requestMant.flag_disponible = Convert.ToBoolean(request.QueryStringParameters["disponible"] ?? "false");
                requestMant.flag_mascota = Convert.ToBoolean(request.QueryStringParameters["mascota"] ?? "false");
                var listInmuebles = await _inmuebleApplication.MantInmuebles(context, requestMant);
                return ResponseEntity(new
                {
                    codigo = listInmuebles.codigo,
                    mensaje = listInmuebles.mensaje,
                    sucess = listInmuebles.codigo == (int)HttpStatusCode.OK
                }, listInmuebles.codigo);
            }
            else
            {
                return ResponseEntity(new { Message = "No cuenta con acceso." }, (int)HttpStatusCode.Unauthorized);
            }
            
        }

        /// <summary>
        /// List tipos de inmueble
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> ListTipoInmueble(APIGatewayProxyRequest request, ILambdaContext context)
        {
            PrintLogMessage(context, JsonConvert.SerializeObject(request));
            bool validateToken = await ValidateAccessToken(request, context);
            if (validateToken)
            {
                var listInmuebles = await _inmuebleApplication.ListTipoInmueble(context);
                return ResponseEntity(new
                {
                    codigo = listInmuebles.codigo,
                    mensaje = listInmuebles.mensaje,
                    sucess = listInmuebles.codigo == (int)HttpStatusCode.OK,
                    listInmuebles.data
                }, listInmuebles.codigo);
            }
            else
            {
                return ResponseEntity(new { Message = "No cuenta con acceso." }, (int)HttpStatusCode.Unauthorized);
            }
        }


        private async Task<bool> ValidateAccessToken(APIGatewayProxyRequest request, ILambdaContext ctx)
        {
            bool validado = false;
            foreach (var obj in request.Headers)
            {

                if (obj.Key == "Authorization")
                {
                    string dato = obj.Value.ToString();
                    if(!string.IsNullOrEmpty(dato))
                    {
                        string token = dato;
                        validado = await _inmuebleApplication.ValidateToken(ctx, token);
                    }
                }

            }


            return validado;
        }
        void PrintLogMessage(ILambdaContext ctx, string msg)
        {
            ctx.Logger.LogLine(string.Format("{0}:{1} - {2}",
                            ctx.AwsRequestId,
                            ctx.FunctionName,
                            msg));
        }
        APIGatewayProxyResponse ResponseEntity(Object result, int statusCodeResponse)
        {
            string body = (result != null) ?
                    JsonConvert.SerializeObject(result) : string.Empty;

            return new APIGatewayProxyResponse
            {
                StatusCode = statusCodeResponse,
                Body = body,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Access-Control-Allow-Origin", "*" }
                }
            };
        }

    }



}
