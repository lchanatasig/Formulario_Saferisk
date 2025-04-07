using Formulario_Saferisk.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Formulario_Saferisk.Servicios
{
    public class InsertarService
    {
        private readonly ILogger<InsertarService> _logger;
        private readonly FormularioDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InsertarService(ILogger<InsertarService> logger, FormularioDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> InsertarDatosAsync(string nombresCompletos, string perfil, string broker, string correo, string ciudad)
        {
            using (SqlConnection connection = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                using (SqlCommand command = new SqlCommand("InsertarDatos", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@nombres_completos", nombresCompletos);
                    command.Parameters.AddWithValue("@perfil", perfil);
                    command.Parameters.AddWithValue("@broker", broker);
                    command.Parameters.AddWithValue("@correo", correo);
                    command.Parameters.AddWithValue("@ciudad", ciudad);

                    // Parámetro para capturar el valor de retorno del SP
                    var returnParameter = new SqlParameter
                    {
                        ParameterName = "@ReturnVal",
                        Direction = ParameterDirection.ReturnValue
                    };
                    command.Parameters.Add(returnParameter);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    int result = (int)returnParameter.Value;
                    return result == 1; // true = éxito, false = correo duplicado
                }
            }
        }


        public async Task<List<Broker>> ListarDatosAsync()
        {
            var brokers = new List<Broker>();

            using (SqlConnection connection = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                using (SqlCommand command = new SqlCommand("ListarDatos", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var broker = new Broker
                            {
                                BrokerId = reader.GetInt32(reader.GetOrdinal("broker_id")),
                                CodAuto = reader.GetString(reader.GetOrdinal("cod_auto")),
                                Categoria = reader.GetString(reader.GetOrdinal("categoria")),
                                Descripcion = reader.GetString(reader.GetOrdinal("descripcion"))
                            };

                            brokers.Add(broker);
                        }
                    }
                }
            }

            return brokers;
        }
    }
}
