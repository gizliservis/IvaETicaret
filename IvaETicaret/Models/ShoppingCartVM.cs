namespace IvaETicaret.Models
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingKart> ListCart { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
