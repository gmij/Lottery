using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Http;
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
	        var id = Request.Cookies["LotteryId"];
	        if (string.IsNullOrEmpty(id))
	        {
		        var instance = _lottery.New();
				Response.Cookies.Append("LotteryId", instance.Id, new CookieOptions() {HttpOnly = true});
				Response.Headers.Add("P3P", "CP=CAO PSA OUR");
				return instance;
	        }
	        else
	        {
		        return _lottery.Get(id);
	        }
        }

		[HttpGet("{id}/{award}/{number}/{additional?}")]
        public SingleLotteryResult Get(string id, string award, int number, bool additional = false)
        {
	        var instance = _lottery.Get(id);
	        return instance.Lottery(instance.Begin(award, number, additional));
        }
		



		[HttpGet("{id}")]
	    public AllLotteryResult Get(string id)
	    {
			var instance = _lottery.Get(id);
		    return instance.Total();
	    }
		
    }

}

