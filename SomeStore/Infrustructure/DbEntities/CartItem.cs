namespace SomeStore.Infrustructure.DbEntities
{
    public class CartItem: BaseEntity
    {
        public virtual Product Product { get; set; }

        public int Quantity { get; set; }
    }
}