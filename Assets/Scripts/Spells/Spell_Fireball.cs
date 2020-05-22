using System.Collections;
using UnityEngine;

public class Spell_Fireball : MonoBehaviour {
    // Standard spell template found under Spell_Fireball_Test
#pragma warning disable 0649
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject fireball;
#pragma warning disable 0649
    private float spellLevel;
    private float movementSpeed = 0.05f;
    private float fireSpeed = 26.0f;
    private float distance = 40.0f;
    private float damage = 20;
    private int collidedObjID;
    private Transform playerTransform;
    private Rigidbody rb;
    private bool spellShot = true;
    private float deltaDistance = 0;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spellLevel = this.gameObject.GetComponent<SpellInformation>().value[0];
        movementSpeed = this.gameObject.GetComponent<SpellInformation>().value [3];
        fireSpeed = this.gameObject.GetComponent<SpellInformation>().value [4];
        distance = this.gameObject.GetComponent<SpellInformation>().value [5];
        damage = this.gameObject.GetComponent<SpellInformation>().value [6];
        playerTransform = GameObject.Find("Player").transform;
    }


    private void OnEnable()
    {
        spellShot = true;
        explosion.SetActive(false);
        fireball.SetActive(true);
        collidedObjID = 0;
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
        if (other.gameObject.GetInstanceID() != collidedObjID)
        {
            switch (other.gameObject.tag)
            {
                case "Obstacle":
                    other.gameObject.GetComponent<Obstacle>().DealDamage(damage);
                    collidedObjID = other.gameObject.GetInstanceID();
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

    // Called externally by Spell_Controller and some other objects on "OnTriggerEnter"
    public void DetonateSpell()
    {
        fireball.SetActive(false);
        explosion.SetActive(true);
        rb.velocity = Vector3.zero;
        if (spellShot)
        {
            SpellController.isCasting = false;
            ScoreController.isCasting = false;
        }
        StartCoroutine(Exploding());
    }

    private IEnumerator Exploding()
    {
        yield return new WaitForSeconds(0.55f);
        this.gameObject.SetActive(false);
    }
}
