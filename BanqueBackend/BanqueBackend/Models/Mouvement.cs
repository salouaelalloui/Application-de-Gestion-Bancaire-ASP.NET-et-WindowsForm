namespace BanqueBackend.Models
{
    public class Mouvement
    {

        public int id_mv { get; set; }
        public int compte_id { get; set; }
       
        public double montant { get; set; }
        public string data_mvt { get; set; }
        public Compte compte { get; set; }
    }
}
