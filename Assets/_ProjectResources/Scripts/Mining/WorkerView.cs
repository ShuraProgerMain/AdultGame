using System;
using EmptySoul.AdultTwitch.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EmptySoul.AdultTwitch.Mining
{
    public class WorkerView : MonoBehaviour
    {

        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI count;
        [SerializeField] private TextMeshProUGUI price;
        [SerializeField] private TextMeshProUGUI profit;
        [SerializeField] private TextMeshProUGUI timer;
        [SerializeField] private Slider progressBar;

        public void Initialize(WorkerParams workerParams, WorkerData data)
        {
            TextUpdate(workerParams, data);
        }
        
        public void UpdateState(WorkerParams workerParams, WorkerData data)
        {
            count.text = DigitHandler.FormatValue(workerParams.count, false);
            profit.text = (workerParams.count * data.profit) >= 1 ? DigitHandler.FormatValue(workerParams.count * data.profit) : (workerParams.count * data.profit).ToString();
            price.text = DigitHandler.FormatValue((data.startPrice * data.heightValue * workerParams.count), false);
        }

        public void SetProgress(float progress)
        {
            progressBar.value = progress;
        }

        public void SetTimer(TimeSpan time)
        {
            timer.text = $"{time.Hours}:{time.Minutes}:{time.Seconds}";
        }
        
        private void TextUpdate(WorkerParams workerParams, WorkerData data)
        {
            icon.sprite = data.icon;
            title.text = data.title;
            UpdateState(workerParams, data);
        }
    }

    [Serializable]
    public class WorkerParams
    {
        public DateTime lastTime = DateTime.MinValue;
        public int count;
    }
}
