﻿using Check_n_Cook.Events;
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

        public List<Ingredient> ShoppingList { get; set; }

        public AppModel()
        {
            this.Receipes = new List<Receipe>();
            this.DishTypes = new List<DishType>();
            this.Shops = new List<Shop>();
            this.FavouriteReceipes = new Dictionary<string, Receipe>();
            this.ReceipeList = new Dictionary<string, ReceipeDate>();
            this.SelectedReceipe = null;
            this.ShoppingList = new List<Ingredient>();
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

        public void AddReceipeFavorite(Receipe receipe)
        {
            this.FavouriteReceipes[receipe.Title] = receipe;
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
                /*
                foreach (ReceipeTimeOfDay receipeTimeOfDay in receipeDate.ReceipeTimeOfDay.Values)
                {
                    Time time2 = receipeTimeOfDay.Time;
                    time2.Date = time.Date;

                    foreach (Receipe receipe in receipeTimeOfDay.Receipes.Values)
                    {
                       
                        this.ReceipeList[time2.Date].ReceipeTimeOfDay[time2.TimeOfDay].RemoveReceipe(receipe);
                    }
                }
                */
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

        public void AddIngredientToShoppingList(Ingredient ingredient)
        {
            this.ShoppingList.Add(ingredient);
            this.RefreshViews(new AddedIngredientEvent(this, ingredient));
        }

        public void RemoveIngredientFromShoppingList(Ingredient ingredient)
        {
            this.ShoppingList.Remove(ingredient);
            this.RefreshViews(new RemovedIngredientEvent(this, ingredient));
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

            foreach (Ingredient ingredient in this.ShoppingList)
            {
                jsonArray.Add(ingredient.ToJsonObject());
            }

            jsonObject["Ingredients"] = jsonArray;

            return jsonObject.Stringify();
        }
    }
}
