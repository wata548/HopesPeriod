using System;
using Unity.VisualScripting;
using UnityEngine;



public class ContactChecker : MonoBehaviour {
    private ContactInfo contactInfo = null;
    private Direction checkerDirection;

    public ContactChecker SetDirection(Direction dir) {

        checkerDirection = dir;
        return this;
    }

    public ContactChecker SetContactInfo(ContactInfo contactInfo) {
        this.contactInfo = contactInfo;
        return this;
    }
    
    private void OnTriggerEnter2D(Collider2D collision) {

        if (contactInfo == null)
            return;
        
        if(collision.CompareTag("Field") || collision.CompareTag("Structures")) {

            contactInfo.ContactIn(checkerDirection);
        }
    } 
    private void OnTriggerExit2D(Collider2D collision) {

        if (contactInfo == null)
            return;
        
        if (collision.CompareTag("Field") || collision.CompareTag("Structures")) {
         
            contactInfo.ContactOut(checkerDirection);
        }
    }
}
