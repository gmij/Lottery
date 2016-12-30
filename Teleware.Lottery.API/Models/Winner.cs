namespace Teleware.Lottery.API.Models
{
	public class Winner
	{

		public Partner Person { get; }

		public string WorkNumber => Person.WorkNumber;

		public Winner(Partner p, Award award)
		{
			Person = p;
			Award = award;
		}

		public Award Award { get; }
		
	}
}