using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Make sure the Staff Names are properly punctuated in the Inspector: We use gameObject.name
public class OWStaffUIController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject activeStaff;
    [SerializeField] private GameObject activeSpell;
    [SerializeField] private GameObject selectText;
    [SerializeField] private Image selectBase;
    [SerializeField] private GameObject checkMark;
    [SerializeField] private SpellAndStaffPrefabs spellAndStaffPrefabs;
    [SerializeField] private Text staffDescriptionHeader;
#pragma warning restore 0649
    private GlobalGameManager ggm;
    private int activeStaffIndex;
    // TODO: Get active spell from ClientData on Awake. SetActiveSpell when selected


    private void Awake()
    {
        ggm = GlobalGameManager.instance;
    }

    private void OnEnable()
    {
        CanvasEnabled();
    }

    private void OnDisable()
    {
        DisableSpell(activeStaffIndex);
        DisableStaff(activeStaffIndex);
    }

    public void CanvasEnabled()
    {
        activeStaffIndex = spellAndStaffPrefabs.GetIndexOfSelectedSpell();
        PopulateSelectButton(true);
        EnableStaff(activeStaffIndex);
        EnableSpell(activeStaffIndex);
        activeStaff.SetActive(true);
        activeSpell.SetActive(true);
        gameObject.GetComponent<Canvas>().enabled = true;
    }

    public void LeftButton()
    {
        // TODO: make arrow green if there's one left or right. otherwise make it gray.
        if (activeStaffIndex > 0)
        {
            DisableStaff(activeStaffIndex);
            DisableSpell(activeStaffIndex);
            activeStaffIndex = activeStaffIndex - 1;
            if (activeStaffIndex == spellAndStaffPrefabs.GetIndexOfSelectedSpell())
            {
                PopulateSelectButton(true);
            }
            else
                PopulateSelectButton(false);
            EnableStaff(activeStaffIndex);
            EnableSpell(activeStaffIndex);
        }
    }

    public void RightButton()
    {
        // TODO: Check if spell is selected, if so, set things appropriately
        if (activeStaffIndex < (activeStaff.transform.childCount - 1))
        {
            DisableStaff(activeStaffIndex);
            DisableSpell(activeStaffIndex);
            activeStaffIndex = activeStaffIndex + 1;
            if (activeStaffIndex == spellAndStaffPrefabs.GetIndexOfSelectedSpell())
            {
                PopulateSelectButton(true);
            }
            else
                PopulateSelectButton(false);
            EnableStaff(activeStaffIndex);
            EnableSpell(activeStaffIndex);
        }
    }

    public void SelectButton()
    {
        spellAndStaffPrefabs.SetIndexOfSelectedSpell(activeStaffIndex);
        PopulateSelectButton(true);
    }

    private void PopulateSelectButton(bool isSelected)
    {
        if (isSelected)
        {
            selectBase.color = new Color32(59, 234, 70, 255);
            selectText.SetActive(false);
            checkMark.SetActive(true);
            spellAndStaffPrefabs.SetIndexOfSelectedSpell(activeStaffIndex);
            ggm.SetStaff(spellAndStaffPrefabs.GetStaffPrefab(activeStaffIndex));
            ggm.SetSpell(spellAndStaffPrefabs.GetSpellPrefab(activeStaffIndex));
        }
        else
        {
            selectBase.color = new Color32(255, 255, 255, 255);
            checkMark.SetActive(false);
            selectText.SetActive(true);
        }
    }

    private void DisableStaff(int index)
    {
        activeStaff.transform.GetChild(index).gameObject.SetActive(false);
    }

    private void DisableSpell(int index)
    {
        activeSpell.transform.GetChild(index).gameObject.SetActive(false);

    }

    private void EnableStaff(int index)
    {
        staffDescriptionHeader.text = activeStaff.transform.GetChild(index).gameObject.name;
        activeStaff.transform.GetChild(index).gameObject.SetActive(true);
    }

    private void EnableSpell(int index)
    {
        activeSpell.transform.GetChild(index).gameObject.SetActive(true);
    }

    public void ExitButton()
    {
        // Have to turn off the Staff and the ActiveStaff Parent
        activeStaff.SetActive(false);
        activeSpell.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
