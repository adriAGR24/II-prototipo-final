using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    public void OnClick_PedirCarta()
    {
        Debug.Log("Botón Pedir pulsado");
        EventManager.OnPlayerHit?.Invoke();
    }

    public void OnClick_Plantarse()
    {
        Debug.Log("Botón Plantarse pulsado");
        EventManager.OnPlayerStand?.Invoke();
    }

    public void OnClick_EmpezarJuego()
    {
        EventManager.OnGameStart?.Invoke();
    }
}