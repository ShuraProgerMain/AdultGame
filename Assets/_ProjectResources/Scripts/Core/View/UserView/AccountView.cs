using EmptySoul.AdultTwitch.Core.UserData;
using EmptySoul.AdultTwitch.Utils;
using TMPro;
using UnityEngine;

namespace EmptySoul.AdultTwitch.Core.View.UserView
{
    public class AccountView : MonoBehaviour, IView
    {
        [SerializeField] private GameObject buttonLogin;
        [SerializeField] private GameObject dataContainer;
        [SerializeField] private TextMeshProUGUI nameField;
        [SerializeField] private TextMeshProUGUI coinsField;

        private User _user;
        private void Start()
        {
            ViewBroker.Add(this);
        }

        public void SetUser(ref User user)
        {
            _user = user;
            nameField.text = user.Params.Name;
            coinsField.text = DigitHandler.FormatValue(user.Params.CoinsAmount, false);
            buttonLogin.SetActive(false);
            dataContainer.SetActive(true);
        }

        public void UpdateCoins()
        {
            coinsField.text = DigitHandler.FormatValue(_user.Params.CoinsAmount, false);
        }
    }
}