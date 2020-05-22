using UnityEngine.UI;
using UnityEngine;

public class SkinConstraints : MonoBehaviour
{
    [Range (0, 4)]
    public int skinRarity;
    public string skinName;
    public float scaleAdjustment;
    public Vector3 positionAdjustment;
    public Sprite sprite;
}
