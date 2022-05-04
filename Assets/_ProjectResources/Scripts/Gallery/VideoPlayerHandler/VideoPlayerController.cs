using System;
using EmptySoul.AdultTwitch.Core.Authorize;
using EmptySoul.AdultTwitch.Core.Controllers;
using EmptySoul.AdultTwitch.Core.GlobalEvents;
using EmptySoul.AdultTwitch.Core.UISystem;
using UnityEngine;
using UnityEngine.Video;

namespace EmptySoul.AdultTwitch.Gallery.VideoPlayerHandler
{
    public class VideoPlayerController : MonoBehaviour, IController
    {
        public VideoPlayerParameters Parameters { get; private set; }

        private void OnEnable()
        {
            ControllersBroker.Add(this);
            EventsHandler.AddListener<PlayVideo>(OnPlayVideo);
        }

        private void OnDisable()
        {
            ControllersBroker.Remove<VideoPlayerController>();
            EventsHandler.RemoveListener<PlayVideo>(OnPlayVideo);
        }

        private void OnPlayVideo(PlayVideo evt)
        {
            Parameters = evt.Parameters;

            var e = Events.UIMiddleLayer;
            e.Value = EMiddleLayerViews.VideoPlayer;
            EventsHandler.Broadcast(e);
        }
    }

    public struct VideoPlayerParameters
    {
        public Vector2 StartPosition { get; set; }
        public Rect Rect { get; set; }
        public VideoClip Clip { get; set; }
    }
}
