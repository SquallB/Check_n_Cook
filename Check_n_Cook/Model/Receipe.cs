using Check_n_Cook.Model.Grade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using System.Runtime.Serialization.Json;
using System.IO;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Runtime.Serialization;

namespace Check_n_Cook.Model
{
    [DataContract]
    public class Receipe
    {
        public int Id { get; set; }

        public String Title { get; set; }

        public User Author { get; set; }

        public DateTime PublicationDate { get; set; }

        public DishType DishType { get; set; }

        public Rating Rating { get; set; }

        public Difficulty Difficulty { get; set; }

        public int fileId { get; set; }

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
            this.PlanningEntry = null;
            AddToPlan(new PlanningEntry(DateTime.Today, PlanningType.MIDI));

        }
        public Receipe(String title, User author, DateTime publicationDate, DishType dishType, int rating, int difficulty, int cost, bool vegetarian, bool withAlcohol, PlanningEntry planEntry)
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

        public async Task<bool> FileExistsAsync(StorageFolder folder, string fileName)
        {
            try
            {
                await folder.GetFileAsync(fileName);
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }
        public async void AddToPlan(PlanningEntry planEntry) {



            MemoryStream stream1 = new MemoryStream();

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Receipe));

            ser.WriteObject(stream1, this);
            StringBuilder sb = new StringBuilder();

                using (StreamReader sr = new StreamReader(stream1))
                {
                    sb.AppendLine(stream1.ToString());

                }
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                //StorageFile file = await StorageFile.GetFileFromApplicationUriAsync("ms-appdata:///local/file.txt"); 
                // StreamWriteTo.WriteTo(stream1, "test.json");
                StorageFile file;
                int i = 1;

                string filename = "receipe" + i + ".json";
                    file = null;
                    bool isFinished = false;
                    while (await FileExistsAsync(ApplicationData.Current.RoamingFolder, filename) != false && isFinished != true)
                    {
                        i++;
                        filename = "receipe" + i + ".json";
                        if (await FileExistsAsync(ApplicationData.Current.RoamingFolder, filename) == false)
                        {
                            isFinished = true;
                            await FileIO.WriteTextAsync(await ApplicationData.Current.RoamingFolder.GetFileAsync(filename), sb.ToString());

                        }
                    }
                
                
        }
        public void RemovePlanEntry()
        {







        }


        public Receipe(String title, User author, DateTime publicationDate, DishType dishType, int rating, int difficulty, int cost, bool vegetarian, bool withAlcohol, string image) :
            this(title, author, publicationDate, dishType, rating, difficulty, cost, vegetarian, withAlcohol)
        {
            this.Image = image;
        }
    }
}
