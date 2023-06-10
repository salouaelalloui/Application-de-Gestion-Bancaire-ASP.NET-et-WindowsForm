using BanqueBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using System.Reflection.PortableExecutable;

namespace BanqueBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MouvementController : ControllerBase
    {
       
        private readonly IConfiguration _configuration;
        public MouvementController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

      

        private static List<Mouvement> comptes = new List<Mouvement>();
       
        [HttpGet]
        [Route("{id}")]
        public Compte GetCompteById(int id)
        {
            string query = @"SELECT *  from Compte WHERE ""id"" = @id";
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
                            
                            int compteId = reader.GetInt32(reader.GetOrdinal("id"));
                            double compteSolde =  reader.GetDouble(reader.GetOrdinal("solde"));
                            string compteNom = reader.GetString(reader.GetOrdinal("nom"));
                          

                            Compte compte = new Compte()
                            {
                                id = compteId,
                                nom = compteNom,
                                solde= compteSolde,
                               
                            };

                            return compte;
                        }
                    }
                }
            }

            return null; 
        }
        [HttpPost("debit/{compteId}/{montant}")]
        public List<Fin> Debit(int compteId, double montant)
        {
            List<Fin> comptes = new List<Fin>();
            Compte c = GetCompteById(compteId);
            double mt = montant;
            if (c.solde > 0 ) {
                c.solde -= montant;
            }
            else
            {
                return null; 
            }
           

            // Create a Mouvement record to log the transaction
            Mouvement mouvement = new Mouvement
            {
                compte_id = c.id,
                montant = montant,
                data_mvt = "Debit",
               
            };
            string queryin = @"UPDATE compte SET solde = @solde  WHERE id = @id";
            string query = @"INSERT INTO mouvement (compte_id, montant,data_mvt) VALUES (@compte_id, @montant, @data_mvt) ";
            string queryfinal = @" SELECT c.nom, c.solde, m.montant ,m.data_mvt FROM compte c JOIN mouvement m ON c.id = m.compte_id WHERE c.id = @id";
            string conString = _configuration.GetConnectionString("GestionBancaire");

            using (NpgsqlConnection connection = new NpgsqlConnection(conString))
            {
                connection.Open();

                using (NpgsqlCommand cm = new NpgsqlCommand(queryin, connection))
                {
                    cm.Parameters.AddWithValue("@id", c.id);
                    cm.Parameters.AddWithValue("@solde", c.solde);


                    cm.ExecuteNonQuery();
                }
                using (NpgsqlCommand insertt = new NpgsqlCommand(query, connection))
                {
                    insertt.Parameters.AddWithValue("@compte_id", c.id);
                    insertt.Parameters.AddWithValue("@montant", mouvement.montant) ;
                    insertt.Parameters.AddWithValue("@data_mvt", mouvement.data_mvt);

                    insertt.ExecuteNonQuery();
                }
                using (NpgsqlCommand querycpt = new NpgsqlCommand(queryfinal, connection))
                {
                    querycpt.Parameters.AddWithValue("@id", c.id);

                    using (NpgsqlDataReader reader = querycpt.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            // int compteId = reader.GetInt32(reader.GetOrdinal("id"));
                            double compteSolde = reader.GetDouble(reader.GetOrdinal("solde"));
                            string compteNom = reader.GetString(reader.GetOrdinal("nom"));
                            double compteMontant = reader.GetDouble(reader.GetOrdinal("montant"));
                            string compteMvt = reader.GetString(reader.GetOrdinal("data_mvt"));
                            Fin compt = new Fin()
                            {

                                nom = compteNom,
                                solde = compteSolde,
                                montant = compteMontant,
                                data_mvt = compteMvt,
                            };

                            comptes.Add(compt);
                            
                        }
                    }
                }
            }

                return comptes;
        }





















        [HttpPost("credit/{compteId}/{montant}")]
        public List<Fin> Credit(int compteId, double montant)
        {
            List<Fin> comptes = new List<Fin>();
            Compte c = GetCompteById(compteId);
           /* if (c == null)
            {
                return "not found";
            }*/
            c.solde += montant;

            
            Mouvement mouvement = new Mouvement
            {
                compte_id = c.id,
                montant = montant,
                data_mvt = "Credit",

            };
            string queryin = @"UPDATE compte SET solde = @solde  WHERE id = @id";
            string query = @"INSERT INTO mouvement (compte_id, montant,data_mvt) VALUES (@compte_id, @montant, @data_mvt) ";
            string queryfinal = @" SELECT c.nom, c.solde, m.montant ,m.data_mvt FROM compte c JOIN mouvement m ON c.id = m.compte_id WHERE c.id = @id";


            // string querycompte = @"SELECT ""nom"" , ""solde""  from Compte WHERE ""id"" = @id";
            //string querymouvement = @"SELECT ""data_mvt"" , ""montant""  from Compte WHERE ""compte_id"" = @compte_id";

            string conString = _configuration.GetConnectionString("GestionBancaire");

            using (NpgsqlConnection connection = new NpgsqlConnection(conString))
            {
                connection.Open();

                using (NpgsqlCommand cm = new NpgsqlCommand(queryin, connection))
                {
                    cm.Parameters.AddWithValue("@id", c.id);
                    cm.Parameters.AddWithValue("@solde", c.solde);


                    cm.ExecuteNonQuery();
                }
                using (NpgsqlCommand insertt = new NpgsqlCommand(query, connection))
                {
                    insertt.Parameters.AddWithValue("@compte_id", c.id);
                    insertt.Parameters.AddWithValue("@montant", montant);
                    insertt.Parameters.AddWithValue("@data_mvt", mouvement.data_mvt);

                    insertt.ExecuteNonQuery();
                }
                using (NpgsqlCommand querycpt = new NpgsqlCommand(queryfinal, connection))
                {
                    querycpt.Parameters.AddWithValue("@id", c.id);

                    using (NpgsqlDataReader reader = querycpt.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                           // int compteId = reader.GetInt32(reader.GetOrdinal("id"));
                            double compteSolde = reader.GetDouble(reader.GetOrdinal("solde"));
                            string compteNom = reader.GetString(reader.GetOrdinal("nom"));
                            double compteMontant = reader.GetDouble(reader.GetOrdinal("montant"));
                            string compteMvt = reader.GetString(reader.GetOrdinal("data_mvt"));
                            Fin compt = new Fin()
                            {
                                
                                nom = compteNom,
                                solde = compteSolde,
                                montant = compteMontant,
                                data_mvt = compteMvt,
                            };

                            comptes.Add(compt);
                        }
                    }
                }
            }

            return comptes;
        }










       
       




















    }
}
