using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OrchestraArmy.Entity.Entities.Players;


public class HealthSliderController : MonoBehaviour
{
    
    private Slider _healthSlider;

    public GameObject Player;
    
    private Player _player;
    
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        _healthSlider = (Slider)this.GetComponent("Slider");
        _player = (Player)Player.GetComponent("Player");
        _healthSlider.maxValue = _player.EntityData.MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        _healthSlider.value = _player.EntityData.Health;
    }
}
