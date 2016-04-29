using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using System.IO;

public sealed class AppController : MonoBehaviour
{
    private static AppController instance = null;
    public static AppController Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        RegisterDownload();
    }

    private void RegisterDownload()
    {
        DownloadController dlCtrl = FindObjectOfType<DownloadController>();
        if (dlCtrl != null)
        {
            dlCtrl.RaiseDownloadComplete += HandleDownloadComplete;
            dlCtrl.Init();
        }
    }

    private void HandleDownloadComplete(object sender, DownloadCompleteArgs arg)
    {
        DownloadController dlCtrl = FindObjectOfType<DownloadController>();
        if (dlCtrl != null)
        {
            dlCtrl.RaiseDownloadComplete -= HandleDownloadComplete;
        }
        SceneManager.LoadScene("FrontScene");
    }
}
