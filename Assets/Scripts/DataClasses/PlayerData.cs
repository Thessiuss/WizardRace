using UnityEngine;

public class PlayerData : MonoBehaviour {
    
    public int acceleration;
    public bool freeMovement;
    public float jumpHeight;
    public float maxVelocity;
    public float minVelocity;
    [Range (1.1f, 100.0f)]
    public float maxSpeedBonus;
    public GameObject skinBody;
    public GameObject skinFeet;
    public GameObject skinHands;
    public GameObject skinHead;
}
