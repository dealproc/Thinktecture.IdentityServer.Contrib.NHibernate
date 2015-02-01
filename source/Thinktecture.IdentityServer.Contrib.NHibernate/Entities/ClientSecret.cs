using System;
namespace Thinktecture.IdentityServer.NH.Entities {
    public class ClientSecret {
        public virtual int Id { get; set; }
        public virtual string Value { get; set; }
        public virtual string ClientSecretType { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTimeOffset? Expiration { get; set; }
        public virtual Client Client { get; set; }
    }
}
