using System;
using EmptySoul.AdultTwitch.Core.ConstantData;
using EmptySoul.AdultTwitch.Core.GlobalEvents;
using EmptySoul.AdultTwitch.Core.View;
using EmptySoul.AdultTwitch.Core.View.UserView;
using Newtonsoft.Json;

namespace EmptySoul.AdultTwitch.Core.UserData
{
    public sealed class User
    {
        private readonly UserParams _params;
        public UserParams Params => _params; 

        public User(string name)
        {
            EventsHandler.AddListener<CurrencyOperation>(OnCurrencyOperation);
            
            _params = new UserParams
            {
                Name = name,
                CoinsAmount = name.Contains("ShuraProger") ? 1_000_000 : 1000
            };
            
            WorkWithBinary.SaveToBinary(ConstantFromSave.UserParamsSavePath(_params.Name), JsonConvert.SerializeObject(_params));
        }

        public User(UserParams param)
        {
            EventsHandler.AddListener<CurrencyOperation>(OnCurrencyOperation);
            _params = param;
        }

        private void OnCurrencyOperation(CurrencyOperation evt)
        {
            switch (evt.Operation)
            {
                case ECurrencyOperation.Add:
                    _params.CoinsAmount += evt.Amount;
                    evt.Collback?.Invoke(true);
                    break;
                case ECurrencyOperation.Remove:
                    evt.Collback?.Invoke(RemoveCurrency(evt.Amount));
                    break;
                case ECurrencyOperation.Check:
                    evt.Collback?.Invoke(_params.CoinsAmount >= evt.Amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            WorkWithBinary.SaveToBinary(ConstantFromSave.UserParamsSavePath(_params.Name), JsonConvert.SerializeObject(_params));
            if(ViewBroker.Get<AccountView>() is AccountView view)
                view.UpdateCoins();

        }

        private bool RemoveCurrency(double count)
        {
            if (count > _params.CoinsAmount) return false;

            _params.CoinsAmount -= count;
            return true;
        }
    }

    public class UserParams
    {
        public string Name = "None";
        public double CoinsAmount = 1000;
    }
}