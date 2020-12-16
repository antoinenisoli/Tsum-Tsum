using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RegularPet : Pet
{
    SpriteRenderer spr => GetComponentInChildren<SpriteRenderer>();

    [SerializeField] Sprite[] mySprites;
    public int scoreValue = 5;
    public int petId;

    [SerializeField] Material lightMat;
    Material baseMat;

    private void Awake()
    {
        baseMat = spr.material;
        petId = Random.Range(0, mySprites.Length);
        spr.sprite = mySprites[petId];
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
