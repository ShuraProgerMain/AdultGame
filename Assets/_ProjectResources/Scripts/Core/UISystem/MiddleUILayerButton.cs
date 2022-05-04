using EmptySoul.AdultTwitch.Core.GlobalEvents;

namespace EmptySoul.AdultTwitch.Core.UISystem
{
    public class MiddleUILayerButton : UILayerButton<EMiddleLayerViews>
    {
        public override void OpenWindow()
        {
            var e = Events.UIMiddleLayer;
            e.Value = state;
            EventsHandler.Broadcast(e);
        }
    }
}