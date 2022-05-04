using System.IO;
using EmptySoul.AdultTwitch.Core.Authorize;
using EmptySoul.AdultTwitch.Core.ConstantData;
using Newtonsoft.Json;
using UnityEngine;

namespace EmptySoul.AdultTwitch.Core.UserData
{
    public class UsersBroker : IContextable
    {
        private GameContext _context;
        
        public UsersBroker(GameContext context)
        {
            _context = context;
        }
        
        public void CreateNewUser(string userName)
        {
            Directory.CreateDirectory(ConstantFromSave.UserSaveDirectoryPath(userName));
            
            if (_context.Get<DataManager>() is DataManager manager) 
                manager.InitUser(new User(userName));
        }

        public void LoadOldUser(string userName)
        {
            if(Directory.Exists(ConstantFromSave.UserSaveDirectoryPath(userName)))
            {
                var saveData = WorkWithBinary.GetBinaryData(ConstantFromSave.UserParamsSavePath(userName));
                
                if (_context.Get<DataManager>() is DataManager manager)
                    if (saveData is not null)
                        manager.InitUser(new User(JsonConvert.DeserializeObject<UserParams>(saveData)));
            }
        }
    }
}