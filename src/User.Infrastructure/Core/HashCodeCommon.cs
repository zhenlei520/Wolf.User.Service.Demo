// Copyright (c) zhenlei520 All rights reserved.

namespace User.Infrastructure.Core
{
    public static class HashCodeCommon
    {
        #region 重写HashCode方法

        /// <summary>
        /// 重写HashCode方法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetHashCode(this object str)
        {
            if (str is string)
            {
                return GetHashCode(str.ToString());
            }

            return GetHashCode(ServiceProvider.GetJsonProvider().Serializer(str));
        }

        /// <summary>
        /// 重写HashCode方法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetHashCode(this string str)
        {
            unchecked
            {
                int hash1 = (5381 << 16) + 5381;
                int hash2 = hash1;

                for (int i = 0; i < str.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }

        #endregion
    }
}