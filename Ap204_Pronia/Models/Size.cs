using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace Ap204_Pronia.Models
{
    public class Size
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 10, ErrorMessage = "You can write maximum 10 charachter")]
        public string Name { get; set; }

        public List<Plant> Plants { get; set; }

        
    }
}
