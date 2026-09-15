using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float leftBound = -8f;   // Adjust to match your camera/platform width
    public float rightBound = 8f;

    [Header("Hit Feedback (optional)")]
    public float invincibilityTime = 1f; // Brief invincibility after getting hit
    private bool isInvincible = false;

    // NOTE: Damage is now only triggered via the HeadHitbox child object (see PlayerHeadHitbox.cs).
    // This body collider is left for physical presence only and does not deal damage on its own.

    void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.IsGameActive())
            return; // Freeze player once game over

        HandleMovement();
    }

    void HandleMovement()
    {
        // Works with A/D or Left/Right arrow keys by default (Input Manager "Horizontal" axis)
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        Vector3 newPosition = transform.position + Vector3.right * horizontalInput * moveSpeed * Time.deltaTime;

        // Clamp so the player can't move off the platform/screen
        newPosition.x = Mathf.Clamp(newPosition.x, leftBound, rightBound);

        transform.position = newPosition;
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }

    // Called by PlayerHeadHitbox.cs when a falling object hits the head zone specifically
    public void TakeHit()
    {
        if (isInvincible) return;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PlayerHit();
        }

        StartCoroutine(InvincibilityFrames());
    }

    System.Collections.IEnumerator InvincibilityFrames()
    {
        isInvincible = true;
        // Placeholder flash effect - swap for a sprite blink later
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }
}
