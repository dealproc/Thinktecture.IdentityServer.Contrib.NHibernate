﻿namespace Thinktecture.IdentityServer.NH.Entities {
    public class ClientClaim {
        public virtual int Id { get; set; }
        public virtual string Type { get; set; }
        public virtual string Value { get; set; }
        public virtual Client Client { get; set; }
    }
}