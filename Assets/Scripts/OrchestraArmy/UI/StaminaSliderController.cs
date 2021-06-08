using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OrchestraArmy.Entity.Entities.Players;

public class StaminaSliderController : MonoBehaviour
{
    private Slider _staminaSlider;
    
    public GameObject Player;
    
    private Player _player;
    
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        _staminaSlider = (Slider)this.GetComponent("Slider");
        _player = (Player)Player.GetComponent("Player");
        _staminaSlider.maxValue = _player.EntityData.Stamina;
    }

    // Update is called once per frame
    void Update()
    {
        _staminaSlider.value = _player.EntityData.Stamina;

    }
}
