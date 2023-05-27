using System.ComponentModel.DataAnnotations;

namespace MyAPI.Models.Dto
{
    public class VillaDto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(30)]
        public string Description { get; set; }
        [Required]
        public string Rate { get; set; }

        public int PinCode { get; set; }

        public string ImageUrl { get; set; }
    }
}
