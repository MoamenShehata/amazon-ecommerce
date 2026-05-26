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

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("amazon-product-catalog","Amazon Store Catalog APIs")
                {
                    Scopes = { "amazon.catalog" },
                },
                
                new ApiResource("amazon-cart","Amazon Shopping cart")
                {
                    Scopes = { "amazon.cart" },
                },
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            [
                new ApiScope("amazon.catalog","Amazon Store Catalog APIs"),
                new ApiScope("amazon.cart","Amazon Store Catalog APIs"),
            ];

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
                    AllowedScopes = { "openid", "profile", "email", "amazon.catalog","amazon.cart" },
                    AlwaysIncludeUserClaimsInIdToken = true
                },

                new Client
                {
                    AllowedGrantTypes = GrantTypes.Code,

                    ClientId = "amazon.angular.internal",
                    ClientSecrets = { new Secret("48C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    RequireClientSecret = false,
                    RequirePkce = true,

                    RedirectUris = {
                        "http://localhost:59017/auth/login",
                        "http://localhost:59017/silent-refresh.html",
                        "http://localhost:59017/cart/checkout",
                    },
                    FrontChannelLogoutUri = "http://localhost:59017/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:59017", "http://localhost:59017" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "email", "amazon.catalog.internal" },
                    AlwaysIncludeUserClaimsInIdToken = true
                },
            ];
    }
}
