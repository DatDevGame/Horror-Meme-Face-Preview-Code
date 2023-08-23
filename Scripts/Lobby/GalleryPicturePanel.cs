using _GAME.Scripts.Inventory;
using Assets._SDK.Analytics;
using Assets._SDK.Game;
using Assets._SDK.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Assets._SDK.Ads;
using UnityEngine.Android;

public class GalleryPicturePanel : MonoBehaviour
{
    private const int MAX_SIZE = 512;
    private const float SPRITE_WIDTH_SIZE = 4f;
	private const float SPRITE_HEIGHT_SIZE = 8f;

	public Button NextBotJugglerButton;
    public Button TakePictureButton;
    public Button CloseButton;

    public Sprite UploadPhoto;
    public Sprite TakePhoto;

    public Image PermissionDeniedImg;

    [SerializeField]
    private Sprite spriteDefault;

    private bool _isPhotoFromPlayer = false;
    private void Start()
    {
        NextBotJugglerButton.onClick.AddListener(JustPlay);
        CloseButton.onClick.AddListener(ClosePanel);
    }

    private void OnEnable()
    {
        HandleGalleryPictureStart();
    }
    private void OnDisable()
    {
        TakePictureButton.onClick.RemoveAllListeners();
    }

    private void PermissionCameraRequest()
    {
        NativeCamera.Permission result = NativeCamera.RequestPermission();

		if (result == NativeCamera.Permission.Denied || result == NativeCamera.Permission.ShouldAsk)
            PermissionDenied();
        else
            PermissionGranted();
    }
    private void PermissionGalleryRequest()
    {
        NativeGallery.Permission result = NativeGallery.RequestPermission(NativeGallery.PermissionType.Read);

        if (result == NativeGallery.Permission.Denied || result == NativeGallery.Permission.ShouldAsk)
            PermissionDenied();
        else
            PermissionGranted();
    }
    private void PermissionDenied()
    {
        TakePictureButton.interactable = false;
        PermissionDeniedImg.gameObject.SetActive(true);
    }
    private void PermissionGranted()
    {
        TakePictureButton.interactable = true;
        PermissionDeniedImg.gameObject.SetActive(false);
    }


    private void ClosePanel()
    {
        this.gameObject.SetActive(false);
    }

    public void HandleGalleryPictureStart()
    {
		TakePictureButton.onClick.RemoveAllListeners();
		PermissionGranted();
        Mission selectedMission = (Mission)GameManager.Instance.MissionInventory.PlayingMission;
        PhotoTypeMission photoTypeMission = selectedMission.PhotoTypeMission;
        _isPhotoFromPlayer = true;
        switch (photoTypeMission)
        {
            case PhotoTypeMission.None:
                Debug.Log("None");
                break;

            case PhotoTypeMission.TakePhoto:
                TakePictureButton.GetComponent<Image>().sprite = TakePhoto;
                TakePictureButton.onClick.AddListener(TakePicture);

				break;

            case PhotoTypeMission.UploadPhoto:
                TakePictureButton.GetComponent<Image>().sprite = UploadPhoto;
                TakePictureButton.onClick.AddListener(GalleryPictures);
                break;
        }
    }
    private void GalleryPictures()
	{
		PermissionGalleryRequest();
		AdsManager.Instance.ShowRewarded(_ =>
        {
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
            {
                Debug.Log("Image path: " + path);
                if (path != null)
                {
                    // Create Texture from selected image
                    Texture2D texture = NativeGallery.LoadImageAtPath(path, MAX_SIZE);
                    if (texture == null)
                    {
                        Debug.Log("Couldn't load texture from " + path);
                        return;
                    }
                    AddPictureForEnemy(texture);
                }
            });
        }, GameManager.Instance.MissionInventory.PlayingMission.Order, AnalyticParamKey.SPECIAL_MISSION);
    }
    private void TakePicture()
	{
        PermissionCameraRequest();
  //      bool hasPermissionCamera = Permission.HasUserAuthorizedPermission(Permission.Camera);

		//if (hasPermissionCamera)
		//{
		//	Permission.RequestUserPermission(Permission.Camera);
		//	hasPermissionCamera = Permission.HasUserAuthorizedPermission(Permission.Camera);
		//}

		NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
            {
                Debug.Log("Image path: " + path);
                if (path != null)
                {
                    // Create a Texture2D from the captured image
                    Texture2D texture = NativeCamera.LoadImageAtPath(path, MAX_SIZE);
                    if (texture == null)
                    {
                        Debug.Log("Couldn't load texture from " + path);
                        return;
                    }

                    AddPictureForEnemy(texture);
                }
            }, MAX_SIZE);

        if (permission != NativeCamera.Permission.Granted)
                JustPlay();
    }
    private void JustPlay()
    {
        _isPhotoFromPlayer = false;
        SetSprite(spriteDefault);
        AbstractGameManager.Instance.Fire(GameTrigger.Play);
    }
    private void AddPictureForEnemy(Texture2D texture)
    {
        Rect rec = new Rect(0, 0, texture.width, texture.height);
        var spriteGallery = Sprite.Create(texture, rec, Vector2.zero, 100);
		SetSprite(spriteGallery);
        AbstractGameManager.Instance.Fire(GameTrigger.Play);
    }
    private void SetSprite(Sprite sprite)
    {
        var dataEnemy = GameManager.Instance.Resources.PictureEnemySetting;
        dataEnemy.skin.Image = sprite;
        GameObject obj = dataEnemy.skin.Model;
        var renderers = obj.GetComponentsInChildren<SpriteRenderer>();


		foreach (var renderer in renderers)
        {
			renderer.sprite = sprite;
			renderer.size = new Vector2(SPRITE_WIDTH_SIZE, SPRITE_HEIGHT_SIZE);
		}


        if(_isPhotoFromPlayer)
            SetPosition(renderers, new Vector3(2.5f, -2f, 0));
        else
            SetPosition(renderers, new Vector3(0, 0.3f, 0));
    }

    private static void SetPosition(SpriteRenderer[] renderers, Vector3 position)
    {
        renderers[0].transform.localPosition = position;
        if(renderers.Length > 1)
            renderers[1].transform.localPosition = new Vector3(-position.x, position.y, position.z);
    }
}
