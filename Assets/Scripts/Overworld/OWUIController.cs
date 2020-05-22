using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OWUIController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject alchemyPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject levelInfoPanel;
    [SerializeField] private GameObject playerPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject spinnerPanel;
    [SerializeField] private GameObject staffPanel;
    [SerializeField] private Text rightPanelLevelText;
    [SerializeField] private TextMeshProUGUI smallBlue;
    [SerializeField] private TextMeshProUGUI smallGreen;
    [SerializeField] private TextMeshProUGUI smallPurple;
    [SerializeField] private TextMeshProUGUI smallRed;
    [SerializeField] private TextMeshProUGUI smallYellow;
    [SerializeField] private TextMeshProUGUI mediumBlue;
    [SerializeField] private TextMeshProUGUI mediumGreen;
    [SerializeField] private TextMeshProUGUI mediumPurple;
    [SerializeField] private TextMeshProUGUI mediumRed;
    [SerializeField] private TextMeshProUGUI mediumYellow;
    [SerializeField] private TextMeshProUGUI largeBlue;
    [SerializeField] private TextMeshProUGUI largeGreen;
    [SerializeField] private TextMeshProUGUI largePurple;
    [SerializeField] private TextMeshProUGUI largeRed;
    [SerializeField] private TextMeshProUGUI largeYellow;
    [SerializeField] private TextMeshProUGUI hugeBlue;
    [SerializeField] private TextMeshProUGUI hugeGreen;
    [SerializeField] private TextMeshProUGUI hugePurple;
    [SerializeField] private TextMeshProUGUI hugeRed;
    [SerializeField] private TextMeshProUGUI hugeYellow;
#pragma warning restore 0649
    private Dictionary<string, int> crystalInventory;
    private PlayFabData pfd;
    private ProgressData progData;
    private OWPlayerController pc;
    private bool finishedPopulating;                        // Not used just yet.
    private int levelCount;                                 // How many total levels there are.

    // TODO: On awake, check which wand is selected and set the according Prefab and Staff Panel
    // TODO: If there was a server time out, retry populating all data.
    private void Awake()
    {
        levelInfoPanel.gameObject.SetActive(false);
        crystalInventory = new Dictionary<string, int>()
        {
            {"SmallBlue", 0 },
            {"SmallGreen", 0 },
            {"SmallPurple", 0 },
            {"SmallRed", 0 },
            {"SmallYellow", 0 },
            {"MediumBlue", 0 },
            {"MediumGreen", 0 },
            {"MediumPurple", 0 },
            {"MediumRed", 0 },
            {"MediumYellow", 0 },
            {"LargeBlue", 0 },
            {"LargeGreen", 0 },
            {"LargePurple", 0 },
            {"LargeRed", 0 },
            {"LargeYellow", 0 },
            {"HugeBlue", 0 },
            {"HugeGreen", 0 },
            {"HugePurple", 0 },
            {"HugeRed", 0 },
            {"HugeYellow", 0}
        };
        ToggleUI(true);
    }

    private void Start()
    {
        progData = GameObject.Find("Data/ClientData").GetComponent<ProgressData>();
        pc = playerPanel.GetComponent<OWPlayerController>();
        pfd = GameObject.Find("GlobalGameManager/PlayFabController/PlayFabDataController").GetComponent<PlayFabData>();
        levelCount = GameObject.Find("LevelDesign/Levels").transform.childCount;
        ToggleUI(false);
    }

    private void SetCrystalQuantities()
    {
        smallBlue.text = crystalInventory["SmallBlue"].ToString();
        smallGreen.text = crystalInventory["SmallGreen"].ToString();
        smallPurple.text = crystalInventory["SmallPurple"].ToString();
        smallRed.text = crystalInventory["SmallRed"].ToString();
        smallYellow.text = crystalInventory["SmallYellow"].ToString();
        mediumBlue.text = crystalInventory["MediumBlue"].ToString();
        mediumGreen.text = crystalInventory["MediumGreen"].ToString();
        mediumPurple.text = crystalInventory["MediumPurple"].ToString();
        mediumRed.text = crystalInventory["MediumRed"].ToString();
        mediumYellow.text = crystalInventory["MediumYellow"].ToString();
        largeBlue.text = crystalInventory["LargeBlue"].ToString();
        largeGreen.text = crystalInventory["LargeGreen"].ToString();
        largePurple.text = crystalInventory["LargePurple"].ToString();
        largeRed.text = crystalInventory["LargeRed"].ToString();
        largeYellow.text = crystalInventory["LargeYellow"].ToString();
        hugeBlue.text = crystalInventory["HugeBlue"].ToString();
        hugeGreen.text = crystalInventory["HugeGreen"].ToString();
        hugePurple.text = crystalInventory["HugePurple"].ToString();
        hugeRed.text = crystalInventory["HugeRed"].ToString();
        hugeYellow.text = crystalInventory["HugeYellow"].ToString();
    }

    private void ToggleUI(bool toggle)
    {
        alchemyPanel.SetActive(toggle);
        inventoryPanel.SetActive(toggle);
        levelInfoPanel.SetActive(toggle);
        playerPanel.SetActive(toggle);
        settingsPanel.SetActive(toggle);
        spinnerPanel.SetActive(toggle);
        staffPanel.SetActive(toggle);
}

    public void OnStaffButton()
    {
        staffPanel.SetActive(true);
    }

    public void OnAlchemyButton()
    {
        alchemyPanel.SetActive(true);
    }

    public void OnInventoryButton()
    {
        crystalInventory = progData.GetCrystalInventory();
        SetCrystalQuantities();
        inventoryPanel.SetActive(true);
    }

    public void OnPlayerButton()
    {
        playerPanel.SetActive(true);
        pc.OnPlayerControllerEnable();
    }

    public void OnResetPlayerData()
    {
        PlayerPrefs.DeleteAll();
        for (int i = 0; i < levelCount; i++)
        {
            pfd.OnUpdateLevelCompletionData("Level " + (i + 1), string.Empty);
        }

    }

    public void OnSpinnerButton()
    {
        spinnerPanel.SetActive(true);
    }

    public void ExitButton(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void OnActivate(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void OnLevelClick(LevelData ld)
    {
        rightPanelLevelText.text = ld.GetLevelName();
        levelInfoPanel.gameObject.SetActive(true);
    }

    public void CloseLevelInfoPanel()
    {
        levelInfoPanel.gameObject.SetActive(false);
    }

    
}
