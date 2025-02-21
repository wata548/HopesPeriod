using System;
using UnityEngine;

public class MonsterContactCheck : MonoBehaviour {
     
     //TODO: Invincibilty is not staic
     private bool Contact(GameObject contact) {
          
          var checkDamage = contact.GetComponent<IDamageAble>();
          if (checkDamage == null) {
               return false;
          }
         
          var damage = checkDamage.Damage;
          if (damage <= 0)
               return false;
          if (!Invincibility.Instance.TurnOn()) {
               return false;
          }

          CharactersInfoBattle.Instance.DamageDistribute(damage);
          Debug.Log("Damaged");
          return true;
     } 
     
     private void OnCollisionEnter2D(Collision2D other) {

          Contact(other.gameObject);
     }

     private void OnTriggerEnter2D(Collider2D other) {

          Contact(other.gameObject);
     }
}