using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public AdvancedDialogSO[] conversation;
    private Transform player;
    private SpriteRenderer speechBubble;
    private AdvancedDialogManager advancedDialogManager;
    private bool dialogueInitiated;

    // Start is called before the first frame update
    void Start()
    {
        advancedDialogManager = GameObject.Find("DialogueManager").GetComponent<AdvancedDialogManager>();
        speechBubble = GetComponent<SpriteRenderer>();
        speechBubble.enabled = false;
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !dialogueInitiated)
        {
            //speech bubble on
            speechBubble.enabled = true;

            //find the player's transform
            player = collision.gameObject.GetComponent<Transform>();

            advancedDialogManager.InitiateDialogue(this);
            dialogueInitiated = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    { 
        if(collision.gameObject.tag == "Player")
        {
            speechBubble.enabled = false;

            advancedDialogManager.TurnOffDialogue();
            dialogueInitiated = false;
        }
    }
}
