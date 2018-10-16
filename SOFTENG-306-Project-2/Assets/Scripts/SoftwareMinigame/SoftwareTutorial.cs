using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SoftwareTutorial : MonoBehaviour
{
    private int slideIndex = 0;
    private GameObject currentSlide;

    public Canvas InstructionCanvas;

    private Transform slides;

    public Text numberIndicator;
    // Use this for initialization
    void Start()
    {
        if (InstructionCanvas == null)
        {
            InstructionCanvas = GameObject.Find("InstructionCanvas").GetComponent<Canvas>();
        }
        Open();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Open()
    {
        slides = transform.GetChild(0);
        slideIndex = 0;
        gameObject.SetActive(true);
        foreach (Transform child in slides)
        {
            child.gameObject.SetActive(false);
        }
        InstructionCanvas.gameObject.SetActive(false);
        SetSlide();
    }

    public void Close()
    {
        InstructionCanvas.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void PrevSlide()
    {
        slideIndex--;
        SetSlide();
    }

    public void NextSlide()
    {
        slideIndex++;
        SetSlide();
    }

    private void SetSlide()
    {
        if (slideIndex < 0) slideIndex = 0;

        if (currentSlide != null)
        {
            currentSlide.SetActive(false);
        }

        if (slideIndex >= slides.childCount)
        {
            Close();
            return;
        }

        currentSlide = slides.GetChild(slideIndex).gameObject;
        currentSlide.SetActive(true);

        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < slides.childCount; i++)
        {
            if (i == slideIndex)
            {
                stringBuilder.Append("<color=\"yellow\">•</color> ");
            }
            else
            {
                stringBuilder.Append("• ");
            }
        }

        numberIndicator.text = stringBuilder.ToString();
    }
}