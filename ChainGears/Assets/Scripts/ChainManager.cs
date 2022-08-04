using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ChainManager : MonoBehaviour
{
    [SerializeField] private AudioClip setOnePartOfChainAudioClip;

    [SerializeField] private GameObject _chainPrefab; 
    [SerializeField] public Transform _chainParent; 
    public Transform _currentParent; 
    private GameObject _chain;
    private GameObject _lastChain; 
    private GameObject _firstChain; 
    private Camera _mainCamera;
    private Vector3 _firstTouchPos;
    private Vector3 _currentTouchPos;

    [HideInInspector]
    public List<GameObject> chainsList = new List<GameObject>();

    public static List<GameObject> twistedChainList = new List<GameObject>();
    
    public static List<Transform> chainParentList = new List<Transform>();
    public static bool isCollision;
    public static bool gameOver; // Переменная становится правдой если игрок коснулся gameover коллайдера
    public static int gameOverCollideCount; // Переменная становится больше при каждом касании gameover коллайдера
    public static ChainManager Instance;

    public List<GameObject> gearList = new List<GameObject>();

    private int timerForAudio = 5;

    private void Awake() {
        if(Instance == null){
            Instance = this;
        }
    }
    private void Start()
    {
        chainsList.Clear();
        _currentParent = _chainParent;
        
        _mainCamera = Camera.main;
        gearList.Clear();
        gameOver = false;
        gameOverCollideCount = 0;
    }

        Vector3 touchPosition;
    private void Update()
    {
        // print(GameManager.inGame);
        if (GameManager.isWin||!GameManager.inGame  )
            return;

        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit) && Input.GetMouseButton(0) && !isCollision)
        { 
            touchPosition = raycastHit.point;
            touchPosition.y = 0.6f; 
            _currentTouchPos = touchPosition;
            _chainParent = _chainParent == null ? _chain.transform : _chainParent;
            _chainParent.transform.LookAt(touchPosition); 

            if (Input.GetMouseButtonDown(0) &&raycastHit.collider.tag =="StartZone" && chainsList.Count < 1 /*&& !EventSystem.current.IsPointerOverGameObject()*/)
            {
                StartDrawing();
                
            }
           else if (Vector3.Distance(_firstTouchPos, _currentTouchPos)>=.13 && chainsList.Count > 0)
            {
                Drawing(raycastHit); 
                GameManager.Instance.rotateButton.interactable = true;
            }

        }
         
        if (chainsList.Count >1 && _chainParent.transform.localRotation.y > 0.70 || _chainParent.transform.localRotation.y < -0.70)
        {
            RemoveParent();
        }
        
        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject()&& chainsList.Count > 0)
        {

            GlobalEventManager.OnEndDrawing.Invoke();
            chainsList.Clear();
            _chainParent = _currentParent;
            chainParentList.Clear();

        }
        if (Input.GetKey (KeyCode.A))
        {
            print("GEAr COUNT " + gearList.Count) ;
        }
    }

    private void StartDrawing()
    { 
        chainParentList.Add(_currentParent);
        _firstTouchPos = touchPosition;
        _chainParent.position = touchPosition;
        _chainParent.rotation = Quaternion.identity;
        _chain = Instantiate(_chainPrefab, _chainParent.position, Quaternion.identity, _chainParent);
        SphereCollider sphereCollider = _chain.AddComponent<SphereCollider>();
        sphereCollider.radius = 0.2f;
        sphereCollider.isTrigger = true;
        AudioPlayer.instance.PlaySound(setOnePartOfChainAudioClip);
        _firstChain = _chain;
        _chain.tag = "BeginningOfChain"; 
        chainsList.Add(_chain);
        
    }

    private void Drawing(RaycastHit raycastHit)
    {
        if (raycastHit.collider.tag != "Chain")
        {
            _firstTouchPos = touchPosition;
            _lastChain = chainsList[chainsList.Count - 1];
            _chain = Instantiate(_chainPrefab, _lastChain.transform.position, _lastChain.transform.rotation, _chain.transform);
            if (timerForAudio == 0)
            {
                AudioPlayer.instance.PlaySound(setOnePartOfChainAudioClip);
                timerForAudio = 5;
            }
            else
            {
                timerForAudio--;
            }
            chainsList.Add(_chain);
            _chain.transform.localPosition = new Vector3(0, 0, _chain.transform.localPosition.z + 0.15f);
            var joint = _chain.GetComponent<HingeJoint>();
            joint.connectedBody = _lastChain.GetComponent<Rigidbody>();
        }
        else if (raycastHit.collider.tag == "Chain" && chainsList.Count > 1)
        {
            if(_chain.GetComponent<Chain_test>().loseCollide){
                ChainManager.gameOverCollideCount = Mathf.Clamp(ChainManager.gameOverCollideCount - 1, 0, 100);
            }
            Destroy(_chain);
            chainParentList.Remove(_chain.transform);
            if(chainParentList.Count>1)
                _chainParent = chainParentList[chainParentList.Count-1];
            else
                _chainParent = _currentParent;
            chainsList.RemoveAt(chainsList.Count - 1);
            _chain = chainsList[chainsList.Count - 1]; ;
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
        chainsList.Clear();
        _chainParent = _currentParent;
        chainParentList.Clear();
        gearList.Clear();
    }
}
