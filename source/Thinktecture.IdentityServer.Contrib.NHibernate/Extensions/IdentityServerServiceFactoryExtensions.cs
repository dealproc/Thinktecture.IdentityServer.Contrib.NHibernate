using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Caches.SysCache;
using System;
using Thinktecture.IdentityServer.Core.Configuration;
using Thinktecture.IdentityServer.Core.Services;
using Thinktecture.IdentityServer.NH.Stores;
namespace Thinktecture.IdentityServer.NH.Extensions {
    public static class IdentityServerServiceFactoryExtensions {
        public static void RegisterOperationalServices(this IdentityServerServiceFactory factory, NHibernateServiceOptions options) {
            if (factory == null) { throw new ArgumentNullException("factory"); }

            factory.AuthorizationCodeStore = new Registration<IAuthorizationCodeStore, AuthorizationCodeStore>();
            factory.TokenHandleStore = new Registration<ITokenHandleStore, TokenHandleStore>();
            factory.ConsentStore = new Registration<IConsentStore, ConsentStore>();
            factory.RefreshTokenStore = new Registration<IRefreshTokenStore, RefreshTokenStore>();
        }

        public static void RegisterConfigurationServices(this IdentityServerServiceFactory factory, NHibernateServiceOptions options) {
            if (factory == null) { throw new ArgumentNullException("factory"); }
            factory.ClientStore = new Registration<IClientStore, ClientStore>();
            factory.ScopeStore = new Registration<IScopeStore, ScopeStore>();
        }

        public static void RegisterDatabaseComponents(this IdentityServerServiceFactory factory, NHibernateServiceOptions options) {
            if (factory == null) { throw new ArgumentNullException("factory"); }

            var configuration = Fluently.Configure()
                .Database(
                    FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2012
                        .Driver<NHibernate.Driver.SqlClientDriver>()
                        .Dialect<NHibernate.Dialect.MsSql2012Dialect>()
                        .ConnectionString(options.ConnectionString)
                )
                .ExposeConfiguration(c => c.SetProperty(NHibernate.Cfg.Environment.ReleaseConnections, "on_close"))
                .ExposeConfiguration(c => c.SetProperty(NHibernate.Cfg.Environment.ShowSql, "false"))
                .ExposeConfiguration(c => c.SetProperty(NHibernate.Cfg.Environment.UseSecondLevelCache, "true"))
                .ExposeConfiguration(c => c.SetProperty(NHibernate.Cfg.Environment.DefaultSchema, NHibernateServiceOptions.Schema))
                .Cache(c => c.ProviderClass<SysCacheProvider>().UseQueryCache().UseSecondLevelCache())
                .Mappings(m => {
                    m.AutoMappings.Add(
                        AutoMap
                            .AssemblyOf<NHibernateServiceOptions>(new AutomappingConfiguration())
                            .UseOverridesFromAssemblyOf<NHibernateServiceOptions>()
                            .Conventions.AddFromAssemblyOf<NHibernateServiceOptions>()
                    );
                })
                .BuildConfiguration();

            var sessionFactory = configuration.BuildSessionFactory();
            factory.Register(new Registration<ISessionFactory>(sessionFactory));
        }
    }
}