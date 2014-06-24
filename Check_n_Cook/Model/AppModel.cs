using Check_n_Cook.Events;
using Check_n_Cook.Model.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace Check_n_Cook.Model
{
    /// <summary>
    /// Class that contains all the data needed for the application.
    /// </summary>
    public class AppModel : AbstractModel
    {
        /// <summary>
        /// Gets or sets the receipes. These are the results of the searches.
        /// </summary>
        /// <value>
        /// The receipes that were found.
        /// </value>
        public List<Receipe> Receipes { get; set; }

        /// <summary>
        /// Gets or sets the shops. Used to store the closest shops the user's position.
        /// </summary>
        /// <value>
        /// The shops.
        /// </value>
        public List<Shop> Shops { get; set; }

        /// <summary>
        /// Gets or sets the favourite receipes.
        /// </summary>
        /// <value>
        /// The favourite receipes.
        /// </value>
        public Dictionary<string, Receipe> FavouriteReceipes { get; set; }

        /// <summary>
        /// Gets or sets the selected receipe. Used when the user click on a receipe to have details on it.
        /// </summary>
        /// <value>
        /// The selected receipe.
        /// </value>
        public Receipe SelectedReceipe { get; set; }

        /// <summary>
        /// Gets or sets the receipe list. Used on the user's planning of receipes. The receipes are organised by date and moment of day.
        /// </summary>
        /// <value>
        /// The receipe list.
        /// </value>
        public Dictionary<string, ReceipeDate> ReceipeList { get; set; }

        /// <summary>
        /// Gets or sets the shopping list. The list contains contains ingredients organised by groups (often receipes).
        /// </summary>
        /// <value>
        /// The shopping list.
        /// </value>
        public Dictionary<string, ShoppingListGroup> ShoppingList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppModel"/> class.
        /// </summary>
        public AppModel()
        {
            this.Receipes = new List<Receipe>();
            this.Shops = new List<Shop>();
            this.FavouriteReceipes = new Dictionary<string, Receipe>();
            this.ReceipeList = new Dictionary<string, ReceipeDate>();
            this.SelectedReceipe = null;
            this.ShoppingList = new Dictionary<string, ShoppingListGroup>();
        }

        /// <summary>
        /// Adds a receipe to the list of search results..
        /// </summary>
        /// <param name="receipe">The receipe.</param>
        public void AddReceipe(Receipe receipe)
        {
            this.Receipes.Add(receipe);

            this.RefreshViews(new AddedReceipeEvent(this, receipe));
        }

        /// <summary>
        /// Removes a receipe from the list of search results..
        /// </summary>
        /// <param name="receipe">The receipe.</param>
        public void RemoveReceipe(Receipe receipe)
        {
            this.Receipes.Remove(receipe);
            this.RefreshViews(new RemovedReceipeEvent(this, receipe));
        }

        /// <summary>
        /// Adds a favorite receipe.
        /// </summary>
        /// <param name="receipe">The receipe.</param>
        public void AddFavoriteReceipe(Receipe receipe)
        {
            this.FavouriteReceipes[receipe.Title] = receipe;
        }

        /// <summary>
        /// Removes a favorite receipe.
        /// </summary>
        /// <param name="receipe">The receipe.</param>
        public void RemoveFavoriteReceipe(Receipe receipe)
        {
            this.FavouriteReceipes.Remove(receipe.Title);
        }

        /// <summary>
        /// Adds a receipe to the planning.
        /// </summary>
        /// <param name="receipe">The receipe.</param>
        /// <param name="timeOfDay">The time of day.</param>
        /// <param name="date">The date.</param>
        public void AddReceipeList(Receipe receipe, string timeOfDay, string date)
        {
            ReceipeDate receipeDate = null;

            if (ReceipeList.ContainsKey(date))
            {
                receipeDate = ReceipeList[date];
            }
            else
            {
                receipeDate = new ReceipeDate(date);
                ReceipeList.Add(date, receipeDate);
            }

            receipeDate.ReceipeTimeOfDay[timeOfDay].AddReceipe(receipe);
        }

        /// <summary>
        /// Removes all the receipes from the planning at the given time.
        /// </summary>
        /// <param name="time">The time.</param>
        public void RemoveReceipeDate(Time time)
        {
            if (this.ReceipeList.ContainsKey(time.Date))
            {
                ReceipeDate receipeDate = this.ReceipeList[time.Date];
                this.ReceipeList.Remove(time.Date);
                this.RefreshViews(new RemovedReceipeDateEvent(this));
            }
        }

        /// <summary>
        /// Removes a receipe from the planning.
        /// </summary>
        /// <param name="receipe">The receipe.</param>
        /// <param name="timeOfDay">The time of day.</param>
        /// <param name="date">The date.</param>
        public void RemoveReceipeList(Receipe receipe, string timeOfDay, string date)
        {
            if (this.ReceipeList.Count != 0 && this.ReceipeList[date] != null && this.ReceipeList[date].ReceipeTimeOfDay[timeOfDay] != null)
            {
                this.ReceipeList[date].ReceipeTimeOfDay[timeOfDay].RemoveReceipe(receipe);
                bool deletedReceipeDate = true;
                foreach (ReceipeTimeOfDay receipeTimeOfDay in this.ReceipeList[date].ReceipeTimeOfDay.Values)
                {
                    if (receipeTimeOfDay.Receipes.Count != 0)
                    {
                        deletedReceipeDate = false;
                    }
                }

                if (deletedReceipeDate)
                {
                    this.ReceipeList.Remove(date);
                }
                this.RefreshViews(new RemovedReceipeListEvent(this, receipe, new Time(date, timeOfDay)));
            }
        }

        /// <summary>
        /// Clears the list of receipes containing the search results.
        /// </summary>
        public void ClearReceipes()
        {
            this.Receipes.Clear();
            this.RefreshViews(new ClearedReceipeEvent(this));
        }

        /// <summary>
        /// Selects a receipe.
        /// </summary>
        /// <param name="receipe">The receipe.</param>
        public void SelectReceipe(Receipe receipe)
        {
            this.SelectedReceipe = receipe;
            this.RefreshViews(new SelectedReceipeEvent(this, receipe));
        }
        /// <summary>
        /// Adds a shop to the list of close shops.
        /// </summary>
        /// <param name="shop">The shop.</param>
        public void AddShop(Shop shop)
        {
            this.Shops.Add(shop);
            this.RefreshViews(new AddedShopEvent(this, shop));
        }

        /// <summary>
        /// Removes a shop from the list of close shops.
        /// </summary>
        /// <param name="shop">The shop.</param>
        public void RemoveShop(Shop shop)
        {
            this.Shops.Remove(shop);
            this.RefreshViews(new RemovedShopEvent(this, shop));
        }

        /// <summary>
        /// Adds a group to the shopping list, containing all the ingredients of a receipe.
        /// </summary>
        /// <param name="receipe">The receipe.</param>
        public void CreateShoppingList(Receipe receipe)
        {
            if (!this.ShoppingList.ContainsKey(receipe.Title))
            {
                this.ShoppingList.Add(receipe.Title, new ShoppingListGroup(receipe.Title));

                foreach (Ingredient ing in receipe.ingredients)
                {
                    this.AddIngredientToShoppingList(ing, receipe.Title);
                }
            }
        }

        /// <summary>
        /// Adds a group to the shopping list.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        public void AddShoppingListGroup(String groupName)
        {
            if (!this.ShoppingList.ContainsKey(groupName))
            {
                this.ShoppingList.Add(groupName, new ShoppingListGroup(groupName));
                this.RefreshViews(new AddedShoppingListGroupEvent(this, groupName));
            }
        }

        /// <summary>
        /// Removes a group from the shopping list.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        public void RemoveShoppingListGroup(String groupName)
        {
            if (this.ShoppingList.ContainsKey(groupName))
            {
                this.ShoppingList.Remove(groupName);
                this.RefreshViews(new RemovedShoppingListGroupEvent(this, groupName));
            }
        }

        /// <summary>
        /// Adds an ingredient to the shopping list, in the group with the given name.
        /// </summary>
        /// <param name="ingredient">The ingredient.</param>
        /// <param name="groupName">Name of the group.</param>
        public void AddIngredientToShoppingList(Ingredient ingredient, String groupName)
        {
            ItemIngredient itemIngredient = ToolItem.CreateItemIngredient(ingredient);
            itemIngredient.Group = groupName;
            this.ShoppingList[groupName].Items.Add(itemIngredient);
            this.RefreshViews(new AddedIngredientEvent(this, ingredient, groupName));
        }

        /// <summary>
        /// Removes an ingredient from the shopping list, in the group with the given name.
        /// </summary>
        /// <param name="itemIngredient">The item ingredient.</param>
        /// <param name="groupName">Name of the group.</param>
        public void RemoveIngredientFromShoppingList(ItemIngredient itemIngredient, String groupName)
        {
            this.ShoppingList[groupName].Items.Remove(itemIngredient);
            this.RefreshViews(new RemovedIngredientEvent(this, itemIngredient.Ingredient, groupName));
        }

        /// <summary>
        /// Gets or sets the local receipes.
        /// </summary>
        /// <value>
        /// The local receipes.
        /// </value>
        public JsonArray LocalReceipes{get; set;}

        /// <summary>
        /// Extracts the personnal receipes.
        /// </summary>
        /// <returns>true if an error occured, false otherwise</returns>
        public async Task<bool> ExtractPersonnalReceipes()
        {
            StorageFolder folder = KnownFolders.PicturesLibrary;
            bool error = false;
            try
            {
                StorageFile receipesFile = await folder.GetFileAsync("personalReceipes.json");
                String jsonString = await FileIO.ReadTextAsync(receipesFile);
                JsonObject jsonObject = JsonObject.Parse(jsonString);
                JsonArray jsonArray = jsonObject.GetNamedArray("Receipes");
                this.LocalReceipes = jsonArray;
            }
            catch
            {
                error = true;
            }
            return error;
        }

        /// <summary>
        /// Stringifies the favourite receipes, and converts them to a JSON string.
        /// </summary>
        /// <returns>a JSON string of the favourite receipes</returns>
        public String StringifyFavouriteReceipes()
        {
            JsonObject jsonObject = new JsonObject();
            JsonArray jsonArray = new JsonArray();

            foreach (Receipe receipe in this.FavouriteReceipes.Values)
            {
                jsonArray.Add(receipe.ToJsonObject());
            }

            jsonObject["Receipes"] = jsonArray;

            return jsonObject.Stringify();
        }

        /// <summary>
        /// Stringifies the planning of receipes, and converts it to a JSON string.
        /// </summary>
        /// <returns>a JSON string of the planning of receipes</returns>
        public String StringifyReceipesList()
        {
            JsonObject jsonObject = new JsonObject();
            JsonArray jsonArray = new JsonArray();

            foreach (ReceipeDate receipeDate in this.ReceipeList.Values)
            {
                jsonArray.Add(receipeDate.ToJsonObject());
            }

            jsonObject["Receipes"] = jsonArray;

            return jsonObject.Stringify();
        }

        /// <summary>
        /// Stringifies the shopping list, and converts them to a JSON string.
        /// </summary>
        /// <returns>a JSON string of the shopping list</returns>
        public String StringifyShoppingList()
        {
            JsonObject jsonObject = new JsonObject();
            JsonArray jsonArray = new JsonArray();

            foreach (ShoppingListGroup group in this.ShoppingList.Values)
            {
                JsonObject groupObject = new JsonObject();
                JsonArray ingredientArray = new JsonArray();

                foreach (ItemIngredient itemIngredient in group.Items)
                {
                    ingredientArray.Add(itemIngredient.Ingredient.ToJsonObject());
                }

                groupObject["Name"] = JsonValue.CreateStringValue(group.Name);
                groupObject["Ingredients"] = ingredientArray;

                jsonArray.Add(groupObject);
            }

            jsonObject["ShoppingList"] = jsonArray;

            return jsonObject.Stringify();
        }
    }
}
