// Decompiled with JetBrains decompiler
// Type: OVRDebugInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class OVRDebugInfo : MonoBehaviour
{
  private GameObject debugUIManager;
  private GameObject debugUIObject;
  private GameObject riftPresent;
  private GameObject fps;
  private GameObject ipd;
  private GameObject fov;
  private GameObject height;
  private GameObject depth;
  private GameObject resolutionEyeTexture;
  private GameObject latencies;
  private GameObject texts;
  private string strRiftPresent;
  private string strFPS;
  private string strIPD;
  private string strFOV;
  private string strHeight;
  private string strDepth;
  private string strResolutionEyeTexture;
  private string strLatencies;
  private float updateInterval = 0.5f;
  private float accum;
  private int frames;
  private float timeLeft;
  private bool initUIComponent;
  private bool isInited;
  private float offsetY = 55f;
  private float riftPresentTimeout;
  private bool showVRVars;

  private void Awake()
  {
    this.debugUIManager = new GameObject();
    this.debugUIManager.name = "DebugUIManager";
    this.debugUIManager.transform.parent = GameObject.Find("LeftEyeAnchor").transform;
    RectTransform rectTransform = this.debugUIManager.AddComponent<RectTransform>();
    rectTransform.sizeDelta = new Vector2(100f, 100f);
    rectTransform.localScale = new Vector3(1f / 1000f, 1f / 1000f, 1f / 1000f);
    rectTransform.localPosition = new Vector3(0.01f, 0.17f, 0.53f);
    rectTransform.localEulerAngles = Vector3.zero;
    Canvas canvas = this.debugUIManager.AddComponent<Canvas>();
    canvas.renderMode = RenderMode.WorldSpace;
    canvas.pixelPerfect = false;
  }

  private void Update()
  {
    if (this.initUIComponent && !this.isInited)
      this.InitUIComponents();
    if (Input.GetKeyDown(KeyCode.Space) && (double) this.riftPresentTimeout < 0.0)
    {
      this.initUIComponent = true;
      this.showVRVars = !this.showVRVars;
    }
    this.UpdateDeviceDetection();
    if (this.showVRVars)
    {
      this.debugUIManager.SetActive(true);
      this.UpdateVariable();
      this.UpdateStrings();
    }
    else
      this.debugUIManager.SetActive(false);
  }

  private void OnDestroy() => this.isInited = false;

  private void InitUIComponents()
  {
    float num1 = 0.0f;
    int fontSize = 20;
    this.debugUIObject = new GameObject();
    this.debugUIObject.name = "DebugInfo";
    this.debugUIObject.transform.parent = GameObject.Find("DebugUIManager").transform;
    this.debugUIObject.transform.localPosition = new Vector3(0.0f, 100f, 0.0f);
    this.debugUIObject.transform.localEulerAngles = Vector3.zero;
    this.debugUIObject.transform.localScale = new Vector3(1f, 1f, 1f);
    if (!string.IsNullOrEmpty(this.strFPS))
      this.fps = this.VariableObjectManager(this.fps, "FPS", num1 -= this.offsetY, this.strFPS, fontSize);
    if (!string.IsNullOrEmpty(this.strIPD))
      this.ipd = this.VariableObjectManager(this.ipd, "IPD", num1 -= this.offsetY, this.strIPD, fontSize);
    if (!string.IsNullOrEmpty(this.strFOV))
      this.fov = this.VariableObjectManager(this.fov, "FOV", num1 -= this.offsetY, this.strFOV, fontSize);
    if (!string.IsNullOrEmpty(this.strHeight))
      this.height = this.VariableObjectManager(this.height, "Height", num1 -= this.offsetY, this.strHeight, fontSize);
    if (!string.IsNullOrEmpty(this.strDepth))
      this.depth = this.VariableObjectManager(this.depth, "Depth", num1 -= this.offsetY, this.strDepth, fontSize);
    if (!string.IsNullOrEmpty(this.strResolutionEyeTexture))
      this.resolutionEyeTexture = this.VariableObjectManager(this.resolutionEyeTexture, "Resolution", num1 -= this.offsetY, this.strResolutionEyeTexture, fontSize);
    if (!string.IsNullOrEmpty(this.strLatencies))
    {
      float num2;
      this.latencies = this.VariableObjectManager(this.latencies, "Latency", num2 = num1 - this.offsetY, this.strLatencies, 17);
      num2 = 0.0f;
    }
    this.initUIComponent = false;
    this.isInited = true;
  }

  private void UpdateVariable()
  {
    this.UpdateIPD();
    this.UpdateEyeHeightOffset();
    this.UpdateEyeDepthOffset();
    this.UpdateFOV();
    this.UpdateResolutionEyeTexture();
    this.UpdateLatencyValues();
    this.UpdateFPS();
  }

  private void UpdateStrings()
  {
    if ((Object) this.debugUIObject == (Object) null)
      return;
    if (!string.IsNullOrEmpty(this.strFPS))
      this.fps.GetComponentInChildren<Text>().text = this.strFPS;
    if (!string.IsNullOrEmpty(this.strIPD))
      this.ipd.GetComponentInChildren<Text>().text = this.strIPD;
    if (!string.IsNullOrEmpty(this.strFOV))
      this.fov.GetComponentInChildren<Text>().text = this.strFOV;
    if (!string.IsNullOrEmpty(this.strResolutionEyeTexture))
      this.resolutionEyeTexture.GetComponentInChildren<Text>().text = this.strResolutionEyeTexture;
    if (!string.IsNullOrEmpty(this.strLatencies))
    {
      this.latencies.GetComponentInChildren<Text>().text = this.strLatencies;
      this.latencies.GetComponentInChildren<Text>().fontSize = 14;
    }
    if (!string.IsNullOrEmpty(this.strHeight))
      this.height.GetComponentInChildren<Text>().text = this.strHeight;
    if (string.IsNullOrEmpty(this.strDepth))
      return;
    this.depth.GetComponentInChildren<Text>().text = this.strDepth;
  }

  private void RiftPresentGUI(GameObject guiMainOBj)
  {
    this.riftPresent = this.ComponentComposition(this.riftPresent);
    this.riftPresent.transform.SetParent(guiMainOBj.transform);
    this.riftPresent.name = "RiftPresent";
    RectTransform component = this.riftPresent.GetComponent<RectTransform>();
    component.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    component.localScale = new Vector3(1f, 1f, 1f);
    component.localEulerAngles = Vector3.zero;
    Text componentInChildren = this.riftPresent.GetComponentInChildren<Text>();
    componentInChildren.text = this.strRiftPresent;
    componentInChildren.fontSize = 20;
  }

  private void UpdateDeviceDetection()
  {
    if ((double) this.riftPresentTimeout < 0.0)
      return;
    this.riftPresentTimeout -= Time.deltaTime;
  }

  private GameObject VariableObjectManager(
    GameObject gameObject,
    string name,
    float posY,
    string str,
    int fontSize)
  {
    gameObject = this.ComponentComposition(gameObject);
    gameObject.name = name;
    gameObject.transform.SetParent(this.debugUIObject.transform);
    RectTransform component = gameObject.GetComponent<RectTransform>();
    component.localPosition = new Vector3(0.0f, posY -= this.offsetY, 0.0f);
    Text componentInChildren = gameObject.GetComponentInChildren<Text>();
    componentInChildren.text = str;
    componentInChildren.fontSize = fontSize;
    gameObject.transform.localEulerAngles = Vector3.zero;
    component.localScale = new Vector3(1f, 1f, 1f);
    return gameObject;
  }

  private GameObject ComponentComposition(GameObject GO)
  {
    GO = new GameObject();
    GO.AddComponent<RectTransform>();
    GO.AddComponent<CanvasRenderer>();
    GO.AddComponent<Image>();
    GO.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 50f);
    GO.GetComponent<Image>().color = new Color(0.02745098f, 0.1764706f, 0.2784314f, 0.7843137f);
    this.texts = new GameObject();
    this.texts.AddComponent<RectTransform>();
    this.texts.AddComponent<CanvasRenderer>();
    this.texts.AddComponent<Text>();
    this.texts.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 50f);
    this.texts.GetComponent<Text>().font = UnityEngine.Resources.GetBuiltinResource(typeof (Font), "Arial.ttf") as Font;
    this.texts.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
    this.texts.transform.SetParent(GO.transform);
    this.texts.name = "TextBox";
    return GO;
  }

  private void UpdateIPD() => this.strIPD = string.Format("IPD (mm): {0:F4}", (object) (float) ((double) OVRManager.profile.ipd * 1000.0));

  private void UpdateEyeHeightOffset() => this.strHeight = string.Format("Eye Height (m): {0:F3}", (object) OVRManager.profile.eyeHeight);

  private void UpdateEyeDepthOffset() => this.strDepth = string.Format("Eye Depth (m): {0:F3}", (object) OVRManager.profile.eyeDepth);

  private void UpdateFOV() => this.strFOV = string.Format("FOV (deg): {0:F3}", (object) OVRManager.display.GetEyeRenderDesc(XRNode.LeftEye).fov.y);

  private void UpdateResolutionEyeTexture()
  {
    OVRDisplay.EyeRenderDesc eyeRenderDesc1 = OVRManager.display.GetEyeRenderDesc(XRNode.LeftEye);
    OVRDisplay.EyeRenderDesc eyeRenderDesc2 = OVRManager.display.GetEyeRenderDesc(XRNode.RightEye);
    double renderViewportScale = (double) XRSettings.renderViewportScale;
    this.strResolutionEyeTexture = string.Format("Resolution : {0} x {1}", (object) (float) (int) (renderViewportScale * (double) (eyeRenderDesc1.resolution.x + eyeRenderDesc2.resolution.x)), (object) (float) (int) (renderViewportScale * (double) Mathf.Max(eyeRenderDesc1.resolution.y, eyeRenderDesc2.resolution.y)));
  }

  private void UpdateLatencyValues()
  {
    OVRDisplay.LatencyData latency = OVRManager.display.latency;
    if ((double) latency.render < 9.99999997475243E-07 && (double) latency.timeWarp < 9.99999997475243E-07 && (double) latency.postPresent < 9.99999997475243E-07)
      this.strLatencies = string.Format("Latency values are not available.");
    else
      this.strLatencies = string.Format("Render: {0:F3} TimeWarp: {1:F3} Post-Present: {2:F3}\nRender Error: {3:F3} TimeWarp Error: {4:F3}", (object) latency.render, (object) latency.timeWarp, (object) latency.postPresent, (object) latency.renderError, (object) latency.timeWarpError);
  }

  private void UpdateFPS()
  {
    this.timeLeft -= Time.unscaledDeltaTime;
    this.accum += Time.unscaledDeltaTime;
    ++this.frames;
    if ((double) this.timeLeft > 0.0)
      return;
    this.strFPS = string.Format("FPS: {0:F2}", (object) ((float) this.frames / this.accum));
    this.timeLeft += this.updateInterval;
    this.accum = 0.0f;
    this.frames = 0;
  }
}
