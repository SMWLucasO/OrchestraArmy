using System;
using System.Collections.Generic;
using OrchestraArmy.Event.Events;

namespace OrchestraArmy.Event
{
    public static class EventManager
    {
        // List of dynamics to save the pain of generic lists, guaranteed to be IListener because the only way to add is through bind
        private static Dictionary<Type, IList<dynamic>> _listeners = new Dictionary<Type, IList<dynamic>>();

        public static void Invoke<T>(T invokedEvent) where T: IEvent
        {
            if (!_listeners.ContainsKey(invokedEvent.GetType()))
            {
                // Nothing is bound to the event, just return
                return;
            }
            
            var listeners = new dynamic[_listeners[invokedEvent.GetType()].Count];
            _listeners[invokedEvent.GetType()].CopyTo(listeners, 0);

            foreach (var listener in listeners)
            {
                if (listener is IListener<T> l)
                    l.OnEvent(invokedEvent);
            }
        }
        
        public static void Bind<T>(IListener<T> listener) where T: IEvent
        {
            if (!_listeners.ContainsKey(typeof(T)))
                _listeners.Add(typeof(T), new List<dynamic>());
            
            _listeners[typeof(T)].Add(listener);
        }
        
        public static void Unbind<T>(IListener<T> listener) where T: IEvent
        {
            _listeners[typeof(T)].Remove(listener);
        }
    }
}