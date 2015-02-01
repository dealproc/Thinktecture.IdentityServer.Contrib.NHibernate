using NHibernate;
using System;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;
using Thinktecture.IdentityServer.NH.Entities;
namespace Thinktecture.IdentityServer.NH.Stores {
    public class RefreshTokenStore : BaseTokenStore<RefreshToken>, IRefreshTokenStore {
        public RefreshTokenStore(ISessionFactory sessionFactory, IScopeStore scopeStore, IClientStore clientStore) : base(sessionFactory, TokenType.RefreshToken, scopeStore, clientStore) { }
        public override Task StoreAsync(string key, RefreshToken value) {
            using (var session = _SessionFactory.OpenSession()) {
                var nhToken = new Entities.Token {
                    Key = key,
                    SubjectId = value.SubjectId,
                    ClientId = value.ClientId,
                    JsonCode = ConvertToJson(value),
                    Expiry = DateTimeOffset.UtcNow.AddSeconds(value.LifeTime),
                    TokenType = _TokenType
                };
                session.SaveOrUpdate(nhToken);
                session.Flush();
                return Task.FromResult(0);
            }
        }
    }
}