using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerNUmber2 : MonoBehaviour {
    public player_state m_state;
    private Rigidbody m_rb;
    private Animator m_animator;

    [Header("player Status")]
    public float speed_groundMove;
    public float speed_airSpeed;
    public float jump_force;
    public float jump_forceHang;
    public float attack_power;

    [Header("check")]
    public float check_groundDis;
    public float check_forward_heightCheck;
    public float check_forward_distanceCheck;
    public float check_headForward_height;

    [Header("Hook Crabibg")]
    public Transform grabing_point;
    public float grabing_range;
    public float grabing_swipe_force;
    public float grabing_jump_force;

    [Header("genaral")]
    public Vector2 climb_pos;
    public Collider sword_col;

    private FixedJoint wall;

    // Use this for initialization
    void Start ()
    {
        this.GetComponent<SpringJoint>().spring = 0;
        this.GetComponent<SpringJoint>().damper = 0;

        sword_col.enabled = false;
        m_animator = this.GetComponent<Animator>();
        m_rb = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
		if(m_state == player_state.onGround)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                m_state = player_state.on_ui;
                PlayerSkill.instace.open_skill_navegation();
                return;
            }

            checked_ground();
            m_animator.ResetTrigger("runAttack");
            m_animator.SetBool("Fall", false);
            m_animator.ResetTrigger("Jump");
            playerJump();
            playerMove();
            playerAttack();
        }
        else if(m_state == player_state.inAir)
        {
            checked_grabingPointInRange();
            checked_ground();
            m_animator.SetBool("isRun", false);
            m_animator.SetBool("Fall", true);
            checked_wallToGrabing();
            playerAirMove();
            playerToGrabing();
        }
        else if(m_state == player_state.Hook)
        {
            playerOnHook();
        }
        else if(m_state == player_state.Hang)
        {
            m_animator.SetBool("Fall", false);
            playerHang();
        }
        else if(m_state == player_state.Attack)
        {
            m_animator.SetBool("isRun", false);
            playerAttack();
        }
	}

    //*****************ACTION*******************
    #region player action

    void playerMove()
    {
        if(Input.GetKey(KeyCode.A))
        {
            //move left
            m_rb.velocity = Vector3.up * m_rb.velocity.y + (Vector3.left * speed_groundMove);
            m_animator.SetBool("isRun", true);

            if(transform.localScale.z > 0)
            {
                transform.localScale = new Vector3(1,1,-1);
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //move left
            m_rb.velocity = Vector3.up * m_rb.velocity.y + (Vector3.right * speed_groundMove);
            m_animator.SetBool("isRun", true);

            if (transform.localScale.z < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            m_animator.SetBool("isRun", false);
        }
    }

    void playerJump()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            m_animator.SetTrigger("Jump");
            m_rb.velocity = new Vector3(m_rb.velocity.x, jump_force, m_rb.velocity.z);
        }
    }
    
    void playerAirMove()
    {
        if(m_rb.velocity.x > 0 && Input.GetKey(KeyCode.A))
        {
            m_rb.velocity = m_rb.velocity + (Vector3.left * speed_airSpeed);
            transform.localScale = new Vector3(1,1,-1);
        }
        else if(m_rb.velocity.x < 0 && Input.GetKey(KeyCode.D))
        {
            m_rb.velocity = m_rb.velocity + (Vector3.right * speed_airSpeed);
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void playerHang()
    {
        //drop
        if(Input.GetKeyDown(KeyCode.S))
        {
            m_animator.SetBool("isHang", false);
        }
        else if(Input.GetKeyDown(KeyCode.K))//jump
        {
            if(check_forwardObj(check_headForward_height) != null && check_forwardObj(check_headForward_height).tag == "Ground")
            {
                m_animator.SetTrigger("Climb");
            }
            else
            {
                m_animator.SetTrigger("HangJump");
            }
        }
    }

    int animation_index = 0;

    void playerAttack()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            //attack
            if(m_rb.velocity != Vector3.zero)
            {
                m_animator.SetTrigger("runAttack");
            }
            else
            {
                animation_index = Random.Range(1, 4);

                m_animator.SetInteger("Attack", animation_index);
                m_animator.speed = 1 + (PlayerPrefs.GetInt("water_lv", 0) * 0.25f);
            }

            m_rb.velocity = Vector3.zero;
            m_state = player_state.Attack;
        }
        else if(Input.GetKey(KeyCode.I))
        {
            //use skill
            m_state = player_state.Attack;
            m_rb.velocity = Vector3.zero;
        }
    }

    void playerToGrabing()
    { 
        if (grabing_point != null)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                //this.GetComponent<SpringJoint>().connectedBody = grabing_point.GetComponent<Rigidbody>();
                this.GetComponent<SpringJoint>().spring = 1000;
                this.GetComponent<SpringJoint>().damper = 1;
                this.GetComponent<SpringJoint>().maxDistance = Vector3.Distance(transform.position, grabing_point.position);
                this.GetComponent<SpringJoint>().minDistance = Vector2.Distance(transform.position, grabing_point.position);
                this.GetComponent<SpringJoint>().connectedAnchor = grabing_point.position;
                m_animator.SetBool("Hanging", true);
                m_state = player_state.Hook;
            }
        }
    }

    void playerOnHook()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            //jump
            this.GetComponent<SpringJoint>().spring = 0;
            this.GetComponent<SpringJoint>().damper = 0;
            m_rb.velocity = new Vector3(m_rb.velocity.x, grabing_jump_force, m_rb.velocity.z);
            m_animator.SetBool("Hanging", false);
            m_state = player_state.inAir;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            m_rb.velocity = Vector3.up * m_rb.velocity.y + (Vector3.left * grabing_swipe_force);

            if (transform.localScale.z > 0)
            {
                transform.localScale = new Vector3(1, 1, -1);
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            m_rb.velocity = Vector3.up * m_rb.velocity.y + (Vector3.right * grabing_swipe_force);

            if (transform.localScale.z < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    #endregion

    //***************CHECKED********************
    #region player checked

    void checked_ground()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, out hit, check_groundDis))
        {
            m_state = player_state.onGround;
        }
        else
        {
            m_state = player_state.inAir;
        }
    }

    private GameObject check_forwardObj(float height)
    {
        RaycastHit hit;

        if (transform.localScale.z > 0)
        {
            if (Physics.Raycast(transform.position + (Vector3.up * height), Vector3.right, out hit, check_forward_distanceCheck))
            {
                return hit.collider.gameObject;
            }
        }
        else if (transform.localScale.z < 0)
        {
            if (Physics.Raycast(transform.position + (Vector3.up * height), Vector3.left, out hit, check_forward_distanceCheck))
            {
                return hit.collider.gameObject;
            }
        }

        return null;
    }

    void checked_wallToGrabing()
    {
        if(check_forwardObj(check_forward_heightCheck) != null)
        {
            if (check_forwardObj(check_forward_heightCheck).GetComponent<FixedJoint>() != null)
            {
                //hang
                m_animator.SetBool("isHang", true);
                check_forwardObj(check_forward_heightCheck).GetComponent<FixedJoint>().connectedBody = m_rb;
                wall = check_forwardObj(check_forward_heightCheck).GetComponent<FixedJoint>();
                m_state = player_state.Hang;
            }
        }
    }

    void checked_grabingPointInRange()
    {
        GameObject[] grabing_points = GameObject.FindGameObjectsWithTag("GrabingPoint");
        float shortestDistance = Mathf.Infinity;
        Transform nearestPoint = null;

        foreach (GameObject point in grabing_points)
        {
            float distanceToCurrentPoint = Vector3.Distance(transform.position, point.transform.position);

            //shortest grabing
            if(distanceToCurrentPoint < shortestDistance)
            {
                shortestDistance = distanceToCurrentPoint;
                nearestPoint = point.transform;
            }
        }

        //check in range?
        if (shortestDistance <= grabing_range)
        {
            grabing_point = nearestPoint;
        }
        else if (shortestDistance > grabing_range)
        {
            grabing_point = null;
        }
    }

    #endregion

    //**************ANIMATION EVENT*********
    #region Animation event

    void playerDrop()
    {
        wall.connectedBody = null;
        m_rb.isKinematic = true;
        m_rb.velocity = Vector3.right * transform.localScale.z * -1 * 3;
    }

    void playerEndDrop()
    {
        m_rb.isKinematic = false;
        transform.localScale = new Vector3(1, 1, transform.localScale.z * -1);
        m_state = player_state.inAir;
    }

    private RigidbodyConstraints M_RBINFORFIX;

    void playerHangJump()
    {
        M_RBINFORFIX = m_rb.constraints;
        m_rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        wall.connectedBody = null;
        m_rb.velocity = m_rb.velocity + (Vector3.up * jump_forceHang);
    }

    void playerEndHangJump()
    {
        m_rb.constraints = M_RBINFORFIX;
        m_animator.ResetTrigger("HangJump");
        m_state = player_state.inAir;
    }

    void Fall()
    {
        m_state = player_state.inAir;
    }

    void playerEndClimb()
    {
        transform.position = new Vector3(transform.position.x + (transform.localScale.x * climb_pos.x), transform.position.y + climb_pos.y, transform.position.z); 
    }

    void playerStartAttack()
    {
        //open attac colider
        m_animator.SetInteger("Attack", 0);
        m_animator.ResetTrigger("runAttack");
        sword_col.enabled = true;
        if (PlayerPrefs.GetInt("fire_lv", 0) == 0)
        {
            sword_col.GetComponent<weaponCollider>().dmg = attack_power;
        }
        else
        {
            sword_col.GetComponent<weaponCollider>().dmg = attack_power + (PlayerPrefs.GetInt("fire_lv", 0) * 0.5f);
        }
    }

    void playerEndAttack()
    {
        //off attack colider
        if (m_animator.GetInteger("Attack") == 0)
        {
            m_state = player_state.onGround;
        }
        sword_col.enabled = false;
        sword_col.GetComponent<weaponCollider>().dmg = attack_power;

        if(m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && m_animator.GetInteger("Attack") == 1)
        {
            Debug.Log("case1");
            m_animator.SetInteger("Attack", 0);
            m_state = player_state.onGround;
        }
        else if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && m_animator.GetInteger("Attack") == 2)
        {
            Debug.Log("case2");
            m_animator.SetInteger("Attack", 0);
            m_state = player_state.onGround;
        }
        else if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("attack3") && m_animator.GetInteger("Attack") == 3)
        {
            Debug.Log("case3");
            m_animator.SetInteger("Attack", 0);
            m_state = player_state.onGround;
        }

        m_animator.speed = 1f;
    }

    void Hook()
    {
        m_state = player_state.Hook;
    }

    #endregion

    //************Build in function************
    #region Build in function

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, grabing_range);
    }

    #endregion
}
