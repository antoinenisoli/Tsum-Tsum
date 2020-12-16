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
        foreach (var item in hits)
        {
            Pet thisPet = item.GetComponent<Pet>();
            if (thisPet != this)
                thisPet.Death(thisPet.destroyDuration);
        }

        Death(destroyDuration);
    }
}
