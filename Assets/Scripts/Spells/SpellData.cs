using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellData : MonoBehaviour {
    public GameObject staff;
    public GameObject spell;
    public float spellMultiplier;
    public float spellMultiplierCap;

    // index0 is always spell level
    public List<float> spellValues;
}
