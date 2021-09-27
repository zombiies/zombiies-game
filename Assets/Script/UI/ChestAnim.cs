using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAnim : MonoBehaviour
{
    [SerializeField] Animator animator = default;
    [SerializeField] GameObject objCard = default;
     
    public void SetAnimation(bool isOpen)
    {
        animator.SetBool("isOpen", isOpen);
    }
    public void WhenChestOpen()
    {
        objCard.SetActive(true);
    }
}
