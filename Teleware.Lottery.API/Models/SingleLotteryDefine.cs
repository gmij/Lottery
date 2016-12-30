namespace Teleware.Lottery.API.Models
{
	public class SingleLotteryDefine
	{
		public SingleLotteryDefine(string id)
		{
			LotteryInstanceId = id;
		}

		public string LotteryInstanceId { get; }

		public string Ranking { get; set; }

		public int Number { get; set; }
	}
}