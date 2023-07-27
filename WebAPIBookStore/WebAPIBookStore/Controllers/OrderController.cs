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
        private readonly IMapper _mapper;

        public OrderController(IOrderRepository orderRepository,IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
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

        [HttpGet("{status}")]
        public IActionResult GetOrderByStatus(string status)
        {
            var orders = _mapper.Map<List<Order>>(_orderRepository.GetOrderByStatus(status));
            if (orders.Count() <= 0)
                return NotFound();

            return ModelState.IsValid ? Ok(orders) : BadRequest(ModelState);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOrder(int id)
        {
            if (!_orderRepository.OrderExists(id))
                return NotFound();

            var order = _mapper.Map<Order>(_orderRepository.GetOrder(id));
            return ModelState.IsValid ? Ok(order) : BadRequest(ModelState);
        }

        [HttpPost]
        public IActionResult CreateOrder(OrderCreate orderCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cartItemIds = orderCreate.cartItemIds;
            if (cartItemIds.Count <= 0)
                return BadRequest(ModelState);

            var orderMap = _mapper.Map<Order>(orderCreate.orderDto);
            if (!_orderRepository.CreateOrder(cartItemIds, orderMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut]
        public IActionResult UpdateOrder(int orderId, string status, int manageId)
        {
            if (!_orderRepository.OrderExists(orderId))
                return NotFound("Not found order");

            if (!_userRepository.ManageExists(manageId))
                return NotFound("Not found manage");

            return !_orderRepository.UpdateOrder(orderId, status, manageId) ? Ok("Successfully updated") : BadRequest();
        }
        
    }
}
