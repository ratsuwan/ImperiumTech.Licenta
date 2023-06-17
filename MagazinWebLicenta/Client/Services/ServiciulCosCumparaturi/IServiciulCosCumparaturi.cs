namespace MagazinWebLicenta.Client.Services.ServiciulCosCumparaturi
{
	public interface IServiciulCosCumparaturi
	{
		event Action OnChange;
		Task AddToCart(CartItem cartItem);
		Task<List<CartProductResponse>> GetCartProducts();
		Task RemoveProductFromCart(int productId, int productTypeId);
		Task UpdateQuantity(CartProductResponse product);
		Task StoreCartItems(bool emptyLocalCart);
		Task GetCartItemsCount();
	}
}
