using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAIAttackController : MonoBehaviour
{
    private bool isAttacking;

    private bool closeHinge;

    public bool CloseHinge { get => closeHinge; set => closeHinge = value; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerTarget")
        {
            Debug.Log("Attack");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "PlayerTarget")
        {
            if (!isAttacking)
            {
                isAttacking = true;
                closeHinge = true;
                Invoke("OpenHinge", 1.0f);
                Invoke("EndAttack", 2.0f);
            }
        }
    }

    void OpenHinge()
    {
        closeHinge = false;
    }

    void EndAttack()
    {
        isAttacking = false;
    }
}
