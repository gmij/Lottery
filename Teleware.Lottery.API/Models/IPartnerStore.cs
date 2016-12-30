using System.Collections.Generic;
using System.IO;
using Excel;

namespace Teleware.Lottery.API.Models
{
	public interface IPartnerStore
	{
		IList<Partner> Get();
	}

	class PartnerStore : IPartnerStore
	{
		IList<Partner> IPartnerStore.Get()
		{
			var file = Path.Combine(Directory.GetCurrentDirectory(), "partner.xls");
			using (var stream = File.Open(file, FileMode.Open, FileAccess.Read))
			{
				var excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
				var list = new List<Partner>();
				//	跳过首行的标题行
				excelReader.Read();
				while (excelReader.Read())
				{
					var p = new Partner()
					{
						WorkNumber = excelReader.GetString(0),
						Name = excelReader.GetString(1),
						Department = excelReader.GetString(2)
					};
					if (string.IsNullOrEmpty(p.Name))
						continue;
					list.Add(p);
				}
				excelReader.Close();
				stream.Close();
				return list;
			}
		}
	}
}