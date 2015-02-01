namespace Thinktecture.IdentityServer.NH {
    public class NHibernateServiceOptions {
        /// <summary>
        ///  Set this before you build your container so that it's used everywhere.
        /// </summary>
        public static string Schema { get; set; }
        public string ConnectionString { get; set; }
        static NHibernateServiceOptions() {
            Schema = "dbo";
        }
    }
}