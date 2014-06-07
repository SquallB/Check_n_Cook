using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Windows.Data.Json;

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
        public async static void extractReceipeFromMarmiton(Receipe receipe) {
            string url = "http://www.marmiton.org/recettes/recette_s_" + receipe.Id + ".aspx";
            HttpClient http = new System.Net.Http.HttpClient();

            try
            {
                HttpResponseMessage response = await http.GetAsync(String.Format(url,null));
                string htmlpage = await response.Content.ReadAsStringAsync();

                receipe.HtmlReceipe = htmlpage;
                
            }
            catch (Exception ex)
            {
                // Log Error.
                /*txb_result.Text =
                    "I'm sorry, but I couldn't load the page," +
                    " possibly due to network problems." +
                    "Here's the error message I received: "
                    + ex.ToString();*/
            }

            
        }
    
    }

    
}
