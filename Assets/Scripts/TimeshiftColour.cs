using UnityEngine;

public class TimeshiftColour : MonoBehaviour {

    [HideInInspector]
    public Material normalMaterial;
    public Material reverseMaterial;

    Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    public void ShiftTimeStart()
    {
        normalMaterial = renderer.material;
        renderer.material = GetMaterialForTimeshift();
    }

    public void ShiftTimeStop()
    {
        renderer.material = normalMaterial;
    }

    private void OnEnable()
    {
        Start();
    }

    Material GetMaterialForTimeshift()
    {
        if (reverseMaterial)
            return reverseMaterial;
        else
            return normalMaterial;
    }
}
