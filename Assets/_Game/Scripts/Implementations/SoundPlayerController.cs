using System.Collections;
using UnityEngine;

namespace WOBH
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundPlayerController : MonoBehaviour
    {
        [SerializeField] private AudioClip shoot;
        [SerializeField] AudioClip fight;
        [SerializeField] AudioClip scream;
        [SerializeField] AudioClip reload;
        [SerializeField] AudioClip noBullets;
        [SerializeField] AudioClip getAmmunition;
        private AudioSource audioSource;

        private void Awake() => audioSource = GetComponent<AudioSource>();

        public void Shoot() => audioSource.PlayOneShot(shoot);

        public void Scream()
        {
            StartCoroutine(ScreamCourotine());

            //audioSource.PlayOneShot(scream);
        }

        private IEnumerator ScreamCourotine()
        {
            var wait = new WaitForSeconds(fight.length - 1f);

            audioSource.PlayOneShot(fight);
            yield return wait;
            audioSource.PlayOneShot(scream);
        }

        public void Reload() => audioSource.PlayOneShot(reload);

        public void GetAmmunition() => audioSource.PlayOneShot(getAmmunition);

        public void NoBullets() => audioSource.PlayOneShot(noBullets);

        public static AudioClip Combine(params AudioClip[] clips)
        {
            if (clips == null || clips.Length == 0)
                return null;

            int length = 0;
            for (int i = 0; i < clips.Length; i++)
            {
                if (clips[i] == null)
                    continue;

                length += clips[i].samples * clips[i].channels;
            }

            float[] data = new float[length];
            length = 0;
            for (int i = 0; i < clips.Length; i++)
            {
                if (clips[i] == null)
                    continue;

                float[] buffer = new float[clips[i].samples * clips[i].channels];
                clips[i].GetData(buffer, 0);
                //System.Buffer.BlockCopy(buffer, 0, data, length, buffer.Length);
                buffer.CopyTo(data, length);
                length += buffer.Length;
            }

            if (length == 0)
                return null;

            AudioClip result = AudioClip.Create("Combine", 44100 * 2, 2, 44100, false, null);
            result.SetData(data, 0);

            return result;
        }
    }
}