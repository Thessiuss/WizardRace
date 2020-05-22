using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellInformation : MonoBehaviour
{
    // This class is for populating the UI with the accurate information pertaining to the spell.
    // SpellData passes along info through the GlobalGameManager, but this is used particularly for
    // Passing around needed information pertaining to a certain spell that is not useful for 
    // actually firing the spell.

    public List<string> description;
    public List<float> value;
}
