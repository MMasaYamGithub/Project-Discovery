using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;

// Edited version of the RigitbodyFirstPersonController standard asset to use a model like the ThirdPersonCharacter asset
namespace DefinitiveStudios.Discovery.Prototype.Player.Entity.Components.Player {

    // TODO: Update to work better with gravitational fields
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class PlayerController : MonoBehaviour {

        [Serializable]
        public class MovementSettings {
            public float ForwardSpeed = 3.0f;   // Speed when walking forward
            public float BackwardSpeed = 2.0f;  // Speed when walking backwards
            public float StrafeSpeed = 2.0f;    // Speed when walking sideways
            public float RunMultiplier = 2.0f;   // Speed when sprinting
            public KeyCode RunKey = KeyCode.LeftShift;
            public float JumpForce = 45f;
            public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
            [HideInInspector]
            public float CurrentTargetSpeed = 3f;
            [HideInInspector]
            public float TurnAmount = 0f;

#if !MOBILE_INPUT
            private bool running;
#endif

            public void UpdateDesiredTargetSpeed(Vector2 input) {
                if (input == Vector2.zero) {
                    CurrentTargetSpeed = 0;
                    return;
                }
                if (input.x > 0 || input.x < 0) {
                    //strafe
                    CurrentTargetSpeed = StrafeSpeed;
                }
                if (input.y < 0) {
                    //backwards
                    CurrentTargetSpeed = BackwardSpeed;
                }
                if (input.y > 0) {
                    //forwards
                    //handled last as if strafing and moving forward at the same time forwards speed should take precedence
                    CurrentTargetSpeed = ForwardSpeed;
                }
#if !MOBILE_INPUT
                if (Input.GetKey(RunKey)) {
                    CurrentTargetSpeed *= RunMultiplier;
                    running = true;
                } else {
                    running = false;
                }
#endif
            }

#if !MOBILE_INPUT
            public bool Running {
                get { return running; }
            }
#endif
        }


        [Serializable]
        public class AdvancedSettings {
            public float groundCheckDistance = 0.01f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )
            public float stickToGroundHelperDistance = 0.5f; // stops the character
            public float slowDownRate = 20f; // rate at which the controller comes to a stop when there is no input
            public bool airControl; // can the user control the direction that is being moved in the air
        }

        [Serializable]
        public class AnimationSettings {
            public float runCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
            public float animationSpeedScale = 6f; // The Animator uses values between 0 and 1 to map animations such as walking (0 to 0.5) and running (0.5 to 1). 
                                                   // We need to scale the current speed so that the walk animation will play when the character is walking.
                                                   // The alternative is to use absolute values: animator.SetFloat("Forward", (movementSettings.Running)? 1f : 0.5f, 0.1f, Time.deltaTime);
                                                   // but this won't scale with speed.
        }


        public Camera cam;
        public Animator animator;
        public MovementSettings movementSettings = new MovementSettings();
        public MouseLook mouseLook = new MouseLook();
        public AdvancedSettings advancedSettings = new AdvancedSettings();
        public AnimationSettings animationSettings = new AnimationSettings();


        private Rigidbody rigidBody;
        private CapsuleCollider capsule;
        private float yRotation;
        private Vector3 groundContactNormal;
        private bool jump, previouslyGrounded, jumping, isGrounded;


        public Vector3 Velocity {
            get { return rigidBody.velocity; }
        }

        public bool Grounded {
            get { return isGrounded; }
        }

        public bool Jumping {
            get { return jumping; }
        }

        public bool Running {
            get {
#if !MOBILE_INPUT
                return movementSettings.Running;
#else
	            return false;
#endif
            }
        }


        private void Start() {
            rigidBody = GetComponent<Rigidbody>();
            capsule = GetComponent<CapsuleCollider>();
            mouseLook.Init(transform, cam.transform);
        }


        private void Update() {
            RotateView();
            UpdateAnimator();

            if (CrossPlatformInputManager.GetButtonDown("Jump") && !jump) {
                jump = true;
            }
        }


        private void FixedUpdate() {
            GroundCheck();
            Vector2 input = GetInput();

            if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && (advancedSettings.airControl || isGrounded)) {
                // always move along the camera forward as it is the direction that it being aimed at
                Vector3 desiredMove = cam.transform.forward * input.y + cam.transform.right * input.x;
                desiredMove = Vector3.ProjectOnPlane(desiredMove, groundContactNormal).normalized;

                desiredMove.x = desiredMove.x * movementSettings.CurrentTargetSpeed;
                desiredMove.z = desiredMove.z * movementSettings.CurrentTargetSpeed;
                desiredMove.y = desiredMove.y * movementSettings.CurrentTargetSpeed;
                if (rigidBody.velocity.sqrMagnitude <
                    (movementSettings.CurrentTargetSpeed * movementSettings.CurrentTargetSpeed)) {
                    rigidBody.AddForce(desiredMove * SlopeMultiplier(), ForceMode.Impulse);
                }
            }

            if (isGrounded) {
                rigidBody.drag = 5f;

                if (jump) {
                    rigidBody.drag = 0f;
                    rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);
                    rigidBody.AddForce(new Vector3(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
                    jumping = true;
                }

                if (!jumping && Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && rigidBody.velocity.magnitude < 1f){
                    rigidBody.Sleep();
                }
            }
            else {
                rigidBody.drag = 0f;
                if (previouslyGrounded && !jumping) {
                    StickToGroundHelper();
                }
            }
            jump = false;
        }


        private float SlopeMultiplier() {
            float angle = Vector3.Angle(groundContactNormal, Vector3.up);
            return movementSettings.SlopeCurveModifier.Evaluate(angle);
        }


        private void StickToGroundHelper() {
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, capsule.radius, Vector3.down, out hitInfo,
                                   ((capsule.height / 2f) - capsule.radius) +
                                   advancedSettings.stickToGroundHelperDistance))
            {
                if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f) {
                    rigidBody.velocity = Vector3.ProjectOnPlane(rigidBody.velocity, hitInfo.normal);
                }
            }
        }


        private Vector2 GetInput() {

            Vector2 input = new Vector2 {
                x = CrossPlatformInputManager.GetAxis("Horizontal"),
                y = CrossPlatformInputManager.GetAxis("Vertical")
            };
            movementSettings.UpdateDesiredTargetSpeed(input);
            return input;
        }


        private void RotateView() {
            //avoids the mouse looking if the game is effectively paused
            if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

            // get the rotation before it's changed
            float oldYRotation = transform.eulerAngles.y;

            mouseLook.LookRotation(transform, cam.transform);

            if (isGrounded || advancedSettings.airControl) {
                // Rotate the rigidbody velocity to match the new direction that the character is looking
                Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, rigidBody.transform.up);
                rigidBody.velocity = velRotation * rigidBody.velocity;

                // Get a smoothed rotation difference to animate turning. Animator turn range is -1 to 1, hence the smoothing and clamping. 
                movementSettings.TurnAmount = Mathf.Clamp((transform.eulerAngles.y - oldYRotation) * 0.1f, -1, 1);
            }
        }


        /// sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
        private void GroundCheck() {
            previouslyGrounded = isGrounded;
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, capsule.radius, Vector3.down, out hitInfo,
                                   ((capsule.height / 2f) - capsule.radius) + advancedSettings.groundCheckDistance)) {
                isGrounded = true;
                groundContactNormal = hitInfo.normal;
            }
            else {
                isGrounded = false;
                groundContactNormal = rigidBody.transform.up;
            }
            if (!previouslyGrounded && isGrounded && jumping) {
                jumping = false;
            }
        }

        void UpdateAnimator() {
            // update the animator parameters
            animator.SetFloat("Forward", movementSettings.CurrentTargetSpeed/animationSettings.animationSpeedScale, 0.1f, Time.deltaTime);            
            animator.SetFloat("Turn", movementSettings.TurnAmount, 0.1f, Time.deltaTime);
            //animator.SetBool("Crouch", m_Crouching);
            animator.SetBool("OnGround", isGrounded);
            if (!isGrounded) {
                animator.SetFloat("Jump", rigidBody.velocity.y);
            }

            // calculate which leg is behind, so as to leave that leg trailing in the jump animation
            // (This code is reliant on the specific run cycle offset in our animations,
            // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
            float runCycle = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime + animationSettings.runCycleLegOffset, 1);
            float jumpLeg = (runCycle < 0.5f/*k_half*/ ? 1 : -1) * movementSettings.CurrentTargetSpeed;
            if (isGrounded) {
                animator.SetFloat("JumpLeg", jumpLeg);
            }

            // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
            // which affects the movement speed because of the root motion.
            if (isGrounded) {
                animator.speed = 1f; //m_AnimSpeedMultiplier
            } else {
                // don't use that while airborne
                animator.speed = 1;
            }
        }
    }
}
