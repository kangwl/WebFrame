using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;

namespace Test.Util.Redis.Extension
{
    /// <summary>
    ///     hash 操作扩展
    /// </summary>
    public static class HashExtension
    {
        public static bool HashSetModelEXT<TModel>(this IDatabase db, string key, string hashField, TModel value,
            When when = When.Always, CommandFlags flags = CommandFlags.None) where TModel : class, new()
        {
            return db.HashSet(key, hashField, value.ToJson(), when, flags);
        }

        /// <summary>
        ///     存在会覆盖，不在会创建
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool HashSetEXT(this IDatabase db, Dictionary<string, dynamic> dic, string key)
        {
            var count = dic.Count;
            db.HashSet(key, dic.Select(pair => new HashEntry(pair.Key, pair.Value)).ToArray());
            return true;
        }

        public static TModel HashGetModelEXT<TModel>(this IDatabase db, string key, string hashField,
            CommandFlags flags = CommandFlags.None) where TModel : class
        {
            return db.HashGet(key, hashField, flags).ToModel<TModel>();
        }


        public static string[] HashGetMutiEXT(this IDatabase db, string key, params string[] fieldList)
        {
            var redisValues = db.HashGet(key, fieldList.Select(one => (RedisValue) one).ToArray())
                .Select(one => one.ToStringEXT()).ToArray();
            return redisValues;
        }

        public static void HashSetModelsEXT<TModel>(this IDatabase db, string key, Dictionary<string, TModel> dicModels,
            CommandFlags flags = CommandFlags.None) where TModel : class, new()
        {
            var hashEntries =
                dicModels.Select(pair => new HashEntry(pair.Key, pair.Value.ToJson())).ToList();

            db.HashSet(key, hashEntries.ToArray(), flags);
        }

        public static List<TModel> HashGetModelsEXT<TModel>(this IDatabase db, string key) where TModel : class
        {
            var hashEntries = db.HashGetAll(key);

            var tModels =
                hashEntries.Select(hashEntry => hashEntry.Value)
                    .Select(redisValue => redisValue.ToModel<TModel>())
                    .ToList();
            return tModels;
        }
    }
}