using System;
using System.Collections.Generic;
using EmptySoul.AdultTwitch.Core.View;

namespace EmptySoul.AdultTwitch.Core.Controllers
{
    public static class ControllersBroker
    {
        public static GameContext Context { get; set; }
        private static readonly Dictionary<Type, IController> _controllers = new();

        public static void Add(IController contextable)
        {
            if(_controllers.ContainsKey(contextable.GetType())) return;
            
            _controllers.Add(contextable.GetType(), contextable);
        }

        public static IController Get<T>()
        {
            if (_controllers.TryGetValue(typeof(T), out var value))
            {
                return value;
            }

            return null;
        }

        public static void Remove<T>() where T: IController
        {
            if (_controllers.TryGetValue(typeof(T), out var value))
            {
                _controllers.Remove(typeof(T));
            }
        }
    }
}