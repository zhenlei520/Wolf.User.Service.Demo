// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Linq;
using AutoMapper;

namespace User.Grpc.Service.Extension.AutoConversionProfile
{
    /// <summary>
    /// 
    /// </summary>
    public class AutoMapperConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        public static void Configure()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes().Where(y => y.GetInterfaces().Contains(typeof(IProfile)))).ToList();
            var config = new MapperConfiguration(cfg => { cfg.AddMaps(types); });
        }
    }
}