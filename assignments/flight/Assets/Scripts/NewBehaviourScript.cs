using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public int scoreValue = 1; // Adjust the score value as needed
    public ParticleSystem visualEffect; // Reference to the Particle System for the visual effect

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has entered the trigger zone of the ring
        if (other.CompareTag("Player"))
        {
            Debug.Log("Ring touched the player.");

            // Access the GameManager and update the score
            GameManager gameManager = GameManager.SharedInstance;
            if (gameManager != null)
            {
                gameManager.UpdateScore(scoreValue);

                // Enable the visual effect (Particle System) and play it
                if (visualEffect != null)
                {
                    visualEffect.Play();
                }

                // Destroy the ring
                Destroy(gameObject);
            }
        }
    }
}