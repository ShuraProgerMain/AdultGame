using System;
using System.Threading.Tasks;
using EmptySoul.AdultTwitch.Core.GlobalEvents;
using EmptySoul.AdultTwitch.Core.UISystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EmptySoul.AdultTwitch.Utils
{
    public class WarningView : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;

        private async void OnEnable()
        {
            await Appear();
            await Task.Delay(2000);
            await Disappear();
        }

        private async Task Appear()
        {
            await GraduateHelper.GraduateAsync(Progress, .35f);
        }
        
        private async Task Disappear()
        {
            await GraduateHelper.GraduateAsync(Progress, 1f, true);
            var e = Events.UIMiddleLayer;
            e.Value = EMiddleLayerViews.None;
            EventsHandler.Broadcast(e);
        }

        private void Progress(float progress)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, progress);
            text.color = new Color(text.color.r, text.color.g, text.color.b, progress);
        }
    }
}
