using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Macs;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;
using WebAPIBookStore.Repository;

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

        public CartItemController(ICartItemRepository cartItemRepository, IUserRepository userRepository,IOrderRepository orderRepository,IProductRepository productRepository,IMapper mapper) 
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
            var cartItems = _mapper.Map<List<CartItem>>(_cartItemRepository.GetCartItems());
            if (cartItems.Count() <=0)
                return NotFound();

            return ModelState.IsValid ? Ok(cartItems) : BadRequest(ModelState);
        }

        [HttpGet("{orderId}")]
        public IActionResult GetCartItemByOrderId(int orderId)
        {
            if (!_orderRepository.OrderExists(orderId))
                return NotFound("Not found order");

            var cartItems = _cartItemRepository.GetCartItemByOrderId(orderId);
            if (cartItems.Count() <= 0)
                return BadRequest();

            return ModelState.IsValid ? Ok(cartItems) : BadRequest(ModelState);
        }

        [HttpGet("cartItem/{userId}")]
        public IActionResult GetCartItemByUserId(int userId)
        {
            if (!_userRepository.UserExists(userId))
                return NotFound("Not found user");

            var cartItems = _mapper.Map<List<CartItemDto>>(_userRepository.GetCartItemByUserId(userId));
            if (cartItems.Count() <= 0)
                return NotFound();

            return ModelState.IsValid ? Ok(cartItems) : BadRequest(ModelState);
        }

        [HttpPost]
        public IActionResult CreateCartItem(int productId, int userId, CartItemCreate cartItemCreate)
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
        public IActionResult UpdateCartItem(CartItemCreate cartItemUpdate, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_cartItemRepository.CartItemExists(id))
                return NotFound("Not found cartitem");

            var cartItemMap = _mapper.Map<CartItem>(cartItemUpdate);
            if (!_cartItemRepository.UpdateCartItem(cartItemMap, id))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCartItem(int id)
        {
            if (!_cartItemRepository.CartItemExists(id))
                return NotFound();

            if (!_cartItemRepository.DeleteCartItem(id))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }
    }
}
