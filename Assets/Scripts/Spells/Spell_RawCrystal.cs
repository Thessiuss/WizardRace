using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_RawCrystal : MonoBehaviour
{
    // Standard spell template found under Spell_Fireball_Test
    private float spellLevel;
    private float movementSpeed;
    private float fireSpeed;
    private float distance;
    private float damage;

    private Transform playerTransform;
    private Rigidbody rb;
    private bool spellShot = true;
    private float deltaDistance = 0;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spellLevel = this.gameObject.GetComponent<SpellInformation>().value [0];
        movementSpeed = this.gameObject.GetComponent<SpellInformation>().value [3];
        fireSpeed = this.gameObject.GetComponent<SpellInformation>().value [4];
        distance = this.gameObject.GetComponent<SpellInformation>().value [5];
        damage = this.gameObject.GetComponent<SpellInformation>().value [6];
        playerTransform = GameObject.Find("Player").transform;
    }


    private void OnEnable()
    {
        spellShot = true;
    }


    private void Update()
    {
        deltaDistance = Mathf.Abs(transform.position.x - playerTransform.position.x);
        if (deltaDistance > distance)
        {
            this.gameObject.SetActive(false);
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Obstacle":
                other.gameObject.GetComponent<Obstacle>().DealDamage(damage);
                DetonateSpell();
                break;
            case "Wisp":
                other.gameObject.GetComponent<Wisp>().TriggerWisp();
                break;
            case "Floor":
                DetonateSpell();
                break;
            case "HiddenFloor":
                DetonateSpell();
                break;
            case "Wall":
                DetonateSpell();
                break;
            case "HiddenWall":
                DetonateSpell();
                break;
        }
    }


    public void TriggerSpell()
    {
        float playerSpeed = playerTransform.gameObject.GetComponent<Rigidbody>().velocity.x;
        Vector3 direction = new Vector3(Mathf.Abs(playerSpeed) + fireSpeed, 0, 0);
        rb.isKinematic = false;
        rb.velocity = direction;
        spellShot = false;
        transform.parent = null;
    }


    public void DetonateSpell()
    {
        if (spellShot)
        {
            SpellController.isCasting = false;
            ScoreController.isCasting = false;
        }
        this.gameObject.SetActive(false);
    }
}
