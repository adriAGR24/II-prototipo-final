using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CroupierVisuals : MonoBehaviour
{

    [Header("Configuración")]
    [Header("Audio Timing")]
    public float retrasoAntesDeMoverCarta = 2.5f; 

    public Transform manoSpawnPoint;
    public Transform zonaJugador;
    public Transform zonaCroupier;
    public float velocidadVuelo = 2.0f;
    public float alturaArco = 0.2f;

    private Animator animatorCroupier; 
    private Vector3 offsetJug = Vector3.zero;
    private Vector3 offsetCrup = Vector3.zero;
    private GameObject cartaOcultaCroupier;

    void Start()
    {
        animatorCroupier = GetComponent<Animator>();
        if (animatorCroupier == null) Debug.LogError("¡OJO! No encuentro el Animator en este objeto.");
        EventManager.OnCardDealt += AnimarReparto;
        EventManager.OnGameStart += ResetMesa;
        EventManager.OnDealerReveal += RevelarCartaOculta;
    }

    void ResetMesa()
    {
        offsetJug = Vector3.zero;
        offsetCrup = Vector3.zero;
        cartaOcultaCroupier = null;
    }

    void AnimarReparto(GameObject carta, bool esParaJugador, bool esBocaAbajo)
    {
        carta.SetActive(false); 
        Vector3 destinoBase = esParaJugador ? zonaJugador.position : zonaCroupier.position;
        Vector3 offsetActual = esParaJugador ? offsetJug : offsetCrup;
        Vector3 destinoFinal = destinoBase + offsetActual;
        destinoFinal.y += 0.02f; 

        float separacion = 0.17f;
        if (esParaJugador) offsetJug += new Vector3(separacion, 0, 0);
        else offsetCrup += new Vector3(separacion, 0, 0);

        Quaternion rotacionFinal = esBocaAbajo 
            ? Quaternion.Euler(90, 0, 0)
            : Quaternion.Euler(-90, 0, 0);

        if (esBocaAbajo && !esParaJugador) cartaOcultaCroupier = carta;        
        StartCoroutine(RepartoConRetraso(carta, destinoFinal, rotacionFinal));
    }

    IEnumerator RepartoConRetraso(GameObject carta, Vector3 destino, Quaternion rotFinal)
    {
        
        if (animatorCroupier != null)
        {
            animatorCroupier.SetTrigger("Repartir");
        }
        yield return new WaitForSeconds(retrasoAntesDeMoverCarta);
        carta.transform.position = manoSpawnPoint.position;
        carta.transform.rotation = manoSpawnPoint.rotation;
        carta.SetActive(true); 
        StartCoroutine(MoverCartaConArco(carta.transform, destino, rotFinal));
    }

    
    
    
    IEnumerator MoverCartaConArco(Transform carta, Vector3 destino, Quaternion rotFinal)
    {
        float t = 0;
        Vector3 inicio = carta.position;
        Quaternion rotInicio = carta.rotation;

        while (t < 1)
        {
            t += Time.deltaTime * velocidadVuelo;
            Vector3 posActual = Vector3.Lerp(inicio, destino, t);
            float arco = 4 * alturaArco * t * (1 - t); 
            posActual.y += arco;
            carta.position = posActual;
            carta.rotation = Quaternion.Lerp(rotInicio, rotFinal, t);
            yield return null;
        }
        carta.position = destino;
        carta.rotation = rotFinal;
    }
    
    void RevelarCartaOculta()
    {
        if (cartaOcultaCroupier != null) StartCoroutine(VoltearCarta(cartaOcultaCroupier));
    }

    IEnumerator VoltearCarta(GameObject carta)
    {
        float t = 0;
        Quaternion inicio = carta.transform.rotation;
        Quaternion fin = Quaternion.Euler(-90, 0, 0); 
        while (t < 1)
        {
            t += Time.deltaTime * 3;
            carta.transform.rotation = Quaternion.Lerp(inicio, fin, t);
            float salto = Mathf.Sin(t * Mathf.PI) * 0.05f;
            carta.transform.position += new Vector3(0, salto, 0);
            yield return null;
        }
    }
}