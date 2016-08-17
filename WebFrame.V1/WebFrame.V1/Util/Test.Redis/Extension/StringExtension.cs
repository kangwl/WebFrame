using System;
using StackExchange.Redis;

namespace Test.Util.Redis.Extension
{
    public static class StringExtension
    {
        public static bool StringSetModelEXT<TModel>(this IDatabase db, string key, TModel tModel,
            TimeSpan? expiry = default(TimeSpan?), When when = When.Always, CommandFlags flags = CommandFlags.None)
            where TModel : class, new()
        {
            var json = tModel.ToJson();
            return db.StringSet(key, json, expiry, when, flags);
        }

        public static TModel StringGetModelEXT<TModel>(this IDatabase db, string key,
            CommandFlags flags = CommandFlags.None) where TModel : class
        {
            var redisValue = db.StringGet(key, flags);

            return redisValue.ToModel<TModel>();
        }
    }
}