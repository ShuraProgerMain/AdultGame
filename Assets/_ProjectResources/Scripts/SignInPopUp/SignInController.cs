using EmptySoul.AdultTwitch.Core.GlobalEvents;
using EmptySoul.AdultTwitch.Core.UISystem;
using TMPro;
using UnityEngine;

namespace EmptySoul.AdultTwitch.SignInPopUp
{
    public class SignInController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField textInput;

        public void SignIn()
        {
            var value = textInput.text;
            var temp = value.Replace(",", string.Empty);
            if (string.IsNullOrEmpty(temp)) return;

            var e = Events.Authorize;
            e.UserName = value;
            EventsHandler.Broadcast(e);
            
            var ui = Events.UIMiddleLayer;
            ui.Value = EMiddleLayerViews.None;
            EventsHandler.Broadcast(ui);
        }
    }
}
