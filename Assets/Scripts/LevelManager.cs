using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    struct PieceFormation
    {
        public PieceFormation(byte name, float rotation, Vector2 position) {
            Name = name;
            Rotation = rotation;
            Position = position;
            float xoffset = 0;
            if (name == 0) xoffset = -1.5f;
            else if (name == 1) xoffset = -3f;
            else if (name == 2) xoffset = -3f;
            else if (name == 3) xoffset = -3f;
            else if (name == 4) xoffset = -3f;
            else if (name == 5) xoffset = -4.5f;

            Offset = new Vector3(xoffset, 0, 0);
        }

        public byte Name { get; }
        public float Rotation { get; }
        public Vector2 Position { get; }
        public Vector3 Offset { get; }

    }

    List<List<PieceFormation>> formations;
    public GameObject cubePreFab;
    public GameObject wallPrefab;
    List<GameObject> cubes;

    public int currentLevel = 0;
    public int currentPieceCount;
    public GameObject[] piecePrefabs;
    public bool debugMode;

    public ParticleSystem explosionParticles;
    // Start is called before the first frame update
    void Start()
    {
        if (debugMode)
        {
            InitializeFormations();
            cubes = new List<GameObject>();

            StartCoroutine(DebugMode());
        }
        else {
            GameEventManager.OnMessage += OnMessage;
            cubes = new List<GameObject>();
            currentLevel = PlayerPrefs.GetInt("currentLevel", 0);
            InitializeFormations();
            StartCoroutine(InitializeLevelData());
        }

    }

    IEnumerator DebugMode() {
        int count = 0;
        foreach (var fmlist in formations)
        {
            foreach (var formation in fmlist)
            {
                GameObject pieceObj = Instantiate(piecePrefabs[formation.Name], new Vector3(formation.Position.x, 0, formation.Position.y), Quaternion.identity, transform) as GameObject;
                pieceObj.transform.parent = transform;
                yield return new WaitForEndOfFrame();
                Piece p = pieceObj.GetComponent<Piece>();
                p.ActivatePreview(true);
            }
            Debug.Log(count++);
            yield return new WaitForSeconds(10f);
            ClearGameArea();

        }
    }
    IEnumerator InitializeLevelData() {
        yield return new WaitForEndOfFrame();
        GameEventManager.OnMessage("level_data_loaded",null);
        LoadLevel();
    }
    void InitializeFormations() {
        formations = new List<List<PieceFormation>>();
        //0
        formations.Add(new List<PieceFormation>() {
            new PieceFormation(2,0,new Vector3(5,5))
        });
        //1
        formations.Add(new List<PieceFormation>() {
            new PieceFormation(1,0,new Vector3(5,7))
        });
        //2
        formations.Add(new List<PieceFormation>() {
            new PieceFormation(2,0,new Vector3(5,9)),
            new PieceFormation(1,0,new Vector3(5,5)),
            new PieceFormation(1,0,new Vector3(5,1)),
            new PieceFormation(0,0,new Vector3(1,10)),
            new PieceFormation(0,0,new Vector3(1,6)),
            new PieceFormation(0,0,new Vector3(1,2)),
            new PieceFormation(0,0,new Vector3(12,2)),
            new PieceFormation(0,0,new Vector3(12,6)),
            new PieceFormation(0,0,new Vector3(12,10)),
        });
        //3
        formations.Add(new List<PieceFormation>() {
            new PieceFormation(2,0,new Vector3(5,9)),
            new PieceFormation(2,0,new Vector3(5,2)),
            new PieceFormation(0,0,new Vector3(1,10)),
            new PieceFormation(0,0,new Vector3(1,6)),
            new PieceFormation(0,0,new Vector3(1,2)),
            new PieceFormation(0,0,new Vector3(12,2)),
            new PieceFormation(0,0,new Vector3(12,6)),
            new PieceFormation(0,0,new Vector3(12,10)),
        });
        //4
        formations.Add(new List<PieceFormation>() {
            new PieceFormation(5,0,new Vector3(4,6)),
            new PieceFormation(0,0,new Vector3(10,2)),
            new PieceFormation(0,0,new Vector3(4,2)),
            new PieceFormation(0,0,new Vector3(2,10)),
            new PieceFormation(0,0,new Vector3(12,10)),
        });

        //5
        formations.Add(new List<PieceFormation>() {
            new PieceFormation(1,0,new Vector3(5,0)),
            new PieceFormation(1,0,new Vector3(5,4)),
            new PieceFormation(3,0,new Vector3(5,9)),
            new PieceFormation(0,0,new Vector3(1,6)),
            new PieceFormation(0,0,new Vector3(2,10)),
            new PieceFormation(0,0,new Vector3(11,6)),
            new PieceFormation(0,0,new Vector3(12,10)),
        });
        //6
        formations.Add(new List<PieceFormation>() {
            new PieceFormation(4,0,new Vector3(5,6)),
            new PieceFormation(1,0,new Vector3(5,2)),
            new PieceFormation(0,0,new Vector3(2,4)),
            new PieceFormation(0,0,new Vector3(3,9)),
            new PieceFormation(0,0,new Vector3(11,4)),
            new PieceFormation(0,0,new Vector3(11,9)),
        });
        //7
        formations.Add(new List<PieceFormation>() {
            new PieceFormation(5,0,new Vector3(1,5)),
            new PieceFormation(1,0,new Vector3(1,2)),
            new PieceFormation(3,0,new Vector3(8,2)),
            new PieceFormation(0,0,new Vector3(8,10)),
            new PieceFormation(0,0,new Vector3(12,10)),
        });
        //8
        formations.Add(new List<PieceFormation>() {
            new PieceFormation(0,0,new Vector3(2,1)),
            new PieceFormation(0,0,new Vector3(5,1)),
            new PieceFormation(1,0,new Vector3(8,1)),
            new PieceFormation(1,0,new Vector3(2,4)),
            new PieceFormation(4,0,new Vector3(8,4)),
            new PieceFormation(5,0,new Vector3(2,7)),
            new PieceFormation(0,0,new Vector3(2,10)),
            new PieceFormation(0,0,new Vector3(8,10)),
            new PieceFormation(1,0,new Vector3(2,13)),
            new PieceFormation(0,0,new Vector3(8,13)),
            new PieceFormation(0,0,new Vector3(11,13)),
        });

        //9
        formations.Add(new List<PieceFormation>() {
            new PieceFormation(0,0,new Vector3(2,1)),
            new PieceFormation(0,0,new Vector3(5,1)),
            new PieceFormation(1,0,new Vector3(8,1)),

            new PieceFormation(5,0,new Vector3(0,4)),
            new PieceFormation(4,0,new Vector3(9,4)),
            new PieceFormation(3,0,new Vector3(6,7)),

            new PieceFormation(1,0,new Vector3(3,10)),

            new PieceFormation(0,0,new Vector3(2,13)),
            new PieceFormation(1,0,new Vector3(5,13)),
            new PieceFormation(0,0,new Vector3(11,13)),
        });

        //10
        formations.Add(new List<PieceFormation>() {
            new PieceFormation(0,0,new Vector3(2,1)),
            new PieceFormation(1,0,new Vector3(5,1)),
            new PieceFormation(0,0,new Vector3(11,1)),

            new PieceFormation(5,0,new Vector3(0,4)),
            new PieceFormation(1,0,new Vector3(9,4)),

            new PieceFormation(3,0,new Vector3(0,10)),

            new PieceFormation(2,0,new Vector3(6,7)),

            new PieceFormation(1,0,new Vector3(6,13)),

            new PieceFormation(0,0,new Vector3(12,7)),
            new PieceFormation(0,0,new Vector3(12,10)),
            new PieceFormation(0,0,new Vector3(12,13)),

        });

    }
    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator PlayFinishAnimation() {
        foreach (GameObject t in cubes) {
            LeanTween.moveY(t.gameObject, 5f, 1f).setEaseOutCirc();
        }

        yield return new WaitForSeconds(1f);

        foreach (GameObject t in cubes)
        {
            LeanTween.moveY(t.gameObject, 3f, .02f);
        }
        explosionParticles.gameObject.SetActive(true);
        explosionParticles.Play();

        GameEventManager.OnMessage("level_completed", currentLevel);
    }
    void ClearGameArea() {
        cubes.Clear();
        foreach (Transform ch in transform)
        {
            Destroy(ch.gameObject);
        }
    }

    public void NextLevel() {
        currentLevel++;
        PlayerPrefs.SetInt("currentLevel", currentLevel);
        LoadLevel();
    }

    void LoadLevel()
    {
        explosionParticles.gameObject.SetActive(false);
        StartCoroutine(LoadPixelData());
    }

    IEnumerator LoadPixelData() {
        int levelSize = 16;
        int maxLevels = 146;
        GameEventManager.OnMessage("level_loading_started", currentLevel);
        yield return new WaitForEndOfFrame();
        ClearGameArea();
        yield return new WaitForEndOfFrame();

        TextAsset asset = Resources.Load("Levels/" + (currentLevel%maxLevels)) as TextAsset;
        Debug.Log(asset);
        List<Color32> lvl = new List<Color32>();
        for (int i = 0; i < asset.bytes.Length; i += 4) {
            lvl.Add(new Color32(
                asset.bytes[i],
                asset.bytes[i+1],
                asset.bytes[i+2],
                asset.bytes[i+3]
                ));
        }
        for (int i = 0; i < levelSize; i++)
        {
            for (int j = 0; j < levelSize; j++)
            {
                Color32 cl = lvl[j * 16 + i];
                GameObject g;
                if (cl.a == 0)
                {
                    g = Instantiate(wallPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                    g.transform.parent = transform;
                    g.transform.localPosition = new Vector3(j, 0, 15 - i);
                    continue;

                }
                g = Instantiate(cubePreFab, Vector3.zero, Quaternion.identity) as GameObject;
                g.transform.parent = transform;
                g.transform.localPosition = new Vector3(j, 0, 15 - i);
                g.GetComponent<Renderer>().material.SetColor("_BaseColor", cl);
                g.name = String.Format("{0},{1}", j, i);
                cubes.Add(g);
            }
        }
        Debug.Log(cubes.Count + " cubes inside list");
        yield return new WaitForEndOfFrame();
        StartCoroutine(LoadPieces());
    }

    List<PieceFormation> SelectFormation(int level=0) {

        List<PieceFormation> retVal = new List<PieceFormation>();

        List<PieceFormation> selected = formations[0];
        int selectedNdx=0;
        if (level == 0)
            return formations[0];
        else if (level == 1)
            return formations[1];
        else if (level>=2 && level < 5)
        {
            selectedNdx = UnityEngine.Random.Range(2, 5);

        }
        else if (level >= 5 && level < 10)
        {
            selectedNdx = UnityEngine.Random.Range(3, 6);
        }
        else if (level >= 10 && level < 15)
        {
            selectedNdx = UnityEngine.Random.Range(4, 7);
        }
        else if (level >= 15)
        {
            selectedNdx = UnityEngine.Random.Range(2, formations.Count);
        }
        selected = formations[selectedNdx];

        System.Random random = new System.Random();
        IEnumerable<int> lst = Enumerable.Range(0, selected.Count).OrderBy(x => random.Next()).Take(selected.Count);
        foreach (var ndx in lst)
        {
            retVal.Add(selected[ndx]);
        }

        return retVal;
    }

    void CapturePieces(GameObject piece) {
        Debug.Log("Capturing piece");

        BoxCollider[] colliders = piece.GetComponents<BoxCollider>();
        foreach(GameObject cube in cubes)
        {
            if (cube == null)
                continue;
            BoxCollider bx = cube.GetComponent<BoxCollider>();
            
            foreach (BoxCollider pieceCollider in colliders) {
                if (bx.bounds.Intersects(pieceCollider.bounds)) {
                    cube.transform.parent = piece.transform.GetChild(0);
                    break;
                }
            }
        }
        Debug.Log("Capturing piece end.");

    }

    int CalculateCaptureCount(GameObject piece) {
        BoxCollider[] colliders = piece.GetComponents<BoxCollider>();
        int count = 0;
        foreach (GameObject cube in cubes) {
            BoxCollider bx = cube.GetComponent<BoxCollider>();

            foreach (BoxCollider pieceCollider in colliders)
            {
                if (bx.bounds.Intersects(pieceCollider.bounds))
                {
                    count++;
                }
            }
        }
        return count;
    }
    void RevertPiece(GameObject piece) {
        Debug.Log("Reverting piece.");

        foreach (Transform t in piece.transform.GetChild(0)) {
            t.parent = transform;
        }
    }
    //IEnumerator LoadPieces() {
    //    //Place
    //    List<PieceFormation> formationList = SelectFormation();
    //    //TODO: rotate
    //    int count = 0;

    //    foreach (PieceFormation formation in formationList)
    //    {
    //        //Create drag
    //        GameObject pieceObj = Instantiate(piecePrefabs[formation.Name], new Vector3(formation.Position.x, 0, formation.Position.y), Quaternion.identity, transform) as GameObject;
    //        Piece piece = pieceObj.GetComponent<Piece>();
    //        yield return new WaitForEndOfFrame();
    //        piece.mode = Piece.Mode.Drag;
    //        piece.gameObject.transform.position = new Vector3(0 + count * 5, 3, -10);
    //        piece.name = "piece" + count;

    //        //Create placeholder
    //        pieceObj = Instantiate(piecePrefabs[formation.Name], new Vector3(formation.Position.x, 0, formation.Position.y), Quaternion.identity, transform) as GameObject;
    //        piece = pieceObj.GetComponent<Piece>();
    //        //yield return new WaitForEndOfFrame();
    //        piece.mode = Piece.Mode.PlaceHolder;
    //        piece.tag = "Placeholder";
    //        piece.name = "piece" + count;

    //        count++;

    //    }
    //    currentPieceCount = count;
    //}
    IEnumerator LoadPieces()
    {
        //Place
        List<PieceFormation> formationList = SelectFormation(currentLevel);

        int count = 0;

        int numberOfPieces = 1;
        int selectedPieces = 0;
        if (currentLevel < 2) {
            numberOfPieces = 1;
        }
        else if (currentLevel < 10 && currentLevel >= 2)
            numberOfPieces = 2;
        else
            numberOfPieces = 3;

        Vector3[] piecePos = new Vector3[] {
            new Vector3(7.5f,3,-12),

            new Vector3(2.5f,3,-12),
            new Vector3(11.5f,3,-12),

            new Vector3(2f,3,-15),
            new Vector3(7.5f,3,-8.7f),
            new Vector3(14f,3,-15),
        };

        foreach (PieceFormation formation in formationList)
        {
            Debug.Log("Selecting piece: "+count);
            //Create drag
            GameObject pieceObj = Instantiate(piecePrefabs[formation.Name], new Vector3(formation.Position.x, 0, formation.Position.y), Quaternion.identity, transform) as GameObject;
            yield return new WaitForEndOfFrame();

            if (CalculateCaptureCount(pieceObj) > 8)
            {
                CapturePieces(pieceObj);
            }
            else {
                Destroy(pieceObj);
                continue;
            }

            int ndx = numberOfPieces * (numberOfPieces - 1) / 2;
            print(ndx);
            pieceObj.transform.position = piecePos[ndx + count] + formation.Offset ;
            pieceObj.name = "piece" + count;

            //Create placeholder
            pieceObj = Instantiate(piecePrefabs[formation.Name], new Vector3(formation.Position.x, 0, formation.Position.y), Quaternion.identity, transform) as GameObject;
            Piece piece = pieceObj.GetComponent<Piece>();
            piece.mode = Piece.Mode.PlaceHolder;
            piece.tag = "Placeholder";
            piece.name = "piece" + count;

            count++;
            if (numberOfPieces == count)
                break;

        }
        if (numberOfPieces != count)
        {

            Debug.LogError("Pieces are not correctly selected.");
        }
        currentPieceCount = count;

        GameEventManager.OnMessage("level_loaded", currentLevel);

    }

    void OnMessage(string msg, object obj) {

        if (msg == "piece_matched") {
            Debug.Log("Piece matched");
            currentPieceCount--;
            if (currentPieceCount == 0) {
                StartCoroutine(PlayFinishAnimation());
            }
        }
    }
}
