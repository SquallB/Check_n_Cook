using Check_n_Cook.Events;
using System;
using System.Collections.Generic;
using Windows.Data.Json;

namespace Check_n_Cook.Model
{
    public class AppModel : AbstractModel
    {
        public List<Receipe> Receipes { get; set; }

        public List<DishType> DishTypes { get; set; }

        public List<Shop> Shops { get; set; }

        public List<Receipe> FavouriteReceipes { get; set; }

        public Receipe SelectedReceipe { get; set; }

        public Dictionary<string, ReceipeDate> ReceipeList { get; set; }

        public AppModel()
        {
            this.Receipes = new List<Receipe>();
            this.DishTypes = new List<DishType>();
            this.Shops = new List<Shop>();
            this.FavouriteReceipes = new List<Receipe>();
            this.ReceipeList = new Dictionary<string, ReceipeDate>();
            this.SelectedReceipe = null;
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

            receipeDate.ReceipeTimeOfDay[timeOfDay].Receipes.Add(receipe);
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

        public String StringifyFavouriteReceipes()
        {
            JsonObject jsonObject = new JsonObject();
            JsonArray jsonArray = new JsonArray();

            foreach (Receipe receipe in this.FavouriteReceipes)
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
    }
}
