using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

    private RectTransform rectTransform;
    private Image foreground;
    public float progress = 0.0f;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        foreground = transform.Find("Foreground").GetComponent<Image>();

        Vector2 size = foreground.GetComponent<RectTransform>().sizeDelta;
        size.x = 0;
        foreground.GetComponent<RectTransform>().sizeDelta = size;
    }

    private void Update()
    {
        progress = Mathf.Clamp01(progress);
        Vector2 size = foreground.GetComponent<RectTransform>().sizeDelta;
        //size.x = Mathf.MoveTowards(size.x, progress * rectTransform.rect.width, 1);
        size.x = progress * rectTransform.rect.width;
        foreground.GetComponent<RectTransform>().sizeDelta = size;
    }

}
