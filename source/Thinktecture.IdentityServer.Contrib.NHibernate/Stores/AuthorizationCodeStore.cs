using NHibernate;
using System;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;
using Thinktecture.IdentityServer.NH.Entities;
namespace Thinktecture.IdentityServer.NH.Stores {
    public class AuthorizationCodeStore : BaseTokenStore<AuthorizationCode>, IAuthorizationCodeStore {
        public AuthorizationCodeStore(ISessionFactory sessionFactory, IScopeStore scopeStore, IClientStore clientStore) : base(sessionFactory, TokenType.AuthorizationCode, scopeStore, clientStore) { }
        public override Task StoreAsync(string key, AuthorizationCode value) {
            using (var session = _SessionFactory.OpenSession()) {
                var nhToken = new Entities.Token {
                    Key = key,
                    SubjectId = value.SubjectId,
                    ClientId = value.ClientId,
                    JsonCode = ConvertToJson(value),
                    Expiry = DateTimeOffset.UtcNow.AddSeconds(value.Client.AuthorizationCodeLifetime),
                    TokenType = _TokenType
                };
                session.SaveOrUpdate(nhToken);
                session.Flush();
                return Task.FromResult(0);
            }
        }
    }
}