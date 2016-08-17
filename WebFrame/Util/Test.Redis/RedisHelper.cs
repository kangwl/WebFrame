using System;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;
using Test.Util.Redis.Extension;

namespace Test.Util.Redis
{
    /// <summary>
    ///     redis操作帮助类
    ///     不建议使用有序列化的方法
    /// </summary>
    public static class RedisHelper
    {
        /// <summary>
        ///     配置
        /// </summary>
        public static class Option
        {
            static Option()
            {
                RedisServerIP = "127.0.0.1";
                Port = 6379;
                AllowAdmin = false;
                Password = "";
            }

            public static string RedisServerIP { get; set; }
            public static int Port { get; set; }
            public static bool AllowAdmin { get; set; }
            public static string Password { get; set; }
        }

        #region main

        private static readonly object _locker = new object();
        private static ConnectionMultiplexer _instance;


        private static readonly ConfigurationOptions options = new ConfigurationOptions
        {
            AllowAdmin = Option.AllowAdmin,
            EndPoints =
            {
                {Option.RedisServerIP, Option.Port}
            },
            Password = Option.Password
        };

        /// <summary>
        ///     使用一个静态属性来返回已连接的实例，如下列中所示。这样，一旦 ConnectionMultiplexer 断开连接，便可以初始化新的连接实例。
        /// </summary>
        public static ConnectionMultiplexer connection
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null || !_instance.IsConnected)
                        {
                            if (string.IsNullOrEmpty(Option.RedisServerIP))
                            {
                                Option.RedisServerIP = "127.0.0.1";
                            }
                            _instance = ConnectionMultiplexer.Connect(options);
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        ///     数据库操作
        /// </summary>
        private static IDatabase Db => connection.GetDatabase();

        //server
        // private static IServer Server => connection.GetServer(options.EndPoints[0]);

        #endregion

        #region common methods

        public static bool SetExpire(string key, DateTime dtExpire)
        {
            return Db.KeyExpire(key, dtExpire);
        }

        public static bool SetExpire(string key, TimeSpan timeSpan)
        {
            return Db.KeyExpire(key, timeSpan);
        }

        #endregion

        #region hash

        public static bool HashSet(string hashKey, Dictionary<string, dynamic> dic)
        {
            return Db.HashSetEXT(dic, hashKey);
        }

        public static bool HashDelete(string key, string field)
        {
            return Db.HashDelete(key, field);
        }

        public static bool HashDelete(string key, params string[] fields)
        {
            var removedCount = Db.HashDelete(key, fields.Select(one => (RedisValue) one).ToArray());
            return removedCount == fields.Length;
        }

        public static double HashIncrement(string key, string field, double val)
        {
            return Db.HashIncrement(key, field, val);
        }

        public static long HashIncrement(string key, string field, long val = 1L)
        {
            return Db.HashIncrement(key, field, val);
        }

        public static double HashDecrement(string key, string field, double val)
        {
            return Db.HashDecrement(key, field, val);
        }

        public static long HashDecrement(string key, string field, long val = 1L)
        {
            return Db.HashDecrement(key, field, val);
        }

        public static bool HashExists(string key, string field)
        {
            return Db.HashExists(key, field);
        }

        public static Dictionary<string, string> HashGetAll(string key)
        {
            var hashEntries = Db.HashGetAll(key);
            return hashEntries.ToDictionary<HashEntry, string, string>(entry => entry.Name, entry => entry.Value);
        }

        public static IEnumerable<string> HashKeys(string key)
        {
            var redisValues = Db.HashKeys(key);
            return redisValues.Select(one => one.ToStringEXT());
        }

        public static IEnumerable<string> HashValues(string key)
        {
            var redisValues = Db.HashValues(key);
            return redisValues.Select(one => one.ToStringEXT());
        }

        public static long HashLength(string key)
        {
            return Db.HashLength(key);
        }


        public static string HashGet(string hashkey, string hashField)
        {
            return Db.HashGet(hashkey, hashField);
        }

        public static string[] HashGetMuti(string hashkey, params string[] hashFields)
        {
            return Db.HashGetMutiEXT(hashkey, hashFields);
        }

        /// <summary>
        ///     根据hashkey获取一条记录
        /// </summary>
        /// <param name="hashkey">hash记录的key</param>
        /// <param name="hashFields">hash一条记录的字段</param>
        /// <returns></returns>
        public static Dictionary<string, string> HashGetDic(string hashkey, params string[] hashFields)
        {
            var dic = new Dictionary<string, string>();
            var redisValues = Db.HashGet(hashkey, hashFields.Select(one => (RedisValue) one).ToArray());
            for (var i = 0; i < hashFields.Length; i++)
            {
                var field = hashFields[i];
                string val = redisValues[i];
                dic.Add(field, val);
            }
            return dic;
        }

        #endregion

        #region list

        /// <summary>
        ///     返回列表长度
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ListLeftPush(string key, string value)
        {
            return Db.ListLeftPush(key, value);
        }

        public static long ListLeftPush(string key, List<string> values)
        {
            return Db.ListLeftPush(key, values.Select(one => (RedisValue) one).ToArray());
        }

        public static string ListLeftPop(string key)
        {
            return Db.ListLeftPop(key);
        }


        public static string ListRightPop(string key)
        {
            return Db.ListRightPop(key);
        }

        /// <summary>
        ///     auto
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ListRightPopLeftPush(string key, string value)
        {
            var redisValue = Db.ListRightPopLeftPush(value, key);
            return redisValue;
        }

        public static long ListRightPush(string key, string value)
        {
            return Db.ListRightPush(key, value);
        }

        public static long ListRightPush(string key, List<string> values)
        {
            return Db.ListRightPush(key, values.Select(one => (RedisValue) one).ToArray());
        }

        public static long ListRemove(string key, string value)
        {
            return Db.ListRemove(key, value);
        }

        public static IEnumerable<string> ListRange(string key, long start = 0, long stop = -1)
        {
            var redisValues = Db.ListRange(key, start, stop);
            return redisValues.Select(one => one.ToStringEXT());
        }

        public static long ListInsertAfter(string key, string positionVal, string val)
        {
            return Db.ListInsertAfter(key, positionVal, val);
        }

        public static long ListInsertBefore(string key, string positionVal, string val)
        {
            return Db.ListInsertBefore(key, positionVal, val);
        }

        public static void ListSetByIndex(string key, long index, string value)
        {
            Db.ListSetByIndex(key, index, value);
        }

        public static string ListGetByIndex(string key, long index)
        {
            return Db.ListGetByIndex(key, index);
        }

        public static IEnumerable<string> ListSort(string key, long skip = 0, long take = -1, bool asc = true,
            bool sortNumeric = true)
        {
            var redisValues = Db.Sort(key, skip, take, asc ? Order.Ascending : Order.Descending,
                sortNumeric ? SortType.Numeric : SortType.Alphabetic);
            return redisValues.Select(one => one.ToStringEXT());
        }

        #endregion

        #region set

        public static bool SetAdd(string key, string value)
        {
            return Db.SetAdd(key, value);
        }

        /// <summary>
        ///     返回添加的数量
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static long SetAdd(string key, List<string> values)
        {
            return Db.SetAdd(key, values.Select(one => (RedisValue) one).ToArray());
        }

        public static bool SetRemove(string key, string value)
        {
            return Db.SetRemove(key, value);
        }

        public static bool SetRemove(string key, List<string> values)
        {
            var removeCount = Db.SetRemove(key, values.Select(one => (RedisValue) one).ToArray());

            return values.Count == removeCount;
        }

        public static bool SetContains(string key, string value)
        {
            return Db.SetContains(key, value);
        }

        /// <summary>
        ///     获取set长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long SetLength(string key)
        {
            return Db.SetLength(key);
        }

        public static List<string> SetScan(string key, string parten, int pageSize = 10)
        {
            var redisValues = Db.SetScan(key, parten, pageSize);
            return redisValues.Select(one => one.ToStringEXT()).ToList();
        }

        public static IEnumerable<string> SetSort(string key, long skip = 0, long take = -1, bool asc = true,
            bool sortNumeric = true)
        {
            var redisValues = Db.Sort(key, skip, take, asc ? Order.Ascending : Order.Descending,
                sortNumeric ? SortType.Numeric : SortType.Alphabetic);
            return redisValues.Select(one => one.ToStringEXT());
        }

        #endregion

        #region sorted set

        public static bool SortedSetAdd(string key, string value, double score)
        {
            return Db.SortedSetAdd(key, value, score);
        }

        public static bool SortedSetAdd(string key, Dictionary<string, double> dic)
        {
            var entries = dic.Select(pair => new SortedSetEntry(pair.Key, pair.Value)).ToArray();

            var count = Db.SortedSetAdd(key, entries);
            return count == dic.Count;
        }

        public static double SortedSetDecrement(string key, string member, double value)
        {
            return Db.SortedSetDecrement(key, member, value);
        }

        public static double SortedSetIncrement(string key, string member, double value)
        {
            return Db.SortedSetIncrement(key, member, value);
        }

        public static Dictionary<string, double> SortedSetRangeByRankWithScores(string key, long start = 0,
            long stop = -1, bool asc = true)
        {
            var sortedSetEntries = Db.SortedSetRangeByRankWithScores(key, start, stop,
                asc ? Order.Ascending : Order.Descending);
            var dic = sortedSetEntries.ToDictionary(entry => entry.Element.ToModel<string>(),
                entry => entry.Score);
            return dic;
        }

        public static IEnumerable<string> SortedSetRangeByRank(string key, long start = 0,
            long stop = -1, bool asc = true)
        {
            var redisValues = Db.SortedSetRangeByRank(key, start, stop,
                asc ? Order.Ascending : Order.Descending);
            return redisValues.Select(one => one.ToStringEXT());
        }

        public static IEnumerable<string> SortedSetRangeByScore(string key, bool asc = true, long skip = 0,
            long take = -1)
        {
            var redisValues = Db.SortedSetRangeByScore(key, order: asc ? Order.Ascending : Order.Descending,
                skip: skip, take: take);

            return redisValues.Select(one => one.ToStringEXT());
        }

        public static Dictionary<string, double> SortedSetRangeByScoreWithScores(string key, bool asc = true,
            long skip = 0,
            long take = -1)
        {
            var sortedSetEntries = Db.SortedSetRangeByScoreWithScores(key,
                order: asc ? Order.Ascending : Order.Descending,
                skip: skip, take: take);
            var dic = sortedSetEntries.ToDictionary(entry => entry.Element.ToStringEXT(),
                entry => entry.Score);
            return dic;
        }

        public static long? SortedSetRank(string key, string member, bool asc = true)
        {
            return Db.SortedSetRank(key, member, asc ? Order.Ascending : Order.Descending);
        }

        public static bool SortedSetRemove(string key, string member)
        {
            return Db.SortedSetRemove(key, member);
        }

        public static long SortedSetRemove(string key, List<string> members)
        {
            return Db.SortedSetRemove(key, members.Select(one => (RedisValue) one).ToArray());
        }

        #endregion

        #region string

        public static bool StringSet(string key, dynamic value,
            TimeSpan? expiry = default(TimeSpan?))
        {
            return Db.StringSet(key, value, expiry);
        }

        public static string StringGet(string key)
        {
            var redisValue = Db.StringGet(key);

            return redisValue;
        }

        public static double StringIncrement(string key, double value = 1)
        {
            var ret = Db.StringIncrement(key, value);
            return ret;
        }

        #endregion
    }
}