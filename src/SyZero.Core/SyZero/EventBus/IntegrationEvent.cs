﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SyZero.EventBus
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        public IntegrationEvent(IntegrationEvent integrationEvent)
        {
            Id = integrationEvent.Id;
            CreationDate = integrationEvent.CreationDate;
        }

        public Guid Id { get; }
        public DateTime CreationDate { get; }
    }
}
