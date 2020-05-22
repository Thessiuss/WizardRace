using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Animator anim;
#pragma warning restore 0649
    private CapsuleCollider capCollider;
    private Vector3 expPos = new Vector3 (0, 0.88f, 0.77f);
    private Vector3 contPos = new Vector3 (0, 0.41f, 0.15f);
    private Vector3 squiPos = new Vector3(0, 0.06f, 0.03f);
    private bool squish;
    private bool expand;
    private bool contract;

    private void Awake()
    {
        capCollider = this.transform.GetChild(0).gameObject.GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (anim.GetBool("Attack"))
        {
            if (squish)
                ColliderModify(squiPos, 0.88f, 0.94f);
            if (expand)
                ColliderModify(expPos, 0.70f, 3.14f);
            if (contract)
                ColliderModify(contPos, 0.64f, 1.57f);
        }
        if (!anim.GetBool("Attack"))
        {
            capCollider.radius = 0.64f;
            capCollider.height = 1.57f;
            capCollider.center = contPos;
        }
    }
    
    private void ColliderModify(Vector3 pos, float radius, float height)
    {
        capCollider.center = Vector3.MoveTowards(capCollider.center, pos, Time.deltaTime / 0.40f);
        capCollider.radius = Mathf.MoveTowards(capCollider.radius, radius, Time.deltaTime / 0.70f);
        capCollider.height = Mathf.MoveTowards(capCollider.height, height, Time.deltaTime / 0.10f);
    }
    
    public void Squish()
    {
        squish = true;
        expand = false;
        contract = false;
    }

    public void Expand()
    {
        squish = false;
        expand = true;
        contract = false;
    }

    public void Contract()
    {
        squish = false;
        expand = false;
        contract = true;
    }
}
