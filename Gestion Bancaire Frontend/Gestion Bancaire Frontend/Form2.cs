using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gestion_Bancaire_Frontend
{
    public partial class Form2 : Form
    {
        public List<Compte> ListeObjets { get; set; }
       

        public Form2()
        {
            InitializeComponent();
           
        }
        private void Form2_Load(object sender, EventArgs e)
        {
          

        }
        public void AfficherListe(List<Compte> liste, string nom , double solde)

        {
            
            label6.Text = nom;
            label5.Text = solde.ToString()+ " DH";
            dataGridView1.Rows.Clear(); // Effacer les lignes existantes dans la DataGridView

            foreach (Compte element in liste)
            {
                dataGridView1.Rows.Add(element.montant, element.data_mvt); // Ajouter une nouvelle ligne avec les valeurs du montant et de la data_mvt
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {


        }
    }
    
}
