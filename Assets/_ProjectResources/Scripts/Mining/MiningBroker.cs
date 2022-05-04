
using System.Collections.Generic;
using System.Threading.Tasks;
using EmptySoul.AdultTwitch.Core.Controllers;
using EmptySoul.AdultTwitch.Core.GlobalEvents;
using EmptySoul.AdultTwitch.Core.UISystem;
using EmptySoul.AdultTwitch.Utils;
using UnityEngine;

namespace EmptySoul.AdultTwitch.Mining
{
    public class MiningBroker : MonoBehaviour
    {
        [SerializeField] private MiningView view;
        [SerializeField] private WorkerBroker workerBroker;
        [SerializeField] private Transform parent;
        
        private MiningController _controller;
        private readonly List<WorkerBroker> _activeWorkers = new();
        
        private async void Start()
        {
            _controller = ControllersBroker.Get<MiningController>() as MiningController;

            while (!_controller.IsReady)
            {
                await Task.Yield();
            }
            
            view.ShowWorkers();

            for (int i = 0; i < _controller.ActiveWorkers.Count; i++)
            {
                InitWorker(_controller.ActiveWorkers[i]);
            }
            
            InitNextWorkerPurchase();
        }

        public void PurchaseNextWorker()
        {
            view.NextWorkerBuyButtonOff();
            var e = Events.CurrencyOperation;
            e.Operation = ECurrencyOperation.Remove;
            e.Amount = _controller.Dates[_controller.WorkersCount].startPrice;
            e.Collback = AddNextWorker;
            EventsHandler.Broadcast(e);
        }

        private void AddNextWorker(bool result)
        {
            if (result)
            {
                _controller.AddWorker(new WorkerParams(){count = 1}, _controller.Dates[_controller.WorkersCount], InitWorker);
            }
            else
            {
                var e = Events.UIMiddleLayer;
                e.Value = EMiddleLayerViews.Warning;
                EventsHandler.Broadcast(e);
            }
        }

        private void InitWorker(WorkerController workerController)
        {
            var worker = Instantiate(workerBroker, parent);
            worker.Initialize(workerController);
            _activeWorkers.Add(worker);
            InitNextWorkerPurchase();
        }
        
        private void InitNextWorkerPurchase()
        {
            if(_controller.Dates.Length <= _controller.WorkersCount) return;
            
            view.NextWorkerBuyButtonOn(_controller.Dates[_controller.WorkersCount].title, DigitHandler.FormatValue(_controller.Dates[_controller.WorkersCount].startPrice));
        }
    }
}
