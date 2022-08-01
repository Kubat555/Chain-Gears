using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChainManager : MonoBehaviour
{

    [SerializeField] private GameObject _chainPrefab; 
    [SerializeField] public Transform _chainParent; 
    private GameObject _chain;
    private GameObject _lastChain;
    private GameObject _firstChain;
    private GameObject _startChain;
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
        chainParentList.Add( _chainParent);
        _mainCamera = Camera.main;
        GlobalEventManager.OnChainInGain.AddListener(ChangeParentChain);
    }

    

    private void Update()
    {
        if (GameManager.isWin||!GameManager.inGame)
            return;
        Vector3 touchPosition;
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit) && Input.GetMouseButton(0))
        {
            touchPosition = raycastHit.point;
            touchPosition.y = 0.6f;


            _currentTouchPos = touchPosition;
            if (Input.GetMouseButtonDown(0)&& raycastHit.collider.tag != "Chain"&& _chainsList.Count<1)
            {
                _firstTouchPos = touchPosition;
                _chainParent.position = touchPosition;
                _chain = Instantiate(_chainPrefab, _chainParent.position, Quaternion.identity, _chainParent);
                _chain.tag = "BeginningOfChain";
                _startChain = _chain;
                _firstChain = _chain;
                _chainsList.Add(_chain);
               // _chain.GetComponent<Rigidbody>().isKinematic = true;
            }
           else if (Vector3.Distance(_firstTouchPos, _currentTouchPos)>=.33)
            {
                if (raycastHit.collider.tag != "Chain"  && !GameManager.isTwisted)
                {
                    _firstTouchPos = touchPosition;
                    _lastChain = _chainsList[_chainsList.Count - 1];
                    _chain = Instantiate(_chainPrefab, new Vector3(_lastChain.transform.position.x, _lastChain.transform.position.y, _lastChain.transform.position.z), _lastChain.transform.rotation, _chain.transform);
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
            
            else if(  Vector3.Distance(_firstTouchPos, _currentTouchPos) >= .22)
            {
               
            }
            /*else if (Vector3.Distance(_firstTouchPos, _currentTouchPos) <= 0)
            {
             //   Destroy(_chain);
            }*/
            _chainParent = _chainParent == null ? _chain.transform : _chainParent;
            _chainParent.transform.LookAt(touchPosition);
            /*else
            {
                _chain.transform.localPosition = touchPosition;
            }*/
        }
        if (Input.GetKey(KeyCode.D))
        {
            print("WIN "+ GameManager.isWin);
            print("TWISTED "+ GameManager.isTwisted);
            print("Collisison  "+ isCollision);
            print("Twisted count   "+ twistedCount);
           /* for (int i = 0; i < _chainsList.Count-1; i++)
            {
                
                _chainsList[i].transform.DOLocalMove(_chainsList[i+1].transform.localPosition, .51f);
            }*/
        }
        if (_chainsList.Count > 1&&_chainParent.transform.localRotation.y>0.70|| _chainParent.transform.localRotation.y < -0.70)
        {
             
            if (chainParentList.Count>1)
            {

                _chainParent.transform.localRotation = Quaternion.identity;
                chainParentList.Remove(_chainParent);
                _chainParent = chainParentList[chainParentList.Count-1].transform;
                _chainParent.transform.localRotation = Quaternion.identity;
            }
            //_firstChain = _firstChain == null ? _lastChain : _firstChain; 
            //  _firstChain.transform.parent = null;
            //   _firstChain = _chain;


            //_chain.transform.parent = _chainParent;
            // _chain.transform.localPosition = Vector3.zero;
            // _chain.transform.localRotation = Quaternion.Euler(Vector3.zero);
            // _chainPrefab.GetComponent<HingeJoint>().axis = new Vector3(0, 1, 0);


        }

        /*private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radiusForSpawn);
        }*/
        if (Input.GetMouseButtonUp(0))
        {

            GlobalEventManager.OnEndDrawing.Invoke();
        }
    }

    private void ChangeParentChain()
    {
        
    }
}
