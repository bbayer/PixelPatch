using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Piece : MonoBehaviour
{
    public enum Mode
    {
        Drag,
        PlaceHolder,
        Idle
    }

    public Mode mode = Mode.Drag;
    // Start is called before the first frame update
    public bool isMatched=false;
    GameObject placeHolder;
    void Start()
    {
        GameEventManager.OnMessage += OnMessage;
        ActivatePreview(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActivatePreview(bool act) {
        foreach (Transform t  in transform.GetChild(1)) {
            t.gameObject.SetActive(act);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (mode == Mode.Drag )
        {
            if (other.tag == "Placeholder")
            {
                if (gameObject.name == other.name)
                {
                    isMatched = true;
                    Debug.Log("Piece match");
                    placeHolder = other.gameObject;
                }
            }
        }

        if(mode == Mode.PlaceHolder && other.tag == "Piece" && gameObject.name == other.name)
        {
            ActivatePreview(true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (mode == Mode.Drag)
        {
            if (other.tag == "Placeholder" && gameObject.name == other.name)
            {
                isMatched = false;
                Debug.Log("Piece left");

            }
        }

        if (mode == Mode.PlaceHolder && other.tag == "Piece" && gameObject.name == other.name)
        {
            ActivatePreview(false);
        }
    }

    IEnumerator PieceMatchAnimation()
    {
        float delay = .05f;
        float tweenDuration = 1f;
        List<Transform> transforms = new List<Transform>();
        Transform root = transform.GetChild(0);
        for (int i = 0; i < root.childCount; i++)
        {
            Transform t = root.GetChild(i);
            transforms.Add(t);
            string name = t.name;
            string[] coords = name.Split(new char[] { ',' });
            LeanTween.move(t.gameObject, new Vector3(Convert.ToInt32(coords[0]), 0, 15 - Convert.ToInt32(coords[1])), tweenDuration).setEaseOutElastic();
            //t.position = new Vector3(Convert.ToInt32(coords[0]), 0, 15 - Convert.ToInt32(coords[1]));
        }
        mode = Mode.Idle;
        //yield return new WaitForSeconds(transform.childCount*delay+tweenDuration);
        yield return null;
        foreach (var t in transforms)
        {
            t.parent = gameObject.transform.parent;
        }
        yield return null;

        GameEventManager.OnMessage("piece_matched", gameObject);
        Destroy(gameObject);
        Destroy(placeHolder);

    }

    void OnMessage(string msg, object obj)
    {

        if (msg == "drag_end")
        {

            if (mode == Mode.Drag && isMatched) {
                StartCoroutine(PieceMatchAnimation());
            }
        }
    }
}