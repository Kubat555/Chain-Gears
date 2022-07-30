using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainController : MonoBehaviour
{

    // [SerializeField] GameObject chain;
    // [SerializeField] bool isHead = false;
    // static ChainController activeChain;


    public float radiusForSpawn;

    [SerializeField] private GameObject _chainPrefab;
    [SerializeField] private Transform _chainParent;
    private GameObject _chain;
    private GameObject _lastChain;
    private Camera _mainCamera;

    [HideInInspector]
    public Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _mainCamera = Camera.main;
    }

    

    private void Update()
    {
        
        Vector3 touchPosition;
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit) && Input.GetMouseButton(0))
        {
            touchPosition = raycastHit.point;
            touchPosition.y = 0.6f;
            if (Input.GetMouseButtonDown(0)&& raycastHit.collider.tag != "Chain")
            {
                _chain = Instantiate(_chainPrefab,touchPosition,Quaternion.identity, _chainParent);
                _chain.GetComponent<Rigidbody>().isKinematic = true;
            } 
            
           else if (raycastHit.collider.tag != "Chain")
            {
                _lastChain = _chain;
                _chain = Instantiate(_chainPrefab, touchPosition, _lastChain.transform.rotation, _chainParent);
                var joint = _chain.GetComponent<HingeJoint>();
                joint.connectedBody = _lastChain.GetComponent<Rigidbody>();

            } 
             
            _chain.transform.LookAt(touchPosition);
            /*else
            {
                _chain.transform.localPosition = touchPosition;
            }*/
        }


        /*private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radiusForSpawn);
        }*/
    }
}
