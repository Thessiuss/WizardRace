using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAndStaffPrefabs : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private List<GameObject> spellPrefabs;
    [SerializeField] private List<GameObject> staffPrefabs;
    [SerializeField] private int indexOfSelectedSpell;
#pragma warning restore 0649

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("SelectedStaff"))
        {
            indexOfSelectedSpell = 0;
        }
        else
        {
            indexOfSelectedSpell = PlayerPrefs.GetInt("SelectedStaff");
        }
    }

    public int GetIndexOfSelectedSpell() {
        return indexOfSelectedSpell;
    }
    public void SetIndexOfSelectedSpell(int index) {
        PlayerPrefs.SetInt("SelectedStaff", indexOfSelectedSpell);
        indexOfSelectedSpell = index;
    }
    public GameObject GetStaffPrefab(int index)
    {
        return staffPrefabs [index];
    }
    public int GetStaffPrefabCount()
    {
        return staffPrefabs.Count;
    }
    public List<GameObject> GetStaffPrefabList()
    {
        return staffPrefabs;
    }
    public GameObject GetSpellPrefab(int index)
    {
        return spellPrefabs [index];
    }
}
