// Decompiled with JetBrains decompiler
// Type: HTTPUnlocker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using SimpleJSON;
using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class HTTPUnlocker : MonoBehaviour
{
  private UnityEngine.UI.Text status_text;
  private bool coroutine_running;

  private void Start() => this.status_text = this.transform.Find("Panel/Status").GetComponent<UnityEngine.UI.Text>();

  public void StartUnlocking()
  {
    if (!this.coroutine_running)
    {
      this.status_text.text = "Contacting server....";
      TMP_InputField component = this.transform.Find("Panel/licensekey").GetComponent<TMP_InputField>();
      if (component.text.Length < 19)
        this.status_text.text = "Full license key must be of the form XXXX-XXXX-XXXX-XXXX";
      else if (LicenseData.DoesLicenseExist(component.text))
      {
        this.status_text.text = "This license is already enabled.";
      }
      else
      {
        this.coroutine_running = true;
        this.StartCoroutine(this.RunHTTPCoroutine(component.text));
      }
    }
    else
      this.status_text.text = "Unlock request in progress, please wait until it is complete.";
  }

  private string authenticate(string username, string password)
  {
    string s = username + ":" + password;
    return "Basic " + Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(s));
  }

  private IEnumerator RunHTTPCoroutine(string license_key)
  {
    JSONNode jsonNode;
    using (UnityWebRequest www = UnityWebRequest.Get("https://xrcsimulator.org/wp-json/lmfwc/v2/licenses/activate/" + license_key))
    {
      www.SetRequestHeader("AUTHORIZATION", this.authenticate("ck_9baaa4b34b6149c8b576fd7ddbdd5f971f6a21b8", "cs_ca7dbf4b6b9c215f9276f1f746dbe568a9e71e59"));
      yield return (object) www.SendWebRequest();
      jsonNode = JSON.Parse(www.downloadHandler.text);
      if (www.isNetworkError || www.isHttpError)
      {
        this.status_text.text = !jsonNode.HasKey("message") ? "Error! Unable to connect to master server!" : (string) jsonNode["message"];
        this.coroutine_running = false;
        yield break;
      }
    }
    if (!jsonNode.HasKey("data") || !jsonNode["data"].HasKey("attributes"))
    {
      this.status_text.text = "Something went wrong: did not receive required data back. Contact support@xrcsimulator.org if this persists.";
      this.coroutine_running = false;
    }
    else
    {
      LicenseData licenseData = new LicenseData();
      if (!licenseData.DecodeLicense(jsonNode["data"]))
      {
        this.status_text.text = "Something went wrong: unable to determine license features. Contact support@xrcsimulator.org if this persists.";
        this.coroutine_running = false;
      }
      else
      {
        GLOBALS.myLicenses.Add(licenseData);
        this.status_text.text = "License activated: Thank you for supporting XRC Simulator!";
        if (licenseData.DaysLeft() < 9999)
        {
          UnityEngine.UI.Text statusText = this.status_text;
          statusText.text = statusText.text + "\r\nDays Left = " + (object) licenseData.DaysLeft();
        }
        this.status_text.text += "\r\nFeatures enabled: \r\n";
        this.status_text.text += licenseData.GetFeatureString();
        Settings.SavePrefs();
        this.coroutine_running = false;
      }
    }
  }
}
