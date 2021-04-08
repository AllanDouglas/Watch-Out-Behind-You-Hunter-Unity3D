using System.Collections;
using UnityEngine;

namespace WOBH
{
    public class EnemySoundController : MonoBehaviour
    {
        [SerializeField] AudioClip scream;
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        public void Scream() => audioSource.PlayOneShot(scream);
    }
}