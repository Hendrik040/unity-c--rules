using UnityEngine;

public class SampleUnityScript : MonoBehaviour
{
    // Example 1: GameObject field
    public GameObject playerObject;
    
    // Example 2: Component field
    public Transform targetTransform;
    
    // Example 3: MonoBehaviour field
    public SampleUnityScript otherScript;

    void Update()
    {
        // CASE 1: This is VALID in Unity but triggers false positive
        // (uses Unity's operator true/false overload)
        if (playerObject)
        {
            Debug.Log("Player exists");
        }

        // CASE 2: Also valid, same issue
        if (targetTransform)
        {
            Debug.Log("Target exists");
        }

        // CASE 3: Also valid
        if (otherScript)
        {
            Debug.Log("Other script exists");
        }

        // CASE 4: The workaround - explicitly checks null
        // (still uses Unity's overloaded == operator)
        if (playerObject != null)
        {
            Debug.Log("Player exists (explicit check)");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // CASE 5: Common pattern in Unity
        if (other)
        {
            Debug.Log($"Collision with {other.name}");
        }
    }
}