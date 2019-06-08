using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy_meleeBehavevior : MonoBehaviour {
    private Enemy_Seeker m_eye;
    private Rigidbody m_rb;
    private Transform playerSee;

    [Header("enemy status")]
    public float speed;
    public float enemy_scene;
    public float enemy_stopDistance;

    //selft state
    private bool isCanFollowPlayer = false;

	// Use this for initialization
	void Start ()
    {
        //set my varible
        m_rb = this.GetComponent<Rigidbody>();
        m_eye = this.GetComponent<Enemy_Seeker>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_eye.checkPlayerInRange();

        //walk to player
        if(m_eye.PlayerHadsee != null)
        {
            playerSee = m_eye.PlayerHadsee;
            if (Vector3.Distance(transform.position, playerSee.position) <= enemy_stopDistance)
            {
                //start attack

                return;
            }
            enemy_walkToPlayer();
        }
        else if(m_eye.PlayerHadsee == null)//iDle
        {
            check_isCanFollowPlayer();

            //can't follow or can
            //walk to player
            if(isCanFollowPlayer)
            {
                if (Vector3.Distance(transform.position, playerSee.position) <= enemy_stopDistance)
                {
                    return;
                }
                enemy_walkToPlayer();
            }
            
        }
	}

    //*********************ENEMY ACTION*****************
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

    //********************BUILD IN********************
    #region build in

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemy_scene);
    }

    #endregion
}
