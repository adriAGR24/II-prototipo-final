using UnityEngine;
using UnityEngine.UI;
using Whisper;
using Whisper.Utils;
using System.Text.RegularExpressions;

public class VoiceControlBlackjack : MonoBehaviour
{
    [Header("Referencias Whisper")]
    public WhisperManager whisper;
    public MicrophoneRecord microphoneRecord;
    
    [Header("UI Debug (Opcional)")]
    public Text outputText; 

    private void Awake()
    {
        microphoneRecord.OnRecordStop += OnRecordStop;
    }

    private async void Start()
    {
        if (whisper != null)
        {
            Debug.Log("Cargando modelo Whisper...");
            await whisper.InitModel();
            Debug.Log("Modelo Whisper cargado y listo.");
        }
    }

    private void OnDestroy()
    {
        microphoneRecord.OnRecordStop -= OnRecordStop;
    }
    
    public void EmpezarEscucha()
    {
        Debug.Log("Escuchando");
        if (!microphoneRecord.IsRecording)
        {
            if(outputText) outputText.text = "Escuchando...";
            microphoneRecord.StartRecord();
        }
    }

    public void TerminarEscucha()
    {
        Debug.Log("Terminando");
        if (microphoneRecord.IsRecording)
        {
            if(outputText) outputText.text = "Procesando...";
            microphoneRecord.StopRecord();
        }
    }

    private async void OnRecordStop(AudioChunk recordedAudio)
    {
        var res = await whisper.GetTextAsync(recordedAudio.Data, recordedAudio.Frequency, recordedAudio.Channels);
        
        if (res == null || string.IsNullOrEmpty(res.Result)) 
            return;

        string textoDetectado = res.Result;
        Debug.Log($"Whisper entendió: {textoDetectado}");
        
        if (outputText) outputText.text = $"Entendido: {textoDetectado}";

        ProcesarComandos(textoDetectado);
    }

    void ProcesarComandos(string texto)
    {
        if (Regex.IsMatch(texto, @"\b(pedir|dame|carta|otra|hit|más)\b", RegexOptions.IgnoreCase))
        {
            Debug.Log("Voz: Jugador pide carta");
            EventManager.OnPlayerHit?.Invoke();
        }
        
        else if (Regex.IsMatch(texto, @"\b(planto|plantarse|basta|listo|stand|quedo|bien)\b", RegexOptions.IgnoreCase))
        {
            Debug.Log("Voz: Jugador se planta");
            EventManager.OnPlayerStand?.Invoke();
        }

        else if (Regex.IsMatch(texto, @"\b(jugar|empezar|reiniciar|play|start|nuevo|nueva)\b", RegexOptions.IgnoreCase))
        {
            Debug.Log("Voz: Reiniciar juego");
            EventManager.OnGameStart?.Invoke();
        }
    }
}