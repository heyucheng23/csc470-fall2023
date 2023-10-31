using UnityEngine;

namespace FlyingSystem
{
    public class FlyingVehicleFlyingSystem : MonoBehaviour
    {
        [Header("Object References")]
        public Transform rootTransform;

        public Transform rollRootTransform;
        public Transform meshRootTransform;

        [Header("General Attributes")]
        public float normalForwardSpeed = 60.0f;
        public float maximumForwardSpeed = 75.0f;
        public float boostAcceleration = 10.0f;
        public float backwardSpeed = 30.0f;
        public float slowDownAcceleration = 25.0f;
        public bool remainPitchZeroWhenHovering = true;

        [Header("Turning Attributes")]
        public float meshYawTurningSpeed = 30.0f;
        [Range(0.0f, 100.0f)]
        public float meshYawTurningSmoothingFactor = 2.0f;
        [Range(0.0f, 100.0f)]
        public float meshYawTurningRecoverySmoothingFactor = 0.5f;

        public float meshPitchTurningSpeed = 45.0f;
        [Range(0.0f, 100.0f)]
        public float meshPitchTurningSmoothingFactor = 4.0f;
        [Range(0.0f, 100.0f)]
        public float meshPitchTurningRecoverySmoothingFactor = 0.5f;

        public float maximumMeshRollAngle = 30.0f;
        [Range(0.0f, 100.0f)]
        public float meshRollTurningSmoothingFactor = 2.0f;
        [Range(0.0f, 100.0f)]
        public float meshRollTurningRecoverySmoothingFactor = 0.75f;
        public float meshTurningRecoveryDelay = 0.5f;

        [Header("Custom Attributes")]
        public bool calculatePowerConsumption = true;
        public float currentPower = 100.0f;
        public float maximumPower = 800.0f;
        public float powerDecreaseSpeed = 0.1f;
        public float powerDecreaseSpeedWhenBoosting = 0.25f;
        public AnimationCurve speedRemainingPowerRatioAnimationCurve;

        public bool calculateCarryingWeight = true;
        public float currentCarryingWeight = 0.5f;
        public float maximumCarryingWeight = 200.0f;
        public AnimationCurve speedCarryingWeightRatioAnimationCurve;

        // Flying attributes
        [HideInInspector]
        public bool enabledFlyingLogic = true;

        [HideInInspector]
        public bool inAir = false;

        [HideInInspector]
        public Vector3 flyingDirection;

        [HideInInspector]
        public float flyingSpeed;

        [HideInInspector]
        public Vector3 flyingVelocity;

        [HideInInspector]
        public bool flyingAtNormalSpeed = false;
        [HideInInspector]
        public bool boosting = false;

        private float currentFlyingSpeed;

        // Movement variables
        private bool isMovingForward = false;
        private float forwardDirection = 1.0f;

        // Turning variables
        private bool isTurning = false;
        private bool isLookingUp = false;
        private float targetMeshLocalRotationX, targetMeshLocalRotationY, targetMeshLocalRotationZ;

        [HideInInspector]
        public float powerPercentage = 1.0f;
        private float powerFactor = 1.0f;

        [HideInInspector]
        public float weightPercentage = 0.0f;
        private float weightFactor = 1.0f;

        void Update()
        {
            if (enabledFlyingLogic)
                Fly();
        }

        public void TakeOff()
        {
            inAir = true;
        }

        public void Land()
        {
            inAir = false;
        }

        public void AddForwardInput(float value)
        {
            isMovingForward = true;

            forwardDirection = value;
        }

        public void StopMovingForward()
        {
            isMovingForward = false;
        }

        public void AddYawInput(float value)
        {
            isTurning = true;

            if (isMovingForward)
            {
                if (forwardDirection > 0.0f)
                    targetMeshLocalRotationY += value * meshYawTurningSpeed * Time.deltaTime;
                else
                    targetMeshLocalRotationY -= value * meshYawTurningSpeed * Time.deltaTime;
            }
            else
                targetMeshLocalRotationY += value * meshYawTurningSpeed * Time.deltaTime;

            targetMeshLocalRotationZ = -value * maximumMeshRollAngle;
        }

        public void StopTurning()
        {
            if (!isMovingForward && !isLookingUp)
                Invoke("LateStopTurning", meshTurningRecoveryDelay);
            else
                LateStopTurning();
        }

        void LateStopTurning()
        {
            isTurning = false;
            targetMeshLocalRotationZ = 0.0f;
        }

        public void AddPitchInput(float value)
        {
            isLookingUp = true;
            targetMeshLocalRotationX += value * meshPitchTurningSpeed * Time.deltaTime;
        }

        public void StopPitchInput()
        {
            isLookingUp = false;
        }

        public void AddWeight(float increaseValue)
        {
            currentCarryingWeight += Mathf.Clamp(currentCarryingWeight + increaseValue, 0.0f, maximumCarryingWeight);
            weightPercentage = currentCarryingWeight / maximumCarryingWeight;
        }

        void Fly()
        {
            // Lerp the pitch, yaw, roll to their target values
            rollRootTransform.localRotation = Quaternion.Lerp(rollRootTransform.localRotation, Quaternion.Euler(0.0f, targetMeshLocalRotationY, rollRootTransform.localRotation.eulerAngles.z), meshYawTurningSmoothingFactor * Time.deltaTime);

            if (isTurning)
                rollRootTransform.localRotation = Quaternion.Lerp(rollRootTransform.localRotation, Quaternion.Euler(0.0f, rollRootTransform.localRotation.eulerAngles.y, targetMeshLocalRotationZ), meshRollTurningSmoothingFactor * Time.deltaTime);
            else
                rollRootTransform.localRotation = Quaternion.Lerp(rollRootTransform.localRotation, Quaternion.Euler(0.0f, rollRootTransform.localRotation.eulerAngles.y, 0.0f), meshRollTurningRecoverySmoothingFactor * Time.deltaTime);

            if (!isMovingForward && !isTurning && !isLookingUp && remainPitchZeroWhenHovering)
                targetMeshLocalRotationX = Mathf.Lerp(targetMeshLocalRotationX, 0.0f, meshPitchTurningRecoverySmoothingFactor * Time.deltaTime);

            meshRootTransform.localRotation = Quaternion.Lerp(meshRootTransform.localRotation, Quaternion.Euler(targetMeshLocalRotationX, 0.0f, 0.0f), meshPitchTurningSmoothingFactor * Time.deltaTime);

            if (calculatePowerConsumption)
            {
                // Power reduces faster when it is boosting
                if (inAir)
                {
                    if (boosting)
                        currentPower = Mathf.Clamp(currentPower - powerDecreaseSpeed * Time.deltaTime, 0.0f, maximumPower);
                    else
                        currentPower = Mathf.Clamp(currentPower - powerDecreaseSpeedWhenBoosting * Time.deltaTime, 0.0f, maximumPower);

                    powerPercentage = currentPower / maximumPower;
                    powerFactor = speedRemainingPowerRatioAnimationCurve.Evaluate(1.0f - powerPercentage);
                }
            }
            else
                powerFactor = 1.0f;

            if (calculateCarryingWeight)
            {
                // Carrying too much weight will slow down the speed
                weightPercentage = currentCarryingWeight / maximumCarryingWeight;
                weightFactor = speedCarryingWeightRatioAnimationCurve.Evaluate(weightPercentage);
            }
            else
                weightFactor = 1.0f;

            if (isMovingForward)
            {
                if (forwardDirection > 0.01f)
                {
                    if (boosting)
                        currentFlyingSpeed = Mathf.Clamp(currentFlyingSpeed + boostAcceleration * Time.deltaTime, -backwardSpeed, maximumForwardSpeed);
                    else
                    {
                        if (currentFlyingSpeed > normalForwardSpeed)
                            currentFlyingSpeed -= boostAcceleration * Time.deltaTime;
                        else
                            currentFlyingSpeed = Mathf.Clamp(currentFlyingSpeed + boostAcceleration * Time.deltaTime, -backwardSpeed, normalForwardSpeed);
                    }
                }
                else if (forwardDirection < -0.01f)
                {
                    currentFlyingSpeed = Mathf.Clamp(currentFlyingSpeed - boostAcceleration * Time.deltaTime, -backwardSpeed, maximumForwardSpeed);
                }

                flyingDirection = forwardDirection * meshRootTransform.forward;

                flyingSpeed = currentFlyingSpeed * powerFactor * weightFactor;
                flyingVelocity = meshRootTransform.forward * flyingSpeed;

                rootTransform.position += flyingVelocity * Time.deltaTime;
            }
            else
            {
                if (currentFlyingSpeed > 0.0f)
                    currentFlyingSpeed = Mathf.Clamp(currentFlyingSpeed - slowDownAcceleration * Time.deltaTime, 0.0f, normalForwardSpeed);
                else if (currentFlyingSpeed < 0.0f)
                    currentFlyingSpeed = Mathf.Clamp(currentFlyingSpeed + slowDownAcceleration * Time.deltaTime, -backwardSpeed, 0.0f);

                flyingDirection = forwardDirection * meshRootTransform.forward;

                flyingSpeed = currentFlyingSpeed * powerFactor * weightFactor;
                flyingVelocity = meshRootTransform.forward * flyingSpeed;

                rootTransform.position += flyingVelocity * Time.deltaTime;
            }
        }
    }
}