// Decompiled with JetBrains decompiler
// Type: ClientLow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClientLow : MonoBehaviour
{
  [NonSerialized]
  public static ClientLow instance;
  private bool DEBUG;
  public bool VR_ENABLED;
  public Scorekeeper scorer;
  private Dictionary<string, int> netmonitor = new Dictionary<string, int>();
  private string netmonitor_header;
  private GameObject top_canvas;
  private ApplicationManager application_manager;
  private bool found_position;
  private bool holding_mode;
  public int framecount;
  public int FPS;
  private GameObject big_message;
  private Dictionary<int, GameObject> allFieldElements = new Dictionary<int, GameObject>();
  private GameObject redtextobj;
  private GameObject bluetextobj;
  private GameObject field_redscore;
  private GameObject field_bluescore;
  private GameObject messageLog;
  private List<ClientLow.LogLine> allmessages = new List<ClientLow.LogLine>();
  private Thread thread_incoming;
  private bool first_time_game_settings = true;
  private static bool killme = false;
  private static bool field_load = false;
  private static bool elements_load = false;
  private static bool scorer_load = false;
  public static bool configuration_done = false;
  private static bool gui_load = false;
  private long lastSendingTime;
  private bool second_load;
  private long lastFrameCount;
  public int playback_index;
  public RobotInterface3D highlited_robot;
  public RobotInterface3D selected_robot;
  private GameObject popup_menu;
  private string connectString = "";
  private long time_last_packet = -1;
  private bool interpolation_on;
  public ClientLow.ConnectionStates connection_state;
  private long time_last_sent = -1;
  private long time_started = -1;
  private long time_last_count_check = -1;
  public string myStartfield = "";
  public bool playbackmode;
  private string oldPlayerName;
  private int spectator_curr_camera_player;
  private bool key_space_last;
  private GameObject player_camera;
  private GameObject spectator_cam1;
  private GameObject spectator_cam2;
  public GameObject main_camera;
  private GameObject vr_camera;
  private Vector3 vr_starting_pos;
  private Quaternion vr_starting_rot;
  private Transform robot_ref;
  private Transform camera_ref;
  private GameObject mycameratracker;
  private Dropdown flagrequest_menu;
  public GameObject name_parent;
  public Transform name_rl;
  public Transform name_rc;
  public Transform name_rr;
  public Transform name_bl;
  public Transform name_bc;
  public Transform name_br;
  public Transform scorer_overlays;
  public int overlay_mode;
  private bool old_keys_combo_overlay;
  public int overlay_details;
  private bool old_keys_combo_overlay2;
  private UnityEngine.UI.Text detailsText;
  private string old_overlay_string = "";
  public Vector3 screen_pos;
  private Dictionary<string, string> clientFlags = new Dictionary<string, string>();
  public static string serverIP = "127.0.0.1";
  public static int serverPORT = GLOBALS.UDP_PORT;
  public static string ourPlayerName = "newbie";
  private List<byte[]> allReceivedData = new List<byte[]>();
  public Dictionary<string, string> serverFlags = new Dictionary<string, string>();
  private System.Threading.Semaphore allReceivedDataSemaphore = new System.Threading.Semaphore(1, 1);
  public Dictionary<int, ClientLow.Player> players = new Dictionary<int, ClientLow.Player>();
  public bool players_changed;
  public int latest_message_id;
  public int last_checked_message_id;
  public int received_msg_count;
  public long last_update_time;
  private int id_current_players = -1;
  private int id_server_error = -1;
  private int client_confirmed_this_msg_id;
  public bool IsAdmin;
  private GameObject myPlayer;
  private RobotInterface3D myRobot_working;
  private RobotInterface3D myRobot_saved;
  public string myPosition = "";
  private GameObject spec_myPlayer;
  private Robot_Spectator spec_myRobot;
  private bool use_spec_myRobot;
  private GameSettings ourgamesettings;
  private IEnumerator autosave_enumerator;
  public GameObject Tournament_top;
  public GameObject Tournament_msg;
  public GameObject Tournament_ready;
  private GameObject ChampsObject;
  private bool no_champs_exists;
  private System.Threading.Semaphore udpSemaphore = new System.Threading.Semaphore(1, 1);
  private int datareceivederrors;
  private UdpClient m_udpClient;
  private IPEndPoint receiverEP;
  private int ourClientId = -1;
  private int curr_tracked_packet = 1;
  private int server_confirmed_this_msg_id = -1;
  private Dictionary<int, string> client_sent_packets = new Dictionary<int, string>();

  private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
  {
    if (scene.name == "fieldElements")
      ClientLow.elements_load = true;
    if (scene.name == "field")
    {
      ClientLow.field_load = true;
      SceneManager.SetActiveScene(scene);
    }
    if (scene.name == "Scoring")
      ClientLow.scorer_load = true;
    if (scene.name == "MultiPlayer_gui")
      ClientLow.gui_load = true;
    if (!ClientLow.elements_load || !ClientLow.field_load || !ClientLow.gui_load || !ClientLow.scorer_load || ClientLow.configuration_done)
      return;
    this.messageLog = GameObject.Find("MessageLogText");
    GameObject gameObject1 = GameObject.Find("DetailsOverlay");
    if ((bool) (UnityEngine.Object) gameObject1)
      this.detailsText = gameObject1.GetComponent<UnityEngine.UI.Text>();
    this.Tournament_msg = GameObject.Find("TournamentMSG");
    this.Tournament_top = GameObject.Find("Tournament");
    this.Tournament_ready = this.Tournament_top.transform.Find("Ready").gameObject;
    this.Tournament_top.SetActive(false);
    this.allFieldElements.Clear();
    foreach (GameObject gameObject2 in GameObject.FindGameObjectsWithTag("GameElement"))
    {
      gameElement component = gameObject2.GetComponent<gameElement>();
      if (this.allFieldElements.ContainsKey(component.id))
        Debug.Log((object) ("Field element " + (object) component.id + " is not unique id."));
      else
        this.allFieldElements.Add(component.id, gameObject2);
    }
    this.ConfigureElements();
    this.scorer = GameObject.Find("Scorekeeper").GetComponent<Scorekeeper>();
    this.scorer_overlays = GameObject.Find("ScorerOverlays").transform;
    this.top_canvas = GameObject.Find("Canvas");
    this.big_message = GameObject.Find("BIGMESSAGE");
    if ((bool) (UnityEngine.Object) this.big_message)
      this.big_message.SetActive(false);
    this.scorer.shadowmode = true;
    this.scorer.ScorerReset();
    ClientLow.configuration_done = true;
    this.PrepOverlays();
    foreach (Component component in UnityEngine.Resources.FindObjectsOfTypeAll<Camera>())
      MyUtils.SetCameraQualityLevel(component.gameObject);
    MyUtils.QualityLevel_DisableObjects();
    this.ourgamesettings = UnityEngine.Object.FindObjectOfType<GameSettings>();
    if ((bool) (UnityEngine.Object) this.ourgamesettings)
      this.ourgamesettings.Init();
    this.application_manager = UnityEngine.Object.FindObjectOfType<ApplicationManager>();
  }

  private void ConfigureElements()
  {
    foreach (GameObject gameObject in this.allFieldElements.Values)
    {
      BoxCollider component1 = gameObject.GetComponent<BoxCollider>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        component1.enabled = false;
      Joint component2 = gameObject.GetComponent<Joint>();
      if ((bool) (UnityEngine.Object) component2)
        UnityEngine.Object.Destroy((UnityEngine.Object) component2);
      Rigidbody component3 = gameObject.GetComponent<Rigidbody>();
      if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
      {
        component3.detectCollisions = false;
        component3.isKinematic = true;
        UnityEngine.Object.Destroy((UnityEngine.Object) component3);
      }
    }
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("FieldStructure"))
    {
      BoxCollider component4 = gameObject.GetComponent<BoxCollider>();
      Rigidbody component5 = gameObject.GetComponent<Rigidbody>();
      if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
        component4.enabled = false;
      if ((UnityEngine.Object) component5 != (UnityEngine.Object) null)
      {
        component5.detectCollisions = false;
        component5.isKinematic = true;
        UnityEngine.Object.Destroy((UnityEngine.Object) component5);
      }
    }
  }

  private void TurnOnInterpolation()
  {
    foreach (GameObject gameObject in this.allFieldElements.Values)
    {
      interpolation interpolation = gameObject.GetComponent<interpolation>();
      if ((UnityEngine.Object) interpolation == (UnityEngine.Object) null)
        interpolation = gameObject.AddComponent<interpolation>();
      interpolation.enabled = true;
    }
    this.interpolation_on = true;
  }

  private void TurnOnInterpolationInObject(GameObject inobject)
  {
    for (int index = 0; index < inobject.transform.childCount; ++index)
    {
      interpolation interpolation = inobject.transform.GetChild(index).GetComponent<interpolation>();
      if ((UnityEngine.Object) interpolation == (UnityEngine.Object) null)
        interpolation = inobject.transform.GetChild(index).gameObject.AddComponent<interpolation>();
      interpolation.enabled = true;
      Rigidbody component = inobject.transform.GetChild(index).GetComponent<Rigidbody>();
      if ((bool) (UnityEngine.Object) component)
      {
        component.detectCollisions = false;
        component.isKinematic = true;
      }
    }
    interpolation interpolation1 = inobject.GetComponent<interpolation>();
    if ((UnityEngine.Object) interpolation1 == (UnityEngine.Object) null)
      interpolation1 = inobject.AddComponent<interpolation>();
    interpolation1.enabled = true;
    Rigidbody component1 = inobject.GetComponent<Rigidbody>();
    if (!(bool) (UnityEngine.Object) component1)
      return;
    component1.detectCollisions = false;
    component1.isKinematic = true;
  }

  public void UpdateScore()
  {
    if ((UnityEngine.Object) this.redtextobj == (UnityEngine.Object) null)
    {
      this.redtextobj = GameObject.Find("REDSCORE");
      this.field_redscore = GameObject.Find("FIELD_RED");
      if ((UnityEngine.Object) this.redtextobj == (UnityEngine.Object) null)
        return;
    }
    if ((UnityEngine.Object) this.bluetextobj == (UnityEngine.Object) null)
    {
      this.bluetextobj = GameObject.Find("BLUESCORE");
      this.field_bluescore = GameObject.Find("FIELD_BLUE");
      if ((UnityEngine.Object) this.bluetextobj == (UnityEngine.Object) null)
        return;
    }
    string str1 = "";
    string str2 = "";
    if (!this.serverFlags.TryGetValue("REDSCORE", out str1) || !this.serverFlags.TryGetValue("BLUESCORE", out str2))
      return;
    this.redtextobj.GetComponent<UnityEngine.UI.Text>().text = str1;
    this.bluetextobj.GetComponent<UnityEngine.UI.Text>().text = str2;
    if ((bool) (UnityEngine.Object) this.field_redscore)
      this.field_redscore.GetComponent<TextMesh>().text = str1;
    if ((bool) (UnityEngine.Object) this.field_bluescore)
      this.field_bluescore.GetComponent<TextMesh>().text = str2;
    this.scorer.ReceiveServerData(this.serverFlags);
  }

  public void ShowMessage(string message)
  {
    if (!(bool) (UnityEngine.Object) this.messageLog)
      return;
    ClientLow.LogLine logLine = new ClientLow.LogLine();
    logLine.TextLine = UnityEngine.Object.Instantiate<GameObject>(this.messageLog, this.messageLog.transform.parent, false);
    logLine.time_of_message = ClientLow.GetTimeMillis();
    UnityEngine.UI.Text component = logLine.TextLine.GetComponent<UnityEngine.UI.Text>();
    component.text = message;
    component.enabled = true;
    Canvas.ForceUpdateCanvases();
    float num = (float) component.cachedTextGenerator.lineCount * 25f;
    foreach (ClientLow.LogLine allmessage in this.allmessages)
    {
      Vector2 anchoredPosition = allmessage.TextLine.GetComponent<RectTransform>().anchoredPosition;
      anchoredPosition.y += num;
      allmessage.TextLine.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
    }
    this.allmessages.Add(logLine);
  }

  private void UpdateMessage()
  {
    for (int index = this.allmessages.Count - 1; index >= 0; --index)
    {
      ClientLow.LogLine allmessage = this.allmessages[index];
      long num = ClientLow.GetTimeMillis() - allmessage.time_of_message;
      if (num > GLOBALS.MESSAGE_DISPLAY_PERIOD && this.allmessages.Count - index > GLOBALS.MESSAGES_TO_KEEP)
      {
        this.allmessages.RemoveAt(index);
        UnityEngine.Object.Destroy((UnityEngine.Object) allmessage.TextLine);
      }
      else
      {
        UnityEngine.UI.Text component = allmessage.TextLine.GetComponent<UnityEngine.UI.Text>();
        if ((UnityEngine.Object) component == (UnityEngine.Object) null)
          break;
        float a = 1f;
        if ((double) num > 0.9 * (double) GLOBALS.MESSAGE_DISPLAY_PERIOD)
        {
          a = (float) (GLOBALS.MESSAGE_DISPLAY_PERIOD - num) / (0.1f * (float) GLOBALS.MESSAGE_DISPLAY_PERIOD);
          if ((double) a < 0.0)
          {
            a = 0.0f;
            component.enabled = false;
          }
          else
            component.enabled = true;
        }
        Color color = new Color(component.color.r, component.color.g, component.color.b, a);
        component.color = color;
      }
    }
  }

  public void ResetChatCounter()
  {
    for (int index = 0; index < this.allmessages.Count; ++index)
    {
      if (this.allmessages.Count - index <= GLOBALS.MESSAGES_TO_KEEP)
        this.allmessages[index].time_of_message = ClientLow.GetTimeMillis();
    }
  }

  private bool ClientStart()
  {
    MyUtils.LogMessageToFile("starting client udp...", false);
    if (!this.startUdp())
      return false;
    MyUtils.LogMessageToFile("OK, started udp.", false);
    switch (GLOBALS.UDP_ALGORITHM)
    {
      case 0:
        this.m_udpClient.BeginReceive(new AsyncCallback(this.DataReceived), (object) this.m_udpClient);
        break;
      case 1:
        this.thread_incoming = new Thread(new ThreadStart(this.DataReceive));
        this.thread_incoming.Start();
        if (this.thread_incoming.IsAlive)
        {
          MyUtils.LogMessageToFile("udp thread is alive.", false);
          break;
        }
        MyUtils.LogMessageToFile("udp thread is NOT alive.... yet...");
        break;
      default:
        int udpAlgorithm = GLOBALS.UDP_ALGORITHM;
        break;
    }
    ClientLow.instance = this;
    return true;
  }

  private void OnApplicationQuit() => ClientLow.killme = true;

  private void OnDisable()
  {
    ClientLow.killme = true;
    ClientLow.configuration_done = false;
    if (this.thread_incoming != null)
      this.thread_incoming.Abort();
    if (this.m_udpClient != null)
    {
      this.m_udpClient.Close();
      this.m_udpClient.Dispose();
      this.m_udpClient = (UdpClient) null;
    }
    MyUtils.CloseScorefiles();
    MyUtils.PB_ClearRecording();
    SceneManager.sceneLoaded -= new UnityAction<Scene, LoadSceneMode>(this.OnLevelFinishedLoading);
    GLOBALS.CLIENT_MODE = false;
    GLOBALS.topclient = (ClientLow) null;
    GLOBALS.FORCE_OLD_BHELP = false;
  }

  private void OnEnable()
  {
    GLOBALS.LOGS_PATH = (Application.isEditor ? "." : Application.persistentDataPath) + Path.DirectorySeparatorChar.ToString() + "logs";
    ClientLow.field_load = false;
    ClientLow.elements_load = false;
    ClientLow.scorer_load = false;
    ClientLow.configuration_done = false;
    ClientLow.gui_load = false;
    this.connection_state = ClientLow.ConnectionStates.NOTSTARTED;
    Physics.autoSimulation = false;
    SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.OnLevelFinishedLoading);
    SceneManager.LoadScene("Scenes/" + GLOBALS.GAME + "/fieldElements", LoadSceneMode.Additive);
    SceneManager.LoadScene("Scenes/MultiPlayer_gui", LoadSceneMode.Additive);
    SceneManager.LoadScene("Scenes/" + GLOBALS.GAME + "/field", LoadSceneMode.Additive);
    ClientLow.killme = false;
    GLOBALS.CLIENT_MODE = true;
    GLOBALS.topclient = this;
    GLOBALS.FORCE_OLD_BHELP = false;
  }

  private void Start()
  {
  }

  private void FixedUpdate()
  {
  }

  private void Update()
  {
    this.playback_index = MyUtils.playback_index;
    this.players_changed = false;
    ++this.framecount;
    if (MyUtils.GetTimeMillis() - this.lastFrameCount >= 250L)
    {
      this.FPS = (int) ((long) (this.framecount * 1000) / (MyUtils.GetTimeMillis() - this.lastFrameCount));
      this.lastFrameCount = MyUtils.GetTimeMillis();
      this.framecount = 0;
    }
    if (!this.playbackmode && GLOBALS.UDP_ALGORITHM == 2)
      this.DataReceiveSerial();
    int num = this.DEBUG ? 1 : 0;
    if (!this.second_load && ClientLow.gui_load)
    {
      this.second_load = true;
      SceneManager.LoadScene("Scenes/" + GLOBALS.GAME + "/Scoring", LoadSceneMode.Additive);
    }
    if (this.myPosition == "Spectator" || this.myPosition.Length < 1 || this.playbackmode)
      this.ProcessMouse();
    if (!this.playbackmode)
    {
      this.MonitorConnection();
      this.UpdatePacketLoss();
      if (this.connection_state == ClientLow.ConnectionStates.LOST && !ClientLow.killme && !GLOBALS.now_playing)
      {
        this.ShowMessage("LOST Connection to server!!!");
        MyUtils.LogMessageToFile("Lost connection to server...");
        this.found_position = false;
        this.connection_state = ClientLow.ConnectionStates.NOTSTARTED;
        this.ResetAllStates();
        return;
      }
      this.allReceivedDataSemaphore.WaitOne();
      try
      {
        if (ClientLow.killme)
          return;
        if (this.allReceivedData.Count > 0)
          this.onReceivedData(ref this.allReceivedData);
      }
      catch (Exception ex)
      {
        MyUtils.LogMessageToFile("Exception occured during update onReceivedData " + (object) ex);
      }
      finally
      {
        this.allReceivedData.Clear();
        this.allReceivedDataSemaphore.Release();
      }
      GLOBALS.time_after_data_received = MyUtils.GetTimeSinceStart();
    }
    if (GLOBALS.now_playing)
    {
      if (GLOBALS.now_paused)
        MyUtils.PB_UpdateDuringPause();
      this.ProcessPlayback();
    }
    long timeMillis = ClientLow.GetTimeMillis();
    if (timeMillis - this.lastSendingTime < GLOBALS.CLIENT_SEND_UPDATE_DELAY)
      return;
    this.lastSendingTime = timeMillis;
    this.UpdateScore();
    this.DoKeyboardStuff();
    MyUtils.DoScoringFiles(this.serverFlags);
    this.ProcessScoringFileCommands();
    this.ApplyOverlays();
    if (this.playbackmode && (this.use_spec_myRobot || (UnityEngine.Object) this.myRobot_working == (UnityEngine.Object) this.spec_myRobot))
      this.spec_myRobot.updateGamepadVars();
    this.UpdateMessage();
    if (this.connection_state != ClientLow.ConnectionStates.CONNECTED)
      return;
    this.SendMyInputs();
    this.SendFlags();
  }

  private void ProcessMouse()
  {
    if (!(bool) (UnityEngine.Object) this.main_camera)
      return;
    Ray ray = this.main_camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
    UnityEngine.RaycastHit raycastHit = new UnityEngine.RaycastHit();
    ref UnityEngine.RaycastHit local = ref raycastHit;
    if (Physics.Raycast(ray, out local, 1000f, 8388608, QueryTriggerInteraction.Collide))
    {
      Transform transform = raycastHit.transform;
      RobotInterface3D componentInParent = raycastHit.transform.GetComponentInParent<RobotInterface3D>();
      if ((bool) (UnityEngine.Object) componentInParent && (UnityEngine.Object) componentInParent != (UnityEngine.Object) this.highlited_robot)
      {
        if ((bool) (UnityEngine.Object) this.highlited_robot)
          this.highlited_robot.Highlite(false);
        this.highlited_robot = componentInParent;
        this.highlited_robot.Highlite(true);
      }
    }
    else if ((bool) (UnityEngine.Object) this.highlited_robot)
    {
      this.highlited_robot.Highlite(false);
      this.highlited_robot = (RobotInterface3D) null;
    }
    if (this.application_manager.IsPointerOverActive() || !Input.GetMouseButtonDown(0))
      return;
    if ((bool) (UnityEngine.Object) this.selected_robot)
      this.selected_robot.Select(false);
    this.selected_robot = this.highlited_robot;
    if ((bool) (UnityEngine.Object) this.selected_robot)
      this.selected_robot.Select(true);
    this.UpdateAdminPopupMenu();
  }

  private void UpdateAdminPopupMenu()
  {
    if (!(bool) (UnityEngine.Object) this.popup_menu)
    {
      this.popup_menu = MyUtils.FindGlobal("AdminPopUp");
      if (!(bool) (UnityEngine.Object) this.popup_menu)
        return;
    }
    if ((bool) (UnityEngine.Object) this.selected_robot)
      this.popup_menu.SetActive(true);
    else
      this.popup_menu.SetActive(false);
  }

  public void CB_Referee(string what_to_do)
  {
    if (!(bool) (UnityEngine.Object) this.selected_robot || this.selected_robot.deleted)
      return;
    if (!(what_to_do == "kick"))
    {
      if (!(what_to_do == "warn"))
      {
        if (!(what_to_do == "reset"))
        {
          if (!(what_to_do == "5s"))
          {
            if (!(what_to_do == "clear"))
              return;
            this.SendChat("/SERVER ROBOTCOUNTERRESET=" + (object) this.selected_robot.myRobotID.id);
          }
          else
            this.SendChat("/SERVER ROBOTCOUNTER=" + (object) this.selected_robot.myRobotID.id + "=5");
        }
        else
          this.SendChat("/SERVER RESET=" + (object) this.selected_robot.myRobotID.id);
      }
      else
        this.SendChat("/SERVER MESSAGE=" + (object) this.selected_robot.myRobotID.id + "=<size=35><color=red>*****</color> WARNING GIVEN <color=red>*****</color></size>");
    }
    else
      this.SendChat("/SERVER KICKID=" + (object) this.selected_robot.myRobotID.id);
  }

  private void ProcessScoringFileCommands()
  {
    List<string> scoringFilesCommand = MyUtils.GetScoringFilesCommand();
    if (scoringFilesCommand == null)
      return;
    foreach (string msg in scoringFilesCommand)
      this.SendChat(msg);
  }

  private void ProcessPlayback()
  {
    for (Saved_Data next = MyUtils.PB_GetNext(); next != null; next = MyUtils.PB_GetNext())
      this.ProcessData(next.data, next.timestamp, true);
  }

  public bool IsPlaybackDataEndOfMatch(Saved_Data currdata)
  {
    if (currdata.data.Length < 1 || currdata.data[0] != "FLG")
      return false;
    string str1 = currdata.data[1];
    char[] chArray1 = new char[1]{ '\u0013' };
    foreach (string str2 in str1.Split(chArray1))
    {
      char[] chArray2 = new char[1]{ '\u0012' };
      string[] strArray1 = str2.Split(chArray2);
      if (strArray1.Length >= 4 && int.Parse(strArray1[0]) == int.Parse(strArray1[1]))
      {
        int num1 = 2;
        while (num1 < strArray1.Length)
        {
          string[] strArray2 = strArray1;
          int index1 = num1;
          int num2 = index1 + 1;
          string str3 = strArray2[index1];
          string[] strArray3 = strArray1;
          int index2 = num2;
          num1 = index2 + 1;
          string str4 = strArray3[index2];
          if (str3 == "FIREWORKS")
            return true;
        }
      }
    }
    return false;
  }

  public bool IsPlaybackDataStartOfMatch(Saved_Data currdata)
  {
    if (currdata.data.Length < 1 || currdata.data[0] != "FLG")
      return false;
    string str1 = currdata.data[1];
    char[] chArray1 = new char[1]{ '\u0013' };
    foreach (string str2 in str1.Split(chArray1))
    {
      char[] chArray2 = new char[1]{ '\u0012' };
      string[] strArray1 = str2.Split(chArray2);
      if (strArray1.Length >= 4 && int.Parse(strArray1[0]) == int.Parse(strArray1[1]))
      {
        int num1 = 2;
        while (num1 < strArray1.Length)
        {
          string[] strArray2 = strArray1;
          int index1 = num1;
          int num2 = index1 + 1;
          string str3 = strArray2[index1];
          string[] strArray3 = strArray1;
          int index2 = num2;
          num1 = index2 + 1;
          string str4 = strArray3[index2];
          if (str3 == "COUNTDOWN")
            return true;
        }
      }
    }
    return false;
  }

  private void LateUpdate() => this.UpdateTrackingCamera();

  public static long GetTimeMillis() => DateTime.Now.Ticks / 10000L;

  private void MonitorConnection()
  {
    if (this.playbackmode)
      return;
    if (this.time_started == -1L)
      this.time_started = ClientLow.GetTimeMillis();
    if (this.connection_state == ClientLow.ConnectionStates.LOST || this.connection_state == ClientLow.ConnectionStates.NOTSTARTED)
      return;
    long timeMillis = ClientLow.GetTimeMillis();
    if (this.connection_state == ClientLow.ConnectionStates.CONNECTING)
    {
      if (this.time_last_packet > 0L)
        this.connection_state = ClientLow.ConnectionStates.CONNECTED;
      else if (timeMillis - this.time_last_sent >= GLOBALS.CLIENT_CONNECT_RETRY_TIME)
      {
        this.sendUdpData(this.connectString);
        this.time_last_sent = timeMillis;
      }
    }
    if (this.time_last_packet != -1L && timeMillis - this.time_last_packet >= GLOBALS.CLIENT_DISCONNECT_TIMEOUT || this.time_last_packet == -1L && timeMillis - this.time_started >= GLOBALS.CLIENT_DISCONNECT_TIMEOUT)
    {
      this.connection_state = ClientLow.ConnectionStates.LOST;
    }
    else
    {
      if (!GLOBALS.ENABLE_UDP_STATS || timeMillis - this.time_last_count_check <= GLOBALS.SERVER_MESSAGE_COUNT_TIME)
        return;
      string message = "";
      if (this.netmonitor != null)
      {
        foreach (KeyValuePair<string, int> keyValuePair in this.netmonitor)
          message = message + "\n" + keyValuePair.Key + "=" + (object) (float) ((double) keyValuePair.Value / (double) (timeMillis - this.time_last_count_check) * 1000.0);
        this.ShowMessage(message);
        this.netmonitor.Clear();
      }
      this.time_last_count_check = timeMillis;
    }
  }

  public bool Connect(
    string ipfield,
    string portfield,
    string pName,
    string passfield,
    string startfield)
  {
    this.ResetAllStates();
    if (pName == "")
    {
      this.ShowMessage("You must enter a name!");
      return false;
    }
    if (this.connection_state != ClientLow.ConnectionStates.NOTSTARTED)
      return true;
    ClientLow.ourPlayerName = pName;
    if (ipfield != "")
      ClientLow.serverIP = ipfield;
    int result = 0;
    if (portfield.Length > 0 && int.TryParse(portfield, out result))
      ClientLow.serverPORT = result;
    if (!this.ClientStart())
    {
      this.ShowMessage("Unable to establish a UDP connection to server!");
      return false;
    }
    this.time_last_packet = -1L;
    this.interpolation_on = false;
    this.first_time_game_settings = true;
    if (!LicenseData.CheckRobotIsUnlocked(this.scorer.CorrectRobotChoice(GLOBALS.RobotModel), GLOBALS.robotskins))
      GLOBALS.robotskins = "Default";
    string str = startfield == "Spectator" || startfield == "Admin" ? "AvatarSpectator" : GLOBALS.RobotModel;
    this.connectString = "NAMEIS\u0011" + pName + "\u0011" + passfield + "\u0011" + startfield + "\u0011" + str + "\u0011" + GLOBALS.GAME + "\u0011" + Application.version + "\u0011" + GLOBALS.DriveTrain + "\u0011" + (object) GLOBALS.speed + "\u0011" + (object) GLOBALS.acceleration + "\u0011" + (object) GLOBALS.weight + "\u0011" + (object) GLOBALS.turning_scaler + "\u0011" + GLOBALS.fieldcentric.ToString() + "\u0011" + GLOBALS.activebreaking.ToString() + "\u0011" + GLOBALS.tankcontrol.ToString() + "\u0011" + GLOBALS.skins + "\u0011" + GLOBALS.robotskins + "\u0011";
    this.connection_state = ClientLow.ConnectionStates.CONNECTING;
    this.time_started = -1L;
    this.MonitorConnection();
    this.myStartfield = startfield;
    return true;
  }

  public void InitPlaybackMode()
  {
    this.playbackmode = true;
    this.oldPlayerName = ClientLow.ourPlayerName;
    ClientLow.ourPlayerName = "Spectator";
    this.SpawnPlayer("Spectator", "-1", "AvatarSpectator", "Spectator");
  }

  public void Playback_Stopped()
  {
    ClientLow.ourPlayerName = this.oldPlayerName;
    foreach (interpolation interpolation in UnityEngine.Object.FindObjectsOfType<interpolation>())
      interpolation.StopMovement();
    if (!(this.myPosition != "Spectator"))
      return;
    this.myRobot_working = this.myRobot_saved;
    this.DoKeyboardStuff(true);
  }

  private void DoKeyboardStuff(bool force_camera_change = false)
  {
    if (!force_camera_change && GLOBALS.keyboard_inuse)
      return;
    bool flag = (Input.GetKey(KeyCode.Space) || GLOBALS.JoystickMap["Jcontrolls_camera"].GetButton()) && !this.key_space_last;
    this.key_space_last = Input.GetKey(KeyCode.Space) || GLOBALS.JoystickMap["Jcontrolls_camera"].GetButton();
    if (!(force_camera_change | flag) || (UnityEngine.Object) this.main_camera == (UnityEngine.Object) null || (UnityEngine.Object) this.player_camera == (UnityEngine.Object) null)
      return;
    if ((bool) (UnityEngine.Object) this.spec_myRobot)
    {
      this.use_spec_myRobot = false;
      this.spec_myRobot.disable_motion = true;
    }
    if (this.myPosition == "Spectator" || GLOBALS.now_playing)
    {
      List<int> intList = new List<int>((IEnumerable<int>) this.players.Keys);
      if ((bool) (UnityEngine.Object) this.selected_robot && (bool) (UnityEngine.Object) this.selected_robot.myRobotID)
      {
        if (intList.Contains(this.selected_robot.myRobotID.id))
        {
          int num = intList.IndexOf(this.selected_robot.myRobotID.id);
          this.spectator_curr_camera_player = this.spectator_curr_camera_player != num ? num : -2;
        }
      }
      else
        ++this.spectator_curr_camera_player;
      if (this.players.Count < this.spectator_curr_camera_player + 1)
        this.spectator_curr_camera_player = (bool) (UnityEngine.Object) this.spectator_cam2 || GLOBALS.now_playing ? -2 : -1;
      if (this.spectator_curr_camera_player < 0 && (this.myPosition == "Spectator" || this.spectator_curr_camera_player != -2))
      {
        GLOBALS.camera_follows = false;
        if ((bool) (UnityEngine.Object) this.spectator_cam2 && this.spectator_curr_camera_player == -2)
        {
          this.main_camera.transform.position = this.spectator_cam2.transform.position;
          this.main_camera.transform.rotation = this.spectator_cam2.transform.rotation;
        }
        else
        {
          this.main_camera.transform.position = this.spectator_cam1.transform.position;
          this.main_camera.transform.rotation = this.spectator_cam1.transform.rotation;
        }
        this.myRobot_working = (RobotInterface3D) null;
      }
      else
      {
        GLOBALS.camera_follows = true;
        this.robot_ref = (Transform) null;
        if (this.spectator_curr_camera_player < 0)
        {
          this.SpawnSpectator();
          this.use_spec_myRobot = true;
          this.spec_myRobot.disable_motion = false;
        }
        else
        {
          this.myRobot_working = this.players[intList[this.spectator_curr_camera_player]].robot;
          if ((UnityEngine.Object) this.myRobot_working == (UnityEngine.Object) this.spec_myRobot)
            this.spec_myRobot.disable_motion = false;
        }
        if ((bool) (UnityEngine.Object) this.mycameratracker)
        {
          this.mycameratracker.transform.parent = (Transform) null;
          UnityEngine.Object.Destroy((UnityEngine.Object) this.mycameratracker);
          this.mycameratracker = (GameObject) null;
        }
        this.main_camera.transform.rotation = Quaternion.identity;
      }
      this.main_camera.transform.position /= GLOBALS.worldscale / 2f;
    }
    else
    {
      if ((UnityEngine.Object) this.myRobot_working != (UnityEngine.Object) this.myRobot_saved)
        this.myRobot_working = this.myRobot_saved;
      GLOBALS.camera_follows = !GLOBALS.camera_follows;
      if ((bool) (UnityEngine.Object) this.scorer)
        this.scorer.OnCameraViewChanged();
      this.DoCameraViewChanged();
    }
  }

  public void DoCameraViewChanged()
  {
    if (!GLOBALS.camera_follows)
    {
      this.main_camera.transform.position = this.player_camera.transform.position;
      this.main_camera.transform.rotation = this.player_camera.transform.rotation;
      if ((bool) (UnityEngine.Object) this.vr_camera)
      {
        this.vr_camera.transform.position = this.vr_starting_pos;
        this.vr_camera.transform.rotation = this.vr_starting_rot;
      }
      this.main_camera.transform.position /= GLOBALS.worldscale / 2f;
    }
    else
    {
      this.robot_ref = (Transform) null;
      if ((bool) (UnityEngine.Object) this.mycameratracker)
      {
        this.mycameratracker.transform.parent = (Transform) null;
        UnityEngine.Object.Destroy((UnityEngine.Object) this.mycameratracker);
      }
      this.mycameratracker = (GameObject) null;
    }
  }

  private void SetCamera(string RobotPosition)
  {
    this.player_camera = GameObject.Find(RobotPosition + " Cam");
    if ((UnityEngine.Object) this.player_camera == (UnityEngine.Object) null)
      this.player_camera = GameObject.Find("Spectator Cam");
    this.spectator_cam1 = GameObject.Find("Spectator Cam");
    this.spectator_cam2 = GameObject.Find("Spectator Cam 2");
    this.main_camera = GameObject.Find("MainCamera");
    this.vr_camera = GameObject.Find("OVRCamera");
    GameObject gameObject = GameObject.Find("OVRCameraScaling");
    MyUtils.SetCameraQualityLevel(this.main_camera);
    if ((UnityEngine.Object) this.main_camera != (UnityEngine.Object) null && (UnityEngine.Object) this.player_camera != (UnityEngine.Object) null)
    {
      cameraOrbit component = this.main_camera.GetComponent<cameraOrbit>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.enabled = false;
      this.main_camera.transform.position = this.player_camera.transform.position;
      this.main_camera.transform.rotation = this.player_camera.transform.rotation;
      this.main_camera.transform.position /= GLOBALS.worldscale / 2f;
      float num = 60f * Mathf.Pow(2f / GLOBALS.worldscale, -1.15f);
      this.main_camera.GetComponent<Camera>().fieldOfView = num;
    }
    if (!((UnityEngine.Object) this.vr_camera != (UnityEngine.Object) null) || !((UnityEngine.Object) this.player_camera != (UnityEngine.Object) null))
      return;
    this.vr_camera.transform.position = this.player_camera.transform.position with
    {
      y = 0.2f
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

  private void UpdateTrackingCamera()
  {
    RobotInterface3D robotInterface3D = this.use_spec_myRobot ? (RobotInterface3D) this.spec_myRobot : this.myRobot_working;
    if (!GLOBALS.camera_follows || !(bool) (UnityEngine.Object) this.main_camera || !(bool) (UnityEngine.Object) robotInterface3D || GLOBALS.CAMERA_COUNTDOWN_CONTROL)
      return;
    if (this.holding_mode && this.myPosition != "Spectator")
    {
      if (!(bool) (UnityEngine.Object) this.player_camera || !(bool) (UnityEngine.Object) this.player_camera.transform || !(bool) (UnityEngine.Object) this.main_camera.transform)
        return;
      this.main_camera.transform.position = this.player_camera.transform.position;
      this.main_camera.transform.rotation = this.player_camera.transform.rotation;
      this.robot_ref = (Transform) null;
    }
    else
    {
      bool flag = this.myPosition == "Spectator" && (!GLOBALS.camera_follows || !(bool) (UnityEngine.Object) this.myRobot_working.myRobotID);
      if (!(bool) (UnityEngine.Object) this.robot_ref)
      {
        GameObject gameObject1 = GameObject.Find("robot_ref");
        GameObject gameObject2 = GameObject.Find("camera_ref");
        if (!(bool) (UnityEngine.Object) gameObject1 || !(bool) (UnityEngine.Object) gameObject2)
          return;
        this.robot_ref = gameObject1.transform;
        this.camera_ref = gameObject2.transform;
        if (!(bool) (UnityEngine.Object) this.robot_ref || !(bool) (UnityEngine.Object) this.camera_ref)
          return;
        this.mycameratracker = new GameObject();
        if (!flag)
        {
          this.mycameratracker.transform.position = (this.camera_ref.position - this.robot_ref.position) * 2f / GLOBALS.worldscale;
          Quaternion rotation = this.mycameratracker.transform.rotation with
          {
            eulerAngles = this.camera_ref.rotation.eulerAngles - this.robot_ref.rotation.eulerAngles
          };
          Vector3 eulerAngles = rotation.eulerAngles;
          eulerAngles.x -= (float) (((double) GLOBALS.worldscale - 2.0) * 5.0);
          rotation.eulerAngles = eulerAngles;
          this.mycameratracker.transform.rotation = rotation;
          this.mycameratracker.transform.RotateAround(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1f, 0.0f), 90f);
        }
        else
          this.mycameratracker.transform.localRotation = Quaternion.Euler(0.0f, 90f, 0.0f);
        Quaternion rotation1 = this.mycameratracker.transform.rotation;
        Vector3 localPosition = this.mycameratracker.transform.localPosition;
        Vector3 localScale = this.mycameratracker.transform.localScale;
        Quaternion localRotation = this.mycameratracker.transform.localRotation;
        if ((UnityEngine.Object) robotInterface3D.rb_body == (UnityEngine.Object) null)
          return;
        this.mycameratracker.transform.SetParent(robotInterface3D.rb_body.transform, false);
        this.mycameratracker.transform.localPosition = localPosition;
        this.mycameratracker.transform.localScale = localScale;
        this.mycameratracker.transform.localRotation = localRotation;
        this.main_camera.transform.rotation = localRotation;
        if ((bool) (UnityEngine.Object) this.vr_camera && (bool) (UnityEngine.Object) this.vr_camera.transform)
          this.vr_camera.transform.rotation = this.mycameratracker.transform.rotation;
      }
      if (!(bool) (UnityEngine.Object) this.mycameratracker || !(bool) (UnityEngine.Object) this.mycameratracker.transform)
        return;
      Vector3 position1 = this.mycameratracker.transform.position;
      if (!flag)
        position1.y = this.camera_ref.position.y * (float) (1.0 + (2.0 - (double) GLOBALS.worldscale) * 0.219999998807907);
      Vector3 position2 = this.main_camera.transform.position;
      float cameraAveraging = (float) GLOBALS.CAMERA_AVERAGING;
      this.main_camera.transform.position = position2 + (position1 - position2) / cameraAveraging;
      Vector3 eulerAngles1 = this.mycameratracker.transform.rotation.eulerAngles;
      if (!flag)
      {
        eulerAngles1.x = this.main_camera.transform.rotation.eulerAngles.x;
        eulerAngles1.z = this.main_camera.transform.rotation.eulerAngles.z;
      }
      Quaternion rotation2 = this.main_camera.transform.rotation;
      Quaternion b = Quaternion.Euler(eulerAngles1);
      this.main_camera.transform.rotation = (double) cameraAveraging > 1.10000002384186 ? Quaternion.Lerp(rotation2, b, 1f / cameraAveraging) : b;
      if (!(bool) (UnityEngine.Object) this.vr_camera || !(bool) (UnityEngine.Object) this.vr_camera.transform)
        return;
      position1.y = 0.0f;
      this.vr_camera.transform.position = position1;
      this.vr_camera.transform.rotation = b;
    }
  }

  public void FlagRequest(Dropdown menu)
  {
    this.flagrequest_menu = menu;
    if (this.ourClientId == -1)
    {
      this.FlagReset();
    }
    else
    {
      switch (menu.value)
      {
        case 1:
          if (this.holding_mode)
            break;
          this.clientFlags["RESTARTALL"] = "1";
          break;
        case 2:
          this.RequestPosReset();
          break;
      }
    }
  }

  public bool RequestPosReset()
  {
    if (this.holding_mode)
      return false;
    this.clientFlags["RESETMYPOS"] = "1";
    return true;
  }

  private void FlagReset()
  {
    if ((UnityEngine.Object) this.flagrequest_menu == (UnityEngine.Object) null)
      return;
    this.flagrequest_menu.value = 0;
    this.flagrequest_menu.itemText.text = this.flagrequest_menu.options[0].text;
  }

  public int GetConnectionStatus()
  {
    switch (this.connection_state)
    {
      case ClientLow.ConnectionStates.NOTSTARTED:
      case ClientLow.ConnectionStates.LOST:
        return -1;
      case ClientLow.ConnectionStates.CONNECTING:
        return 0;
      case ClientLow.ConnectionStates.CONNECTED:
        return this.found_position ? 1 : 0;
      default:
        return -1;
    }
  }

  public void SendChat(string msg)
  {
    if (msg.StartsWith("/SET "))
    {
      string[] strArray = msg.Substring(5).Split('=');
      strArray[0] = strArray[0].TrimEnd();
      if (strArray[0] == "OUTPUT_SCORE_FILES")
      {
        GLOBALS.OUTPUT_SCORING_FILES = true;
        if (strArray.Length <= 1)
          return;
        MyUtils.status_file_dir = strArray[1];
      }
      else if (strArray[0] == "TURN_OFF_HUD")
      {
        if (!(bool) (UnityEngine.Object) this.top_canvas)
          return;
        this.top_canvas.SetActive(false);
      }
      else if (strArray[0] == "TURN_ON_HUD")
      {
        if (!(bool) (UnityEngine.Object) this.top_canvas)
          return;
        this.top_canvas.SetActive(true);
      }
      else if (strArray[0] == "FIREWORKS_BLUE")
        this.scorer.StartFireworks(false);
      else if (strArray[0] == "FIREWORKS_RED")
        this.scorer.StartFireworks(true);
      else if (strArray[0] == "SHOW_VS")
        this.scorer.ShowGameStartOverlayNoAnimation();
      else if (strArray[0] == "HIDE_VS")
      {
        this.scorer.ShowGameStartOverlayNoAnimation(false);
      }
      else
      {
        if (!(strArray[0] == "HIDE_CHAMPS"))
          return;
        this.DisableChampsMode();
      }
    }
    else
      this.clientFlags["CHAT"] = msg;
  }

  public void Toggle_HUD()
  {
    if ((bool) (UnityEngine.Object) this.top_canvas)
      this.top_canvas.SetActive(!this.top_canvas.activeSelf);
    GameObject gameObject = GameObject.Find("ServerInfoWindow");
    if (!(bool) (UnityEngine.Object) gameObject)
      return;
    gameObject.SetActive(false);
  }

  public void PrepOverlays()
  {
    this.name_parent = GameObject.Find("/Canvas2/Names");
    if (!(bool) (UnityEngine.Object) this.name_parent)
      return;
    this.name_rl = this.name_parent.transform.Find("RL");
    this.name_rc = this.name_parent.transform.Find("RC");
    this.name_rr = this.name_parent.transform.Find("RR");
    this.name_bl = this.name_parent.transform.Find("BL");
    this.name_bc = this.name_parent.transform.Find("BC");
    this.name_br = this.name_parent.transform.Find("BR");
  }

  public void ApplyOverlays()
  {
    if ((UnityEngine.Object) this.main_camera == (UnityEngine.Object) null || (UnityEngine.Object) this.name_parent == (UnityEngine.Object) null)
      return;
    bool flag1 = Input.GetKey(KeyCode.Alpha1) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
    if (!GLOBALS.keyboard_inuse & flag1 && !this.old_keys_combo_overlay)
    {
      ++this.overlay_mode;
      if (this.overlay_mode > 2)
        this.overlay_mode = 0;
    }
    this.old_keys_combo_overlay = flag1;
    bool flag2 = Input.GetKey(KeyCode.Alpha2) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
    if (!GLOBALS.keyboard_inuse & flag2 && !this.old_keys_combo_overlay2)
    {
      ++this.overlay_details;
      if (this.overlay_details > 3)
        this.overlay_details = 0;
    }
    this.old_keys_combo_overlay2 = flag2;
    this.name_rl.gameObject.SetActive(false);
    this.name_rc.gameObject.SetActive(false);
    this.name_rr.gameObject.SetActive(false);
    this.name_bl.gameObject.SetActive(false);
    this.name_bc.gameObject.SetActive(false);
    this.name_br.gameObject.SetActive(false);
    if (this.overlay_mode != 0)
    {
      foreach (ClientLow.Player currplayer in this.players.Values)
      {
        if ((bool) (UnityEngine.Object) currplayer.robot && !currplayer.robot.isSpectator && (bool) (UnityEngine.Object) currplayer.robot.rb_body)
        {
          string startingPos = currplayer.robot.myRobotID.starting_pos;
          if (!(startingPos == "Red Left"))
          {
            if (!(startingPos == "Red Center"))
            {
              if (!(startingPos == "Red Right"))
              {
                if (!(startingPos == "Blue Left"))
                {
                  if (!(startingPos == "Blue Center"))
                  {
                    if (startingPos == "Blue Right")
                      this.SetOverlayNamePos(currplayer, this.name_br, 5f);
                  }
                  else
                    this.SetOverlayNamePos(currplayer, this.name_bc, 3f);
                }
                else
                  this.SetOverlayNamePos(currplayer, this.name_bl, 1f);
              }
              else
                this.SetOverlayNamePos(currplayer, this.name_rr, 4f);
            }
            else
              this.SetOverlayNamePos(currplayer, this.name_rc, 2f);
          }
          else
            this.SetOverlayNamePos(currplayer, this.name_rl, 0.0f);
        }
      }
    }
    if (!(bool) (UnityEngine.Object) this.detailsText)
      return;
    if (this.detailsText.text.Length > 0)
      this.detailsText.text = "";
    if (this.overlay_details != 0)
    {
      if (this.overlay_details == 1 || this.overlay_details == 3)
      {
        UnityEngine.UI.Text detailsText = this.detailsText;
        detailsText.text = detailsText.text + "FPS=" + (object) this.FPS;
      }
      if (this.overlay_details == 2 || this.overlay_details == 3)
      {
        foreach (string key in MyUtils.score_details.Keys)
        {
          if (!(key == "OPR"))
          {
            string str = !GLOBALS.ScorefilesDescription.ContainsKey(key) ? key : GLOBALS.ScorefilesDescription[key];
            UnityEngine.UI.Text detailsText = this.detailsText;
            detailsText.text = detailsText.text + "\n" + str + "=" + MyUtils.score_details[key];
          }
        }
        if (MyUtils.score_details.ContainsKey("OPR"))
        {
          UnityEngine.UI.Text detailsText = this.detailsText;
          detailsText.text = detailsText.text + "\n" + GLOBALS.ScorefilesDescription["OPR"] + "     \n" + MyUtils.score_details["OPR"];
        }
      }
    }
    if (!(bool) (UnityEngine.Object) this.myRobot_working || !(bool) (UnityEngine.Object) this.scorer_overlays || !(bool) (UnityEngine.Object) this.myRobot_working.myRobotID)
      return;
    int id = this.myRobot_working.myRobotID.id;
    string overlaysString = this.scorer.GetOverlaysString(id);
    if (!this.old_overlay_string.Equals(overlaysString))
    {
      foreach (Component component in this.scorer_overlays.transform)
        UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
      this.scorer.GetOverlays(id, this.scorer_overlays);
    }
    this.old_overlay_string = overlaysString;
  }

  private void SetOverlayNamePos(ClientLow.Player currplayer, Transform name_tr, float pos_offset)
  {
    name_tr.GetComponent<TextMeshPro>().text = currplayer.playerName;
    Vector3 localPosition = name_tr.localPosition;
    this.screen_pos = this.main_camera.GetComponent<Camera>().WorldToScreenPoint(currplayer.robot.rb_body.transform.position);
    if ((double) this.screen_pos.z < 0.0)
      return;
    float num1 = (this.screen_pos.x - (float) this.main_camera.GetComponent<Camera>().pixelWidth / 2f) / (float) this.main_camera.GetComponent<Camera>().pixelWidth;
    float num2 = (this.screen_pos.y - (float) this.main_camera.GetComponent<Camera>().pixelHeight / 2f) / (float) this.main_camera.GetComponent<Camera>().pixelHeight;
    ref Vector3 local1 = ref localPosition;
    double num3 = (double) num1;
    Rect rect = this.name_parent.GetComponent<RectTransform>().rect;
    double width = (double) rect.width;
    double num4 = num3 * width;
    local1.x = (float) num4;
    if (this.overlay_mode == 1)
    {
      ref Vector3 local2 = ref localPosition;
      rect = this.name_parent.GetComponent<RectTransform>().rect;
      double num5 = (double) rect.height / 2.0 - (double) GLOBALS.OVERLAY_NAME_Y_OFFSET - (double) pos_offset * (double) GLOBALS.OVERLAY_NAME_Y_INCREMENT;
      local2.y = (float) num5;
    }
    else
    {
      ref Vector3 local3 = ref localPosition;
      double num6 = (double) num2;
      rect = this.name_parent.GetComponent<RectTransform>().rect;
      double height = (double) rect.height;
      double num7 = num6 * height - 10.0;
      local3.y = (float) num7;
    }
    name_tr.localPosition = localPosition;
    name_tr.gameObject.SetActive(true);
  }

  private void SendMyInputs()
  {
    if ((UnityEngine.Object) this.myPlayer == (UnityEngine.Object) null || (UnityEngine.Object) this.myRobot_working == (UnityEngine.Object) null)
      return;
    if (GLOBALS.now_playing)
    {
      if (!this.use_spec_myRobot && !((UnityEngine.Object) this.myRobot_working == (UnityEngine.Object) this.spec_myRobot))
        return;
      this.spec_myRobot.updateGamepadVars();
    }
    else
    {
      this.myRobot_working.updateGamepadVars();
      if (this.myPosition == "Spectator")
        return;
      this.sendUdpData("MYINPUTS\u0011" + MyUtils.BoolToString(this.myRobot_working.gamepad1_a) + "\u0012" + MyUtils.BoolToString(this.myRobot_working.gamepad1_b) + "\u0012" + MyUtils.BoolToString(this.myRobot_working.gamepad1_x) + "\u0012" + MyUtils.BoolToString(this.myRobot_working.gamepad1_y) + "\u0012" + this.myRobot_working.gamepad1_right_stick_y.ToString("0.###") + "\u0012" + this.myRobot_working.gamepad1_right_stick_x.ToString("0.###") + "\u0012" + this.myRobot_working.gamepad1_left_stick_x.ToString("0.###") + "\u0012" + this.myRobot_working.gamepad1_left_stick_y.ToString("0.###") + "\u0012" + MyUtils.BoolToString(this.myRobot_working.gamepad1_dpad_down) + "\u0012" + MyUtils.BoolToString(this.myRobot_working.gamepad1_dpad_up) + "\u0012" + MyUtils.BoolToString(this.myRobot_working.gamepad1_dpad_left) + "\u0012" + MyUtils.BoolToString(this.myRobot_working.gamepad1_dpad_right) + "\u0012" + MyUtils.BoolToString(this.myRobot_working.gamepad1_right_bumper) + "\u0012" + MyUtils.BoolToString(this.myRobot_working.gamepad1_left_bumper) + "\u0012" + this.myRobot_working.gamepad1_left_trigger.ToString("0.###") + "\u0012" + this.myRobot_working.gamepad1_right_trigger.ToString("0.###") + "\u0012" + MyUtils.BoolToString(this.myRobot_working.gamepad1_stop) + "\u0012" + MyUtils.BoolToString(this.myRobot_working.gamepad1_restart) + "\u0012");
      if (!this.myRobot_working.gamepad1_reset_changed || !this.myRobot_working.gamepad1_reset)
        return;
      this.RequestPosReset();
    }
  }

  private void SendFlags()
  {
    if (this.clientFlags.Count < 1)
      this.clientFlags["NotUsed"] = "1";
    if (this.ourClientId < 0)
      return;
    if (GLOBALS.now_playing)
    {
      this.SendFlagsUDPTracked("NotUsed\u00121");
    }
    else
    {
      string messageraw = "";
      bool flag = true;
      foreach (string key in this.clientFlags.Keys)
      {
        if (!flag)
          messageraw += "\u0012";
        flag = false;
        messageraw = messageraw + key + "\u0012" + this.clientFlags[key];
      }
      this.SendFlagsUDPTracked(messageraw);
      this.clientFlags.Remove("CHAT");
      this.clientFlags.Remove("RESTARTALL");
      this.clientFlags.Remove("RESETMYPOS");
      this.clientFlags.Remove("GameSettings");
      this.FlagReset();
    }
  }

  public void UpdatePacketLoss()
  {
    if (ClientLow.GetTimeMillis() - this.last_update_time < 500L)
      return;
    this.last_update_time = ClientLow.GetTimeMillis();
    if (this.latest_message_id - this.last_checked_message_id <= 0)
    {
      this.clientFlags["PL"] = "0";
    }
    else
    {
      this.clientFlags["PL"] = ((int) (100.0 * (1.0 - (double) this.received_msg_count / (double) (this.latest_message_id - this.last_checked_message_id)))).ToString();
      this.last_checked_message_id = this.latest_message_id;
      this.received_msg_count = 0;
    }
  }

  public void onReceivedData(ref List<byte[]> messages)
  {
    if (messages.Count < 1)
      return;
    for (int index = messages.Count - 35; index < messages.Count; ++index)
    {
      if (index < 0)
        index = 0;
      byte[] message = messages[index];
      List<byte[]> extracted_data = new List<byte[]>();
      if (!MyUtils.ExtractMessageHeader(message, extracted_data))
        MyUtils.LogMessageToFile("OnReceiveData split failed on _. " + (object) message);
      else if (!Encoding.UTF8.GetString(extracted_data[0]).Equals("7p0"))
      {
        MyUtils.LogMessageToFile("OnReceiveData passcode failed. " + (object) message);
      }
      else
      {
        int result1 = 0;
        if (!int.TryParse(Encoding.UTF8.GetString(extracted_data[3]), out result1))
          MyUtils.LogMessageToFile("OnReceiveData extraction of length failed. " + (object) message);
        else if (result1 != extracted_data[4].Length)
        {
          MyUtils.LogMessageToFile("OnReceiveData lengths did not equal. Length =" + result1.ToString() + " Actual=" + extracted_data[3].Length.ToString());
        }
        else
        {
          int result2 = 0;
          if (!int.TryParse(Encoding.UTF8.GetString(extracted_data[1]), out result2))
          {
            MyUtils.LogMessageToFile("OnReceiveData extraction of compression failed. " + (object) message);
          }
          else
          {
            int result3 = -1;
            if (!int.TryParse(Encoding.UTF8.GetString(extracted_data[2]), out result3))
              result3 = -1;
            string str = MyUtils.DecompressMessage(extracted_data[4], result2);
            string[] strArray = str.Split('\u0011');
            if (strArray.Length < 2)
            {
              MyUtils.LogMessageToFile("OnReceiveData split failed on \u0011 " + str);
            }
            else
            {
              MyUtils.PB_RecordData(strArray, result3);
              if (!GLOBALS.now_playing)
                this.ProcessData(strArray, result3);
            }
          }
        }
      }
    }
  }

  private void ProcessData(string[] splitData, int timestamp, bool playback_mode = false)
  {
    string str = splitData[0];
    if (str == "NEW")
      return;
    if (!(str == "4.5"))
    {
      if (!(str == "3.2"))
      {
        if (!(str == "PLYRS"))
        {
          if (!(str == "ERR"))
          {
            if (str == "FLG")
              this.OnServerFlags(splitData, playback_mode);
            else
              MyUtils.LogMessageToFile("OnReceiveData invalid HEADER:" + splitData[0]);
          }
          else
            this.onServerError(splitData, playback_mode);
        }
        else
          this.onCurrentPlayers(splitData, playback_mode);
      }
      else
        this.updateElementLocations(splitData, timestamp, playback_mode);
    }
    else
    {
      this.found_position = true;
      this.OnPlayerLocations(splitData, timestamp, playback_mode);
    }
  }

  private void onCurrentPlayers(string[] data, bool playback_mode = false)
  {
    if (data.Length < 4)
    {
      MyUtils.LogMessageToFile("onCurrentPlayers data length < 4 at " + (object) data.Length);
    }
    else
    {
      int result1 = -1;
      if (!int.TryParse(data[1], out result1))
      {
        MyUtils.LogMessageToFile("onCurrentPlayers invalid packet age:" + data[1]);
      }
      else
      {
        int result2 = -1;
        if (!int.TryParse(data[2], out result2))
        {
          MyUtils.LogMessageToFile("onCurrentPlayers invalid packet age:" + data[2]);
        }
        else
        {
          if (result1 != result2)
            return;
          if (!playback_mode)
          {
            if (this.id_current_players > result1)
            {
              MyUtils.LogMessageToFile("onCurrentPlayers dropped old message.");
              return;
            }
            this.id_current_players = result1;
          }
          List<ClientLow.Player> playerList = new List<ClientLow.Player>((IEnumerable<ClientLow.Player>) this.players.Values);
          List<string[]> strArrayList = new List<string[]>();
          for (int index1 = 3; index1 < data.Length; ++index1)
          {
            bool flag = true;
            string[] strArray = data[index1].Split('\u0012');
            if (strArray.Length >= 4)
            {
              if (playerList.Count > 0)
              {
                for (int index2 = playerList.Count - 1; index2 >= 0; --index2)
                {
                  int result3 = -1;
                  if (int.TryParse(strArray[1], out result3) && playerList[index2].connectionId == result3)
                  {
                    flag = false;
                    if ((bool) (UnityEngine.Object) playerList[index2].robot)
                      playerList[index2].robot.SetStates(strArray[4]);
                    playerList.RemoveAt(index2);
                    break;
                  }
                }
              }
              if (flag)
              {
                if (ClientLow.ourPlayerName.Equals(strArray[0]))
                  this.SpawnPlayer(strArray[0], strArray[1], strArray[2], strArray[3], strArray.Length > 5 ? strArray[5] : "0", strArray.Length > 6 ? strArray[6] : "");
                else
                  strArrayList.Add(new string[6]
                  {
                    strArray[0],
                    strArray[1],
                    strArray[2],
                    strArray[3],
                    strArray.Length > 5 ? strArray[5] : "0",
                    strArray.Length > 6 ? strArray[6] : ""
                  });
              }
            }
          }
          for (int index = 0; index < strArrayList.Count; ++index)
            this.SpawnPlayer(strArrayList[index][0], strArrayList[index][1], strArrayList[index][2], strArrayList[index][3], strArrayList[index][4], strArrayList[index][5]);
          for (int index = playerList.Count - 1; index >= 0; --index)
          {
            if (!this.playbackmode || playerList[index].connectionId != this.ourClientId)
              this.PlayerDisconnected(playerList[index].connectionId);
          }
        }
      }
    }
  }

  private void updateElementLocations(string[] locations, int timestamp = -1, bool playback_mode = false)
  {
    if (!this.interpolation_on)
      this.TurnOnInterpolation();
    int result1 = -1;
    if (!int.TryParse(locations[1], out result1))
    {
      MyUtils.LogMessageToFile("UpdateElementLocations invalid packet age:" + locations[1]);
    }
    else
    {
      int result2 = -1;
      if (!int.TryParse(locations[2], out result2))
      {
        MyUtils.LogMessageToFile("UpdateElementLocations invalid packet age:" + locations[2]);
      }
      else
      {
        if (result1 != result2)
          return;
        if (!playback_mode && result1 > this.latest_message_id)
          this.latest_message_id = result1;
        if (!playback_mode)
          ++this.received_msg_count;
        bool flag = false;
        for (int index1 = 3; index1 < locations.Length; ++index1)
        {
          string[] getdata = locations[index1].Split('\u0012');
          if (getdata.Length >= 1 && getdata[0].Length >= 1)
          {
            int num1 = 0;
            try
            {
              string[] strArray1 = getdata;
              int index2 = num1;
              int num2 = index2 + 1;
              int key = int.Parse(strArray1[index2]);
              if (!playback_mode)
              {
                if (this.allFieldElements[key].GetComponent<gameElement>().lastupdateID > result1)
                {
                  flag = true;
                  continue;
                }
                this.allFieldElements[key].GetComponent<gameElement>().lastupdateID = result1;
              }
              string[] strArray2 = getdata;
              int index3 = num2;
              int startpos = index3 + 1;
              string str = strArray2[index3];
              int num3;
              if ((bool) (UnityEngine.Object) this.allFieldElements[key].GetComponent<BandwidthHelper>())
              {
                num3 = startpos + this.allFieldElements[key].GetComponent<BandwidthHelper>().Set(getdata, startpos, timestamp);
              }
              else
              {
                string[] strArray3 = getdata;
                int index4 = startpos;
                int num4 = index4 + 1;
                float x1 = float.Parse(strArray3[index4]);
                string[] strArray4 = getdata;
                int index5 = num4;
                int num5 = index5 + 1;
                float y1 = float.Parse(strArray4[index5]);
                string[] strArray5 = getdata;
                int index6 = num5;
                int num6 = index6 + 1;
                float z1 = float.Parse(strArray5[index6]);
                string[] strArray6 = getdata;
                int index7 = num6;
                int num7 = index7 + 1;
                float x2 = float.Parse(strArray6[index7]);
                string[] strArray7 = getdata;
                int index8 = num7;
                int num8 = index8 + 1;
                float y2 = float.Parse(strArray7[index8]);
                string[] strArray8 = getdata;
                int index9 = num8;
                num3 = index9 + 1;
                float z2 = float.Parse(strArray8[index9]);
                this.allFieldElements[key].GetComponent<interpolation>().SetPosition(new Vector3(x1, y1, z1), timestamp);
                this.allFieldElements[key].GetComponent<interpolation>().SetRotation(new Vector3(x2, y2, z2), timestamp);
              }
            }
            catch (Exception ex)
            {
              MyUtils.LogMessageToFile("updateElementLocations exception! " + ex.ToString());
            }
          }
        }
        if (!(!playback_mode & flag))
          return;
        --this.received_msg_count;
      }
    }
  }

  private void OnPlayerLocations(string[] data, int timestamp = -1, bool playback_mode = false)
  {
    int result1 = -1;
    if (!int.TryParse(data[1], out result1))
    {
      MyUtils.LogMessageToFile("OnPlayerLocations invalid packet age:" + data[1]);
    }
    else
    {
      int result2 = -1;
      if (!int.TryParse(data[2], out result2))
      {
        MyUtils.LogMessageToFile("OnPlayerLocations invalid packet age:" + data[2]);
      }
      else
      {
        if (result1 != result2)
          return;
        if (!playback_mode && this.latest_message_id < result1)
          this.latest_message_id = result1;
        if (!playback_mode)
          ++this.received_msg_count;
        try
        {
          this.netmonitor_header = "4.5";
          for (int index1 = 3; index1 < data.Length; ++index1)
          {
            if (data[index1].Contains('\u0012'.ToString()))
            {
              string[] getdata = data[index1].Split('\u0012');
              int key1 = int.Parse(getdata[0]);
              int.Parse(getdata[1]);
              int num = 2;
              if (this.players.ContainsKey(key1))
              {
                if (!playback_mode && GLOBALS.ENABLE_UDP_STATS)
                {
                  string key2 = this.netmonitor_header + "_" + (object) key1 + "_in";
                  if (this.netmonitor.ContainsKey(key2))
                    ++this.netmonitor[key2];
                  else
                    this.netmonitor.Add(key2, 1);
                }
                if (!playback_mode && this.players[key1].messageId > result1)
                {
                  --this.received_msg_count;
                  break;
                }
                this.players[key1].messageId = result1;
                int childCount = this.players[key1].avatar.transform.childCount;
                int index2 = num;
                while (index2 < getdata.Length - 1)
                {
                  try
                  {
                    int index3 = int.Parse(getdata[index2]);
                    if (index3 >= childCount)
                    {
                      MyUtils.LogMessageToFile("OnPlayerLocations index > childcount: " + index3.ToString() + ">" + childCount.ToString());
                      break;
                    }
                    if (index3 < 0)
                    {
                      this.players[key1].avatar.transform.GetComponent<interpolation>().SetPosition(new Vector3(float.Parse(getdata[index2 + 1]), float.Parse(getdata[index2 + 2]), float.Parse(getdata[index2 + 3])), timestamp);
                      this.players[key1].avatar.transform.GetComponent<interpolation>().SetRotation(new Vector3(float.Parse(getdata[index2 + 4]), float.Parse(getdata[index2 + 5]), float.Parse(getdata[index2 + 6])), timestamp);
                      index2 += 7;
                    }
                    else if ((bool) (UnityEngine.Object) this.players[key1].avatar.transform.GetChild(index3).GetComponent<BandwidthHelper>())
                    {
                      index2 = this.players[key1].avatar.transform.GetChild(index3).GetComponent<BandwidthHelper>().Set(getdata, index2 + 1, timestamp);
                    }
                    else
                    {
                      this.players[key1].avatar.transform.GetChild(index3).GetComponent<interpolation>().SetLocalPosition(new Vector3(float.Parse(getdata[index2 + 1]), float.Parse(getdata[index2 + 2]), float.Parse(getdata[index2 + 3])), timestamp);
                      this.players[key1].avatar.transform.GetChild(index3).GetComponent<interpolation>().SetLocalRotation(new Vector3(float.Parse(getdata[index2 + 4]), float.Parse(getdata[index2 + 5]), float.Parse(getdata[index2 + 6])), timestamp);
                      index2 += 7;
                    }
                  }
                  catch (Exception ex)
                  {
                    MyUtils.LogMessageToFile("Error in OnPlayerLocations: " + (object) ex);
                    return;
                  }
                }
                if (!playback_mode && GLOBALS.ENABLE_UDP_STATS)
                {
                  string key3 = this.netmonitor_header + "_" + (object) key1 + "_out";
                  if (this.netmonitor.ContainsKey(key3))
                    ++this.netmonitor[key3];
                  else
                    this.netmonitor.Add(key3, 1);
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
          MyUtils.LogMessageToFile("Client OnPlayerLocations exception: " + (object) ex);
        }
      }
    }
  }

  private void onServerError(string[] data, bool playback_mode = false)
  {
    int result = -1;
    if (!int.TryParse(data[1], out result))
    {
      MyUtils.LogMessageToFile("onServerError invalid packet age:" + data[1]);
    }
    else
    {
      if (!playback_mode)
      {
        if (this.id_server_error > result)
          return;
        this.id_server_error = result;
      }
      if (data.Length < 4)
        return;
      this.ShowMessage(data[3]);
    }
  }

  private void OnServerFlags(string[] splitData, bool playback_mode = false)
  {
    this.serverFlags.Clear();
    string str1 = splitData[1];
    char[] chArray1 = new char[1]{ '\u0013' };
    foreach (string str2 in str1.Split(chArray1))
    {
      char[] chArray2 = new char[1]{ '\u0012' };
      string[] strArray1 = str2.Split(chArray2);
      if (strArray1.Length >= 4)
      {
        int num1 = int.Parse(strArray1[0]);
        int num2 = int.Parse(strArray1[1]);
        if (num1 == num2)
        {
          if (!playback_mode)
          {
            if (num1 <= this.client_confirmed_this_msg_id)
            {
              if (num1 + 60 < this.client_confirmed_this_msg_id)
              {
                this.client_confirmed_this_msg_id = num1;
                continue;
              }
              continue;
            }
            this.client_confirmed_this_msg_id = num1;
          }
          int num3 = 2;
          while (num3 < strArray1.Length)
          {
            string[] strArray2 = strArray1;
            int index1 = num3;
            int num4 = index1 + 1;
            string key = strArray2[index1];
            string[] strArray3 = strArray1;
            int index2 = num4;
            num3 = index2 + 1;
            string str3 = strArray3[index2];
            this.serverFlags[key] = str3;
          }
          if (!playback_mode && this.serverFlags.ContainsKey("TRACKED_FLAG_ID"))
            this.server_confirmed_this_msg_id = int.Parse(this.serverFlags["TRACKED_FLAG_ID"]);
          this.ProcessServerFlags();
        }
      }
    }
  }

  private void soundStart(string message)
  {
    string str1 = message;
    char[] chArray1 = new char[1]{ '\u0014' };
    foreach (string str2 in str1.Split(chArray1))
    {
      char[] chArray2 = new char[1]{ '\u0015' };
      string[] strArray = str2.Split(chArray2);
      if (strArray.Length >= 4)
      {
        int key1 = int.Parse(strArray[0]);
        string name = strArray[1];
        float crossFadeTime = float.Parse(strArray[2]);
        float volume = float.Parse(strArray[3]);
        if (key1 > 0)
        {
          this.allFieldElements[key1].GetComponent<AudioManager>().Play(name, crossFadeTime, volume, true);
        }
        else
        {
          int key2 = key1 * -1;
          if (this.players.ContainsKey(key2))
          {
            GameObject avatar = this.players[key2].avatar;
            if ((bool) (UnityEngine.Object) avatar)
            {
              AudioManager componentInChildren = avatar.GetComponentInChildren<AudioManager>();
              if ((bool) (UnityEngine.Object) componentInChildren)
                componentInChildren.Play(name, crossFadeTime, volume, true);
            }
          }
        }
      }
    }
  }

  private void soundStop(string message)
  {
    string str1 = message;
    char[] chArray1 = new char[1]{ '\u0014' };
    foreach (string str2 in str1.Split(chArray1))
    {
      char[] chArray2 = new char[1]{ '\u0015' };
      string[] strArray = str2.Split(chArray2);
      if (strArray.Length >= 3)
      {
        int key1 = int.Parse(strArray[0]);
        string name = strArray[1];
        float crossFadeTime = float.Parse(strArray[2]);
        if (key1 > 0)
        {
          this.allFieldElements[key1].GetComponent<AudioManager>().Stop(name, crossFadeTime, true);
        }
        else
        {
          int key2 = key1 * -1;
          if (this.players.ContainsKey(key2))
          {
            GameObject avatar = this.players[key2].avatar;
            if ((bool) (UnityEngine.Object) avatar)
            {
              AudioManager componentInChildren = avatar.GetComponentInChildren<AudioManager>();
              if ((bool) (UnityEngine.Object) componentInChildren)
                componentInChildren.Stop(name, crossFadeTime, true);
            }
          }
        }
      }
    }
  }

  private void SpawnPlayer(
    string playerName,
    string cnnIDraw,
    string robotmodel,
    string RobotPosition,
    string skins = "0",
    string robot_skin = "")
  {
    int result = -1;
    if (!int.TryParse(cnnIDraw, out result))
      MyUtils.LogMessageToFile("SpawnPlayer had invalid cnnIDraw " + cnnIDraw);
    if (RobotPosition == "Spectator" && !ClientLow.ourPlayerName.Equals(playerName))
      return;
    GameObject gameObject = GameObject.Find(RobotPosition);
    Vector3 start_pos = Vector3.zero;
    Quaternion start_rot = Quaternion.identity;
    if ((bool) (UnityEngine.Object) gameObject)
    {
      start_pos = gameObject.transform.position;
      start_rot = gameObject.transform.rotation;
    }
    GameObject inobject;
    try
    {
      inobject = MyUtils.InstantiateRobot(robotmodel, start_pos, start_rot, skins, robot_skin);
    }
    catch (Exception ex)
    {
      this.ShowMessage("Missing robot model for player " + playerName + ". Can't show player!");
      return;
    }
    foreach (interpolation componentsInChild in inobject.GetComponentsInChildren<interpolation>())
    {
      if ((UnityEngine.Object) componentsInChild != (UnityEngine.Object) null)
        componentsInChild.enabled = true;
    }
    ClientLow.Player player = new ClientLow.Player()
    {
      avatar = inobject,
      robot = inobject.GetComponent<RobotInterface3D>(),
      playerName = playerName,
      model = robotmodel,
      position = RobotPosition,
      skins = skins,
      robot_skins = robot_skin,
      connectionId = result,
      messageId = 0,
      isRed = RobotPosition.StartsWith("Red"),
      isSpectator = RobotPosition == "Spectator"
    };
    if ((bool) (UnityEngine.Object) player.robot)
    {
      player.robot.SetName(playerName);
      player.robot.SetKinematic();
      if (RobotPosition == "Spectator")
      {
        player.robot.isSpectator = true;
        player.robot.Initialize();
        this.spec_myRobot = (Robot_Spectator) player.robot;
      }
      else
      {
        this.TurnOnInterpolationInObject(inobject);
        RobotID robotId = inobject.AddComponent<RobotID>();
        robotId.id = result;
        robotId.starting_pos = RobotPosition;
        player.robot.Initialize();
        player.robot.SetColorFromPosition(RobotPosition);
      }
    }
    this.players.Add(result, player);
    this.players_changed = true;
    if (ClientLow.ourPlayerName.Equals(playerName))
    {
      this.ourClientId = result;
      this.myPlayer = inobject;
      this.myPosition = RobotPosition;
      this.myRobot_working = player.robot;
      this.myRobot_saved = player.robot;
      this.SetCamera(RobotPosition);
      if ((bool) (UnityEngine.Object) player.robot)
      {
        GLOBALS.I_AM_SPECTATOR = player.robot.isSpectator;
        if ((bool) (UnityEngine.Object) player.robot.myRobotID)
          GLOBALS.I_AM_RED = player.robot.myRobotID.is_red;
      }
    }
    foreach (BoxCollider componentsInChild in inobject.GetComponentsInChildren<BoxCollider>())
    {
      if (componentsInChild.gameObject.layer == GLOBALS.LAYER_RobotBoundry)
      {
        Rigidbody rigidbody = componentsInChild.gameObject.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        rigidbody.detectCollisions = true;
        rigidbody.WakeUp();
      }
    }
    this.scorer.FieldChanged();
  }

  private void SpawnSpectator()
  {
    if ((bool) (UnityEngine.Object) this.spec_myRobot)
      return;
    GameObject gameObject1 = GameObject.Find("Spectator");
    Vector3 start_pos = Vector3.zero;
    Quaternion start_rot = Quaternion.identity;
    if ((bool) (UnityEngine.Object) gameObject1)
    {
      start_pos = gameObject1.transform.position;
      start_rot = gameObject1.transform.rotation;
    }
    GameObject gameObject2;
    try
    {
      gameObject2 = MyUtils.InstantiateRobot("AvatarSpectator", start_pos, start_rot);
    }
    catch (Exception ex)
    {
      this.ShowMessage("Missing spectaqtor robot model!");
      return;
    }
    foreach (interpolation componentsInChild in gameObject2.GetComponentsInChildren<interpolation>())
    {
      if ((UnityEngine.Object) componentsInChild != (UnityEngine.Object) null)
        componentsInChild.enabled = true;
    }
    Robot_Spectator component = gameObject2.GetComponent<Robot_Spectator>();
    component.isSpectator = true;
    component.SetKinematic();
    component.Initialize();
    this.spec_myPlayer = gameObject2;
    this.spec_myRobot = component;
  }

  public bool isSpectator() => this.myPosition == "Spectator";

  private void PlayerDisconnected(int cnnId)
  {
    if ((bool) (UnityEngine.Object) this.players[cnnId].robot)
      this.players[cnnId].robot.deleted = true;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.players[cnnId].avatar);
    this.players[cnnId].robot = (RobotInterface3D) null;
    this.players.Remove(cnnId);
    this.players_changed = true;
  }

  private IEnumerator FinishAutosave()
  {
    while (MyUtils.PB_AutoSaveInProgress())
    {
      MyUtils.PB_AutoSaveToFile();
      yield return (object) null;
    }
    this.autosave_enumerator = (IEnumerator) null;
  }

  private void ProcessServerFlags()
  {
    if (!this.playbackmode && this.ourClientId == -1)
      return;
    this.big_message.SetActive(false);
    this.holding_mode = false;
    this.IsAdmin = this.myStartfield == "Admin";
    foreach (string key in this.serverFlags.Keys)
    {
      switch (key)
      {
        case "ADMIN":
          this.IsAdmin = int.Parse(this.serverFlags["ADMIN"]) == this.ourClientId || this.myStartfield == "Admin";
          continue;
        case "CHAT":
          this.ShowMessage(this.serverFlags[key]);
          continue;
        case "COUNTDOWN":
          this.scorer.StartCountdown();
          continue;
        case "FIREWORKS":
          this.scorer.StartFireworks(this.serverFlags["FIREWORKS"] == "RED");
          MyUtils.PB_AutoSaveToFile();
          this.autosave_enumerator = this.FinishAutosave();
          this.StartCoroutine(this.autosave_enumerator);
          continue;
        case "GameSettings":
          if (!(bool) (UnityEngine.Object) this.ourgamesettings)
            this.ourgamesettings = UnityEngine.Object.FindObjectOfType<GameSettings>();
          if ((bool) (UnityEngine.Object) this.ourgamesettings)
            this.ourgamesettings.SetString(this.serverFlags[key]);
          if (this.first_time_game_settings)
          {
            this.scorer.ScorerReset();
            this.first_time_game_settings = false;
            continue;
          }
          continue;
        case "HOLDING":
          if (this.myPosition != "Spectator")
          {
            this.big_message.GetComponent<UnityEngine.UI.Text>().text = "WAITING FOR TOURNAMENT TO START...";
            this.big_message.SetActive(true);
          }
          this.holding_mode = true;
          continue;
        case "RESTARTALL":
          this.clientFlags.Clear();
          this.FlagReset();
          this.ShowMessage("Field reset by " + this.serverFlags[key]);
          continue;
        case "SCORER":
          this.scorer.OnScorerInterrupt(this.serverFlags[key]);
          continue;
        case "SOUND_PLAY":
          this.soundStart(this.serverFlags[key]);
          continue;
        case "SOUND_STOP":
          this.soundStop(this.serverFlags[key]);
          continue;
        case "TM_ANIM1":
          if (this.serverFlags[key][0] == '1')
          {
            this.scorer.ShowGameStartOverlay();
            continue;
          }
          this.scorer.ShowGameStartOverlay(false);
          continue;
        case "TM_ANIM2":
          if (this.serverFlags[key][0] == '1')
          {
            this.scorer.ShowGameStartOverlay(passivemode: true);
            continue;
          }
          this.scorer.ShowGameStartOverlay(false, true);
          continue;
        case "TM_STATE":
          this.ProcessTournamentMode();
          continue;
        default:
          continue;
      }
    }
    this.ToggleChampsMode();
    this.serverFlags.Remove("RESTARTALL");
    this.serverFlags.Remove("CHAT");
    this.serverFlags.Remove("SOUND_PLAY");
    this.serverFlags.Remove("SOUND_STOP");
    this.serverFlags.Remove("COUNTDOWN");
    this.serverFlags.Remove("FIREWORKS");
    this.serverFlags.Remove("GameSettings");
    this.serverFlags.Remove("SCORER");
  }

  private void ProcessTournamentMode()
  {
    string str1 = "NO";
    string str2 = "";
    if (!this.serverFlags.TryGetValue("TM_STATE", out str1))
    {
      this.Tournament_top.SetActive(false);
    }
    else
    {
      this.serverFlags.TryGetValue("TM_MSG", out str2);
      UnityEngine.UI.Text component = this.Tournament_msg.GetComponent<UnityEngine.UI.Text>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        return;
      component.text = str2;
      component.enabled = true;
      if (!(str1 == "NO"))
      {
        if (!(str1 == "WAITING"))
        {
          if (str1 == "COUNTDOWN" || str1 == "RUNNING" || str1 == "END")
            ;
          this.clientFlags["READY"] = "";
          this.Tournament_ready.GetComponent<Toggle>().isOn = false;
          this.Tournament_ready.SetActive(false);
        }
        else
        {
          this.Tournament_ready.SetActive(true);
          this.clientFlags["READY"] = !this.Tournament_ready.GetComponent<Toggle>().isOn ? "" : "READY";
        }
        this.Tournament_top.SetActive(true);
      }
      else
        this.Tournament_top.SetActive(false);
    }
  }

  public void DisableChampsMode()
  {
    if ((bool) (UnityEngine.Object) this.ChampsObject)
      this.ChampsObject.SetActive(false);
    this.no_champs_exists = true;
  }

  public void ToggleChampsMode()
  {
    if (this.no_champs_exists)
      return;
    if (this.serverFlags.ContainsKey("CHAMPS"))
    {
      if (!(bool) (UnityEngine.Object) this.ChampsObject)
      {
        ChampsInit[] objectsOfTypeAll = UnityEngine.Resources.FindObjectsOfTypeAll<ChampsInit>();
        if (objectsOfTypeAll.Length < 1)
        {
          this.no_champs_exists = true;
          return;
        }
        this.ChampsObject = objectsOfTypeAll[0].gameObject;
      }
      if (this.ChampsObject.activeSelf)
        return;
      this.ChampsObject.SetActive(true);
      foreach (TextMeshPro componentsInChild in this.ChampsObject.GetComponentsInChildren<TextMeshPro>())
      {
        if (componentsInChild.name.StartsWith("UserText"))
          componentsInChild.text = this.serverFlags["CHAMPS"];
      }
    }
    else
    {
      if (!(bool) (UnityEngine.Object) this.ChampsObject || !this.ChampsObject.activeSelf)
        return;
      this.ChampsObject.SetActive(false);
    }
  }

  private void DataReceived(IAsyncResult ar)
  {
    if (ClientLow.killme || this.connection_state == ClientLow.ConnectionStates.LOST)
      return;
    UdpClient asyncState = (UdpClient) ar.AsyncState;
    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
    try
    {
      byte[] numArray = asyncState.EndReceive(ar, ref remoteEP);
      if (this.allReceivedDataSemaphore.WaitOne())
      {
        this.allReceivedData.Add(numArray);
        this.allReceivedDataSemaphore.Release();
      }
    }
    catch (Exception ex)
    {
      if (this.datareceivederrors < 10)
        MyUtils.LogMessageToFile("DR Exception: " + ex.ToString(), false);
      ++this.datareceivederrors;
    }
    if (ClientLow.killme)
      return;
    asyncState.BeginReceive(new AsyncCallback(this.DataReceived), ar.AsyncState);
  }

  private void DataReceive()
  {
    while (!ClientLow.killme && this.connection_state != ClientLow.ConnectionStates.LOST)
    {
      int millisecondsTimeout = 0;
      byte[] numArray;
      try
      {
        this.udpSemaphore.WaitOne();
        if (this.m_udpClient.Available < 1)
        {
          millisecondsTimeout = 1;
          continue;
        }
        numArray = this.m_udpClient.Receive(ref this.receiverEP);
      }
      catch (Exception ex)
      {
        MyUtils.LogMessageToFile("DR GLOBAL udpClient Exception: " + ex.ToString(), false);
        this.ShowMessage("DR GLOBAL udpClient Exception: " + ex.ToString());
        continue;
      }
      finally
      {
        this.udpSemaphore.Release();
      }
      if (millisecondsTimeout > 0)
      {
        Thread.Sleep(millisecondsTimeout);
      }
      else
      {
        this.time_last_packet = ClientLow.GetTimeMillis();
        this.allReceivedDataSemaphore.WaitOne();
        this.allReceivedData.Add(numArray);
        this.allReceivedDataSemaphore.Release();
        Thread.Sleep(0);
      }
    }
  }

  private void DataReceiveSerial()
  {
    if (ClientLow.killme || this.m_udpClient == null)
      return;
    while (this.m_udpClient.Available > 10)
    {
      IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
      try
      {
        this.allReceivedData.Add(this.m_udpClient.Receive(ref remoteEP));
        this.time_last_packet = ClientLow.GetTimeMillis();
      }
      catch (Exception ex)
      {
        if (this.datareceivederrors > 10)
        {
          MyUtils.LogMessageToFile("Error while receiving: " + (object) ex);
          ++this.datareceivederrors;
        }
      }
    }
  }

  public void sendUdpData(string messageraw)
  {
    byte[] second = MyUtils.CompressMessage(messageraw);
    byte[] dgram = MyUtils.CombineByteArrays(Encoding.UTF8.GetBytes("7p0\u0011" + 1.ToString() + "\u0011" + (object) this.ourClientId + "\u0011" + second.Length.ToString() + "\u0011"), second);
    try
    {
      this.udpSemaphore.WaitOne();
      this.m_udpClient.Send(dgram, dgram.Length);
    }
    catch (Exception ex)
    {
      MyUtils.LogMessageToFile("exception in sending!! " + (object) ex);
    }
    finally
    {
      this.udpSemaphore.Release();
    }
  }

  public bool SendFlagsUDPTracked(string messageraw)
  {
    ++this.curr_tracked_packet;
    this.client_sent_packets[this.curr_tracked_packet] = this.curr_tracked_packet.ToString() + "\u0012" + this.curr_tracked_packet.ToString() + "\u0012" + messageraw;
    if (this.server_confirmed_this_msg_id < 0)
      this.server_confirmed_this_msg_id = this.curr_tracked_packet - 1;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("FLG\u0011");
    bool flag = false;
    int key1 = this.server_confirmed_this_msg_id + 1;
    if (this.curr_tracked_packet - key1 > 30)
      key1 = this.curr_tracked_packet - 30 + 1;
    for (; key1 <= this.curr_tracked_packet; ++key1)
    {
      if (flag)
        stringBuilder.Append('\u0013');
      else
        flag = true;
      stringBuilder.Append(this.client_sent_packets[key1] + "\u0012TRACKED_FLAG_ID\u0012" + (object) this.client_confirmed_this_msg_id);
    }
    this.sendUdpData(stringBuilder.ToString());
    int num = this.server_confirmed_this_msg_id + 1;
    if (this.curr_tracked_packet - num > 30)
      num = this.curr_tracked_packet - 30 + 1;
    int key2 = num - 1;
    while (key2 > 0 && this.client_sent_packets.ContainsKey(key2))
    {
      this.client_sent_packets.Remove(key2);
      --key1;
    }
    return true;
  }

  private void DEBUG_print_flags_udp(string message)
  {
    MyUtils.LogMessageToFile("CS: Server last received: server_confirmed_this_msg_id = " + (object) this.server_confirmed_this_msg_id);
    MyUtils.LogMessageToFile("CS: Client current id: curr_tracked_packet = " + (object) this.curr_tracked_packet);
    MyUtils.LogMessageToFile("SC: Client last received: client_confirmed_this_msg_id = " + (object) this.client_confirmed_this_msg_id);
    MyUtils.LogMessageToFile("Fifo buffer size: server_sent_packets = " + (object) this.client_sent_packets.Count);
    MyUtils.LogMessageToFile("  curr_tracked_packet = " + (object) this.curr_tracked_packet);
    string str1 = message.Split('\u0011')[1];
    char[] chArray1 = new char[1]{ '\u0013' };
    foreach (string str2 in str1.Split(chArray1))
    {
      char[] chArray2 = new char[1]{ '\u0012' };
      string[] strArray1 = str2.Split(chArray2);
      int result;
      int.TryParse(strArray1[0], out result);
      MyUtils.LogMessageToFile("    MSG ID: " + (object) result);
      int num1 = 1;
      while (num1 < strArray1.Length)
      {
        string[] strArray2 = strArray1;
        int index1 = num1;
        int num2 = index1 + 1;
        string str3 = strArray2[index1];
        string[] strArray3 = strArray1;
        int index2 = num2;
        num1 = index2 + 1;
        string str4 = strArray3[index2];
        MyUtils.LogMessageToFile("       KEY[" + str3 + "]=" + str4);
      }
    }
  }

  private bool startUdp()
  {
    this.m_udpClient = new UdpClient();
    try
    {
      this.receiverEP = new IPEndPoint(IPAddress.Parse(ClientLow.serverIP), ClientLow.serverPORT);
      this.m_udpClient.Connect(this.receiverEP);
    }
    catch (Exception ex)
    {
      this.ShowMessage("ERROR: " + (object) ex);
      MyUtils.LogMessageToFile("UDP Start Error: " + (object) ex);
      return false;
    }
    this.m_udpClient.DontFragment = false;
    return true;
  }

  private void ResetAllStates()
  {
    this.latest_message_id = 0;
    this.last_checked_message_id = 0;
    this.received_msg_count = 0;
    this.last_update_time = 0L;
    this.id_current_players = -1;
    this.id_server_error = -1;
    this.client_confirmed_this_msg_id = 0;
    this.found_position = false;
    this.holding_mode = false;
    this.connection_state = ClientLow.ConnectionStates.NOTSTARTED;
    this.time_started = -1L;
    this.ourClientId = -1;
    this.curr_tracked_packet = 1;
    this.server_confirmed_this_msg_id = -1;
    this.myStartfield = "";
    this.clientFlags.Clear();
    this.allReceivedData.Clear();
    this.serverFlags.Clear();
    this.client_sent_packets.Clear();
    MyUtils.CloseScorefiles();
    if (this.players.Count > 0)
    {
      foreach (int key in this.players.Keys)
      {
        if ((bool) (UnityEngine.Object) this.players[key].robot)
          this.players[key].robot.deleted = true;
        UnityEngine.Object.Destroy((UnityEngine.Object) this.players[key].avatar);
      }
      this.players.Clear();
      this.players_changed = true;
    }
    if ((UnityEngine.Object) this.myPlayer != (UnityEngine.Object) null)
    {
      if ((bool) (UnityEngine.Object) this.myRobot_working)
        this.myRobot_working.deleted = true;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.myPlayer);
      this.myPlayer = (GameObject) null;
    }
    this.myRobot_working = (RobotInterface3D) null;
    if (this.m_udpClient != null)
    {
      this.m_udpClient.Close();
      this.m_udpClient.Dispose();
      this.m_udpClient = (UdpClient) null;
    }
    foreach (GameObject gameObject in this.allFieldElements.Values)
    {
      if ((bool) (UnityEngine.Object) gameObject && (bool) (UnityEngine.Object) gameObject.GetComponent<gameElement>())
        gameObject.GetComponent<gameElement>().lastupdateID = -1;
    }
  }

  public void ChangeGameSettings(string newsettings)
  {
    if (!this.IsAdmin)
      return;
    this.clientFlags["GameSettings"] = newsettings;
  }

  private class LogLine
  {
    public GameObject TextLine;
    public long time_of_message;
  }

  public enum ConnectionStates
  {
    NOTSTARTED,
    CONNECTING,
    CONNECTED,
    LOST,
  }

  public class Player
  {
    public string playerName;
    public string skins;
    public string robot_skins;
    public string model;
    public string position;
    public GameObject avatar;
    public RobotInterface3D robot;
    public int connectionId;
    public bool isRed;
    public bool isSpectator;
    public int messageId;
  }
}
