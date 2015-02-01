using NHibernate;
using System;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Services;
using Thinktecture.IdentityServer.NH.Entities;
namespace Thinktecture.IdentityServer.NH.Stores {
    public class TokenHandleStore : BaseTokenStore<Thinktecture.IdentityServer.Core.Models.Token>, ITokenHandleStore {
        public TokenHandleStore(ISessionFactory sessionFactory, IScopeStore scopeStore, IClientStore clientStore) : base(sessionFactory, TokenType.TokenHandle, scopeStore, clientStore) { }
        public override Task StoreAsync(string key, Thinktecture.IdentityServer.Core.Models.Token value) {
            return Task.Factory.StartNew(() => {
                using (var session = _SessionFactory.OpenSession()) 
                using (var transaction = session.BeginTransaction()) {
                    var nhToken = new Entities.Token {
                        Key = key,
                        SubjectId = value.SubjectId,
                        ClientId = value.ClientId,
                        JsonCode = ConvertToJson(value),
                        Expiry = DateTimeOffset.UtcNow.AddSeconds(value.Lifetime),
                        TokenType = _TokenType
                    };
                    session.SaveOrUpdate(nhToken);
                    session.Flush();
                    transaction.Commit();
                    return Task.FromResult(0);
                }
            });
        }
    }
}