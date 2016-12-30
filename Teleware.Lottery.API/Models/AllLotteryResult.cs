using System.Collections.Generic;
using System.Linq;

namespace Teleware.Lottery.API.Models
{
	public class AllLotteryResult
	{
		public AllLotteryResult(IList<Winner> winners)
		{
			Winners = winners.GroupBy(w => w.Award).ToDictionary(w => w.Key.Name, w=> w.Select(p => p.Person).ToList());
		}

		public Dictionary<string, List<Partner>> Winners { get; }
	}
}