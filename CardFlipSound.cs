using UnityEngine;

public class CardFlipSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoVoltear;

    void OnEnable()
    {
        EventManager.OnDealerReveal += ReproducirFlip;
    }

    void OnDisable()
    {
        EventManager.OnDealerReveal -= ReproducirFlip;
    }

    void ReproducirFlip()
    {
        if (audioSource == null || sonidoVoltear == null)
            return;

        audioSource.PlayOneShot(sonidoVoltear);
    }
}
