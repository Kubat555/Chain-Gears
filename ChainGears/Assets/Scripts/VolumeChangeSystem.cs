using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeChangeSystem : MonoBehaviour
{
    [SerializeField] private Slider sliderEffects;

    [SerializeField] private AudioSource audioEffects;
    [SerializeField] private AudioSource audioEngineEffects;

    [SerializeField] private Button buttonApply;

    private float sliderEffectsPrevValue;

    private void Awake()
    {

        sliderEffectsPrevValue = PlayerPrefs.GetFloat("EffectsVolume");

        audioEffects.volume = sliderEffectsPrevValue;

        sliderEffects.value = sliderEffectsPrevValue;

        buttonApply.interactable = false;
    }

    public void SaveChanges()
    {
        if (sliderEffectsPrevValue != sliderEffects.value)
        {
            PlayerPrefs.SetFloat("EffectsVolume", sliderEffects.value);

            sliderEffectsPrevValue = sliderEffects.value;

            buttonApply.interactable = false;
        }

    }

    public void ResetChanges()
    {
        sliderEffects.value = sliderEffectsPrevValue;

        buttonApply.interactable = false;
    }
}
