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

		/// <summary>
		/// 补充名额，主要用于抽取没到场的人员，以及临时增加的名额（如有嘉宾到场，增设的特等奖）
		/// </summary>
		public bool Additional { get; set; }
	}
}