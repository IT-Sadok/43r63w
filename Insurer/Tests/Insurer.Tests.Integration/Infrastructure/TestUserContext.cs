using Shared.ContextAccessor;

namespace Insurer.Tests.Integration.Infrastructure;

public class TestUserContext : IUserContextAccessor
{
    public UserContextModel GetUserContext()
    {
        return new UserContextModel
        {
            UserId = "1",
            UserName = "TestUser",
            Roles = ["Agent"]
        };
    }
}