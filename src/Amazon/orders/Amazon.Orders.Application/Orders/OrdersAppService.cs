using Amazon.Orders.Application.Orders.Dtos;
using Amazon.Orders.Application.Orders.Mappers;
using Amazon.Orders.Domain.Orders;
using Amazon.Orders.Domain.Orders.ValueObjects;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Extensions;
using Moamen.SDKs.Repository;
using Moamen.SDKs.Repository.Pagination;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Orders.Application.Orders
{
    public class OrdersAppService(
        IEfCoreRepository<Order, Guid> _ordersRepo,
        OrdersService _ordersService,
        IUnitOfWork _unitOfWork)
    {
        public async Task<RestResponse<OrderDetailsDto>> GetByUserAsync(Guid requesterUserId, Guid orderId)
        {
            var orderResult = await _ordersService.GetByUserAsync(requesterUserId, orderId);
            if (!orderResult.IsSuccess)
                return orderResult.MapTo(null as OrderDetailsDto);

            return RestResponse<OrderDetailsDto>.Success(orderResult.Value.ToDetailsDto());
        }

        public async Task<PagedResult<OrderForListDto, DateTime>> GetCustomerOrdersPageAsync(Guid customerId, SearchOrdersRequest pageRequest)
        {
            var page = pageRequest.PageNumber == 1
            ? await _ordersRepo.GetPageAsync(new PagedRequest(pageRequest.PageNumber, pageRequest.PageSize), c => c.CreatedOn, [x => x.Owner.Id == customerId])
            : await _ordersRepo.GetPageAsync(pageRequest.PageSize, c => c.CreatedOn, DateTime.Parse(pageRequest.LastSeenValue), [x => x.Owner.Id == customerId]);

            return new PagedResult<OrderForListDto, DateTime>(page.Items.Select(o => o.ToForListDto()), page.TotalCount, page.LastSeenValue);
        }

        public async Task<RestResponse<OrderCreatedDto>> PlaceAsync(Guid customerId, string customerEmail, OrderCreateDto request)
        {
            var result = await _ordersService.PlaceOrderAsync(new CustomerInfo(customerId, customerEmail), request.ShoppingCart, request.PaymentInfo, request.DeliveryAddress);
            if (result.IsSuccess)
            {
                await _unitOfWork.CommitAsync();
                return result.MapTo(new OrderCreatedDto(result.Value.Id));
            }

            return result.MapTo((OrderCreatedDto)null);
        }

        public async Task<RestResponse<bool>> CancelAsync(Guid requesterUserId, Guid orderId)
        {
            var result = await _ordersService.CancelAsync(requesterUserId, orderId);
            if (!result.IsSuccess)
                return result;

            await _unitOfWork.CommitAsync();
            return RestResponse<bool>.Success(true);
        }

        public async Task<RestResponse<bool>> StartProcessingAsync(Guid requesterUserId, Guid orderId)
        {
            // validate user permissions

            var result = await _ordersService.StartProcessingAsync(requesterUserId, orderId);
            if (!result.IsSuccess)
                return result;

            await _unitOfWork.CommitAsync();
            return RestResponse<bool>.Success(true);
        }

        public async Task<RestResponse<bool>> StartShippingAsync(Guid requesterUserId, Guid orderId)
        {
            // validate user permissions

            var result = await _ordersService.StartShippingAsync(requesterUserId, orderId);
            if (!result.IsSuccess)
                return result;

            await _unitOfWork.CommitAsync();
            return RestResponse<bool>.Success(true);
        }

        public async Task<RestResponse<bool>> ShippingCompletedAsync(Guid requesterUserId, Guid orderId)
        {
            // validate user permissions

            var result = await _ordersService.ShippingCompletedAsync(requesterUserId, orderId);
            if (!result.IsSuccess)
                return result;

            await _unitOfWork.CommitAsync();
            return RestResponse<bool>.Success(true);
        }

        public async Task<RestResponse<bool>> DeliveryAcceptedAsync(Guid requesterUserId, Guid orderId)
        {
            // validate user permissions

            var result = await _ordersService.DeliveryAcceptedAsync(requesterUserId, orderId);
            if (!result.IsSuccess)
                return result;

            await _unitOfWork.CommitAsync();
            return RestResponse<bool>.Success(true);
        }

        public async Task<RestResponse<bool>> CompletedAsync(Guid requesterUserId, Guid orderId)
        {
            // validate user permissions

            var result = await _ordersService.CompletedAsync(requesterUserId, orderId);
            if (!result.IsSuccess)
                return result;

            await _unitOfWork.CommitAsync();
            return RestResponse<bool>.Success(true);
        }
    }
}