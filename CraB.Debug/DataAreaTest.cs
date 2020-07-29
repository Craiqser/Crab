using CraB.Sql;

namespace CraB.Debug
{
	[ConnectionKey("Work")]
	public class DataAreaTest
	{
		/// <summary>Код компании.</summary>
		public string Id { get; set; }

		/// <summary>Название компании.</summary>
		public string Name { get; set; }
	}
}
