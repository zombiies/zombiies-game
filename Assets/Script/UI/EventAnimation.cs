using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAnimation : MonoBehaviour
{
    [SerializeField] Transform _oldTranformParent;
    private Vector3 _oldPos;
    private void Awake()
    {
        _oldPos = _oldTranformParent.localPosition;
    }
    public void EndAnim()
    {
        transform.parent.localPosition = _oldPos;
        transform.parent.gameObject.SetActive(false);
    }    
}
