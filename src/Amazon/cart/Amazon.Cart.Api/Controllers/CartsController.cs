//using Amazon.Cart.Application;
//using Amazon.Cart.Application.Dtos;
//using Amazon.SharedKernel.API;
//using Microsoft.AspNetCore.Mvc;

//namespace Amazon.Cart.Api.Controllers;

//public class CartsController(CartAppService _cartService) : ApiControllerBase
//{
//    [HttpPost]
//    public async Task<IActionResult> Create([FromBody] CartCreateDto cartModel)
//    {
//        var result = await _cartService.CreateCartAsync(cartModel);
//        if (result.IsSuccess)
//            return CreatedAtRoute(nameof(GetById), new { cartId = result.Value.CartId }, result.Value);

//        return RestResult(result);
//    }

//    [HttpGet("{cartId}", Name = nameof(GetById))]
//    public async Task<IActionResult> GetById(Guid cartId) => RestResult(await _cartService.GetByIdAsync(cartId));
//}
