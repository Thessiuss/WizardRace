using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameObject levelDesign;

	private CollisionFlags flags;
#pragma warning disable 0649
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject eyeLeft;
    [SerializeField] private GameObject eyeRight;
    [SerializeField] private GameObject footLeft;
    [SerializeField] private GameObject footRight;
    [SerializeField] private GameObject handLeft;
    [SerializeField] private GameObject handRight;
    [SerializeField] private GameObject hair;
    [SerializeField] private GameObject hat;
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject wizardModel;
#pragma warning restore 0649

    private GameObject staff;
    private GlobalGameManager ggm;
    private LevelController lc;
    private Animator anim;
    private Rigidbody playerRB;
    private Transform playerTransform;
    private Vector3 rcOrigin;
    private Vector3 rcOriginOffset = new Vector3 (0, 0.05f, 0);
    private float gravity;
	private float jumpHeight;
	private float maxSpeedBonus;
    private float maxVelocity;
    private float minVelocity;
    private float acceleration;
    private float direction = 0;
    private float fallMult;
    private float lowJumpMult;
    private int groundedLayerMask;
    private bool isGrounded;
    private bool crashed;
    private bool canJump;
	private bool doubleJump;
    private bool freeMovement;
    private bool moveHorizontal;
    private bool moveHorizontalPC;

    private void Awake()
    {
        rcOrigin = new Vector3();
        playerRB = GetComponent<Rigidbody>();
        playerTransform = this.transform;
        // Layers: Default, SpellBeneficial, Obstacle, hiddenObstacle
        groundedLayerMask = (1 << 0) | (1 << 11) | (1 << 16) | (1 << 17);
        crashed = false;
        moveHorizontal = false;
    }

    void Start ()
    {
        Input.multiTouchEnabled = true;
        ggm = GlobalGameManager.instance;
        lc = GameObject.Find("LevelController").GetComponent<LevelController>();
        staff = ggm.GetStaff();
        Instantiate(staff, handRight.transform); 
        PopulateSkins();
        maxVelocity = ggm.GetMaxVelocity();
        minVelocity = ggm.GetMinVelocity();
        acceleration = ggm.GetAcceleration();
        gravity = ggm.GetGravity();
        jumpHeight = ggm.GetJumpHeight ();
        freeMovement = ggm.GetFreeMovement();
        lowJumpMult = ggm.GetLowJumpMult();
        fallMult = ggm.GetFallMult();
        anim = this.transform.GetChild(0).GetComponent<Animator>();
        Physics.gravity = new Vector3(0.0f, ggm.GetGravity(), 0.0f);
        // May need to turn into switch
        if (!freeMovement)
        {
            playerRB.constraints = RigidbodyConstraints.FreezePositionX;
        }
        // Part of PopulateSkins, have to turn off and turn on to reset the animator controller with the skins.
        wizardModel.SetActive(true);
 	}

	void Update ()
    {
        if (LevelController.isActive)
        {
            if (Input.GetButtonDown("Vertical") && !crashed)
            {

                JumpButtonDown();
            }
            else if (!Input.GetButton("Vertical") && playerRB.velocity.y > 0)
            {
                JumpButtonUp();
            }
            // Adjusts the wizard's animation & speed each frame
        }
        AnimationMovementSpeed();
    }

	void FixedUpdate ()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            direction = Input.GetAxisRaw("Horizontal");
            moveHorizontalPC = true;
        }
        else
        {
            moveHorizontalPC = false;
        }

        if (LevelController.isActive)
        {
            if ((moveHorizontal || moveHorizontalPC) && freeMovement && !crashed)
            {
                HorizontalMovement(direction);
            }
            else
            {
                if (isGrounded && playerRB.velocity.x != 0)
                {
                    SlowDown();
                }
            }
        }
        // Sometimes at the start, collisions send the wizard moving around.
        // This ensures he's stationary until the level is active.
        if (!LevelController.isActive && playerRB.velocity.sqrMagnitude != 0)
        {
            playerRB.velocity = Vector3.zero;
            anim.SetBool("Walking", false);
        }
        if (playerRB.velocity.y < 0 && playerRB.velocity.y > -25.0f) {
            playerRB.AddForce(0, -fallMult, 0, ForceMode.Acceleration);
        }
        if (playerRB.velocity.y < -25)
        {
            playerRB.AddForce(0, -gravity, 0, ForceMode.Acceleration);
        }
    }

	private void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Floor" || other.gameObject.tag == "HiddenFloor")
        {
            isGrounded = true;
            doubleJump = true;
            anim.SetBool("Jumping", false);
            anim.ResetTrigger("DoubleJump");
        }
        if (other.gameObject.tag == "Wisp")
        {
            // TODO: Make sure to set the value for the wisp received
            other.gameObject.GetComponent<Wisp>().CollectWisp();
        }
        if (other.gameObject.name == "FinishLine" && LevelController.isActive)
        {
            lc.EndLevel();
        }
	}

    private void OnTriggerExit(Collider other)
    {
        // When going from tile to tile, it considers it an exit event.
        if ((other.gameObject.tag == "Floor" 
            || other.gameObject.tag == "HiddenFloor") 
            && playerRB.velocity.y > 0.01f)
        {
            isGrounded = false;
            // Starts handling the wizard if he accidentally hit another collider on his first jump.
        }
    }


    #region Jumping
    // This function is also called externally by the InGameUIManager JumpButton
    public void JumpButtonDown ()
    {
		canJump = AbleToJump();
		if (canJump && !crashed)
        {
            playerRB.velocity = new Vector3(playerRB.velocity.x, jumpHeight, playerRB.velocity.z);
			canJump = false;
            anim.SetBool("Jumping", true);
        }
    }

    private bool AbleToJump()
    {
        rcOrigin = playerTransform.position + rcOriginOffset;
        if (Physics.Raycast(rcOrigin, Vector3.down, 0.3f, groundedLayerMask))
        {
            return true;
        }
        if (doubleJump)
        {
            doubleJump = false;
            anim.SetTrigger("DoubleJump");
            return true;
        }
        return false;
    }

    public void JumpButtonUp()
    {
        if (playerRB.velocity.y > 0 && playerRB.velocity.y > -25)
        {
            playerRB.AddForce(0, -lowJumpMult *2, 0, ForceMode.Acceleration);
        }
    }
    #endregion Jumping


    #region Movement
    // Called by UI in the inspector RightButton & JumpRight
    public void RightMovementDown()
    {
        direction = 1.0f;
        moveHorizontal = true;
    }
    public void RightMovementUp()
    {
        moveHorizontal = false;
    }

    // Called by UI in the inspector LeftButton & JumpLeft
    public void LeftMovementDown()
    {
        direction = -1.0f;
        moveHorizontal = true;
    }
    public void LeftMovementUp()
    {
        moveHorizontal = false;
    }

    // Called by the movement button from InGameUIManager
    public void HorizontalMovement(float direction)
    {
        float playerVX = (playerRB.velocity.x);
        if (direction * playerVX < minVelocity)
        {
            playerRB.AddForce(Vector3.right * (Time.deltaTime * minVelocity * direction * 250.0f), ForceMode.Acceleration);
        }
        if (playerVX >= minVelocity || playerVX <= -minVelocity)
        {
            playerRB.AddForce(Vector3.right * (Time.deltaTime * acceleration * (direction * maxVelocity - playerVX)), ForceMode.Acceleration);
        }
    }

    private void PopulateSkins()
    {
        GameObject skin = ggm.GetPlayerBodySkin();
        PopulateSkinsHelper(skin);
        skin = ggm.GetPlayerFeetSkin();
        PopulateSkinsHelper(skin);
        skin = ggm.GetPlayerHandsSkin();
        PopulateSkinsHelper(skin);
        skin = ggm.GetPlayerHeadSkin();
        PopulateSkinsHelper(skin);
        wizardModel.SetActive(false);
    }
    private void PopulateSkinsHelper(GameObject skinPrefab)
    {
        GameObject skin = Instantiate(skinPrefab);
        int childcount = skin.transform.childCount;
        for (int i = 0; i < childcount; i++)
        {
            string childName = skin.transform.GetChild(0).name.ToLower();
            if (childName.Contains("body") || childName.Contains("chest") || childName.Contains("robe"))
            {
                skin.transform.GetChild(0).transform.SetParent(body.transform, false);
            }
            else if (childName.Contains("hand") && childName.Contains("l"))
            {
                skin.transform.GetChild(0).transform.SetParent(handLeft.transform, false);
            }
            else if (childName.Contains("hand") && childName.Contains("r"))
            {
                skin.transform.GetChild(0).transform.SetParent(handRight.transform, false);
            }
            else if ((childName.Contains("foot") && childName.Contains("l")) ||
                (childName.Contains("feet") && childName.Contains("l")))
            {
                skin.transform.GetChild(0).transform.SetParent(footLeft.transform, false);
            }
            else if ((childName.Contains("foot") && childName.Contains("r")) ||
                (childName.Contains("feet") && childName.Contains("r")))
            {
                skin.transform.GetChild(0).transform.SetParent(footRight.transform, false);
            }
            else if (childName.Contains("hair") || childName.Contains("beard"))
            {
                skin.transform.GetChild(0).transform.SetParent(hair.transform, false);
            }
            else if (childName.Contains("hat") || childName.Contains("helmet"))
            {
                skin.transform.GetChild(0).transform.SetParent(hat.transform, false);
            }
            else if (childName.Contains("head"))
            {
                skin.transform.GetChild(0).transform.SetParent(head.transform, false);
            }
            else if (childName.Contains("eye") && childName.Contains("l"))
            {
                skin.transform.GetChild(0).transform.SetParent(eyeLeft.transform, false);
            }
            else if (childName.Contains("eye") && childName.Contains("r"))
            {
                skin.transform.GetChild(0).transform.SetParent(eyeRight.transform, false);
            }
            else
                Debug.Log("go.child0.name: " + skin.transform.GetChild(0).name);
        }
    }

    private void SlowDown()
    {
        if (playerRB.velocity.x < 1 && playerRB.velocity.x > -1)
        {
            playerRB.velocity = new Vector3(0, playerRB.velocity.y, playerRB.velocity.z);
            anim.SetBool("Walking", false);
            return;
        }
        float negDirection = (playerRB.velocity.x > 0) ? -1 : 1;
        playerRB.AddForce(Vector3.right * negDirection * Time.deltaTime * 1000, ForceMode.Acceleration);
    }

    // Called by Obstacle script when collided with an object
    public void SetPlayerVelocity(float penalty)
    {
        float velocityMagnitude = playerRB.velocity.magnitude;
        velocityMagnitude = (minVelocity * penalty) + (velocityMagnitude * (1 - penalty));
        // If the crash was too fast, run the animation and turn off activities.
        if (!crashed)
        {
            if ((playerRB.velocity.x - velocityMagnitude) > maxVelocity * 0.3f)
            {
                crashed = true;
                anim.SetTrigger("Crashed");
                anim.SetBool("GetUp", false);
                StartCoroutine("Crash");
            }
            else
            {
                crashed = true;
                anim.SetTrigger("Crashed");
                playerRB.velocity = playerRB.velocity.normalized * velocityMagnitude;
                StartCoroutine("Stumble");
            }
        }
    }

    private void IncreaseVelocity()
    {
        if (playerRB.velocity.x < maxVelocity)
        {
            playerRB.AddForce(Vector3.right * acceleration);
        }
        if (playerRB.velocity.x > maxVelocity)
        {
            playerRB.velocity = Vector3.right * maxVelocity;
        }
    }
    #endregion Movement

    // Called every frame in Update
    private void AnimationMovementSpeed()
    {
        float curSpeed = playerRB.velocity.x;
        if (curSpeed < 0.1f && curSpeed > -0.1f) {
            // Make sure it's idle
            anim.SetBool("Walking", false);
            return;
        }
        if (curSpeed >= -10 && curSpeed <= 10)
        {
            anim.SetBool("Walking", true);
            anim.SetBool("Running", false);
            curSpeed = curSpeed * 0.5f;
            anim.SetFloat("WalkSpeed", curSpeed);
            return;
        }
        if (curSpeed > 10 || curSpeed < -10)
        {
            anim.SetBool("Running", true);
            curSpeed = (curSpeed + (0.03f * curSpeed) - 0.3f) * (0.1f);
            anim.SetFloat("RunSpeed", curSpeed);
            return;
        }
    }

    // called as a coroutine by setplayer velocity
    private IEnumerator Crash()
    {
        while (Mathf.Abs(playerRB.velocity.x) > 4.0f)
        {
            yield return new WaitForSeconds(0.05f);
        }
        anim.SetBool("Settle", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("GetUp", true);
        yield return new WaitForSeconds(1.2f);
        crashed = false;
        anim.SetBool("Settle", false);
        yield break;
    }

    // called as a coroutine by setplayer velocity
    private IEnumerator Stumble()
    {
        yield return new WaitForSeconds (0.1f);
        crashed = false;
        yield break;
    }
}
	 