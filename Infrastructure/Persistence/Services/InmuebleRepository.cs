using Amazon.Lambda.Core;
using AwsDotnetCsharp.Infrastructure.Persistence.Interface;
using AwsDotnetCsharp.Models.Request;
using AwsDotnetCsharp.Models.Response;
using AwsDotnetCsharp.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace AwsDotnetCsharp.Infrastructure.Persistence.Services
{
    public class InmuebleRepository : IInmuebleRepository
    {
        SqlConnection cnn = new SqlConnection();
        public InmuebleRepository()
        {
        }
        public async Task<ListInmueblesResponse> ListInmueble(ILambdaContext contextLambda, string vendedor)
        {
            cnn = null;
            SqlDataReader read = null;
            ListInmueblesResponse result = new ListInmueblesResponse();
            InmuebleBE response = new InmuebleBE();
            List<InmuebleBE> lresponse = new List<InmuebleBE>();
            try
            {
                using (cnn = Connection.ConnectToSQL())
                {
                    if (cnn != null)
                    {
                        await cnn.OpenAsync();
                        SqlCommand com = new SqlCommand(); // Create a object of SqlCommand class
                        com.Connection = cnn; //Pass the connection object to Command
                        com.CommandType = CommandType.StoredProcedure; // We will use stored proc
                        com.CommandText = "dbo.List_Inmob"; //Stored Procedure Name
                        com.Parameters.Add(new SqlParameter("@vendedor", vendedor));
                        com.CommandTimeout = 240;
                        read = await com.ExecuteReaderAsync(CommandBehavior.SingleResult);

                        result = new ListInmueblesResponse();
                        response = new InmuebleBE();
                        lresponse = new List<InmuebleBE>();
                        while (await read.ReadAsync())
                        {
                            response = new InmuebleBE
                            {
                                cinmueble = read.IsDBNull(0) ? 0 : read.GetInt32(0),
                                ctipo_inmueble = read.IsDBNull(1) ? 0 : read.GetInt32(1),
                                vendedor = read.IsDBNull(2) ? "" : read.GetString(2),
                                cant_habitaciones = read.IsDBNull(3) ? 0 : read.GetInt32(3),
                                flag_disponible = read.IsDBNull(4) ? false : read.GetBoolean(4),
                                flag_mascota = read.IsDBNull(5) ? false : read.GetBoolean(5),
                            };
                            lresponse.Add(response);
                        }
                        result = new ListInmueblesResponse
                        {
                            codigo = (int)HttpStatusCode.OK,
                            mensaje = "Listado obtenido",
                            data = lresponse
                        };
                        read.Close();
                        com.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                result = new ListInmueblesResponse
                {
                    codigo = (int)HttpStatusCode.InternalServerError,
                    mensaje = ex.Message,
                    data = new List<InmuebleBE>()
                };
            }
            return result;
        }
        public async Task<ListTipoInmuebleResponse> ListTipoInmueble(ILambdaContext contextLambda)
        {
            cnn = null;
            SqlDataReader read = null;
            ListTipoInmuebleResponse result = new ListTipoInmuebleResponse();
            TipoInmuebleBE response = new TipoInmuebleBE();
            List<TipoInmuebleBE> lresponse = new List<TipoInmuebleBE>();
            try
            {
                using (cnn = Connection.ConnectToSQL())
                {
                    if (cnn != null)
                    {
                        await cnn.OpenAsync();
                        SqlCommand com = new SqlCommand(); // Create a object of SqlCommand class
                        com.Connection = cnn; //Pass the connection object to Command
                        com.CommandType = CommandType.StoredProcedure; // We will use stored proc
                        com.CommandText = "dbo.List_TipoInmob"; //Stored Procedure Name
                        com.CommandTimeout = 240;
                        read = await com.ExecuteReaderAsync(CommandBehavior.SingleResult);

                        result = new ListTipoInmuebleResponse();
                        response = new TipoInmuebleBE();
                        lresponse = new List<TipoInmuebleBE>();
                        while (await read.ReadAsync())
                        {
                            response = new TipoInmuebleBE
                            {
                                ctipo_inmueble = read.IsDBNull(0) ? 0 : read.GetInt32(0),
                                descripcion = read.IsDBNull(1) ? "" : read.GetString(1),
                            };
                            lresponse.Add(response);
                        }
                        result = new ListTipoInmuebleResponse
                        {
                            codigo = (int)HttpStatusCode.OK,
                            mensaje = "Listado obtenido",
                            data = lresponse
                        };
                        read.Close();
                        com.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                result = new ListTipoInmuebleResponse
                {
                    codigo = (int)HttpStatusCode.InternalServerError,
                    mensaje = ex.Message,
                    data = new List<TipoInmuebleBE>()
                };
            }
            return result;
        }
        public async Task<SaveResult> MantInmuebles(ILambdaContext contextLambda, MantInmuebleRequest request)
        {
            cnn = null;
            SqlDataReader read = null;
            SaveResult result = new SaveResult();
            try
            {
                using (cnn = Connection.ConnectToSQL())
                {
                    if (cnn != null)
                    {
                        await cnn.OpenAsync();
                        SqlCommand com = new SqlCommand(); // Create a object of SqlCommand class
                        com.Connection = cnn; //Pass the connection object to Command
                        com.CommandType = CommandType.StoredProcedure; // We will use stored proc
                        com.CommandText = "dbo.Mant_Inmob"; //Stored Procedure Name
                        com.Parameters.Add(new SqlParameter("@cinmueble", request.cinmueble));
                        com.Parameters.Add(new SqlParameter("@ctipo_inmueble", request.tipo_inmueble));
                        com.Parameters.Add(new SqlParameter("@vendedor", request.vendedor));
                        com.Parameters.Add(new SqlParameter("@cant_habit", request.cant_habitaciones));
                        com.Parameters.Add(new SqlParameter("@disponible", request.flag_disponible));
                        com.Parameters.Add(new SqlParameter("@mascota", request.flag_mascota));
                        com.Parameters.Add(new SqlParameter("@cusuario", request.cusuario));
                        com.CommandTimeout = 240;
                        read = await com.ExecuteReaderAsync(CommandBehavior.SingleResult);

                        result = new SaveResult();
                        while (await read.ReadAsync())
                        {
                            result = new SaveResult
                            {
                                codigo = read.IsDBNull(0) ? 0 : read.GetInt32(0),
                                mensaje = read.IsDBNull(1) ? "" : read.GetString(1),
                            };
                        }
                        read.Close();
                        com.Dispose();
                    }
                }

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
    }
}
