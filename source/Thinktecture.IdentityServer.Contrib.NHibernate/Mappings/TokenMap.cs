using FluentNHibernate.Mapping;
using NHibernate.Type;
using Thinktecture.IdentityServer.NH.Entities;
namespace Thinktecture.IdentityServer.NH.Mappings {
    public class TokenMap : ClassMap<Token> {
        public TokenMap() {
            Schema(NHibernateServiceOptions.Schema);
            
            CompositeId()
                .KeyProperty(x => x.Key, set => { set.ColumnName("\"Key\""); })
                .KeyProperty(x => x.TokenType, set => { }).CustomType<EnumStringType<TokenType>>();

            Map(x => x.ClientId);
            Map(x => x.Expiry);
            Map(x => x.JsonCode);
            Map(x => x.SubjectId);
        }
    }
}