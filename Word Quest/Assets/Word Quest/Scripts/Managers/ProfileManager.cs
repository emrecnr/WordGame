using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private GameObject _profilePhotosPanel;
    [SerializeField] private TMP_Text _nickname;
    [SerializeField] private Image profileImage;
    [SerializeField] private TMP_Text coin;

    [Header(" Profile Photos ")]
    [SerializeField]List<Image> profilePhotos;


    
    [Header(" Settings ")]
    private Sprite _currentProfilePhoto;
    private int _profilePhotoIndex;

    private void Start()
    {
        _profilePhotosPanel.gameObject.SetActive(false);
        LoadProfile();
    }

    public void EditProfile()
    {
        VibrationManager.Vibrate();

        _profilePhotosPanel.gameObject.SetActive(true);
    }

    public void SelectPhoto(int index)
    {
        int newIndex = index - 1;
        for (int i = 0; i < profilePhotos.Count; i++)
        {
            _profilePhotoIndex = newIndex;
            if (i == newIndex) _currentProfilePhoto = profilePhotos[newIndex].sprite ;
            else
            {
                continue;
            }
        }
    }

    public void SavePhoto()
    {
        VibrationManager.Vibrate();

        profileImage.sprite = _currentProfilePhoto;
        PlayerPrefs.SetInt("ProfilePhoto", _profilePhotoIndex);
        _profilePhotosPanel.gameObject.SetActive(false);
    }

    private void LoadProfile()
    {
        // Profile Photo Load
        _profilePhotoIndex = PlayerPrefs.GetInt("ProfilePhoto");
        _currentProfilePhoto = profilePhotos[_profilePhotoIndex].sprite;
        profileImage.sprite = _currentProfilePhoto;


        // Coin Load
        //coin.text = DataManager.Instance.Coin.ToString();

    }
}
