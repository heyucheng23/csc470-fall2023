using UnityEngine;
using UnityEngine.UI;
using FlyingSystem;

public class HumanoidAircraftController : MonoBehaviour
{
    public Transform springArmTransform;
    public Camera characterCamera;
    public Rigidbody rootRigidbody;

    private HumanoidAircraftFlyingSystem humanoidAircraftFlyingSystem;

    public ParticleSystem jetPackParticleSystem;

    public bool activated = false;

    public float cameraSpeed = 300.0f;

    public float fuelAmount = 100.0f;

    public float depletedSpeed = 2.0f;

    public Slider fuelSlider;

    [Header("General Attributes")]
    public bool takeOff;
    public bool boosting;

    [Header("Flying")]
    public bool hoverMode = false;
    public float verticalBoostThrust = 5.0f;

    [Header("Mobile")]
    public Joystick joystick;
    public bool mobileInputControl = false;
    public float mobileCameraSpeed = 300.0f;
    private float screenCenterX;

    private bool fuelDepleted = false;

    void Start()
    {
        if (activated)
            Activate();

        humanoidAircraftFlyingSystem = this.GetComponent<HumanoidAircraftFlyingSystem>();

        screenCenterX = Screen.width / 2.0f;
    }

    void Update()
    {
        if (activated)
        {
            if (fuelAmount > 0)
            {
                if (!mobileInputControl)
                {
                    PCCameraControlLogic();
                    PCInputControlLogic();
                }
                else
                {
                    MobileCameraControlLogic();
                    MobileInputControlLogic();
                }
            }
            else if (!fuelDepleted)
            {
                FuelDepletionActions();
                fuelDepleted = true;
            }
        }
    }

    void FuelDepletionActions()
    {
        // Disable movement controls here.
        humanoidAircraftFlyingSystem.AddPitchInput(0.0f);
        humanoidAircraftFlyingSystem.AddYawInput(0.0f);
        MobileInputControlLogic(); // Clear joystick input.

        // Slow down the aircraft's velocity.
        rootRigidbody.velocity *= 0.5f;

        // Update the UI to indicate that fuel is depleted.
        if (fuelSlider != null)
        {
            fuelSlider.value = 0.0f;
        }
    }

    void RestrictMovementOnFuelDepletion()
    {
        // Disable movement controls here.
        humanoidAircraftFlyingSystem.AddPitchInput(0.0f);
        humanoidAircraftFlyingSystem.AddYawInput(0.0f);
        MobileInputControlLogic(); // Clear joystick input.

        // Slow down the aircraft's velocity.
        rootRigidbody.velocity *= 0.5f;
    }

    public void Activate()
    {
        activated = true;
        characterCamera.enabled = true;
        characterCamera.GetComponent<AudioListener>().enabled = true;
    }

    public void Deactivate()
    {
        activated = false;
        characterCamera.enabled = false;
        characterCamera.GetComponent<AudioListener>().enabled = false;
    }

    void PCCameraControlLogic()
    {
        // Camera view follows the roll
        springArmTransform.rotation = Quaternion.Euler(springArmTransform.rotation.eulerAngles.x - Input.GetAxis("Mouse Y") * cameraSpeed * Time.deltaTime, springArmTransform.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * cameraSpeed * Time.deltaTime, 0.0f);
    }

    void MobileCameraControlLogic()
    {
        // Temporarily use mouse to simulate the touch
        if (Input.GetMouseButton(0) && Input.mousePosition.x > screenCenterX)
        {
            springArmTransform.Rotate(Vector3.up * mobileCameraSpeed * Input.GetAxis("Mouse X") * Time.deltaTime);
            springArmTransform.Rotate(-Vector3.right * mobileCameraSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime);
        }

        // Only detects on mobile devices
        if (Input.touchCount > 0)
        {
            for (var i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).position.x > screenCenterX && Input.GetTouch(i).phase == TouchPhase.Moved)
                {
                    springArmTransform.Rotate(Vector3.up * mobileCameraSpeed * Input.GetTouch(i).deltaPosition.x * Time.deltaTime);
                    springArmTransform.Rotate(-Vector3.right * mobileCameraSpeed * Input.GetTouch(i).deltaPosition.y * Time.deltaTime);
                }
            }
        }
    }

    void PCInputControlLogic()
    {
        // Hold down to move forward / backward
        if (Input.GetKey(KeyCode.W))
            humanoidAircraftFlyingSystem.AddPitchInput(1.0f);
        else if (Input.GetKey(KeyCode.S))
            humanoidAircraftFlyingSystem.AddPitchInput(-1.0f);

        // Hold down to move left / right
        if (Input.GetKey(KeyCode.A))
            humanoidAircraftFlyingSystem.AddYawInput(-1.0f);
        else if (Input.GetKey(KeyCode.D))
            humanoidAircraftFlyingSystem.AddYawInput(1.0f);

        if (Input.GetKeyUp(KeyCode.C))
            SwitchHoverModeOnOrOff();

        if (Input.GetKey(KeyCode.Space))
            VerticalBoost();

        // Boost on / off
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            Boost();
    }

    void MobileInputControlLogic()
    {
        if (joystick != null)
        {
            if (joystick.isMoving)
            {
                humanoidAircraftFlyingSystem.AddPitchInput(joystick.inputAxisY);
                humanoidAircraftFlyingSystem.AddYawInput(joystick.inputAxisX);
            }
        }
    }

    void ConsumeFuel(float amount)
    {
        fuelAmount -= amount;
        fuelAmount = Mathf.Clamp(fuelAmount, 0.0f, 100.0f); // Ensure fuel doesn't go below 0 or above the maximum
    }

    void UpdateFuelBar()
    {
        if (fuelSlider != null)
        {
            fuelSlider.value = fuelAmount / 100.0f; // Assuming a maximum fuel amount of 100
        }
    }

    public void AddFuel(float amount)
    {
        fuelAmount += amount;
        fuelAmount = Mathf.Clamp(fuelAmount, 0.0f, 100.0f);
        UpdateFuelBar(); // Update the fuel bar when fuel is added
    }

     void ReduceMovementOnFuelDepletion()
    {
        // Reduce the speed of the aircraft.
        humanoidAircraftFlyingSystem.flyingSpeed = depletedSpeed;

        // You can still allow some limited controls for the player.
        if (Input.GetKey(KeyCode.Space))
        {
            VerticalBoost();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            Boost();
        }

    }

    public void VerticalBoost()
    {
         if (!humanoidAircraftFlyingSystem.hoverMode && fuelAmount > 0)
        {
            rootRigidbody.AddRelativeForce(Vector3.up * verticalBoostThrust);
            jetPackParticleSystem.Play();
            ConsumeFuel(0.01f);
            UpdateFuelBar();
        }
    }

    public void Boost()
    {
        humanoidAircraftFlyingSystem.boosting = !humanoidAircraftFlyingSystem.boosting;
        boosting = humanoidAircraftFlyingSystem.boosting;
    }

    public void MobileTurnLeft()
    {
        humanoidAircraftFlyingSystem.AddYawInput(-1.0f);
    }

    public void MobileTurnRight()
    {
        humanoidAircraftFlyingSystem.AddYawInput(1.0f);
    }

    public void SwitchHoverModeOnOrOff()
    {
        humanoidAircraftFlyingSystem.hoverMode = !humanoidAircraftFlyingSystem.hoverMode;
        hoverMode = humanoidAircraftFlyingSystem.hoverMode;

        if (!humanoidAircraftFlyingSystem.hoverMode)
        {
            if (!rootRigidbody.useGravity)
            {
                rootRigidbody.useGravity = true;
                rootRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            }
        }
        else
        {
            if (rootRigidbody.useGravity)
            {
                rootRigidbody.useGravity = false;
                rootRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            }
        }
    }


    public void Ascend()
    {
        if (rootRigidbody.useGravity)
            rootRigidbody.useGravity = false;

        rootRigidbody.velocity = Vector3.zero;

        humanoidAircraftFlyingSystem.AddPitchInput(1.0f);
    }

    public void Descend()
    {
        if (rootRigidbody.useGravity)
            rootRigidbody.useGravity = false;

        rootRigidbody.velocity = Vector3.zero;

        humanoidAircraftFlyingSystem.AddPitchInput(-1.0f);
    }

    public float GetFlyingSpeed()
    {
        return humanoidAircraftFlyingSystem.flyingSpeed;
    }

    public float GetPowerPercentage()
    {
        return humanoidAircraftFlyingSystem.powerPercentage;
    }

    public float GetWeightPercentage()
    {
        return humanoidAircraftFlyingSystem.weightPercentage;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Road")
            if (humanoidAircraftFlyingSystem.inAir)
                humanoidAircraftFlyingSystem.Land();
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.name == "Road")
            if (!humanoidAircraftFlyingSystem.inAir)
                humanoidAircraftFlyingSystem.TakeOff();
    }
}