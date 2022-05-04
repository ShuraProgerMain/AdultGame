using EmptySoul.AdultTwitch.Core.Authorize;
using EmptySoul.AdultTwitch.Core.Controllers;
using EmptySoul.AdultTwitch.Core.GlobalEvents;
using EmptySoul.AdultTwitch.Core.UserData;
using EmptySoul.AdultTwitch.Core.View;
using EmptySoul.AdultTwitch.Core.View.UserView;
using UnityEngine;

namespace EmptySoul.AdultTwitch.Core
{
    public class DataManager : MonoBehaviour, IContextable
    {
        private User _activeUser;
        private InitCore _initCore;

        public string UserName => _activeUser.Params.Name;
        public bool Authorized { get; set; }
        
        private void Awake()
        {
            _initCore = new InitCore();
            _initCore.Context.Add(this);
            ViewBroker.Context = _initCore.Context;
            ControllersBroker.Context = _initCore.Context;
        }

        public void InitUser(User user)
        {
            _activeUser = user;
            Debug.Log($"{_activeUser.Params.Name} with {_activeUser.Params.CoinsAmount} coins");

            if(ViewBroker.Get<AccountView>() is AccountView account)
            {
                account.SetUser(ref _activeUser);
            }
            
            Authorized = true;
            EventsHandler.Broadcast(Events.Authorized);
        }
    }
}