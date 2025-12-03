using UnityEngine;

public class SwordTargetLocator : MonoBehaviour
{
    public Transform GetSwordTargetTransform()
    {
        return gameObject.transform;
    }
}
