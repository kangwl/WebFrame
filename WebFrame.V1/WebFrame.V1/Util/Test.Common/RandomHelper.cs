using System;
using System.Text;

namespace Test.Util.Common
{
    public class RandomHelper
    {
        /// <summary>
        ///     生成数字串随机数
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string CreateNumberStr(int len = 8)
        {
            var sb = new StringBuilder();
            for (var j = 0; j < len; j++)
            {
                var rand = new Random(Guid.NewGuid().GetHashCode());
                var n = rand.Next(1, 10);
                sb.Append(n);
            }
            return sb.ToString();
        }

        public static string CreateOrderNO(int len = 9)
        {
            var randomPart = CreateNumberStr(len);
            var orderNO = DateTime.Now.ToString("yyyyMMdd") + randomPart;
            return orderNO;
        }
    }
}