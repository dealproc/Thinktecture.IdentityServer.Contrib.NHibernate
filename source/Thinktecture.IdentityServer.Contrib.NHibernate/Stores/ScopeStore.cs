using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;
namespace Thinktecture.IdentityServer.NH.Stores {
    public class ScopeStore : IScopeStore {
        readonly ISessionFactory _SessionFactory;
        public ScopeStore(ISessionFactory sessionFactory) {
            _SessionFactory = sessionFactory;
        }
        public Task<IEnumerable<Scope>> FindScopesAsync(IEnumerable<string> scopeNames) {
            return Task.Factory.StartNew(() => {
                using (var session = _SessionFactory.OpenSession()) {
                    var query = session
                        .QueryOver<Entities.Scope>()
                        .Fetch(s => s.ScopeClaims).Eager;

                    if (scopeNames != null && scopeNames.Any()) {
                        query = query.WhereRestrictionOn(x => x.Name).IsIn(scopeNames.ToArray());
                    }

                    return ToModel(query.List());
                }
            });
        }
        public Task<IEnumerable<Scope>> GetScopesAsync(bool publicOnly = true) {
            return Task.Factory.StartNew(() => {
                using (var session = _SessionFactory.OpenSession()) {
                    var query = session
                        .QueryOver<Entities.Scope>()
                        .Fetch(s => s.ScopeClaims).Eager;


                    if (publicOnly) {
                        query = query.Where(s => s.ShowInDiscoveryDocument);
                    }

                    return ToModel(query.List());
                }
            });
        }

        private static IEnumerable<Scope> ToModel(IEnumerable<Entities.Scope> query) {
            return query.Select(x => new Scope {
                ClaimsRule = x.ClaimsRule,
                Description = x.Description,
                DisplayName = x.DisplayName,
                Emphasize = x.Emphasize,
                Enabled = x.Enabled,
                IncludeAllClaimsForUser = x.IncludeAllClaimsForUser,
                Name = x.Name,
                Required = x.Required,
                ShowInDiscoveryDocument = x.ShowInDiscoveryDocument,
                Type = (ScopeType)x.Type,
                Claims = new List<ScopeClaim>(x.ScopeClaims.Select(sc => new ScopeClaim {
                    AlwaysIncludeInIdToken = sc.AlwaysIncludeInIdToken,
                    Description = sc.Description,
                    Name = sc.Name
                }))
            });
        }
    }
}