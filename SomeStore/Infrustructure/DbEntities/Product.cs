using System.ComponentModel.DataAnnotations;

namespace SomeStore.Infrustructure.DbEntities
{
    public class Product : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0.01, 10000)]
        public decimal Price { get; set; }

        [Required]
        public string ImageUrl { get; set; }
    }
}
