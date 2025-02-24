using System.Collections.Generic;
using UnityEngine;


public class InventoryUI : MonoBehaviour
{
    public Inventory Inventory;
    public InventorySlotUI SlotPrefab;

    private List<InventorySlotUI> _shownObjects;
    private InventorySlotUI _selectedSlot; // Slot seleccionado

    void Start()
    {
        FillInventory(Inventory);
    }

    private void OnEnable()
    {
        Inventory.OnInventoryChange += UpdateInventory;
    }

    private void OnDisable()
    {
        Inventory.OnInventoryChange -= UpdateInventory;
    }

    private void UpdateInventory()
    {
        ClearInventory();
        FillInventory(Inventory);
    }

    private void ClearInventory()
    {
        foreach (var item in _shownObjects)
        {
            if (item) Destroy(item.gameObject);
        }

        _shownObjects.Clear();
    }

    private void FillInventory(Inventory inventory)
    {
        if (_shownObjects == null) _shownObjects = new List<InventorySlotUI>();

        if (_shownObjects.Count > 0) ClearInventory();

        for (int i = 0; i < inventory.Length; i++)
        {
            var slotUI = AddSlot(inventory.GetSlot(i));
            _shownObjects.Add(slotUI);
        }
    }

    private InventorySlotUI AddSlot(ItemSlot inventorySlot)
    {
        var element = Instantiate(SlotPrefab, transform);
        element.Initialize(inventorySlot, this);
        return element;
    }

    public void SelectSlot(InventorySlotUI slot)
    {
        // Si ya hay un slot seleccionado, lo deseleccionamos
        if (_selectedSlot != null)
        {
            _selectedSlot.SetSelected(false);
        }

        // Asignamos el nuevo slot seleccionado
        _selectedSlot = slot;
        _selectedSlot.SetSelected(true);
    }
    public void UseItem(ItemBase item)
    {
        Inventory.RemoveItem(item);
    }

}
