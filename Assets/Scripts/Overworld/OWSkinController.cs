using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OWSkinController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private List<GameObject> bodySkins;
    [SerializeField] private List<GameObject> feetSkins;
    [SerializeField] private List<GameObject> handSkins;
    [SerializeField] private List<GameObject> headSkins;
    [SerializeField] private int skinBodyIndex;
    [SerializeField] private int skinHandsIndex;
    [SerializeField] private int skinFeetIndex;
    [SerializeField] private int skinHairIndex;
    [SerializeField] private int skinHatIndex;
    [SerializeField] private int skinHeadIndex;
    [SerializeField] private int skinEyesIndex;
#pragma warning restore 0649
    private GlobalGameManager ggm;
    private int skinBodyCount;
    private int skinFeetCount;
    private int skinHandsCount;
    private int skinHeadCount;
    // TODO: Change the list here to false, then add according to PlayFab data
    private List<bool> ownedBodySkins = new List<bool>() { true, true, true, true, true, true };
    private List<bool> ownedFeetSkins = new List<bool>() { true, true, true, true, true, true };
    private List<bool> ownedHandSkins = new List<bool>() { true, true, true, true, true, true };
    private List<bool> ownedHeadSkins = new List<bool>() { true, true, true, true, true, true };

    private void Awake()
    {
        // TODO: Check from Playfab if they even have the skin available to them and populate in ownedSkins.
        if (!PlayerPrefs.HasKey("SkinBody"))
        {
            PlayerPrefs.SetInt("SkinBody", 0);
        }
        else
        {
            skinBodyIndex = PlayerPrefs.GetInt("SkinBody");
        }
        if (!PlayerPrefs.HasKey("SkinHands"))
        {
            PlayerPrefs.SetInt("SkinHands", 0);
        }
        else
        {
            skinHandsIndex = PlayerPrefs.GetInt("SkinHands");
        }
        if (!PlayerPrefs.HasKey("SkinFeet"))
        {
            PlayerPrefs.SetInt("SkinFeet", 0);
        }
        else
        {
            skinFeetIndex = PlayerPrefs.GetInt("SkinFeet");
        }
        if (!PlayerPrefs.HasKey("SkinHair"))
        {
            PlayerPrefs.SetInt("SkinHair", 0);
        }
        else
        {
            skinHairIndex = PlayerPrefs.GetInt("SkinHair");
        }
        if (!PlayerPrefs.HasKey("SkinHat"))
        {
            PlayerPrefs.SetInt("SkinHat", 0);
        }
        else
        {
            skinHatIndex = PlayerPrefs.GetInt("SkinHat");
        }
        if (!PlayerPrefs.HasKey("SkinHead"))
        {
            PlayerPrefs.SetInt("SkinHead", 0);
        }
        else
        {
            skinHeadIndex = PlayerPrefs.GetInt("SkinHead");
        }
        if (!PlayerPrefs.HasKey("SkinEyes"))
        {
            PlayerPrefs.SetInt("SkinEyes", 0);
            Debug.Log("doesn't have eyes key: ");
        }
        else
        {
            skinEyesIndex = PlayerPrefs.GetInt("SkinEyes");
        }
        if (!PlayerPrefs.HasKey("SkinBodyCount"))
        {
            PlayerPrefs.SetInt("SkinBodyCount", 2);
        }
        else
        {
            skinBodyCount = PlayerPrefs.GetInt("SkinFeetCount");
        }
        if (!PlayerPrefs.HasKey("SkinFeetCount"))
        {
            PlayerPrefs.SetInt("SkinFeetCount", 2);
        }
        else
        {
            skinFeetCount = PlayerPrefs.GetInt("SkinFeetCount");
        }
        if (!PlayerPrefs.HasKey("SkinHandsCount"))
        {
            PlayerPrefs.SetInt("SkinHandsCount", 2);
        }
        else
        {
            skinHandsCount = PlayerPrefs.GetInt("SkinHandsCount");
        }
        if (!PlayerPrefs.HasKey("SkinHeadCount"))
        {
            PlayerPrefs.SetInt("SkinHeadCount", 2);
        }
        else
        {
            skinHeadCount = PlayerPrefs.GetInt("SkinHeadCount");
        }
    }

    private void Start()
    {
        ggm = GlobalGameManager.instance;
    }

    public List<GameObject> GetAvailableBodySkins()
    {
        List<GameObject> availableSkins = GetAvailableSkins(bodySkins, ownedBodySkins);
        return availableSkins;
    }
    public List<GameObject> GetAvailableFeetSkins()
    {
        List<GameObject> availableSkins = GetAvailableSkins(feetSkins, ownedFeetSkins);
        return availableSkins;
    }
    public List<GameObject> GetAvailableHandSkins()
    {
        List<GameObject> availableSkins = GetAvailableSkins(handSkins, ownedHandSkins);
        return availableSkins;
    }
    public List<GameObject> GetAvailableHeadSkins()
    {
        List<GameObject> availableSkins = GetAvailableSkins(headSkins, ownedHeadSkins);
        return availableSkins;
    }
    public int GetBodyIndex()
    {
        int count = GetAvailableSkinsCount(bodySkins, ownedBodySkins);
        if (count < PlayerPrefs.GetInt("SkinBodyCount") || (skinBodyIndex >= count))
        {
            ggm.UseNotification("Your selected skin is no longer available.", 4);
            SetBodyIndex(1);
            PlayerPrefs.SetInt("SkinBodyCount", count);
            return 1;
        }
        else
        {
            return skinBodyIndex;
        }
    }
    public int GetFeetIndex()
    {
        int count = GetAvailableSkinsCount(feetSkins, ownedFeetSkins);
        if (count < PlayerPrefs.GetInt("SkinFeetCount") || (skinFeetIndex >= count))
        {
            ggm.UseNotification("Your selected skin is no longer available.", 4);
            SetFeetIndex(1);
            PlayerPrefs.SetInt("SkinFeetCount", count);
            return 1;
        }
        else
        {
            return skinFeetIndex;
        }
    }
    public int GetHandsIndex()
    {
        int count = GetAvailableSkinsCount(handSkins, ownedHandSkins);
        if (count < PlayerPrefs.GetInt("SkinHandsCount") || (skinHandsIndex >= count))
        {
            ggm.UseNotification("Your selected skin is no longer available.", 4);
            SetHandsIndex(1);
            PlayerPrefs.SetInt("SkinHandsCount", count);
            return 1;
        }
        else
        {
            return skinHandsIndex;
        }
    }
    public int GetHeadIndex()
    {
        int count = GetAvailableSkinsCount(headSkins, ownedHeadSkins);
        if (count < PlayerPrefs.GetInt("SkinHeadCount") || (skinHeadIndex >= count))
        {
            ggm.UseNotification("Your selected skin is no longer available.", 4);
            SetHeadIndex(1);
            PlayerPrefs.SetInt("SkinHeadCount", count);
            return 1;
        }
        else
        {
            return skinHeadIndex;
        }
    }
    public void SetBodyIndex(int index)
    {
        skinBodyIndex = index;
        PlayerPrefs.SetInt("SkinBody", index);
    }
    public void SetFeetIndex(int index)
    {
        skinFeetIndex = index;
        PlayerPrefs.SetInt("SkinFeet", index);
    }
    public void SetHandsIndex(int index)
    {
        skinHandsIndex = index;
        PlayerPrefs.SetInt("SkinHands", index);
    }
    public void SetHeadIndex(int index)
    {
        skinHeadIndex = index;
        PlayerPrefs.SetInt("SkinHead", index);
    }

    private int GetAvailableSkinsCount(List<GameObject> skins, List<bool> ownedSkins)
    {
        List<GameObject> list = new List<GameObject>();
        list = GetAvailableSkins(skins, ownedSkins);
        return list.Count;
    }

    private List<GameObject> GetAvailableSkins(List<GameObject> skins, List<bool> ownedSkins)
    {
        List<GameObject> availableSkins = new List<GameObject>();
        for (int i = 0; i < skins.Count; i++)
        {
            if (ownedSkins[i])
            {
                availableSkins.Add(skins[i]);
            }
        }
        if (availableSkins.Count == 0)
        {
            Debug.Log("OWSC 156, count == 0 triggering");
            availableSkins.Add(skins[0]);
            availableSkins.Add(skins[1]);
        }
        return availableSkins;
    }

    public List<int> GetSkinIndexes()
    {
        List<int> skins = new List<int>()
        {
            skinBodyIndex,
            skinHandsIndex,
            skinFeetIndex,
            skinHairIndex,
            skinHatIndex,
            skinHeadIndex,
            skinEyesIndex
        };
        return skins;
    }
}
