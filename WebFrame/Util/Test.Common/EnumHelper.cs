using System;
using System.Linq;

namespace Test.Util.Common
{
    public static class EnumHelper
    {
        /// <summary>
        ///     返回枚举项的描述信息。
        /// </summary>
        /// <param name="value">要获取描述信息的枚举项。</param>
        /// <returns>枚举想的描述信息。</returns>
        public static string GetEnumDescription(this Enum value)
        {
            try
            {
                var enumType = value.GetType();
                // 获取枚举常数名称。
                var name = Enum.GetName(enumType, value);
                if (name != null)
                {
                    // 获取枚举字段。
                    var fieldInfo = enumType.GetField(name);
                    if (fieldInfo != null)
                    {
                        // 获取描述的属性。
                        return fieldInfo.CustomAttributes.First().ConstructorArguments.First().Value.ToString();
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}