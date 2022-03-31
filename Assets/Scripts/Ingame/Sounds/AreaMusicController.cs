using UnityEngine;
using Warborn.Characters.Player.PlayerModel.Collisions;

namespace Warborn.Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class AreaMusicController : MonoBehaviour
    {
        [SerializeField] private AudioSource MusicToPlay;
        private void Start()
        {
            MusicToPlay = this.gameObject.GetComponent<AudioSource>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == CollisionType.PLAYER)
            {
                MusicToPlay.Play();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == CollisionType.PLAYER)
            {
                MusicToPlay.Stop();
            }
        }
    }

}

