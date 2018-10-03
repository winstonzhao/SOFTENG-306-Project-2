using UnityEngine;

public class SoftwareEndScreen : MonoBehaviour
{
    public Canvas InstructionCanvas;

    private Transform slides;
    
    // Use this for initialization
    void Start()
    {
        slides = transform.Find("ExitSlide");
        Open();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Open()
    {
        gameObject.SetActive(true);
        InstructionCanvas.gameObject.SetActive(false);
    }

    public void Close()
    {
        InstructionCanvas.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    // Used this method to link back to lobby
    public void EndMiniGame()
    {
       
    }
}