using System;
using System.Collections.Generic;

namespace EmptySoul.AdultTwitch.Core.GlobalEvents
{
    public static class EventsHandler
    {
        private static Dictionary<Type, Action<GameEvent>> events = new Dictionary<Type, Action<GameEvent>>();
        private static Dictionary<Delegate, Action<GameEvent>> eventsLookups = new Dictionary<Delegate, Action<GameEvent>>();

        public static void AddListener<T>(Action<T> action) where T : GameEvent
        {
            if (!eventsLookups.ContainsKey(action))
            {
                Action<GameEvent> newAction = e => action((T) e);
                eventsLookups[action] = newAction;

                if (events.TryGetValue(typeof(T), out Action<GameEvent> internalAction))
                {
                    events[typeof(T)] = internalAction += newAction;
                }
                else
                {
                    events[typeof(T)] = newAction;
                }
            }
        }

        public static void RemoveListener<T>(Action<T> action) where T : GameEvent
        {
            if (eventsLookups.TryGetValue(action, out var value))
            {
                if (events.TryGetValue(typeof(T), out var tempAction))
                {
                    tempAction -= value;
                    if (tempAction == null)
                    {
                        events.Remove(typeof(T));
                    }
                    else
                    {
                        events[typeof(T)] = tempAction;
                    }
                }

                eventsLookups.Remove(action);
            }
        }

        public static void Broadcast(GameEvent action)
        {
            if (events.TryGetValue(action.GetType(), out var value))
            {
                value?.Invoke(action);
            }
        }
    }
}