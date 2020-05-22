using UnityEngine.UI;
using UnityEngine;

public class BubbleCard : MonoBehaviour
{
    OWPlayerController owpc;
    OWSkinController owsc;
    private bool bodySelected;
    private bool feetSelected;
    private bool handsSelected;
    private bool headSelected;

    private void Awake()
    {
        bodySelected = false;
        feetSelected = false;
        handsSelected = false;
        headSelected = false;
    }

    private void Start()
    {
        // TODO: Get the GameObjects of the skins and get the available image.
        owpc = GameObject.Find("UICanvas1/PlayerPanel").GetComponent<OWPlayerController>();
        owsc = GameObject.Find("Data/ClientData").GetComponent<OWSkinController>();
    }

    public void SelectPrefab()
    {
        bodySelected = owpc.GetBodySelected();
        feetSelected = owpc.GetFeetSelected();
        handsSelected = owpc.GetHandsSelected();
        headSelected = owpc.GetHeadSelected();
        int currentIndex = this.transform.GetSiblingIndex();
        bool isIndex = false;
        GameObject parent = this.transform.parent.gameObject;
        for (int i=0; i < parent.transform.childCount; i++)
        {
            isIndex = (i == currentIndex);
            PopulateSelectButton(parent.transform.GetChild(i).gameObject, isIndex);
        }
        if (bodySelected)
        {
            owpc.GetAndToggleBodySkin(false);
            owsc.SetBodyIndex(currentIndex);
            owpc.GetAndToggleBodySkin(true);
        }
        else if (feetSelected)
        {
            owpc.GetAndToggleFeetSkin(false);
            owsc.SetFeetIndex(currentIndex);
            owpc.GetAndToggleFeetSkin(true);
        }
        else if (handsSelected)
        {
            owpc.GetAndToggleHandsSkin(false);
            owsc.SetHandsIndex(currentIndex);
            owpc.GetAndToggleHandsSkin(true);
        }
        else if (headSelected)
        {
            owpc.GetAndToggleHeadSkin(false);
            owsc.SetHeadIndex(currentIndex);
            owpc.GetAndToggleHeadSkin(true);
        }
    }

    private void PopulateSelectButton(GameObject card, bool selected)
    {
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
    }
}
