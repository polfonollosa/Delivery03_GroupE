using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image Image;
    public TextMeshProUGUI AmountText;
    public Image SelectionHighlight; 

    private Canvas _canvas;
    private Transform _parent;
    private ItemBase _item;
    private InventoryUI _inventory;

    public void Initialize(ItemSlot slot, InventoryUI inventory)
    {
        Image.sprite = slot.Item.ImageUI;
        Image.SetNativeSize();

        AmountText.text = slot.Amount.ToString();
        AmountText.enabled = slot.Amount > 1;

        _item = slot.Item;
        _inventory = inventory;

      
        SetSelected(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _inventory.SelectSlot(this);
    }

    public void SetSelected(bool isSelected)
    {
        if (SelectionHighlight != null)
        {
            SelectionHighlight.enabled = isSelected;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _parent = transform.parent;
        transform.SetParent(_canvas.transform, true);
        transform.SetAsLastSibling(); // Mueve el objeto arriba en la jerarquía
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Mueve el objeto con el cursor usando el delta del ratón
        transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0);
    }



    public void OnEndDrag(PointerEventData eventData)
    {
        // Encuentra el objeto en el que soltamos el ítem
        RaycastHit2D hitData = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

        if (hitData)
        {
            Debug.Log("Drop over object: " + hitData.collider.gameObject.name);

            var consumer = hitData.collider.GetComponent<IConsume>();
            bool consumable = _item is ConsumableItem;

            if ((consumer != null) && consumable)
            {
                (_item as ConsumableItem).Use(consumer);
                _inventory.UseItem(_item);
            }
        }

        // Regresa el objeto a su posición original en el inventario
        transform.SetParent(_parent.transform);
        transform.localPosition = Vector3.zero;
    }

}
