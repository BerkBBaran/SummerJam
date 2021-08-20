using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    bool canInteract = false;

    // Set in editor
    public Transform dropPoint;

    [HideInInspector] public List<Pickable> interactableObjects = null;
    public Pickable closestItem = null;

    private int TriggerCount = 0;
    private bool isCollided = false;

    //!!!!!! may be change UI
    private GameObject InteractableTextGO;
    private GameObject eButton;
    private TextMeshProUGUI InteractableTextTMP;

    public Inventory inventory;
    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        interactableObjects = new List<Pickable>();

        if (inventory == null) 
        {
            Debug.LogError("Missing inventory script in scene!!");
        }
    }
    private void Start()
    {
        // Find method may be change
        eButton = GameObject.Find("eButton");
        InteractableTextGO = GameObject.Find("InteractableText");
        InteractableTextTMP = InteractableTextGO.GetComponent<TextMeshProUGUI>();

    }
    private void Update()
    {
        // Pick up item
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (canInteract && interactableObjects.Count > 0)
            {
                closestItem.OnInteract(this);
            }
        }

        // Drop item
        if (Input.GetKeyUp(KeyCode.G))
        {
            if (!inventory.HasSpace())
            {
                // remove from inventory
                // drop to ground
                inventory.item.transform.position = dropPoint.position;
                inventory.item.gameObject.SetActive(true);

                inventory.RemoveItem();
            }
        }

        if (interactableObjects.Count > 0)
        {
            FindClosestItem();
        }

        if(isCollided)
        {   
            if(closestItem != null)
                InteractableTextTMP.text = closestItem.objectText;
        }
    }


    public void CanInteract(Pickable item)
    {
        canInteract = true;
        interactableObjects.Add(item);

        // UI part 
        TriggerCount++;
        isCollided = true;
        eButton.GetComponent<Image>().enabled = true;

    }
    public void RemoveInteraction(Pickable item)
    {
        if(interactableObjects.Contains(item))
        {
            interactableObjects.Remove(item);
            if(interactableObjects.Count == 0)
            {
                canInteract = false;
            }
        }

        // UI part 
        TriggerCount--;
        if (TriggerCount == 0)
        {
            isCollided = false;
            InteractableTextTMP.text = null;
            eButton.GetComponent<Image>().enabled = false;
        }
    }
    
   
    private void FindClosestItem()
    {
        // calculate closest distance item
        float closestDist = 9999;
        Pickable currClosest = interactableObjects[0];

        for (int i = 0; i < interactableObjects.Count; i++)
        {
            var dist = Vector2.Distance(transform.position, interactableObjects[i].transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                currClosest = interactableObjects[i];
            }
        }
        closestItem = currClosest;
    }
}
