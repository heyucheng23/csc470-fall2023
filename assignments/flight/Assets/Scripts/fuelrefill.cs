using UnityEngine;

public class FuelRefill : MonoBehaviour
{
    public float fuelAmount = 50.0f; // Adjust the amount of fuel refilled as needed

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has entered the trigger zone of the fuel object
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger zone.");
            // Access the player's fuel controller script (assuming it's on the player object)
            HumanoidAircraftController playerController = other.GetComponent<HumanoidAircraftController>();

            if (playerController != null)
            {
                // Refill the player's fuel and destroy the fuel object
                playerController.AddFuel(fuelAmount);
                Destroy(gameObject);
                Debug.Log("Fuel refilled and object destroyed.");
            }
        }
    }
}