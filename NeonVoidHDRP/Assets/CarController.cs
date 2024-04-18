using UnityEngine;

public class CarController : MonoBehaviour
{
    public Transform carCameraTransform; // Assign this from the inspector
    public Transform driverSeatTransform;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter: Detected an object - " + other.gameObject.name + " with tag " + other.tag);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered car trigger zone");
            other.GetComponent<InputManager>().SetNearCar(this, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit: Detected an object - " + other.gameObject.name + " with tag " + other.tag);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited car trigger zone");
            other.GetComponent<InputManager>().SetNearCar(this, false);
        }
    }


    public void EnterCar()
    {
        Debug.Log("Player has entered the car.");
        // Here you can disable player controls and enable car driving mechanics
    }
}
