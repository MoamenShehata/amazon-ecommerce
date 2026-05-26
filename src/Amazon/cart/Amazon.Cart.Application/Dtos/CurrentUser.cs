namespace Amazon.Cart.Application.Dtos;

public abstract class CurrentUser
{
    public Guid Id { get; }
    public string Email { get; }
    public bool IsAuthenticated { get; }

    protected CurrentUser(Guid id, string email, bool isAuthenticated)
    {
        Id = id;
        Email = email;
        IsAuthenticated = isAuthenticated;
    }
}

public class AuthenticatedUser : CurrentUser
{
    public AuthenticatedUser(Guid id, string email) : base(id, email, true)
    {
    }
}

public class AnonymousUser() : CurrentUser(Guid.Empty, null, false) { }