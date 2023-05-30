using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public struct OptionSettings
{
    public static float MusicSetting = 0.75f;
    public static float SfxSetting = 0.75f;
    public static Resolution ScreenResolution;
    public static int ScreenRefreshRate = 60;
    public static bool Fullscreen = true;
}

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private AudioMixer sfxMixer;

    private Resolution[] _resolutions;
    private List<Resolution> _filteredResolutions;
    private int _currentResolutionIndex;

    // Start is called before the first frame update
    void Start()
    {
        SetupDropdown();
        SetupToggle();
        SetupSliders();
    }

    private void SetupToggle()
    {
        fullscreenToggle.isOn = OptionSettings.Fullscreen;
    }

    private void SetupSliders()
    {
  
        musicSlider.value = OptionSettings.MusicSetting;
        volumeSlider.value = OptionSettings.SfxSetting;
    }

    private void SetupDropdown()
    {
        // get all resolutions, clear default dropdown options, and save current refresh rate
        _resolutions = Screen.resolutions;
        _filteredResolutions = new List<Resolution>();
        resolutionDropdown.ClearOptions();
        OptionSettings.ScreenRefreshRate = Screen.currentResolution.refreshRate;

        // filter resolutions by those that match current refresh rate
        for (int i = 0; i < _resolutions.Length; ++i)
        {
            if (_resolutions[i].refreshRate == OptionSettings.ScreenRefreshRate) 
                _filteredResolutions.Add(_resolutions[i]);
        }

        // create dropdown options based on filter resolutions and save current resolution settings
        List<string> resolutionOptions = new List<string>();
        for (int i = 0; i < _filteredResolutions.Count; ++i)
        {
            Resolution res = _filteredResolutions[i];
            resolutionOptions.Add($"{res.width}x{res.height} {res.refreshRate} Hz");
            if (res.Equals(Screen.currentResolution))
            {
                _currentResolutionIndex = i;
                OptionSettings.ScreenResolution = res;
                OptionSettings.Fullscreen = Screen.fullScreen;
            }
        }
        
        // populate dropdown and set default option to current resolution
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = _currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int index)
    {
        Resolution res = _filteredResolutions[index];
        OptionSettings.ScreenResolution = res;
        Screen.SetResolution(res.width, res.height, OptionSettings.Fullscreen);
    }

    public void SetFullscreen(bool val)
    {
        OptionSettings.Fullscreen = val;
        Screen.fullScreen = val;
    }

    //takes in a float from 0.001-1 and converts it to db
    private float convertToDb(float val)
    {
        return ((Mathf.Log10(val * 20) - 0.5f) * 10f);
    }
    
    public void UpdateMusicSetting(float val)
    {
        OptionSettings.MusicSetting = val;
        musicMixer.SetFloat("Music_Vol", convertToDb(val));
        Debug.Log("New music volume: " + OptionSettings.MusicSetting);
    }
    
    public void UpdateSfxSetting(float val)
    {
        OptionSettings.SfxSetting = val;
        sfxMixer.SetFloat("SFX_Vol", convertToDb(val));
        
        Debug.Log("New sfx volume: " + OptionSettings.SfxSetting);
    }
}
