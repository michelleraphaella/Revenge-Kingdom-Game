using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("------- AUDIO SOURCE -------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("------- AUDIO CLIP -------")]
    public AudioClip background;
    public AudioClip death;

    private static AudioManager instance;

    public Button button;
    public Sprite onSprite;
    public Sprite offSprite;

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Start playing background music if assigned
            if (background != null)
            {
                musicSource.clip = background;
                musicSource.loop = true;
                musicSource.Play();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 1)
        {
            button = GameObject.Find("button on off volume").GetComponent<Button>();

            if (button != null)
            {
                button.onClick.AddListener(ToggleBackgroundMusic);
            }
        }
    }

    private void Update()
    {
       
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            // Ensure the sfxSource is not null and is available
            if (!sfxSource.isPlaying || sfxSource.clip != clip)
            {
                sfxSource.PlayOneShot(clip);
            }
        }
    }

    public void ChangeBackgroundMusic(AudioClip newClip)
    {
        if (newClip != null)
        {
            musicSource.Stop();
            musicSource.clip = newClip;
            musicSource.Play();
        }
    }

    // Method to toggle the background music
    public void ToggleBackgroundMusic()
    {
        Debug.Log("Toggling background music. Currently playing: " + musicSource.isPlaying);
        if (musicSource.mute == false)
        {
            button.image.sprite = onSprite;
            musicSource.mute = true;
            Debug.Log("Background music paused.");
        }
        //else
        //{
        //    button.image.sprite = offSprite;
        //    musicSource.mute = false;
        //    Debug.Log("Background music playing.");
        //}
    }
}
