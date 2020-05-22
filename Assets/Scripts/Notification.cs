using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    private Text notificationText;

    public void UseNotification(string description, float duration)
    {
        StartCoroutine(PopupNotification(description, duration));
    }

    public void UseNotification(string description, float duration, int type)
    {
        if (type == 0)
        {
            StartCoroutine(PopupNotification(description, duration));
        }
        else
        {
            OnNotification(type);
        }
    }

    public void OnExit(int index)
    {
        this.gameObject.transform.GetChild(index).gameObject.SetActive(false);
    }

    private IEnumerator PopupNotification(string description, float duration)
    {
        CanvasGroup cg = this.gameObject.GetComponent<CanvasGroup>();
        notificationText = this.gameObject.transform.GetChild(0)        // NotificationPopUp
            .transform.GetChild(1)                                      // Image
            .transform.GetChild(0).GetComponent<Text>();                // Text
        cg.alpha = 1;
        notificationText.text = description;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);

        while (cg.alpha > 0)
        {
            cg.alpha -= Time.deltaTime * 3;
            yield return null;
        }
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnNotification(int type)
    {
        this.gameObject.transform.GetChild(type).gameObject.SetActive(true);
        notificationText = this.gameObject.transform.GetChild(type)        // NotificationType
             .transform.GetChild(1)                                      // Image
             .transform.GetChild(0).GetComponent<Text>();                // Text
    }
}
