using System.Security.Claims;
using Duende.IdentityServer.Models;

namespace Amazon.Identity.Presentation
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource(ClaimTypes.Role,[ClaimTypes.Role]),
                new IdentityResources.Email(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("scope1"),
                new ApiScope("scope2"),
            };

        public static IEnumerable<Client> Clients =>
            [
                new Client
                {
                    AllowedGrantTypes = GrantTypes.Code,

                    ClientId = "amazon.angular",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    RequireClientSecret = false,
                    RequirePkce = true,

                    RedirectUris = { 
                        "http://localhost:4200/auth/login",
                        "http://localhost:62832/auth/login",
                        "http://localhost:4200/silent-refresh.html",
                        "http://localhost:4200/cart/checkout",
                    },
                    FrontChannelLogoutUri = "http://localhost:4200/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:4200","http://localhost:62832" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "email" },
                    AlwaysIncludeUserClaimsInIdToken = true
                },
            ];
    }
}
