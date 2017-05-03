using System.ComponentModel.DataAnnotations;
using System.Web;
using SomeStore.Infrustructure;
using SomeStore.Infrustructure.DbEntities;
using SomeStore.Infrustructure.Helpers;

namespace SomeStore.Models
{
    public class ProductEditViewModel
    {
        public ProductEditViewModel()
        {
        }

        public ProductEditViewModel(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            ImageUrl = product.ImageUrl;
        }

        #region Properties
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public HttpPostedFileBase Image { get; set; }

        public string ImageUrl { get; set; }

        [Range(0.01, 10000)]
        public decimal Price { get; set; }
        #endregion

        #region Methods
        public Product GetProduct()
        {
            var imgPath = ImageHelper.SaveImage(Image);

            return new Product
            {
                Id = Id,
                Name = Name,
                Description = Description,
                ImageUrl = string.IsNullOrWhiteSpace(imgPath) ? ImageUrl : imgPath,
                Price = Price
            };
        }
        #endregion
    }
}