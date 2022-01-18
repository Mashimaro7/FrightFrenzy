// Decompiled with JetBrains decompiler
// Type: OVRSceneSampleController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.XR;

public class OVRSceneSampleController : MonoBehaviour
{
  public KeyCode quitKey = KeyCode.Escape;
  public Texture fadeInTexture;
  public float speedRotationIncrement = 0.05f;
  private OVRPlayerController playerController;
  private OVRCameraRig cameraController;
  public string layerName = "Default";
  private bool visionMode = true;
  private OVRGridCube gridCube;

  private void Awake()
  {
    OVRCameraRig[] componentsInChildren1 = this.gameObject.GetComponentsInChildren<OVRCameraRig>();
    if (componentsInChildren1.Length == 0)
      Debug.LogWarning((object) "OVRMainMenu: No OVRCameraRig attached.");
    else if (componentsInChildren1.Length > 1)
      Debug.LogWarning((object) "OVRMainMenu: More then 1 OVRCameraRig attached.");
    else
      this.cameraController = componentsInChildren1[0];
    OVRPlayerController[] componentsInChildren2 = this.gameObject.GetComponentsInChildren<OVRPlayerController>();
    if (componentsInChildren2.Length == 0)
      Debug.LogWarning((object) "OVRMainMenu: No OVRPlayerController attached.");
    else if (componentsInChildren2.Length > 1)
      Debug.LogWarning((object) "OVRMainMenu: More then 1 OVRPlayerController attached.");
    else
      this.playerController = componentsInChildren2[0];
  }

  private void Start()
  {
    if (!Application.isEditor)
    {
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
    }
    if (!((Object) this.cameraController != (Object) null))
      return;
    this.gridCube = this.gameObject.AddComponent<OVRGridCube>();
    this.gridCube.SetOVRCameraController(ref this.cameraController);
  }

  private void Update()
  {
    this.UpdateRecenterPose();
    this.UpdateVisionMode();
    if ((Object) this.playerController != (Object) null)
      this.UpdateSpeedAndRotationScaleMultiplier();
    if (Input.GetKeyDown(KeyCode.F11))
      Screen.fullScreen = !Screen.fullScreen;
    if (Input.GetKeyDown(KeyCode.M))
      XRSettings.showDeviceView = !XRSettings.showDeviceView;
    if (!Input.GetKeyDown(this.quitKey))
      return;
    Application.Quit();
  }

  private void UpdateVisionMode()
  {
    if (!Input.GetKeyDown(KeyCode.F2))
      return;
    this.visionMode ^= this.visionMode;
    OVRManager.tracker.isEnabled = this.visionMode;
  }

  private void UpdateSpeedAndRotationScaleMultiplier()
  {
    float moveScaleMultiplier = 0.0f;
    this.playerController.GetMoveScaleMultiplier(ref moveScaleMultiplier);
    if (Input.GetKeyDown(KeyCode.Alpha7))
      moveScaleMultiplier -= this.speedRotationIncrement;
    else if (Input.GetKeyDown(KeyCode.Alpha8))
      moveScaleMultiplier += this.speedRotationIncrement;
    this.playerController.SetMoveScaleMultiplier(moveScaleMultiplier);
    float rotationScaleMultiplier = 0.0f;
    this.playerController.GetRotationScaleMultiplier(ref rotationScaleMultiplier);
    if (Input.GetKeyDown(KeyCode.Alpha9))
      rotationScaleMultiplier -= this.speedRotationIncrement;
    else if (Input.GetKeyDown(KeyCode.Alpha0))
      rotationScaleMultiplier += this.speedRotationIncrement;
    this.playerController.SetRotationScaleMultiplier(rotationScaleMultiplier);
  }

  private void UpdateRecenterPose()
  {
    if (!Input.GetKeyDown(KeyCode.R))
      return;
    OVRManager.display.RecenterPose();
  }
}
