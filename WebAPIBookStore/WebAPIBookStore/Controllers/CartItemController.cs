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

        [HttpGet("Order/orderId")]
        public IActionResult GetCartItemByOrderId([FromQuery] int orderId)
        {
            if (!_orderRepository.OrderExists(orderId))
                return NotFound("Not found order");

            var cartItems = _cartItemRepository.GetCartItemByOrderId(orderId);
            return ModelState.IsValid ? Ok(cartItems) : BadRequest(ModelState);
        }

        [HttpGet("User/userId")]
        public IActionResult GetCartItemByUserId([FromQuery] int userId)
        {
            if (!_userRepository.UserExists(userId))
                return NotFound("Not found user");

            var cartItems = _cartItemRepository.GetCartItemByUserId(userId);
            return ModelState.IsValid ? Ok(cartItems) : BadRequest(ModelState);
        }

        [HttpPost]
        public IActionResult CreateCartItem([FromQuery] int productId, [FromQuery] int userId, [FromBody] CartItemCreate cartItemCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userRepository.UserExists(userId))
                return NotFound("Not found user");

            if (!_productRepository.ProductExists(productId))
                return NotFound("Not found product");

            var cartItemMap = _mapper.Map<CartItem>(cartItemCreate);
            if (!_cartItemRepository.CreateCartItem(productId, userId, cartItemMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut]
        public IActionResult UpdateCartItem([FromBody] CartItemCreate cartItemInput, [FromQuery] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cartItemUpdate = _cartItemRepository.GetCartItem(id);
            if (cartItemUpdate == null)
                return NotFound("Not found cartItem");

            var cartItemMap = _mapper.Map<CartItem>(cartItemInput);
            if (!_cartItemRepository.UpdateCartItem(cartItemMap, cartItemUpdate))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }

        [HttpDelete("id")]
        public IActionResult DeleteCartItem([FromQuery] int id)
        {
            var deleteCartItem = _cartItemRepository.GetCartItem(id);
            if (deleteCartItem == null)
                return NotFound();

            if (!_cartItemRepository.DeleteCartItem(deleteCartItem))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }
    }
}
