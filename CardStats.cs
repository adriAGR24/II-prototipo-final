using UnityEngine;

public class CardStats : MonoBehaviour
{
    public string palo; 
    public int valor;   

    public void ConfigurarCarta(string _palo, int _valor)
    {
        palo = _palo;
        valor = _valor;
        this.name = $"{_palo}_{_valor}"; 
    }
}