using UnityEngine;

public class ColouredButtonLightController : MonoBehaviour {

    [SerializeField] private Light _light;

    public void SetLightActive() {
        _light.enabled = true;
    }

    public void SetLightInactive() {
        _light.enabled = false;
    }
}
