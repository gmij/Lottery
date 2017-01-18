namespace Teleware.Lottery.API.Models
{
	public class Winner
	{

		public Winner()
		{
			
		}

		public Winner(Partner p, Award award)
		{
			Person = p;
			Award = award;
		}

		public Partner Person { get; set; }

		public string WorkNumber => Person.WorkNumber;

		public Award Award { get; set; }
	}
}