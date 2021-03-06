﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Teleware.Lottery.API.Models
{
	internal class Lottery : ILottery
	{
		private readonly LotteryDefine _define;

		private readonly IList<LotteryInstance> _instances;

		private readonly IPartnerStore _store;

		private ILottery This => this;

		public Lottery()
		{
			var definePath = ComboPath("lotterydefine.json");
			var defineBody = File.ReadAllText(definePath);
			if (string.IsNullOrEmpty(defineBody))
				throw new FileNotFoundException("找不到奖项定义文件", definePath);
			_define = JsonConvert.DeserializeObject<LotteryDefine>(defineBody);
			_instances = This.Load();// new List<LotteryInstance>();
			_store = new PartnerStore();
		}

		private string ComboPath(string fileName)
		{
			return Path.Combine(Directory.GetCurrentDirectory(), fileName);
		}


		SingleLotteryResult ILottery.Lottery(SingleLotteryDefine define)
		{
			var instance = This.Get(define.LotteryInstanceId);
			if (instance == null)
				throw new ArgumentOutOfRangeException(nameof(define.LotteryInstanceId), "找不到该抽奖实例");
			var award = instance.Define.Awards.FirstOrDefault(a => a.Id == define.Ranking);
			if (award == null)
				throw new ArgumentNullException(nameof(award), "找不到该奖项");

			//	判断剩余该奖项的剩余名称，没有名额了则直接返回
			var over = instance.AwardOver(award);

			//	非增补情况下， 要对人员数量进行检查，
			//	增补情况下， 暂时不做检查。
			if (!define.Additional)
			{
				//	如果希望抽取的数量，大于奖项设定数，退出
				if (award.Number < define.Number)
					define.Number = award.Number;
				//	判断剩余该奖项的剩余名称，没有名额了则直接返回
				if (over == 0)
					return new EmptyLotteryResult();
				//	奖项的剩余名额不足
				if (define.Number > over)
					define.Number = over;
			}
			var r = new Random();
			var list = new List<Winner>(define.Number);
			for (var i = 0; i < define.Number; i++)
				while (true)
				{
					Partner p;
					//	检查随机的人员，是否已中奖
					if (!instance.TryAddWinner(r.Next(instance.Max), award, out p))
						continue;
					list.Add(new Winner(p, award));
					break;
				}
			This.Save();
			return new SingleLotteryResult
			{
				Over = over - define.Number,
				Winners = list
			};
		}

		LotteryInstance ILottery.New(LotteryDefine define)
		{
			var instance = new LotteryInstance(define ?? _define, This.Lottery);
			instance.Define.Partners = _store.Get();
			_instances.Add(instance);
			return instance;
		}

		LotteryInstance ILottery.Get(string id)
		{
			return _instances.FirstOrDefault(instance => instance.Id == id);
		}

        LotteryInstance ILottery.GetExistsInstance()
        {
            return _instances.FirstOrDefault();
        }

		void ILottery.Save()
		{
			var content = JsonConvert.SerializeObject(_instances);
			var file = ComboPath("lotteryInstance.json");
			File.WriteAllText(file, content);
		}

		IList<LotteryInstance> ILottery.Load()
		{
			var file = ComboPath("lotteryInstance.json");
			if (File.Exists(file))
			{
				var content = File.ReadAllText(file);
				var instances = JsonConvert.DeserializeObject<IList<LotteryInstance>>(content);
				foreach (var instance in instances)
				{
					instance.LotteryFunc = This.Lottery;
				}
				return instances.TakeWhile(instance => instance.Define.Awards.Sum(a => a.Number) > instance.Winners.Count).ToList();

			}
			return new List<LotteryInstance>();
		}
	}
}