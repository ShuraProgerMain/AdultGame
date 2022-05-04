using System;
using System.Collections.Generic;

namespace EmptySoul.AdultTwitch.Core.View
{
    public static class ViewBroker
    {
        public static GameContext Context { get; set; }
        private static Dictionary<Type, IView> _views = new();

        public static void Add(IView contextable)
        {
            if(_views.ContainsKey(contextable.GetType())) return;
            
            _views.Add(contextable.GetType(), contextable);
        }

        public static IView Get<T>()
        {
            if (_views.TryGetValue(typeof(T), out var value))
            {
                return value;
            }

            return null;
        }
        
        public static void Remove<T>() where T: IView
        {
            if (_views.TryGetValue(typeof(T), out var value))
            {
                _views.Remove(typeof(T));
            }
        }
    }
    
    public interface IView {}
}