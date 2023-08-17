using Microsoft.AspNetCore.Mvc;
using WebAPIBookStore.Consts;
using WebAPIBookStore.Dto;
using WebAPIBookStore.UseCase;

namespace WebAPIBookStore.Controllers
{
    [Route("api/CartItem")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly CartItemUseCase _cartItemUseCase;
        public CartItemController(CartItemUseCase cartItemUseCase)
        {
            _cartItemUseCase = cartItemUseCase;
        }

        [HttpGet]
        public IActionResult GetCartItems()
        {
            var output = _cartItemUseCase.Get();
            return output.Error != StatusCodeAPI.NotFound ? Ok(output) : NotFound(output);
        }

        [HttpGet("Order")]
        public IActionResult GetCartItemByOrderId([FromQuery] int orderId)
        {
            var output = _cartItemUseCase.GetByOrderId(orderId);
            return output.Error != StatusCodeAPI.NotFound ? Ok(output) : NotFound(output);
        }

        [HttpGet("User")]
        public IActionResult GetCartItemByUserId([FromQuery] int userId)
        {
            var output = _cartItemUseCase.GetByUserId(userId);
            return output.Error != StatusCodeAPI.NotFound ? Ok(output) : NotFound(output);
        }

        [HttpPost]
        public IActionResult CreateCartItem([FromBody] CartItemDto cartItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var output = _cartItemUseCase.Post(cartItemDto);
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
        public IActionResult UpdateCartItem([FromBody] CartItemDto cartItemInput)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var output = _cartItemUseCase.Put(cartItemInput);
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

        [HttpDelete("{id:int}")]
        public IActionResult DeleteCartItem([FromRoute] int id)
        {
            var output = _cartItemUseCase.Delete(id);
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
