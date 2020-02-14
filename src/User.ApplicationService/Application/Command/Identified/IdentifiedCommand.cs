// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using User.Domain.AggregatesModel.Enumeration;

namespace User.ApplicationService.Application.Command.Identified
{
    public abstract class IdentifiedCommand<T, R> : IRequest<R> where T : IRequest<R>
    {
        #region Property

        /// <summary>
        ///
        /// </summary>
        public T Command { get; }

        /// <summary>
        /// 唯一键
        /// </summary>
        public string Id { get; }

        public IdentityType IdentityType { get; }

        #endregion

        #region Ctor

        /// <summary>
        ///  唯一表
        /// </summary>
        /// <param name="command"></param>
        /// <param name="id"></param>
        /// <param name="identityType"></param>
        public IdentifiedCommand(T command, string id, IdentityType identityType)
        {
            Command = command;
            Id = id;
            IdentityType = identityType;
        }

        #endregion
    }
}
