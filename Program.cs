using System;
using System.Text;
using System.Threading;
using System.Diagnostics;
using InfluxData.Net.InfluxDb;
using InfluxData.Net.Common.Enums;
using System.Collections.Generic;

namespace Hello_World
{
    class Program
    {

        static string[] levels = { "INFO", "DEBUG", "WARN", "ERROR" };
        static string[] sellerNicks = { "无敌测试部", "孔方时代", "test-sop", "婷美化妆品旗舰店", "国玛餐具", "远久回味", "小d是妖精", "牛郎游戏币", "costar2011", "高大上测试", "莱茵手表旗舰店", "ebestwang", "苏泊尔沃克尔专卖店", "tb_3436852", "长江踏浪", "springfield旗舰店", "憨尼鹿旗舰店", "1005314678_10", "欣源花坊", "微笑天使风铃", "小建轮滑", "dotcomhats", "service.trade_node", "索兰旗舰店", "jiayesweety", "足之有道家居专营店", "军雪雪", "百信药业", "15161102606zyl", "宏达加盟", "潜水艇济南专卖店", "亚当爱户外", "亿林图书专营店", "tb_9150475", "奈美根西安专递", "幸满衣族", "ccrx", "宝来车品", "卓彭母婴专营店", "钰烽旗舰店", "peskoe半球全球专卖店", "luan1288", "tb1104616", "贪吃飒零食店正品", "腾易电器", "考试人生", "vyannine", "咱只为您服务", "zhufuzhi", "张康康92", "棒棒糖糖1", "三个好爸爸", "岚爵旗舰店", "yixiu8017", "skyto2048", "lqkk5201", "威登保罗旗舰店", "tb_5396610", "男人帮服" };
        static string[] className = { "min", "join", "hex", "replace", "contains", "srid", "current_timestamp", "show contributors", "variance", "drop server", "show authors", "var_samp", "concat", "geometry hierarchy", "char function", "datetime", "show create trigger", "show create procedure", "open", "integer", "lower", "show columns", "create trigger", "month", "tinyint", "show triggers", "master_pos_wait", "regexp", "if statement", "^", "drop view", "within", "week", "show plugins", "drop function udf", "prepare", "lock", "updatexml", "reset slave", "show binary logs", "polygon", "minute", "day", "mid", "uuid", "linestring", "sleep", "connection_id", "delete", "round", "nullif", "close", "stop slave", "timediff", "replace function", "use", "linefromtext", "case operator", "show master status", "addtime", "spatial", "to_seconds", "timestampdiff", "upper", "from_unixtime", "mediumblob", "sha2", "ifnull", "show function code", "show errors", "least", "=", "reverse", "isnull", "binary", "blob data type", "boundary", "create user", "point", "current_user", "lcase", "<=", "show profiles", "update", "is not null", "case statement" };


        static void Main(string[] args)
        {

            int numThread = 1;
            string batchSize = "1";
            string insertTimes = "1";

            // Console.WriteLine("input number of threads:");
            // numThread = Convert.ToInt32(Console.ReadLine());
            // Console.WriteLine("input batch size:");
            // batchSize = Console.ReadLine();
            // Console.WriteLine("input cycle times:");
            // insertTimes = Console.ReadLine();

            InsertData("1,1,1");

            for (int i = 0; i < numThread; i++)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(InsertData));
                thread.Start(i.ToString() + "," + batchSize.ToString() + "," + insertTimes.ToString());
            }

            Console.WriteLine("Hello World!");
        }

        public static void InsertData(object param)
        {
            string threadId = param.ToString().Split(',')[0];
            int batchSize = Convert.ToInt32(param.ToString().Split(',')[1]);
            int insertTimes = Convert.ToInt32(param.ToString().Split(',')[2]);
            //Influxdb写入地址
            string influxUrl = "http://114.55.6.199:30001/";

            string message = string.Empty;
            message = string.Format("System.Web.HttpRequestValidationException (0x80004005): 从客户端(ajaxEserviceId=\" <? xml version = \"1.0\"...\")中检测到有潜在危险的 Request.QueryString 值。在 System.Web.HttpRequest.ValidateString(String value, String collectionKey, RequestValidationSource requestCollection)在 System.Web.HttpValueCollection.Get(String name)在 Kedao.QianNiuAngular.Controllers.BaseController.OnActionExecuting(ActionExecutingContext filterContext)");

            Random randomIndex = new Random();
            DateTime start = DateTime.Now;
            for (int i = 0; i < insertTimes; i++)
            {
                StringBuilder log = new StringBuilder();


                List<InfluxData.Net.InfluxDb.Models.Point> points = new List<InfluxData.Net.InfluxDb.Models.Point>();
                for (int j = 0; j < batchSize; j++)
                {
                    var pointToWrite = new InfluxData.Net.InfluxDb.Models.Point()
                    {
                        Name = "log", // serie/measurement/table to write into
                        Tags = new Dictionary<string, object>()
                        {
                            { "level", levels[randomIndex.Next(0, 3)] },
                            { "sellerNick", sellerNicks[randomIndex.Next(0, 58)] },
                            { "className", className[randomIndex.Next(0, 85)] }
                        },
                        Fields = new Dictionary<string, object>()
                        {
                            { "createTime", DateTime.Now },
                            { "message", message },
                            { "serverIp", "192.168.1.125" }
                        },
                        Timestamp = DateTime.UtcNow // optional (can be set to any DateTime moment)
                    };

                    points.Add(pointToWrite);

                    // log.AppendFormat("clx_test,level='{0}',sellerNick='{1}',className='{2}' createTime='{3}',message='{4}',serverIp='{5}'", levels[randomIndex.Next(0, 3)], sellerNicks[randomIndex.Next(0, 58)], className[randomIndex.Next(0, 85)], DateTime.Now, message, "192.168.1.125");
                    // log.AppendFormat("clx_test,level=ERROR,sellerNick=wdcs,className=contributors createTime='{0}',message=error,serverIp={1} {2}", DateTime.Now, "192.168.1.125", DateTime.Now.Millisecond);
                    // log.Append(System.Environment.NewLine);
                }

                InfluxDbClient influxDbClient = new InfluxDbClient(influxUrl, "log", "AsDf123654", InfluxDbVersion.v_1_3);
                var response = influxDbClient.Client.WriteAsync(points, "clx_test");
#if DEBUG
                Debug.WriteLine(String.Format("[Error] {0} {1}", 1, 3));
#endif

                Stopwatch inWatch = new Stopwatch();
                inWatch.Start();


                //influxUrl = "http://114.55.6.199:30001/query?q=show databases";write?db=clx_test
                log.Clear();


                // string result = httpClient.DoPost(influxUrl, log.ToString());
                inWatch.Stop();
                long milSeconds = inWatch.ElapsedMilliseconds;

                DateTime pause = DateTime.Now;
                double time = (pause - start).TotalMilliseconds;

                string status = string.Format("进程【{0}】：batch = 【{1}】;current speed = 【{2}】/s;total speed = 【{3}】/s", threadId, batchSize, batchSize * 1000 / milSeconds, batchSize * 1000 * i / time);

                Console.WriteLine(status);
                Console.ReadKey();
            }


        }
    }
}
