using Check_n_Cook.Model.Grade;
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
    /// <summary>
    /// 
    /// </summary>
    public class URLDataRetriever
    {
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public String URL { get; set; }
        /// <summary>
        /// Gets or sets the advanced search.
        /// </summary>
        /// <value>
        /// The advanced search.
        /// </value>
        public List<string> AdvancedSearch { get; set; }
       
        /// <summary>
        /// Gets or sets the advanced difficulty.
        /// </summary>
        /// <value>
        /// The advanced difficulty.
        /// </value>
        public int AdvancedDifficulty { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [advanced vegetarian].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [advanced vegetarian]; otherwise, <c>false</c>.
        /// </value>
        public bool AdvancedVegetarian { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [advanced alcool].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [advanced alcool]; otherwise, <c>false</c>.
        /// </value>
        public bool AdvancedAlcool { get; set; }

        /// <summary>
        /// Gets the date time from string.
        /// </summary>
        /// <param name="dateString">The date string.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the items array from json object.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the receipe from json item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="keyWord">The key word.</param>
        /// <param name="nbItemsPerPage">The nb items per page.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<bool> GetData(String keyWord, int nbItemsPerPage, int startIndex, AppModel model)
        {
            HttpClient http = new System.Net.Http.HttpClient();
            bool error = false;
            model.ClearReceipes();
            try
            {
                foreach (var jsonItem in model.LocalReceipes)
                {
                    Receipe localToAdd = new Receipe(jsonItem.Stringify());
                    if (checkInstructions( localToAdd,keyWord)|| checkTitle(localToAdd,keyWord))
                    {
                        addReceipe(localToAdd,model);
                    }
                }
            }catch(Exception ex){

            }
            
            try
            {
                
                HttpResponseMessage response = await http.GetAsync(String.Format(this.URL, keyWord, nbItemsPerPage, startIndex));
                string jsonString = await response.Content.ReadAsStringAsync();

                JsonObject jsonObject = JsonObject.Parse(jsonString);
                JsonArray jsonArray = getItemsArrayFromJSONObject(jsonObject);

                
                
                foreach (var item in jsonArray)
                {
                    Receipe receipe = getReceipeFromJSONItem(item.GetObject());
                    addReceipe(receipe, model);
                    
                }
                
                
            }
            catch (Exception ex)
            {
                error = true;
            }

            return error;
        }

        /// <summary>
        /// Gets the data by ingredients.
        /// </summary>
        /// <param name="keyWords">The key words.</param>
        /// <param name="nbItemsPerPage">The nb items per page.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<bool> GetDataByIngredients(String[] keyWords, int nbItemsPerPage, int startIndex, AppModel model)
        {
            bool error = false;
            model.ClearReceipes();
            try
            {
                //we add all receipes created by the user
                foreach (var jsonItem in model.LocalReceipes)
                {
                    Receipe localToAdd = new Receipe(jsonItem.Stringify());
                    addReceipe(localToAdd, model);
                    
                }
            }
            catch (Exception ex)
            {

            }
            try
            {
                
                List<Receipe>[] results = new List<Receipe>[keyWords.Length];
                for (int i = 0; i < keyWords.Length; i++)
                {
                    
                    results[i] = new List<Receipe>();
                    keyWords[i] = keyWords[i].ToUpper();
                    
                }
                //we add all basic research associated to each keyword to the model
                foreach (var keyWord in keyWords)
                {
                   await this.GetData(keyWord, nbItemsPerPage, startIndex, model);
                }
               
                int bestResults = 1;// confidence level
                
                foreach (var receipe in model.Receipes)
                {
                    //we check the research parameters to go faster
                    if (checkType(receipe))
                    {
                        if (checkDifficulty(receipe))
                        {
                            if (checkOptions(receipe))
                            {
                                //used to compute the confidence level
                                int count = 0;

                                //case of receipes from marmiton
                                if (receipe.Id != -1)
                                {
                                    //retrieves ingredients
                                    ReceipeRetriever rr = new ReceipeRetriever();
                                    var task = rr.extractReceipeFromMarmiton(receipe);


                                    if ((await task) == true)
                                    {
                                        var task2 = rr.cleanHtmlEntities(receipe.HtmlReceipe, receipe);
                                        rr.handleIngredients(rr.ingredientPart, receipe);
                                    }
                                }
                                
                                //we compute the confidence level
                                foreach (var keyWord in keyWords)
                                {
                                    //each key word in the receipe informations add 1 confidence level
                                    if (checkIngredients(receipe, keyWord) || checkInstructions(receipe, keyWord) || checkTitle(receipe, keyWord))
                                    {
                                        count++;
                                    }
                                }
                                results[count].Add(receipe);
                                if (count > bestResults)
                                {
                                    bestResults = count;

                                }

                            }
                        }
                    }
                   
                    
                }
                
                model.ClearReceipes();

                
                //only best results are displayed
                foreach (var receipe in results[bestResults])
                {
                    model.AddReceipe(receipe);
                }
                
            }
            catch (Exception ex)
            {
                
                error = true;
            }

            return error;
        }

        /// <summary>
        /// Checks the difficulty.
        /// </summary>
        /// <param name="receipe">The receipe.</param>
        /// <returns></returns>
        public bool checkDifficulty(Receipe receipe)
        {
            return (AdvancedDifficulty == 0) ||(AdvancedDifficulty == receipe.Difficulty.Value);
        }

        /// <summary>
        /// Checks the options.
        /// </summary>
        /// <param name="receipe">The receipe.</param>
        /// <returns></returns>
        public bool checkOptions(Receipe receipe)
        {
            return ((receipe.Vegetarian == AdvancedVegetarian) || (AdvancedVegetarian == false)) && ((receipe.WithAlcohol == AdvancedAlcool) || (AdvancedAlcool == true));
        }
        /// <summary>
        /// Checks the type.
        /// </summary>
        /// <param name="receipe">The receipe.</param>
        /// <returns></returns>
        public bool checkType(Receipe receipe)
        {
            return (AdvancedSearch == null || AdvancedSearch.Count == 0) || AdvancedSearch.Contains(receipe.DishType.Name);
           
        }
        /// <summary>
        /// Adds the receipe.
        /// </summary>
        /// <param name="receipe">The receipe.</param>
        /// <param name="model">The model.</param>
        public void addReceipe(Receipe receipe, AppModel model)
        {
            if (checkType(receipe))
            {
                if (checkDifficulty(receipe))
                {
                    if (checkOptions(receipe))
                    {
                        model.AddReceipe(receipe);
                    }
                }
               
            }
        }
        /// <summary>
        /// Checks the title.
        /// </summary>
        /// <param name="receipe">The receipe.</param>
        /// <param name="keyWord">The key word.</param>
        /// <returns></returns>
        public bool checkTitle(Receipe receipe,string keyWord)
        {
            return receipe.Title.ToUpper().IndexOf(keyWord.ToUpper()) >= 0;
        }

        /// <summary>
        /// Checks the instructions.
        /// </summary>
        /// <param name="receipe">The receipe.</param>
        /// <param name="keyWord">The key word.</param>
        /// <returns></returns>
        public bool checkInstructions(Receipe receipe, string keyWord)
        {
            return (receipe.ToDoInstructions != null) && (receipe.ToDoInstructions.ToUpper().IndexOf(keyWord.ToUpper()) >= 0);
            
        }
        /// <summary>
        /// Checks the ingredients.
        /// </summary>
        /// <param name="receipe">The receipe.</param>
        /// <param name="keyWord">The key word.</param>
        /// <returns></returns>
        public bool checkIngredients(Receipe receipe, string keyWord)
        {
            bool containsKey = false;

            foreach (var ingredient in receipe.ingredients)
            {
                if (ingredient.name.ToUpper().IndexOf(keyWord.ToUpper()) >= 0 || ingredient.unity.ToUpper().IndexOf(keyWord.ToUpper()) >= 0)
                {
                    containsKey = true;

                }
            }
            return containsKey;

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="URLDataRetriever"/> class.
        /// </summary>
        public URLDataRetriever()
        {
            this.URL = "http://m.marmiton.org/webservices/json.svc/GetRecipeSearch?SiteId=1&KeyWord={0}&SearchType=0&ItemsPerPage={1}&StartIndex={2}";
            AdvancedDifficulty = 0;
        }
    }
}
