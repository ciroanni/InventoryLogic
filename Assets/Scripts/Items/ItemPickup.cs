using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [Min(1)]
    [SerializeField] private int quantity = 1;

    void Update()
    {
        //lo faccio ruotare giusto per renderlo più visibile e interessante da raccogliere
        transform.Rotate(Vector3.up, 50f * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventorySystem inventory = other.GetComponent<InventorySystem>();
            if (inventory != null && inventory.AddItem(itemData, quantity))
            {
                Destroy(gameObject);
            }
        }
    }
}
