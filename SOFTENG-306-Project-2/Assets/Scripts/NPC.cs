using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ultimate_Isometric_Toolkit.Scripts.Core;

public abstract class NPC : MonoBehaviour {

    public Sprite RHSSprite;
    private IsoTransform _playerPos;
    private IsoTransform _npcPos;
    private DialogCanvasManager _canvas;
    private Sprite _playerSprite;

	// Use this for initialization
	void Start () {
        _playerPos = GameObject.Find("Player").GetComponent<IsoTransform>();
        _npcPos = GetComponent<IsoTransform>();
        _canvas = GameObject.Find("DialogCanvas").GetComponent<DialogCanvasManager>();
        _playerSprite = GameObject.Find("Player").GetComponent<SaveSprite>().Sprite;
    }

    public abstract Dialog GetDialog();
	
	// Update is called once per frame
	void Update () {
        float x1 = _playerPos.Position.x;
        float x2 = _npcPos.Position.x;
        float y1 = _playerPos.Position.z;
        float y2 = _npcPos.Position.z;

        float x = Mathf.Abs(x1 - x2);
        float y = Mathf.Abs(y1 - y2);

        float distance = Mathf.Sqrt(x * x + y * y);

        if (Input.GetKeyDown("space") && distance < 1.5 && _canvas.Showing == false)
        {
            _canvas.ShowDialog(GetDialog(), _playerSprite, RHSSprite);
        }

    }
}
