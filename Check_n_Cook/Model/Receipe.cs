using Check_n_Cook.Model.Grade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Check_n_Cook.Model
{
    public class Receipe
    {
        public int Id { get; set; }

        public String Title { get; set; }

        public String Author { get; set; }

        public DateTime PublicationDate { get; set; }

        public DishType DishType { get; set; }

        public Rating Rating { get; set; }

        public Difficulty Difficulty { get; set; }

        public Cost Cost { get; set; }

        public bool Vegetarian { get; set; }

        public string HtmlReceipe { get; set; }

        public string ToDoInstructions { get; set; }

        public string ingredientsHTML { get; set; }

        public bool WithAlcohol { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public PlanningEntry PlanningEntry { get; set; }



        public Receipe() : this("", null, DateTime.Now, null, 0, 0, 0, false, false) {}

        public Receipe(String title, String author, DateTime publicationDate, DishType dishType, int rating, int difficulty, int cost, bool vegetarian, bool withAlcohol)
        {
            this.Title = title;
            this.Author = author;
            this.PublicationDate = publicationDate;
            this.DishType = dishType;
            this.Rating = new Rating(rating);
            this.Difficulty = new Difficulty(difficulty);
            this.Cost = new Cost(cost);
            this.Vegetarian = vegetarian;
            this.WithAlcohol = withAlcohol;
            this.PlanningEntry = null;
        }
        public Receipe(String title, String author, DateTime publicationDate, DishType dishType, int rating, int difficulty, int cost, bool vegetarian, bool withAlcohol, PlanningEntry planEntry)
        {
            this.Title = title;
            this.Author = author;
            this.PublicationDate = publicationDate;
            this.DishType = dishType;
            this.Rating = new Rating(rating);
            this.Difficulty = new Difficulty(difficulty);
            this.Cost = new Cost(cost);
            this.Vegetarian = vegetarian;
            this.WithAlcohol = withAlcohol;
            this.PlanningEntry = planEntry;
        }

        public void AddToPlan(PlanningEntry planEntry) {







        }
        public void RemovePlanEntry()
        {







        }


        public Receipe(String title, String author, DateTime publicationDate, DishType dishType, int rating, int difficulty, int cost, bool vegetarian, bool withAlcohol, string image) :
            this(title, author, publicationDate, dishType, rating, difficulty, cost, vegetarian, withAlcohol)
        {
            this.Image = image;
        }

        public String Stringify()
        {
            JsonArray jsonArray = new JsonArray();

            JsonObject jsonObject = new JsonObject();
            jsonObject["Id"] = JsonValue.CreateNumberValue(this.Id);
            jsonObject["Title"] = JsonValue.CreateStringValue(this.Title);
            /*jsonObject["PublicationDate"] = JsonValue.CreateStringValue(this.PublicationDate.ToString());
            jsonObject["ToDoInstructions"] = JsonValue.CreateStringValue(this.ToDoInstructions):*/

            return jsonObject.Stringify();
        }

        public JsonObject ToJsonObject()
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject.SetNamedValue("Id", JsonValue.CreateNumberValue(this.Id));
            jsonObject.SetNamedValue("Title", JsonValue.CreateStringValue(this.Title));
            jsonObject.SetNamedValue("Image", JsonValue.CreateStringValue(this.Image));

            return jsonObject;
        }

        public Receipe(String jsonString)
        {
            JsonObject jsonObject = JsonObject.Parse(jsonString);
            this.Id = (int)jsonObject.GetNamedNumber("Id", 0.0);
            this.Title = jsonObject.GetNamedString("Title", "");
            this.Image = jsonObject.GetNamedString("Image", "");
            this.ToDoInstructions = "";
        }
    }
}
