﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using LCHelper;
using UnityEngine.UI;

public class DownloadController : MonoBehaviour
{
	
    private const string serverUrl = "https://raw.githubusercontent.com/fafase/unity-utilities/master/Scripts/lcdata.json";
    public event EventHandler<DownloadCompleteArgs> RaiseDownloadComplete;
    protected void OnDownloadComplete(DownloadCompleteArgs arg)
    {
        if (RaiseDownloadComplete != null)
        {
            RaiseDownloadComplete(this, arg);
        }
    }

	public void Init ()
    {
        StartCoroutine(FetchDataCoroutine());
	}

    private IEnumerator FetchDataCoroutine()
    {
        WWW www = new WWW(serverUrl);
        yield return www;
        if (string.IsNullOrEmpty(www.error) == false)
        {
            OnDownloadComplete(new DownloadCompleteArgs(null));
            yield break;
        }
        if (string.IsNullOrEmpty(www.text) == false)
        {
            System.IO.File.WriteAllText(ConstString.DataPath, www.text);
            PlayerPrefs.SetString(ConstString.JsonData, www.text);
			Debug.Log (www.text);
        }
		yield return new WaitForSeconds(2f);
        OnDownloadComplete(new DownloadCompleteArgs(www.text));
        www.Dispose();
    }


}

public class DownloadCompleteArgs : EventArgs
{
    public readonly string json = null;
    public DownloadCompleteArgs( string json)
    {
        this.json = json;
    }
}
