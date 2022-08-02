using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearScript : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] bool clockwise;
    public int direction;
    Rigidbody rb;
    bool isRotate = false;
   

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        isRotate = false;
        GlobalEventManager.OnRotateStart.AddListener(StartRotate);
        GlobalEventManager.OnRotateStop.AddListener(StopRotate);
    }
    private void FixedUpdate() {
        if(isRotate){
            RotateGear();
        }
    }

    public void RotateGear(){
        // rb.WakeUp();
        // rb.AddTorque(Vector3.up * direction * speed * Time.deltaTime);
        if(clockwise)
            transform.Rotate(Vector3.up * direction * speed * Time.deltaTime);
        else
            transform.Rotate(Vector3.down * direction * speed * Time.deltaTime);

    }

    void StartRotate(){
        Debug.Log("Start rotate");
        isRotate = true;
    }

    void StopRotate(){
        isRotate = false;
    }
}
