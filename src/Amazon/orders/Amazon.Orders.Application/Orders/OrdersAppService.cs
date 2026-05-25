using Amazon.Orders.Application.Orders.Dtos;
using Amazon.Orders.Domain.Orders;
using Amazon.Orders.Domain.Orders.ValueObjects;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Extensions;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Moamen.SDKs.Repository.Pagination;

namespace Amazon.Orders.Application.Orders
{
    public class OrdersAppService(
        IEfCoreRepository<Order, Guid> _ordersRepo,
        OrdersService _ordersService,
        IUnitOfWork _unitOfWork)
    {
        public async Task<RestResponse<OrderDetailsDto>> GetByIdAsync(Guid id)
        {
            var order = await _ordersRepo.GetInstanceAsync(id, x => x.Include(d => d.Items));
            if (order == null)
                return RestResponse<OrderDetailsDto>.NotFound($"Order ({id}) was not found");

            return RestResponse<OrderDetailsDto>.Success(new OrderDetailsDto(id, order.Items.Select(i => new OrderItemDto(i.ProductInfo.Name, "", i.ProductInfo.UnitPrice, i.Quantity)).ToList()));
        }

        public async Task<PagedResult<OrderForListDto, DateTime>> GetCustomerOrdersPageAsync(Guid customerId, SearchOrdersRequest pageRequest)
        {
            var page = pageRequest.PageNumber == 1
            ? await _ordersRepo.GetPageAsync(new PagedRequest(pageRequest.PageNumber, pageRequest.PageSize), c => c.CreatedOn, [x => x.Customer.Id == customerId])
            : await _ordersRepo.GetPageAsync(pageRequest.PageSize, c => c.CreatedOn, DateTime.Parse(pageRequest.LastSeenValue), [x => x.Customer.Id == customerId]);

            var dtos = page.Items.Select(o => new OrderForListDto(o.Id, o.CreatedOn, "Pending"));

            return new PagedResult<OrderForListDto, DateTime>(dtos, page.TotalCount, page.LastSeenValue);
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