using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// This component is to be assigned to a button in the inspector that will hold a list of craftable items & display them on-click in the crafting list.
public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private Transform craftSlotParent;

    [SerializeField]
    private GameObject craftSlotPrefab;

    [SerializeField]
    private List<ItemData_Equipment> craftEquipment;

    private void Start()
    {
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetupCraftList();
        SetupDefaultCraftWindow();
    }

    // This method will call setting up the craft list on a mouse-down event.
    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
    }

    // This list resets current craftSlots list, and instantiates new craftSlots objects to add to the new list.
    public void SetupCraftList()
    {
        for (var i = 0; i < craftSlotParent.childCount; i++)
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }

        for (var i = 0; i < craftEquipment.Count; i++)
        {
            var newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<UI_CraftSlot>().SetupCraftSlot(craftEquipment[i]);
        }
    }

    public void SetupDefaultCraftWindow()
    {
        if (craftEquipment[0] != null)
        {
            GetComponentInParent<UI>().craftWindow.SetupCraftWindow(craftEquipment[0]);
        }
    }
}