using DG.Tweening;
using EmptySoul.AdultTwitch.Core;
using EmptySoul.AdultTwitch.Core.Controllers;
using EmptySoul.AdultTwitch.Core.GlobalEvents;
using EmptySoul.AdultTwitch.Core.UISystem;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Task = System.Threading.Tasks.Task;

namespace EmptySoul.AdultTwitch.Gallery.VideoPlayerHandler
{
    public class VideoPlayerView : MonoBehaviour
    {
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private RawImage image;
        [SerializeField] private Image background;
        [SerializeField] private GameObject exit;
        [SerializeField] private RenderTexture texture;

        private VideoPlayerParameters _parameters;

        private void OnEnable()
        {
            var controller = ControllersBroker.Get<VideoPlayerController>() as VideoPlayerController;
            _parameters = controller.Parameters;
            Appear();
        }

        public async void CloseVideo()
        {
            await Disappear();
            var e = Events.UIMiddleLayer;
            e.Value = EMiddleLayerViews.None;
            EventsHandler.Broadcast(e);
        }

        private async void Appear()
        {
            PreparePlayer();
            videoPlayer.Pause();
            background.DOFade(.8f, 1f);
            image.rectTransform.DOMove(new Vector2(Screen.width / 2, Screen.height / 2), 1);
            await image.rectTransform.DOScale(Vector3.one, 1f).AsyncWaitForCompletion();
            exit.SetActive(true);
            videoPlayer.Play();
        }

        private async Task Disappear()
        {
            exit.SetActive(false);
            videoPlayer.Pause();
            background.DOFade(0f, 1f);
            image.rectTransform.DOMove(_parameters.StartPosition, 1);
            await image.rectTransform.DOScale(Vector3.one * .2f, 1f).AsyncWaitForCompletion();
            
            RenderTexture rt = RenderTexture.active;
            RenderTexture.active = texture;
            GL.Clear(true, true, new Color(1f, 1f, 1f, 0f));
            RenderTexture.active = rt;
        }


        private void PreparePlayer()
        {
            videoPlayer.clip = _parameters.Clip;
            image.rectTransform.DOMove(_parameters.StartPosition, 0);
            image.rectTransform.DOScale(Vector3.one * .2f, 0);
        }
    }
}