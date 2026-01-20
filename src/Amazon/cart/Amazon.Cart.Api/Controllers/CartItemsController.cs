using Amazon.Cart.Application;
using Amazon.Cart.Application.Dtos;
using Amazon.SharedKernel.API;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.Cart.Api.Controllers;

[Route("api/carts/{cartId}/items")]
public class CartItemsController(CartService _cartService) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddItemCart(Guid cartId, [FromBody] CartItemCreateDto cartItem)
    {
        var result = await _cartService.AddItemToCartAsync(cartId, cartItem);
        if (result.IsSuccess)
            return Ok(result.Value);

        return RestResult(result);
    }
}