// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using AutoMapper;

namespace User.Grpc.Service.Extension.AutoConversionProfile.EntityToRpc
{
    /// <summary>
    /// 用户转注册用户响应信息
    /// </summary>
    public class UsersToRegisterUserReplyProfile : Profile, IProfile
    {
        /// <summary>
        /// 
        /// </summary>
        public UsersToRegisterUserReplyProfile()
        {
            CreateMap<Domain.AggregatesModel.UserAggregate.Users, RegisterUserReply>()
                .ForMember(x => x.Birthday,
                    opt => opt.MapFrom(x =>
                        x.BirthDay == null ? "" : x.BirthDay.Value.ToString("yyyy-MM-dd HH:mm:ss")));
        }
    }
}