using System;
using System.Collections.Generic;
using System.Linq;

namespace Teleware.Lottery.API.Models
{
	public class LotteryInstance
	{
		private readonly Func<SingleLotteryDefine, SingleLotteryResult> _func;

		public LotteryInstance(LotteryDefine define, Func<SingleLotteryDefine, SingleLotteryResult> lotteryFunc)
		{
			Define = define;
			Id = Guid.NewGuid().ToString("d");
			_func = lotteryFunc;
			Winners = new List<Winner>(define.Awards.Sum(a => a.Number));
		}

		public string Id { get; }

		public LotteryDefine Define { get; }

		public int Max => LotteryPool.Count;

		private IList<Partner> LotteryPool => Define.Partners;

		private IList<Winner> Winners { get; }

		public int AwardOver(Award award)
		{
			return award.Number - Winners.Count(w => w.Award == award);
		}

		public SingleLotteryDefine Begin(string award, int number, bool additional = false)
		{
			return new SingleLotteryDefine(Id) {Ranking = award, Number = number, Additional = additional};
		}

		public bool TryAddWinner(int workNum, Award award, out Partner p)
		{
			p = null;
			if ((workNum > Max) || (workNum < 0))
				return false;
			p = LotteryPool[workNum];
			var wn = p.WorkNumber;
			if (Winners.Any(w => w.WorkNumber == wn))
				return false;
			Winners.Add(new Winner(p, award));
			return true;
		}

		public SingleLotteryResult Lottery(SingleLotteryDefine define)
		{
			return _func(define);
		}

		public AllLotteryResult Total()
		{
			return new AllLotteryResult(Winners);
		}
	}
}