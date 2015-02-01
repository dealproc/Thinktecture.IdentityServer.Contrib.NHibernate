using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using System;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Services;
using Thinktecture.IdentityServer.NH.Logging;
namespace Thinktecture.IdentityServer.NH.Stores {
    public class ClientStore : IClientStore {
        static readonly ILog Logger = LogProvider.For<ClientStore>();
        ISessionFactory _SessionFactory;
        public ClientStore(ISessionFactory sessionFactory) {
            _SessionFactory = sessionFactory;
        }
        public Task<Thinktecture.IdentityServer.Core.Models.Client> FindClientByIdAsync(string clientId) {
            return Task.Factory.StartNew<Thinktecture.IdentityServer.Core.Models.Client>(() => {
                using (var session = _SessionFactory.OpenSession()) {
                    Logger.Log(LogLevel.Debug, () => string.Format("Retrieving information for client: '{0}'", clientId));
                    Entities.Client client = null;
                    try {

                        client = session.QueryOver<Entities.Client>()
                            .Where(x => x.ClientId == clientId)
                            .Fetch(x => x.ClientSecrets).Eager
                            .Fetch(x => x.ClientSecrets).Eager
                            .Fetch(x => x.RedirectUris).Eager
                            .Fetch(x => x.PostLogoutRedirectUris).Eager
                            .Fetch(x => x.ScopeRestrictions).Eager
                            .Fetch(x => x.IdentityProviderRestrictions).Eager
                            .Fetch(x => x.Claims).Eager
                            .Fetch(X => X.CustomGrantTypeRestrictions).Eager
                            .SingleOrDefault();

                    } catch (Exception ex) {
                        Logger.Log(LogLevel.Error, () => "Could not retrieve client from database.", ex);
                        return null;
                    }
                    if (client == null) {
                        Logger.Log(LogLevel.Debug, () => "Client was not found.");
                        return null;
                    }

                    Logger.Log(LogLevel.Debug, () => "Client found.  Building model and returning to caller.");
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