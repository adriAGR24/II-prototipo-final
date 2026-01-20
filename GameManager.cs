using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Referencias Generales")]
    public DeckManager deckManager;
    public ShuffleSound shuffleSound;

    [Header("Referencias UI (Textos)")]
    public TextMeshProUGUI txtPuntosJugador;  
    public TextMeshProUGUI txtPuntosCroupier; 
    public TextMeshProUGUI txtMensajeCentral; 

    [Header("Referencias UI Imágenes")]
    public GameObject imagenWin;  
    public GameObject imagenLose; 

    [Header("NPC y Animaciones")] 
    public Animator npcAnimator;               
    public string triggerVictoria = "Victoria"; 
    public string triggerDerrota = "Derrota";  

    private List<CardStats> manoJugador = new List<CardStats>();
    private List<CardStats> manoCroupier = new List<CardStats>();
    private bool juegoTerminado = false;
    private bool cartaCroupierRevelada = false; 

    void Start()
    {
        EventManager.OnPlayerHit += PlayerPideCarta;
        EventManager.OnPlayerStand += PlayerSePlanta;
        EventManager.OnGameStart += IniciarPartida;

        OcultarImagenes();

        ActualizarMensaje("Dispara a Start para jugar");
        ActualizarMarcador(0, 0); 
    }

    void IniciarPartida()
    {
        juegoTerminado = false;
        cartaCroupierRevelada = false; 
        manoJugador.Clear();
        manoCroupier.Clear();

        OcultarImagenes();

        if (npcAnimator != null)
        {
            npcAnimator.ResetTrigger(triggerVictoria);
            npcAnimator.ResetTrigger(triggerDerrota);
        }

        ActualizarMensaje("Barajando cartas...");
        StartCoroutine(SecuenciaBarajarYRepartir());
    }

    void OcultarImagenes()
    {
        if (imagenWin != null) imagenWin.SetActive(false);
        if (imagenLose != null) imagenLose.SetActive(false);
    }

    IEnumerator SecuenciaBarajarYRepartir()
    {
        if(shuffleSound != null) shuffleSound.ReproducirBarajado();
        yield return new WaitForSeconds(1.5f);
        
        deckManager.PrepararNuevaRonda();
        
        ActualizarMensaje("Repartiendo...");
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(RepartoInicialSecuencia());
    }

    IEnumerator RepartoInicialSecuencia()
    {
        yield return new WaitForSeconds(0.5f);
        DarCarta(esJugador: true, bocaAbajo: false);

        yield return new WaitForSeconds(0.5f);
        DarCarta(esJugador: false, bocaAbajo: false);

        yield return new WaitForSeconds(0.5f);
        DarCarta(esJugador: true, bocaAbajo: false);

        yield return new WaitForSeconds(0.5f);
        DarCarta(esJugador: false, bocaAbajo: true);

        ActualizarMensaje("TU TURNO: ¿Pides o te Plantas?");
        CheckBlackjackNatural();
    }

    void DarCarta(bool esJugador, bool bocaAbajo)
    {
        GameObject cartaObj = deckManager.SacarCarta();
        CardStats datos = cartaObj.GetComponent<CardStats>();

        if (esJugador) manoJugador.Add(datos);
        else manoCroupier.Add(datos);

        EventManager.OnCardDealt?.Invoke(cartaObj, esJugador, bocaAbajo);

        RefrescarUI();

        if (!bocaAbajo)
        {
            string quien = esJugador ? "Jugador" : "Croupier";
            ActualizarMensaje($"{quien}: {datos.valor} de {datos.palo}");
        }
    }

    void PlayerPideCarta()
    {
        if (juegoTerminado) return;
        
        DarCarta(esJugador: true, bocaAbajo: false);
        
        int puntos = CalcularPuntos(manoJugador);
        
        if (puntos > 21) 
        {
            GameOver("¡TE PASASTE!", false); 
        }
        else
        {
            ActualizarMensaje("¿Otra carta?");
        }
    }

    void PlayerSePlanta()
    {
        if (juegoTerminado) return;
        
        int puntosJug = CalcularPuntos(manoJugador);
        ActualizarMensaje($"Te plantas con {puntosJug}. Turno Croupier.");
        
        StartCoroutine(TurnoCroupierIA());
    }

    IEnumerator TurnoCroupierIA()
    {
        yield return new WaitForSeconds(1f);
        
        cartaCroupierRevelada = true; 
        EventManager.OnDealerReveal?.Invoke(); 
        RefrescarUI(); 
        
        ActualizarMensaje("Croupier revela su carta...");
        yield return new WaitForSeconds(1.5f);

        int puntosCrupier = CalcularPuntos(manoCroupier);

        while (puntosCrupier < 17)
        {
            ActualizarMensaje("Croupier pide carta...");
            yield return new WaitForSeconds(1f);
            
            DarCarta(esJugador: false, bocaAbajo: false);
            puntosCrupier = CalcularPuntos(manoCroupier);
        }

        ResolverGanador();
    }

    void ResolverGanador()
    {
        int jug = CalcularPuntos(manoJugador);
        int crup = CalcularPuntos(manoCroupier);

        if (crup > 21) GameOver("¡GANASTE! La casa se pasó.", true);
        else if (jug > crup) GameOver($"¡GANASTE! {jug} vs {crup}", true);
        else if (jug < crup) GameOver($"LA CASA GANA. {crup} vs {jug}", false);
        else GameOver("EMPATE", true); 
    }

    void GameOver(string mensaje, bool esVictoria)
    {
        juegoTerminado = true;
        
        EventManager.OnGameOver?.Invoke(mensaje);

        if(txtMensajeCentral != null) txtMensajeCentral.text = "";
        if(txtPuntosJugador != null) txtPuntosJugador.text = "";
        if(txtPuntosCroupier != null) txtPuntosCroupier.text = "";

        OcultarImagenes(); 

        if (esVictoria)
        {
            if (imagenWin != null) imagenWin.SetActive(true);
        }
        else
        {
            if (imagenLose != null) imagenLose.SetActive(true);
        }

        if (npcAnimator != null)
        {
            if (esVictoria)
            {
                npcAnimator.SetTrigger(triggerVictoria);
            }
            else
            {
                npcAnimator.SetTrigger(triggerDerrota);
            }
        }
    }

    void RefrescarUI()
    {
        if (juegoTerminado) return;

        int pJugador = CalcularPuntos(manoJugador);
        int pCroupier = 0;

        if (cartaCroupierRevelada)
        {
            pCroupier = CalcularPuntos(manoCroupier);
        }
        else
        {
            if(manoCroupier.Count > 0)
                pCroupier = manoCroupier[0].valor; 
        }

        ActualizarMarcador(pJugador, pCroupier);
    }

    void ActualizarMarcador(int puntosJ, int puntosC)
    {
        if(txtPuntosJugador != null) 
            txtPuntosJugador.text = $"JUGADOR\n<size=150%>{puntosJ}</size>";
        
        if(txtPuntosCroupier != null) 
        {
            string scoreStr = cartaCroupierRevelada ? puntosC.ToString() : $"{puntosC} + ?";
            txtPuntosCroupier.text = $"CROUPIER\n<size=150%>{scoreStr}</size>";
        }
    }

    void ActualizarMensaje(string texto)
    {
        if(txtMensajeCentral != null)
        {
            txtMensajeCentral.color = Color.white; 
            txtMensajeCentral.text = texto;
        }
    }

    int CalcularPuntos(List<CardStats> mano)
    {
        int total = 0;
        int ases = 0;

        foreach (var c in mano)
        {
            total += c.valor; 
            if (c.valor == 11) ases++;
        }

        while (total > 21 && ases > 0)
        {
            total -= 10;
            ases--;
        }
        return total;
    }

    void CheckBlackjackNatural()
    {
        if (CalcularPuntos(manoJugador) == 21)
        {
            GameOver("¡BLACKJACK!", true); 
        }
    }
}