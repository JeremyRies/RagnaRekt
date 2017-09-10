using UnityEngine;

namespace Sound
{
    public class BackgroundMusic : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _clip;
        [SerializeField]
        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource.loop = true;
            _audioSource.clip = _clip;
            _audioSource.Play();
        }
    }
}