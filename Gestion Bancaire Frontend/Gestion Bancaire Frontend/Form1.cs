using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Gestion_Bancaire_Frontend
{
    public partial class Form1 : Form
    {
        private HttpClient client;
        string nom;
        double solde;
        public Form1()
        {
            InitializeComponent();
            client = new HttpClient();
        }

        private  void textBox1_TextChangedAsync(object sender, EventArgs e)
        {
            
           

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
           // ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            try
            {
                int id = textBox1.Text.GetHashCode();

                if (int.TryParse(textBox1.Text, out id))
                {
                    string apiUrl = $"https://localhost:7155/api/Compte/{id}" ;
                    string userInput = textBox1.Text;
                    //Console.WriteLine("Vous avez entré : " + apiUrl);
                   // System.Console.WriteLine(id);
                   using (HttpClient client = new HttpClient())
                    {
                        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                        
                        HttpResponseMessage response = await client.GetAsync(apiUrl);
                       
                        response.EnsureSuccessStatusCode();

                        string responseContent = await response.Content.ReadAsStringAsync();

                        nom= label4.Text = responseContent;
                       

                    }
                }
            }
            catch (HttpRequestException ex)
            {
                // Gérer l'exception HttpRequestException
                Console.WriteLine("Une exception HttpRequestException s'est produite :");
                Console.WriteLine("Message : " + ex.Message);
                Console.WriteLine("StackTrace : " + ex.StackTrace);

                if (ex.InnerException != null)
                {
                    Console.WriteLine("InnerException : " + ex.InnerException.Message);
                    Console.WriteLine("InnerException StackTrace : " + ex.InnerException.StackTrace);
                }
            }
            catch (Exception ex)
            {
                // Gérer toutes les autres exceptions
                Console.WriteLine("Une exception s'est produite :");
                Console.WriteLine("Message : " + ex.Message);
                Console.WriteLine("StackTrace : " + ex.StackTrace);

                if (ex.InnerException != null)
                {
                    Console.WriteLine("InnerException : " + ex.InnerException.Message);
                    Console.WriteLine("InnerException StackTrace : " + ex.InnerException.StackTrace);
                }
            }
           

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(textBox1.Text);
                double montant = int.Parse(textBox2.Text);
                Console.WriteLine("Vous avez entré : " + montant);
                if (debit.Checked)
                {
                    string apiUrl = $"https://localhost:7155/api/Mouvement/debit/{id}/{montant}";
                    //string userInput = textBox1.Text;
                    //Console.WriteLine("Vous avez entré : " + apiUrl);
                    // System.Console.WriteLine(id);
                    using (HttpClient client = new HttpClient())
                    {
                        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                        Dictionary<string, string> requestBody = new Dictionary<string, string>
                                {
                                    { "id", id.ToString() },
                                    { "montant", montant.ToString() }
                                };
                        string jsonBody = JsonConvert.SerializeObject(requestBody);
                        HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                        response.EnsureSuccessStatusCode();

                        string responseContent = await response.Content.ReadAsStringAsync();


                        List<Compte> liste = JsonConvert.DeserializeObject<List<Compte>>(responseContent);
                        Console.WriteLine(liste);
                       
                        foreach (Compte element in liste)
                        {
                            solde =element.solde; // Ajouter une nouvelle ligne avec les valeurs du montant et de la data_mvt
                        }
                        Console.WriteLine(solde);
                        Form2 form2 = new Form2();
                        form2.AfficherListe(liste,nom,solde);
                        
                        form2.Show();
                    }
                   

                }
                else if (credit.Checked)
                    {
                    string apiUrl = $"https://localhost:7155/api/Mouvement/credit/{id}/{montant}";
                    //string userInput = textBox1.Text;
                    //Console.WriteLine("Vous avez entré : " + apiUrl);
                    // System.Console.WriteLine(id);
                    using (HttpClient client = new HttpClient())
                    {
                        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                        Dictionary<string, string> requestBody = new Dictionary<string, string>
                                {
                                    { "id", id.ToString() },
                                    { "montant", montant.ToString() }
                                };
                        string jsonBody = JsonConvert.SerializeObject(requestBody);
                        HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                        
                        response.EnsureSuccessStatusCode();

                        string responseContent = await response.Content.ReadAsStringAsync();

                        List<Compte> liste = JsonConvert.DeserializeObject<List<Compte>>(responseContent);
                        Console.WriteLine(liste);
                        foreach (Compte element in liste)
                        {
                            solde = element.solde; // Ajouter une nouvelle ligne avec les valeurs du montant et de la data_mvt
                        }
                        Form2 form2 = new Form2();
                        form2.AfficherListe(liste, nom, solde);
                        form2.Show();


                    }

                }
                
               
            }
            catch (HttpRequestException ex)
            {
                // Gérer l'exception HttpRequestException
                Console.WriteLine("Une exception HttpRequestException s'est produite :");
                Console.WriteLine("Message : " + ex.Message);
                Console.WriteLine("StackTrace : " + ex.StackTrace);

                if (ex.InnerException != null)
                {
                    Console.WriteLine("InnerException : " + ex.InnerException.Message);
                    Console.WriteLine("InnerException StackTrace : " + ex.InnerException.StackTrace);
                }
            }
            catch (Exception ex)
            {
                // Gérer toutes les autres exceptions
                Console.WriteLine("Une exception s'est produite :");
                Console.WriteLine("Message : " + ex.Message);
                Console.WriteLine("StackTrace : " + ex.StackTrace);

                if (ex.InnerException != null)
                {
                    Console.WriteLine("InnerException : " + ex.InnerException.Message);
                    Console.WriteLine("InnerException StackTrace : " + ex.InnerException.StackTrace);
                }
            }
           

           


        }

        private void debit_CheckedChanged(object sender, EventArgs e)
        {
            if (debit.Checked)
            {
                
            }
            else
            {
                
            }

        }

        private void credit_CheckedChanged(object sender, EventArgs e)
        {
            if (credit.Checked)
            {
               
            }
            else
            {
                
            }
        }
    }

 
}
