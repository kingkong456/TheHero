using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum player_state
{
    onGround,
    Jump,
    inAir,
    Hang,
    HangJump,
    Attack,
    Hook
}

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class player_controller : MonoBehaviour
    {
        //my componenet var
        private Animator m_animator;
        private ThirdPersonCharacter m_character;
        public player_state m_state;
        private bool m_jump;
        private Vector3 m_move;
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;
        private Rigidbody m_rb;

        [Header("Hanging Status")]
        public float HangJumpForce;
        public float forward_distanceCheck;
        public float forward_heightCheck;

        // Use this for initialization
        void Start()
        {
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            //set my varible
            m_rb = this.GetComponent<Rigidbody>();
            m_character = this.GetComponent<ThirdPersonCharacter>();
            m_animator = this.GetComponent<Animator>();
        }

        private void Update()
        {
            if(!m_jump)
            {
                m_jump = Input.GetKeyDown(KeyCode.K);
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Debug.DrawRay(transform.position + Vector3.up * forward_heightCheck, Vector3.right, Color.red, forward_distanceCheck);

            bool crouch = Input.GetKey(KeyCode.C);
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_move = v * Vector3.forward + h * Vector3.right;
            }

            // pass all parameters to the character control script
            m_character.Move(m_move, crouch, m_jump);
            m_jump = false;

            if(!m_character.m_IsGrounded)
            {
                checkToHangWall();
            }
        }

        void checkToHangWall()
        {
            if (check_forwardObj() != null)
            {
                if (check_forwardObj().tag == "Wall")
                {
                    m_animator.SetBool("isHang", true);
                    check_forwardObj().GetComponent<FixedJoint>().connectedBody = m_rb;
                }
            }
        }

        private GameObject check_forwardObj()
        {
            RaycastHit hit;

            if (transform.localScale.z > 0)
            {
                if (Physics.Raycast(transform.position + (Vector3.up * forward_heightCheck), Vector3.right, out hit, forward_distanceCheck))
                {
                    return hit.collider.gameObject;
                }
            }
            else if (transform.localScale.z < 0)
            {
                if (Physics.Raycast(transform.position + (Vector3.up * forward_heightCheck), Vector3.left, out hit, forward_distanceCheck))
                {
                    return hit.collider.gameObject;
                }
            }

            return null;
        }

        void checkEndHookHop()
        {
            GameObject objFound = check_forwardObj();

            if (objFound != null)
            {
                //found what
                //wall found 
                m_rb.velocity = Vector3.zero;

                if (objFound.tag == "Wall" || objFound.tag == "Ground")
                {
                    m_rb.useGravity = false;
                    m_animator.SetTrigger("Hang");
                }
            }
            else
            {
                //fall
                m_animator.SetBool("Fall", true);
            }
        }

        void HangJump()
        {
            m_rb.velocity = Vector3.up * HangJumpForce;
        }
    }
}
