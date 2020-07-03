using CraB.Sql;

namespace CraB.Debug
{
	[ConnectionKey("Work"), TableName("DATAAREA")]
	public class DATAAREA
	{
		public string ID {get; set;}
		public string NAME { get; set; }
		public int ISVIRTUAL { get; set; }
		public int TIMEZONE { get; set; }
		public long RECID { get; set; }
	}
}
