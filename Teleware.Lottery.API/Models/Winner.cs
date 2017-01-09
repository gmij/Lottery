namespace Teleware.Lottery.API.Models
{
	public class Winner
	{
		public Winner(Partner p, Award award)
		{
			Person = p;
			Award = award;
		}

		public Partner Person { get; }

		public string WorkNumber => Person.WorkNumber;

		public Award Award { get; }
	}
}