using System;
using EmptySoul.AdultTwitch.Core.Controllers;
using EmptySoul.AdultTwitch.Core.GlobalEvents;
using EmptySoul.AdultTwitch.Core.UISystem;
using EmptySoul.AdultTwitch.Gallery.VideoPlayerHandler;
using UnityEngine;

namespace EmptySoul.AdultTwitch.Gallery
{
    public class ProductController : MonoBehaviour
    {
        [SerializeField] private ProductView productView;
        
        private ProductParams _productParams;
        private Action _saveAction;
        public bool IsAvailableForPurchase { get; set; } = true;

        public void Init(ProductParams newParams, bool availableForPurchase, Action saveAction)
        {
            IsAvailableForPurchase = availableForPurchase;
            _saveAction = saveAction;
            _productParams = newParams;
        
            productView.Init(newParams, IsAvailableForPurchase);
        }

        public void CheckPricing()
        {
            productView.UpdatePricingView();
        }

        public void OnClick()
        {
            if (IsAvailableForPurchase)
            {
                Purchase();
            }
            else
            {
                PlayVideo();
            }
        }

        private void Purchase()
        {
            var e = Events.CurrencyOperation;
            e.Operation = ECurrencyOperation.Remove;
            e.Amount = _productParams.price;
            e.Collback = OnPurchase;
            EventsHandler.Broadcast(e);
        }

        private void OnPurchase(bool result)
        {
            if (result)
            {
                IsAvailableForPurchase = false;
                productView.OnPurchased();
                PlayVideo();
                _saveAction?.Invoke();
                var contr = ControllersBroker.Get<GalleryController>() as GalleryController;
                contr.UpdateViewAllProducts();
            }
            else
            {
                var e = Events.UIMiddleLayer;
                e.Value = EMiddleLayerViews.Warning;
                EventsHandler.Broadcast(e);
            }
        }

        private void PlayVideo()
        {
            var e = Events.PlayVideo;
            e.Parameters = new VideoPlayerParameters()
            {
                Clip = _productParams.clip,
                Rect = productView.Rect,
                StartPosition = productView.transform.position
            };
            EventsHandler.Broadcast(e);
        }
    }
}
