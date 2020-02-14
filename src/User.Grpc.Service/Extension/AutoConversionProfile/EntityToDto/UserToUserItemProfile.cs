// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using AutoMapper;
using User.Grpc.Service.Dto;

namespace User.Grpc.Service.Extension.AutoConversionProfile.EntityToDto
{
    /// <summary>
    /// 
    /// </summary>
    public class UserToUserItemProfile : Profile, IProfile
    {
        /// <summary>
        /// 
        /// </summary>
        public UserToUserItemProfile()
        {
            CreateMap<Domain.AggregatesModel.UserAggregate.Users, UserItemDto>()
                .ForMember(x => x.BirthdayStr,
                    opt => opt.Ignore())
                .ForMember(x => x.GenderStr,
                    opt => opt.Ignore());
        }
    }
}