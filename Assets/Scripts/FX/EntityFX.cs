using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    [Header("Flash FX")]
    [SerializeField]
    private Material hitMaterial;

    [SerializeField]
    public float flashDuration = 0.2f;

    [Header("Ailment Colors")]
    [SerializeField]
    private Color[] electricityColor;

    [SerializeField]
    private float electricityColorChangeFreq = 0.3f;

    [Space]
    [SerializeField]
    private Color[] ignitedColor;

    [SerializeField]
    private float ignitedColorChangeFreq = 0.25f;

    [Space]
    [SerializeField]
    private Color[] chilledColor;

    [SerializeField]
    private float chilledColorChangeFreq = 0.5f;

    [Space]
    [SerializeField]
    private Color[] poisonedColor;

    [SerializeField]
    private float poisonedColorChangeFreq = 0.5f;

    [Space]
    [SerializeField]
    private Color[] enchantedColor;

    [SerializeField]
    private float enchantedColorChangeFreq = 0.5f;

    private GameObject myHealthBar;

    private Material ogMaterial;
    private Color ogSpriteColor;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        ogMaterial = sr.material;
        ogSpriteColor = sr.color;

        myHealthBar = GetComponentInChildren<UI_HealthBar>()?.gameObject;
    }

    public IEnumerator FlashFX()
    {
        sr.material = hitMaterial;

        yield return new WaitForSeconds(flashDuration);

        sr.material = ogMaterial;
        sr.color = ogSpriteColor;
    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.red;
        }
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
        {
            myHealthBar?.SetActive(false);
            sr.color = Color.clear;
        }
        else
        {
            myHealthBar?.SetActive(true);
            sr.color = Color.white;
        }
    }

    #region Igniton FX

    public void IgnitedFxFor(float _seconds)
    {
        InvokeRepeating("IgnitedColorFX", 0, ignitedColorChangeFreq);
        Invoke("CancelColorChange", _seconds);
    }

    private void IgnitedColorFX()
    {
        if (sr.color != ignitedColor[0])
        {
            sr.color = ignitedColor[0];
        }
        else
        {
            sr.color = ignitedColor[1];
        }
    }

    #endregion

    #region Chill FX

    public void ChilledFxFor(float _seconds)
    {
        InvokeRepeating("ChilledColorFX", 0, chilledColorChangeFreq);
        Invoke("CancelColorChange", _seconds);
    }

    private void ChilledColorFX()
    {
        if (sr.color != chilledColor[0])
        {
            sr.color = chilledColor[0];
        }
        else
        {
            sr.color = chilledColor[1];
        }
    }

    #endregion

    #region Poison FX

    public void PoisonedFxFor(float _seconds)
    {
        InvokeRepeating("PoisonedColorFX", 0, poisonedColorChangeFreq);
        Invoke("CancelColorChange", _seconds);
    }

    private void PoisonedColorFX()
    {
        if (sr.color != poisonedColor[0])
        {
            sr.color = poisonedColor[0];
        }
        else
        {
            sr.color = poisonedColor[1];
        }
    }

    #endregion

    #region Enchant FX

    public void EnchantedFxFor(float _seconds)
    {
        InvokeRepeating("EnchantedColorFX", 0, enchantedColorChangeFreq);
        Invoke("CancelColorChange", _seconds);
    }

    private void EnchantedColorFX()
    {
        if (sr.color != enchantedColor[0])
        {
            sr.color = enchantedColor[0];
        }
        else
        {
            sr.color = enchantedColor[1];
        }
    }

    #endregion

    #region Electricity FX

    private void ElectricityColorFX()
    {
        if (sr.color != electricityColor[0])
        {
            sr.color = electricityColor[0];
        }
        else
        {
            sr.color = electricityColor[1];
        }
    }

    public void ElectricityFXFor(float _sec)
    {
        InvokeRepeating("ElectricityColorFX", 0, electricityColorChangeFreq);
        Invoke("CancelColorChange", _sec);
    }

    #endregion
}