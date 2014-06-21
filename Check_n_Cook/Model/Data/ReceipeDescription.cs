using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Data
{
    public class ReceipeDescription
    {
        public string Author { get; set; }
        public string DishType { get; set; }
        public string PublicationDate { get; set; }
        public string Vegetarien { get; set; }
        public string Alcool { get; set; }
        public string Description { get; set; }
        public string Cost { get; set; }
        public string Difficulty { get; set; }
        public string Rating { get; set; }
        public string Image { get; set; }
        public string AlcoolVege { get; set; }
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
