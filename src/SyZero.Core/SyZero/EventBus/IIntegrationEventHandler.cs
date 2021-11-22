using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SyZero.EventBus
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
    {
        Task Handle(TIntegrationEvent @event);
    }

  


    public interface IIntegrationEventHandler
    {
    }
}
