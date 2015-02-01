using FluentNHibernate.Mapping;
using NHibernate.Type;
using Thinktecture.IdentityServer.Core.Models;
namespace Thinktecture.IdentityServer.NH.Mappings {
    public class ClientMap : ClassMap<Thinktecture.IdentityServer.NH.Entities.Client> {
        public ClientMap() {
            Schema(NHibernateServiceOptions.Schema);
            Id(x => x.Id);

            Map(x => x.Enabled).Default("True");

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
        }
    }
}