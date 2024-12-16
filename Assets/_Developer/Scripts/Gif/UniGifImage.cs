/*
UniGif
Copyright (c) 2015 WestHillApps (Hironari Nishioka)
This software is released under the MIT License.
http://opensource.org/licenses/mit-license.php
*/

using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Security.AccessControl;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// Texture Animation from GIF image
/// </summary>
public class UniGifImage : MonoBehaviour
{
    public enum State
    {
        None,
        Loading,
        Ready,
        Playing,
        Pause,
    }

    // Target row image
    [SerializeField]
    private RawImage m_rawImage;
    // Image Aspect Controller
    //[SerializeField]
    //private UniGifImageAspectController m_imgAspectCtrl;
    // Textures filter mode
    [SerializeField]
    private FilterMode m_filterMode = FilterMode.Point;
    // Textures wrap mode
    [SerializeField]
    private TextureWrapMode m_wrapMode = TextureWrapMode.Clamp;
    // Load from url on start
    [SerializeField]
    private bool m_loadOnStart;
    // GIF image url (WEB or StreamingAssets path)
    [SerializeField]
    private string m_loadOnStartUrl;
    // Rotating on loading
    [SerializeField]
    private bool m_rotateOnLoading;
    // Debug log flag
    [SerializeField]
    private bool m_outputDebugLog;

    public int myEmojiNumber = 0;

    public static UniGifImage Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Decoded GIF texture list
    private List<UniGif.GifTexture> m_gifTextureList;
    // Delay time
    private float m_delayTime;
    // Texture index
    private int m_gifTextureIndex;
    // loop counter
    private int m_nowLoopCount;

    /// <summary>
    /// Now state
    /// </summary>
    public State nowState
    {
        get;
        private set;
    }

    /// <summary>
    /// Animation loop count (0 is infinite)
    /// </summary>
    public int loopCount
    {
        get;
        private set;
    }

    /// <summary>
    /// Texture width (px)
    /// </summary>
    public int width
    {
        get;
        private set;
    }

    /// <summary>
    /// Texture height (px)
    /// </summary>
    public int height
    {
        get;
        private set;
    }

    private void Start()
    {
        //if (m_rawImage == null)
        //{
        //    m_rawImage = GetComponent<RawImage>();
        //}
        //if (m_loadOnStart)
        //{            
        //    SetGifFromUrl(m_loadOnStartUrl);
        //}

        // Example of loading multiple URLs on start

        //string[] urlsToLoad = new string[]
        //{
        //    "http://example.com/gif1.gif",
        //    "http://example.com/gif2.gif",
        //    "http://example.com/gif3.gif"
        //};
        //LoadGifTextures(urlsToLoad);

    }

    public void LoadGifTextures(string[] urls, bool autoPlay = true)
    {
        foreach (string url in urls)
        {
            StartCoroutine(SetGifFromUrlCoroutine(url, autoPlay));
        }

    }

    private void OnDestroy()
    {
        Clear();
    }
    private void Update()
    {
        switch (nowState)
        {
            case State.None:
            case State.Ready:
            case State.Pause:
                break;

            case State.Loading:
                if (m_rotateOnLoading)
                {
                    transform.Rotate(0f, 0f, 30f * Time.deltaTime, Space.Self);
                }
                break;

            case State.Playing:
                if (m_rawImage == null || m_gifTextureList == null || m_gifTextureList.Count <= 0)
                {
                    return;
                }
                if (m_delayTime > Time.time)
                {
                    return;
                }
                // Change texture
                m_gifTextureIndex = (m_gifTextureIndex + 1) % m_gifTextureList.Count;
                m_rawImage.texture = m_gifTextureList[m_gifTextureIndex].m_texture2d;
                m_delayTime = Time.time + m_gifTextureList[m_gifTextureIndex].m_delaySec;

                if (loopCount > 0 && m_gifTextureIndex == 0)
                {
                    m_nowLoopCount++;
                    if (m_nowLoopCount >= loopCount)
                    {
                        Stop();
                    }
                }
                break;
        }
    }

    /// <summary>
    /// Set GIF texture from url
    /// </summary>
    /// <param name="url">GIF image url (WEB or StreamingAssets path)</param>
    /// <param name="autoPlay">Auto play after decode</param>
    public void SetGifFromUrl(string url, bool autoPlay = true)
    {
        StartCoroutine(LoadSprite(url, autoPlay));
    }

    /// <summary>
    /// Set GIF texture from url
    /// </summary>
    /// <param name="url">GIF image url (WEB or StreamingAssets path)</param>
    /// <param name="autoPlay">Auto play after decode</param>
    /// <returns>IEnumerator</returns>
    /// 
 /*   public IEnumerator SetGifFromUrlCoroutine(string url, bool autoPlay = true)
    {
        if (string.IsNullOrEmpty(url))
        {
            Debug.LogError("URL is nothing.");
            yield break;
        }

        if (nowState == State.Loading)
        {
            Debug.LogWarning("Already loading.");
            yield break;
        }
        nowState = State.Loading;

        string path;
        if (url.StartsWith("http"))
        {
            // from WEB
            path = url;
        }

        // Load file        
        // using (WWW www = new WWW(Application.persistentDataPath + "/ZipFile/Emoji/Gif/1.gif"))
        using (WWW www = new WWW(url))
        {
            yield return www;
            if (string.IsNullOrEmpty(www.error) == false)
            {
                Debug.LogError("File load error.\n" + www.error);
                nowState = State.None;
                yield break;
            }

            Clear();
            nowState = State.Loading;
            StartCoroutine(UniGif.GetTextureListCoroutine(www.bytes, (gifTexList, loopCount, width, height) =>
            {
                if (gifTexList != null)
                {
                    m_gifTextureList = gifTexList;
                    this.loopCount = loopCount;
                    this.width = width;
                    this.height = height;
                    nowState = State.Ready;

                    //m_imgAspectCtrl.FixAspectRatio(width, height);

                    if (m_rotateOnLoading)
                    {
                        transform.localEulerAngles = Vector3.zero;
                    }

                    if (autoPlay)
                    {
                        Play();
                    }
                    else
                    {
                        m_rawImage.texture = m_gifTextureList[0].m_texture2d;
                    }
                }
                else
                {
                    Debug.LogError("Gif texture get error.");
                    nowState = State.None;
                }
            },
    m_filterMode, m_wrapMode, m_outputDebugLog));
            // Get GIF textures           
        }
    }*/

    public IEnumerator SetGifFromUrlCoroutine(string url, bool autoPlay = true)
    {
        // Check if URL is valid
        if (string.IsNullOrEmpty(url))
        {
            Debug.LogError("URL is empty.");
            yield break;
        }

        // Check if already loading
        if (nowState == State.Loading)
        {
            Debug.LogWarning("Already loading.");
            yield break;
        }

        // Set loading state
        nowState = State.Loading;

        // Use UnityWebRequest for better performance and functionality
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // Send request asynchronously
            yield return request.SendWebRequest();

            // Check for errors
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to load GIF file: " + request.error);
                nowState = State.None;
                yield break;
            }

            // Clear previous textures and prepare for new ones
            Clear();
            nowState = State.Loading;

            // Process loaded bytes into GIF textures
            yield return StartCoroutine(UniGif.GetTextureListCoroutine(request.downloadHandler.data,
                (gifTexList, loopCount, width, height) =>
                {
                    if (gifTexList != null)
                    {
                    // Assign loaded textures and metadata
                    m_gifTextureList = gifTexList;
                        this.loopCount = loopCount;
                        this.width = width;
                        this.height = height;
                        nowState = State.Ready;

                    // Reset rotation if necessary
                    if (m_rotateOnLoading)
                        {
                            transform.localEulerAngles = Vector3.zero;
                        }

                    // Auto play or display first frame
                    if (autoPlay)
                        {
                            Play();
                        }
                        else
                        {
                            m_rawImage.texture = m_gifTextureList[0].m_texture2d;
                        }
                    }
                    else
                    {
                        Debug.LogError("Failed to get GIF textures.");
                        nowState = State.None;
                    }
                },
                m_filterMode, m_wrapMode, m_outputDebugLog));
        }
    }


    public IEnumerator LoadSprite(string path, bool autoPlay = true)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("URL is nothing.");
            yield break;
        }
        if (nowState == State.Loading)
        {
            Debug.LogWarning("Already loading.");
            yield break;
        }

        Clear();
        nowState = State.Loading;

        byte[] bytes = File.ReadAllBytes(path);

        yield return StartCoroutine(UniGif.GetTextureListCoroutine(bytes, (gifTexList, loopCount, width, height) =>
        {
            if (gifTexList != null)
            {
                m_gifTextureList = gifTexList;
                this.loopCount = loopCount;
                this.width = width;
                this.height = height;
                nowState = State.Ready;

                //m_imgAspectCtrl.FixAspectRatio(width, height);

                //if (m_rotateOnLoading)
                //{
                //    transform.localEulerAngles = Vector3.zero;
                //}

                if (autoPlay)
                {
                    Play();
                }
            }
            else
            {
                Debug.LogError("Gif texture get error.");
                nowState = State.None;
            }
        },
            m_filterMode, m_wrapMode, m_outputDebugLog));
    }

    /// <summary>
    /// Clear GIF texture
    /// </summary>
    public void Clear()
    {
        if (m_rawImage != null)
        {
            m_rawImage.texture = null;
        }

        if (m_gifTextureList != null)
        {
            for (int i = 0; i < m_gifTextureList.Count; i++)
            {
                if (m_gifTextureList[i] != null)
                {
                    if (m_gifTextureList[i].m_texture2d != null)
                    {
                        Destroy(m_gifTextureList[i].m_texture2d);
                        m_gifTextureList[i].m_texture2d = null;
                    }
                    m_gifTextureList[i] = null;
                }
            }
            m_gifTextureList.Clear();
            m_gifTextureList = null;
        }

        nowState = State.None;
    }

    /// <summary>
    /// Play GIF animation
    /// </summary>
    public void Play()
    {
        if (nowState != State.Ready)
        {
            Debug.LogWarning("State is not READY.");
            return;
        }
        if (m_rawImage == null || m_gifTextureList == null || m_gifTextureList.Count <= 0)
        {
            Debug.LogError("Raw Image or GIF Texture is nothing.");
            return;
        }
        nowState = State.Playing;
        m_rawImage.texture = m_gifTextureList[0].m_texture2d;
        m_delayTime = Time.time + m_gifTextureList[0].m_delaySec;
        m_gifTextureIndex = 0;
        m_nowLoopCount = 0;
    }

    /// <summary>
    /// Stop GIF animation
    /// </summary>
    public void Stop()
    {
        if (nowState != State.Playing && nowState != State.Pause)
        {
            Debug.LogWarning("State is not Playing and Pause.");
            return;
        }
        nowState = State.Ready;
    }

    /// <summary>
    /// Pause GIF animation
    /// </summary>
    public void Pause()
    {
        if (nowState != State.Playing)
        {
            Debug.LogWarning("State is not Playing.");
            return;
        }
        nowState = State.Pause;
    }

    /// <summary>
    /// Resume GIF animation
    /// </summary>
    public void Resume()
    {
        if (nowState != State.Pause)
        {
            Debug.LogWarning("State is not Pause.");
            return;
        }
        nowState = State.Playing;
    }
}