using Amazon.Lambda.Core;
using AwsDotnetCsharp.Domain.Interface;
using AwsDotnetCsharp.Infrastructure.Persistence.Interface;
using AwsDotnetCsharp.Models.Request;
using AwsDotnetCsharp.Models.Response;
using AwsDotnetCsharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using AwsDotnetCsharp.Application.Interface;
using AwsDotnetCsharp.Infrastructure.Utils;

namespace AwsDotnetCsharp.Application.Services
{
    public class InmuebleApplication : IInmuebleApplication
    {
        private readonly IInmuebleRepository _repository;
        private readonly IInmuebleDomain _domain;
        public InmuebleApplication(IInmuebleRepository inmuebleRepository, IInmuebleDomain inmuebleDomain)
        {
            _repository = inmuebleRepository;
            _domain = inmuebleDomain;
        }
        public async Task<GenerateTokenResponse> GenerateToken(ILambdaContext contextLambda)
        {
            GenerateTokenResponse result = new GenerateTokenResponse();
            try
            {
                result = await this.GenerateTokenAutenticacion();
            }
            catch (Exception ex)
            {
                result = new GenerateTokenResponse
                {
                    access_token = "",
                    expires = 0
                };
            }
            return result;
        }

        public async Task<ListInmueblesResponse> ListarInmuebles(ILambdaContext contextLambda, string vendedor)
        {
            ListInmueblesResponse listInmueble = new ListInmueblesResponse();
            try
            {
                listInmueble = await _repository.ListInmueble(contextLambda, vendedor);
            }
            catch (Exception ex)
            {
                listInmueble = new ListInmueblesResponse
                {
                    codigo = (int)HttpStatusCode.InternalServerError,
                    mensaje = ex.Message,
                    data = new List<InmuebleBE>()
                };
            }
            return listInmueble;
        }
        public async Task<ListTipoInmuebleResponse> ListTipoInmueble(ILambdaContext contextLambda)
        {
            ListTipoInmuebleResponse listInmueble = new ListTipoInmuebleResponse();
            try
            {
                listInmueble = await _repository.ListTipoInmueble(contextLambda);
            }
            catch (Exception ex)
            {
                listInmueble = new ListTipoInmuebleResponse
                {
                    codigo = (int)HttpStatusCode.InternalServerError,
                    mensaje = ex.Message,
                    data = new List<TipoInmuebleBE>()
                };
            }
            return listInmueble;
        }

        public async Task<SaveResult> MantInmuebles(ILambdaContext contextLambda, MantInmuebleRequest request)
        {
            SaveResult result = new SaveResult();
            try
            {
                result = await _repository.MantInmuebles(contextLambda, request);
            }
            catch (Exception ex)
            {
                result = new SaveResult
                {
                    codigo = (int)HttpStatusCode.InternalServerError,
                    mensaje = ex.Message
                };
            }
            return result;
        }

        #region " Token "
        public async Task<bool> ValidateToken(ILambdaContext contextLambda, string token)
        {
            try
            {
                string secretKeyToken = await _domain.GetSecretKeyToken();
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secretKeyToken);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = "WebApiJwt.com",
                    ValidAudience = "server",
                    ClockSkew = TimeSpan.Zero
                };

                SecurityToken validatedToken;
                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // Devuelve el resultado de la validaci�n y los claims del token.
                (bool tokenValido, List<Claim> claims) = (true, new List<Claim>(claimsPrincipal.Claims));
                if (tokenValido)
                    Helpers.PrintLogMessage(contextLambda, "Token valido: ");
                else
                    Helpers.PrintLogMessage(contextLambda, "token invalido: ");

                return tokenValido;
            }
            catch(Exception ex)
            {
                Helpers.PrintLogMessage(contextLambda, "Error token: ");
                return false;
            }
        }
        public async Task<GenerateTokenResponse> GenerateTokenAutenticacion()
        {
            int durationToke = 15;
            GenerateTokenResponse result = new GenerateTokenResponse();
            string secretKeyToken = await _domain.GetSecretKeyToken();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyToken));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var listClaims = new List<Claim>
                {
                    new Claim("usuarioid", "1"),
                    new Claim(ClaimTypes.Role, "Admin"),
                };

            // Define la configuración del token, como su duración y las claims.
            var token = new JwtSecurityToken(
                issuer: "WebApiJwt.com",
                audience: "server",
                claims: listClaims,
                expires: DateTime.UtcNow.AddMinutes(durationToke),
                signingCredentials: creds
            );

            // Genera el token como una cadena JWT.
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);
            return result = new GenerateTokenResponse
            {
                access_token = tokenString,
                expires = durationToke
            };
        }
        #endregion
    }
}
