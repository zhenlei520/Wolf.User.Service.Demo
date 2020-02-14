// Copyright (c) zhenlei520 All rights reserved.

using System;
using System.Linq;

namespace User.ApplicationService.Infrastructure
{
    public static class GenericTypeExtensions
    {
        #region 获取泛型类名
        
        /// <summary>
        ///  获取泛型类名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetGenericTypeName(this Type type)
        {
            var typeName = string.Empty;

            if (type.IsGenericType)
            {
                var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
                typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
            }
            else
            {
                typeName = type.Name;
            }

            return typeName;
        }

        public static string GetGenericTypeName(this object @object)
        {
            return @object.GetType().GetGenericTypeName();

        }
        #endregion
    }
}
