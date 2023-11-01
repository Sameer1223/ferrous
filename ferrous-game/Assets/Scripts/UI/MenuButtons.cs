using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class MenuButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
{
    [Header("Selected Button Visuals")]
    [SerializeField] private GameObject _selectOverlay;
    [SerializeField] private Color _selectedColour, _unselectedColour;
    [SerializeField] private TextMeshProUGUI _btnText;

    [Header("Sounds")]
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _selectClip, _clickClip;


    public void OnPointerDown(PointerEventData eventData)
    {
        _selectOverlay.SetActive(false);
        _btnText.color = _unselectedColour;

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _selectOverlay.SetActive(true);
        _btnText.color = _selectedColour;
        _source.PlayOneShot(_clickClip);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _selectOverlay.SetActive(true);
        _btnText.color = _selectedColour;
        _source.PlayOneShot(_selectClip);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _selectOverlay.SetActive(false);
        _btnText.color = _unselectedColour;
    }

    // controller / keyboard

    public void OnSelect(BaseEventData eventData)
    {
        _selectOverlay.SetActive(true);
        _btnText.color = _selectedColour;
        _source.PlayOneShot(_selectClip);

    }

    public void OnDeselect(BaseEventData eventData)
    {
        _selectOverlay.SetActive(false);
        _btnText.color = _unselectedColour;
    }

    public void ButtonClicked()
    {
        _selectOverlay.SetActive(false);
        _btnText.color = _unselectedColour;
        _source.PlayOneShot(_clickClip);

    }
}
