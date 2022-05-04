using UnityEngine;

namespace EmptySoul.AdultTwitch.Core.UISystem
{
    public abstract class UILayerButton<T> : MonoBehaviour
    {
        [SerializeField] protected T state;

        public abstract void OpenWindow();
    }
}