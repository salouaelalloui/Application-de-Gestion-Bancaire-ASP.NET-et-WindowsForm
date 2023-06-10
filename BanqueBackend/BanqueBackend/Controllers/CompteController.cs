using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;

namespace BanqueBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompteController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public CompteController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("{id}")]
        public  string GetNomById(int id)
        {
            string query = @"SELECT ""nom"" from Compte WHERE ""id"" = @id";
            string conString = _configuration.GetConnectionString("GestionBancaire");
            using (NpgsqlConnection connection = new NpgsqlConnection(conString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetString(0);
                        }
                    }
                }
            }

            return "s"; // Retourne null si l'ID n'est pas trouvé ou s'il y a une erreur
        }
        [HttpGet]

        public List<dynamic> Get()
        {
            List<dynamic> result = new List<dynamic>();
            string query = @" select ""id"" as ""id"",""nom"" as ""nom"" from Compte ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("GestionBancaire");
            NpgsqlDataReader myreader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    using (NpgsqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            dynamic item = new System.Dynamic.ExpandoObject();

                            for (int i = 0; i < myReader.FieldCount; i++)
                            {
                                var columnName = myReader.GetName(i);
                                ((IDictionary<string, object>)item).Add(columnName, myReader[columnName]);
                            }

                            result.Add(item);
                        }

                        
                    }



                }
                return result;
            }

        }
    }
}
