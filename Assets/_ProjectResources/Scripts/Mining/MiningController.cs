using System;
using System.Collections.Generic;
using EmptySoul.AdultTwitch.Core;
using EmptySoul.AdultTwitch.Core.ConstantData;
using EmptySoul.AdultTwitch.Core.Controllers;
using EmptySoul.AdultTwitch.Core.GlobalEvents;
using EmptySoul.AdultTwitch.Core.UISystem;
using EmptySoul.AdultTwitch.Core.UserData;
using EmptySoul.AdultTwitch.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace EmptySoul.AdultTwitch.Mining
{
    public class MiningController : MonoBehaviour
    {
        [SerializeField] private MiningView view;
        [SerializeField] private WorkerController prefabController;
        [SerializeField] private WorkerData[] dates;
        [SerializeField] private Transform parent;

        [SerializeField] private List<WorkerController> _activeWorkers = new();

        private int _workersCount;
        private void OnEnable()
        {
            EventsHandler.AddListener<Authorized>(Initialize);
            
            var dataManager = ControllersBroker.Context.Get<DataManager>() as DataManager;
            if (dataManager.Authorized)
            {
                Load();
            }
        }

        private void OnDisable()
        {
            Save();
            EventsHandler.RemoveListener<Authorized>(Initialize);
        }

        private void OnDestroy()
        {
            Save();
        }

        private void Initialize(Authorized authorized)
        {
            Load();
        }

        private void Save()
        {
            var list = new List<WorkerParams>();

            foreach (var worker in _activeWorkers)
            {
                worker.WorkerParams.lastTime = DateTime.UtcNow;
                list.Add(worker.WorkerParams);
            }
            
            var dataManager = ControllersBroker.Context.Get<DataManager>() as DataManager;
            WorkWithBinary.SaveToBinary(ConstantFromSave.MiningProgressSavePath(dataManager.UserName), 
                JsonConvert.SerializeObject(list));
        }

        private void Load()
        {
            var dataManager = ControllersBroker.Context.Get<DataManager>() as DataManager;
            var data = WorkWithBinary.GetBinaryData(ConstantFromSave.MiningProgressSavePath(dataManager.UserName));

            if (string.IsNullOrEmpty(data))
            {
                _workersCount = 1;
                AddWorker(new WorkerParams(){count = 1}, dates[0]);
            }
            else
            {
                var list = JsonConvert.DeserializeObject<List<WorkerParams>>(data);

                for (int index = 0; index < list.Count; index++)
                {
                    AddWorker(list[index], dates[index]);
                }

                _workersCount = list.Count;
            }

            view.ShowWorkers();
            InitNextWorkerPurchase();
        }

        private void AddWorker(WorkerParams workerParams, WorkerData data)
        {
            var worker = Instantiate(prefabController, parent);
            worker.Initialize(workerParams, data);
            _activeWorkers.Add(worker);
        }

        public void PurchaseNextWorker()
        {
            var e = Events.CurrencyOperation;
            e.Operation = ECurrencyOperation.Remove;
            e.Amount = dates[_workersCount].startPrice;
            e.Collback = AddNextWorker;
            EventsHandler.Broadcast(e);
        }

        private void AddNextWorker(bool result)
        {
            if (result)
            {
                AddWorker(new WorkerParams(){count = 1}, dates[_workersCount]);
            }
            else
            {
                var e = Events.UIMiddleLayer;
                e.Value = EMiddleLayerViews.Warning;
                EventsHandler.Broadcast(e);
            }
            
            InitNextWorkerPurchase();
        }

        private void InitNextWorkerPurchase()
        {
            if(dates.Length <= _workersCount) return;
            
            view.Init(dates[_workersCount].title, DigitHandler.FormatValue(dates[_workersCount].startPrice));
        }
    }
}
