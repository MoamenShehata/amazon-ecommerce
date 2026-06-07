using Amazon.Cart.Application;
using Amazon.Cart.Application.Dtos;
using Amazon.SharedKernel.API;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.Cart.Api.Controllers;

[Route("api/carts/{cartId}/items")]
public class CartItemsController(CartAppService _cartService) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddItemToCart(Guid cartId, [FromBody] CartItemCreateDto cartItem)
    {
        var result = await _cartService.AddItemToCartAsync(cartId, cartItem);
        if (result.IsSuccess)
            return Ok(result.Value);

        return RestResult(result);
    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> RemoveItemFromCart(Guid cartId, Guid productId)
    {
        var result = await _cartService.RemoveItemFromCartAsync(cartId, productId);
        return RestResult(result);
    }

    [HttpDelete("RemoveAllProductItems/{productId}")]
    public async Task<IActionResult> RemoveAllProductItems(Guid cartId, Guid productId)
    {
        var result = await _cartService.RemoveAllProductItemsAsync(cartId, productId);
        return RestResult(result);
    }
}