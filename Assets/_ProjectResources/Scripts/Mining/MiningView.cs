using System;
using TMPro;
using UnityEngine;

namespace EmptySoul.AdultTwitch.Mining
{
    public class MiningView : MonoBehaviour
    {
        [SerializeField] private GameObject buyButton;
        [SerializeField] private GameObject signInPls;
        [SerializeField] private TextMeshProUGUI buyButtonTitle;
        [SerializeField] private TextMeshProUGUI buyButtonPrice;

        public void ShowWorkers()
        {
            signInPls.SetActive(false);
        }
        public void NextWorkerBuyButtonOn(string title, string price)
        {
            buyButton.transform.SetSiblingIndex(Int32.MaxValue);
            buyButtonTitle.text = title;
            buyButtonPrice.text = price;
            buyButton.SetActive(true);
        }

        public void NextWorkerBuyButtonOff()
        {
            buyButton.SetActive(false);
        }
    }
}
