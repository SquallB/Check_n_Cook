using Check_n_Cook.Events;
using Check_n_Cook.Model.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace Check_n_Cook.Model
{
    public class AppModel : AbstractModel
    {
        public List<Receipe> Receipes { get; set; }

        public List<DishType> DishTypes { get; set; }

        public List<Shop> Shops { get; set; }

        public Dictionary<string, Receipe> FavouriteReceipes { get; set; }

        public Receipe SelectedReceipe { get; set; }

        public Dictionary<string, ReceipeDate> ReceipeList { get; set; }

        public Dictionary<string, ShoppingListGroup> ShoppingList { get; set; }

        public List<ItemReceipe> PreviousReceipeSearch { get; set; }
        public AppModel()
        {
            this.Receipes = new List<Receipe>();
            this.DishTypes = new List<DishType>();
            this.Shops = new List<Shop>();
            this.FavouriteReceipes = new Dictionary<string, Receipe>();
            this.ReceipeList = new Dictionary<string, ReceipeDate>();
            this.SelectedReceipe = null;
            this.ShoppingList = new Dictionary<string, ShoppingListGroup>();
            this.PreviousReceipeSearch = new List<ItemReceipe>();
        }

        public void AddReceipe(Receipe receipe)
        {
            this.Receipes.Add(receipe);

            if (!this.DishTypes.Contains(receipe.DishType))
            {
                this.DishTypes.Add(receipe.DishType);
            }


            this.RefreshViews(new AddedReceipeEvent(this, receipe));
        }

        public void AddFavoriteReceipe(Receipe receipe)
        {
            this.FavouriteReceipes[receipe.Title] = receipe;
        }

        public void RemoveFavoriteReceipe(Receipe receipe)
        {
            this.FavouriteReceipes.Remove(receipe.Title);
        }

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

        public void RemoveReceipeDate(Time time)
        {
            if (this.ReceipeList.ContainsKey(time.Date))
            {
                ReceipeDate receipeDate = this.ReceipeList[time.Date];
                this.ReceipeList.Remove(time.Date);
                this.RefreshViews(new RemovedReceipeDateEvent(this));
            }
        }

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

        public void RemoveReceipe(Receipe receipe)
        {
            this.Receipes.Remove(receipe);
            this.RefreshViews(new RemovedReceipeEvent(this, receipe));
        }

        public void ClearReceipes()
        {
            this.Receipes.Clear();
            this.RefreshViews(new ClearedReceipeEvent(this));
        }


        public void SelectReceipe(Receipe receipe)
        {
            this.SelectedReceipe = receipe;
            this.RefreshViews(new SelectedReceipeEvent(this, receipe));
        }
        public void AddShop(Shop shop)
        {
            this.Shops.Add(shop);
            this.RefreshViews(new AddedShopEvent(this, shop));
        }

        public void RemoveShop(Shop shop)
        {
            this.Shops.Remove(shop);
            this.RefreshViews(new RemovedShopEvent(this, shop));
        }
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

        public void AddShoppingListGroup(String groupName)
        {
            if (!this.ShoppingList.ContainsKey(groupName))
            {
                this.ShoppingList.Add(groupName, new ShoppingListGroup(groupName));
                this.RefreshViews(new AddedShoppingListGroupEvent(this, groupName));
            }
        }

        public void RemoveShoppingListGroup(String groupName)
        {
            if (this.ShoppingList.ContainsKey(groupName))
            {
                this.ShoppingList.Remove(groupName);
                this.RefreshViews(new RemovedShoppingListGroupEvent(this, groupName));
            }
        }

        public void AddIngredientToShoppingList(Ingredient ingredient, String groupName)
        {
            ItemIngredient itemIngredient = ToolItem.CreateItemIngredient(ingredient);
            itemIngredient.Group = groupName;
            this.ShoppingList[groupName].Items.Add(itemIngredient);
            this.RefreshViews(new AddedIngredientEvent(this, ingredient, groupName));
        }

        public void RemoveIngredientFromShoppingList(ItemIngredient itemIngredient, String groupName)
        {
            this.ShoppingList[groupName].Items.Remove(itemIngredient);
            this.RefreshViews(new RemovedIngredientEvent(this, itemIngredient.Ingredient, groupName));
        }

        public JsonArray LocalReceipes{get; set;}
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
