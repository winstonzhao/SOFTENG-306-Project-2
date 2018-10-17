using UnityEngine;

namespace SoftwareMinigame
{
    public class SoftwareNotFinished : MonoBehaviour
    {
        public void Open()
        {
            GetComponent<Canvas>().gameObject.SetActive(true);
        }

        public void Close()
        {
            GetComponent<Canvas>().gameObject.SetActive(false);
        }

    }
}