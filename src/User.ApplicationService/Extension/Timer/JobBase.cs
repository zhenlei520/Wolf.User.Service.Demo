// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Hangfire;
using Hangfire.Storage;

namespace User.ApplicationService.Extension.Timer
{
    /// <summary>
    /// 
    /// </summary>
    public class JobBase : BaseServiceProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public static void StartJob()
        {
            if (User.Infrastructure.Core.ServiceProvider.GetAppConfig().RunTask)
            {
                BackgroundJob.Schedule(() => Console.WriteLine("Hello Hangfire"), TimeSpan.FromMinutes(1));
            }
            else
            {
                using (var connection = JobStorage.Current.GetConnection())
                {
                    foreach (var recurringJob in connection.GetRecurringJobs())
                    {
                        RecurringJob.RemoveIfExists(recurringJob.Id);
                    }
                }

            }
        }
    }
}