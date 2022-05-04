using System;
using System.Threading.Tasks;
using EmptySoul.AdultTwitch.Core.GlobalEvents;
using EmptySoul.AdultTwitch.Core.UISystem;
using UnityEditor.UIElements;
using UnityEngine;

namespace EmptySoul.AdultTwitch.Mining
{
    public class WorkerController : MonoBehaviour
    {
        [SerializeField] private WorkerView view;
        private WorkerData _data;
        [SerializeField] private WorkerParams _workerParams;

        private DateTime _startTime;
        private DateTime _endTime;

        private bool IsEnable;
        
        public WorkerParams WorkerParams => _workerParams;

        private void Start()
        {
            IsEnable = true;
            Mining();
        }

        private void OnDisable()
        {
            IsEnable = false;
        }

        public void Initialize(WorkerParams worker, WorkerData data)
        {
            _workerParams = worker;
            _data = data;
            var waitTime = new TimeSpan(_data.workTime.hours, _data.workTime.minutes, _data.workTime.seconds);
            
            if (worker.lastTime > DateTime.MinValue)
            {
                var dif = DateTime.UtcNow - worker.lastTime;
                Debug.Log($"Diff time {dif.TotalSeconds} and result {(dif.TotalSeconds / waitTime.TotalSeconds) * worker.count}");
                SendProfit((dif.TotalSeconds / waitTime.TotalSeconds) * worker.count);
            }
        }

        public void PurchaseWorker()
        {
            var e = Events.CurrencyOperation;
            e.Operation = ECurrencyOperation.Remove;
            e.Amount = _data.startPrice * _data.heightValue * _workerParams.count;
            e.Collback = Purchase;
            EventsHandler.Broadcast(e);
        }

        private void Purchase(bool result)
        {
            if (result)
            {
                _workerParams.count++;
                view.UpdateState(_workerParams, _data);
            }
            else
            {
                var e = Events.UIMiddleLayer;
                e.Value = EMiddleLayerViews.Warning;
                EventsHandler.Broadcast(e);
            }
        }

        private async void Mining()
        {
            view.Initialize(_workerParams, _data);

            while (IsEnable)
            {
                await Process();
                SendProfit(_data.profit * _workerParams.count);
            }
        }

        private void SendProfit(double profit)
        {
            var e = Events.CurrencyOperation;
            e.Operation = ECurrencyOperation.Add;
            e.Amount = profit;
            e.Collback = b => { };
            EventsHandler.Broadcast(e);
        }

        private async Task Process()
        {
            var waitTime = new TimeSpan(_data.workTime.hours, _data.workTime.minutes, _data.workTime.seconds);
            _startTime = DateTime.UtcNow;
            _endTime = _startTime + waitTime;
            
            while (_endTime > DateTime.UtcNow && IsEnable)
            {
                view.SetTimer((_endTime - DateTime.UtcNow));
                view.SetProgress(Mathf.InverseLerp((float)_startTime.TimeOfDay.TotalSeconds,
                    (float)_endTime.TimeOfDay.TotalSeconds, (float)DateTime.UtcNow.TimeOfDay.TotalSeconds));
                await Task.Yield();
            }
        }
    }
}