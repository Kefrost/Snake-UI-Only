using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Assets : MonoBehaviour
{

    private void Awake()
    {
        StartCoroutine(DownloadTailTexture());
        StartCoroutine(DownloadCurvedTailTexture());
    }

    IEnumerator DownloadTailTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://drive.google.com/uc?export=download&id=1K6p8_O4earj28172Gfpveb3lZPyd7cuh");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D tail = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite tailSprite = Sprite.Create(tail, new Rect(0, 0, tail.width, tail.height), new Vector2(0, 0), 100f);
            GameManager.Instance.straightTail = tailSprite;
        }
    }

    IEnumerator DownloadCurvedTailTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://drive.google.com/uc?export=download&id=1rciXzLBQ_Hef9HLvqP7U39kP_UduC8Og");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D tail = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite tailSprite = Sprite.Create(tail, new Rect(0, 0, tail.width, tail.height), new Vector2(0, 0), 100f);
            GameManager.Instance.curvedTail = tailSprite;
        }
    }
}
