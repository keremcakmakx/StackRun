using UnityEngine;

public class CutObjectController : MonoBehaviour
{
    public Renderer visual;
    private bool _isActive;
    private float _timer;

    public void SetTransform(float scaleX, float positionX, Material material)
    {
        visual.sharedMaterial = material;
        transform.SetPositionX(positionX);
        transform.SetLocalScaleX(scaleX);
        gameObject.SetActive(true);
    }
    public void OnEnable()
    {
        _isActive = true;
    }

    public void OnDisable()
    {
        _isActive = false;
    }

    private void Update()
    {
        
    }
}
