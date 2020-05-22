using UnityEngine;

public class OutOfBoundsDeactivator : MonoBehaviour {

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "RearTrigger")
        {
            this.transform.parent.gameObject.SetActive(false);
        }
    }
}
