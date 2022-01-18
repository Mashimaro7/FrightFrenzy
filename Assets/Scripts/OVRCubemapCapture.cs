// Decompiled with JetBrains decompiler
// Type: OVRCubemapCapture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public class OVRCubemapCapture : MonoBehaviour
{
  public bool autoTriggerAfterLaunch = true;
  public float autoTriggerDelay = 1f;
  private float autoTriggerElapse;
  public KeyCode triggeredByKey = KeyCode.F8;
  public string pathName;
  public int cubemapSize = 2048;

  private void Update()
  {
    if (this.autoTriggerAfterLaunch)
    {
      this.autoTriggerElapse += Time.deltaTime;
      if ((double) this.autoTriggerElapse >= (double) this.autoTriggerDelay)
      {
        this.autoTriggerAfterLaunch = false;
        OVRCubemapCapture.TriggerCubemapCapture(this.transform.position, this.cubemapSize, this.pathName);
      }
    }
    if (!Input.GetKeyDown(this.triggeredByKey))
      return;
    OVRCubemapCapture.TriggerCubemapCapture(this.transform.position, this.cubemapSize, this.pathName);
  }

  public static void TriggerCubemapCapture(Vector3 capturePos, int cubemapSize = 2048, string pathName = null)
  {
    GameObject gameObject = new GameObject("CubemapCamera", new System.Type[1]
    {
      typeof (Camera)
    });
    gameObject.hideFlags = HideFlags.HideAndDontSave;
    gameObject.transform.position = capturePos;
    gameObject.transform.rotation = Quaternion.identity;
    Camera component = gameObject.GetComponent<Camera>();
    component.farClipPlane = 10000f;
    component.enabled = false;
    Cubemap cubemap = new Cubemap(cubemapSize, TextureFormat.RGB24, false);
    OVRCubemapCapture.RenderIntoCubemap(component, cubemap);
    OVRCubemapCapture.SaveCubemapCapture(cubemap, pathName);
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) cubemap);
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) gameObject);
  }

  public static void RenderIntoCubemap(Camera ownerCamera, Cubemap outCubemap)
  {
    int width = outCubemap.width;
    int height = outCubemap.height;
    CubemapFace[] cubemapFaceArray = new CubemapFace[6]
    {
      CubemapFace.PositiveX,
      CubemapFace.NegativeX,
      CubemapFace.PositiveY,
      CubemapFace.NegativeY,
      CubemapFace.PositiveZ,
      CubemapFace.NegativeZ
    };
    Vector3[] vector3Array = new Vector3[6]
    {
      new Vector3(0.0f, 90f, 0.0f),
      new Vector3(0.0f, -90f, 0.0f),
      new Vector3(-90f, 0.0f, 0.0f),
      new Vector3(90f, 0.0f, 0.0f),
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3(0.0f, 180f, 0.0f)
    };
    RenderTexture active = RenderTexture.active;
    float fieldOfView = ownerCamera.fieldOfView;
    float aspect = ownerCamera.aspect;
    Quaternion rotation = ownerCamera.transform.rotation;
    RenderTexture renderTexture = new RenderTexture(width, height, 24);
    renderTexture.antiAliasing = 8;
    renderTexture.dimension = TextureDimension.Tex2D;
    renderTexture.hideFlags = HideFlags.HideAndDontSave;
    Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGB24, false);
    texture2D.hideFlags = HideFlags.HideAndDontSave;
    ownerCamera.targetTexture = renderTexture;
    ownerCamera.fieldOfView = 90f;
    ownerCamera.aspect = 1f;
    Color[] colors = new Color[texture2D.height * texture2D.width];
    for (int index1 = 0; index1 < cubemapFaceArray.Length; ++index1)
    {
      ownerCamera.transform.eulerAngles = vector3Array[index1];
      ownerCamera.Render();
      RenderTexture.active = renderTexture;
      texture2D.ReadPixels(new Rect(0.0f, 0.0f, (float) width, (float) height), 0, 0);
      Color[] pixels = texture2D.GetPixels();
      for (int index2 = 0; index2 < height; ++index2)
      {
        for (int index3 = 0; index3 < width; ++index3)
          colors[index2 * width + index3] = pixels[(height - 1 - index2) * width + index3];
      }
      outCubemap.SetPixels(colors, cubemapFaceArray[index1]);
    }
    outCubemap.SmoothEdges();
    RenderTexture.active = active;
    ownerCamera.fieldOfView = fieldOfView;
    ownerCamera.aspect = aspect;
    ownerCamera.transform.rotation = rotation;
    ownerCamera.targetTexture = active;
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) texture2D);
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) renderTexture);
  }

  public static bool SaveCubemapCapture(Cubemap cubemap, string pathName = null)
  {
    int width = cubemap.width;
    int height = cubemap.height;
    int x = 0;
    int y = 0;
    string path1;
    string path2;
    if (string.IsNullOrEmpty(pathName))
    {
      path1 = Application.persistentDataPath + "/OVR_ScreenShot360/";
      path2 = (string) null;
    }
    else
    {
      path1 = Path.GetDirectoryName(pathName);
      path2 = Path.GetFileName(pathName);
      if (path1[path1.Length - 1] != '/' || path1[path1.Length - 1] != '\\')
        path1 += "/";
    }
    if (string.IsNullOrEmpty(path2))
      path2 = "OVR_" + DateTime.Now.ToString("hh_mm_ss") + ".png";
    string extension = Path.GetExtension(path2);
    bool flag;
    if (extension == ".png")
      flag = true;
    else if (extension == ".jpg")
    {
      flag = false;
    }
    else
    {
      Debug.LogError((object) ("Unsupported file format" + extension));
      return false;
    }
    try
    {
      Directory.CreateDirectory(path1);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("Failed to create path " + path1 + " since " + ex.ToString()));
      return false;
    }
    Texture2D tex = new Texture2D(width * 6, height, TextureFormat.RGB24, false);
    if ((UnityEngine.Object) tex == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "[OVRScreenshotWizard] Failed creating the texture!");
      return false;
    }
    CubemapFace[] cubemapFaceArray = new CubemapFace[6]
    {
      CubemapFace.PositiveX,
      CubemapFace.NegativeX,
      CubemapFace.PositiveY,
      CubemapFace.NegativeY,
      CubemapFace.PositiveZ,
      CubemapFace.NegativeZ
    };
    foreach (CubemapFace face in cubemapFaceArray)
    {
      Color[] pixels = cubemap.GetPixels(face);
      Color[] colors = new Color[pixels.Length];
      for (int index1 = 0; index1 < height; ++index1)
      {
        for (int index2 = 0; index2 < width; ++index2)
          colors[index1 * width + index2] = pixels[(height - 1 - index1) * width + index2];
      }
      tex.SetPixels(x, y, width, height, colors);
      x += width;
    }
    try
    {
      byte[] bytes = flag ? tex.EncodeToPNG() : tex.EncodeToJPG();
      File.WriteAllBytes(path1 + path2, bytes);
      Debug.Log((object) ("Cubemap file created " + path1 + path2));
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("Failed to save cubemap file since " + ex.ToString()));
      return false;
    }
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) tex);
    return true;
  }
}
