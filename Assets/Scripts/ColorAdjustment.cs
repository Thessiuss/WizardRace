using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorAdjustment : MonoBehaviour
{
    public Color color;
    [Range(0.0f, 1.0f)]
    public float metallic;
    [Range(0.0f, 1.0f)]
    public float smoothness;
#pragma warning disable 0649
    [SerializeField] private ParticleSystem ps;
#pragma warning restore 0649

    private Renderer rend;

    private void OnEnable()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = color;
        rend.material.SetFloat("_Metallic", metallic);
        rend.material.SetFloat("_Glossiness", smoothness);
        if (ps != null)
        {
            ParticleSystem.MainModule ma = ps.main;
            ma.startColor = color;
        }
    }
}
