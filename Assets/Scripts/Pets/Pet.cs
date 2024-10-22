﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum PetType
{
    Bear,
    Panda,
    Chicken,
    Dog,
    Crocodile,
    Cow,
}

public class Pet : MonoBehaviour
{
    protected Selections selections => FindObjectOfType<Selections>();

    protected bool selected;

    [Header("Pet")]
    public int scoreValue = 5;
    public PetType petId;
    public float destroyDuration = 0.5f;
    [SerializeField] GameObject explosionFx;

    public void Clear()
    {
        selected = false;
    }

    public virtual void Death(float bonusDelay)
    {
        StartCoroutine(DestroyPet(bonusDelay, this));
    }

    IEnumerator DestroyPet(float duration, Pet pet)
    {
        yield return new WaitForSeconds(duration);
        pet.transform.GetChild(0).DOScale(Vector3.one * 1.5f, (destroyDuration + duration) / 2);
        pet.transform.GetChild(0).DOScale(Vector3.one * 0.1f, destroyDuration + duration).SetDelay((destroyDuration + duration) / 2);
        Instantiate(explosionFx, pet.transform.position, Quaternion.identity);
        if (selections.allPets.Contains(pet))
            selections.allPets.Remove(pet);

        EventManager.Instance.onNewPet.Invoke(duration + pet.destroyDuration);
        Destroy(pet.gameObject, destroyDuration + duration);
    }
}
