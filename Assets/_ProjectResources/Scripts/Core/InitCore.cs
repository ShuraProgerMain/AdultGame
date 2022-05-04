using EmptySoul.AdultTwitch.Core.Authorize;
using EmptySoul.AdultTwitch.Core.UserData;
using EmptySoul.AdultTwitch.Mining;

namespace EmptySoul.AdultTwitch.Core
{
    public class InitCore
    {
        private readonly GameContext _context;
        public GameContext Context => _context;
        
        public InitCore()
        {
            _context = new GameContext();
            
            _context.Add(new AuthorizeProvider(_context));
            _context.Add(new UsersBroker(_context));
        }
    }
}