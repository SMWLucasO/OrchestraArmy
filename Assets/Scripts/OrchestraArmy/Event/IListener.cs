using OrchestraArmy.Event.Events;

namespace OrchestraArmy.Event
{
    public interface IListener<in T> where T: IEvent
    {
        public void OnEvent(T invokedEvent);
    }
}