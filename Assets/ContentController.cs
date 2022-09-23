using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentController : MonoBehaviour
{
    [SerializeField] RectTransform scrollContent;
    [SerializeField] float height;
    [SerializeField] float weight;
    public void UpdateSize(int length)
    {
        if (height > 0)
            scrollContent.sizeDelta = new Vector2(scrollContent.sizeDelta.x, height + height * length);
        else if (weight > 0)
            scrollContent.sizeDelta = new Vector2(weight + weight * length, scrollContent.sizeDelta.y);
    }

    public void UpdateSize(float length)
    {
        scrollContent.sizeDelta = new Vector2(scrollContent.sizeDelta.x, length);
    }

    public void Clear()
    {
        scrollContent.sizeDelta = new Vector2(scrollContent.sizeDelta.x, height); 
    }
}
