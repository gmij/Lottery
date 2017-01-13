namespace Teleware.Lottery.API.Models
{
	public interface ILottery
	{
		LotteryInstance New(LotteryDefine define = null);

		SingleLotteryResult Lottery(SingleLotteryDefine define);

		LotteryInstance Get(string id);

	    LotteryInstance GetExistsInstance();
	}
}