using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Services;
namespace Thinktecture.IdentityServer.NH.Stores {
    public class ConsentStore : IConsentStore {
        ISessionFactory _SessionFactory;
        public ConsentStore(ISessionFactory sessionFactory) {
            _SessionFactory = sessionFactory;
        }
        public Task<Thinktecture.IdentityServer.Core.Models.Consent> LoadAsync(string subject, string client) {
            return Task.Factory.StartNew<Thinktecture.IdentityServer.Core.Models.Consent>(() => {
                using (var session = _SessionFactory.OpenSession()) {
                    var found = session.Query<Entities.Consent>().FirstOrDefault(x => x.ClientId == client && x.Subject == subject);
                    if (found == null) {
                        return default(Thinktecture.IdentityServer.Core.Models.Consent);
                    }
                    var result = new Thinktecture.IdentityServer.Core.Models.Consent {
                        Subject = found.Subject,
                        ClientId = found.ClientId,
                        Scopes = ParseScopes(found.Scopes)
                    };
                    return result;
                }
            });
        }
        public Task UpdateAsync(Thinktecture.IdentityServer.Core.Models.Consent consent) {
            return Task.Factory.StartNew(() => {
                using (var session = _SessionFactory.OpenSession()) {
                    var item = session.Query<Entities.Consent>()
                        .FirstOrDefault(x => x.ClientId == consent.ClientId && x.Subject == consent.Subject)
                        ??
                        new Entities.Consent {
                            Subject = consent.Subject,
                            ClientId = consent.ClientId
                        };

                    if (consent.Scopes == null || !consent.Scopes.Any()) {
                        session.Delete(item);
                    } else {
                        item.Scopes = StringifyScopes(consent.Scopes);
                        session.SaveOrUpdate(item);
                    }
                }
            });
        }
        public Task<IEnumerable<Thinktecture.IdentityServer.Core.Models.Consent>> LoadAllAsync(string subject) {
            return Task.Factory.StartNew<IEnumerable<Thinktecture.IdentityServer.Core.Models.Consent>>(() => {
                using (var session = _SessionFactory.OpenSession()) {
                    var found = session.Query<Entities.Consent>().Where(c => c.Subject == subject).ToArray();
                    return found.Select(x => new Thinktecture.IdentityServer.Core.Models.Consent {
                        Subject = x.Subject,
                        ClientId = x.ClientId,
                        Scopes = ParseScopes(x.Scopes)
                    }).ToArray();
                }
            });
        }
        public Task RevokeAsync(string subject, string client) {
            return Task.Factory.StartNew(() => {
                using (var session = _SessionFactory.OpenSession()) {
                    var found = session.Query<Entities.Consent>().FirstOrDefault(c => c.Subject == subject && c.ClientId == client);
                    if (found != null) {
                        session.Delete(found);
                        session.Flush();
                    }
                }
            });
        }

        private IEnumerable<string> ParseScopes(string scopes) {
            if (scopes == null || string.IsNullOrWhiteSpace(scopes)) {
                return Enumerable.Empty<string>();
            }
            return scopes.Split(',');
        }
        private string StringifyScopes(IEnumerable<string> scopes) {
            if (scopes == null || !scopes.Any()) {
                return null;
            }
            return string.Join(",", scopes);
        }
    }
}