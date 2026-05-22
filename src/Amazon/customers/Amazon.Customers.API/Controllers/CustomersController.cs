using Amazon.Customers.Application;
using Amazon.SharedKernel.API;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.Customers.API.Controllers
{
    [Route("api/[controller]/{id}")]
    public class CustomersController(CustomerAppService _customerAppService) : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCustomerProfile(Guid id)
        {
            var result = await _customerAppService.GetCustomerProfileAsync(id);
            if (result.IsSuccess)
                return Ok(result.Value);

            return RestResult(result);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateShoppingCart([FromBody] CartCreateDto cartModel)
        //{
        //    var result = await _cartService.CreateCartAsync(cartModel);
        //    return Ok(result);
        //}
    }
}
