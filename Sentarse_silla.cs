using UnityEngine;

public class SistemaSentarse : MonoBehaviour
{
    public Transform jugadorXROrigin; 
    public Transform puntoDeAsiento;

    public void SentarJugador()
    {
        jugadorXROrigin.position = puntoDeAsiento.position;
        jugadorXROrigin.rotation = puntoDeAsiento.rotation;
    }
}

