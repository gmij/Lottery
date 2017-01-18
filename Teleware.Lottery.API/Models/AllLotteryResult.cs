using System.Collections.Generic;
using System.Linq;

namespace Teleware.Lottery.API.Models
{
	public class AllLotteryResult
	{
		public AllLotteryResult(IList<Winner> winners)
		{
			Winners = new Dictionary<string, List<Partner>>();
			foreach (var winner in winners)
			{
				if (!Winners.ContainsKey(winner.Award.Name))
				{
					Winners.Add(winner.Award.Name, new List<Partner>() {winner.Person});
				}
				else
				Winners[winner.Award.Name].Add(winner.Person);
			}

			//Winners = winners.GroupBy(w => w.Award).ToDictionary(w => w.Key.Name, w => w.Select(p => p.Person).ToList());
		}

		public Dictionary<string, List<Partner>> Winners { get; }
	}
}