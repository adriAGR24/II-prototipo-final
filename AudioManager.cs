using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Música de ambiente")]
    public AudioSource musicaAmbiente;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ReproducirMusicaAmbiente();
    }

    void ReproducirMusicaAmbiente()
    {
        if (musicaAmbiente != null && !musicaAmbiente.isPlaying)
        {
            musicaAmbiente.loop = true;
            musicaAmbiente.Play();
        }
    }

    public void CambiarVolumenMusica(float volumen)
    {
        if (musicaAmbiente != null)
            musicaAmbiente.volume = volumen;
    }
}
