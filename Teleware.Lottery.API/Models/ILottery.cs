using System.Collections;
using System.Collections.Generic;

namespace Teleware.Lottery.API.Models
{
	public interface ILottery
	{
		LotteryInstance New(LotteryDefine define = null);

		SingleLotteryResult Lottery(SingleLotteryDefine define);

		LotteryInstance Get(string id);

	    LotteryInstance GetExistsInstance();

		void Save();

		IList<LotteryInstance> Load();

	}
}