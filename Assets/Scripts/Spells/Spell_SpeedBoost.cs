using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_SpeedBoost : MonoBehaviour {
    // Standard spell template found under Spell_Fireball
    public float spellLevel;
    public float movementSpeed = 0.05f;
    public float fireSpeed = 20.0f;
    public float distance = 30.0f;

    private GlobalGameManager ggm;
    private Rigidbody rb;
    private Vector3 direction = new Vector3(1, 0, 0);
    private bool spellShot = true;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ggm = GlobalGameManager.instance;
        spellLevel = ggm.GetSpellLevel();
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            Destroy(other.gameObject);
            if (spellShot)
            {
                SpellController.isCasting = false;
                ScoreController.isCasting = false;
            }
            Destroy(this.gameObject);
        }
    }


    public void TriggerSpell()
    {
        rb.velocity = direction * fireSpeed * spellLevel;
        spellShot = false;
    }
}
