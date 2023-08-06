using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CartItemController(ICartItemRepository cartItemRepository,
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

        [HttpGet]
        public IActionResult GetCartItems()
        {
            var cartItems = _cartItemRepository.GetCartItems();
            if (cartItems.Count <= 0)
                return NotFound();

            var cartItemMaps = _mapper.Map<List<CartItemDto>>(cartItems);
            return ModelState.IsValid ? Ok(cartItemMaps) : BadRequest(ModelState);
        }

        [HttpGet("Order/{orderId}")]
        public IActionResult GetCartItemByOrderId(int orderId)
        {
            if (!_orderRepository.OrderExists(orderId))
                return NotFound("Not found order");

            var cartItems = _cartItemRepository.GetCartItemByOrderId(orderId);
            if (cartItems.Count <= 0)
                return NotFound();

            return ModelState.IsValid ? Ok(cartItems) : BadRequest(ModelState);
        }

        [HttpGet("User/{userId}")]
        public IActionResult GetCartItemByUserId(int userId)
        {
            if (!_userRepository.UserExists(userId))
                return NotFound("Not found user");

            var cartItems = _cartItemRepository.GetCartItemByUserId(userId);
            if (cartItems.Count <= 0)
                return NotFound();

            return ModelState.IsValid ? Ok(cartItems) : BadRequest(ModelState);
        }

        [HttpPost]
        public IActionResult CreateCartItem([FromBody] CartItemDto cartItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userRepository.UserExists(cartItemDto.UserId))
                return NotFound("Not found user");

            if (!_productRepository.ProductExists(cartItemDto.ProductId))
                return NotFound("Not found product");

            var cartItemMap = _mapper.Map<CartItem>(cartItemDto);
            return _cartItemRepository.CreateCartItem(cartItemMap) ? Ok("Successfully created") : BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult UpdateCartItem([FromBody] CartItemDto cartItemInput)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cartItemUpdate = _cartItemRepository.GetCartItem(cartItemInput.Id);
            if (cartItemUpdate == null)
                return NotFound("Not found cartItem");

            var cartItemMap = _mapper.Map<CartItem>(cartItemInput);
            return _cartItemRepository.UpdateCartItem(cartItemMap, cartItemUpdate) ? Ok("Successfully updated") : BadRequest(ModelState);  
        }

        [HttpDelete]
        public IActionResult DeleteCartItem([FromQuery] int id)
        {
            var deleteCartItem = _cartItemRepository.GetCartItem(id);
            if (deleteCartItem == null)
                return NotFound();

            return _cartItemRepository.DeleteCartItem(deleteCartItem) ? Ok("Successfully deleted") : BadRequest(ModelState);
        }
    }
}
