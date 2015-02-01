namespace Thinktecture.IdentityServer.NH.Entities {
    public class Consent {
        public virtual int Id { get; set; }
        public virtual string Subject { get; set; }
        public virtual string ClientId { get; set; }
        public virtual string Scopes { get; set; }
    }
}