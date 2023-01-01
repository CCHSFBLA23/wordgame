using UnityEngine;
using UnityEngine.UI;

public class OptionsHandler : MonoBehaviour
{
    [Header("Volume Sliders")]
    public Slider masterSlider;
    public Slider effectsSlider;
    public Slider musicSlider;

    // Updates all volume
    public void volumeUpdate(Slider slider)
    {
        // If masterSlider changed update AudioListener volume immediately
        if (slider == masterSlider)
            AudioListener.volume = slider.value;

        // If musicSlider changed update all music AudioSource volumes
        if (slider == musicSlider)
        {
            for (int i = 0; i < AudioManager.musicSources.Count; i++)
                AudioManager.musicSources[i].volume = slider.value;
        }

        SaveSystem.SaveOptionsData(this);
    }

    private void Awake()
    {
        // Set options to last set value
        OptionsData values = SaveSystem.LoadOptionsData();
        masterSlider.value = values.masterVolume;
        effectsSlider.value = values.effectsVolume;
        musicSlider.value = values.musicVolume;
    }
}
