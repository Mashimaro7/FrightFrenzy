// Decompiled with JetBrains decompiler
// Type: CountdownCameraController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class CountdownCameraController : MonoBehaviour
{
  private CountdownCameraController.CameraStates currentCameraState;
  private float stateStartTime;
  private bool stateFinished = true;
  public static CountdownCameraController instance;
  private Vector3 savedStartingLocation;
  private Quaternion savedStartingRotation;
  private RobotInterface3D[] allRobots;
  private int currentPlayerIndex;
  public Vector3 ORBIT_START_POS = new Vector3(1.4f, 0.2f, 0.0f);
  public Vector3 ORBIT_DIRECTION = new Vector3(1f, 0.0f, 0.0f);
  public Vector3 ORBIT_LOOK_OFFSET = new Vector3(0.0f, 0.3f, 0.0f);
  public float ORBIT_SPEED = 0.5f;
  public Vector3 BUMPERSLIDE_START_POS = new Vector3(-0.3f, 1f, 1f);
  public Vector3 BUMPERSLIDE_LOOK_OFFSET = new Vector3(0.0f, 0.3f, 0.0f);
  public Vector3 BUMPERSLIDE_DIRECTION = new Vector3(0.0f, 0.0f, 1f);
  public float BUMPERSLIDE_SPEED = 0.5f;
  public Vector3 OVERHEAD_ZOOMOUT_START_POS = new Vector3(-0.3f, 1f, 1f);
  public Vector3 OVERHEAD_ZOOMOUT_LOOK_OFFSET = new Vector3(0.0f, 0.3f, 0.0f);
  public float OVERHEAD_ZOOMOUT_SPEED = 0.1f;

  private float stateElapsedTime => Time.time - this.stateStartTime;

  private void SetState(int index)
  {
    this.currentCameraState = (CountdownCameraController.CameraStates) index;
    this.stateFinished = true;
    this.stateStartTime = Time.time;
  }

  private void NextState() => this.SetState((int) (this.currentCameraState + 1));

  private Transform _camera
  {
    get
    {
      Transform transform = ClientLow.instance?.main_camera?.transform;
      if (transform != null)
        return transform;
      return SinglePlayer.main_camera?.transform;
    }
  }

  private RobotInterface3D currentRobotInView => this.allRobots?[this.currentPlayerIndex];

  private Transform currentFocusedBody => this.currentRobotInView.rb_body.transform;

  private void Awake() => CountdownCameraController.instance = this;

  private void LateUpdate()
  {
    if (this.currentCameraState == CountdownCameraController.CameraStates.DoingNothing && this.stateFinished)
      this.stateFinished = false;
    if (this.currentCameraState == CountdownCameraController.CameraStates.FieldAnimation1 && this.stateFinished)
    {
      this.stateFinished = false;
      this._camera.gameObject.GetComponent<Animator>().Play("IntroAnimation1", -1, 0.0f);
    }
    if (this.currentCameraState == CountdownCameraController.CameraStates.OrbitAroundPlayer)
    {
      if (this.stateFinished)
      {
        this.stateFinished = false;
        this._camera.position = this.currentFocusedBody.localToWorldMatrix.MultiplyPoint(this.ORBIT_START_POS);
      }
      this._camera.Translate(Time.deltaTime * this.ORBIT_SPEED * this.ORBIT_DIRECTION);
      this._camera.LookAt(this.currentFocusedBody.position + this.ORBIT_LOOK_OFFSET);
      if ((double) this.stateElapsedTime >= 1.0)
      {
        this.NextState();
        ++this.currentPlayerIndex;
        this.currentPlayerIndex %= this.allRobots.Length;
      }
    }
    if (this.currentCameraState == CountdownCameraController.CameraStates.SlideByBumper)
    {
      if (this.stateFinished)
      {
        this.stateFinished = false;
        this._camera.position = this.currentFocusedBody.localToWorldMatrix.MultiplyPoint(this.BUMPERSLIDE_START_POS);
        this._camera.LookAt(this.currentFocusedBody.position + this.BUMPERSLIDE_LOOK_OFFSET);
      }
      this._camera.Translate(Time.deltaTime * this.BUMPERSLIDE_SPEED * this.BUMPERSLIDE_DIRECTION);
      if ((double) this.stateElapsedTime >= 1.0)
      {
        this.NextState();
        ++this.currentPlayerIndex;
        this.currentPlayerIndex %= this.allRobots.Length;
      }
    }
    if (this.currentCameraState != CountdownCameraController.CameraStates.OverheadZoomout)
      return;
    if (this.stateFinished)
    {
      this.stateFinished = false;
      this._camera.position = this.currentFocusedBody.localToWorldMatrix.MultiplyPoint(this.OVERHEAD_ZOOMOUT_START_POS);
      this._camera.LookAt(this.currentFocusedBody.position + this.OVERHEAD_ZOOMOUT_LOOK_OFFSET);
    }
    this._camera.Translate(Time.deltaTime * this.OVERHEAD_ZOOMOUT_SPEED * this._camera.forward * -1f);
    if ((double) this.stateElapsedTime < 1.0)
      return;
    this.NextState();
    this.Stop();
  }

  public void Stop()
  {
    this._camera.position = this.savedStartingLocation;
    this._camera.rotation = this.savedStartingRotation;
    this.SetState(0);
    GLOBALS.CAMERA_COUNTDOWN_CONTROL = false;
  }

  public void StartCountdown_Internal()
  {
    this.allRobots = Object.FindObjectsOfType<RobotInterface3D>();
    if (this.allRobots.Length == 0)
      this.allRobots = (RobotInterface3D[]) null;
    this.currentPlayerIndex = Random.Range(0, this.allRobots.Length);
    this.SetState(2);
    this.savedStartingLocation = this._camera.position;
    this.savedStartingRotation = this._camera.rotation;
    GLOBALS.CAMERA_COUNTDOWN_CONTROL = true;
  }

  public static void StartCountdown() => CountdownCameraController.instance.StartCountdown_Internal();

  private enum CameraStates
  {
    DoingNothing,
    OverheadLook,
    FieldAnimation1,
    OrbitAroundPlayer,
    SlideByBumper,
    OverheadZoomout,
  }
}
