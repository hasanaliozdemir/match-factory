using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Rigidbody rig;
    [SerializeField] private Collider itemCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisableShadow()
    {
        // Implementation to disable the item's shadow

    }

    public void DisablePhysics()
    {
        // Implementation to disable the item's collider/physics
        rig.isKinematic = true;
        itemCollider.enabled = false;
    }
}
