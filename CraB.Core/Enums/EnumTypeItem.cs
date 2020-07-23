using System;
using System.Collections.Generic;

namespace CraB.Core
{
	public class EnumTypeItem
	{
		public Dictionary<string, object> StringToValue { get; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
		public Dictionary<int, string> ValueToString { get; } = new Dictionary<int, string>();
	}
}
