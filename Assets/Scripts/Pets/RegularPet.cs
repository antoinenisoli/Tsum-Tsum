using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RegularPet : Pet
{
    [Serializable]
    class TypeRate
    {
        public string name => myType.ToString();
        public PetType myType;
        [Range(0, 1)]
        public float rate = 0.5f;
        public Sprite thisSprite;
    }

    SpriteRenderer spr => GetComponentInChildren<SpriteRenderer>();

    [Header("Regular Pet")]
    [SerializeField] Material lightMat;
    Material baseMat;
    [SerializeField] TypeRate[] availableTypes = new TypeRate[System.Enum.GetValues(typeof(PetType)).Length];

    private void Awake()
    {
        baseMat = spr.material;
        bool done = false;
        while (!done)
        {
            foreach (var type in availableTypes)
            {
                float randomValue = UnityEngine.Random.value;
                if (randomValue > (1 - type.rate))
                {
                    petId = type.myType;
                    spr.sprite = type.thisSprite;
                    done = true;
                    break;
                }
            }
        }
    }

    private void OnMouseEnter()
    {
        if (!Input.GetMouseButton(0))
            return;

        if (petId == selections.myPetReference.petId)
        {
            if (selected == false)
            {
                selected = true;
                if (!selections.selectedPets.Contains(this))
                    selections.selectedPets.Add(this);
            }
            else
            {
                selected = false;
                if (selections.selectedPets.Contains(this))
                    selections.selectedPets.Remove(this);
            }
        }
    }

    public void Select()
    {
        selections.myPetReference = this;
        selected = true;
        if (!selections.selectedPets.Contains(this))
            selections.selectedPets.Add(this);
    }

    private void Update()
    {
        spr.material = selections.selectedPets.Contains(this) ? lightMat : baseMat;
    }
}
