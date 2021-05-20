using System;
using OrchestraArmy.Enum;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Player;
using UnityEngine;

namespace OrchestraArmy
{
    public class AudioManager: MonoBehaviour, IListener<PlayerAttackEvent>
    {
        public AudioSource AudioSource;

        public void Start()
        {
            EventManager.Bind<PlayerAttackEvent>(this);
        }

        public void OnEvent(PlayerAttackEvent invokedEvent)
        {
            AudioSource.pitch = ((int) invokedEvent.Tone) * 0.1f + 1f;
            AudioSource.Play();
        }
    }
}