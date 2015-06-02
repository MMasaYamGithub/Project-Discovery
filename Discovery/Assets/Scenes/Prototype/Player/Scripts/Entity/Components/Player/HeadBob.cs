using System;
using UnityEngine;
using UnityStandardAssets.Utility;


// Edited version of Standard Asset Headbob
namespace DefinitiveStudios.Discovery.Prototype.Player.Entity.Components.Player
{
    public class HeadBob : MonoBehaviour
    {
        public Camera Camera;
        public CurveControlledBob motionBob = new CurveControlledBob();
        public LerpControlledBob jumpAndLandingBob = new LerpControlledBob();
        public PlayerController playerController;
        public float StrideInterval;
        [Range(0f, 1f)]
        public float RunningStrideLengthen;
        public float zOffset = 0;

        // private CameraRefocus m_CameraRefocus;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;


        private void Start()
        {
            motionBob.Setup(Camera, StrideInterval);
            m_OriginalCameraPosition = Camera.transform.localPosition;
            //     m_CameraRefocus = new CameraRefocus(Camera, transform.root.transform, Camera.transform.localPosition);
        }


        private void Update()
        {
            //  m_CameraRefocus.GetFocusPoint();
            Vector3 newCameraPosition;
            if (playerController.Velocity.magnitude > 0 && playerController.Grounded)
            {
                Camera.transform.localPosition = motionBob.DoHeadBob(playerController.Velocity.magnitude * (playerController.Running ? RunningStrideLengthen : 1f));
                newCameraPosition = Camera.transform.localPosition;
                newCameraPosition.y = Camera.transform.localPosition.y - jumpAndLandingBob.Offset();
                newCameraPosition.z += zOffset;
            }
            else
            {
                newCameraPosition = Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - jumpAndLandingBob.Offset();
            }
            Camera.transform.localPosition = newCameraPosition;

            if (!m_PreviouslyGrounded && playerController.Grounded)
            {
                StartCoroutine(jumpAndLandingBob.DoBobCycle());
            }

            m_PreviouslyGrounded = playerController.Grounded;
            //  m_CameraRefocus.SetFocusPoint();
        }
    }
}
