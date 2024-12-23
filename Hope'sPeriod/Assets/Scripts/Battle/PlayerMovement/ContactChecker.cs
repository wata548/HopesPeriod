using System;
using Unity.VisualScripting;
using UnityEngine;

class ContactInfo {

    private static Direction contact = DirectionInfo.NONE;

    public static bool Check(Direction dir) {

        return Convert.ToByte(contact & dir) != 0;
    }

    public static void ContactIn(Direction dir) {

        contact |= dir;
    }

    public static void ContactOut(Direction dir) {

        contact ^= dir;
    }
}

public class ContactChecker : MonoBehaviour
{
    private Direction checkerDirection;

    public void SetDirection(Direction dir) {

        checkerDirection = dir;
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if(collision.CompareTag("Field") || collision.CompareTag("Structures")) {

            ContactInfo.ContactIn(checkerDirection);
        }
    } 
    private void OnTriggerExit2D(Collider2D collision) {

        if (collision.CompareTag("Field") || collision.CompareTag("Structures")) {
         
            ContactInfo.ContactOut(checkerDirection);
        }
    }
}
