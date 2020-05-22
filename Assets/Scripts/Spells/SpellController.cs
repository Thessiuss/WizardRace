using UnityEngine;
using UnityEngine.EventSystems;

public class SpellController : MonoBehaviour
{
	// ----------Test variables--------------- //
	// ---------------------------------------- //
    public static bool isCasting;
    
    private Touch touch;
    private GameObject spellPrefab;
    private GameObject spellArea;
	private GameObject activeSpell;
	private GameObject activeSpellPrefab;
    private GlobalGameManager ggm;
    private ObjectPool spellPool;
	private Ray ray;
	private Vector3 hitPos;
    private Vector3 mousePos;
	private int validTouchIndex;
    private int temp;
    private int spellFingerID;
    private int layerMask;
    private bool validTouch = false;
    private bool uiBlocking = false;


	void Awake ()
    {
        ggm = GlobalGameManager.instance;
	}


	void Start ()
    {
        Input.multiTouchEnabled = true;
        activeSpellPrefab = ggm.GetSpell();
        spellArea = GameObject.FindWithTag ("SpellArea");
        PopulatePool();
        validTouch = false;
	}


	void Update ()
    {        
        if (LevelController.isActive)
        {
            // ----------------- PC code ---------------------- //
            if (!ggm.mobileBuild)
            {
                if (Input.GetMouseButtonDown(0) && !isCasting)
                {
                    hitPos = SpellCastCheck(Input.mousePosition);
                    uiBlocking = EventSystem.current.IsPointerOverGameObject();
                    if (hitPos != Vector3.zero && !uiBlocking)
                    {
                        GetSpell();
                    }
                }
                // Mouse moved so we check the new position and adjust.
                if (isCasting && (mousePos != Input.mousePosition))
                {
                    if (activeSpell == null)
                    {
                        GetSpell();
                    }
                    Vector3 pos = SpellCastCheck(Input.mousePosition);
                    if (pos != Vector3.zero)
                    {
                        // Use a move toward
                        activeSpell.transform.position = pos;
                        mousePos = Input.mousePosition;
                    }
                }
                if (Input.GetMouseButtonUp(0) && isCasting)
                {
                    TriggerSpell();
                    isCasting = false;
                    ScoreController.isCasting = false;
                }
            }
        }

        // --------------- Mobile Code --------------------- //
        if (Input.touchCount > 0 && ggm.mobileBuild)
        {
            // TODO: Make sure the appropriate touch event is then checked
            // TODO: Move the spell relative to the position of the finger
            // if it moves outside of the collider, fire spell & set validtouch false
            validTouch = CheckTouches();
            if (validTouch)
            {
                // !EventSystem.current.IsPointerOverGameObject();
                switch (Input.GetTouch(validTouchIndex).phase)
                {
                    case TouchPhase.Began:
                        //TODO: Spawn the selected spell ahead of the finger touch
                        GetSpell();
                        break;
                    case TouchPhase.Moved:
                        Vector3 pos = SpellCastCheck(Input.GetTouch(validTouchIndex).position);
                        if (activeSpell == null)
                        {
                            GetSpell();
                        }
                        if (pos != Vector3.zero && isCasting)
                        {
                            activeSpell.transform.position = pos;
                            break;
                        }
                        else if (pos != Vector3.zero && !isCasting)
                        {
                            GetSpell();
                            break;
                        }
                        else
                        {
                            CheckTouches();
                        }
                        break;           
                    case TouchPhase.Ended:
                        TriggerSpell();
                        isCasting = false;
                        ScoreController.isCasting = false;
                        validTouch = false;
                        spellFingerID = -1;
                        break;
                    case TouchPhase.Canceled:
                        validTouch = CheckTouches();
                        if (!validTouch)
                        {
                            TriggerSpell();
                            isCasting = false;
                            ScoreController.isCasting = false;
                        }
                        break;
                }
            }
		}
	}

    private bool CheckTouches()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).fingerId == spellFingerID)
            {
                return true;
            }
            hitPos = SpellCastCheck(Input.GetTouch(i).position);
            if (hitPos != Vector3.zero)
            {
                validTouchIndex = i;
                spellFingerID = Input.GetTouch(i).fingerId;
                return true;
            }
        }
        spellFingerID = -1;
        return false;
    }

    private void PopulatePool() {
        spellPool = ScriptableObject.CreateInstance<ObjectPool>();
        spellPool.Populate(5, activeSpellPrefab, this.gameObject, true);
    }

    // Checks the position the touch is sent on the UI/Phone and returns the real world point only if it's inside the spell area
    // TODO Should use RaycastHit as it does not go on for infinity
    // TODO: Getting a Ray (position & direction) at the point where it was touched
    public Vector3 SpellCastCheck (Vector2 position)
    {
		Ray ray = Camera.main.ScreenPointToRay (position);
		RaycastHit hitInfo;
        layerMask = 1 << 8;
		if (Physics.Raycast (ray, out hitInfo, 100.0f, layerMask))
        {
			bool inside = PointInSphere (spellArea.transform.position, hitInfo.point, 40f);
			if (hitInfo.collider.tag == "SpellArea" && inside)
            {
				return hitInfo.point;
			} 
		}
		return Vector3.zero;
	}

    public bool PointInSphere(Vector3 center, Vector3 point, float radius)
    {
        bool inside;
        // quicker to compare squares rather than square roots
        inside = (center - point).sqrMagnitude < Mathf.Pow(radius, 2);
        return inside;
    }

    private void GetSpell()
    {
        activeSpell = spellPool.GetPooledObject(this.gameObject);
        activeSpell.GetComponent<Rigidbody>().velocity = Vector3.zero;
        activeSpell.transform.position = hitPos;
        // May need to change the child space
        activeSpell.SetActive(true);
        isCasting = true;
        mousePos = Input.mousePosition;
        ScoreController.isCasting = true;
    }

	// This handles the appropriate script from the active spell
	public void TriggerSpell ()
    {
		// TODO: Use a Try/Catch for this in case there's an error
		switch (activeSpell.gameObject.name)
        {
		    case "FireBall(Clone)":
			    activeSpell.GetComponent<Spell_Fireball>().TriggerSpell();
			    break;
            case "SpeedBoost(Clone)":
                activeSpell.GetComponent<Spell_SpeedBoost>().TriggerSpell();
                break;
            case "Redstone(Clone)":
                activeSpell.GetComponent<Spell_RedStone>().TriggerSpell();
                break;
            case "SunFlower(Clone)":
                activeSpell.GetComponent<Spell_Sunflower>().TriggerSpell();
                break;
            case "RawCrystal(Clone)":
                activeSpell.GetComponent<Spell_RawCrystal>().TriggerSpell();
                break;
		}
	}
}
