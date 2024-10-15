using System;
using System.Collections.Generic;

namespace TGLibrary {
	public static class LocalExtensions {
		// Based on https://github.com/morelinq/MoreLINQ/blob/master/MoreLinq/MaxBy.cs
		public static (TSource Max, TSource Min) ExtremaBy<TSource, TKey>(this IEnumerable<TSource> collection, Func<TSource, TKey> selector)
		  where TKey : IComparable<TKey> {
			if (collection == null) throw new ArgumentNullException($"'collection' not allowed to be null.");

			using (var e = collection.GetEnumerator()) {
				if (!e.MoveNext())
					return (default(TSource), default(TSource));

				TSource maxItem = e.Current;
				TKey maxKey = selector(maxItem);
				TSource minItem = e.Current;
				TKey minKey = selector(minItem);
				while (e.MoveNext()) {
					TSource currentItem = e.Current;
					TKey currentKey = selector(currentItem);
					if (currentKey.CompareTo(maxKey) > 0) {
						maxItem = currentItem;
						maxKey = currentKey;
					}
					if (currentKey.CompareTo(minKey) < 0) {
						minItem = currentItem;
						minKey = currentKey;
					}
				}

				return (maxItem, minItem);
			}
		}

		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> collection, Func<TSource, TKey> selector)
		  where TKey : IComparable<TKey> {
			return collection.ExtremaBy(selector).Max;
		}
		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> collection, Func<TSource, TKey> selector)
		  where TKey : IComparable<TKey> {
			return collection.ExtremaBy(selector).Min;
		}
	}
}
