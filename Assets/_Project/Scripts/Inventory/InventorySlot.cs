using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [HideInInspector] public GameObject itemInfoWindow;
    [HideInInspector] public Image slotIcon;
    [HideInInspector] public bool full;
    protected InventoryController inventoryController;
    public SO_Equipment item;
    
    protected void Awake()
    {
        slotIcon = transform.GetChild(0).GetComponent<Image>();
        itemInfoWindow = UIManager.instance.itemInfoWindow;
        inventoryController = UIManager.instance.inventoryController;
        UpdateItemIcon();
    }

    private void OnEnable()
    {
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateItemIcon()
    {
        if (item == null)
        {
            slotIcon.enabled = false;
            return;
        }

        slotIcon.enabled = true;
        slotIcon.sprite = item.equipmentSprite;

        float iconScale;
        Vector2 iconPosition;
        
        //ADJUST ICON SCALE AND POSITION
        switch (item.equipmentType)
        {
            case SO_Equipment.EquipmentType.Top:
                iconScale = 2.57f;
                iconPosition = new Vector2(3, 40);
                break;
            case SO_Equipment.EquipmentType.Head:
                iconScale = 1.8f;
                iconPosition = new Vector2(5, -19);
                break;
            case SO_Equipment.EquipmentType.Bottom:
                iconScale = 2.5f;
                iconPosition = new Vector2(5, 74);
                break;
            case SO_Equipment.EquipmentType.Face:
                iconScale = 2.6f;
                iconPosition = new Vector2(1.8f, 5);
                break;
            default: //weapon
                iconScale = 3.2f;
                iconPosition = new Vector2(-39.5f, 111.5f);
                break;
        }

        slotIcon.GetComponent<HoverGrowerButton>().SetOriginalScale(Vector3.one * iconScale);
        slotIcon.transform.localScale = Vector3.one * iconScale;
        slotIcon.GetComponent<RectTransform>().localPosition = iconPosition;
    }

    public void AddItem(SO_Equipment item, bool hoverAnimation)
    {
        this.item = item;
        full = true;
        UpdateItemIcon();
        
        if (hoverAnimation) HoverSlot();
    }

    public void RemoveItem()
    {
        item = null;
        full = false;
    }

    public void DeleteItem()
    {
        RemoveItem();
        UpdateItemIcon();
        UIManager.instance.itemInfoWindow.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;

        HoverSlot();
    }

    public void HoverSlot()
    {
        slotIcon.GetComponent<HoverGrowerButton>().Grow();
        itemInfoWindow.GetComponent<ItemInfoWindow>().UpdateItemInfo(item.name, item.description, item.sellingPrice);
        itemInfoWindow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null) return;
        
        itemInfoWindow.SetActive(false);
        slotIcon.GetComponent<HoverGrowerButton>().Shrink();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        inventoryController.SelectSlot(this);
    }
}
