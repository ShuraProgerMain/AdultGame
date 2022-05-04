using System;
using System.Threading.Tasks;
using UnityEngine;

namespace EmptySoul.AdultTwitch.Utils
{
    public static class GraduateHelper
    {
        public static async Task GraduateAsync(Action<float> action, float duration, bool reverse = false)
        {
            for (float time = 0f; time < duration; time += Time.deltaTime)
            {
                float ratio = time / duration;
                ratio = reverse ? 1f - ratio : ratio;

                float progress = ratio;

                action.Invoke(progress);

                await Task.Yield();
            }

            action.Invoke(reverse ? 0f : 1f);
        }
    }
}