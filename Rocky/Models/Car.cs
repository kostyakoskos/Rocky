using System.ComponentModel.DataAnnotations;

namespace Rocky.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        [Range(1901,2021,ErrorMessage = "Year from 1901 to 2021")]
        public string Year { get; set; }

    }
}
