// Decompiled with JetBrains decompiler
// Type: LicenseData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class LicenseData
{
  public int license_id;
  public int product_id;
  public string licenseKey = "";
  public DateTime expiresDate = new DateTime(1L);
  public Dictionary<string, List<string>> features = new Dictionary<string, List<string>>();
  private string fromstring = "";

  public bool DecodeLicense(JSONNode data)
  {
    try
    {
      if (!data.HasKey("id"))
        return false;
      this.license_id = int.Parse((string) data["id"]);
      if (!data.HasKey("productId"))
        return false;
      this.license_id = int.Parse((string) data["productId"]);
      if (!data.HasKey("licenseKey"))
        return false;
      this.licenseKey = (string) data["licenseKey"];
      if (!data.HasKey("expiresAt"))
        return false;
      if (data["expiresAt"] == (object) null)
      {
        this.expiresDate = new DateTime(0L);
      }
      else
      {
        CultureInfo provider = new CultureInfo("en-US");
        this.expiresDate = DateTime.ParseExact((string) data["expiresAt"] + " -04:00", "yyyy-MM-dd HH:mm:ss zzz", (IFormatProvider) provider, DateTimeStyles.None);
      }
      if (!data.HasKey("attributes"))
        return false;
      JSONNode jsonNode1 = data["attributes"];
      JSONNode.KeyEnumerator enumerator = jsonNode1.Keys.GetEnumerator();
      while (enumerator.MoveNext())
      {
        string current = enumerator.Current;
        JSONNode jsonNode2 = jsonNode1[current];
        List<string> stringList = new List<string>();
        if (jsonNode2.IsArray)
        {
          for (int aIndex = 0; aIndex < jsonNode2.AsArray.Count; ++aIndex)
            stringList.Add((string) jsonNode2[aIndex]);
        }
        else
          stringList.Add((string) jsonNode2);
        this.features[current] = stringList;
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ("Failed to read in internet license data: " + (object) ex));
      return false;
    }
    this.fromstring = this.ToString();
    return true;
  }

  public bool IsFeatureValid(string key, string value)
  {
    this.FromString(this.fromstring);
    return (this.features.ContainsKey("ALL") || this.features.ContainsKey(key) && this.features[key].Contains(value)) && (this.expiresDate.Ticks == 0L || (this.expiresDate - DateTime.Now).TotalSeconds > 0.0);
  }

  public int DaysLeft()
  {
    if (this.expiresDate.Ticks == 0L)
      return 9999;
    TimeSpan timeSpan = this.expiresDate - DateTime.Now;
    return timeSpan.Days < 0 ? -1 : timeSpan.Days;
  }

  public string GetFeatureString()
  {
    string featureString = "";
    foreach (string key in this.features.Keys)
    {
      foreach (string str in this.features[key])
        featureString = featureString + key + "=>" + str + "   ";
    }
    return featureString;
  }

  public static bool DoesLicenseExist(string checkkey)
  {
    foreach (LicenseData license in GLOBALS.myLicenses)
    {
      if (license.licenseKey == checkkey)
        return true;
    }
    return false;
  }

  public new string ToString()
  {
    string data = "" + (object) this.license_id + "," + (object) this.product_id + "," + this.licenseKey + "," + (object) this.expiresDate.Ticks + ",";
    foreach (string key in this.features.Keys)
    {
      foreach (string str in this.features[key])
        data = data + key + "," + str + ",";
    }
    return LicenseData.FixString(data + MyUtils.EncryptRSA(data), 1);
  }

  public bool FromString(string indata)
  {
    this.fromstring = indata;
    this.license_id = 0;
    this.product_id = 0;
    this.expiresDate = new DateTime(1L);
    this.features.Clear();
    try
    {
      string[] strArray1 = LicenseData.FixString(indata, -1).Split(',');
      string signature = strArray1[strArray1.Length - 1];
      if (!MyUtils.RSAVerify(string.Join(",", strArray1, 0, strArray1.Length - 1) + ",", signature))
      {
        Debug.LogError((object) "Corrupted license key when reading from memory. Failed sanity check.");
        return false;
      }
      int num1 = 0;
      string[] strArray2 = strArray1;
      int index1 = num1;
      int num2 = index1 + 1;
      this.license_id = int.Parse(strArray2[index1]);
      string[] strArray3 = strArray1;
      int index2 = num2;
      int num3 = index2 + 1;
      this.product_id = int.Parse(strArray3[index2]);
      string[] strArray4 = strArray1;
      int index3 = num3;
      int num4 = index3 + 1;
      this.licenseKey = strArray4[index3];
      string[] strArray5 = strArray1;
      int index4 = num4;
      int num5 = index4 + 1;
      this.expiresDate = new DateTime(long.Parse(strArray5[index4]));
      this.features.Clear();
      while (num5 < strArray1.Length - 2)
      {
        string[] strArray6 = strArray1;
        int index5 = num5;
        int num6 = index5 + 1;
        string key = strArray6[index5];
        string[] strArray7 = strArray1;
        int index6 = num6;
        num5 = index6 + 1;
        string str = strArray7[index6];
        if (!this.features.ContainsKey(key))
          this.features[key] = new List<string>();
        this.features[key].Add(str);
      }
      return true;
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("Failed reading in license key from memory. Error:" + (object) ex));
      return false;
    }
  }

  public static bool AddLicense(string md5data)
  {
    LicenseData licenseData = new LicenseData();
    if (!licenseData.FromString(md5data))
      return false;
    GLOBALS.myLicenses.Add(licenseData);
    return true;
  }

  public static bool CheckRobotIsUnlocked(string robot, string skin)
  {
    if (skin.Length < 1 || skin.ToUpper() == "DEFAULT")
      return true;
    foreach (LicenseData license in GLOBALS.myLicenses)
    {
      if (license.IsFeatureValid(robot, skin))
        return true;
    }
    return false;
  }

  public static int GetFeatureDaysLeft(string robot, string skin)
  {
    if (skin.Length < 1 || skin.ToUpper() == "DEFAULT")
      return 9999;
    int featureDaysLeft = -1;
    foreach (LicenseData license in GLOBALS.myLicenses)
    {
      if (license.IsFeatureValid(robot, skin))
      {
        int num = license.DaysLeft();
        if (num > featureDaysLeft)
          featureDaysLeft = num;
      }
    }
    return featureDaysLeft;
  }

  private static string FixString(string instring, int mult)
  {
    char[] charArray = instring.ToCharArray();
    for (int index = 0; index < charArray.Length; ++index)
      charArray[index] = (char) ((uint) Convert.ToUInt16(charArray[index]) + (uint) (mult * (index % 3)));
    return new string(charArray);
  }
}
