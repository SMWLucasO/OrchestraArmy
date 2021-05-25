using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OrchestraArmy.Entity.Entities.Players;


public class HealthSliderController : MonoBehaviour
{
    public Slider HealthSlider;
    public GameObject Player;
    private Player _player;
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        _player = (Player)Player.GetComponent("Player");
        HealthSlider.maxValue = _player.EntityData.MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        HealthSlider.value = _player.EntityData.Health;
    }
}
