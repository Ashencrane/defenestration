using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StageSelector;

public class BackgroundManager : MonoBehaviour
{
    /*
    The purpose of the BackgroundManager is to have a scene object that can display any background that is chosen in the
    map select screen. The BackgroundManager instantiates the chosen stage background and assigns the necessary instance
    variables to the background object.

    Modified to also set the correct music
     */
    [SerializeField]
    GameObject[] bgPrefabs;
    [SerializeField]
    float[] stageWidths;
    [SerializeField]
    GameObject cameraObject;
    [SerializeField]
    Vector3 bgPosition = new Vector3(0, 0, 0);
    [SerializeField]
    GameObject leftWall;
    [SerializeField]
    GameObject rightWall;
    [SerializeField]
    AudioManager audioMan;

    private readonly Dictionary<StageMap, int> stageIndices = new Dictionary<StageMap, int>()
    {
        { StageMap.Cathedral, 0 },
        { StageMap.Ballroom, 1 }
    };

    private readonly AudioManager.MUSIC[] musics = new AudioManager.MUSIC[] {AudioManager.MUSIC.Cathedral, AudioManager.MUSIC.Ballroom };


    public int bgToDisplay = 0;
    GameObject bgObj;

    // Start is called before the first frame update
    void Start()
    {
        bgToDisplay = stageIndices[StageSelector.SelectedStageMap];
        bgObj = Instantiate(bgPrefabs[bgToDisplay], bgPosition, Quaternion.identity);
        audioMan.PlayMusic(musics[bgToDisplay]);
        
        BackgroundController bgController = bgObj.GetComponent<BackgroundController>();
        bgController.cameraObject = cameraObject;
        leftWall.transform.position = new Vector3(-stageWidths[bgToDisplay], 0, 0);
        rightWall.transform.position = new Vector3(stageWidths[bgToDisplay], 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
