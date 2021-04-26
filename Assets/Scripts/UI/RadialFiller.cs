using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RadialFiller : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    private Image image;
    
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        image.fillAmount = slider.value;
    }
}
