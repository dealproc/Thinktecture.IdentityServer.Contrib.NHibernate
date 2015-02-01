using System;
using System.Collections.Generic;
namespace Thinktecture.IdentityServer.NH.Extensions {
    public static class EnumerableExtensions {
        public static void ForEach<TObject>(this IEnumerable<TObject> collection, Action<TObject> toDo) {
            foreach (var item in collection) {
                toDo.Invoke(item);
            }
        }
    }
}