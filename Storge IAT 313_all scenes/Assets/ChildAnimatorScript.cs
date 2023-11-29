using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildAnimatorScript : MonoBehaviour
{
    public void AnimationEventMethod()
    {
        Debug.Log("AnimationEventMethod called");
        EnemyDamage parentScript = GetComponentInParent<EnemyDamage>();
        if (parentScript != null)
        {
            parentScript.ApplyAttackDamage();
        }
    }
    public void player_damage()
    {
        Debug.Log("AnimationEventMethod called");
        EnemyDamage parentScript = GetComponentInParent<EnemyDamage>();
        if (parentScript != null)
        {
            parentScript.ApplyAttackDamage();
        }
    }

}

