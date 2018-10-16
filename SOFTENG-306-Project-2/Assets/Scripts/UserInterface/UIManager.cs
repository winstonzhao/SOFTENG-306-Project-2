using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager> {

    private GameObject ui;

    public void ShowUI()
    {
        var uiPrefab = Resources.Load<GameObject>("Prefabs/User Interface/Player Interface");
        this.ui = Instantiate(uiPrefab);
    }
    
    public void DestroyUI()
    {
        if (ui != null)
        {
            Destroy(this.ui);
        }   
    }

    public void ToggleUI()
    {
        if (ui)
        {
            DestroyUI();
        }
        else
        {
            ShowUI();
        }
    }
}
