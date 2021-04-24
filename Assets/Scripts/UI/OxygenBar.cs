using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class OxygenBar : MonoBehaviour
{
    public Player player;

    private Slider slider;
    
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        slider.value = (float)player.GetOxygenLevel() / player.GetMaxOxygenLevel();
    }
}
