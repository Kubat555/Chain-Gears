using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearScript : MonoBehaviour
{
    [SerializeField] float speed;
    public int direction;
    Rigidbody rb;
    bool isRotate = false;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        isRotate = false;
        GlobalEventManager.RotateStart.AddListener(StartRotate);
        GlobalEventManager.RotateStop.AddListener(StopRotate);
    }
    private void FixedUpdate() {
        if(isRotate){
            RotateGear();
        }
    }

    public void RotateGear(){   
        // rb.WakeUp();
        // rb.AddTorque(Vector3.up * direction * speed * Time.deltaTime);
        transform.Rotate(Vector3.up * direction * speed * Time.deltaTime);
    }

    void StartRotate(){
        Debug.Log("Start rotate");
        isRotate = true;
    }

    void StopRotate(){
        isRotate = false;
    }
}
