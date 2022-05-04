using System;
using EmptySoul.AdultTwitch.Core;
using EmptySoul.AdultTwitch.Core.GlobalEvents;
using EmptySoul.AdultTwitch.Utils;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;

namespace EmptySoul.AdultTwitch.Gallery
{
    public class ProductView : MonoBehaviour
    {
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private RawImage rawImage;
        [SerializeField] private TextMeshProUGUI price;
        [SerializeField] private Image substrate;
        [SerializeField] private Color offColorText;
        [SerializeField] private Color onColorText;
        [SerializeField] private Color offColorSubstrate;
        [SerializeField] private Color onColorSubstrate;
        [SerializeField] private GameObject fromPurchase;
        [SerializeField] private GameObject availableView;
        
        private ProductParams productParams;
        private bool _available;

        public Rect Rect => rawImage.rectTransform.rect;
        public RectTransform RectTransform => rawImage.rectTransform;
        
        public void Init(ProductParams newParams, bool available)
        {
            _available = available;
            productParams = newParams;

            CheckAvailable(SetView);
        }

        public void UpdatePricingView()
        {
            CheckAvailable((result => substrate.color = result ? onColorSubstrate : offColorSubstrate));
        }
        
        private void SetView(bool result)
        {
            videoPlayer.clip = productParams.clip;
            videoPlayer.targetTexture = productParams.texture;
            rawImage.texture = productParams.texture;
            substrate.color = result ? onColorSubstrate : offColorSubstrate;

            if (_available)
            {
                price.text = DigitHandler.FormatValue(productParams.price, false);
                price.color = result ? onColorText : offColorText;
            }
            else
            {
                OnAvailable();
            }
        }

        private void CheckAvailable(Action<bool> action)
        {
            if (_available)
            {
                var e = Events.CurrencyOperation;
                e.Operation = ECurrencyOperation.Check;
                e.Amount = productParams.price;
                e.Collback = action;
                EventsHandler.Broadcast(e);
            }
        }

        public void OnAvailable()
        {
            substrate.color = offColorSubstrate;
            fromPurchase.SetActive(false);
            availableView.SetActive(true);
        }
    }

    [System.Serializable]
    public struct ProductParams
    {
        public VideoClip clip;
        public RenderTexture texture;
        public long price;
    }
}
