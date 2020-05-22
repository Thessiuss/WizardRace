using System.Collections;
using UnityEngine;

public class Spell_Sunflower : MonoBehaviour {
    // Standard spell template found under Spell_Fireball_Test
#pragma warning disable 0649
    [SerializeField] private GameObject flowerBase;
    [SerializeField] private GameObject sunflowerSeed;
#pragma warning restore 0649

    private float spellLevel;
    private float movementSpeed;
    private float duration;
    private float distance;
    private float damage;

    private Transform playerTransform;
    private Transform spellTransform;
    private Rigidbody rb;
    private bool spellShot = true;
    private bool hasTriggered;
    private float deltaDistance = 0;
    private float distanceToGround;
    private float triggeredTime;
    private int groundLayerMask = 0;


    void Awake()
    {
        groundLayerMask = (1 << 0) | (1 << 16) | (1 << 17);
        rb = GetComponent<Rigidbody>();
        spellLevel = this.gameObject.GetComponent<SpellInformation>().value[0];
        movementSpeed = this.gameObject.GetComponent<SpellInformation>().value [3];
        duration = this.gameObject.GetComponent<SpellInformation>().value [4];
        distance = this.gameObject.GetComponent<SpellInformation>().value [5];
        damage = this.gameObject.GetComponent<SpellInformation>().value [6];
        playerTransform = GameObject.Find("Player").transform;
        spellTransform = this.transform;
    }


    private void OnEnable()
    {
        spellShot = true;
        distanceToGround = 0;
        triggeredTime = 0;
        flowerBase.gameObject.SetActive(false);
        sunflowerSeed.gameObject.SetActive(true);
    }


    private void Update()
    {
        if (hasTriggered)
        {
            deltaDistance = Mathf.Abs(transform.position.x - playerTransform.position.x);
            if (deltaDistance > distance)
            {
                this.gameObject.SetActive(false);
            }
            this.transform.position -= new Vector3(0, 
                ((distanceToGround * 0.25f) * (duration - triggeredTime -1) * Time.deltaTime/duration), 0);
            triggeredTime += Time.deltaTime;
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
        spellShot = false;
        transform.parent = null;
        flowerBase.gameObject.SetActive(true);
        sunflowerSeed.gameObject.SetActive(false);
        StartCoroutine(SpellDuration());
        StartCoroutine(Falling());
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

    private IEnumerator SpellDuration()
    {
        yield return new WaitForSeconds(duration);
        DetonateSpell();
    }

    private IEnumerator Falling()
    {
        RaycastHit hit;
        Vector3 rcOrigin = new Vector3 (this.transform.position.x, this.transform.position.y - 0.07f, this.transform.position.z);
        if (Physics.Raycast(rcOrigin, Vector3.down, out hit, 200.0f, groundLayerMask))
        {
            distanceToGround = Vector3.Distance(hit.point, rcOrigin);
            hasTriggered = true;
            yield return null;
        }
        else
        {
            DetonateSpell();
        }
    }
}
