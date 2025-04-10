using UnityEngine;
using UnityEngine.UI;

public class RagdollSystem : MonoBehaviour
{
    private Rigidbody[] rbs;
    private Collider[] colls;

    private Rigidbody mainRb;
    private Collider mainColl;

    private void Awake()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        colls = GetComponentsInChildren<Collider>();
        mainRb = GetComponent<Rigidbody>();
        mainColl = GetComponent<Collider>();
        ToggleRagdollState(true);
    }

    public void ToggleRagdollState(bool state)
    {
        if (!state)
        {
            mainRb.isKinematic = true;
            mainColl.enabled = false;
        }
        for (int i = 1; i < rbs.Length; i++)
        {
            rbs[i].isKinematic = state;
        }

        for (int i = 1; i < colls.Length; i++)
        {
            colls[i].isTrigger = state;
        }
    }
}
