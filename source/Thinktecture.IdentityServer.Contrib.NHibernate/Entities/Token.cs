using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Thinktecture.IdentityServer.NH.Entities {
    public class Token : IEquatable<Token> {
        [Key, Column(Order = 0)]
        public virtual string Key { get; set; }
        [Key, Column(Order = 1)]
        public virtual TokenType TokenType { get; set; }

        public virtual string SubjectId { get; set; }
        public virtual string ClientId { get; set; }
        public virtual string JsonCode { get; set; }
        public virtual DateTimeOffset Expiry { get; set; }

        public virtual bool Equals(Token other) {
            return Key.Equals(other.Key) && TokenType.Equals(other.TokenType);
        }
        public override bool Equals(object obj) {
            Token other = obj as Token;
            if (other == null) { return false; }
            return Equals(other);
        }
        public override int GetHashCode() {
            return (Key + "|" + TokenType).GetHashCode();
        }
    }
}