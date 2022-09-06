using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Esoteric.Finance.Abstractions
{
    public static class EsotericLinqExtensions
    {
        public static bool NullSafeAny<T>([NotNullWhen(true)] this IEnumerable<T>? collection, Func<T, bool> predicate) => collection?.Any(predicate) == true;
        public static bool NullSafeAny<T>([NotNullWhen(true)] this IEnumerable<T>? collection) => collection?.Any() == true;
        public static bool NullOrNotAny<T>([NotNullWhen(false)] this IEnumerable<T>? collection, Func<T, bool> predicate) => collection?.Any(predicate) != true;
        public static bool NullOrNotAny<T>([NotNullWhen(false)] this IEnumerable<T>? collection) => collection?.Any() != true;
    }
}
