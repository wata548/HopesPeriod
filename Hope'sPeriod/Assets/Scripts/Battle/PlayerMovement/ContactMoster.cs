using System;
using UnityEngine;

public class ContactMoster : MonoBehaviour {
     
     private bool Contact(GameObject contact) {
          
          var checkDamage = contact.GetComponent<IDamageAble>();
          if (checkDamage == null) {
               return false;
          }
         
          var damage = checkDamage.Damage;
          if (!Invincibility.Instance.TurnOn()) {
               return false;
          }

          Debug.Log("fuck Damage");
          return true;
     } 
     
     private void OnCollisionEnter2D(Collision2D other) {

          Contact(other.gameObject);
     }

     private void OnTriggerEnter2D(Collider2D other) {

          Contact(other.gameObject);
     }
}