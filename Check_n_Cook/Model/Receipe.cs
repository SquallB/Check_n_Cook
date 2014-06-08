using Check_n_Cook.Model.Grade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model
{
    public class Receipe
    {
        public int Id { get; set; }

        public String Title { get; set; }

        public User Author { get; set; }

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


        public string image { get; set; }

        public Receipe() : this("", null, DateTime.Now, null, 0, 0, 0, false, false) {}

        public Receipe(String title, User author, DateTime publicationDate, DishType dishType, int rating, int difficulty, int cost, bool vegetarian, bool withAlcohol)
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
        }
    }
}
