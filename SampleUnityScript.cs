using UnityEngine;

// Example 1: A plain C# class (NOT a Unity type)
// This does NOT inherit from UnityEngine.Object
public class Foobar
{
    public string name;
    public int value;
}

// Example 2: A Unity MonoBehaviour (IS a Unity type)
public class SampleUnityScript : MonoBehaviour
{
    // PLAIN C# OBJECT - does NOT have operator true/false
    private Foobar plainFoo = new Foobar();
    
    // UNITY OBJECTS - DO have operator true/false
    public GameObject playerObject;
    public Transform targetTransform;
    public SampleUnityScript otherScript;
    
    // More Unity types
    public Rigidbody rb;
    public Collider col;
    public Camera mainCamera;
    public Light lightComponent;
    public AudioSource audioSource;
    public Renderer renderer;
    public Material material;
    public Texture texture;
    public ScriptableObject data;

    void Update()
    {
        // ============================================================
        // CORRECTLY FLAGGED: This SHOULD be an error
        // ============================================================
        // plainFoo is NOT a UnityEngine.Object, so this is actually invalid
        // A standard C# class doesn't have operator true/false
        // This SHOULD be flagged by analyzers
        // if (plainFoo)  // UNCOMMENT TO SEE VALID ERROR
        // {
        //     Debug.Log("This is correctly an error");
        // }
        
        // The correct way for plain C# objects:
        if (plainFoo != null)  // This is fine
        {
            Debug.Log($"Foo value: {plainFoo.value}");
        }
        
        // ============================================================
        // FALSE POSITIVES: These SHOULD NOT be errors but will be flagged
        // ============================================================
        
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
        // CASE 5: Common pattern in Unity - FALSE POSITIVE
        if (other)
        {
            Debug.Log($"Collision with {other.name}");
        }
    }
    
    void LogicalOperators()
    {
        // CASE 6: Logical AND with Unity objects - FALSE POSITIVE
        if (playerObject && targetTransform)
        {
            Debug.Log("Both exist");
        }
        
        // CASE 7: Logical OR with Unity objects - FALSE POSITIVE
        if (playerObject || otherScript)
        {
            Debug.Log("At least one exists");
        }
        
        // CASE 8: Negation - FALSE POSITIVE
        if (!playerObject)
        {
            Debug.Log("Player doesn't exist");
        }
        
        // CONTRAST: This would be CORRECTLY flagged (if uncommented)
        // if (!plainFoo)  // Plain C# object, no operator !
        // {
        //     Debug.Log("This is correctly an error");
        // }
    }
    
    void CheckWithTernary()
    {
        // CASE 9: Ternary operator with Unity object - FALSE POSITIVE
        string status = playerObject ? "Active" : "Inactive";
        Debug.Log(status);
        
        // CASE 10: Ternary with object assignment - FALSE POSITIVE
        GameObject target = targetTransform ? targetTransform.gameObject : null;
        
        // CONTRAST: This would be CORRECTLY flagged (if uncommented)
        // string fooStatus = plainFoo ? "Exists" : "Null";  // Plain C# object
    }
    
    void CheckVariousComponents()
    {
        // CASE 11-18: All Unity types - all FALSE POSITIVES
        if (rb) Debug.Log("Has Rigidbody");
        if (col) Debug.Log("Has Collider");
        if (mainCamera) Debug.Log("Has Camera");
        if (lightComponent) Debug.Log("Has Light");
        if (audioSource) Debug.Log("Has AudioSource");
        if (renderer) Debug.Log("Has Renderer");
        if (material) Debug.Log("Has Material");
        if (texture) Debug.Log("Has Texture");
    }
    
    void GetComponentPattern()
    {
        // CASE 19: EXTREMELY common in Unity - FALSE POSITIVE
        Rigidbody rbComp = GetComponent<Rigidbody>();
        if (rbComp)
        {
            rbComp.AddForce(Vector3.up * 10f);
        }
        
        // CASE 20: Another common pattern - FALSE POSITIVE
        Transform parent = transform.parent;
        if (parent)
        {
            Debug.Log($"Parent: {parent.name}");
        }
        
        // CONTRAST: Getting a plain component (correctly handled)
        Foobar customComponent = GetComponent<Foobar>();
        if (customComponent != null)  // Must use explicit null check
        {
            Debug.Log("Has custom component");
        }
    }
    
    void FindObjectPatterns()
    {
        // CASE 21: Very common Unity pattern - FALSE POSITIVE
        GameObject player = GameObject.Find("Player");
        if (player)
        {
            Debug.Log("Found player");
        }
        
        // CASE 22: FindObjectOfType pattern - FALSE POSITIVE
        SampleUnityScript script = FindObjectOfType<SampleUnityScript>();
        if (script)
        {
            Debug.Log("Found script");
        }
    }
    
    void CheckScriptableObject()
    {
        // CASE 23: ScriptableObjects also have the operator - FALSE POSITIVE
        if (data)
        {
            Debug.Log("Data exists");
        }
    }
    
    void NullCoalescingExample()
    {
        // CASE 24: Null coalescing with Unity objects - FALSE POSITIVE
        GameObject obj = playerObject ?? (otherScript ? otherScript.gameObject : null);
        
        // CASE 25: While loop with Unity object - FALSE POSITIVE
        while (playerObject)
        {
            Debug.Log("This will never execute but is valid syntax");
            break;
        }
    }
    
    void DemonstrateTheDifference()
    {
        // ============================================================
        // SUMMARY: The key difference
        // ============================================================
        
        // PLAIN C# CLASS (Foobar):
        // - Does NOT inherit from UnityEngine.Object
        // - Does NOT have operator true/false
        // - if (plainFoo) is INVALID and SHOULD be flagged ✓
        // - Must use: if (plainFoo != null) ✓
        
        // UNITY TYPES (GameObject, Transform, MonoBehaviour, etc.):
        // - Inherit from UnityEngine.Object
        // - HAVE operator true/false overloaded
        // - if (unityObject) is VALID but gets flagged as error ✗ FALSE POSITIVE
        // - Can use either: if (unityObject) ✓ or if (unityObject != null) ✓
        
        // The problem: Standard C# analyzers don't know about Unity's operator overloads
        // So they treat ALL of these as errors, when only the plain C# one actually is
    }
}