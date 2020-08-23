using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class WeaponInteractionHandler : MonoBehaviour
{
    [Header("Prefab References")]
    public GameObject ProjectilePrefab;

    [Header("Audio References")]
    public AudioClip FireSound;

    [Header("Transform References")]
    public Transform GunMuzzle;

    [Header("SteamVR Action References")]
    public SteamVR_Action_Boolean FireWeapon;

    private Interactable _interactable;
    private AudioSource _audioSource;
    
    private void Awake()
    {
        if (!ProjectilePrefab)
        {
            Debug.LogWarning(gameObject.name.ToString() + " is missing a Prefab reference!");
        }

        if (!FireSound)
        {
            Debug.LogWarning(gameObject.name.ToString() + " is missing an Audio reference!");
        }

        if (!GunMuzzle)
        {
            Debug.LogWarning(gameObject.name.ToString() + " is missing a Transform reference!");
        }

        if (FireWeapon == null)
        {
            Debug.LogWarning(gameObject.name.ToString() + " is missing a SteamVR Action reference!");
        }

        if (GetComponent<Interactable>() != null)
        {
            _interactable = GetComponent<Interactable>();
            _interactable.onAttachedToHand += GetHandAction;
            _interactable.onDetachedFromHand += RemoveHandAction;
        }
        else
        {
            Debug.LogError("Failed to get Interactable on " + gameObject.name.ToString());
        }

        if (GetComponent<AudioSource>() != null)
        {
            _audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogError("Failed to get AudioSource on " + gameObject.name.ToString());
        }

        _audioSource.clip = FireSound;
    }

    private void OnDisable()
    {
        _interactable.onAttachedToHand -= GetHandAction;
        _interactable.onDetachedFromHand -= RemoveHandAction;
    }

    private void Update()
    {
        Debug.DrawRay(GunMuzzle.position, GunMuzzle.forward * 100.0f, Color.green);
    }

    void GetHandAction(Hand hand)
    {
        if (hand != null)
        {
            FireWeapon.AddOnStateDownListener(PlayFireSound, hand.handType);
            FireWeapon.AddOnStateDownListener(DoRayCast, hand.handType);
            FireWeapon.AddOnStateDownListener(FireProjectile, hand.handType);
        }
    }
    void RemoveHandAction(Hand hand)
    {
        if (hand != null)
        {
            FireWeapon.RemoveOnStateDownListener(PlayFireSound, hand.handType);
            FireWeapon.RemoveOnStateDownListener(DoRayCast, hand.handType);
            FireWeapon.RemoveOnStateDownListener(FireProjectile, hand.handType);
        }
    }

    private void PlayFireSound(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.LogWarning("BANG!");

        if (_audioSource.clip != FireSound)
        {
            _audioSource.clip = FireSound;
        }
        _audioSource.Play();
    }

    private void DoRayCast(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Ray ray = new Ray(GunMuzzle.position, GunMuzzle.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity) == true)
        {
            hit.collider.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    private void FireProjectile(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Instantiate(ProjectilePrefab, GunMuzzle.transform.position, GunMuzzle.rotation);
    }

}
