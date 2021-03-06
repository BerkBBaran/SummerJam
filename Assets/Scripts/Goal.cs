using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Goal : Pickable
{

    //for Talkable part
    public TextMeshProUGUI overheadText;
    private string incorrectItemText;
    public float disappearTime = 3f;

    // limonata bardagi
    // bardak -> limon, su, seker
    public List<string> requiredItems = new List<string> { "Lemon", "Sugar" };

    public Sprite completedSprite; // sprite that is shown when the item is complete: lemonade

    bool isComplete = false;

    Inventory inventory;
    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        // talkable
        incorrectItemText = overheadText.text;
        overheadText.text = "";
    }
    public override void OnInteract(PlayerInteraction playerInt)
    {
        // check player inventory
        // if has one of the required items, use it, switch to another state.
        // if all req's are met, go on to final state -> turns into lemonade.

        if (isComplete)
        {
            if (playerInt.inventory.HasSpace())
            {
                playerInt.inventory.Add(playerInt.closestItem);
                playerInt.interactableObjects.Remove(playerInt.closestItem);
                Debug.LogFormat("{0} has been collected.", objectName);
                gameObject.SetActive(false);
            }
            else
            {
                // show the item text on interact
                // bittiginde eger hala elinde item varsa incorrectitemtext cikacak
                overheadText.gameObject.SetActive(true);
                overheadText.text = incorrectItemText;

                StopAllCoroutines();
                StartCoroutine(ShowOverheadText());
            }
        }
        else // still has required items
        {
            if (inventory.item == null) //Has no Item
            {
                //Cant Take glass, nothing happens
            }
            else //Has Item
            {
                string itemName = inventory.item.objectName;
                if (requiredItems.Contains(itemName))
                {
                    // found a required item, use it for this Goal
                    inventory.RemoveItem();
                    requiredItems.Remove(itemName);
                    playerInt.CloseDropUI();
                    if (requiredItems.Count == 0)
                    {   
                        isComplete = true;
                    }
                }
                else
                {
                    // show the item text on interact

                    overheadText.gameObject.SetActive(true);
                    overheadText.text = incorrectItemText;

                    StopAllCoroutines();
                    StartCoroutine(ShowOverheadText());
                }

            }

            
        }

        // decision parameters:
        // is in inventory?
        // has all requirements met?
    }



    private IEnumerator ShowOverheadText()
    {
        yield return new WaitForSeconds(disappearTime);
        overheadText.text = "";
    }


}
