using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    /*
    The purpose of the BackgroundManager is to have a scene object that can display any background that is chosen in the
    map select screen. The BackgroundManager instantiates the chosen stage background and assigns the necessary instance
    variables to the background object.
     */
    [SerializeField]
    GameObject[] bgPrefabs;
    [SerializeField]
    GameObject cameraObject;
    [SerializeField]
    Vector3 bgPosition = new Vector3(0, 0, 0);

    public int bgToDisplay = 0;
    GameObject bgObj;

    // Start is called before the first frame update
    void Start()
    {
        bgObj = Instantiate(bgPrefabs[bgToDisplay], bgPosition, Quaternion.identity);
        BackgroundController bgController = bgObj.GetComponent<BackgroundController>();
        bgController.cameraObject = cameraObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
