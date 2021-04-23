using OrchestraArmy.Event.Event;

namespace OrchestraArmy.Event
{
    public interface IListener<in T> where T: IEvent
    {
        public void OnEvent(T invokedEvent);
    }
}