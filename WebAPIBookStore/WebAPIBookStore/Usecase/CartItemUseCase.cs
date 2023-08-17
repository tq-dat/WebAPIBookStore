using AutoMapper;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;
using WebAPIBookStore.Usecase;

namespace WebAPIBookStore.UseCase
{
    public class CartItemUseCase
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUseCaseOutput _useCaseOutput;

        public CartItemUseCase(
            ICartItemRepository cartItemRepository,
            IUserRepository userRepository,
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IMapper mapper,
            IUseCaseOutput useCaseOutput)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _cartItemRepository = cartItemRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _useCaseOutput = useCaseOutput;
        }

        public Output Get()
        {
            var cartItems = _cartItemRepository.GetCartItems();
            if (!cartItems.Any())
            {
                return _useCaseOutput.NotFound("Not found cartitem");
            }

            return _useCaseOutput.Success(_mapper.Map<List<CartItemDto>>(cartItems));
        }

        public Output GetByOrderId(int orderId)
        {
            var output = new Output();
            if (!_orderRepository.OrderExists(orderId))
            {
                return _useCaseOutput.NotFound("Not found orderId");
            }

            var cartItems = _cartItemRepository.GetCartItemByOrderId(orderId);
            if (!cartItems.Any())
            {
                return _useCaseOutput.NotFound("Not found any cartitem in order");
            }

            return _useCaseOutput.Success(cartItems);
        }

        public Output GetByUserId(int userId)
        {
            if (!_userRepository.UserExists(userId))
            {
                return _useCaseOutput.NotFound("Not found order");
            }

            var cartItems = _cartItemRepository.GetCartItemByUserId(userId);
            if (!cartItems.Any())
            {
                return _useCaseOutput.NotFound("Not found any cartitem for user");
            }

            return _useCaseOutput.Success(cartItems);
        }

        public Output Post(CartItemDto cartItemDto)
        {
            if (!_userRepository.UserExists(cartItemDto.UserId))
            {
                return _useCaseOutput.NotFound("Not found userId");
            }

            if (!_productRepository.ProductExists(cartItemDto.ProductId))
            {
                return _useCaseOutput.NotFound("Not found productId");
            }

            var cartItemMap = _mapper.Map<CartItem>(cartItemDto);
            if (!_cartItemRepository.CreateCartItem(cartItemMap))
            {
                return _useCaseOutput.InternalServer("Something went wrong while saving");
            }

            return _useCaseOutput.Success(_mapper.Map<CartItemDto>(cartItemMap));
        }

        public Output Put(CartItemDto cartItemInput)
        {
            var cartItemUpdate = _cartItemRepository.GetCartItem(cartItemInput.Id);
            if (cartItemUpdate == null)
            {
                return _useCaseOutput.NotFound("Not found cartItem");
            }

            var cartItemMap = _mapper.Map<CartItem>(cartItemInput);
            if (!_cartItemRepository.UpdateCartItem(cartItemMap, cartItemUpdate))
            {
                return _useCaseOutput.InternalServer("Something went wrong while saving");
            }

            return _useCaseOutput.Success(_mapper.Map<CartItemDto>(cartItemUpdate));
        }

        public Output Delete(int id)
        {
            var deleteCartItem = _cartItemRepository.GetCartItem(id);
            if (deleteCartItem == null)
            {
                return _useCaseOutput.NotFound("Not found cartItem");
            }

            if (!_cartItemRepository.DeleteCartItem(deleteCartItem))
            {
                return _useCaseOutput.InternalServer("Something went wrong while saving");
            }

            return _useCaseOutput.Success(_mapper.Map<CartItemDto>(deleteCartItem));
        }
    }
}
