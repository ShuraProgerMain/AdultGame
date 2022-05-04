using System;
using UnityEngine;

namespace EmptySoul.AdultTwitch.Mining
{
    [CreateAssetMenu (fileName = "Worker", menuName = "TwitchAdult/Worker")]
    public class WorkerData : ScriptableObject
    {
        public string title;
        public double profit;
        public WorkTime workTime;
        public double startPrice;
        public float heightValue;
    }

    [Serializable]
    public struct WorkTime
    {
        public int hours;
        public int minutes;
        public int seconds;
    }
}
