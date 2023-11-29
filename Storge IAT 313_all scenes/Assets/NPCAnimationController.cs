using UnityEngine;

public class NPCAnimationController : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform parentTransform;
    private Vector3 lastPosition;
    private float movementThreshold = 0.0005f;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        parentTransform = transform.parent; // Reference to the parent transform
        lastPosition = parentTransform.position;
    }

    void Update()
    {
        Vector3 currentPosition = parentTransform.position;
        Vector3 movementDelta = currentPosition - lastPosition;

        UpdateAnimationState(movementDelta);
        lastPosition = currentPosition;
    }

    private void UpdateAnimationState(Vector3 movementDelta)
    {
        bool isWalking = movementDelta.magnitude > movementThreshold;
        animator.SetBool("walking", isWalking);

        if (isWalking)
        {
            // Flip the sprite based on the direction of movement
            spriteRenderer.flipX = movementDelta.x < 0;
        }
    }
}
