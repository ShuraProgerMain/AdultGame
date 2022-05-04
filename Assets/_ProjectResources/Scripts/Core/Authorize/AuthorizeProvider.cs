using System.IO;
using EmptySoul.AdultTwitch.Core.GlobalEvents;
using EmptySoul.AdultTwitch.Core.UserData;

namespace EmptySoul.AdultTwitch.Core.Authorize
{
    public class AuthorizeProvider : IContextable
    {
        private readonly GameContext _context;
        
        public AuthorizeProvider(GameContext context)
        {
            _context = context;
            EventsHandler.AddListener<GlobalEvents.Authorize>(AccessVerification);
        }

        private void AccessVerification(GlobalEvents.Authorize evt)
        {
            if (Directory.Exists(ConstantData.ConstantFromSave.DirectoryPath))
            {
                if (Directory.Exists(ConstantData.ConstantFromSave.UserSaveDirectoryPath(evt.UserName)))
                {
                    Login(evt.UserName);
                    return;
                }
            }
            
            Register(evt.UserName);
        }

        private void Register(string name)
        {
            if (_context.Get<UsersBroker>() is UsersBroker broker) 
                broker.CreateNewUser(name);
        }

        private void Login(string name)
        {
            if (_context.Get<UsersBroker>() is UsersBroker broker) 
                broker.LoadOldUser(name);
        }
    }

    public interface ISaveObject
    {
        
    }

    public interface IContextable { }
}