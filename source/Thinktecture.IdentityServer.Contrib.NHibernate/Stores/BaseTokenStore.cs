using Newtonsoft.Json;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;
using Thinktecture.IdentityServer.NH.Entities;
using Thinktecture.IdentityServer.NH.Extensions;
namespace Thinktecture.IdentityServer.NH.Stores {
    public abstract class BaseTokenStore<T> where T : class {
        protected readonly TokenType _TokenType;
        protected readonly ISessionFactory _SessionFactory;
        private readonly IScopeStore _ScopeStore;
        private readonly IClientStore _ClientStore;

        protected BaseTokenStore(ISessionFactory sessionFactory, TokenType tokenType, IScopeStore scopeStore, IClientStore clientStore) {
            if (sessionFactory == null) { throw new ArgumentNullException("sessionFactory"); }
            if (scopeStore == null) { throw new ArgumentNullException("scopeStore"); }
            if (clientStore == null) { throw new ArgumentNullException("clientStore"); }

            _SessionFactory = sessionFactory;
            _TokenType = tokenType;
            _ScopeStore = scopeStore;
            _ClientStore = clientStore;
        }
        JsonSerializerSettings GetJsonSerializerSettings() {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new Serialization.ClaimConverter());
            settings.Converters.Add(new Serialization.ClaimsPrincipalConverter());
            settings.Converters.Add(new Serialization.ClientConverter(_ClientStore));
            settings.Converters.Add(new Serialization.ScopeConverter(_ScopeStore));
            return settings;
        }
        protected string ConvertToJson(T value) {
            return JsonConvert.SerializeObject(value, GetJsonSerializerSettings());
        }
        protected T ConvertFromJson(string json) {
            return JsonConvert.DeserializeObject<T>(json, GetJsonSerializerSettings());
        }
        public Task<T> GetAsync(string key) {
            return Task.Factory.StartNew<T>(() => {
                using (var session = _SessionFactory.OpenSession()) {
                    var token = session.Query<Entities.Token>().FirstOrDefault(x => x.TokenType == _TokenType && x.Key == key);

                    if (token == null || token.Expiry < DateTimeOffset.UtcNow) {
                        return null;
                    }

                    return ConvertFromJson(token.JsonCode);
                }
            });
        }
        public Task RemoveAsync(string key) {
            return Task.Factory.StartNew(() => {
                using (var session = _SessionFactory.OpenSession()) {
                    var token = session.Query<Entities.Token>().FirstOrDefault(x => x.TokenType == _TokenType && x.Key == key);

                    if (token != null) {
                        session.Delete(token);
                        session.Flush();
                    }
                }
            });
        }
        public Task<IEnumerable<ITokenMetadata>> GetAllAsync(string subject) {
            return Task.Factory.StartNew(() => {
                using (var session = _SessionFactory.OpenSession()) {
                    var tokens = session.Query<Entities.Token>()
                        .Where(x => x.SubjectId == subject && x.TokenType == _TokenType)
                        .Select(x => ConvertFromJson(x.JsonCode))
                        .ToArray();

                    return tokens.Cast<ITokenMetadata>();
                }
            });
        }
        public Task RevokeAsync(string subject, string client) {
            return Task.Factory.StartNew(() => {
                using (var session = _SessionFactory.OpenSession()) {
                    var found = session.Query<Entities.Token>()
                        .Where(x => x.SubjectId == subject && x.ClientId == client && x.TokenType == _TokenType).ToArray();
                    found.ForEach(token => session.Delete(token));
                    session.Flush();
                }
            });
        }

        public abstract Task StoreAsync(string key, T value);
    }
}