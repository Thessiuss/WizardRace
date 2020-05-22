using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class OWPlayerController : MonoBehaviour
{
    // NOTE: This script fires before OWSkinController
    // TODO: update the list when someone acquires a staff (a staff is made).
#pragma warning disable 0649
    [SerializeField] private GameObject bodyLoc;
    [SerializeField] private GameObject bodyPrefab;
    [SerializeField] private GameObject bodySkinUI;
    [SerializeField] private GameObject bubbleCardPrefab;
    [SerializeField] private GameObject eyeLeftLoc;
    [SerializeField] private GameObject eyeRightLoc;
    [SerializeField] private GameObject feetPrefab;
    [SerializeField] private GameObject feetSkinUI;
    [SerializeField] private GameObject footLeftLoc;
    [SerializeField] private GameObject footRightLoc;
    [SerializeField] private GameObject hairLoc;
    [SerializeField] private GameObject handLeftLoc;
    [SerializeField] private GameObject handRightLoc;
    [SerializeField] private GameObject handsPrefab;
    [SerializeField] private GameObject handsSkinUI;
    [SerializeField] private GameObject hatLoc;
    [SerializeField] private GameObject headLoc;
    [SerializeField] private GameObject headPrefab;
    [SerializeField] private GameObject headSkinUI;
    [SerializeField] private GameObject necklaceSkinUI;
    [SerializeField] private GameObject skinSelectContent;
    [SerializeField] private GameObject staffSkinUI;
#pragma warning restore 0649
    private bool bodySelected;
    private bool feetSelected;
    private bool handsSelected;
    private bool headSelected;
    private GameObject leftArrowSS;
    private GameObject rightArrowSS;        // Skin Select Panel Right Arrow
    private List<GameObject> staffPrefabs = new List<GameObject>();
    private List<GameObject> availableHeadSkins;
    private List<GameObject> availableFeetSkins;
    private List<GameObject> availableHandSkins;
    private List<GameObject> availableBodySkins;
    private ObjectPool bubbleCardPool;
    private OWSkinController owsc;
    private RectTransform skinContentRT;
    private SpellAndStaffPrefabs sasf;
    private TextMeshProUGUI skinPanelHeader;
    private int activeCards;
    private int bodyIndex;
    private int feetIndex;
    private int handsIndex;
    private int headIndex;
    private int staffIndex;

    private void Awake()
    {
        sasf = GameObject.Find("Data/ClientData").GetComponent<SpellAndStaffPrefabs>();
        owsc = GameObject.Find("Data/ClientData").GetComponent<OWSkinController>();
        leftArrowSS = GameObject.Find("UICanvas1/PlayerPanel/SkinSelectionPanel/LeftArrow");
        rightArrowSS = GameObject.Find("UICanvas1/PlayerPanel/SkinSelectionPanel/RightArrow");
        skinPanelHeader = GameObject.Find("UICanvas1/PlayerPanel/SkinSelectionPanel/ParchmentPanel/Header/HeaderText")
            .GetComponent<TextMeshProUGUI>();
        activeCards = 1;
        bodySelected = false;
        feetSelected = false;
        handsSelected = false;
        headSelected = false;
        skinContentRT = skinSelectContent.GetComponent<RectTransform>();
        InstantiateStaffs();
        GetAvailableSkins();
        InstantiateSkins(availableBodySkins, bodyPrefab, bodySkinUI);
        InstantiateSkins(availableFeetSkins, feetPrefab, feetSkinUI);
        InstantiateSkins(availableHandSkins, handsPrefab, handsSkinUI);
        InstantiateSkins(availableHeadSkins, headPrefab, headSkinUI);
        GetAndToggleAllSkins(false);
        PopulateBubbleCardPool();
    }

    private void Update()
    {
        if (skinContentRT.localPosition.x > -509 && leftArrowSS.activeInHierarchy)
        {
            leftArrowSS.SetActive(false);
        }
        if (skinContentRT.localPosition.x <= -509 && !leftArrowSS.activeInHierarchy)
        {
            leftArrowSS.SetActive(true);
        }
        if (skinContentRT.localPosition.x < -510 * (activeCards-1) + 509 && rightArrowSS.activeInHierarchy)
        {
            rightArrowSS.SetActive(false);
        }
        if (skinContentRT.localPosition.x >= -510 * (activeCards-1) + 509 && !rightArrowSS.activeInHierarchy)
        {
            rightArrowSS.SetActive(true);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < staffPrefabs.Count; i++)
        {
            staffPrefabs[i].SetActive(false);
        }
        GetAndToggleAllSkins(false);
    }

    private void InstantiateSkins(List<GameObject> availableSkins, GameObject prefab, GameObject parent)
    {
        // TODO: Destroy current instantiated skins
        // TODO: instantiate current skins.
        for (int i = 0; i < availableSkins.Count; i++)
        {
            GameObject heirarchyPrefab = Instantiate(prefab);
            heirarchyPrefab.transform.SetParent(parent.transform, false);
            GameObject go = Instantiate(availableSkins[i], this.transform);
            GameObject goUI = Instantiate(availableSkins[i], this.transform);
            // TODO: Activate the proper index
            foreach (Transform child in go.transform)
            {
                child.gameObject.SetActive(false);
            }
            int childCount = go.transform.childCount;       // Had to separate out. reparenting children won't finish
            for (int j=0; j < childCount; j++)
            {
                string childName = go.transform.GetChild(0).name.ToLower();
                if (childName.Contains("body") || childName.Contains("chest") || childName.Contains("robe"))
                {
                    go.transform.GetChild(0).transform.SetParent(bodyLoc.transform, false);
                    goUI.transform.GetChild(0).transform.SetParent(heirarchyPrefab.transform.GetChild(0).transform, false);
                }
                else if (childName.Contains("hand") && childName.Contains("l"))
                {
                    go.transform.GetChild(0).transform.SetParent(handLeftLoc.transform, false);
                    goUI.transform.GetChild(0).transform.SetParent(heirarchyPrefab.transform.GetChild(0).transform, false);
                }
                else if (childName.Contains("hand") && childName.Contains("r"))
                {
                    go.transform.GetChild(0).transform.SetParent(handRightLoc.transform, false);
                    goUI.transform.GetChild(0).transform.SetParent(heirarchyPrefab.transform.GetChild(1).transform, false);
                }
                else if (childName.Contains("foot") && childName.Contains("l") || 
                    (childName.Contains("feet") && childName.Contains("l")))
                {
                    go.transform.GetChild(0).transform.SetParent(footLeftLoc.transform, false);
                    goUI.transform.GetChild(0).transform.SetParent(heirarchyPrefab.transform.GetChild(0).transform, false);
                }
                else if (childName.Contains("foot") && childName.Contains("r") || 
                    (childName.Contains("feet") && childName.Contains("r")))
                {
                    go.transform.GetChild(0).transform.SetParent(footRightLoc.transform, false);
                    goUI.transform.GetChild(0).transform.SetParent(heirarchyPrefab.transform.GetChild(1).transform, false);
                }
                else if (childName.Contains("hair") || childName.Contains("beard"))
                {
                    go.transform.GetChild(0).transform.SetParent(hairLoc.transform, false);
                    goUI.transform.GetChild(0).transform.SetParent(heirarchyPrefab.transform.GetChild(0).transform, false);
                }
                else if (childName.Contains("hat") || childName.Contains("helmet"))
                {
                    go.transform.GetChild(0).transform.SetParent(hatLoc.transform, false);
                    goUI.transform.GetChild(0).transform.SetParent(heirarchyPrefab.transform.GetChild(1).transform, false);
                }
                else if (childName.Contains("head"))
                {
                    go.transform.GetChild(0).transform.SetParent(headLoc.transform, false);
                    goUI.transform.GetChild(0).transform.SetParent(heirarchyPrefab.transform.GetChild(2).transform, false);
                }
                else if (childName.Contains("eye") && childName.Contains("l"))
                {
                    go.transform.GetChild(0).transform.SetParent(eyeLeftLoc.transform, false);
                    goUI.transform.GetChild(0).transform.SetParent(heirarchyPrefab.transform.GetChild(3).transform, false);
                }
                else if (childName.Contains("eye") && childName.Contains("r"))
                {
                    go.transform.GetChild(0).transform.SetParent(eyeRightLoc.transform, false);
                    goUI.transform.GetChild(0).transform.SetParent(heirarchyPrefab.transform.GetChild(4).transform, false);
                }
                else
                    Debug.Log("go.child0.name: " + go.transform.GetChild(0).name);
            }
            heirarchyPrefab.SetActive(false);
            Destroy(go);
            goUI.transform.SetParent(heirarchyPrefab.transform, false);
            goUI.transform.SetSiblingIndex(0);
        }
    }

    private void InstantiateStaffs()
    {
        for (int i = 0; i < sasf.GetStaffPrefabCount(); i++)
        {
            GameObject go = Instantiate(sasf.GetStaffPrefab(i), handRightLoc.transform.GetChild(0).transform);
            staffPrefabs.Add(go);
            go.SetActive(false);
        }
    }

    private void GetAvailableSkins()
    {
        availableBodySkins = owsc.GetAvailableBodySkins();
        availableFeetSkins = owsc.GetAvailableFeetSkins();
        availableHandSkins = owsc.GetAvailableHandSkins();
        availableHeadSkins = owsc.GetAvailableHeadSkins();
    }

    private void GetAndToggleAllSkins(bool active)
    {
        int bodyRarity = GetAndToggleBodySkin(active);
        int feetRarity = GetAndToggleFeetSkin(active);
        int handsRarity = GetAndToggleHandsSkin(active);
        int headRarity = GetAndToggleHeadSkin(active);
    }

    private void PopulateBubbleCardPool()
    {
        bubbleCardPool = ScriptableObject.CreateInstance<ObjectPool>();
        bubbleCardPool.Populate(availableHeadSkins.Count, bubbleCardPrefab, skinSelectContent, true);
    }

    private void PopulateInitialBubbleCard(GameObject card, bool selected)
    {
        #region SelectButton
        Image selectBase = card.transform.GetChild(1).transform.GetChild(1).GetComponent<Image>();
        GameObject checkMark = card.transform.GetChild(1).transform.GetChild(2).gameObject;
        GameObject selectText = card.transform.GetChild(1).transform.GetChild(4).gameObject;
        if (selected)
        {
            selectBase.color = new Color32(59, 234, 70, 255);
            selectText.SetActive(false);
            checkMark.SetActive(true);
        }
        else
        {
            selectBase.color = new Color32(255, 255, 255, 255);
            checkMark.SetActive(false);
            selectText.SetActive(true);
        }
        #endregion SelectButton

    }

    public bool GetBodySelected()
    {
        return bodySelected;
    }
    public bool GetFeetSelected()
    {
        return feetSelected;
    }
    public bool GetHandsSelected()
    {
        return handsSelected;
    }
    public bool GetHeadSelected()
    {
        return headSelected;
    }

    public void OnArrowRight()
    {
        skinContentRT.localPosition += new Vector3 (-510, 0, 0);
    }
    public void OnArrowLeft()
    {
        skinContentRT.localPosition += new Vector3(510, 0, 0);
    }

    public void OnPlayerControllerEnable()
    {
        GetAvailableSkins();
        int i = sasf.GetIndexOfSelectedSpell();
        staffPrefabs[i].SetActive(true);
        GetAndToggleAllSkins(true);
    }

    #region SkinSelectionButtons
    public void OnSkinSelectionBodyEnable()
    {
        bodySelected = true;
        availableBodySkins = owsc.GetAvailableBodySkins();
        int currentBodySkin = owsc.GetBodyIndex();
        SkinSelectionEnableHandler(availableBodySkins, currentBodySkin, bodySkinUI, "Body Skin");
    }

    public void OnSkinSelectionFeetEnable()
    {
        feetSelected = true;
        availableFeetSkins = owsc.GetAvailableFeetSkins();
        int currentFeetSkin = owsc.GetFeetIndex();
        SkinSelectionEnableHandler(availableFeetSkins, currentFeetSkin, feetSkinUI, "Feet Skin");
    }

    public void OnSkinSelectionHandsEnable()
    {
        handsSelected = true;
        availableHandSkins = owsc.GetAvailableHandSkins();
        int currentHandSkin = owsc.GetHandsIndex();
        SkinSelectionEnableHandler(availableHandSkins, currentHandSkin, handsSkinUI, "Hands Skin");
    }

    public void OnSkinSelectionHeadEnable()
    {
        headSelected = true;
        availableHeadSkins = owsc.GetAvailableHeadSkins();
        int currentHeadSkin = owsc.GetHeadIndex();
        SkinSelectionEnableHandler(availableHeadSkins, currentHeadSkin, headSkinUI, "Head Skin");
        
    }

    // Too much copy/paste of code, populates the SkinSelectionPanel according to the proper
    private void SkinSelectionEnableHandler(List<GameObject> availableSkins, int currentSkinIndex, GameObject skinUI, string headerText)
    {
        activeCards = availableSkins.Count;             // NOTE: Allows me to adjust the Skin Selection Panel Arrows
        int count = availableSkins.Count;
        skinPanelHeader.text = headerText;
        this.transform.GetChild(0).gameObject.SetActive(false);
        this.transform.GetChild(1).gameObject.SetActive(false);
        this.transform.GetChild(2).gameObject.SetActive(true);
        //This needs to go after the other items are active, so that it can return the proper isActive.
        for (int i = 0; i < count; i++)
        {
            GameObject pooledObject = bubbleCardPool.GetPooledObject();
            pooledObject.SetActive(true);
            bool selected = (i == currentSkinIndex);
            PopulateInitialBubbleCard(pooledObject, selected);
            int skinRarity = skinUI.transform.GetChild(i + 1)
                .transform.GetChild(0).GetComponent<SkinConstraints>().skinRarity;
            SkinRarityColorChanger(pooledObject, skinRarity);
            if (skinRarity > 0)
            {
                pooledObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0)
                .gameObject.SetActive(true);
            }
            else
            {
                pooledObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0)
                .gameObject.SetActive(false);
            }
            Sprite skinSprite = skinUI.transform.GetChild(i + 1)
                .transform.GetChild(0).GetComponent<SkinConstraints>().sprite;
            pooledObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1)
                .GetComponent<Image>().sprite = skinSprite;
            string skinName = skinUI.transform.GetChild(i + 1)
                .transform.GetChild(0).GetComponent<SkinConstraints>().skinName;
            pooledObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2)
                .transform.GetChild(0).GetComponent<Text>().text = skinName;
        }
        // Sets the position of the scroll view
        skinContentRT.localPosition = new Vector3(-510 * currentSkinIndex, 0, 0);
    }

    // Changes the color of the Skin Selection UI buttons.
    private void SkinRarityColorChanger(GameObject skinUI, int skinRarity)
    {
        switch (skinRarity)
        {
            case 0:
                skinUI.GetComponent<Image>().color = new Color32(59, 234, 70, 255);
                break;
            case 1:
                skinUI.GetComponent<Image>().color = new Color32(72, 125, 222, 255);
                break;
            case 2:
                skinUI.GetComponent<Image>().color = new Color32(128, 0, 128, 255);
                break;
            case 3:
                skinUI.GetComponent<Image>().color = new Color32(255, 215, 0, 255);
                break;
        }
    }

    // TODO: Change this to a return button, and create a full out exit button
    public void OnSkinSelectionDisable()
    {
        bodySelected = false;
        feetSelected = false;
        handsSelected = false;
        headSelected = false;
        this.transform.GetChild(0).gameObject.SetActive(true);
        this.transform.GetChild(1).gameObject.SetActive(true);
        this.transform.GetChild(2).gameObject.SetActive(false);
        for (int i = 0; i < skinSelectContent.transform.childCount; i++)
        {
            skinSelectContent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    #endregion SkinSelectionButtons

    public int GetAndToggleBodySkin(bool active)
    {
        bodyIndex = owsc.GetBodyIndex();
        bodyLoc.transform.GetChild(bodyIndex).gameObject.SetActive(active);
        bodySkinUI.transform.GetChild(bodyIndex + 1).gameObject.SetActive(active);
        if (active)
        {
            int skinRarity = bodySkinUI.transform.GetChild(bodyIndex + 1)
                .transform.GetChild(0).GetComponent<SkinConstraints>().skinRarity;
            SkinRarityColorChanger(bodySkinUI, skinRarity);
            return skinRarity;
        }
        return -1;
    }

    public int GetAndToggleFeetSkin(bool active)
    {
        feetIndex = owsc.GetFeetIndex();
        footLeftLoc.transform.GetChild(feetIndex).gameObject.SetActive(active);
        footRightLoc.transform.GetChild(feetIndex).gameObject.SetActive(active);
        feetSkinUI.transform.GetChild(feetIndex + 1).gameObject.SetActive(active);
        if (active)
        {
            int skinRarity = feetSkinUI.transform.GetChild(feetIndex + 1)
                    .transform.GetChild(0).GetComponent<SkinConstraints>().skinRarity;
            SkinRarityColorChanger(feetSkinUI, skinRarity);
            return skinRarity;
        }
        return -1;
    }

    public int GetAndToggleHandsSkin(bool active)
    {
        handsIndex = owsc.GetHandsIndex();
        handLeftLoc.transform.GetChild(handsIndex).gameObject.SetActive(active);
        handRightLoc.transform.GetChild(handsIndex + 1).gameObject.SetActive(active);
        handsSkinUI.transform.GetChild(handsIndex + 1).gameObject.SetActive(active);
        if (active)
        {
            int skinRarity = handsSkinUI.transform.GetChild(handsIndex + 1)
                .transform.GetChild(0).GetComponent<SkinConstraints>().skinRarity;
            SkinRarityColorChanger(handsSkinUI, skinRarity);
            return skinRarity;
        }
        return -1;
    }

    public int GetAndToggleHeadSkin(bool active)
    {
        headIndex = owsc.GetHeadIndex();
        eyeLeftLoc.transform.GetChild(headIndex).gameObject.SetActive(active);
        eyeRightLoc.transform.GetChild(headIndex).gameObject.SetActive(active);
        hairLoc.transform.GetChild(headIndex).gameObject.SetActive(active);
        hatLoc.transform.GetChild(headIndex).gameObject.SetActive(active);
        headLoc.transform.GetChild(headIndex).gameObject.SetActive(active);
        headSkinUI.transform.GetChild(headIndex + 1).gameObject.SetActive(active);
        if (active)
        {
            int skinRarity = headSkinUI.transform.GetChild(headIndex + 1)
                .transform.GetChild(0).GetComponent<SkinConstraints>().skinRarity;
            SkinRarityColorChanger(headSkinUI, skinRarity);
            return skinRarity;
        }
        return -1;
    }
}
