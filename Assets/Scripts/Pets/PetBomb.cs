using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetBomb : BonusPet
{
    [SerializeField] float effectRadius;
    [SerializeField] LayerMask petLayer;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, effectRadius);
    }

    public override void Effect()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, effectRadius, petLayer);
        List<Pet> pets = new List<Pet>();
        foreach (var hit in hits)
        {
            Pet thisPet = hit.GetComponent<Pet>();
            pets.Add(thisPet);

            if (thisPet != this)
                thisPet.Death(0);
        }

        foreach (var item in pets)
        {
            EventManager.Instance.onNewPet.Invoke(item.destroyDuration + 0.5f);
        }

        print(pets.Count);
        pets.Clear();
        selections.SendScore(pets);
        Death(0);
    }
}
