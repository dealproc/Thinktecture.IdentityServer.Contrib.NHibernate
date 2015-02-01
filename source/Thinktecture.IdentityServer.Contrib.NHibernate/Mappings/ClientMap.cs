using FluentNHibernate.Mapping;
using NHibernate.Type;
using Thinktecture.IdentityServer.Core.Models;
namespace Thinktecture.IdentityServer.NH.Mappings {
    public class ClientMap : ClassMap<Thinktecture.IdentityServer.NH.Entities.Client> {
        public ClientMap() {
            Schema(NHibernateServiceOptions.Schema);
            Id(x => x.Id);

            HasMany(x => x.ClientSecrets).AsSet().Cascade.All();

            HasMany(x => x.RedirectUris).AsSet().Cascade.All();
            HasMany(x => x.PostLogoutRedirectUris).AsSet().Cascade.All();
            HasMany(x => x.ScopeRestrictions).AsSet().Cascade.All();

            HasMany(x => x.IdentityProviderRestrictions).AsSet().Cascade.All();

            HasMany(x => x.Claims).AsSet().Cascade.All();

            HasMany(x => x.CustomGrantTypeRestrictions).AsSet().Cascade.All();

            Map(x => x.Flow).CustomType<EnumStringType<Flows>>();
            Map(x => x.RefreshTokenUsage).CustomType<EnumStringType<TokenUsage>>();
            Map(x => x.RefreshTokenExpiration).CustomType<EnumStringType<TokenExpiration>>();
            Map(x => x.AccessTokenType).CustomType<EnumStringType<AccessTokenType>>();

            Map(x => x.AbsoluteRefreshTokenLifetime);
            Map(x => x.AccessTokenLifetime);
            Map(x => x.AllowRememberConsent);
            Map(x => x.AlwaysSendClientClaims);
            Map(x => x.AuthorizationCodeLifetime);
            Map(x => x.ClientId);
            Map(x => x.ClientName);
            Map(x => x.ClientUri);
            Map(x => x.Enabled);
            Map(x => x.EnableLocalLogin);
            Map(x => x.IdentityTokenLifetime);
            Map(x => x.IncludeJwtId);
            Map(x => x.LogoUri);
            Map(x => x.PrefixClientClaims);
            Map(x => x.RequireConsent);
            Map(x => x.SlidingRefreshTokenLifetime);
        }
    }
}