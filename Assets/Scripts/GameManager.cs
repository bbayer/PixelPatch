using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // Use this for initialization

    bool startDrag=false;
    Vector3 draggedStartPosition;
    GameObject draggedObject;
    Vector3 touchOffset;
    int wrongMoveCount = 0;
    Plane gameAreaPlane;
    public ParticleSystem levelCompletedParticules;
    Animator animator;
    public GameObject levelLoadingPanel;
    void Awake()
    {
        animator = Camera.main.GetComponent<Animator>();
        
        gameAreaPlane = new Plane(Vector3.up, -3);
        GameEventManager.OnMessage += OnMessageHandler;
        levelCompletedParticules.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Piece")
                {
                    draggedObject = hit.collider.gameObject;
                    draggedStartPosition = draggedObject.transform.position;
                    touchOffset =  hit.point - draggedStartPosition;
                    startDrag = true;
                    GameEventManager.OnMessage("drag_start", draggedObject);

                }
            }
        }

        if (Input.GetMouseButtonUp(0) && startDrag) {
            startDrag = false;
            GameEventManager.OnMessage("drag_end", draggedObject);
            if (draggedObject != null && !draggedObject.GetComponent<Piece>().isMatched) {
                LeanTween.move(draggedObject, draggedStartPosition, 1).setEaseOutElastic();
               //draggedObject.transform.position = draggedStartPosition;
                wrongMoveCount++;

            }
        }
        if (startDrag) {
            Vector3 screenPt = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            Vector3 offset = new Vector3(touchOffset.x, 0, touchOffset.z);
            if (gameAreaPlane.Raycast(ray, out distance)) {
                draggedObject.transform.position = ray.GetPoint(distance)-offset;
            }
            
        }
    }

    void OnMessageHandler(string msg, object obj) {

        if (msg == "level_loading_started") {
            Camera.main.transform.localRotation = Quaternion.Euler(145, 0, 0);
            animator.SetBool("animate", false);

        }
        else if (msg == "level_completed")
        {
            GameEventManager.OnMessage("wrong_move_count", wrongMoveCount);
            levelCompletedParticules.gameObject.SetActive(true);
            levelCompletedParticules.Play();
            animator.SetBool("animate", true);

        }
        else if (msg == "level_loaded")
        {
            wrongMoveCount = 0;
            Camera.main.transform.localRotation = Quaternion.Euler(75, 0, 0);
            levelCompletedParticules.gameObject.SetActive(false);

        }

        if (msg == "level_data_loaded") {
            levelLoadingPanel.SetActive(false);
        }
    }
}
