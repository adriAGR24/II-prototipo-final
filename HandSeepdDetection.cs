using UnityEngine;
using UnityEngine.XR;

public class HandSpeedDetector : MonoBehaviour
{
    [Header("Ajustes")]
    public float velocidadUmbral = 1.5f; 
    public float cooldown = 3f;

    private float ultimoAviso;

    void Update()
    {

        if (ManoSeMueveRapido(XRNode.RightHand) ||
            ManoSeMueveRapido(XRNode.LeftHand))
        {
            if (Time.time - ultimoAviso > cooldown)
            {
                EventManager.OnPlayerMoveHandsFast?.Invoke();
                ultimoAviso = Time.time;
            }
        }
    }

    bool ManoSeMueveRapido(XRNode mano)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(mano);

        if (device.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocidad))
        {
            return velocidad.magnitude > velocidadUmbral;
        }

        return false;
    }
}
