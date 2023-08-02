using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IMapper _mapper;

        public OrderController(IOrderRepository orderRepository,
                               IUserRepository userRepository,
                               ICartItemRepository cartItemRepository,
                               IMapper mapper)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _cartItemRepository = cartItemRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            var orders = _orderRepository.GetOrders();
            if (orders.Count <= 0)
                return NotFound();
            
            var orderMaps = _mapper.Map<List<Order>>(orders);
            return ModelState.IsValid? Ok(orderMaps) : BadRequest(ModelState);
        }

        [HttpGet("Status")]
        public IActionResult GetOrderByStatus([FromQuery] string name)
        {
            var orders = _orderRepository.GetOrderByStatus(name);
            if (orders.Count <= 0)
                return NotFound();

            var orderMaps = _mapper.Map<List<Order>>(orders);
            return ModelState.IsValid ? Ok(orderMaps) : BadRequest(ModelState);
        }

        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            var order = _orderRepository.GetOrder(id);
            if (order == null)
                return NotFound();

            return ModelState.IsValid ? Ok(order) : BadRequest(ModelState);
        }

        [HttpGet("User/{userId}")]
        public IActionResult GetOrderByUserId(int userId)
        {
            var orders = _orderRepository.GetOrderByUserId(userId);
            if (orders.Count <= 0)
                return NotFound();

            return Ok(orders);
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] OrderCreate orderCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (orderCreate.CartItemIds.Count <= 0)
                return BadRequest(ModelState);

            if (!_userRepository.UserExists(orderCreate.userId))
                return NotFound("Not found user");

            List<CartItem> cartItems = new List<CartItem>();
            foreach (int id in orderCreate.CartItemIds)
            {
                var cartItem = _cartItemRepository.GetCartItem(id);
                if (cartItem == null)
                    return NotFound();

                if (cartItem.Status == "Paid")
                    return BadRequest();

                cartItems.Add(cartItem);
            }

            var orderMap = _mapper.Map<Order>(orderCreate.OrderDto);
            if (!_orderRepository.CreateOrder(cartItems, orderMap, orderCreate.userId))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut]
        public IActionResult UpdateOrder([FromBody] OrderUpdate input)
        {
            var orderUpdate = _orderRepository.GetOrder(input.orderId);
            if (orderUpdate == null)
                return NotFound("Not found order");

            if (!_userRepository.ManageExists(input.manageId))
                return NotFound("Not found manage");

            if (input.status != "Cancel" || input.status != "Success")
                return BadRequest("Status not true");

            return !_orderRepository.UpdateOrder(orderUpdate, input.status, input.manageId) ? Ok("Successfully updated") : BadRequest();
        }
   
    }
}
