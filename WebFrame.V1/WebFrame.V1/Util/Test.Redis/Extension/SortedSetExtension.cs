using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;

namespace Test.Util.Redis.Extension
{
    public static class SortedSetExtension
    {
        public static bool SortedSetAddModelEXT<TModel>(this IDatabase db, string key, TModel tModel, double score,
            CommandFlags flags = CommandFlags.None) where TModel : class
        {
            return db.SortedSetAdd(key, tModel.ToJson(), score, flags);
        }


        public static long SortedSetAddModelsEXT<TModel>(this IDatabase db, string key, Dictionary<TModel, double> dic,
            CommandFlags flags = CommandFlags.None) where TModel : class
        {
            var entries = dic.Select(pair => new SortedSetEntry(pair.Key.ToJson(), pair.Value)).ToArray();

            return db.SortedSetAdd(key, entries, flags);
        }

        //RedisKey key, RedisValue member, double value, CommandFlags flags = CommandFlags.None
        public static double SortedSetDecrementModelEXT<TModel>(this IDatabase db, string key, TModel member,
            double value,
            CommandFlags flags = CommandFlags.None) where TModel : class
        {
            return db.SortedSetDecrement(key, member.ToJson(), value, flags);
        }

        public static double SortedSetIncrementModelEXT<TModel>(this IDatabase db, string key, TModel member,
            double value,
            CommandFlags flags = CommandFlags.None) where TModel : class
        {
            return db.SortedSetIncrement(key, member.ToJson(), value, flags);
        }

        public static Dictionary<TModel, double> SortedSetRangeByRankWithScoresModelsEXT<TModel>(this IDatabase db,
            string key, long start = 0, long stop = -1, Order order = Order.Ascending,
            CommandFlags flags = CommandFlags.None) where TModel : class
        {
            var sortedSetEntries = db.SortedSetRangeByRankWithScores(key, start, stop, order, flags);
            var dic = sortedSetEntries.ToDictionary(entry => entry.Element.ToModel<TModel>(),
                entry => entry.Score);
            return dic;
        }

        public static List<TModel> SortedSetRangeByRankModelsEXT<TModel>(this IDatabase db, string key, long start = 0,
            long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None) where TModel : class
        {
            var redisValues = db.SortedSetRangeByRank(key, start, stop, order, flags);
            return redisValues.Select(one => one.ToModel<TModel>()).ToList();
        }

        public static List<TModel> SortedSetRangeByScoreModelsEXT<TModel>(this IDatabase db, string key,
            double start = double.NegativeInfinity, double stop = double.PositiveInfinity,
            Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1,
            CommandFlags flags = CommandFlags.None) where TModel : class
        {
            var redisValues = db.SortedSetRangeByScore(key, start, stop, exclude, order, skip, take, flags);

            return redisValues.Select(one => one.ToModel<TModel>()).ToList();
        }

        public static Dictionary<TModel, double> SortedSetRangeByScoreWithScoresModelsEXT<TModel>(this IDatabase db,
            string key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity,
            Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1,
            CommandFlags flags = CommandFlags.None) where TModel : class
        {
            var sortedSetEntries = db.SortedSetRangeByScoreWithScores(key, start, stop, exclude, order,
                skip, take, flags);
            var dic = sortedSetEntries.ToDictionary(entry => entry.Element.ToModel<TModel>(),
                entry => entry.Score);
            return dic;
        }

        public static long? SortedSetRankModelEXT<TModel>(this IDatabase db, string key, TModel member,
            Order order = Order.Ascending, CommandFlags flags = CommandFlags.None) where TModel : class
        {
            return db.SortedSetRank(key, member.ToJson(), order, flags);
        }

        public static bool SortedSetRemoveModelEXT<TModel>(this IDatabase db, string key, TModel member,
            CommandFlags flags = CommandFlags.None) where TModel : class
        {
            return db.SortedSetRemove(key, member.ToJson(), flags);
        }

        public static long SortedSetRemoveModelsEXT<TModel>(this IDatabase db, string key, List<TModel> members,
            CommandFlags flags = CommandFlags.None) where TModel : class
        {
            return db.SortedSetRemove(key, members.Select(one => (RedisValue) one.ToJson()).ToArray(), flags);
        }
    }
}