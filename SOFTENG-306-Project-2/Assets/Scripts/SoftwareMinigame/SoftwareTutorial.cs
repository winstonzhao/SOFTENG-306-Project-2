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

    void Start()
    {
        if (InstructionCanvas == null)
        {
            InstructionCanvas = GameObject.Find("InstructionCanvas").GetComponent<Canvas>();
        }
        Open();
    }

    /// <summary>
    /// Opens the tutorial screen, hiding the instructions behind it
    /// </summary>
    public void Open()
    {
        slides = transform.GetChild(0);
        slideIndex = 0;
        gameObject.SetActive(true);

        // Hide all slides initially
        foreach (Transform child in slides)
        {
            child.gameObject.SetActive(false);
        }

        InstructionCanvas.gameObject.SetActive(false);
        SetSlide();
    }

    /// <summary>
    /// Closes the tutorial screen, showing the instructions behind it
    /// </summary>
    public void Close()
    {
        InstructionCanvas.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Goes to the previous slide
    /// </summary>
    public void PrevSlide()
    {
        slideIndex--;
        SetSlide();
    }

    /// <summary>
    /// Goes to the next slide
    /// </summary>
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

        // Reached the end of the slides
        if (slideIndex >= slides.childCount)
        {
            Close();
            return;
        }

        currentSlide = slides.GetChild(slideIndex).gameObject;
        currentSlide.SetActive(true);

        // Update the current slide indicator
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