using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Array
{
    public List<GameObject> cells = new List<GameObject>();
    public GameObject this[int index] => cells[index];
    public int Length()
    {
        return cells.Count;
    }
}


public class BackgroundController : MonoBehaviour
{
    [SerializeField]
    GameObject cameraObject; // camera object
    [SerializeField]
    Array[] layers;
    [SerializeField]
    GameObject[] rotatables; // sprites that span multiple layers must be rotated because daisy stinks
    [SerializeField]
    float[] layerOffsets; // offset coefficients to be multiplied by the offset
    // layerOffsets.length = layers.length
    [SerializeField]
    float[] rotatableOffsetsPos; // positional coefficient
    [SerializeField]
    float[] rotatableOffsetsRot; // rotational coefficient

    public float centerX = 0;
    public float offset;
    List<List<float>> initialOffsets; // initial offsets of bg elements from center
    List<float> initialOffsetsRotatablesPos;
    List<float> initialOffsetsRotatablesRot;

    // Start is called before the first frame update
    void Start()
    {
        initialOffsets = new List<List<float>>();
        Debug.Log(layers.Length);
        for (int i = 0; i < layers.Length; ++i)
        {
            List<float> offsetRow = new List<float>();
            for (int j = 0; j < layers[i].Length(); ++j)
            {
                offsetRow.Add(layers[i][j].transform.position.x);
                Debug.Log(layers[i][j].transform.position.x);
            }
            initialOffsets.Add(offsetRow);
        }
        initialOffsetsRotatablesPos = new List<float>();
        initialOffsetsRotatablesRot = new List<float>();
        for (int i = 0; i < rotatables.Length; ++i)
        {
            initialOffsetsRotatablesPos.Add(rotatables[i].transform.position.x);
            initialOffsetsRotatablesRot.Add(rotatables[i].transform.eulerAngles.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        offset = cameraObject.transform.position.x - centerX;
        for (int i = 0; i < layers.Length; ++i)
        {
            for (int j = 0; j < layers[i].Length(); ++j)
            {
                Vector3 pos = layers[i][j].transform.position;
                pos.x = offset * layerOffsets[i] + initialOffsets[i][j];
                layers[i][j].transform.position = pos;
            }
        }
        for (int i = 0; i < rotatables.Length; ++i)
        {
            Vector3 pos = rotatables[i].transform.position;
            pos.x = offset * rotatableOffsetsPos[i] + initialOffsetsRotatablesPos[i];
            rotatables[i].transform.position = pos;
            //rotatables[i].transform.rotation
            Vector3 rotation = rotatables[i].transform.eulerAngles;
            rotation.y = offset * rotatableOffsetsRot[i] + initialOffsetsRotatablesRot[i];
            rotatables[i].transform.eulerAngles = rotation;
        }
    }
}
