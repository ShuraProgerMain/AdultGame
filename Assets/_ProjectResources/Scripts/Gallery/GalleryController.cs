using System;
using System.Collections.Generic;
using EmptySoul.AdultTwitch.Core;
using EmptySoul.AdultTwitch.Core.ConstantData;
using EmptySoul.AdultTwitch.Core.Controllers;
using EmptySoul.AdultTwitch.Core.UserData;
using Newtonsoft.Json;
using UnityEngine;

namespace EmptySoul.AdultTwitch.Gallery
{
    public class GalleryController : MonoBehaviour, IController
    {
        [SerializeField] private ProductController prefabProduct;
        [SerializeField] private Transform productParent;
        [SerializeField] private ProductParams[] products;

        private readonly List<ProductController> _availableProducts = new();
        private List<bool> loadData = new();
        private void Start()
        {
            ControllersBroker.Add(this);
            Init();
        }

        private void OnDisable()
        {
            ControllersBroker.Remove<GalleryController>();
        }

        public void UpdateViewAllProducts()
        {
            foreach (var product in _availableProducts)
            {
                product.CheckPricing();
            }
        }

        public void Init()
        {
            Load();
            
            for (var index = 0; index < products.Length; index++)
            {
                var param = products[index];
                var product = Instantiate(prefabProduct, productParent);
                
                product.Init(param, loadData.Count <= 0 || loadData[index], Save);
                
                _availableProducts.Add(product);
            }
        }

        private void Load()
        {
            var dataManager = ControllersBroker.Context.Get<DataManager>() as DataManager;
            var data = WorkWithBinary.GetBinaryData(ConstantFromSave.GalleryProductsSavePath(dataManager.UserName));
            var value = JsonConvert.DeserializeObject<List<bool>>(data);
            
            if(value is not null) loadData.AddRange(value);

        }

        private void Save()
        {
            var listSaves = new List<bool>();
            foreach (var product in _availableProducts)
            {
                listSaves.Add(product.IsAvailableForPurchase);
            }

            var dataManager = ControllersBroker.Context.Get<DataManager>() as DataManager;
            WorkWithBinary.SaveToBinary(ConstantFromSave.GalleryProductsSavePath(dataManager.UserName), JsonConvert.SerializeObject(listSaves));
        }
    }
}
