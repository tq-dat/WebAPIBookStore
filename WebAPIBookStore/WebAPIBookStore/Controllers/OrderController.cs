using Microsoft.AspNetCore.Mvc;
using WebAPIBookStore.Consts;
using WebAPIBookStore.Dto;
using WebAPIBookStore.UseCase;

namespace WebAPIBookStore.Controllers
{
    [Route("api/Order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderUseCase _orderUseCase;
        public OrderController(OrderUseCase orderUseCase)
        {
            _orderUseCase = orderUseCase;
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            var output = _orderUseCase.Get();
            return output.Error != StatusCodeAPI.NotFound ? Ok(output) : NotFound(output);
        }

        [HttpGet("Status")]
        public IActionResult GetOrderByStatus([FromQuery] string name)
        {
            var output = _orderUseCase.GetByStatus(name);
            return output.Error != StatusCodeAPI.NotFound ? Ok(output) : NotFound(output);
        }

        [HttpGet("{id}")]
        public IActionResult GetOrder([FromRoute] int id)
        {
            var output = _orderUseCase.GetById(id);
            return output.Error != StatusCodeAPI.NotFound ? Ok(output) : NotFound(output);
        }

        [HttpGet("User")]
        public IActionResult GetOrderByUserId([FromQuery] int userId)
        {
            var output = _orderUseCase.GetByUserId(userId);
            return output.Error != StatusCodeAPI.NotFound ? Ok(output) : NotFound(output);
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] OrderCreate orderCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var output = _orderUseCase.Post(orderCreate);
            if (!output.Success)
            {
                switch (output.Error)
                {
                    case StatusCodeAPI.NotFound:
                        return NotFound(output);

                    case StatusCodeAPI.InternalServer:
                        return BadRequest(output);
                }
            }

            return Ok(output);
        }

        [HttpPut]
        public IActionResult UpdateOrder([FromBody] OrderUpdate input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var output = _orderUseCase.Put(input);
            if (!output.Success)
            {
                switch (output.Error)
                {
                    case StatusCodeAPI.NotFound:
                        return NotFound(output);

                    case StatusCodeAPI.InternalServer:
                        return BadRequest(output);
                }
            }

            return Ok(output);
        }

    }
}
