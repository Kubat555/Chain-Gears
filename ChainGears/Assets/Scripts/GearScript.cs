using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearScript : MonoBehaviour
{
    [SerializeField] GameObject gear;
    [SerializeField] float speed;
    public int direction;
    Rigidbody rb;
    public static bool isRotate = false;
   

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

        gear.transform.Rotate(Vector3.up * direction * speed * Time.deltaTime);
    }

    void StartRotate(){
        Debug.Log("Start rotate");
        isRotate = true;
    }

    void StopRotate(){
        isRotate = false;
    }

  
}
