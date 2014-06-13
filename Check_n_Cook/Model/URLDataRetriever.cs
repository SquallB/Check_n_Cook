﻿using Check_n_Cook.Model.Grade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Xaml.Controls;

namespace Check_n_Cook.Model
{
    public class URLDataRetriever
    {
        public String URL { get; set; }

        private DateTime getDateTimeFromString(String dateString)
        {
            String date = dateString.Substring(0, dateString.IndexOf('T'));
            int posFirstDash = date.IndexOf('-');
            int posSecondDash = date.IndexOf('-', posFirstDash + 1);
            int year = int.Parse(date.Substring(0, posFirstDash));
            int month = int.Parse(date.Substring(posFirstDash + 1, posSecondDash - posFirstDash - 1));
            int day = int.Parse(date.Substring(posSecondDash + 1, date.Length - posSecondDash - 1));

            String time = dateString.Substring(date.Length + 1, dateString.Length - date.Length - 2);
            int posFirstColon = time.IndexOf(':');
            int posSecondColon = time.IndexOf(':', posFirstColon + 1);
            int hour = int.Parse(time.Substring(0, posFirstColon));
            int minute = int.Parse(time.Substring(posFirstColon + 1, posSecondColon - posFirstColon - 1));
            int second = int.Parse(time.Substring(posSecondColon + 1, time.Length - posSecondColon - 1));

            return new DateTime(year, month, day, hour, minute, second);
        }

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
                if (property.Value.Stringify() != "null")
                {
                    switch (property.Key)
                    {
                        case "author":
                            receipe.Author = property.Value.GetString();
                            break;

                        case "cost":
                            receipe.Cost = new Cost((int)property.Value.GetNumber());
                            break;

                        case "difficulty":
                            receipe.Difficulty = new Difficulty((int)property.Value.GetNumber());
                            break;

                        case "dishType":
                            JsonObject dishType = property.Value.GetObject();
                            foreach (var dishTypeProperty in dishType)
                            {
                                if (dishTypeProperty.Key == "label")
                                {
                                    receipe.DishType = DishType.GetInstance(dishTypeProperty.Value.GetString());
                                }
                            }
                            break;

                        case "id":
                            receipe.Id = (int)property.Value.GetNumber();
                            break;

                        case "isVegetarian":
                            receipe.Vegetarian = property.Value.GetBoolean();
                            break;

                        case "published":
                            receipe.PublicationDate = getDateTimeFromString(property.Value.GetString());
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

                        case "pictures":
                            var picturesArray = property.Value.GetArray();
                            string found = "";
                            if (picturesArray != null)
                            {
                                var pictureObject = picturesArray[1];
                                    foreach (var picture in pictureObject.GetObject())
                                    {

                                        if (picture.Key == "url")
                                        {
                                            found = picture.Value.GetString();
                                        }                                   
                                    }
                                
                            }
                            receipe.Image = found;
                            break;
                    }
                }
            }

            return receipe;
        }

        public async Task<bool> GetData(String keyWord, int nbItemsPerPage, int startIndex, AppModel model)
        {
            HttpClient http = new System.Net.Http.HttpClient();
            bool error = false;

            try
            {
                HttpResponseMessage response = await http.GetAsync(String.Format(this.URL, keyWord, nbItemsPerPage, startIndex));
                string jsonString = await response.Content.ReadAsStringAsync();

                JsonObject jsonObject = JsonObject.Parse(jsonString);
                JsonArray jsonArray = getItemsArrayFromJSONObject(jsonObject);
                foreach (var item in jsonArray)
                {
                    Receipe receipe = getReceipeFromJSONItem(item.GetObject());
                    model.AddReceipe(receipe);
                }
            }
            catch (Exception ex)
            {
                error = true;
            }

            return error;
        }

        public URLDataRetriever()
        {
            this.URL = "http://m.marmiton.org/webservices/json.svc/GetRecipeSearch?SiteId=1&KeyWord={0}&SearchType=0&ItemsPerPage={1}&StartIndex={2}";
        }
    }
}
