using Check_n_Cook.Model.Grade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Check_n_Cook.Model
{
    public class URLDataRetriever
    {
        public String URL { get; set; }

        private JsonArray getItemsArrayFromJSONObject(JsonObject jsonObject)
        {
            foreach (var item in jsonObject)
            {
                if (item.Key == "data")
                {
                    JsonObject itemArray = item.Value.GetObject();
                    foreach (var item2 in itemArray)
                    {
                        if (item2.Key == "items")
                        {
                            return item2.Value.GetArray();
                        }
                    }
                }
            }

            return null;
        }

        private Receipe getReceipeFromJSONItem(JsonObject item)
        {
            Receipe receipe = new Receipe();

            foreach (var property in item)
            {
                switch (property.Key)
                {
                    case "author":
                        receipe.Author = new User(property.Value.GetString());
                        break;

                    case "cost":
                        receipe.Cost = new Cost((int)property.Value.GetNumber());
                        break;

                    case "difficulty":
                        receipe.Difficulty = new Difficulty((int)property.Value.GetNumber());
                        break;

                    case "dishType":
                        receipe.DishType = DishType.GetInstance(property.Value.GetString());
                        break;

                    case "id":
                        receipe.Id = (int)property.Value.GetNumber();
                        break;

                    case "isVegetarian":
                        receipe.Vegetarian = property.Value.GetBoolean();
                        break;

                    case "published":
                        receipe.PublicationDate = new DateTime();
                        break; 

                    case "rating":
                        receipe.Rating = new Rating((int)property.Value.GetNumber());
                        break;

                    case "title":
                        receipe.Title = property.Value.GetString();
                        break;

                    case "withAlcohol":
                        receipe.WithAlcohol = property.Value.GetBoolean();
                        break;
                }
            }

            return receipe;
        }

        public async void getData(String keyWord)
        {
            HttpClient http = new System.Net.Http.HttpClient();

            try
            {
                HttpResponseMessage response = await http.GetAsync(String.Format(this.URL, keyWord));
                string jsonString = await response.Content.ReadAsStringAsync();

                JsonObject jsonObject = JsonObject.Parse(jsonString);
                JsonArray jsonArray = getItemsArrayFromJSONObject(jsonObject);

                foreach (var item in jsonArray)
                {
                    Receipe receipe = getReceipeFromJSONItem(item.GetObject());
                }
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

        public URLDataRetriever()
        {
            this.URL = "http://m.marmiton.org/webservices/json.svc/GetRecipeSearch?SiteId=1&KeyWord={0}&SearchType=0&ItemsPerPage=10&StartIndex=1";
        }
    }
}
