using System;
using EmptySoul.AdultTwitch.Core.GlobalEvents;
using UnityEngine;
using UnityEngine.Serialization;

namespace EmptySoul.AdultTwitch.Core.UISystem
{
    public enum ELowLayerViews
    {
        None = 0,
        Gallery = 1,
        Mining = 2
    }

    public enum EMiddleLayerViews
    {
        None = 0,
        PupUp = 1,
        Warning = 2,
        VideoPlayer = 3
    }

    public class UILayer<T> : MonoBehaviour
    {
        [SerializeField] private T initState;
        [SerializeField] private WindowParametrs<T>[] _parametrs;
        [SerializeField] private Transform parent;
        private GameObject _activeWindow;
        [SerializeField] private T _currentState;

        private void OnEnable()
        {
            EventsHandler.AddListener<UIEvent<T>>(ShowWindow);
        }

        private void OnDisable()
        {
            EventsHandler.RemoveListener<UIEvent<T>>(ShowWindow);
        }

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            ShowWindow(initState);
        }

        private void ShowWindow(UIEvent<T> evt)
        {
            ShowWindow(evt.Value);
        }

        private void ShowWindow(T state)
        {
            if(state.Equals(_currentState)) return;

            var window = FindWindow(state);

            CloseWindow();
            
            if(window.window is null) return;
            
            _activeWindow = Instantiate(window.window, window.customParent == null ? parent : window.customParent);
            _currentState = window.state;
        }

        private void CloseWindow()
        {
            _currentState = default;
            Destroy(_activeWindow);
        }
        private WindowParametrs<T> FindWindow(T state)
        {
            foreach (var pair in _parametrs)
            {
                if (pair.state.Equals(state))
                {
                    return pair;
                }
            }

            return new WindowParametrs<T>();
        }
    }

    [System.Serializable]
    public struct WindowParametrs<T>
    {
        public T state;
        public GameObject window;
        public Transform customParent;
    }
}