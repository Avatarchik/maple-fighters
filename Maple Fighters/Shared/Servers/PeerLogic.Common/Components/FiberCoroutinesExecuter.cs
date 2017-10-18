﻿using System;
using ServerCommunicationInterfaces;
using CommonTools.Coroutines;

namespace PeerLogic.Common.Components
{
    public class FiberCoroutinesExecutor : ThreadSafeCoroutinesExecutorBase
    {
        private readonly IDisposable scheduler;

        public FiberCoroutinesExecutor(IScheduler fiber, int updateRateMs)
        {
            scheduler = fiber.ScheduleOnInterval(Update, 0, updateRateMs);
        }

        public override void Dispose()
        {
            scheduler.Dispose();

            base.Dispose();
        }
    }
}