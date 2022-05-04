
using EmptySoul.AdultTwitch.Core.GlobalEvents;

namespace EmptySoul.AdultTwitch.Core.UISystem
{
    public class LowUILayerButton : UILayerButton<ELowLayerViews>
    {
        public override void OpenWindow()
        {
            var e = Events.UILowLayer;
            e.Value = state;
            EventsHandler.Broadcast(e);
        }
    }
}