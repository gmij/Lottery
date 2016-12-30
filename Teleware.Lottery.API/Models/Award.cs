namespace Teleware.Lottery.API.Models
{
	public class Award
	{
		/// <summary>
		///     奖项ID
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		///     奖项名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///     中奖人数
		/// </summary>
		public int Number { get; set; }

		/// <summary>
		///     奖品
		/// </summary>
		public string Meed { get; set; }
	}
}