using EmptySoul.AdultTwitch.Core.GlobalEvents;
using UnityEngine;

namespace EmptySoul.AdultTwitch.Core.Controllers
{
    public class LeftPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject goToGalleryButton;

        private void OnEnable()
        {
            EventsHandler.AddListener<Authorized>(OnAuthorized);
        }

        private void OnDisable()
        {
            EventsHandler.RemoveListener<Authorized>(OnAuthorized);
        }

        private void OnAuthorized(Authorized evt)
        {
            goToGalleryButton.SetActive(true);
        }
    }
}