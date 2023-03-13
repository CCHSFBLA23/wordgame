[System.Serializable]
public class OptionsData
{
    public float masterVolume;
    public float effectsVolume;
    public float musicVolume;

    public OptionsData(OptionsHandler options)
    {
        masterVolume = options.masterSlider.value;
        effectsVolume = options.effectsSlider.value;
        musicVolume = options.musicSlider.value;
    }
}
