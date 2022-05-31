using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace Ap204_Pronia.Models
{
    public class Plant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string SKUCode { get; set; }
        public string Shipping { get; set; }
        public string Request { get; set; }
        public string Guarantee { get; set; }
        public int? ColorId { get; set; }
        public Color Color { get; set; }
        public int? SizeId { get; set; }
        public Size Size { get; set; }

        public List<PlantImage> PlantImages { get; set; }
        public List<PlantCategory> PlantCategories { get; set; }


        [NotMapped]
        public IFormFile MainImage { get; set; }
        [NotMapped]
        public List< IFormFile> AnotherImage { get; set; }
        [NotMapped]

        public List<int> ImageIds { get; set; }

        [NotMapped]

        public List<int> CategoryIds { get; set; }
    }
}
