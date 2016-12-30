using System.Collections.Generic;
using System.Linq;

namespace Teleware.Lottery.API.Models
{
	public class SingleLotteryResult
	{
		public IList<Partner> Persons => Winners.Select(w => w.Person).ToList();

		public Award Award => Winners.FirstOrDefault()?.Award;

		protected internal IList<Winner> Winners { get; set; } 

		public int Over { get; set; }
	}

	public class EmptyLotteryResult : SingleLotteryResult
	{
		public EmptyLotteryResult()
		{
			Winners = new List<Winner>();
			Over = 0;
		}
	}
}