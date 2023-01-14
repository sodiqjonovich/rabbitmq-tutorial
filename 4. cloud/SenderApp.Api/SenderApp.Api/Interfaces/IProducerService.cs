using SenderApp.Api.Models;

namespace SenderApp.Api.Interfaces
{
    public interface IProducerService
    {
        public void Send(Message message);
    }
}
