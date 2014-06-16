using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model
{
    public class PagesJaunesShopRetriever
    {
        public String URL { get; set; }

        public async Task<bool> GetShops(String locality, AppModel model)
        {
            string postData = "quoiqui=supermarché&ou=" + locality;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            WebRequest request = WebRequest.Create(this.URL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            Stream dataStream = await request.GetRequestStreamAsync();
            dataStream.Write(byteArray, 0, byteArray.Length);
            WebResponse response2 = await request.GetResponseAsync();
            dataStream = response2.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(responseFromServer);
            IEnumerable<HtmlNode> shopsNodes = document.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("visitCardContent"));
            foreach (HtmlNode shopNode in shopsNodes)
            {
                HtmlNode titleNode = shopNode.Descendants("h2").ElementAt(0);
                String title = titleNode.Descendants("span").Where(d => !d.Attributes.Contains("class")).ElementAt(0).InnerText.Replace("&#039;", "'");

                HtmlNode localisationBlock = shopNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("localisationBlock")).ElementAt(0);
                String address = localisationBlock.Descendants("p").ElementAt(0).InnerHtml.Replace("<br>", " ").Replace("&nbsp;", " ").Replace("<strong>", "").Replace("</strong>", "");

                model.AddShop(new Shop(title, address));
            }

            return true;
        }

        public PagesJaunesShopRetriever()
        {
            this.URL = "http://www.pagesjaunes.fr/trouverlesprofessionnels/rechercheClassique.do";
        }
    }
}
