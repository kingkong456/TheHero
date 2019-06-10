using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Hp : MonoBehaviour {
    public float hp_start;
    public float hp_current;
    public GameObject hp_txt_PopUp;
    public float txt_height;
    public bool isHurt = false;

    private Animator m_animator;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        hp_current = hp_start;
    }

    public void tacked_Danmge(float dmg)
    {
        hp_current -= dmg;
        //pop up txt
        spawnTextPopUp((int)hp_current);
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;

        this.GetComponent<Rigidbody>().velocity = Vector3.right * transform.localScale.z * 4 + (Vector3.up * 2);

        m_animator.SetTrigger("HitReact");

        if (hp_current <= 0)
        {
            //die
            m_animator.SetTrigger("die");
            //this.GetComponent<Collider>().enabled = false;
            Destroy(this.gameObject, 3f);
        }
    }

    void startHurt()
    {
        isHurt = true;
    }

    void endHurt()
    {
        isHurt = false;
    }

    void spawnTextPopUp(int txt)
    {
        GameObject go = Instantiate(hp_txt_PopUp, transform.position + (Vector3.up * txt_height), Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = txt.ToString();
    }
}
