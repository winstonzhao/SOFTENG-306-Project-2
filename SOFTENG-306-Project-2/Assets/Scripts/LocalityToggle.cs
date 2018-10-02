using Ultimate_Isometric_Toolkit.Scripts.Core;
using UnityEngine;
using UnityEngine.UI;

public class LocalityToggle : MonoBehaviour
{
    public float Threshold;

    public IsoTransform Watch;

    public GameObject Toggle;

    private IsoTransform Transform;

    private void Start()
    {
        if (Watch == null)
        {
            Watch = GameObject.Find("Player").GetComponent<IsoTransform>();
        }

        Transform = GetComponent<IsoTransform>();
    }

    public void Update()
    {
        if (Watch == null || Toggle == null)
        {
            return;
        }

        var x1 = Transform.Position.x;
        var z1 = Transform.Position.z;
        var x2 = Watch.Position.x;
        var z2 = Watch.Position.z;

        var dx = Mathf.Abs(x2 - x1);
        var dz = Mathf.Abs(z2 - z1);

        var distance = Mathf.Sqrt(dx * dx + dz * dz);

        Toggle.SetActive(distance < Threshold);
    }
}
