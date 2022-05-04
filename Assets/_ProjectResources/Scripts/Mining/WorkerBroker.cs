using EmptySoul.AdultTwitch.Core.GlobalEvents;
using EmptySoul.AdultTwitch.Core.UISystem;
using UnityEngine;

namespace EmptySoul.AdultTwitch.Mining
{
    public class WorkerBroker : MonoBehaviour
    {
        [SerializeField] private WorkerView view;
        private WorkerController _controller;
        
        public void Initialize(WorkerController controller)
        {
            _controller = controller;
            _controller.OnProgressTick = view.SetProgress;
            _controller.OnTimeTick = view.SetTimer;
            view.Initialize(_controller.WorkerParams, _controller.WorkerData);
        }
        
        public void PurchaseWorker()
        {
            var e = Events.CurrencyOperation;
            e.Operation = ECurrencyOperation.Remove;
            e.Amount = _controller.WorkerData.startPrice * _controller.WorkerData.heightValue * _controller.WorkerParams.count;
            e.Collback = Purchase;
            EventsHandler.Broadcast(e);
        }
        
        private void Purchase(bool result)
        {
            if (result)
            {
                _controller.WorkerParams.count++;
                view.UpdateState(_controller.WorkerParams, _controller.WorkerData);
            }
            else
            {
                var e = Events.UIMiddleLayer;
                e.Value = EMiddleLayerViews.Warning;
                EventsHandler.Broadcast(e);
            }
        }
    }
}
