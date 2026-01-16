using Amazon.Orders.Application.Orders.Dtos;
using Amazon.Orders.Domain.Orders;
using Amazon.Orders.Domain.Orders.ValueObjects;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Extensions;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Amazon.Orders.Application.Orders
{
    public class OrdersAppService(
        IEfCoreRepository<Order, Guid> _ordersRepo,
        OrdersService _ordersService,
        IUnitOfWork _unitOfWork)
    {
        public async Task<RestResponse<OrderDto>> GetByIdAsync(Guid id)
        {
            var order = await _ordersRepo.GetInstanceAsync(id, x => x.Include(d => d.Items));
            if (order == null)
                return RestResponse<OrderDto>.NotFound($"Order ({id}) was not found");

            return RestResponse<OrderDto>.Success(new OrderDto(id, order.Items.Select(x => new KeyValuePair<string, int>(x.ProductInfo.Name, x.Quantity)).ToList()));
        }

        public async Task<RestResponse<OrderCreatedDto>> PlaceAsync(OrderCreateDto request)
        {
            var result = await _ordersService.PlaceOrderAsync(new CustomerInfo(request.UserId, request.Email), request.ShoppingCart);
            if (result.IsSuccess)
            {
                await _unitOfWork.CommitAsync();
                return result.MapTo(new OrderCreatedDto(result.Value.Id));
            }

            return result.MapTo((OrderCreatedDto)null);
        }
    }
}