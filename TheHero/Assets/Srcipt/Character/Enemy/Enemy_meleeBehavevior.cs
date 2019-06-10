using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy_meleeBehavevior : MonoBehaviour {
    private Enemy_Seeker m_eye;
    private Rigidbody m_rb;
    private Transform playerSee;
    private Animator m_animator;

    [Header("enemy status")]
    public float speed;
    public float enemy_scene;
    public float enemy_stopDistance;

    [Header("Attack status")]
    public float attack_power;
    public Collider attack_col;

    //selft state
    private bool isCanFollowPlayer = false;

	// Use this for initialization
	void Start ()
    {
        attack_col.enabled = false;

        //set my varible
        m_rb = this.GetComponent<Rigidbody>();
        m_eye = this.GetComponent<Enemy_Seeker>();
        m_animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if(this.GetComponent<Enemy_Hp>().isHurt)
        {
            return;
        }

        m_eye.checkPlayerInRange();

        //walk to player
        if(m_eye.PlayerHadsee != null)
        {
            playerSee = m_eye.PlayerHadsee;
            m_animator.SetBool("isRun", true);

            if (Vector3.Distance(transform.position, playerSee.position) <= enemy_stopDistance)
            {
                //start attack
                m_animator.SetBool("isRun", false);
                m_animator.SetTrigger("Attack");

                return;
            }
            enemy_walkToPlayer();
        }
        else if(m_eye.PlayerHadsee == null)//iDle
        {
            check_isCanFollowPlayer();
            m_animator.SetBool("isRun", false);

            //can't follow or can
            //walk to player
            if(isCanFollowPlayer)
            {
                m_animator.SetBool("isRun", true);

                if (Vector3.Distance(transform.position, playerSee.position) <= enemy_stopDistance)
                {
                    m_animator.SetBool("isRun", false);
                    m_animator.SetTrigger("Attack");

                    return;
                }
                enemy_walkToPlayer();
            }
            
        }
	}

    //*********************ENEMY ACTION***************
    #region enemy action

    void enemy_walkToPlayer()
    {
        if(transform.position.x > playerSee.position.x)
        {
            m_rb.velocity = Vector3.left * speed;

            if(transform.localScale.z < 0)
            {
                enemy_TurnBack();
            }
        }
        else if(transform.position.x < playerSee.position.x)
        {
            m_rb.velocity = Vector3.right * speed;

            if (transform.localScale.z > 0)
            {
                enemy_TurnBack();
            }
        }
    }

    void enemy_TurnBack()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -1 * transform.localScale.z);
    }

    #endregion

    //********************CHECK***********************
    #region check

    void check_isCanFollowPlayer()
    {
        if(playerSee == null)
        {
            return;
        }

        if(Vector3.Distance(transform.position, playerSee.position) <= enemy_scene)
        {
            isCanFollowPlayer = true;
        }
        else
        {
            isCanFollowPlayer = false;
            playerSee = null;
        }
    }

    #endregion

    //********************BUILD IN*********************
    #region build in

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemy_scene);
    }

    #endregion

    //*******************ANAMATION EVENT***************
    #region Animation event

    void start_attack()
    {
        m_animator.ResetTrigger("Attack");
        attack_col.GetComponent<weaponCollider>().dmg = attack_power;
        attack_col.enabled = true;
    }

    void end_attack()
    {
        attack_col.enabled = false;
    }

    #endregion

}
