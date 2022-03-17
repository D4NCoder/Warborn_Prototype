using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Warborn.Characters.Player.Statistics;
using Warborn.Helpers;

public class GUIHealthManager : MonoBehaviour
{
    [SerializeField] private Slider Slider;
    [SerializeField] private StatsController statsController;
    [SerializeField] private float lowestValueSlider = -0.16f;
    void Update()
    {
        float value = statsController.GetHealthController().GetHealth();
        Slider.value = value.Remap(0, statsController.MaxHealth, lowestValueSlider, 1);
    }
}
