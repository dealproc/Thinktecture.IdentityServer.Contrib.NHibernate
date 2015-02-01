using Newtonsoft.Json;
using System;
using System.Linq;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;
namespace Thinktecture.IdentityServer.NH.Serialization {
    public class ScopeLite {
        public string Name { get; set; }
    }
    public class ScopeConverter : JsonConverter {
        readonly IScopeStore _ScopeStore;
        public ScopeConverter(IScopeStore scopeStore) {
            _ScopeStore = scopeStore;
        }
        public override bool CanConvert(Type objectType) {
            return typeof(Scope) == objectType;
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            var source = serializer.Deserialize<ScopeLite>(reader);
            var scopes = AsyncHelper.RunSync(async () => await _ScopeStore.FindScopesAsync(new string[] { source.Name }));
            return scopes.Single();
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var source = (Scope)value;
            var target = new ScopeLite { Name = source.Name };
            serializer.Serialize(writer, target);
        }
    }
}