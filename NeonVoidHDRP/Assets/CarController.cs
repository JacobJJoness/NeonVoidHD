using UnityEngine;

public class CarController : MonoBehaviour
{
    public Transform carCameraTransform; // Assign this from the inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure your player has a tag named "Player"
        {
            other.GetComponent<InputManager>().SetNearCar(this, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<InputManager>().SetNearCar(this, false);
        }
    }

    public void EnterCar()
    {
        Debug.Log("Player has entered the car.");
        // Here you can disable player controls and enable car driving mechanics
    }
}
