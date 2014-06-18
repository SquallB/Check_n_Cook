﻿using HtmlAgilityPack;
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
            String html = node.InnerHtml;
            String text = node.InnerText;

            int counterOfIng = 0;
            var spans = node.Descendants("span");
            foreach (var span in spans)
            {
                html = html.Replace(span.OuterHtml, "");
            }
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

                if (indexIng != "" && indexIng.Length > 0 && indexIng != " " && indexIng != null && counterOfIng > 0 && indexIng.IndexOf('-') != -1)
                {
                    int counterOfWord = 0;

                    string[] listofArgs = indexIng.Split(' ');

                    Ingredient currentIng = new Ingredient();
                    currentIng.name = "";
                    currentIng.unity = "";
                    currentIng.quantity = "";
                    Boolean hasUnity = false;
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

                        var arg = currentArg.Replace(" ", "");
                        if (counterOfWord == 0)
                        {
                            arg = arg.Replace("-", "");
                        }
                        if (counterOfWord == 1 && hasDigit)
                        {
                            currentIng.quantity = arg;


                        }
                        else if (arg.ToUpper() == "cuillère".ToUpper() || arg.ToUpper() == "cuillères".ToUpper() || arg.ToUpper() == "G" || arg.ToUpper() == "L" || arg.ToUpper() == "CL")
                        {

                            currentIng.unity = (string)arg;
                            hasUnity = true;
                            indiceUnity = counterOfWord;
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
                                currentIng.name += arg + " ";

                            }

                        }
                        counterOfWord++;

                    }
                    if (currentIng.name.Length > 0 && counterOfWord > 0 && currentIng.name != "")
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
