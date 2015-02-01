namespace Thinktecture.IdentityServer.NH.Entities {
    public class ClientScopeRestriction {
        public virtual int Id { get; set; }
        public virtual string Scope { get; set; }
        public virtual Client Client { get; set; }
    }
}