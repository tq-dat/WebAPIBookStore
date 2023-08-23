using WebAPIBookStore.Data;
using WebAPIBookStore.Enum;
using WebAPIBookStore.Models;


namespace WebBookStore
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }

        public void SeedDataContext()
        {
            if (!dataContext.CartItems.Any())
            {
                var cartItems = new List<CartItem>()
                {
                    new CartItem()
                    {
                        QuantityOfProduct = 2,
                        Status = CartItemStatus.UnPaid,
                        Order = new Order()
                        {
                            DateOrder = new DateTime(2023,7,24),
                            UserAdminId = null,
                            Status = OrderStatus.Wait

                        },
                        Product = new Product()
                        {
                            Name = "Bạch dạ hành",
                            Description = "Không có",
                            Author = "Higashino Keigo",
                            Price = 150000,
                            ProductCategories = new List<ProductCategory>()
                            {
                                new ProductCategory { Category = new Category() { Name = "Tiểu thuyết"}}
                            }
                        },
                        User = new User()
                        {
                            UserName = "dat123",
                            Password = "dat123",
                            Email = "dat123@gmail.com",
                            Address = "Cau Giay, Ha Noi",
                            Role = Role.User
                        }
                    }
                };
                dataContext.CartItems.AddRange(cartItems);
                dataContext.SaveChanges();
            }
        }
    }
}

