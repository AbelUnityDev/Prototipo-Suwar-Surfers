using UnityEngine;
using UnityEngine.Rendering;

public class ShaderController : MonoBehaviour
{
    [SerializeField, Range(-1f, 1f)] private float curveX;
    [SerializeField, Range(-1f, 1f)] private float curveY;
    [SerializeField] private Material[] materials;
  
    // Update is called once per frame
    void Update()
    {
        foreach (var m in materials)
        {
            m.SetFloat("_CurveX", curveX); 
            m.SetFloat("_CurveY", curveY); 
        }
    }
}
