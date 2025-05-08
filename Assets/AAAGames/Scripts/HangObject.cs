using System;
using UnityEngine;

public class HangObject : MonoBehaviour
{
    public HingeJoint _HingeJoint;

    void Start()
    {
        _HingeJoint = GetComponent<HingeJoint>();

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Connect"))
        {
            if (_HingeJoint.connectedBody == null)
            {
                _HingeJoint.connectedBody = other.gameObject.GetComponent<Rigidbody>();
            }
        }
    }
}