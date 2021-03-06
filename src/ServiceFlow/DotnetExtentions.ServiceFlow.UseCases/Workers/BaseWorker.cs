﻿using System;

namespace DotnetExtentions.ServiceFlow.UseCases.Workers
{
    public abstract class BaseWorker
    {
        private readonly string _instanceKey = Guid.NewGuid().ToString("N");

        public override string ToString()
        {
            return $"{this.GetType().Name}: {_instanceKey}";
        }
    }
}
