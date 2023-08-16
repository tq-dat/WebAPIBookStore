using AutoMapper;
using WebAPIBookStore.Consts;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.UseCase
{
    public class CartItemUseCase
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CartItemUseCase(
            ICartItemRepository cartItemRepository,
            IUserRepository userRepository,
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _cartItemRepository = cartItemRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public Output Get()
        {
            var output = new Output();
            var cartItems = _cartItemRepository.GetCartItems();
            if (!cartItems.Any())
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found cartitem";
                return output;
            }

            output.Success = true;
            output.Data = _mapper.Map<List<CartItemDto>>(cartItems);
            return output;
        }

        public Output GetByOrderId(int orderId)
        {
            var output = new Output();
            if (!_orderRepository.OrderExists(orderId))
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found order";
                return output;
            }

            var cartItems = _cartItemRepository.GetCartItemByOrderId(orderId);
            if (!cartItems.Any())
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found any cartitem in order";
                return output;
            }

            output.Success = true;
            output.Data = cartItems;
            return output;
        }

        public Output GetByUserId(int userId)
        {
            var output = new Output();
            if (!_userRepository.UserExists(userId))
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found order";
                return output;
            }

            var cartItems = _cartItemRepository.GetCartItemByUserId(userId);
            if (!cartItems.Any())
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found any cartitem for user";
                return output;
            }

            output.Success = true;
            output.Data = cartItems;
            return output;
        }

        public Output Post(CartItemDto cartItemDto)
        {
            var output = new Output();
            if (!_userRepository.UserExists(cartItemDto.UserId))
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found userId";
                return output;
            }

            if (!_productRepository.ProductExists(cartItemDto.ProductId))
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found productId";
                return output;
            }

            var cartItemMap = _mapper.Map<CartItem>(cartItemDto);
            if (!_cartItemRepository.CreateCartItem(cartItemMap))
            {
                output.Success = false;
                output.Error = StatusCodeAPI.InternalServer;
                output.Message = "Something went wrong while saving";
                return output;
            }

            output.Success = true;
            output.Data = _mapper.Map<CartItemDto>(cartItemMap);
            return output;
        }

        public Output Put(CartItemDto cartItemInput)
        {
            var output = new Output();
            var cartItemUpdate = _cartItemRepository.GetCartItem(cartItemInput.Id);
            if (cartItemUpdate == null)
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found cartItem";
                return output;
            }

            var cartItemMap = _mapper.Map<CartItem>(cartItemInput);
            if (!_cartItemRepository.UpdateCartItem(cartItemMap, cartItemUpdate))
            {
                output.Success = false;
                output.Error = StatusCodeAPI.InternalServer;
                output.Message = "Something went wrong while saving";
                return output;
            }
            output.Success = true;
            output.Data = _mapper.Map<CartItemDto>(cartItemUpdate);
            return output;
        }

        public Output Delete(int id)
        {
            var output = new Output();
            var deleteCartItem = _cartItemRepository.GetCartItem(id);
            if (deleteCartItem == null)
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found cartItem";
                return output;
            }

            if (!_cartItemRepository.DeleteCartItem(deleteCartItem))
            {
                output.Success = false;
                output.Error = StatusCodeAPI.InternalServer;
                output.Message = "Something went wrong while saving";
                return output;
            }

            output.Success = true;
            output.Data = _mapper.Map<CartItemDto>(deleteCartItem);
            return output;
        }
    }
}
