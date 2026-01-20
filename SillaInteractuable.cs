using UnityEngine;


public class SillaInteractuable : MonoBehaviour
{
    [Header("Configuración de Visuales")]
    public Renderer meshRenderer; 
    public Material materialNormal;
    public Material materialResaltado;

    [Header("Configuración del Jugador")]
    public Transform xrOrigin; 
    public Transform puntoDeSentado; 
    public UnityEngine.XR.Interaction.Toolkit.Locomotion.LocomotionProvider moveProvider; 

    public void ResaltarSilla()
    {
        meshRenderer.material = materialResaltado;
    }

    public void QuitarResaltado()
    {
        if (meshRenderer != null)
        {
            meshRenderer.material = materialNormal;
        }
    }

    public void Sentarse()
    {

        xrOrigin.position = puntoDeSentado.position;
        xrOrigin.rotation = puntoDeSentado.rotation;

        if (moveProvider != null)
        {
            moveProvider.enabled = false;
        }
        
        QuitarResaltado();
        
        RecalibrarAltura();
    }
    
    public void Levantarse()
    {
        if (moveProvider != null)
        {
            moveProvider.enabled = true;
        }
    }

    private void RecalibrarAltura()
    {
        var originComponent = xrOrigin.GetComponent<Unity.XR.CoreUtils.XROrigin>();
        if(originComponent != null)
        {
            originComponent.RequestedTrackingOriginMode = Unity.XR.CoreUtils.XROrigin.TrackingOriginMode.Device;
        }
    }
}