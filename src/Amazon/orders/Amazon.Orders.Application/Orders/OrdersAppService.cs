using Amazon.Orders.Application.Orders.Dtos;
using Amazon.Orders.Application.Orders.Mappers;
using Amazon.Orders.Domain.Orders;
using Amazon.Orders.Domain.Orders.ValueObjects;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Customers;
using Amazon.SharedKernel.Extensions;
using Microsoft.Extensions.Logging;
using Moamen.SDKs.Repository;
using Moamen.SDKs.Repository.Pagination;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Orders.Application.Orders
{
    public class OrdersAppService(
        IEfCoreRepository<Order, Guid> _ordersRepo,
        OrdersService _ordersService,
        IUnitOfWork _unitOfWork,
        ILogger<OrdersAppService> _logger)
    {
        public async Task<RestResponse<OrderDetailsDto>> GetByUserAsync(Guid requesterUserId, Guid orderId)
        {
            var orderResult = await _ordersService.GetByUserForReadAsync(requesterUserId, orderId);
            if (!orderResult.IsSuccess)
                return orderResult.MapTo(null as OrderDetailsDto);

            return RestResponse<OrderDetailsDto>.Success(orderResult.Value.ToDetailsDto());
        }

        public async Task<RestResponse<PagedResult<OrderForListDto, DateTime>>> GetOrdersPageAsync(Guid customerId, SearchOrdersRequest pageRequest)
        {
            var pageResult = await _ordersService.GetOrdersPageByRequestingCustomer(customerId, pageRequest.PageNumber, pageRequest.PageSize, !string.IsNullOrWhiteSpace(pageRequest.LastSeenValue) ? DateTime.Parse(pageRequest.LastSeenValue) : null);
            if (!pageResult.IsSuccess)
                return pageResult.MapTo(null as PagedResult<OrderForListDto, DateTime>);

            return pageResult.MapTo(new PagedResult<OrderForListDto, DateTime>(pageResult.Value.Items.Select(o => o.ToForListDto()), pageResult.Value.TotalCount, pageResult.Value.LastSeenValue));
        }

        public async Task<RestResponse<OrderCreatedDto>> PlaceAsync(Guid customerId, string customerEmail, OrderCreateDto request)
        {
            var result = await _ordersService.PlaceOrderAsync(request.OrderId, new CustomerInfo(customerId, customerEmail, "+000000"), request.ShoppingCart, request.DeliveryAddress);
            if (result.IsSuccess)
            {
                await _unitOfWork.CommitAsync();
                return result.MapTo(new OrderCreatedDto(result.Value.Id));
            }

            return result.MapTo((OrderCreatedDto)null);
        }

        public async Task<RestResponse<bool>> UpdateStatusAsync(Guid requesterUserId, Guid orderId, UpdateOrderStatusRequest request)
        {
            var result = await _ordersService.UpdateStatusAsync(requesterUserId, orderId, request);
            if (!result.IsSuccess)
                return result;

            await _unitOfWork.CommitAsync();
            return RestResponse<bool>.Success(true);
        }

        public async Task<RestResponse<bool>> CancelAsync(Guid requesterUserId, Guid orderId)
        {
            var result = await _ordersService.CancelAsync(requesterUserId, orderId);
            if (!result.IsSuccess)
                return result;

            await _unitOfWork.CommitAsync();
            return RestResponse<bool>.Success(true);
        }
    }
}