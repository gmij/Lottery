# <a href="http://www.teleware.com.cn">Teleware</a>公司的年会抽奖程序

<p>

</p>

<p>
usage:
<br/>
创建新的抽奖
GET  http://localhost:5000/api/lottery

</p>

<p>
抽取奖项
GET http://localhost:5000/api/lottery/{lotteryId}/{奖项ID}/{抽取数量}

</p>

<p>
查看中奖清单
GET http://localhost:5000/api/lottery/{lotteryId}/

</p>

<p>
强制抽取新的奖项
GET http://localhost:5000/api/lottery/{lotteryId}/{奖项ID}/{抽取数量}/true
</p>
