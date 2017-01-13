# <a href="http://www.teleware.com.cn">Teleware</a>公司的年会抽奖程序

<p>
本程序无任何后台存储，整个抽奖过程全运行在内存中，一但关闭后台的控制程序，将清空所有的已中奖信息。
<br/>
后台程序，每启动一次，只支持一个抽奖过程，毕竟单位抽奖，不可能反复多次。 
</p>

<p>
usage:
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
