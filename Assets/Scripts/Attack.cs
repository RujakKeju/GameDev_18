using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int attackDamage = 20;
    public Vector2 knockBack = Vector2.zero;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // lihat jika bisa kenak hit
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null )
        {
            Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knockBack : new Vector2(-knockBack.x, knockBack.y);
            // hit the target
            bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);

            if (gotHit)
            {
            Debug.Log(collision.name + " kenak cuy, damage nya = " + attackDamage);

            }
        }
    }
}
