using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

/// <summary>
/// Randomizes the color of the material assigned to the currently held Interactable object
/// </summary>
public class RandomInteractableColor : MonoBehaviour
{
        [Header("Audio References")]
        public AudioClip ItemSpinSound;

        private Valve.VR.InteractionSystem.Interactable _interactable;
        private AudioSource _audioSource;

        private void Awake()
        {
            if (!ItemSpinSound)
            {
                Debug.LogWarning(gameObject.name.ToString() + "is missing an Audio Reference!");
            }

            if (GetComponent<Valve.VR.InteractionSystem.Interactable>() != null)
            {
                _interactable = GetComponent<Valve.VR.InteractionSystem.Interactable>();
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
                Debug.LogError("Failed to get AudioSource on" + gameObject.name.ToString());
            }
                
            if (GetComponent<MeshRenderer>() == null)
            {
                Debug.LogError("Failed to get MeshRenderer on " + gameObject.name.ToString());
            }

            _interactable.onAttachedToHand += ChangeColor;
            _interactable.onDetachedFromHand += StopChangeColor;
        }

        private void OnDisable()
        {
            _interactable.onAttachedToHand -= ChangeColor;
            _interactable.onDetachedFromHand -= StopChangeColor;
            StopAllCoroutines();
        }

        void ChangeColor(Valve.VR.InteractionSystem.Hand hand)
        {
            StartCoroutine(ChangeColorRoutine(hand));
        }

        void StopChangeColor(Valve.VR.InteractionSystem.Hand hand)
        {
            StopCoroutine(ChangeColorRoutine(hand));
            _audioSource.loop = false;
            _audioSource.Stop();
        }

        IEnumerator ChangeColorRoutine(Valve.VR.InteractionSystem.Hand hand)
        {
            _audioSource.loop = true;
            _audioSource.clip = ItemSpinSound;
            _audioSource.Play();

            while (hand.currentAttachedObject == gameObject)
            {
                var randColor = Random.ColorHSV();
                yield return new WaitForSeconds(0.05f);
                GetComponent<MeshRenderer>().material.color = randColor;
                yield return null;
            }
        }
}
