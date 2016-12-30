using System.Collections.Generic;

namespace Teleware.Lottery.API.Models
{
	/// <summary>
	///     抽奖信息定义
	/// </summary>
	public class LotteryDefine
	{
		public string Name { get; set; }

		public IList<Partner> Partners { get; set; }

		public IList<Award> Awards { get; set; }
	}
}