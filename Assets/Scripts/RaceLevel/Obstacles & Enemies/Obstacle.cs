using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    // Make a reference child for this object to turn on when it dies
    // When that object turns on, it triggers a script to handle the despawn/animations.
    public float health;
    [Range(0,0.99f)]
    public float penalty;
#pragma warning disable 0649
    [SerializeField] private int score;
    [SerializeField] private bool playerTriggers = true;
    [SerializeField] private GameObject disableScriptObj;
#pragma warning restore 0649
    private PlayerController pc;
    private ScoreController sc;
    private float currentHealth;
    private bool dead;

    public void Start()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        sc = GameObject.Find("ScoreController").GetComponent<ScoreController>();
    }

    public void OnEnable()
    {
        currentHealth = health;
        dead = false;
        if (disableScriptObj != null)
            disableScriptObj.SetActive(false);
    }
    public bool IsAlive()
    {
        return !dead;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerTriggers)
        {
            if (other.gameObject.name == "BodyTrigger" || other.gameObject.name == "BodyCollider")
            {
                PlayerCrashed();
            }
            if (other.gameObject.name == "FeetTrigger")
            {
                DealDamage(1000.0f);
            }
        }
    }

    public void PlayerCrashed()
    {
        pc.SetPlayerVelocity(penalty);
        // TODO: Fix to trigger or deal damage.Just for testing
        DealDamage(1000.0f);
    }

    public void DealDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0 && !dead) {
            dead = true;
            sc.AddPointValue(score);
            // Change this to do the animation for each object.
            if (disableScriptObj != null)
            {
                disableScriptObj.SetActive(true);
            }
            else
            {
                this.gameObject.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
