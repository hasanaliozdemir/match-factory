using System.IO.Compression;
using MatchFactory.Scripts.Enums;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Rigidbody rig;
    [SerializeField] private Collider itemCollider;
    [SerializeField] private Renderer itemRenderer;
    
    [Header("Item Data")]
    [SerializeField] private ItemEnum type;

    public ItemEnum Type => type;

    private Material baseMaterial;
    private ItemSpot spot;

    public ItemSpot Spot => spot;
    void Awake()
    {
        baseMaterial = itemRenderer.material;
    }

    public void AssignSpot(ItemSpot spot)
    {
        this.spot = spot;
    }

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
        itemRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

    }

    public void DisablePhysics()
    {
        // Implementation to disable the item's collider/physics
        rig.isKinematic = true;
        itemCollider.enabled = false;
    }

    public void Select(Material outlineMaterial)
    {
        itemRenderer.materials = new Material[2] { baseMaterial, outlineMaterial };

    }

    public void Deselect()
    {
        itemRenderer.materials = new Material[1] { baseMaterial };
    }
}
