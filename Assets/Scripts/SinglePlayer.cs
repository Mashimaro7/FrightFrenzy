// Decompiled with JetBrains decompiler
// Type: SinglePlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SinglePlayer : MonoBehaviour
{
  private bool DEBUG;
  public Scorekeeper scorer;
  public string GAME = "Infinite Recharge";
  private SortedDictionary<int, GameObject> allFieldElements = new SortedDictionary<int, GameObject>();
  private static bool field_load;
  private static bool elements_load;
  private static bool scorer_load;
  private static bool gui_load;
  private static bool configuration_done;
  public Transform scorer_overlays;
  private GameSettings ourgamesettings;
  private float reset_release;
  private GameObject debugLog;
  private int error_count;
  private bool second_load;
  private bool key_space_last;
  private bool gamepad1_restart_old;
  private bool gamepad1_stop_old;
  private string RobotPosition = "Red Left";
  private GameObject player_camera;
  private GameObject vr_camera;
  private Vector3 vr_starting_pos;
  private Quaternion vr_starting_rot;
  private Transform robot_ref;
  private Transform camera_ref;
  private GameObject mycameratracker;
  private GameObject robotobject;
  private RobotInterface3D robotRI3D;
  public List<GameObject> enemyrobots = new List<GameObject>();
  private GameObject redtextobj;
  private GameObject bluetextobj;
  private GameObject field_redscore;
  private GameObject field_bluescore;
  private GameObject clean_run;
  private GameObject clean_run_details;
  private string old_overlay_string = "";

  private void OnEnable()
  {
    SinglePlayer.field_load = false;
    SinglePlayer.gui_load = false;
    SinglePlayer.elements_load = false;
    SinglePlayer.scorer_load = false;
    SinglePlayer.configuration_done = false;
    this.GAME = GLOBALS.GAME;
    GLOBALS.SINGLEPLAYER_MODE = true;
    GLOBALS.topsingleplayer = this;
    Physics.autoSimulation = true;
    GLOBALS.game_option = 1;
    SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.OnLevelFinishedLoading);
    SceneManager.LoadScene("Scenes/SinglePlayer_gui", LoadSceneMode.Additive);
    SceneManager.LoadScene("Scenes/" + this.GAME + "/field", LoadSceneMode.Additive);
    SceneManager.LoadScene("Scenes/" + this.GAME + "/fieldElements", LoadSceneMode.Additive);
  }

  private void RestartLevel()
  {
    this.scorer.Restart();
    if ((bool) (UnityEngine.Object) this.robotRI3D)
      this.robotRI3D.deleted = true;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.robotobject);
    foreach (UnityEngine.Object enemyrobot in this.enemyrobots)
      UnityEngine.Object.Destroy(enemyrobot);
    this.enemyrobots.Clear();
    this.SetCamera();
    this.SpawnPlayer(" ");
    if (this.DEBUG)
      this.SpawnEnemy("Player 3", "Blue Right", "Mecanum");
    this.scorer.ScorerInit();
    foreach (int key in this.allFieldElements.Keys)
    {
      gameElement component = this.allFieldElements[key].GetComponent<gameElement>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.ResetPosition(GLOBALS.game_option);
    }
    this.scorer.OnTimerReset();
    this.scorer.clean_run = true;
    this.scorer.OnTimerClick();
    if (!(bool) (UnityEngine.Object) this.clean_run)
      return;
    this.clean_run.GetComponent<Text>().text = "CLEAN";
    this.clean_run_details.GetComponent<Text>().text = "";
    this.scorer.cleancode = "";
  }

  public void FlagRequest(Dropdown menu)
  {
    if (menu.value > 0)
      GLOBALS.game_option = menu.value;
    this.RestartLevel();
    menu.value = 0;
    menu.itemText.text = menu.options[0].text;
  }

  private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
  {
    if (scene.name == "fieldElements")
      SinglePlayer.elements_load = true;
    if (scene.name == "field")
    {
      SinglePlayer.field_load = true;
      SceneManager.SetActiveScene(scene);
    }
    if (scene.name == "Scoring")
      SinglePlayer.scorer_load = true;
    if (scene.name == "SinglePlayer_gui")
      SinglePlayer.gui_load = true;
    if (!SinglePlayer.elements_load || !SinglePlayer.field_load || !SinglePlayer.scorer_load || !SinglePlayer.gui_load || SinglePlayer.configuration_done)
      return;
    this.debugLog = GameObject.Find("DebugLogText");
    this.allFieldElements.Clear();
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("GameElement"))
    {
      gameElement component = gameObject.GetComponent<gameElement>();
      if (this.allFieldElements.ContainsKey(component.id))
        Debug.Log((object) ("Field element " + (object) component.id + " is not unique id."));
      else
        this.allFieldElements.Add(component.id, gameObject);
      if ((bool) (UnityEngine.Object) gameObject.GetComponent<Rigidbody>())
        gameObject.GetComponent<Rigidbody>().sleepThreshold = 0.0f;
    }
    this.ConfigureElements();
    this.scorer = GameObject.Find("Scorekeeper").GetComponent<Scorekeeper>();
    this.scorer_overlays = GameObject.Find("ScorerOverlays").transform;
    this.SpawnPlayer(" ");
    this.SetCamera();
    SinglePlayer.configuration_done = true;
    foreach (Component component in UnityEngine.Resources.FindObjectsOfTypeAll<Camera>())
      MyUtils.SetCameraQualityLevel(component.gameObject);
    MyUtils.QualityLevel_DisableObjects();
    this.ourgamesettings = UnityEngine.Object.FindObjectOfType<GameSettings>();
    if ((bool) (UnityEngine.Object) this.ourgamesettings)
      this.ourgamesettings.Init();
    this.RestartLevel();
    this.scorer.clean_run = false;
  }

  private void ConfigureElements()
  {
    foreach (GameObject gameObject in this.allFieldElements.Values)
    {
      interpolation component = gameObject.GetComponent<interpolation>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.enabled = false;
    }
  }

  private void correctFieldElements()
  {
    foreach (GameObject currobj in this.allFieldElements.Values)
      this.scorer.CorrectFieldElement(currobj);
  }

  private void CorrectPlayers()
  {
    if (this.scorer.IsTransformOffField(this.robotRI3D.rb_body.transform))
    {
      if ((bool) (UnityEngine.Object) this.robotRI3D)
        this.robotRI3D.deleted = true;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.robotobject);
      this.SpawnPlayer(" ");
    }
    if (this.robotRI3D.GetNeedsReset())
    {
      this.reset_release = Time.time + this.robotRI3D.GetResetDuration();
      if ((bool) (UnityEngine.Object) this.robotRI3D)
        this.robotRI3D.deleted = true;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.robotobject);
      this.SpawnPlayer(" ");
    }
    if ((double) this.reset_release > (double) Time.time)
    {
      this.robotRI3D.HoldRobot();
    }
    else
    {
      if ((double) this.reset_release <= 0.0)
        return;
      this.robotRI3D.HoldRobot(false);
      this.reset_release = 0.0f;
    }
  }

  private void OnDisable()
  {
    SceneManager.sceneLoaded -= new UnityAction<Scene, LoadSceneMode>(this.OnLevelFinishedLoading);
    GLOBALS.SINGLEPLAYER_MODE = false;
    GLOBALS.topsingleplayer = (SinglePlayer) null;
  }

  public void ShowError(string message)
  {
    Text component = this.debugLog.GetComponent<Text>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    if (component.text.Contains("\n"))
    {
      string[] strArray = component.text.Split('\n');
      if (strArray.Length > 6)
      {
        component.text = "";
        for (int index = strArray.Length - 7; index < strArray.Length; ++index)
          component.text = component.text + strArray[index] + "\n";
      }
    }
    component.text = component.text + (object) this.error_count + ":" + message + "\n";
    component.enabled = true;
    ++this.error_count;
  }

  public void ShowMessage(string message) => this.ShowError(message);

  public void ClearError()
  {
    Text component = this.debugLog.GetComponent<Text>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    component.text = "";
    component.enabled = false;
  }

  private void Start()
  {
  }

  private void Update()
  {
    if (!this.second_load && SinglePlayer.gui_load)
    {
      this.second_load = true;
      SceneManager.LoadScene("Scenes/" + GLOBALS.GAME + "/Scoring", LoadSceneMode.Additive);
    }
    if (!SinglePlayer.configuration_done)
      return;
    this.InputControlUpdate();
    this.correctFieldElements();
    this.CorrectPlayers();
    this.UpdateScore();
    this.DoKeyboardStuff();
    if (this.DEBUG)
    {
      this.ClearError();
      this.ShowError("Vel = " + this.robotRI3D.maxvel.ToString("#.00") + " Accel = " + this.robotRI3D.lastacc[4].ToString("#.00") + "," + this.robotRI3D.lastacc[3].ToString("#.00") + "," + this.robotRI3D.lastacc[2].ToString("#.00"));
    }
    this.ApplyOverlays();
  }

  private string GetMD5() => MyUtils.EncryptMD5("RED=" + (object) this.scorer.GetRedScore() + "BLUE=" + (object) this.scorer.GetBlueScore() + "POS=" + this.RobotPosition + "SET=" + ((bool) (UnityEngine.Object) this.ourgamesettings ? (object) this.ourgamesettings.GetString() : (object) "None"));

  private bool VerifyMD5(int red_score, int blue_score, string position, string MD5string) => MyUtils.IsMD5Match("RED=" + (object) red_score + "BLUE=" + (object) blue_score + "POS=" + position + "SET=" + this.ourgamesettings.GetString(), MD5string);

  private void LateUpdate() => this.UpdateTrackingCamera();

  private void DoKeyboardStuff()
  {
    int num = Input.GetKey(KeyCode.Space) || GLOBALS.JoystickMap["Jcontrolls_camera"].GetButton() ? (!this.key_space_last ? 1 : 0) : 0;
    this.key_space_last = Input.GetKey(KeyCode.Space) || GLOBALS.JoystickMap["Jcontrolls_camera"].GetButton();
    if (num == 0)
      return;
    GLOBALS.camera_follows = !GLOBALS.camera_follows;
    if ((bool) (UnityEngine.Object) this.scorer)
      this.scorer.OnCameraViewChanged();
    this.DoCameraViewChanged();
  }

  public void DoCameraViewChanged()
  {
    if (!GLOBALS.camera_follows && (UnityEngine.Object) SinglePlayer.main_camera != (UnityEngine.Object) null && (UnityEngine.Object) this.player_camera != (UnityEngine.Object) null)
    {
      SinglePlayer.main_camera.transform.position = this.player_camera.transform.position;
      SinglePlayer.main_camera.transform.rotation = this.player_camera.transform.rotation;
      if ((bool) (UnityEngine.Object) this.vr_camera)
      {
        this.vr_camera.transform.position = this.vr_starting_pos;
        this.vr_camera.transform.rotation = this.vr_starting_rot;
      }
      SinglePlayer.main_camera.transform.position /= GLOBALS.worldscale / 2f;
    }
    else
    {
      if (!((UnityEngine.Object) this.robot_ref != (UnityEngine.Object) null))
        return;
      this.robot_ref = (Transform) null;
      this.mycameratracker.transform.parent = (Transform) null;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.mycameratracker);
      this.mycameratracker = (GameObject) null;
    }
  }

  private void InputControlUpdate()
  {
    if ((UnityEngine.Object) this.robotobject == (UnityEngine.Object) null)
      return;
    RobotInterface3D component = this.robotobject.GetComponent<RobotInterface3D>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    component.updateGamepadVars();
    if (component.gamepad1_restart && !this.gamepad1_restart_old)
      this.RestartLevel();
    this.gamepad1_restart_old = component.gamepad1_restart;
    if (component.gamepad1_stop && !this.gamepad1_stop_old)
      this.scorer.SetTimerState(Scorekeeper.TimerState.FINISHED);
    this.gamepad1_stop_old = component.gamepad1_stop;
  }

  public void SetPosition(string pos)
  {
    this.RobotPosition = pos;
    this.RestartLevel();
  }

  public static GameObject main_camera { get; private set; }

  public bool SetCamera()
  {
    this.player_camera = GameObject.Find(this.RobotPosition + " Cam");
    if ((UnityEngine.Object) this.player_camera == (UnityEngine.Object) null)
      this.player_camera = GameObject.Find("Spectator Cam");
    if (!(bool) (UnityEngine.Object) SinglePlayer.main_camera)
      SinglePlayer.main_camera = GameObject.Find("MainCamera");
    this.vr_camera = GameObject.Find("OVRCamera");
    GameObject gameObject = GameObject.Find("OVRCameraScaling");
    if ((UnityEngine.Object) SinglePlayer.main_camera != (UnityEngine.Object) null && (UnityEngine.Object) this.player_camera != (UnityEngine.Object) null)
    {
      cameraOrbit component = SinglePlayer.main_camera.GetComponent<cameraOrbit>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.enabled = false;
      SinglePlayer.main_camera.transform.position = this.player_camera.transform.position;
      SinglePlayer.main_camera.transform.rotation = this.player_camera.transform.rotation;
      SinglePlayer.main_camera.transform.position /= GLOBALS.worldscale / 2f;
      float num = 60f * Mathf.Pow(2f / GLOBALS.worldscale, -1.15f);
      SinglePlayer.main_camera.GetComponent<Camera>().fieldOfView = num;
    }
    if ((UnityEngine.Object) this.vr_camera != (UnityEngine.Object) null && (UnityEngine.Object) this.player_camera != (UnityEngine.Object) null)
    {
      this.vr_camera.transform.position = this.player_camera.transform.position with
      {
        y = 0.0f
      };
      Quaternion rotation = this.player_camera.transform.rotation;
      Vector3 eulerAngles = rotation.eulerAngles with
      {
        x = 0.0f,
        z = 0.0f
      };
      rotation.eulerAngles = eulerAngles;
      this.vr_camera.transform.rotation = rotation;
      this.vr_camera.transform.position *= GLOBALS.worldscale / 2f;
      gameObject.transform.localScale *= 2f / GLOBALS.worldscale;
      this.vr_starting_pos = this.vr_camera.transform.position;
      this.vr_starting_rot = this.vr_camera.transform.rotation;
    }
    return true;
  }

  private void UpdateTrackingCamera()
  {
    if (!GLOBALS.camera_follows || (UnityEngine.Object) SinglePlayer.main_camera == (UnityEngine.Object) null || GLOBALS.CAMERA_COUNTDOWN_CONTROL)
      return;
    Transform parent = this.robotobject.transform.Find("Body");
    if ((UnityEngine.Object) parent == (UnityEngine.Object) null)
      return;
    if ((UnityEngine.Object) this.robot_ref == (UnityEngine.Object) null)
    {
      this.robot_ref = GameObject.Find("robot_ref").transform;
      this.camera_ref = GameObject.Find("camera_ref").transform;
      this.mycameratracker = new GameObject();
      this.mycameratracker.transform.position = (this.camera_ref.position - this.robot_ref.position) * 2f / GLOBALS.worldscale;
      Quaternion rotation1 = this.mycameratracker.transform.rotation;
      ref Quaternion local = ref rotation1;
      Quaternion rotation2 = this.camera_ref.rotation;
      Vector3 eulerAngles1 = rotation2.eulerAngles;
      rotation2 = this.robot_ref.rotation;
      Vector3 eulerAngles2 = rotation2.eulerAngles;
      Vector3 vector3 = eulerAngles1 - eulerAngles2;
      local.eulerAngles = vector3;
      Vector3 eulerAngles3 = rotation1.eulerAngles;
      eulerAngles3.x -= (float) (((double) GLOBALS.worldscale - 2.0) * 5.0);
      rotation1.eulerAngles = eulerAngles3;
      this.mycameratracker.transform.rotation = rotation1;
      this.mycameratracker.transform.RotateAround(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1f, 0.0f), 90f);
      Quaternion rotation3 = this.mycameratracker.transform.rotation;
      Vector3 localPosition = this.mycameratracker.transform.localPosition;
      Vector3 localScale = this.mycameratracker.transform.localScale;
      Quaternion localRotation = this.mycameratracker.transform.localRotation;
      if ((UnityEngine.Object) parent == (UnityEngine.Object) null)
        return;
      this.mycameratracker.transform.SetParent(parent, false);
      this.mycameratracker.transform.localPosition = localPosition;
      this.mycameratracker.transform.localScale = localScale;
      this.mycameratracker.transform.localRotation = localRotation;
      SinglePlayer.main_camera.transform.rotation = localRotation;
      if ((bool) (UnityEngine.Object) this.vr_camera)
        this.vr_camera.transform.rotation = this.mycameratracker.transform.rotation;
    }
    Vector3 position = this.mycameratracker.transform.position with
    {
      y = this.camera_ref.position.y * (float) (1.0 + (2.0 - (double) GLOBALS.worldscale) * 0.219999998807907)
    };
    SinglePlayer.main_camera.transform.position = position;
    Vector3 eulerAngles = this.mycameratracker.transform.rotation.eulerAngles with
    {
      x = SinglePlayer.main_camera.transform.rotation.eulerAngles.x,
      z = SinglePlayer.main_camera.transform.rotation.eulerAngles.z
    };
    Quaternion rotation = SinglePlayer.main_camera.transform.rotation with
    {
      eulerAngles = eulerAngles
    };
    SinglePlayer.main_camera.transform.rotation = rotation;
    if (!(bool) (UnityEngine.Object) this.vr_camera)
      return;
    position.y = 0.0f;
    this.vr_camera.transform.position = position;
    this.vr_camera.transform.rotation = rotation;
  }

  private void SpawnPlayer(string playerName)
  {
    Transform transform1 = this.scorer.CorrectRobotPosition(this.RobotPosition, new List<string>());
    this.RobotPosition = transform1.name;
    Transform transform2 = transform1;
    try
    {
      string model = this.scorer.CorrectRobotChoice(GLOBALS.RobotModel);
      GLOBALS.RobotModel = model;
      this.robotobject = MyUtils.InstantiateRobot(model, transform2.position, transform2.rotation, GLOBALS.skins, GLOBALS.robotskins);
    }
    catch (Exception ex)
    {
      this.ShowError("Missing robot model for player " + playerName + ". Can't show player!");
      return;
    }
    this.robotRI3D = this.robotobject.GetComponent<RobotInterface3D>();
    if ((bool) (UnityEngine.Object) this.robotRI3D)
      this.robotRI3D.SetName(playerName);
    this.robotobject.GetComponent<RobotInterface3D>().SetUserParameters();
    this.robotobject.GetComponent<RobotInterface3D>().fieldcentric_rotation = (bool) (UnityEngine.Object) this.player_camera ? this.player_camera.transform.rotation : Quaternion.identity;
    RobotID robotId = this.robotobject.AddComponent<RobotID>();
    robotId.starting_pos = this.RobotPosition;
    robotId.id = 1;
    this.robotobject.GetComponent<RobotInterface3D>().Initialize();
    GLOBALS.client_names[robotId.id] = playerName;
    this.robotobject.GetComponent<RobotInterface3D>().SetColorFromPosition(this.RobotPosition);
    this.scorer.FieldChanged();
    this.robot_ref = (Transform) null;
    this.TurnOffInterpolationInObject(this.robotobject);
  }

  private void TurnOffInterpolationInObject(GameObject inobject)
  {
    foreach (Behaviour componentsInChild in inobject.GetComponentsInChildren<interpolation>())
      componentsInChild.enabled = false;
    interpolation component = inobject.transform.GetComponent<interpolation>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.enabled = false;
  }

  private GameObject SpawnEnemy(
    string playerName,
    string start_postion,
    string drivetrain)
  {
    GameObject gameObject1 = GameObject.Find(start_postion);
    Transform transform = this.transform;
    if ((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null)
      transform = gameObject1.transform;
    GameObject gameObject2 = MyUtils.InstantiateRobot(GLOBALS.RobotModel, transform.position, transform.rotation, GLOBALS.skins, GLOBALS.robotskins);
    gameObject2.GetComponent<RobotInterface3D>().SetKinematic();
    gameObject2.GetComponent<RobotInterface3D>().DriveTrain = drivetrain;
    gameObject2.GetComponent<RobotInterface3D>().max_speed = GLOBALS.speed;
    gameObject2.GetComponent<RobotInterface3D>().max_acceleration = GLOBALS.acceleration;
    gameObject2.GetComponent<RobotInterface3D>().total_weight = GLOBALS.weight;
    gameObject2.GetComponent<RobotInterface3D>().turn_scale = GLOBALS.turning_scaler;
    gameObject2.GetComponent<RobotInterface3D>().fieldcentric = GLOBALS.fieldcentric;
    gameObject2.GetComponent<RobotInterface3D>().activebreaking = GLOBALS.activebreaking;
    gameObject2.GetComponent<RobotInterface3D>().tankcontrol = GLOBALS.tankcontrol;
    gameObject2.GetComponentInChildren<TextMesh>().text = playerName;
    gameObject2.AddComponent<RobotID>().starting_pos = "Blue Left";
    this.scorer.FieldChanged();
    this.enemyrobots.Add(gameObject2);
    return gameObject2;
  }

  public void UpdateScore()
  {
    if ((UnityEngine.Object) this.scorer == (UnityEngine.Object) null)
      return;
    if ((UnityEngine.Object) this.redtextobj == (UnityEngine.Object) null)
      this.redtextobj = GameObject.Find("REDSCORE");
    if ((UnityEngine.Object) this.field_redscore == (UnityEngine.Object) null)
      this.field_redscore = GameObject.Find("FIELD_RED");
    if ((UnityEngine.Object) this.redtextobj == (UnityEngine.Object) null)
      return;
    this.redtextobj.GetComponent<Text>().text = this.scorer.GetRedScore().ToString();
    int num;
    if ((bool) (UnityEngine.Object) this.field_redscore)
    {
      TextMesh component = this.field_redscore.GetComponent<TextMesh>();
      num = this.scorer.GetRedScore();
      string str = num.ToString();
      component.text = str;
    }
    if ((UnityEngine.Object) this.bluetextobj == (UnityEngine.Object) null)
      this.bluetextobj = GameObject.Find("BLUESCORE");
    if ((UnityEngine.Object) this.field_bluescore == (UnityEngine.Object) null)
      this.field_bluescore = GameObject.Find("FIELD_BLUE");
    if ((UnityEngine.Object) this.bluetextobj == (UnityEngine.Object) null)
      return;
    Text component1 = this.bluetextobj.GetComponent<Text>();
    num = this.scorer.GetBlueScore();
    string str1 = num.ToString();
    component1.text = str1;
    if ((bool) (UnityEngine.Object) this.field_bluescore)
    {
      TextMesh component2 = this.field_bluescore.GetComponent<TextMesh>();
      num = this.scorer.GetBlueScore();
      string str2 = num.ToString();
      component2.text = str2;
    }
    if ((UnityEngine.Object) this.clean_run == (UnityEngine.Object) null)
    {
      this.clean_run = GameObject.Find("CLEANRUN");
      this.clean_run_details = GameObject.Find("CLEANRUNDETAILS");
    }
    if ((UnityEngine.Object) this.clean_run == (UnityEngine.Object) null)
      return;
    if (this.scorer.clean_run)
    {
      if (this.scorer.IsTimerFinished() && this.scorer.cleancode.Length < 1)
      {
        this.clean_run.SetActive(true);
        string indata = GLOBALS.VERSION + "," + (object) GLOBALS.GAME_INDEX + "," + DateTime.Now.ToString() + "," + (object) this.scorer.GetRedScore() + "," + (object) this.scorer.GetBlueScore() + "," + this.RobotPosition + "," + GLOBALS.RobotModel + "," + (object) GLOBALS.game_option + "," + ((bool) (UnityEngine.Object) this.ourgamesettings ? (object) this.ourgamesettings.GetCleanString() : (object) "None") + "," + this.scorer.GetTimerText();
        string indata_raw = MyUtils.EncryptAES(indata);
        this.scorer.cleancode = indata_raw;
        this.clean_run_details.GetComponent<Text>().text = indata;
        GUIUtility.systemCopyBuffer = indata_raw;
        this.clean_run.GetComponent<Text>().text = "Code copied to clipboard. Ends with: " + indata_raw.Substring(indata_raw.Length - 23, 23);
        string str3 = MyUtils.DecryptAES(indata_raw);
        this.clean_run_details.GetComponent<Text>().text = str3;
      }
      else
      {
        if (this.clean_run.activeSelf)
          return;
        this.clean_run.SetActive(true);
        this.clean_run.GetComponent<Text>().text = "CLEAN";
        this.clean_run_details.GetComponent<Text>().text = "";
        this.scorer.cleancode = "";
      }
    }
    else
      this.clean_run.SetActive(false);
  }

  public void ApplyOverlays()
  {
    if ((UnityEngine.Object) SinglePlayer.main_camera == (UnityEngine.Object) null)
      return;
    string overlaysString = this.scorer.GetOverlaysString(1);
    if (this.old_overlay_string.Equals(overlaysString))
      return;
    foreach (Component component in this.scorer_overlays.transform)
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
    this.scorer.GetOverlays(1, this.scorer_overlays);
    this.old_overlay_string = overlaysString;
  }
}
