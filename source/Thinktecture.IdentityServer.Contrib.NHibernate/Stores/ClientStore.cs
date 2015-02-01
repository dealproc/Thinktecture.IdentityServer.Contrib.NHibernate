using NHibernate;
using NHibernate.Linq;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Services;
namespace Thinktecture.IdentityServer.NH.Stores {
    public class ClientStore : IClientStore {
        ISessionFactory _SessionFactory;
        public ClientStore(ISessionFactory sessionFactory) {
            _SessionFactory = sessionFactory;
        }
        public Task<Thinktecture.IdentityServer.Core.Models.Client> FindClientByIdAsync(string clientId) {
            return Task.Factory.StartNew<Thinktecture.IdentityServer.Core.Models.Client>(() => {
                using (var session = _SessionFactory.OpenSession()) {
                    var client = session.Query<Entities.Client>()
                        .Fetch(x => x.ClientSecrets)
                        .Fetch(x => x.RedirectUris)
                        .Fetch(x => x.PostLogoutRedirectUris)
                        .Fetch(x => x.ScopeRestrictions)
                        .Fetch(x => x.IdentityProviderRestrictions)
                        .Fetch(x => x.Claims)
                        .Fetch(X => X.CustomGrantTypeRestrictions)
                        .SingleOrDefault(x => x.ClientId == clientId);

                    var model = new Thinktecture.IdentityServer.Core.Models.Client {
                        AbsoluteRefreshTokenLifetime = client.AbsoluteRefreshTokenLifetime,
                        AccessTokenLifetime = client.AccessTokenLifetime,
                        AccessTokenType = client.AccessTokenType,
                        ClientId = client.ClientId,
                        AllowRememberConsent = client.AllowRememberConsent,
                        AlwaysSendClientClaims = client.AlwaysSendClientClaims,
                        AuthorizationCodeLifetime = client.AuthorizationCodeLifetime,
                        Claims = client.Claims.Select(x => new System.Security.Claims.Claim(x.Type, x.Value)).ToList(),
                        ClientName = client.ClientName,
                        ClientSecrets = client.ClientSecrets.Select(x => new Thinktecture.IdentityServer.Core.Models.ClientSecret { ClientSecretType = x.ClientSecretType, Description = x.Description, Expiration = x.Expiration, Value = x.Value }).ToList(),
                        ClientUri = client.ClientUri,
                        CustomGrantTypeRestrictions = client.CustomGrantTypeRestrictions.Select(x => x.GrantType).ToList(),
                        Enabled = client.Enabled,
                        EnableLocalLogin = client.EnableLocalLogin,
                        Flow = client.Flow,
                        IdentityProviderRestrictions = client.IdentityProviderRestrictions.Select(x => x.Provider).ToList(),
                        IdentityTokenLifetime = client.IdentityTokenLifetime,
                        IncludeJwtId = client.IncludeJwtId,
                        LogoUri = client.LogoUri,
                        PostLogoutRedirectUris = client.PostLogoutRedirectUris.Select(x => x.Uri).ToList(),
                        PrefixClientClaims = client.PrefixClientClaims,
                        RedirectUris = client.RedirectUris.Select(x => x.Uri).ToList(),
                        RefreshTokenExpiration = client.RefreshTokenExpiration,
                        RefreshTokenUsage = client.RefreshTokenUsage,
                        RequireConsent = client.RequireConsent,
                        ScopeRestrictions = client.ScopeRestrictions.Select(x => x.Scope).ToList(),
                        SlidingRefreshTokenLifetime = client.SlidingRefreshTokenLifetime
                    };
                    return model;
                }
            });
        }
    }
}