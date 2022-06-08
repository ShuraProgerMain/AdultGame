using System;
using System.Threading.Tasks;
using EmptySoul.AdultTwitch.Core.Controllers;
using EmptySoul.AdultTwitch.Core.GlobalEvents;
using UnityEngine;

namespace EmptySoul.AdultTwitch.Mining
{
    public class WorkerController : IController
    {
        private WorkerData _data;
        private WorkerParams _workerParams;

        private DateTime _startTime;
        private DateTime _endTime;

        private bool IsEnable;

        public Action<TimeSpan> OnTimeTick;
        public Action<float> OnProgressTick;

        public WorkerParams WorkerParams => _workerParams;
        public WorkerData WorkerData => _data;

        // public WorkerController()
        // {
        //     IsEnable = true;
        //     Mining();
        // }
        
        // private void Start()
        // {
        //     IsEnable = true;
        //     Mining();
        // }
        //
        // private void OnDisable()
        // {
        //     IsEnable = false;
        // }

        public void Initialize(WorkerParams worker, WorkerData data)
        {
            _workerParams = worker;
            _data = data;
            var waitTime = new TimeSpan(_data.workTime.hours, _data.workTime.minutes, _data.workTime.seconds);
            
            if (worker.lastTime > DateTime.MinValue)
            {
                var dif = DateTime.UtcNow.TimeOfDay.TotalSeconds - worker.lastTime.TimeOfDay.TotalSeconds;
                var a = dif / waitTime.TotalSeconds;
                Debug.Log(a * worker.count);
                SendProfit(a * worker.count);
            }

            IsEnable = true;
            Mining();
        }

        // public void PurchaseWorker()
        // {
        //     var e = Events.CurrencyOperation;
        //     e.Operation = ECurrencyOperation.Remove;
        //     e.Amount = _data.startPrice * _data.heightValue * _workerParams.count;
        //     e.Collback = Purchase;
        //     EventsHandler.Broadcast(e);
        // }
        //
        // private void Purchase(bool result)
        // {
        //     if (result)
        //     {
        //         _workerParams.count++;
        //         view.UpdateState(_workerParams, _data);
        //     }
        //     else
        //     {
        //         var e = Events.UIMiddleLayer;
        //         e.Value = EMiddleLayerViews.Warning;
        //         EventsHandler.Broadcast(e);
        //     }
        // }

        private async void Mining()
        {
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
                OnTimeTick?.Invoke((_endTime - DateTime.UtcNow));
                OnProgressTick?.Invoke((Mathf.InverseLerp((float)_startTime.TimeOfDay.TotalSeconds,
                    (float)_endTime.TimeOfDay.TotalSeconds, (float)DateTime.UtcNow.TimeOfDay.TotalSeconds)));
                // view.SetTimer((_endTime - DateTime.UtcNow));
                // view.SetProgress(Mathf.InverseLerp((float)_startTime.TimeOfDay.TotalSeconds,
                //     (float)_endTime.TimeOfDay.TotalSeconds, (float)DateTime.UtcNow.TimeOfDay.TotalSeconds));
                await Task.Yield();
            }
        }
    }
}