using UnityEngine;

public class BartenderIdleAnimation : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Contoh: Menjalankan animasi idle
        animator.Play("bartenderIdle");
    }
}
