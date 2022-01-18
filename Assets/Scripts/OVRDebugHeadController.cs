// Decompiled with JetBrains decompiler
// Type: OVRDebugHeadController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.XR;

public class OVRDebugHeadController : MonoBehaviour
{
  [SerializeField]
  public bool AllowPitchLook;
  [SerializeField]
  public bool AllowYawLook = true;
  [SerializeField]
  public bool InvertPitch;
  [SerializeField]
  public float GamePad_PitchDegreesPerSec = 90f;
  [SerializeField]
  public float GamePad_YawDegreesPerSec = 90f;
  [SerializeField]
  public bool AllowMovement;
  [SerializeField]
  public float ForwardSpeed = 2f;
  [SerializeField]
  public float StrafeSpeed = 2f;
  protected OVRCameraRig CameraRig;

  private void Awake()
  {
    OVRCameraRig[] componentsInChildren = this.gameObject.GetComponentsInChildren<OVRCameraRig>();
    if (componentsInChildren.Length == 0)
      Debug.LogWarning((object) "OVRCamParent: No OVRCameraRig attached.");
    else if (componentsInChildren.Length > 1)
      Debug.LogWarning((object) "OVRCamParent: More then 1 OVRCameraRig attached.");
    else
      this.CameraRig = componentsInChildren[0];
  }

  private void Start()
  {
  }

  private void Update()
  {
    if (this.AllowMovement)
      this.transform.position += this.CameraRig.centerEyeAnchor.rotation * Vector3.forward * OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y * Time.deltaTime * this.ForwardSpeed + this.CameraRig.centerEyeAnchor.rotation * Vector3.right * OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x * Time.deltaTime * this.StrafeSpeed;
    if (XRDevice.isPresent || !this.AllowYawLook && !this.AllowPitchLook)
      return;
    Quaternion quaternion1 = this.transform.rotation;
    if (this.AllowYawLook)
      quaternion1 = Quaternion.AngleAxis(OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x * Time.deltaTime * this.GamePad_YawDegreesPerSec, Vector3.up) * quaternion1;
    if (this.AllowPitchLook)
    {
      float y = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y;
      if ((double) Mathf.Abs(y) > 9.99999974737875E-05)
      {
        if (this.InvertPitch)
          y *= -1f;
        Quaternion quaternion2 = Quaternion.AngleAxis(y * Time.deltaTime * this.GamePad_PitchDegreesPerSec, Vector3.left);
        quaternion1 *= quaternion2;
      }
    }
    this.transform.rotation = quaternion1;
  }
}
