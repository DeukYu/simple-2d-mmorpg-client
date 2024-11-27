using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ShaderByTime : MonoBehaviour
{
    private PostProcessVolume _postProcessVolume;

    public PostProcessProfile StandardProfile;
    public PostProcessProfile AutumnProfile;
    public PostProcessProfile WinterProfile;

    public PostProcessProfile NightProfile;

    private void Awake()
    {
        _postProcessVolume = GetComponent<PostProcessVolume>();
    }

    private void Start()
    {
        GetPresentTime();
    }

    private void GetPresentTime()
    {
        if (_postProcessVolume == null)
            return;

        var time= DateTime.Now;
        if(IsNight(time))
        {
            _postProcessVolume.profile = NightProfile;
        }
        else
        {
            if(time.Month >= 3 && time.Month <= 8)
            {
                _postProcessVolume.profile = StandardProfile;
            }
            else if(time.Month >= 9 && time.Month <= 11)
            {
                _postProcessVolume.profile = AutumnProfile;
            }
            else
            {
                _postProcessVolume.profile = WinterProfile;
            }
        }
    }

    private bool IsNight(DateTime dateTime)
    {
        if(dateTime.Hour > 18)
        {
            return true;
        }

        return false;
    }
}
