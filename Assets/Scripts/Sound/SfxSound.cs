using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sound
{
    public class SfxSound : MonoBehaviour
    {
        [SerializeField] private SfxClip[] _clips;
        [SerializeField] private AudioSource _sfxAudioSource;

        private static SfxSound _sfxSingletonInstance;

        public static SfxSound SfxSoundInstance
        {
            get { return _sfxSingletonInstance; }
        }

        private void Start()
        {
            _sfxSingletonInstance = this;
        }

        public void PlayClip(ClipIdentifier clipIdentifier)
        {
            var clips = _clips.Where(clip => clip.Clip == clipIdentifier).ToArray();
            var rand = Random.Range(0, clips.Length);
            var sfxClip = clips[rand];
            _sfxAudioSource.PlayOneShot(sfxClip.AudioClip);
        }

        public void Stop()
        {
            _sfxAudioSource.Stop();
        }

        void Update()
        {
            //testing area

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                PlayClip((ClipIdentifier)1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PlayClip((ClipIdentifier)2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                PlayClip((ClipIdentifier)3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                PlayClip((ClipIdentifier)4);
            }
        }


    }
}