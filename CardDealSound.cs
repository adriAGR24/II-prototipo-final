using UnityEngine;

public class CardDealSound : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sonidoRepartir;

    void OnEnable()
    {
        EventManager.OnCardDealt += ReproducirSonido;
    }

    void OnDisable()
    {
        EventManager.OnCardDealt -= ReproducirSonido;
    }

    void ReproducirSonido(GameObject carta, bool esJugador, bool esBocaAbajo)
    {
        if (sonidoRepartir == null || audioSource == null)
            return;

        audioSource.PlayOneShot(sonidoRepartir);
    }
}
