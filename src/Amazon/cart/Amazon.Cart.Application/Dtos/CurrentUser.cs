namespace Amazon.Cart.Application.Dtos;

public abstract class CurrentUser
{
    public Guid Id { get; }
    public string Email { get; }

    protected CurrentUser(Guid id, string email)
    {
        Id = id;
        Email = email;
    }
}

public class AuthenticatedUser : CurrentUser
{
    public AuthenticatedUser(Guid id, string email) : base(id, email)
    {
    }
}

public class AnonymousUser() : CurrentUser(Guid.Empty, null) { }