using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class AdvancedDialogManager : MonoBehaviour
{
    private AdvancedDialogSO currentConvo;
    private int stepNum;
    private bool dialogueActivated;

    private GameObject dialogueCanvas;
    private TMP_Text actor;
    private Image portrait;
    private TMP_Text dialogueText;

    private string currentSpeaker;
    private Sprite currentPortrait;

    public ActorSO[] actorSO;

    //Button references
    private GameObject[] optionButton;
    private TMP_Text[] optionButtonText;
    private GameObject optionsPanel;

    //Typewritter effect
    [SerializeField]
    private float typingSpeed = 0.02f;
    private Coroutine typeWriterRoutine;
    private bool canContinueText = true;

    private Button endButton;

    //freeze player movement
    private PlayerMovement playerMove;


    // Start is called before the first frame update
    void Start()
    {
        //find player movee scripts
        playerMove = GameObject.Find("Player").GetComponent<PlayerMovement>();

        //Find Buttons
        optionButton = GameObject.FindGameObjectsWithTag("OptionButton");
        optionsPanel = GameObject.Find("OptionPanel");
        optionsPanel.SetActive(false);

        //Find Buttons
        optionButtonText = new TMP_Text[optionButton.Length];
        for (int i = 0; i < optionButton.Length; i++)
        {
            optionButtonText[i] = optionButton[i].GetComponentInChildren<TMP_Text>();
        }

        //turn off the buttons to starts
        for (int i = 0; i < optionButton.Length; i++)
        {
            optionButton[i].SetActive(false);
        }

        dialogueCanvas = GameObject.Find("DialogueCanvas");
        actor = GameObject.Find("ActorText").GetComponent<TMP_Text>();
        portrait = GameObject.Find("Portrait").GetComponent<Image>();
        dialogueText = GameObject.Find("DialogueText").GetComponent<TMP_Text>();

        dialogueCanvas.SetActive(false);

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 8)
        {
            endButton = GameObject.Find("Button ke Final").GetComponent<Button>();
        }
        else if (sceneIndex == 11)
        {
            endButton = GameObject.Find("Button ke 5").GetComponent<Button>();
        }

        // Hide the end button initially
        if (endButton != null)
        {
            endButton.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueActivated && Input.GetButtonDown("Interact") && canContinueText)
        {
            //freeze player movement
            playerMove.enabled = false;

            //cancel dialogue if there are no lines of dialogue remaining
            if(stepNum >= currentConvo.actors.Length)
            {
                TurnOffDialogue();
            }
            //continue dialogue
            else
            {
                PlayDialogue();
            }
        }
    }

    void PlayDialogue()
    { 
        SetActorInfo(true);

        //display dialogue
        actor.text = currentSpeaker;
        portrait.sprite = currentPortrait;

        //if there is a branch
        if (currentConvo.actors[stepNum] == DialogueActors.Branch)
        {
            for (int i = 0; i < currentConvo.optionText.Length; i++)
            {
                if (currentConvo.optionText[i] == null)
                {
                    optionButton[i].SetActive(false);
                }
                else
                {
                    optionButtonText[i].text = currentConvo.optionText[i];
                    optionButton[i].SetActive(true);
                }

                //Set the First button to be auto-selected
                optionButton[0].GetComponent<Button>().Select();
            }
        }

        //Keep the routine from running multiple times at the same time
        if(typeWriterRoutine != null)
        {
            StopCoroutine(typeWriterRoutine);
        }


        if(stepNum < currentConvo.dialogue.Length)
        {
          typeWriterRoutine = StartCoroutine(TypeWriterEffect(dialogueText.text = currentConvo.dialogue[stepNum]));
        }

        else
        {
            optionsPanel.SetActive(true);
        }
        
        dialogueCanvas.SetActive(true);
        stepNum += 1;
    }

    void SetActorInfo(bool recurringCharacter)
    {
        if(recurringCharacter)
        {
            foreach (var actor in actorSO)
            {
                if (actor.name == currentConvo.actors[stepNum].ToString())
                {
                    currentSpeaker = actor.actorName;
                    currentPortrait = actor.actorPortrait;
                    break;
                }
            }
        }

        else
        {
            currentSpeaker = currentConvo.randomActorName;
            currentPortrait = currentConvo.randomActorPortrait;
        }
    }

    public void Option(int optionNum)
    {
        foreach (GameObject button in optionButton)
        {
            button.SetActive(false);
        }

        if (optionNum == 0)
        {
            currentConvo = currentConvo.option0;
            Debug.Log("0");
        }
        else if (optionNum == 1)
        {
            currentConvo = currentConvo.option1;
            Debug.Log("1");
        }
        else if (optionNum == 2)
        {
            currentConvo = currentConvo.option2;
            Debug.Log("2");
        }

        stepNum = 0;
        PlayDialogue();  // Play dialogue immediately after resetting the state
    }

    private IEnumerator TypeWriterEffect(string line)
    {
        dialogueText.text = "";
        canContinueText = false;
        yield return new WaitForSeconds(.5f);
        foreach (char letter in line.ToCharArray())
        {
            if(Input.GetButtonDown("Interact"))
            {
                dialogueText.text = line;
                break;
            }

            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        canContinueText = true;
    }


    public void InitiateDialogue(NPCDialogue npcDialogue)
    {
        //the array we are currently stepping through
        currentConvo = npcDialogue.conversation[0];
        dialogueActivated = true;
    }

    public void TurnOffDialogue()
    {
        CheckShowEndButton();
        stepNum = 0;
        dialogueActivated = false;
        optionsPanel.SetActive(false);
        dialogueCanvas.SetActive(false);
        playerMove.enabled = true;
    }

    private void CheckShowEndButton()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Checking to show end button: Scene " + sceneIndex + " StepNum: " + stepNum + " Dialogue Length: " + currentConvo.dialogue.Length);
        if (stepNum == currentConvo.dialogue.Length)
        {
            if (endButton != null)
            {
                Debug.Log("Showing end button");
                endButton.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("End button is not assigned");
            }
        }
    }
}

public enum DialogueActors
{
    Hugo,
    Bartender,
    Branch,
    Knight,
    King,
    Lancelot,
    Percival,
    Gawain,
    HugoUpgrade
};
