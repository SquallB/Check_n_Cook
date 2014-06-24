using Check_n_Cook.Model.Grade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Check_n_Cook.Model
{
    /// <summary>
    /// Represents a receipe.
    /// </summary>
    public class Receipe
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public String Title { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        public String Author { get; set; }

        /// <summary>
        /// Gets or sets the date of publication.
        /// </summary>
        /// <value>
        /// The date of publication.
        /// </value>
        public DateTime PublicationDate { get; set; }

        /// <summary>
        /// Gets or sets the type of the dish.
        /// </summary>
        /// <value>
        /// The type of the dish.
        /// </value>
        public DishType DishType { get; set; }

        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        /// <value>
        /// The rating.
        /// </value>
        public Rating Rating { get; set; }

        /// <summary>
        /// Gets or sets the difficulty.
        /// </summary>
        /// <value>
        /// The difficulty.
        /// </value>
        public Difficulty Difficulty { get; set; }

        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        /// <value>
        /// The cost.
        /// </value>
        public Cost Cost { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Receipe"/> is vegetarian.
        /// </summary>
        /// <value>
        ///   <c>true</c> if vegetarian; otherwise, <c>false</c>.
        /// </value>
        public bool Vegetarian { get; set; }

        /// <summary>
        /// Gets or sets the HTML receipe.
        /// </summary>
        /// <value>
        /// The HTML receipe.
        /// </value>
        public string HtmlReceipe { get; set; }

        /// <summary>
        /// Gets or sets the instructions.
        /// </summary>
        /// <value>
        /// The instructions.
        /// </value>
        public string ToDoInstructions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [with alcohol].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [with alcohol]; otherwise, <c>false</c>.
        /// </value>
        public bool WithAlcohol { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the planning entry.
        /// </summary>
        /// <value>
        /// The planning entry.
        /// </value>
        public PlanningEntry PlanningEntry { get; set; }

        /// <summary>
        /// The ingredients
        /// </summary>
        public List<Ingredient> ingredients;

        /// <summary>
        /// Initializes a new instance of the <see cref="Receipe"/> class.
        /// </summary>
        public Receipe() : this("", "-", DateTime.Now, null, 0, 0, 0, false, false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Receipe"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="author">The author.</param>
        /// <param name="publicationDate">The publication date.</param>
        /// <param name="dishType">Type of the dish.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="difficulty">The difficulty.</param>
        /// <param name="cost">The cost.</param>
        /// <param name="vegetarian">if set to <c>true</c> [vegetarian].</param>
        /// <param name="withAlcohol">if set to <c>true</c> [with alcohol].</param>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Receipe"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="author">The author.</param>
        /// <param name="publicationDate">The publication date.</param>
        /// <param name="dishType">Type of the dish.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="difficulty">The difficulty.</param>
        /// <param name="cost">The cost.</param>
        /// <param name="vegetarian">if set to <c>true</c> [vegetarian].</param>
        /// <param name="withAlcohol">if set to <c>true</c> [with alcohol].</param>
        /// <param name="planEntry">The plan entry.</param>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Receipe"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="author">The author.</param>
        /// <param name="publicationDate">The publication date.</param>
        /// <param name="dishType">Type of the dish.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="difficulty">The difficulty.</param>
        /// <param name="cost">The cost.</param>
        /// <param name="vegetarian">if set to <c>true</c> [vegetarian].</param>
        /// <param name="withAlcohol">if set to <c>true</c> [with alcohol].</param>
        /// <param name="image">The image.</param>
        public Receipe(String title, String author, DateTime publicationDate, DishType dishType, int rating, int difficulty, int cost, bool vegetarian, bool withAlcohol, string image) :
            this(title, author, publicationDate, dishType, rating, difficulty, cost, vegetarian, withAlcohol)
        {
            this.Image = image;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Receipe"/> class.
        /// </summary>
        /// <param name="jsonString">The json string.</param>
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

        /// <summary>
        /// Creates and returns a JSON Object of the instance.
        /// </summary>
        /// <returns>a JSON object of the instance</returns>
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
    }
}
