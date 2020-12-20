using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PetBomb : BonusPet
{
    [Header("Pet Bomb")]
    [SerializeField] float effectRadius;
    [SerializeField] LayerMask petLayer;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, effectRadius);
    }

    public override void Death(float bonusDelay)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, effectRadius, petLayer);
        List<Pet> pets = new List<Pet>();
        foreach (var hit in hits) { pets.Add(hit.GetComponent<Pet>()); }
        selections.SendScore(pets);

        foreach (var pet in pets)
        {
            if (pet != this)
                pet.Death(0);
        }

        pets.Clear();
        base.Death(bonusDelay);
    }
}
