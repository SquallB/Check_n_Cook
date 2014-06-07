using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using System.Net;
using System.Net.Http;


namespace Check_n_Cook.Model
{
    public class ReceipeRetriever
    {
        public String URL { get; set; }
        public int id { get; set; }

        public ReceipeRetriever()
        {

        }

        public void updateURL(int id)
        {
            this.URL = "http://www.marmiton.org/recettes/recette_s_"+id+".aspx";

        }
        public static void extractReceipeFromMarmiton(Receipe receipe) {
            string url = "http://www.marmiton.org/recettes/recette_s_" + receipe.Id + ".aspx";

            try
            {
                using (WebClient client = new WebClient()) // classe WebClient hérite IDisposable
                {

                    string codeHtml = client.DownloadString(url);
                    receipe.HtmlReceipe = codeHtml;

                }
                
            }
            catch (Exception ex)
            {
            }

            
        }
    
    }

    
}
