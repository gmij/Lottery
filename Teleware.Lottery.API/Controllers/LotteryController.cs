using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teleware.Lottery.API.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Teleware.Lottery.API.Controllers
{
	[Route("api/[controller]")]
	public class LotteryController : Controller
	{
		private readonly ILottery _lottery;

		public LotteryController(ILottery lottery)
		{
			_lottery = lottery;
		}


		// GET: api/values
		[HttpGet]
		public LotteryInstance Get()
		{
			return _lottery.GetExistsInstance() ?? _lottery.New();
		}

		[HttpGet("{id}/{award}/{number}/{additional?}")]
		public SingleLotteryResult Get(string id, string award, int number, bool additional = false)
		{
			var instance = _lottery.Get(id);
			if (instance == null)
			{
				throw new NullReferenceException("服务端已重启，请刷新客户端");
			}
			return instance.Lottery(instance.Begin(award, number, additional));
		}


		[HttpGet("{id}")]
		public AllLotteryResult Get(string id)
		{
			var instance = _lottery.Get(id) ?? _lottery.GetExistsInstance();
			return instance.Total();
		}
	}
}