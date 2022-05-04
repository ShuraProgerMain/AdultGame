using System;
using System.Collections.Generic;
using EmptySoul.AdultTwitch.Core.Authorize;

namespace EmptySoul.AdultTwitch.Core
{
    public sealed class GameContext
    {
        private readonly Dictionary<Type, IContextable> _contextables = new();

        public void Add(IContextable contextable)
        {
            if(_contextables.ContainsKey(contextable.GetType())) return;
            
            _contextables.Add(contextable.GetType(), contextable);
        }

        public IContextable Get<T>()
        {
            if (_contextables.TryGetValue(typeof(T), out var value))
            {
                return value;
            }

            return null;
        }
    }
}