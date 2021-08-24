using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TestRanged : MonoBehaviour
{
    [SerializeField] Transform transMoveTo = default;
    // Start is called before the first frame update
    [SerializeField] Button button;
    [SerializeField] Animator animator;
    void Start()
    {
        animator.enabled = false;
        button.onClick.AddListener(()=>{
            animator.enabled = true;
        transform.DOMove(transMoveTo.position, 1f*(2f/3f)).OnComplete(()=> 
        {
            transform.DOMoveZ(0, 1f - 1f * 2f / 3f).OnComplete(() => { gameObject.SetActive(false); });
        });
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
