using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnTimer : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject parentToDisable;
    [SerializeField] private GameObject particlesToTrigger;
    [SerializeField] private float countDownTime;
#pragma warning restore 0649
    private void OnEnable()
    {
        StartCoroutine(BeginCountdown(countDownTime));
    }
    private void OnDisable()
    {
        if (particlesToTrigger != null)
        {
            particlesToTrigger.SetActive(false);
        }
    }

    private IEnumerator BeginCountdown(float time)
    {
        float deltaTime = 0;
        if (particlesToTrigger != null)
        {
            particlesToTrigger.SetActive(true);
        }
        while (deltaTime < time)
        {
            yield return null;
            deltaTime += Time.deltaTime;
        }
        parentToDisable.SetActive(false);
    }
}
