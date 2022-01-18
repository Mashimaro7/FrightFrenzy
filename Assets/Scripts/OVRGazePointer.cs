// Decompiled with JetBrains decompiler
// Type: OVRGazePointer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class OVRGazePointer : MonoBehaviour
{
  private Transform trailFollower;
  [Tooltip("Should the pointer be hidden when not over interactive objects.")]
  public bool hideByDefault = true;
  [Tooltip("Time after leaving interactive object before pointer fades.")]
  public float showTimeoutPeriod = 1f;
  [Tooltip("Time after mouse pointer becoming inactive before pointer unfades.")]
  public float hideTimeoutPeriod = 0.1f;
  [Tooltip("Keep a faint version of the pointer visible while using a mouse")]
  public bool dimOnHideRequest = true;
  [Tooltip("Angular scale of pointer")]
  public float depthScaleMultiplier = 0.03f;
  public Transform rayTransform;
  private float depth;
  private float hideUntilTime;
  private int positionSetsThisFrame;
  private Vector3 lastPosition;
  private float lastShowRequestTime;
  private float lastHideRequestTime;
  [Tooltip("Radius of the cursor. Used for preventing geometry intersections.")]
  public float cursorRadius = 1f;
  private OVRProgressIndicator progressIndicator;
  private static OVRGazePointer _instance;

  public bool hidden { get; private set; }

  public float currentScale { get; private set; }

  public Vector3 positionDelta { private set; get; }

  public static OVRGazePointer instance
  {
    get
    {
      if ((Object) OVRGazePointer._instance == (Object) null)
      {
        Debug.Log((object) string.Format("Instanciating GazePointer", (object) 0));
        OVRGazePointer._instance = Object.Instantiate<OVRGazePointer>((OVRGazePointer) Resources.Load("Prefabs/GazePointerRing", typeof (OVRGazePointer)));
      }
      return OVRGazePointer._instance;
    }
  }

  public float visibilityStrength => Mathf.Min(!this.hideByDefault ? 1f : Mathf.Clamp01((float) (1.0 - ((double) Time.time - (double) this.lastShowRequestTime) / (double) this.showTimeoutPeriod)), (double) this.lastHideRequestTime + (double) this.hideTimeoutPeriod > (double) Time.time ? (this.dimOnHideRequest ? 0.1f : 0.0f) : 1f);

  public float SelectionProgress
  {
    get => !(bool) (Object) this.progressIndicator ? 0.0f : this.progressIndicator.currentProgress;
    set
    {
      if (!(bool) (Object) this.progressIndicator)
        return;
      this.progressIndicator.currentProgress = value;
    }
  }

  public void Awake()
  {
    this.currentScale = 1f;
    if ((Object) OVRGazePointer._instance != (Object) null && (Object) OVRGazePointer._instance != (Object) this)
    {
      this.enabled = false;
      Object.DestroyImmediate((Object) this);
    }
    else
    {
      OVRGazePointer._instance = this;
      this.trailFollower = this.transform.Find("TrailFollower");
      this.progressIndicator = this.transform.GetComponent<OVRProgressIndicator>();
    }
  }

  private void Update()
  {
    if ((Object) this.rayTransform == (Object) null && (Object) Camera.main != (Object) null)
      this.rayTransform = Camera.main.transform;
    this.transform.position = this.rayTransform.position + this.rayTransform.forward * this.depth;
    if ((double) this.visibilityStrength == 0.0 && !this.hidden)
    {
      this.Hide();
    }
    else
    {
      if ((double) this.visibilityStrength <= 0.0 || !this.hidden)
        return;
      this.Show();
    }
  }

  public void SetPosition(Vector3 pos, Vector3 normal)
  {
    this.transform.position = pos;
    Quaternion rotation = this.transform.rotation;
    rotation.SetLookRotation(normal, this.rayTransform.up);
    this.transform.rotation = rotation;
    this.depth = (this.rayTransform.position - pos).magnitude;
    this.currentScale = this.depth * this.depthScaleMultiplier;
    this.transform.localScale = new Vector3(this.currentScale, this.currentScale, this.currentScale);
    ++this.positionSetsThisFrame;
  }

  public void SetPosition(Vector3 pos) => this.SetPosition(pos, this.rayTransform.forward);

  public float GetCurrentRadius() => this.cursorRadius * this.currentScale;

  private void LateUpdate()
  {
    if (this.positionSetsThisFrame == 0)
    {
      Quaternion rotation = this.transform.rotation;
      rotation.SetLookRotation(this.rayTransform.forward, this.rayTransform.up);
      this.transform.rotation = rotation;
    }
    if ((Object) this.trailFollower == (Object) null)
      return;
    Quaternion rotation1 = this.trailFollower.rotation;
    rotation1.SetLookRotation(this.transform.rotation * new Vector3(0.0f, 0.0f, 1f), (this.lastPosition - this.transform.position).normalized);
    this.trailFollower.rotation = rotation1;
    this.positionDelta = this.transform.position - this.lastPosition;
    this.lastPosition = this.transform.position;
    this.positionSetsThisFrame = 0;
  }

  public void RequestHide()
  {
    if (!this.dimOnHideRequest)
      this.Hide();
    this.lastHideRequestTime = Time.time;
  }

  public void RequestShow()
  {
    this.Show();
    this.lastShowRequestTime = Time.time;
  }

  private void Hide()
  {
    foreach (Component component in this.transform)
      component.gameObject.SetActive(false);
    if ((bool) (Object) this.GetComponent<Renderer>())
      this.GetComponent<Renderer>().enabled = false;
    this.hidden = true;
  }

  private void Show()
  {
    foreach (Component component in this.transform)
      component.gameObject.SetActive(true);
    if ((bool) (Object) this.GetComponent<Renderer>())
      this.GetComponent<Renderer>().enabled = true;
    this.hidden = false;
  }
}
