using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SomeStore.Infrustructure.DbEntities
{
    public class ShoppingCart : BaseEntity
    {
        public virtual List<CartItem> CartItems { get; set; }

        [DisplayFormat(DataFormatString = "{0:F}")]
        public DateTime Created { get; set; }

        public bool IsOrdered { get; set; }
    }
}
