using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Data
{
    /// <summary>
    /// This is a class that represents the description of a receipe
    /// </summary>
    public class ReceipeDescription
    {
        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        public string Author { get; set; }
        /// <summary>
        /// Gets or sets the type of the dish.
        /// </summary>
        /// <value>
        /// The type of the dish.
        /// </value>
        public string DishType { get; set; }
        /// <summary>
        /// Gets or sets the publication date.
        /// </summary>
        /// <value>
        /// The publication date.
        /// </value>
        public string PublicationDate { get; set; }
        /// <summary>
        /// Gets or sets the vegetarien.
        /// </summary>
        /// <value>
        /// The vegetarien.
        /// </value>
        public string Vegetarien { get; set; }
        /// <summary>
        /// Gets or sets the alcool.
        /// </summary>
        /// <value>
        /// The alcool.
        /// </value>
        public string Alcool { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        /// <value>
        /// The cost.
        /// </value>
        public string Cost { get; set; }
        /// <summary>
        /// Gets or sets the difficulty.
        /// </summary>
        /// <value>
        /// The difficulty.
        /// </value>
        public string Difficulty { get; set; }
        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        /// <value>
        /// The rating.
        /// </value>
        public string Rating { get; set; }
        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public string Image { get; set; }
        /// <summary>
        /// Gets or sets the alcool vege.
        /// </summary>
        /// <value>
        /// The alcool vege.
        /// </value>
        public string AlcoolVege { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReceipeDescription"/> class.
        /// </summary>
        /// <param name="receipe">The receipe.</param>
        public ReceipeDescription(Receipe receipe)
        {
            if (receipe.Author == String.Empty || receipe == null)
            {
                this.Author = "Auteur : -";
            }
            else
            {
                this.Author = "Auteur : " + receipe.Author;
            }

            this.PublicationDate = "Publié le : " + receipe.PublicationDate.ToString("d");
            this.Description = receipe.Description;

            if (receipe.WithAlcohol)
            {
                this.Alcool = "Avec alcool";
            }
            else
            {
                this.Alcool = "Sans alcool";
            }

            if (receipe.Vegetarian)
            {
                this.Vegetarien = "Plat végétarien";
            }
            else
            {
                this.Vegetarien = "Plat non végétarien";
            }

            this.Difficulty = receipe.Difficulty.ToString();
            this.Cost = receipe.Cost.ToString();
            this.Rating = receipe.Rating.ToString();
            this.Image = receipe.Image;
            this.DishType = "Type de plat : "+receipe.DishType.Name;
            this.AlcoolVege = this.Alcool +" - "+this.Vegetarien;
        }
    }
}
