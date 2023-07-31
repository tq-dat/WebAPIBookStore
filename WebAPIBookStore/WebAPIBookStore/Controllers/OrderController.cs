using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;
using WebAPIBookStore.Repository;
using static NuGet.Packaging.PackagingConstants;

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
            var orders = _mapper.Map<List<Order>>(_orderRepository.GetOrders());
            if (orders.Count() <= 0)
                return NotFound();

            return ModelState.IsValid? Ok(orders) : BadRequest(ModelState);
        }

        [HttpGet("status")]
        public IActionResult GetOrderByStatus([FromQuery] string status)
        {
            var orders = _mapper.Map<List<Order>>(_orderRepository.GetOrderByStatus(status));
            if (orders.Count() <= 0)
                return NotFound();

            return ModelState.IsValid ? Ok(orders) : BadRequest(ModelState);
        }

        [HttpGet("id")]
        public IActionResult GetOrder([FromQuery] int id)
        {
            var order = _orderRepository.GetOrder(id);
            if (order == null)
                return NotFound();

            return ModelState.IsValid ? Ok(order) : BadRequest(ModelState);
        }

        [HttpGet("user/userId")]
        public IActionResult GetOrderByUserId([FromQuery] int userId)
        {
            var orders = _orderRepository.GetOrderByUserId(userId);
            if (orders.Count() <= 0)
                return NotFound();

            return Ok(orders);
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] OrderCreate orderCreate, [FromQuery] int userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (orderCreate.cartItemIds.Count <= 0)
                return BadRequest(ModelState);

            if (!_userRepository.UserExists(userId))
                return NotFound("Not found user");

            List<CartItem> cartItems = new List<CartItem>();
            foreach (int id in orderCreate.cartItemIds)
            {
                var cartItem = _cartItemRepository.GetCartItem(id);
                if (cartItem == null)
                    return NotFound();

                if (cartItem.Status == "Paid")
                    return BadRequest();

                cartItems.Add(cartItem);
            }

            var orderMap = _mapper.Map<Order>(orderCreate.orderDto);
            if (!_orderRepository.CreateOrder(cartItems, orderMap, userId))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut]
        public IActionResult UpdateOrder([FromQuery] int orderId, [FromQuery] string status, [FromQuery] int manageId)
        {
            var orderUpdate = _orderRepository.GetOrder(orderId);
            if (orderUpdate == null)
                return NotFound("Not found order");

            if (!_userRepository.ManageExists(manageId))
                return NotFound("Not found manage");

            if (status != "Cancel" || status != "Success")
                return BadRequest("Status not true");

            return !_orderRepository.UpdateOrder(orderUpdate, status, manageId) ? Ok("Successfully updated") : BadRequest();
        }
   
    }
}
