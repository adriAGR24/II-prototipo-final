using UnityEngine;

public class ShuffleSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoBarajar;

    public void ReproducirBarajado()
    {
        if (audioSource == null || sonidoBarajar == null)
            return;

        audioSource.clip = sonidoBarajar;
        audioSource.loop = false;
        audioSource.Play();
    }
}
