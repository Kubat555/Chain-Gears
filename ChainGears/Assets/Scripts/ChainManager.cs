using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ChainManager : MonoBehaviour
{

    [SerializeField] private GameObject _chainPrefab; 
    [SerializeField] public Transform _chainParent; 
    public Transform _currentParent; 
    private GameObject _chain;
    private GameObject _lastChain; 
    private GameObject _firstChain; 
    private Camera _mainCamera;
    private Vector3 _firstTouchPos;
    private Vector3 _currentTouchPos;
    private List<GameObject> _chainsList = new List<GameObject>();

    public static int twistedCount;
    
    public static List<Transform> chainParentList = new List<Transform>();
    public static bool isCollision;

    
    private void Start()
    {
        _chainsList.Clear();
        _currentParent = _chainParent;
        
        _mainCamera = Camera.main; 
    }

    

        Vector3 touchPosition;
    private void Update()
    {
        if (GameManager.isWin||!GameManager.inGame)
            return;

        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit) && Input.GetMouseButton(0)&&!EventSystem.current.IsPointerOverGameObject() && !isCollision)
        {
            print("TOUCH");
            touchPosition = raycastHit.point;
            touchPosition.y = 0.6f; 
            _currentTouchPos = touchPosition;
            _chainParent = _chainParent == null ? _chain.transform : _chainParent;
            _chainParent.transform.LookAt(touchPosition); 

            if (Input.GetMouseButtonDown(0) &&raycastHit.collider.tag =="Floor" && _chainsList.Count < 1)
            {
                StartDrawing();
                
            }
           else if (Vector3.Distance(_firstTouchPos, _currentTouchPos)>=.33)
            {
                Drawing(raycastHit); 
            }

        }
         
        if (_chainsList.Count >1 && _chainParent.transform.localRotation.y > 0.70 || _chainParent.transform.localRotation.y < -0.70)
        {
            RemoveParent();
        }
        
        if (Input.GetMouseButtonUp(0))
        {

            GlobalEventManager.OnEndDrawing.Invoke();
            _chainsList.Clear();
            _chainParent = _currentParent;
            chainParentList.Clear();

        }
    }

    private void StartDrawing()
    { 
        chainParentList.Add(_currentParent);
        _firstTouchPos = touchPosition;
        _chainParent.position = touchPosition;
        _chainParent.rotation = Quaternion.identity;
        _chain = Instantiate(_chainPrefab, _chainParent.position, Quaternion.identity, _chainParent);
        _firstChain = _chain;
        _chain.tag = "BeginningOfChain"; 
        _chainsList.Add(_chain);
    }

    private void Drawing(RaycastHit raycastHit)
    {
        if (raycastHit.collider.tag != "Chain")
        {
            _firstTouchPos = touchPosition;
            _lastChain = _chainsList[_chainsList.Count - 1];
            _chain = Instantiate(_chainPrefab, _lastChain.transform.position, _lastChain.transform.rotation, _chain.transform);
            _chainsList.Add(_chain);
            _chain.transform.localPosition = new Vector3(0, 0, _chain.transform.localPosition.z + 0.36f);
            var joint = _chain.GetComponent<HingeJoint>();
            joint.connectedBody = _lastChain.GetComponent<Rigidbody>();
        }
        else if (raycastHit.collider.tag == "Chain" && _chainsList.Count > 1)
        {
            Destroy(_chain);
            chainParentList.Remove(_chain.transform);
            _chainsList.RemoveAt(_chainsList.Count - 1);
            _chain = _chainsList[_chainsList.Count - 1]; ;
            _firstTouchPos = _chain.transform.position;
        }
    }

    private void RemoveParent()
    {
        if (chainParentList.Count > 1)
        {

            _chainParent.transform.localRotation = Quaternion.identity;
            chainParentList.Remove(_chainParent);
            _chainParent = chainParentList[chainParentList.Count - 1].transform;
            _chainParent.transform.localRotation = Quaternion.identity;
        }
    }
     
    public void DestroyAll()
    {
        Destroy(_firstChain);
        GameManager.isWin = false;
        isCollision = false;
        _chainsList.Clear();
        _chainParent = _currentParent;
        chainParentList.Clear();
    }
}
