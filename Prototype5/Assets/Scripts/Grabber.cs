using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    bool isHoldingObject = false;
    GameObject currentObject;
    public Camera cam;
    public Transform holdingPosition;

    private void Start() {
    }
    void Update() {

        if (Input.GetMouseButtonDown(1)) {
            RaycastHit hit;
            // Does the ray intersect any objects
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
                if (hit.distance < 2) {
                    GameObject hitObj = hit.transform.gameObject;
                    if (!isHoldingObject) {
                        Pickupable pickup = hitObj.GetComponent<Pickupable>();
                        if (pickup != null && (pickup.spot == null || pickup.spot.pickupable)) {
                            pickupObject(hitObj);
                        } else if (hitObj.GetComponent<Dispenser>() != null) {
                            hitObj.GetComponent<Dispenser>().dispenseItem();
                        }
                    } else if (isHoldingObject) {
                        Spot spot = hitObj.GetComponent<Spot>();
                        if (spot != null && spot.currentObject == null && spot.placeable) {
                            dropObject(spot);
                        } else if (hitObj.GetComponent<Recycler>() != null) {
                            dropObject(null);
                        }
                    }
                }
            }
        }
    }

    void pickupObject(GameObject newObj) {
        currentObject = newObj;
        Debug.Log("Picked up object");
        isHoldingObject = true;
        currentObject.transform.parent = holdingPosition;
        currentObject.transform.localPosition = new Vector3(0, 0, 0);
        currentObject.transform.rotation = holdingPosition.rotation;
        Spot spot = currentObject.GetComponent<Pickupable>().spot;
        if (spot != null) {
            currentObject.GetComponent<Pickupable>().spot = null;
            spot.currentObject = null;
        }
    }

    void dropObject(Spot newSpot) {
        if (newSpot != null) {
            currentObject.transform.parent = null;
            currentObject.transform.position = newSpot.transform.position;
            currentObject.transform.rotation = newSpot.transform.rotation;
            newSpot.currentObject = currentObject;
            currentObject.GetComponent<Pickupable>().spot = newSpot;
        } else {
            Destroy(currentObject);
        }
        isHoldingObject = false;
        currentObject = null;
        Debug.Log("Dropped Object");
    }

}
