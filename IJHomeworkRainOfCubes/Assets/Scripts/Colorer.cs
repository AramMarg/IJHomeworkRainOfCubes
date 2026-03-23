using UnityEngine;

public class Colorer : MonoBehaviour
{
    private Color _initialColor;

    private Renderer _cubeRenderer;

    // Delete!!!можно Cube cube если сидит он на нем
    public void ChangeColor(Cube cube)
    {
        if (cube.TryGetComponent(out Renderer renderer))
        {
            _cubeRenderer = renderer;

            _initialColor = renderer.material.color;

            renderer.material.color = new Color(Random.value, Random.value, Random.value);
        }
    }

    public void ResetColor()
    {
        _cubeRenderer.material.color = _initialColor;
    }
}
