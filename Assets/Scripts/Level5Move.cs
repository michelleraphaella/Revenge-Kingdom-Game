using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Level5Move : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    float moveX, moveY;
    private Rigidbody2D body;
    private Vector2 axisMovement;

    public GameObject sliderCanvas;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody2D component attached to this game object
        body = GetComponent<Rigidbody2D>();

        // Freeze rotation on the z-axis
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        axisMovement.x = Input.GetAxisRaw("Horizontal");
        axisMovement.y = Input.GetAxisRaw("Vertical");
    }

    void OnMove(InputValue moveValue)
    {
        Vector2 moveVector = moveValue.Get<Vector2>();

        moveX = moveVector.x;
        moveY = moveVector.y;

        CheckForFlipping();
    }

    private void CheckForFlipping()
    {
        bool movingLeft = axisMovement.x < 0;
        bool movingRight = axisMovement.x > 0;

        if (movingLeft)
        {
            StartCoroutine(TransformPlayer(() => {
                transform.localScale = new Vector3(3.9f, transform.localScale.y);
            }));
        }

        if (movingRight)
        {
            StartCoroutine(TransformPlayer(() => {
                transform.localScale = new Vector3(-3.9f, transform.localScale.y);
            }));
        }
    }

    private void FixedUpdate()
    {
        Vector2 move = new Vector2(moveX, moveY);
        body.velocity = move * speed;
    }

    private IEnumerator TransformPlayer(System.Action transformAction)
    {
        if (sliderCanvas != null)
        {
            // Store sliderCanvas's world position and rotation
            Vector3 sliderPosition = sliderCanvas.transform.position;
            Quaternion sliderRotation = sliderCanvas.transform.rotation;

            // Detach the canvas
            sliderCanvas.transform.SetParent(null);

            // Perform the transformation on the player
            transformAction.Invoke();

            // Wait for the end of the frame to ensure the transformation is complete
            yield return new WaitForEndOfFrame();

            // Restore the canvas's world position and rotation
            if (sliderCanvas != null) // Ensure it still exists
            {
                sliderCanvas.transform.position = sliderPosition;
                sliderCanvas.transform.rotation = sliderRotation;

                // Reattach the canvas
                sliderCanvas.transform.SetParent(transform);
            }
        }
    }
}
