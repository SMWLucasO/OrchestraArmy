using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OrchestraArmy.Entity.Entities.Players;

public class StaminaSliderController : MonoBehaviour
{
    public Slider StaminaSlider;
    public GameObject Player;
    private Player _player;
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        _player = (Player)Player.GetComponent("Player");
        StaminaSlider.maxValue = _player.EntityData.MaxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        StaminaSlider.value = _player.EntityData.Stamina;

    }
}
