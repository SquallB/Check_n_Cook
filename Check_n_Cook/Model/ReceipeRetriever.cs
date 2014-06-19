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
            this.URL = "http://www.marmiton.org/recettes/recette_s_" + id + ".aspx";

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
            String text = node.InnerText;
            String html = node.InnerHtml;

            int counterOfIng = 0;
            var spans = node.Descendants("span");
            foreach (var span in spans)
            {
                html = html.Replace(span.OuterHtml, "");
            }
            html = html.Replace("\n", "");
            var linksIng = node.Descendants("a");
            foreach (var currLinkIng in linksIng)
            {
                html = html.Replace(currLinkIng.OuterHtml, currLinkIng.InnerText);
            }

            string[] stringSeparators = new string[] { "<br>" };

            string[] listOfIng = html.Split(stringSeparators, StringSplitOptions.None);
            rec.ingredients = new List<Ingredient>();

            foreach (var indexIng in listOfIng)
            {
                if (indexIng != "" && indexIng.Length > 0 && indexIng != " " && indexIng != null  && indexIng.IndexOf('-') != -1)
                {

                    int counterOfWord = 0;
                    var updatedIng = indexIng;
                    if (indexIng.IndexOf('-') != -1)
                    {
                        int firstIndexIng = indexIng.IndexOf('-');
                        updatedIng = indexIng.Substring(firstIndexIng);
                    }


                    string[] listofArgs = updatedIng.Split(' ');

                    Ingredient currentIng = new Ingredient();
                    currentIng.name = "";
                    currentIng.unity = "";
                    currentIng.quantity = "";
                    Boolean hasUnity = false;
                    Boolean hasQty = false;
                    Boolean needMoreDetailUnity = false;
                    int indiceUnity = 0;
                    foreach (var currentArg in listofArgs)
                    {
                        Boolean hasDigit = false;
                        foreach (char letter in currentArg)
                        {
                            if (Char.IsDigit((letter)))
                            {
                                hasDigit = true;
                            }

                        }

                        var arg = currentArg;
                        if (counterOfWord == 0 )
                        {
                            arg = arg.Replace("-", "");
                        }
                        if (counterOfWord == 1 && hasDigit)
                        {
                            currentIng.quantity = arg;

                            hasQty = true;
                        }
                        else if (arg.ToUpper() == "kg".ToUpper() || arg.ToUpper() == "tasse".ToUpper() ||  arg.ToUpper() == "bol".ToUpper() || arg.ToUpper() == "cuillère".ToUpper() || arg.ToUpper() == "cuillères".ToUpper() || arg.ToUpper() == "G" || arg.ToUpper() == "L" || arg.ToUpper() == "CL")
                        {
                            if (hasQty)
                            {
                                currentIng.unity += (string)arg;
                                hasUnity = true;
                                indiceUnity = counterOfWord;

                            }
                            if(arg.ToUpper() == "cuillère".ToUpper() || arg.ToUpper() == "cuillères".ToUpper()) {
                                needMoreDetailUnity = true;
                            }
                        } else if(needMoreDetailUnity) {
                            if (hasQty && arg.ToUpper() == "à".ToUpper())
                            {
                                currentIng.unity += (string)arg;
                                hasUnity = true;
                                indiceUnity = counterOfWord;
                                needMoreDetailUnity = true;

                            }
                            if (arg.ToUpper() == "soupe".ToUpper() || arg.ToUpper() == "café".ToUpper())
                            {
                                currentIng.unity += (string)arg;
                                hasUnity = true;
                                indiceUnity = counterOfWord;
                                needMoreDetailUnity = false;
                            }

                        }
                        else
                        {
                            if (hasUnity)
                            {
                                if (indiceUnity == counterOfWord - 1)
                                {
                                    arg = arg.Replace("de", "");
                                    arg = arg.Replace("d'", "");
                                    arg = arg.Replace(" ", "");

                                }
                            }
                            if (arg != "" && arg != " ")
                            {
                                arg = arg.Replace(",", " et ");
                                currentIng.name += arg + " ";

                            }

                        }
                        counterOfWord++;

                    }
                    if (currentIng.name.Length > 0 && counterOfWord >= 0 && currentIng.name != "")
                    {
                        string newstring = currentIng.name[0].ToString().ToUpper() + currentIng.name.Substring(1).ToLower();
                        currentIng.name = newstring;
                        rec.ingredients.Add(currentIng);

                    }

                }
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
                    curDiv0.Attributes.RemoveAll();
                    curDiv0.Attributes.Add("style", "font-family:Segoe UI;font-weight: 350;font-size:17px;");

                    String htmlToDo = curDiv0.OuterHtml;

                    var linksToHide = curDiv0.Elements("a");
                    foreach (var currentLink in linksToHide)
                    {
                        htmlToDo = htmlToDo.Replace(currentLink.OuterHtml, currentLink.InnerText);
                    }
                    var divEl2 = curDiv0.Elements("div");
                    foreach (var currDiv2 in divEl2)
                    {

                        var paraph = currDiv2.Elements("p");
                        foreach (var currentParaph in paraph)
                        {
                            var linksToHide2 = currentParaph.Elements("a");
                            foreach (var currentLink2 in linksToHide2)
                            {
                                htmlToDo = htmlToDo.Replace(currentLink2.OuterHtml, currentLink2.InnerText);
                            }
                        }
                    }
                    rec.ToDoInstructions = htmlToDo;
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
