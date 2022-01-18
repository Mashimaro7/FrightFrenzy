// Decompiled with JetBrains decompiler
// Type: MyUtils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.PostProcessing;

public static class MyUtils
{
  private static long start_time = DateTime.Now.Ticks;
  private static Vector3 ftc_pos = new Vector3(-0.25f, 0.0f, 0.0f);
  private static long time_of_last_status = 0;
  public static string status_file_dir;
  public static Dictionary<string, statusfile> status_files;
  public static Dictionary<string, string> score_details;
  private static long time_of_last_command;
  public static List<Saved_Data> recorded_data;
  public static int playback_index;
  public static long playback_offset;
  public static int playback_extra_frames;
  public static int time_offset;
  private static bool autosave_inprogress;
  private static int save_maxtime;
  private static Stream save_outputfile;
  private static List<Saved_Data> save_recorded_data;
  private static int save_position;

  public static long GetTimeMillis() => DateTime.Now.Ticks / 10000L;

  public static long GetTimeMillisSinceStart() => (DateTime.Now.Ticks - MyUtils.start_time) / 10000L;

  public static float GetTimeSinceStart() => (float) (DateTime.Now.Ticks - MyUtils.start_time) / 1E+07f;

  public static byte[] CombineByteArrays(byte[] first, byte[] second)
  {
    byte[] dst = new byte[first.Length + second.Length];
    Buffer.BlockCopy((Array) first, 0, (Array) dst, 0, first.Length);
    Buffer.BlockCopy((Array) second, 0, (Array) dst, first.Length, second.Length);
    return dst;
  }

  public static byte[] CompressMessage(string message, int compression = 1)
  {
    switch (compression)
    {
      case 1:
        return CompressionHelper.Compress(message);
      case 2:
        return ZlibStream.CompressString(message.ToString());
      case 3:
        return BitPacking.Compress(message.ToString());
      default:
        return Encoding.UTF8.GetBytes(message);
    }
  }

  public static string DecompressMessage(byte[] extracted_data, int compression = 1) => Encoding.UTF8.GetString(compression != 0 ? (1 != compression ? (2 != compression ? (3 != compression ? extracted_data : BitPacking.Decompress(extracted_data)) : ZlibStream.UncompressBuffer(extracted_data)) : CompressionHelper.Decompress(extracted_data)) : extracted_data);

  public static bool ExtractMessageHeader(byte[] message, List<byte[]> extracted_data)
  {
    int srcOffset = 0;
    for (int index = 0; index < 4; ++index)
    {
      int count = 0;
      while (count + srcOffset < message.Length && message[count + srcOffset] != (byte) 17)
        ++count;
      if (count + srcOffset == message.Length || count == 0)
        return false;
      byte[] dst = new byte[count];
      Buffer.BlockCopy((Array) message, srcOffset, (Array) dst, 0, count);
      extracted_data.Add(dst);
      srcOffset += count + 1;
    }
    byte[] dst1 = new byte[message.Length - srcOffset];
    Buffer.BlockCopy((Array) message, srcOffset, (Array) dst1, 0, message.Length - srcOffset);
    extracted_data.Add(dst1);
    return true;
  }

  public static byte[] StringToByteArray(string input)
  {
    byte[] byteArray = new byte[input.Length];
    for (int index = 0; index < input.Length; ++index)
      byteArray[index] = (byte) input[index];
    return byteArray;
  }

  public static byte[] CharToByteArray(char[] input, int length)
  {
    byte[] byteArray = new byte[length];
    for (int index = 0; index < length; ++index)
      byteArray[index] = (byte) input[index];
    return byteArray;
  }

  public static void QualityLevel_DisableObjects()
  {
    MinQualityLevel[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll<MinQualityLevel>();
    int qualityLevel = QualitySettings.GetQualityLevel();
    foreach (MinQualityLevel minQualityLevel in objectsOfTypeAll)
    {
      if (minQualityLevel.quality > qualityLevel)
        minQualityLevel.gameObject.SetActive(false);
    }
  }

  public static void SetCameraQualityLevel(GameObject camera)
  {
    if ((UnityEngine.Object) camera == (UnityEngine.Object) null)
      return;
    int qualityLevel = QualitySettings.GetQualityLevel();
    if (qualityLevel <= 1)
    {
      if ((bool) (UnityEngine.Object) camera.GetComponent<VolumetricLightRenderer>())
        camera.GetComponent<VolumetricLightRenderer>().enabled = false;
      if (!(bool) (UnityEngine.Object) camera.GetComponent<PostProcessingBehaviour>())
        return;
      camera.GetComponent<PostProcessingBehaviour>().enabled = false;
    }
    else if (qualityLevel == 2)
    {
      if ((bool) (UnityEngine.Object) camera.GetComponent<PostProcessingBehaviour>())
        camera.GetComponent<PostProcessingBehaviour>().enabled = false;
      if (!(bool) (UnityEngine.Object) camera.GetComponent<VolumetricLightRenderer>())
        return;
      camera.GetComponent<VolumetricLightRenderer>().Resolution = VolumetricLightRenderer.VolumtericResolution.Quarter;
    }
    else
    {
      if (qualityLevel != 3 || !(bool) (UnityEngine.Object) camera.GetComponent<VolumetricLightRenderer>())
        return;
      camera.GetComponent<VolumetricLightRenderer>().Resolution = VolumetricLightRenderer.VolumtericResolution.Half;
    }
  }

  public static bool IsMD5Match(string instring, string MD5)
  {
    string s = MD5.Substring(MD5.Length - 3);
    try
    {
      if (MyUtils.EncryptMD5(instring, int.Parse(s)) == MD5)
        return true;
    }
    catch
    {
      return false;
    }
    return false;
  }

  public static string EncryptMD5(string instring, int random_number = 0)
  {
    if (random_number == 0)
      random_number = new System.Random().Next(100, 999);
    return MyUtils.Md5Sum16("EMPTY<TBD>" + instring + (object) random_number) + random_number.ToString();
  }

  public static string Md5Sum16(string strToEncrypt)
  {
    byte[] hash = new MD5CryptoServiceProvider().ComputeHash(new UTF8Encoding().GetBytes(strToEncrypt));
    string str = "";
    for (int index = 0; index < hash.Length; ++index)
      str += Convert.ToString(hash[index], 16).PadLeft(2, '0');
    str.PadLeft(32, '0');
    return str.Substring(0, 16);
  }

  public static string CreateMD5(int red_score, int blue_score, string position, int rand) => MyUtils.EncryptMD5("RED=" + (object) red_score + "BLUE=" + (object) blue_score + "POS=" + position, rand);

  public static string FindMD5(
    string code,
    int rand,
    int max_num,
    out int red_score,
    out int blue_score,
    out string position)
  {
    string[] strArray = new string[6]
    {
      "Red Left",
      "Red Center",
      "Red Right",
      "Blue Left",
      "Blue Center",
      "Blue Right"
    };
    foreach (string position1 in strArray)
    {
      red_score = 0;
      while (red_score <= max_num)
      {
        blue_score = 0;
        while (blue_score <= max_num)
        {
          string md5 = MyUtils.CreateMD5(red_score, blue_score, position1, rand);
          if (md5.Substring(0, code.Length) == code)
          {
            position = position1;
            return md5;
          }
          ++blue_score;
        }
        ++red_score;
      }
    }
    red_score = 0;
    blue_score = 0;
    position = "None";
    return "NONE FOUND";
  }

  public static string DecryptRSA(string datain)
  {
    RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(new CspParameters()
    {
      KeyContainerName = "xRCPublicKey"
    });
    cryptoServiceProvider.FromXmlString(GLOBALS.XRC_PUBLIC_KEY_XML);
    try
    {
      byte[] bytes = cryptoServiceProvider.Decrypt(Convert.FromBase64String(datain), false);
      string str = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
      Debug.Log((object) ("Decrytped string = " + str));
      return str;
    }
    catch (Exception ex)
    {
      Debug.Log((object) ("Decryption failure: " + (object) ex));
      return "";
    }
  }

  public static string EncryptRSA(string data)
  {
    byte[] bytes = Encoding.UTF8.GetBytes(data);
    return Convert.ToBase64String(MyUtils.CreateProviderFromKey(GLOBALS.XRC_PRIVATEPUBLIC_KEY_XML).SignData(bytes, (object) "SHA1"));
  }

  public static bool RSAVerify(string data, string signature)
  {
    byte[] bytes = Encoding.UTF8.GetBytes(data);
    byte[] signature1 = Convert.FromBase64String(signature);
    return MyUtils.CreateProviderFromKey(GLOBALS.XRC_PUBLIC_KEY_XML).VerifyData(bytes, (object) "SHA1", signature1);
  }

  private static RSACryptoServiceProvider CreateProviderFromKey(string key)
  {
    RSACryptoServiceProvider providerFromKey = new RSACryptoServiceProvider();
    providerFromKey.FromXmlString(key);
    return providerFromKey;
  }

  private static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
  {
    if (plainText == null || plainText.Length <= 0)
      throw new ArgumentNullException(nameof (plainText));
    if (Key == null || Key.Length == 0)
      throw new ArgumentNullException(nameof (Key));
    if (IV == null || IV.Length == 0)
      throw new ArgumentNullException(nameof (IV));
    using (Aes aes = Aes.Create())
    {
      aes.Key = Key;
      aes.IV = IV;
      ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
        {
          using (StreamWriter streamWriter = new StreamWriter((Stream) cryptoStream))
            streamWriter.Write(plainText);
          return memoryStream.ToArray();
        }
      }
    }
  }

  private static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
  {
    if (cipherText == null || cipherText.Length == 0)
      throw new ArgumentNullException(nameof (cipherText));
    if (Key == null || Key.Length == 0)
      throw new ArgumentNullException(nameof (Key));
    if (IV == null || IV.Length == 0)
      throw new ArgumentNullException(nameof (IV));
    AesCryptoServiceProvider cryptoServiceProvider = new AesCryptoServiceProvider();
    byte[] key = cryptoServiceProvider.Key;
    byte[] iv = cryptoServiceProvider.IV;
    using (Aes aes = Aes.Create())
    {
      aes.Key = Key;
      aes.IV = IV;
      ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
      using (MemoryStream memoryStream = new MemoryStream(cipherText))
      {
        using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read))
        {
          using (StreamReader streamReader = new StreamReader((Stream) cryptoStream))
            return streamReader.ReadToEnd();
        }
      }
    }
  }

  public static string ByteArrayToHexString(byte[] ba) => BitConverter.ToString(ba).Replace("-", " ");

  public static byte[] HexStringToByteArray(string hexraw)
  {
    string str = hexraw;
    str.Replace(" ", "");
    int length = str.Length;
    byte[] byteArray = new byte[length / 2];
    for (int startIndex = 0; startIndex < length; startIndex += 2)
      byteArray[startIndex / 2] = Convert.ToByte(str.Substring(startIndex, 2), 16);
    return byteArray;
  }

  private static byte[] GetKey()
  {
    byte[] key = (byte[]) GLOBALS.RANDOMIZER_SEED.Clone();
    int num = 1;
    for (int index = 0; index < key.Length; ++index)
    {
      if (num > 0)
      {
        ++key[index];
        num = -1;
      }
      else
      {
        --key[index];
        num = 1;
      }
    }
    return key;
  }

  public static string EncryptAES(string indata)
  {
    string str = new System.Random().Next(1000, 9999).ToString();
    byte[] bytes = Encoding.ASCII.GetBytes(str + str + str + str);
    return MyUtils.ByteArrayToHexString(MyUtils.EncryptStringToBytes_Aes(indata, MyUtils.GetKey(), bytes)) + " " + str.Substring(0, 2) + " " + str.Substring(2, 2);
  }

  public static string DecryptAES(string indata_raw)
  {
    string str1 = indata_raw.Replace(" ", "");
    string str2 = str1.Substring(str1.Length - 4, 4);
    string hexraw = str1.Substring(0, str1.Length - 4);
    byte[] bytes = Encoding.ASCII.GetBytes(str2 + str2 + str2 + str2);
    return MyUtils.DecryptStringFromBytes_Aes(MyUtils.HexStringToByteArray(hexraw), MyUtils.GetKey(), bytes);
  }

  public static double AngleWrap(double degrees_in) => degrees_in - 360.0 * Math.Floor(degrees_in / 360.0 + 0.5);

  public static float AngleWrap(float degrees_in) => degrees_in - (float) (360.0 * Math.Floor((double) degrees_in / 360.0 + 0.5));

  public static string BoolToString(bool data) => !data ? "0" : "1";

  public static bool StringToBool(string data) => data[0] == '1' || data[0] == 'T' || data[0] == 't';

  public static GameObject InstantiateRobot(
    string model,
    Vector3 start_pos,
    Quaternion start_rot,
    string skins = "0",
    string robot_skin = "")
  {
    GameObject original1 = Resources.Load("Robots/" + model) as GameObject;
    if (!(bool) (UnityEngine.Object) original1)
      return (GameObject) null;
    GameObject gameObject1 = UnityEngine.Object.Instantiate<GameObject>(original1, start_pos, start_rot);
    if (!(bool) (UnityEngine.Object) gameObject1)
      return (GameObject) null;
    if (robot_skin.Length < 1)
      robot_skin = "Real";
    if (GLOBALS.FORCE_REAL)
      robot_skin = "Real";
    GameObject original2 = Resources.Load("Robots/Skins/" + model + "/" + robot_skin) as GameObject;
    if ((bool) (UnityEngine.Object) original2)
    {
      GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(original2, start_pos, start_rot);
      if ((bool) (UnityEngine.Object) gameObject2)
      {
        MyUtils.MergeRobotSkin(gameObject1.transform, gameObject2.transform);
        UnityEngine.Object.Destroy((UnityEngine.Object) gameObject2);
        foreach (RobotSkin componentsInChild in gameObject1.GetComponentsInChildren<RobotSkin>())
          componentsInChild.InitSkin();
      }
    }
    Transform p = gameObject1.transform.Find("Body");
    if (!(bool) (UnityEngine.Object) p)
      return gameObject1;
    RobotInterface3D component = gameObject1.GetComponent<RobotInterface3D>();
    if (!(bool) (UnityEngine.Object) component)
      return gameObject1;
    string[] strArray = skins.Split(':');
    if (strArray.Length != 0 && strArray[0].Length >= 1)
    {
      int key = int.Parse(strArray[0]);
      if (key > 0)
      {
        if (!GLOBALS.skins_eyes.ContainsKey(key))
          return gameObject1;
        GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(GLOBALS.skins_eyes[key], start_pos, start_rot);
        if (!(bool) (UnityEngine.Object) gameObject3)
          return gameObject1;
        gameObject3.transform.SetParent(p);
        gameObject3.transform.localPosition = component.is_FRC ? Vector3.zero : MyUtils.ftc_pos;
        gameObject3.transform.localRotation = Quaternion.identity;
      }
    }
    if (strArray.Length > 1)
    {
      int key = int.Parse(strArray[1]);
      if (key > 0)
      {
        if (!GLOBALS.skins_hats.ContainsKey(key))
          return gameObject1;
        GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(GLOBALS.skins_hats[key], start_pos, start_rot);
        if (!(bool) (UnityEngine.Object) gameObject4)
          return gameObject1;
        gameObject4.transform.SetParent(p);
        gameObject4.transform.localPosition = component.is_FRC ? Vector3.zero : MyUtils.ftc_pos;
        gameObject4.transform.localRotation = Quaternion.identity;
      }
    }
    if (strArray.Length > 2)
    {
      int key = int.Parse(strArray[2]);
      if (key > 0)
      {
        if (!GLOBALS.skins_spoilers.ContainsKey(key))
          return gameObject1;
        GameObject gameObject5 = UnityEngine.Object.Instantiate<GameObject>(GLOBALS.skins_spoilers[key], start_pos, start_rot);
        if (!(bool) (UnityEngine.Object) gameObject5)
          return gameObject1;
        gameObject5.transform.SetParent(p);
        gameObject5.transform.localPosition = Vector3.zero;
        gameObject5.transform.localRotation = Quaternion.identity;
      }
    }
    if (strArray.Length > 3)
    {
      int key = int.Parse(strArray[3]);
      if (key > 0 && GLOBALS.skins_other.ContainsKey(key))
      {
        GameObject gameObject6 = UnityEngine.Object.Instantiate<GameObject>(GLOBALS.skins_other[key], start_pos, start_rot);
        if (!(bool) (UnityEngine.Object) gameObject6)
          return gameObject1;
        gameObject6.transform.SetParent(p);
        gameObject6.transform.localPosition = Vector3.zero;
        gameObject6.transform.localRotation = Quaternion.identity;
      }
    }
    return gameObject1;
  }

  public static void DoScoringFiles(Dictionary<string, string> serverFlags)
  {
    if (MyUtils.GetTimeMillis() - MyUtils.time_of_last_status < GLOBALS.STATUS_FILES_UPDATE_PERIOD)
      return;
    MyUtils.time_of_last_status = MyUtils.GetTimeMillis();
    MyUtils.score_details.Clear();
    if (!serverFlags.ContainsKey("SCORE"))
      return;
    string[] strArray1 = serverFlags["SCORE"].Split(';');
    if (strArray1.Length < 1)
      return;
    for (int index = 0; index < strArray1.Length; ++index)
    {
      string[] strArray2 = strArray1[index].Split('=');
      if (strArray2.Length == 2)
      {
        MyUtils.score_details[strArray2[0]] = strArray2[1];
        if (GLOBALS.OUTPUT_SCORING_FILES)
        {
          if (!MyUtils.status_files.ContainsKey(strArray2[0]))
          {
            if (!Directory.Exists(MyUtils.status_file_dir))
            {
              try
              {
                Directory.CreateDirectory(MyUtils.status_file_dir);
              }
              catch (Exception ex)
              {
                continue;
              }
            }
            statusfile statusfile = new statusfile();
            statusfile.fileinfo = new FileInfo(MyUtils.status_file_dir + Path.DirectorySeparatorChar.ToString() + strArray2[0] + ".txt");
            statusfile.fileinfo.Delete();
            statusfile.stream = statusfile.fileinfo.Open(FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            statusfile.value = "";
            MyUtils.status_files[strArray2[0]] = statusfile;
          }
          statusfile statusFile = MyUtils.status_files[strArray2[0]];
          if (statusFile.stream != null && !(statusFile.value == strArray2[1]))
          {
            statusFile.value = strArray2[1];
            statusFile.stream.SetLength(0L);
            statusFile.stream.Write(Encoding.GetEncoding("UTF-8").GetBytes(strArray2[1].ToCharArray()), 0, strArray2[1].Length);
            statusFile.stream.FlushAsync();
          }
        }
      }
    }
  }

  public static List<string> GetScoringFilesCommand()
  {
    if (MyUtils.GetTimeMillis() - MyUtils.time_of_last_command < 75L)
      return (List<string>) null;
    MyUtils.time_of_last_command = MyUtils.GetTimeMillis();
    if (!GLOBALS.OUTPUT_SCORING_FILES)
      return (List<string>) null;
    if (!Directory.Exists(MyUtils.status_file_dir))
      return (List<string>) null;
    string str1 = MyUtils.status_file_dir + Path.DirectorySeparatorChar.ToString() + "execute.txt";
    if (!File.Exists(str1))
      return (List<string>) null;
    FileInfo fileInfo = new FileInfo(str1);
    FileStream fileStream;
    try
    {
      fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.None);
    }
    catch (IOException ex)
    {
      return (List<string>) null;
    }
    StreamReader streamReader = new StreamReader((Stream) fileStream);
    List<string> scoringFilesCommand = new List<string>();
    for (string str2 = streamReader.ReadLine(); str2 != null; str2 = streamReader.ReadLine())
      scoringFilesCommand.Add(str2);
    string end = streamReader.ReadToEnd();
    if (end.Length > 0)
      scoringFilesCommand.Add(end);
    streamReader.Close();
    File.Delete(str1);
    return scoringFilesCommand;
  }

  public static string GetRedAdj() => MyUtils.score_details.ContainsKey("RedADJ") ? MyUtils.score_details["RedADJ"] : "0";

  public static string GetBlueAdj() => MyUtils.score_details.ContainsKey("BlueADJ") ? MyUtils.score_details["BlueADJ"] : "0";

  public static void CloseScorefiles()
  {
    foreach (statusfile statusfile in MyUtils.status_files.Values)
      statusfile.stream.Close();
    MyUtils.status_files.Clear();
  }

  public static string GetFullName(Transform obj, Transform parent = null)
  {
    string fullName = obj.name;
    while ((UnityEngine.Object) obj.parent != (UnityEngine.Object) null && (UnityEngine.Object) obj.parent != (UnityEngine.Object) parent)
    {
      obj = obj.parent;
      fullName = obj.name + "/" + fullName;
    }
    return fullName;
  }

  public static Vector3 GetPitchYawRollRad(Quaternion rotation)
  {
    float num1 = Mathf.Atan2((float) (2.0 * (double) rotation.y * (double) rotation.w - 2.0 * (double) rotation.x * (double) rotation.z), (float) (1.0 - 2.0 * (double) rotation.y * (double) rotation.y - 2.0 * (double) rotation.z * (double) rotation.z));
    double x = (double) Mathf.Atan2((float) (2.0 * (double) rotation.x * (double) rotation.w - 2.0 * (double) rotation.y * (double) rotation.z), (float) (1.0 - 2.0 * (double) rotation.x * (double) rotation.x - 2.0 * (double) rotation.z * (double) rotation.z));
    float num2 = Mathf.Asin((float) (2.0 * (double) rotation.x * (double) rotation.y + 2.0 * (double) rotation.z * (double) rotation.w));
    double y = (double) num1;
    double z = (double) num2;
    return new Vector3((float) x, (float) y, (float) z);
  }

  public static Vector3 GetPitchYawRollDeg(Quaternion rotation)
  {
    Vector3 pitchYawRollRad = MyUtils.GetPitchYawRollRad(rotation);
    return new Vector3(pitchYawRollRad.x * 57.29578f, pitchYawRollRad.y * 57.29578f, pitchYawRollRad.z * 57.29578f);
  }

  public static void MergeRobotSkin(Transform merged, Transform tomerge)
  {
    List<Transform> transformList = new List<Transform>();
    for (int index = 0; index < tomerge.childCount; ++index)
      transformList.Add(tomerge.GetChild(index));
    for (int index1 = 0; index1 < transformList.Count; ++index1)
    {
      Transform source = transformList[index1];
      Transform target = merged.Find(MyUtils.GetFullName(source, tomerge));
      if ((UnityEngine.Object) target == (UnityEngine.Object) null)
      {
        Transform transform = merged.Find(MyUtils.GetFullName(source.parent, tomerge));
        source.parent = !((UnityEngine.Object) transform == (UnityEngine.Object) null) ? transform : merged;
      }
      else
      {
        if ((UnityEngine.Object) source.GetComponent<Collider>() != (UnityEngine.Object) null)
        {
          Renderer component = target.GetComponent<Renderer>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            component.enabled = true;
        }
        else
          MyUtils.CopyAllComponents(source, target);
        for (int index2 = 0; index2 < source.childCount; ++index2)
          transformList.Add(source.GetChild(index2));
      }
    }
    MyUtils.CopyAllComponents(tomerge, merged);
  }

  public static void CopyAllComponents(Transform source, Transform target)
  {
    foreach (Component component1 in source.GetComponents<Component>())
    {
      Component component2 = target.gameObject.GetComponent(((object) component1).GetType());
      if (!(bool) (UnityEngine.Object) component2)
        component2 = target.gameObject.AddComponent(((object) component1).GetType());
      System.Type type = ((object) component1).GetType();
      foreach (FieldInfo field in type.GetFields())
      {
        if (!field.IsLiteral)
          field.SetValue((object) component2, field.GetValue((object) component1));
      }
      foreach (PropertyInfo property in type.GetProperties())
      {
        if (property.CanWrite && property.CanRead && !(property.Name == "name") && !(property.Name == "parent") && !(property.Name == "hierarchyCapacity"))
          property.SetValue((object) component2, property.GetValue((object) component1, (object[]) null), (object[]) null);
      }
    }
  }

  public static Transform FindHierarchy(Transform parent, string name)
  {
    foreach (Transform componentsInChild in parent.GetComponentsInChildren<Transform>(true))
    {
      if (componentsInChild.name == name)
        return componentsInChild;
    }
    return (Transform) null;
  }

  public static GameObject FindGlobal(string name)
  {
    foreach (GameObject global in Resources.FindObjectsOfTypeAll(typeof (GameObject)) as GameObject[])
    {
      if (global.name == name)
        return global;
    }
    return (GameObject) null;
  }

  public static void LogMessageToFile(string msg, bool isError = true)
  {
    if (!GLOBALS.ENABLE_LOGS)
      return;
    string message = string.Format("{0:G}: {1}.", (object) DateTime.Now, (object) msg);
    if (isError)
      Debug.LogError((object) message);
    else
      Console.WriteLine(message);
  }

  public static void PB_RecordData(string[] data, int time, bool loading_file = false)
  {
    if (!loading_file && !GLOBALS.now_recording)
      return;
    Saved_Data savedData = new Saved_Data();
    savedData.timestamp = time + MyUtils.time_offset;
    savedData.data = data;
    if (MyUtils.recorded_data.Count <= 0)
    {
      MyUtils.recorded_data.Add(savedData);
    }
    else
    {
      for (; savedData.timestamp - MyUtils.recorded_data[MyUtils.recorded_data.Count - 1].timestamp < -43200000; savedData.timestamp += 86400000)
        MyUtils.time_offset += 86400000;
      while (savedData.timestamp - MyUtils.recorded_data[MyUtils.recorded_data.Count - 1].timestamp > 43200000)
        savedData.timestamp -= 86400000;
      int count = MyUtils.recorded_data.Count;
      while (count > 0 && MyUtils.recorded_data[count - 1].timestamp > savedData.timestamp)
        --count;
      MyUtils.recorded_data.Insert(count, savedData);
      if (loading_file)
        return;
      while (MyUtils.recorded_data[MyUtils.recorded_data.Count - 1].timestamp - MyUtils.recorded_data[0].timestamp > GLOBALS.PB_BUFFER_DURATION * 60 * 1000)
        MyUtils.recorded_data.RemoveAt(0);
    }
  }

  public static long PB_GetStartTime() => MyUtils.recorded_data.Count < 1 ? -1L : (long) MyUtils.recorded_data[0].timestamp;

  public static long PB_GetEndTime() => MyUtils.recorded_data.Count < 1 ? -1L : (long) MyUtils.recorded_data[MyUtils.recorded_data.Count - 1].timestamp;

  public static long PB_GetCurrentTime() => MyUtils.PB_ReachedEnd() ? MyUtils.PB_GetEndTime() : (long) MyUtils.recorded_data[MyUtils.playback_index].timestamp;

  public static bool PB_StartPlayback(long start_time)
  {
    MyUtils.playback_index = -1;
    do
      ;
    while (++MyUtils.playback_index < MyUtils.recorded_data.Count && (long) MyUtils.recorded_data[MyUtils.playback_index].timestamp < start_time);
    if (MyUtils.PB_ReachedEnd())
      return false;
    MyUtils.playback_offset = (long) ((double) MyUtils.GetTimeMillisSinceStart() - (double) start_time / (double) GLOBALS.playback_speed);
    return true;
  }

  public static Saved_Data PB_GetNext()
  {
    if (MyUtils.playback_index < 0)
      return (Saved_Data) null;
    if (MyUtils.playback_index >= MyUtils.recorded_data.Count)
      return (Saved_Data) null;
    long num = (long) ((double) (MyUtils.GetTimeMillisSinceStart() - MyUtils.playback_offset) * (double) GLOBALS.playback_speed);
    while (MyUtils.playback_extra_frames > 0)
    {
      int index = MyUtils.playback_index - MyUtils.playback_extra_frames--;
      if (index >= 0)
        return MyUtils.recorded_data[index];
    }
    return (long) MyUtils.recorded_data[MyUtils.playback_index].timestamp < num ? MyUtils.recorded_data[MyUtils.playback_index++] : (Saved_Data) null;
  }

  public static bool PB_ReachedEnd() => MyUtils.playback_index < 0 || MyUtils.playback_index >= MyUtils.recorded_data.Count;

  public static void PB_ClearRecording()
  {
    MyUtils.recorded_data.Clear();
    MyUtils.playback_index = -1;
    MyUtils.playback_offset = -1L;
    MyUtils.time_offset = 0;
  }

  public static void PB_UpdateDuringPause()
  {
    if (MyUtils.playback_index < 0 || MyUtils.playback_index >= MyUtils.recorded_data.Count)
      return;
    MyUtils.playback_offset = (long) ((double) MyUtils.GetTimeMillisSinceStart() - (double) MyUtils.recorded_data[MyUtils.playback_index].timestamp / (double) GLOBALS.playback_speed);
  }

  public static bool PB_ChangeTime(long start_time)
  {
    int index = -1;
    do
      ;
    while (++index < MyUtils.recorded_data.Count && (long) MyUtils.recorded_data[index].timestamp < start_time);
    MyUtils.playback_index = index;
    MyUtils.playback_extra_frames = 20;
    MyUtils.playback_offset = (long) ((double) MyUtils.GetTimeMillisSinceStart() - (double) start_time / (double) GLOBALS.playback_speed);
    return true;
  }

  public static void PB_ChangeSpeed(float speed)
  {
    GLOBALS.playback_speed = speed;
    MyUtils.PB_UpdateDuringPause();
  }

  public static int GetEndRecordingOffset()
  {
    if (!(bool) (UnityEngine.Object) GLOBALS.topclient)
      return 0;
    for (int index = MyUtils.recorded_data.Count - 1; index >= 0; --index)
    {
      if (GLOBALS.topclient.IsPlaybackDataEndOfMatch(MyUtils.recorded_data[index]))
        return MyUtils.recorded_data.Count - index + 1;
    }
    return 0;
  }

  public static int GetStartRecordingOffset(int end_offset)
  {
    if (!(bool) (UnityEngine.Object) GLOBALS.topclient)
      return 0;
    for (int index = MyUtils.recorded_data.Count - end_offset - 1; index >= 0; --index)
    {
      if (GLOBALS.topclient.IsPlaybackDataStartOfMatch(MyUtils.recorded_data[index]))
        return index;
    }
    return 0;
  }

  public static bool PB_AutoSaveInProgress() => MyUtils.autosave_inprogress;

  public static float PB_AutoSavePercentage() => MyUtils.autosave_inprogress ? (float) MyUtils.save_position / (float) MyUtils.save_recorded_data.Count : 1f;

  public static bool PB_AutoSaveToFile()
  {
    if (MyUtils.autosave_inprogress)
    {
      MyUtils.PB_SaveToFile("", MyUtils.save_position, incremental: true, my_recorded_data: MyUtils.save_recorded_data);
      if (!MyUtils.autosave_inprogress)
      {
        MyUtils.save_recorded_data.Clear();
        MyUtils.save_recorded_data = (List<Saved_Data>) null;
        MyUtils.save_outputfile = (Stream) null;
        MyUtils.save_position = 0;
      }
      return true;
    }
    if (!GLOBALS.autosave_recordings || !GLOBALS.now_recording || GLOBALS.autosave_filename.Length < 1)
      return false;
    int num1 = 1;
    string str = "";
    for (; num1 <= 999; ++num1)
    {
      str = GLOBALS.autosave_filename;
      if (str.EndsWith(".xrc"))
        str = str.Remove(str.Length - 4) + (object) num1 + ".xrc";
      if (!File.Exists(str))
        break;
    }
    if (num1 > 999)
    {
      MyUtils.LogMessageToFile("Unable to autosave to file " + GLOBALS.autosave_filename + ". Reached 999 limit on filename ending.");
      return false;
    }
    int endRecordingOffset = MyUtils.GetEndRecordingOffset();
    int startRecordingOffset = MyUtils.GetStartRecordingOffset(endRecordingOffset);
    int num2 = endRecordingOffset <= 10 ? 0 : endRecordingOffset - 10;
    int num3 = startRecordingOffset <= 10 ? 0 : startRecordingOffset - 10;
    MyUtils.save_recorded_data = new List<Saved_Data>();
    for (int index = num3; index < MyUtils.recorded_data.Count - num2; ++index)
      MyUtils.save_recorded_data.Add(new Saved_Data()
      {
        timestamp = MyUtils.recorded_data[index].timestamp,
        data = MyUtils.recorded_data[index].data
      });
    return MyUtils.PB_SaveToFile(str, incremental: true, my_recorded_data: MyUtils.save_recorded_data);
  }

  public static bool PB_SaveToFile(
    string filename,
    int start = 0,
    int stop = 0,
    bool incremental = false,
    List<Saved_Data> my_recorded_data = null)
  {
    if (my_recorded_data == null)
      my_recorded_data = MyUtils.recorded_data;
    Stream stream1;
    if (!incremental || !MyUtils.autosave_inprogress)
    {
      FileInfo fileInfo = new FileInfo(filename);
      FileStream fileStream;
      string s;
      try
      {
        fileStream = fileInfo.Open(FileMode.Create, FileAccess.Write);
        fileStream.Write(Encoding.GetEncoding("UTF-8").GetBytes(GLOBALS.VERSION), 0, Encoding.GetEncoding("UTF-8").GetByteCount(GLOBALS.VERSION));
        fileStream.WriteByte((byte) 22);
        fileStream.Write(Encoding.GetEncoding("UTF-8").GetBytes(GLOBALS.GAME_INDEX.ToString()), 0, Encoding.GetEncoding("UTF-8").GetByteCount(GLOBALS.GAME_INDEX.ToString()));
        fileStream.WriteByte((byte) 22);
        s = "2";
        fileStream.Write(Encoding.GetEncoding("UTF-8").GetBytes(s), 0, Encoding.GetEncoding("UTF-8").GetByteCount(s));
        fileStream.WriteByte((byte) 22);
      }
      catch (Exception ex)
      {
        MyUtils.LogMessageToFile("Open file failed " + (object) ex);
        return false;
      }
      if (s == "3")
      {
        Stream stream2 = (Stream) new GZipStream((Stream) fileStream, CompressionMode.Compress, CompressionLevel.Level2);
      }
      stream1 = !(s == "2") ? (!(s == "1") ? (Stream) fileStream : (Stream) new ZlibStream((Stream) fileStream, CompressionMode.Compress)) : (Stream) new GZipStream((Stream) fileStream, CompressionMode.Compress, CompressionLevel.Level5);
    }
    else
      stream1 = MyUtils.save_outputfile;
    long timeMillis = MyUtils.GetTimeMillis();
    try
    {
      for (int index = start; index < my_recorded_data.Count - stop; ++index)
      {
        if (incremental && MyUtils.GetTimeMillis() - timeMillis > (long) MyUtils.save_maxtime)
        {
          MyUtils.save_position = index;
          MyUtils.save_outputfile = stream1;
          MyUtils.autosave_inprogress = true;
          return true;
        }
        string s = my_recorded_data[index].timestamp.ToString() + "\u0016" + string.Join('\u0017'.ToString(), my_recorded_data[index].data);
        byte[] bytes1 = Encoding.GetEncoding("UTF-8").GetBytes(s);
        byte[] bytes2 = Encoding.GetEncoding("UTF-8").GetBytes(bytes1.Length.ToString());
        stream1.Write(bytes2, 0, bytes2.Length);
        stream1.WriteByte((byte) 22);
        stream1.Write(bytes1, 0, bytes1.Length);
      }
      stream1.Close();
      if (incremental)
      {
        MyUtils.save_position = 0;
        MyUtils.save_outputfile = (Stream) null;
        MyUtils.autosave_inprogress = false;
      }
    }
    catch (Exception ex)
    {
      MyUtils.LogMessageToFile("Open/write file failed " + (object) ex);
      return false;
    }
    return true;
  }

  public static string PB_GetString(Stream inputfile)
  {
    byte[] numArray = new byte[100];
    char[] chars1 = new char[250];
    int byteCount = 100;
    string str1 = "";
    Decoder decoder = Encoding.GetEncoding("UTF-8").GetDecoder();
    while (byteCount >= 100)
    {
      long position = inputfile.Position;
      byteCount = inputfile.Read(numArray, 0, 100);
      int chars2 = decoder.GetChars(numArray, 0, byteCount, chars1, 0);
      for (int length = 0; length < chars2; ++length)
      {
        if (chars1[length] == '\u0016')
        {
          string str2 = str1 + new string(chars1, 0, length);
          inputfile.Position = position + 1L;
          return str2;
        }
        ++position;
      }
      str1 += new string(chars1, 0, chars2);
    }
    return str1;
  }

  public static bool PB_GetLineFromStream(Stream inputfile, out string outline)
  {
    outline = "";
    string s = MyUtils.PB_GetString(inputfile);
    int count = 0;
    ref int local = ref count;
    if (!int.TryParse(s, out local) || inputfile.Length - inputfile.Position < (long) count)
    {
      MyUtils.LogMessageToFile("Read line error at file location " + (object) inputfile.Position + " expecting " + (object) count + " bytes from available " + (object) (inputfile.Length - inputfile.Position) + ".");
      return false;
    }
    byte[] numArray = new byte[count];
    int num = inputfile.Read(numArray, 0, count);
    if (num != count)
    {
      MyUtils.LogMessageToFile("Unable to read all required bytes at file location " + (object) inputfile.Position);
      return false;
    }
    Decoder decoder = Encoding.GetEncoding("UTF-8").GetDecoder();
    char[] chars = new char[decoder.GetCharCount(numArray, 0, num)];
    decoder.GetChars(numArray, 0, num, chars, 0);
    outline = new string(chars);
    return true;
  }

  public static bool PB_LoadFromFile(string filename)
  {
    FileInfo fileInfo = new FileInfo(filename);
    FileStream inputfile = (FileStream) null;
    try
    {
      inputfile = fileInfo.Open(FileMode.Open, FileAccess.Read);
      string str1 = MyUtils.PB_GetString((Stream) inputfile);
      string str2 = MyUtils.PB_GetString((Stream) inputfile);
      string str3 = MyUtils.PB_GetString((Stream) inputfile);
      if (GLOBALS.VERSION != str1 || GLOBALS.GAME_INDEX.ToString() != str2 || str3 != "1")
      {
        if (str1.Length < 1 || str1[0] != 'v' || str2.Length < 1)
        {
          if ((bool) (UnityEngine.Object) GLOBALS.topclient)
            GLOBALS.topclient.ShowMessage("This appears not to be a xRC Data file. Aborting.");
          else
            MyUtils.LogMessageToFile("This appears not to be a xRC Data file. Aborting.");
          return false;
        }
        if (GLOBALS.VERSION != str1)
        {
          if ((bool) (UnityEngine.Object) GLOBALS.topclient)
            GLOBALS.topclient.ShowMessage("Version mis-match: our version = " + GLOBALS.VERSION + ", Data file = " + str1 + ". Issues may occur.");
          else
            MyUtils.LogMessageToFile("Version mis-match: our version = " + GLOBALS.VERSION + ", Data file = " + str1 + ". Issues may occur.");
          GLOBALS.FORCE_OLD_BHELP = (double) float.Parse(str1.Substring(1, str1.Length - 2)) < 6.19999980926514 || str1 == "v6.2a" || str1 == "v6.2b";
        }
        if (GLOBALS.GAME_INDEX.ToString() != str2)
        {
          if ((bool) (UnityEngine.Object) GLOBALS.topclient)
            GLOBALS.topclient.ShowMessage("Wrong game: our game = " + (object) GLOBALS.GAME_INDEX + ", Data file = " + str2);
          else
            MyUtils.LogMessageToFile("Wrong game: our game = " + (object) GLOBALS.GAME_INDEX + ", Data file = " + str2);
          return false;
        }
        if (str3.Length != 1)
        {
          inputfile.Close();
          return MyUtils.PB_LoadFromFile_old(filename);
        }
      }
      MyUtils.PB_ClearRecording();
      Stream stream1 = (Stream) new MemoryStream();
      Stream stream2 = !(str3 == "0") ? (!(str3 == "1") ? (Stream) new GZipStream((Stream) inputfile, CompressionMode.Decompress) : (Stream) new ZlibStream((Stream) inputfile, CompressionMode.Decompress)) : (Stream) inputfile;
      stream2.CopyTo(stream1);
      stream2.Close();
      stream1.Position = 0L;
      while (stream1.Length - stream1.Position > 5L)
      {
        string outline;
        if (!MyUtils.PB_GetLineFromStream(stream1, out outline))
        {
          MyUtils.LogMessageToFile("Read line error at file location " + (object) inputfile.Position);
        }
        else
        {
          string[] strArray = outline.Split('\u0016');
          string s = strArray[0];
          int time = 0;
          ref int local = ref time;
          if (!int.TryParse(s, out local) || strArray.Length < 2)
            MyUtils.LogMessageToFile("Read line error at file location " + (object) inputfile.Position + " time=" + (object) time + ", File Length=" + (object) (inputfile.Length - inputfile.Position));
          else
            MyUtils.PB_RecordData(strArray[1].Split('\u0017'), time, true);
        }
      }
      stream1.Close();
      inputfile.Close();
    }
    catch (Exception ex)
    {
      if ((bool) (UnityEngine.Object) GLOBALS.topclient)
        GLOBALS.topclient.ShowMessage("Open / read file failed " + (object) ex);
      MyUtils.LogMessageToFile("Open/read file failed " + (object) ex);
      inputfile?.Close();
      return false;
    }
    return true;
  }

  public static bool PB_LoadFromFile_old(string filename)
  {
    FileInfo fileInfo = new FileInfo(filename);
    try
    {
      FileStream inputfile = fileInfo.Open(FileMode.Open, FileAccess.Read);
      string str1 = MyUtils.PB_GetString((Stream) inputfile);
      string str2 = MyUtils.PB_GetString((Stream) inputfile);
      if (GLOBALS.VERSION != str1 || GLOBALS.GAME_INDEX.ToString() != str2)
      {
        if (str1.Length < 1 || str1[0] != 'v' || str2.Length < 1)
        {
          if ((bool) (UnityEngine.Object) GLOBALS.topclient)
            GLOBALS.topclient.ShowMessage("This appears not to be a xRC Data file. Aborting.");
          else
            MyUtils.LogMessageToFile("This appears not to be a xRC Data file. Aborting.");
          return false;
        }
        if (GLOBALS.VERSION != str1)
        {
          int num = (bool) (UnityEngine.Object) GLOBALS.topclient ? 1 : 0;
        }
        if (GLOBALS.GAME_INDEX.ToString() != str2)
        {
          if ((bool) (UnityEngine.Object) GLOBALS.topclient)
            GLOBALS.topclient.ShowMessage("Wrong game: our game = " + (object) GLOBALS.GAME_INDEX + ", Data file = " + str2);
          else
            MyUtils.LogMessageToFile("Wrong game: our game = " + (object) GLOBALS.GAME_INDEX + ", Data file = " + str2);
          return false;
        }
      }
      MyUtils.PB_ClearRecording();
      while (inputfile.Length - inputfile.Position > 5L)
      {
        string s = MyUtils.PB_GetString((Stream) inputfile);
        int time = 0;
        ref int local = ref time;
        if (!int.TryParse(s, out local) || inputfile.Length - inputfile.Position < 1L)
          MyUtils.LogMessageToFile("Read line error at file location " + (object) inputfile.Position);
        else
          MyUtils.PB_RecordData(MyUtils.PB_GetString((Stream) inputfile).Split('\u0017'), time, true);
      }
    }
    catch (Exception ex)
    {
      if ((bool) (UnityEngine.Object) GLOBALS.topclient)
        GLOBALS.topclient.ShowMessage("Open / read file failed " + (object) ex);
      MyUtils.LogMessageToFile("Open/read file failed " + (object) ex);
      return false;
    }
    return true;
  }

  public static void AppendFile(string filename, string msg)
  {
    StreamWriter streamWriter = File.AppendText(filename);
    streamWriter.WriteLine(msg);
    streamWriter.Close();
  }

  static MyUtils()
  {
    char directorySeparatorChar = Path.DirectorySeparatorChar;
    string str1 = directorySeparatorChar.ToString();
    directorySeparatorChar = Path.DirectorySeparatorChar;
    string str2 = directorySeparatorChar.ToString();
    MyUtils.status_file_dir = str1 + "temp" + str2 + "xRCsim";
    MyUtils.status_files = new Dictionary<string, statusfile>();
    MyUtils.score_details = new Dictionary<string, string>();
    MyUtils.time_of_last_command = -1L;
    MyUtils.recorded_data = new List<Saved_Data>();
    MyUtils.playback_index = 0;
    MyUtils.playback_offset = -1L;
    MyUtils.playback_extra_frames = 0;
    MyUtils.time_offset = 0;
    MyUtils.autosave_inprogress = false;
    MyUtils.save_maxtime = 20;
    MyUtils.save_position = 0;
  }
}
