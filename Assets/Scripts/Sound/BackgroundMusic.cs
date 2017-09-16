using System.Linq;
using UnityEngine;

namespace Sound
{
    public class BackgroundMusic : MonoBehaviour
    {
        [SerializeField]
        private SfxClip[] _clips;
        [SerializeField]
        private AudioSource _audioSource;
        
        private static BackgroundMusic _backgroundMusicInstance;

        public static BackgroundMusic BackgroundMusicInstance
        {
            get { return _backgroundMusicInstance; }
        }
        private void Awake()
        {
            _backgroundMusicInstance = this;
            _audioSource.loop = true;
        }

        public void SetClip(ClipIdentifier clipIdentifier)
        {
            var clips = _clips.Where(clip => clip.Clip == clipIdentifier).ToArray();
            var rand = Random.Range(0, clips.Length);
            var sfxClip = clips[rand];
            _audioSource.clip = sfxClip.AudioClip;
        }

        public void StartPlay()
        {
            _audioSource.Play();
        }

        public void StopPlay()
        {
            _audioSource.Stop();
        }
    }
}