using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Check_n_Cook.Model
{
    public class ReceipeRetriever
    {
        public String URL { get; set; }
        public int id { get; set; }

        private Task<HtmlDocument> doc;
        public HtmlNode ingredientPart; 

        public ReceipeRetriever()
        {

        }

        public void updateURL(int id)
        {
            this.URL = "http://www.marmiton.org/recettes/recette_s_"+id+".aspx";

        }
        public Boolean isNumber(String number)
        {
            try
            {
                if (number is String)
                {
                    Double.Parse(number as String);
                }
                else
                {
                    Double.Parse(number.ToString());
                    return true;
                }
            }
            catch
            {
                return false;

            }
            return false;


        }
        public void handleIngredients(HtmlNode node, Receipe rec)
        {
            String html = node.InnerText;
            html = html.Replace("<br>", "");
            html = html.Replace("</br>", "");
            var spans = node.Descendants("span");
            foreach (var span in spans)
            {
                html = html.Replace(span.InnerText, "");
            }
            string[] listOfIng = html.Split('-');
            rec.ingredients = new List<Ingredient>();

            foreach (var indexIng in listOfIng)
            {
                int counterOfWord = 0;
                int counterOfIng = 0;

                string[] listofArgs = indexIng.Split(' ');
                Ingredient currentIng = new Ingredient();
                /*currentIng.quantity = counterOfIng;
                currentIng.name = "test";
                currentIng.unity = new Unit("g");*/

                foreach (var currentArg in listofArgs)
                {

                    if (isNumber(currentArg) && counterOfWord <= 1)
                    {
                        currentIng.quantity = Double.Parse(currentArg);


                    }
                    else if (currentArg == "cuillères" || currentArg == "g" || currentArg == "L")
                    {

                        currentIng.unity = new Unit(currentArg);
                    }
                    else if (currentArg != " " && currentArg != "")
                    {
                        currentIng.name = currentArg;
                    }
                    counterOfWord++;

                }
                rec.ingredients.Add(currentIng);

                counterOfIng++;
            }
        }
        public void handleContentReceipe(HtmlNode node, Receipe rec)
        {
        }

        public bool cleanHtmlEntities(string html, Receipe rec)
        {
            int posIngredients = html.IndexOf("<p class=\"m_content_recette_ingredients\">");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var body = doc.DocumentNode.Element("html").Element("body");
            var div = body.Elements("div");
            var isNotEmpty = false;
            rec.ToDoInstructions = "";
            var divs = doc.DocumentNode.Descendants("div");
            foreach (var curDiv0 in divs)
            {
                if (curDiv0.GetAttributeValue("class", "") == "m_content_recette_todo")
                {
                    rec.ToDoInstructions = curDiv0.InnerHtml;
                    isNotEmpty = true;
                }

            }
            var pPart = doc.DocumentNode.Descendants("p");
            foreach (var currentPart in pPart)
            {

                if (currentPart.GetAttributeValue("class", "") == "m_content_recette_ingredients")
                {
                    this.ingredientPart = currentPart;
                    isNotEmpty = true;

                }

            }

            return isNotEmpty;
        }

            
        public async Task<bool> extractReceipeFromMarmiton(Receipe receipe)
        {
            bool isDone = false;
            string title = receipe.Title.ToLower();
            title = title.Replace(" ", "-");
            string accent = "ÀÁÂÃÄÅàáâãäåÒÓÔÕÖØòóôõöøÈÉÊËèéêëÌÍÎÏìíîïÙÚÛÜùúûüÿÑñÇç°";
            string sansAccent = "AAAAAAaaaaaaOOOOOOooooooEEEEeeeeIIIIiiiiUUUUuuuuyNnCc-";

            char[] tableauAccent = accent.ToCharArray();
            char[] tableauSansAccent = sansAccent.ToCharArray();

            for (int i = 0; i < accent.Length; i++)
            {
                title = title.Replace(tableauAccent[i].ToString(), tableauSansAccent[i].ToString());
            }
            this.URL = "http://dev.yougoweb.fr/misn/api2.php?id=" + receipe.Id;
            HttpClient http = new System.Net.Http.HttpClient();

            HttpResponseMessage response = await http.GetAsync(this.URL);
            receipe.HtmlReceipe = await response.Content.ReadAsStringAsync();
             isDone = true;
            return isDone;

            
        }
    
    }

    
}
