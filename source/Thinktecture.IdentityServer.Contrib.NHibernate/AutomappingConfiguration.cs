using FluentNHibernate.Automapping;
using System;
namespace Thinktecture.IdentityServer.NH {
    class AutomappingConfiguration : DefaultAutomappingConfiguration {
        public override bool ShouldMap(Type type) {
            return !string.IsNullOrWhiteSpace(type.Namespace) &&
                type.Namespace.StartsWith(this.GetType().Assembly.GetName().Name + ".Entities"); //Thinktecture.IdentityServer.NH.Entities
        }
        public override bool IsDiscriminated(Type type) {
            return base.IsDiscriminated(type);
        }
        public override bool IsComponent(Type type) {
            return base.IsComponent(type);
        }
    }
}