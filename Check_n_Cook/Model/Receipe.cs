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

        public string IngredientsHTML { get; set; }

        public bool WithAlcohol { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public PlanningEntry PlanningEntry { get; set; }

        public List<Ingredient> ingredients;




        public Receipe() : this("", null, DateTime.Now, null, 0, 0, 0, false, false) { }

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
            this.ingredients = new List<Ingredient>();

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
            this.ingredients = new List<Ingredient>();

        }

        public void AddToPlan(PlanningEntry planEntry)
        {







        }
        public void RemovePlanEntry()
        {







        }


        public Receipe(String title, String author, DateTime publicationDate, DishType dishType, int rating, int difficulty, int cost, bool vegetarian, bool withAlcohol, string image) :
            this(title, author, publicationDate, dishType, rating, difficulty, cost, vegetarian, withAlcohol)
        {
            this.Image = image;
        }

        public Receipe(String jsonString)
        {
            JsonObject jsonObject = JsonObject.Parse(jsonString);
            this.Id = (int)jsonObject.GetNamedNumber("Id");
            this.Title = jsonObject.GetNamedString("Title");
            this.Author = jsonObject.GetNamedString("Author");
            this.PublicationDate = Convert.ToDateTime(jsonObject.GetNamedString("PublicationDate"));
            this.DishType = DishType.GetInstance(jsonObject.GetNamedObject("DishType").GetNamedString("Name"));
            this.Rating = new Rating(jsonObject.GetNamedObject("Rating").Stringify());
            this.Difficulty = new Difficulty(jsonObject.GetNamedObject("Difficulty").Stringify());
            this.Cost = new Cost(jsonObject.GetNamedObject("Cost").Stringify());
            this.Vegetarian = jsonObject.GetNamedBoolean("Vegetarian");
            this.HtmlReceipe = jsonObject.GetNamedString("HtmlReceipe");
            try
            {
                if (jsonObject.GetNamedString("ToDoInstructions") != null)
                {
                    this.ToDoInstructions = jsonObject.GetNamedString("ToDoInstructions");

                }

            }
            catch
            {

            }
            // this.IngredientsHTML = jsonObject.GetNamedString("IngredientsHTML");
            this.WithAlcohol = jsonObject.GetNamedBoolean("WithAlcohol");
            this.Image = jsonObject.GetNamedString("Image");
            this.Description = jsonObject.GetNamedString("Description");

            this.ingredients = new List<Ingredient>();

            JsonArray ingredientsJson = jsonObject.GetNamedArray("Ingredients");
            foreach (var ingredientObject in ingredientsJson)
            {
                Ingredient ingredient = new Ingredient(JsonObject.Parse(ingredientObject.Stringify()));
                ingredients.Add(ingredient);

            }
        }

        public JsonObject ToJsonObject()
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject.SetNamedValue("Id", JsonValue.CreateNumberValue(this.Id));
            jsonObject.SetNamedValue("Title", JsonValue.CreateStringValue(this.Title));
            jsonObject.SetNamedValue("Author", JsonValue.CreateStringValue(this.Author));
            jsonObject.SetNamedValue("PublicationDate", JsonValue.CreateStringValue(this.PublicationDate.ToString()));
            jsonObject.SetNamedValue("DishType", this.DishType.ToJsonObject());
            jsonObject.SetNamedValue("Rating", this.Rating.ToJsonObject());
            jsonObject.SetNamedValue("Difficulty", this.Difficulty.ToJsonObject());
            jsonObject.SetNamedValue("Cost", this.Cost.ToJsonObject());
            jsonObject.SetNamedValue("Vegetarian", JsonValue.CreateBooleanValue(this.Vegetarian));
            jsonObject.SetNamedValue("HtmlReceipe", JsonValue.CreateStringValue("HtmlReceipe"));
            if (this.ToDoInstructions != null && this.ToDoInstructions != "")
            {
                jsonObject.SetNamedValue("ToDoInstructions", JsonValue.CreateStringValue(this.ToDoInstructions));

            }
            //jsonObject.SetNamedValue("ToDoInstructions", JsonValue.CreateStringValue(this.ToDoInstructions));
            // jsonObject.SetNamedValue("IngredientsHTML", JsonValue.CreateStringValue(this.IngredientsHTML));
            jsonObject.SetNamedValue("WithAlcohol", JsonValue.CreateBooleanValue(this.WithAlcohol));
            jsonObject.SetNamedValue("Image", JsonValue.CreateStringValue(this.Image));
            jsonObject.SetNamedValue("Description", JsonValue.CreateStringValue("Description"));

            JsonArray ingredientsJson = new JsonArray();

            foreach (Ingredient ingredient in ingredients)
            {
                ingredientsJson.Add(ingredient.ToJsonObject());
            }
            jsonObject.SetNamedValue("Ingredients", ingredientsJson);

            return jsonObject;
        }

        public String Stringify()
        {
            return this.ToJsonObject().Stringify();
        }
    }
}
