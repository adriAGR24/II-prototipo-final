using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [Header("Arrastra aquí el objeto 'Card_Deck' de tu jerarquía")]
    public Transform contenedorCartas; 
    
    private List<GameObject> barajaActual = new List<GameObject>();
    private List<GameObject> barajaDescartes = new List<GameObject>();

    private void Awake()
    {
        InicializarBarajaDesdeHijos();
    }

    private void OnEnable()
    {
        EventManager.OnGameStart += PrepararNuevaRonda;
    }
    
    private void OnDisable()
    {
        EventManager.OnGameStart -= PrepararNuevaRonda;
    }

    void InicializarBarajaDesdeHijos()
    {
        barajaActual.Clear();

        foreach (Transform cartaInfo in contenedorCartas)
        {
            GameObject cartaObj = cartaInfo.gameObject;
            
            CardStats stats = cartaObj.GetComponent<CardStats>();
            if (stats == null) stats = cartaObj.AddComponent<CardStats>();

            ProcesarNombreYConfigurar(cartaObj.name, stats);

            cartaObj.SetActive(false);
            barajaActual.Add(cartaObj);
        }
    }

    void ProcesarNombreYConfigurar(string nombreOriginal, CardStats stats)
    {
        string nombreLimpio = nombreOriginal.Replace("Card_", "");
        
        string palo = "";
        string rango = "";
        int valor = 0;
        if (nombreLimpio.StartsWith("Heart"))      { palo = "Hearts";   rango = nombreLimpio.Substring(5); } // 5 letras
        else if (nombreLimpio.StartsWith("Club"))  { palo = "Clubs";    rango = nombreLimpio.Substring(4); } // 4 letras
        else if (nombreLimpio.StartsWith("Diamond")){ palo = "Diamonds"; rango = nombreLimpio.Substring(7); } // 7 letras
        else if (nombreLimpio.StartsWith("Spade")) { palo = "Spades";   rango = nombreLimpio.Substring(5); } // 5 letras
        switch (rango)
        {
            case "Ace":   valor = 11; break;
            case "King":  valor = 10; break;
            case "Queen": valor = 10; break;
            case "Jack":  valor = 10; break;
            default:
                int.TryParse(rango, out valor);
                break;
        }
        stats.ConfigurarCarta(palo, valor);
    }

    public void PrepararNuevaRonda()
    {
        barajaActual.AddRange(barajaDescartes);
        barajaDescartes.Clear();
        foreach (var c in barajaActual) c.SetActive(false);

        Barajar();
    }

    public void Barajar()
    {
        for (int i = 0; i < barajaActual.Count; i++)
        {
            GameObject temp = barajaActual[i];
            int randomIndex = Random.Range(i, barajaActual.Count);
            barajaActual[i] = barajaActual[randomIndex];
            barajaActual[randomIndex] = temp;
        }
    }

    public GameObject SacarCarta()
    {
        if (barajaActual.Count == 0) 
        {
            Debug.LogError("¡No quedan cartas!");
            return null;
        }

        GameObject carta = barajaActual[0];
        barajaActual.RemoveAt(0);
        barajaDescartes.Add(carta);
        
        carta.SetActive(true);
        return carta;
    }
}