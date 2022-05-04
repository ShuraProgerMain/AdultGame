using System;
using EmptySoul.AdultTwitch.Core.UISystem;
using EmptySoul.AdultTwitch.Gallery.VideoPlayerHandler;

namespace EmptySoul.AdultTwitch.Core.GlobalEvents
{
    public static class Events
    {
        public static readonly Authorize Authorize = new();
        public static readonly UIEvent<ELowLayerViews> UILowLayer = new();
        public static readonly UIEvent<EMiddleLayerViews> UIMiddleLayer = new();
        public static readonly CurrencyOperation CurrencyOperation = new();
        public static readonly Authorized Authorized = new();
        public static readonly PlayVideo PlayVideo = new();
    }

    public class CurrencyOperation : GameEvent
    {
        public ECurrencyOperation Operation { get; set; }
        public double Amount { get; set; }
        public Action<bool> Collback { get; set; }
    }

    public class Authorize : GameEvent
    {
        public string UserName { get; set; }
    }

    public class Authorized : GameEvent
    {
    }

    public class UIEvent<T> : GameEvent
    {
        public T Value { get; set; }
    }

    public class PlayVideo : GameEvent
    {
        public VideoPlayerParameters Parameters { get; set; }
    }

    public enum ECurrencyOperation
    {
        Add = 0,
        Remove = 1,
        Check = 2
    }
}