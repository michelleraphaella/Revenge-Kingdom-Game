using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public GameObject promptSprite;
    public int sceneIndex;
    private bool playerInTrigger = false; // Menyimpan status apakah pemain berada dalam trigger atau tidak

    void Start()
    {
        if (promptSprite != null)
        {
            promptSprite.SetActive(false); // Pastikan sprite tidak terlihat saat memulai
        }
    }

    public void Home()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void Setting()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void BackSetting()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }


    public void Level()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    public void BackStory()
    {
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }

    public void Bar()
    {
        SceneManager.LoadScene(4, LoadSceneMode.Single);
    }

    public void Tutorial()
    {
        SceneManager.LoadScene(5, LoadSceneMode.Single);
    }

    public void GrassField()
    {
        SceneManager.LoadScene(6, LoadSceneMode.Single);
    }

    public void Journey()
    {
        SceneManager.LoadScene(7, LoadSceneMode.Single);
    }

    public void Castle()
    {
        SceneManager.LoadScene(8, LoadSceneMode.Single);
    }

    public void Final()
    {
        SceneManager.LoadScene(9, LoadSceneMode.Single);
    }

    public void BackHome()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void GameOver1()
    {
        SceneManager.LoadScene(10, LoadSceneMode.Single);
    }

    public void BackStory5()
    {
        SceneManager.LoadScene(11, LoadSceneMode.Single);
    }

    public void Battle5()
    {
        SceneManager.LoadScene(12, LoadSceneMode.Single);
    }

    public void GameOver5()
    {
        SceneManager.LoadScene(13, LoadSceneMode.Single);
    }

    public void Final5()
    {
        SceneManager.LoadScene(14, LoadSceneMode.Single);
    }

    public void Revenge5()
    {
        SceneManager.LoadScene(15, LoadSceneMode.Single);
    }

    void Update()
    {
        // Cek jika pemain berada dalam trigger dan menekan tombol 'E'
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Pastikan pemain menyentuh Collider
        {
            playerInTrigger = true; // Set status pemain berada dalam trigger

            if (promptSprite != null)
            {
                promptSprite.SetActive(true); // Tampilkan sprite
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Pastikan pemain meninggalkan Collider
        {
            playerInTrigger = false; // Set status pemain tidak berada dalam trigger

            if (promptSprite != null)
            {
                promptSprite.SetActive(false); // Sembunyikan sprite
            }
        }
    }

    public void buttonRetry()
    {
        SceneManager.LoadScene(6, LoadSceneMode.Single);
    }

    public void buttonRetry5()
    {
        SceneManager.LoadScene(12, LoadSceneMode.Single);
    }

    public void buttonHome()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
