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
            AudioManager audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
            for (int i = 0; i < audioManager.musicSources.Count; i++)
                audioManager.musicSources[i].volume = slider.value;
        }

        SaveSystem.SaveOptionsData(this);
    }

    private void Awake()
    {
        // Set options to last set value
        OptionsData values = SaveSystem.LoadOptionsData();
        masterSlider.value = values?.masterVolume ?? 0.6f;
        effectsSlider.value = values?.effectsVolume ?? 0.6f;
        musicSlider.value = values?.musicVolume ?? 0.6f;
    }
}
