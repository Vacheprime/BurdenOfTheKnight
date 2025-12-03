using UnityEngine;

public class PlayerFootstepAudio : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement playerMovement;
    public AudioSource audioSource;

    [Header("Audio Clips")]
    public AudioClip walkClip;
    public AudioClip runClip;

    [Header("Settings")]
    public float minMoveSpeed = 0.1f; // minimum to count as movement

    private bool isWalking;
    private bool isRunning;

    void Update()
    {
        float speed = new Vector3(
            playerMovement.rb.linearVelocity.x,
            0,
            playerMovement.rb.linearVelocity.z
        ).magnitude;

        bool isMoving = speed > minMoveSpeed;
        bool running = Input.GetButton("Left Shift");

        if (isMoving)
        {
            if (running)
                PlayRunning();
            else
                PlayWalking();
        }
        else
        {
            StopFootsteps();
        }
    }

    private void PlayWalking()
    {
        if (isWalking) return;

        isWalking = true;
        isRunning = false;

        audioSource.loop = true;
        audioSource.clip = walkClip;
        audioSource.pitch = 1f;
        audioSource.Play();
    }

    private void PlayRunning()
    {
        if (isRunning) return;

        isRunning = true;
        isWalking = false;

        audioSource.loop = true;
        audioSource.clip = runClip;
        audioSource.pitch = 1.2f; // slightly faster sound
        audioSource.Play();
    }

    private void StopFootsteps()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();

        isWalking = false;
        isRunning = false;
    }
}
