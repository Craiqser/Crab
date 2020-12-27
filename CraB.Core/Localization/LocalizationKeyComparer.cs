using System;
using System.Collections.Generic;

namespace CraB.Core
{
	public class LocalizationKeyComparer : IEqualityComparer<Tuple<string, string>>
	{
		public bool Equals(Tuple<string, string> x, Tuple<string, string> y)
		{
			return ((x == null) && (y == null))
				|| (x != null && y != null && StringComparer.Ordinal.Equals(x.Item1, y.Item1)
					&& StringComparer.Ordinal.Equals(x.Item2, y.Item2));
		}

		public int GetHashCode(Tuple<string, string> value)
		{
			return StringComparer.Ordinal.GetHashCode(value?.Item1)
				^ StringComparer.Ordinal.GetHashCode(value.Item2);
		}
	}
}
