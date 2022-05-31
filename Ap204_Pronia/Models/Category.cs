using System.Collections.Generic;

namespace Ap204_Pronia.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<PlantCategory> PlantCategories { get; set; }
    }
}
