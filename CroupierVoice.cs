using UnityEngine;

public class CroupierVoice : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Frases")]
    public AudioClip[] frasesInicio;
    public AudioClip[] frasesVictoria;
    public AudioClip[] frasesDerrota;
    public AudioClip[] frasesAdvertencia;


    void OnEnable()
    {
        EventManager.OnGameStart += FraseInicio;
        EventManager.OnGameOver += FraseFin;
        EventManager.OnPlayerMoveHandsFast += FraseManosRapidas;

    }

    void OnDisable()
    {
        EventManager.OnGameStart -= FraseInicio;
        EventManager.OnGameOver -= FraseFin;
        EventManager.OnPlayerMoveHandsFast -= FraseManosRapidas;

    }

    void FraseInicio()
    {
        ReproducirAleatoria(frasesInicio);
    }

    void FraseFin(string mensaje)
    {
        if (mensaje.Contains("GANASTE") || mensaje.Contains("BLACKJACK"))
        {
            ReproducirAleatoria(frasesVictoria);
        }
        else if (mensaje.Contains("PERDISTE"))
        {
            ReproducirAleatoria(frasesDerrota);
        }
    }

    void ReproducirAleatoria(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0 || audioSource == null)
            return;

        AudioClip clip = clips[Random.Range(0, clips.Length)];
        audioSource.PlayOneShot(clip);
    }

    void FraseManosRapidas()
    {
        ReproducirAleatoria(frasesAdvertencia);
    }

}
