using System.Collections;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using Ultimate_Isometric_Toolkit.Scripts.physics;
using Ultimate_Isometric_Toolkit.Scripts.Utils;
using UnityEngine;

namespace Ultimate_Isometric_Toolkit.Scripts.Examples
{
    public class RaycastTest : MonoBehaviour
    {
        public IsoTransform FlowerPrefab;

        // Update is called once per frame
        void Update()
        {
            //mouse ray in unity's coordinate system (used for debug purposes only)
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var color = Input.GetMouseButton(0) ? Color.red : Color.white;
            Debug.DrawRay(ray.origin, ray.direction * 1000, color);

            //mouse ray in isometric coordinate system 
            var isoRay = Isometric.MouseToIsoRay();

            //do an isometric raycast on left mouse click 
            if (Input.GetMouseButtonDown(0))
            {
                IsoRaycastHit isoRaycastHit;
                if (IsoPhysics.Raycast(isoRay, out isoRaycastHit))
                {
                    Debug.Log("we clicked on " + isoRaycastHit.Collider.name + " at " + isoRaycastHit.Point);
                    SpawnFlower(isoRaycastHit.Point);
                    StartCoroutine(Blink(isoRaycastHit.IsoTransform.GetComponent<SpriteRenderer>(), Color.red));
                }
            }
        }

        private void SpawnFlower(Vector3 position)
        {
            var copy = Instantiate(FlowerPrefab);
            copy.Position = position + new Vector3(0, copy.Size.y / 2, 0);
        }

        private IEnumerator Blink(SpriteRenderer renderer, Color color, float duration = 0.2f)
        {
            float timePassed = 0;
            renderer.color = Color.white;
            while (timePassed < duration)
            {
                timePassed += Time.deltaTime;

                renderer.color = Color.Lerp(Color.white, color, timePassed / duration);
                yield return null;
            }

            renderer.color = Color.white;
        }
    }
}