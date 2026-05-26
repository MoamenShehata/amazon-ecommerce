using Amazon.Cart.Application.Dtos;

namespace Amazon.Cart.Application.Services;

public interface IAuthenticationService
{
    CurrentUser CurrentUser { get; }
}