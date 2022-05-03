using UnityEngine;
using Warborn.Ingame.Characters.Player.PlayerModel.Collisions;

namespace Warborn.Ingame.Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class AreaMusicController : MonoBehaviour
    {
        [SerializeField] private AudioSource musicToPlay;
        private void Start()
        {
            musicToPlay = this.gameObject.GetComponent<AudioSource>();
        }
        private void OnTriggerEnter(Collider _other)
        {
            if (_other.gameObject.layer == CollisionType.PLAYER)
            {
                musicToPlay.Play();
            }
        }

        private void OnTriggerExit(Collider _other)
        {
            if (_other.gameObject.layer == CollisionType.PLAYER)
            {
                musicToPlay.Stop();
            }
        }
    }

}

