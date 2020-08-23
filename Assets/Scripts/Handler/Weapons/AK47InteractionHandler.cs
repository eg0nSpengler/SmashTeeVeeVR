using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class AK47InteractionHandler : MonoBehaviour
{
    [Header("Interactable References")]
    public Valve.VR.InteractionSystem.Interactable ParentInteractable;

    private FixedJoint _fixedJoint;

    private void Awake()
    {   
        if (ParentInteractable != null)
        {
            ParentInteractable.onAttachedToHand += GetAttachedHand;
        }
        else
        {
            Debug.LogError(gameObject.name.ToString() + " is missing an Interactable reference!");
        }

        if (GetComponent<FixedJoint>() != null)
        {
            _fixedJoint = GetComponent<FixedJoint>();
        }
        else
        {
            Debug.LogError("Failed to get FixedJoint on " + gameObject.name.ToString());
        }
    }

    private void GetAttachedHand(Hand hand)
    {
        _fixedJoint.connectedBody = hand.otherHand.GetComponent<Rigidbody>();
    }

}
