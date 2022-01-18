// Decompiled with JetBrains decompiler
// Type: ServerLow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ServerLow : MonoBehaviour
{
  public Scorekeeper scorer;
  public string SERVER_COMMENT = "";
  public bool REGISTER_SERVER;
  public int ROUTER_PORT = 11115;
  public int PORT = 11115;
  public string PASSWORD = "";
  public int max_spectators = 4;
  public bool tournament_mode;
  public bool holding_mode;
  public bool start_when_ready = true;
  public long UPDATE_DELAY = 25;
  public long MAX_BYTES = 6000;
  public string ADMIN = "";
  public bool CHAMPS_MODE;
  public string CHAMPS_TEXT = "SRC Championship";
  private ServerLow.TOURNAMENT_STATES tournament_sm;
  private Dictionary<string, int> netmonitor = new Dictionary<string, int>();
  private string netmonitor_header;
  private Dictionary<string, ServerLow.CacheString> netcache = new Dictionary<string, ServerLow.CacheString>();
  private Dictionary<string, ServerLow.CacheString> netcache_spectators = new Dictionary<string, ServerLow.CacheString>();
  private SortedDictionary<int, GameObject> allFieldElements = new SortedDictionary<int, GameObject>();
  private GameSettings ourgamesettings;
  private GameObject redtextobj;
  private GameObject bluetextobj;
  private GameObject messageBox;
  private long time_of_message;
  private StringBuilder[] msg_lines = new StringBuilder[50];
  private StringBuilder textmsg = new StringBuilder();
  public GameObject clientlineprefab;
  private GameObject messageLog;
  private List<ServerLow.LogLine> allmessages = new List<ServerLow.LogLine>();
  private static bool field_load = false;
  private static bool elements_load = false;
  private static bool scorer_load = false;
  private static bool gui_load = false;
  public static bool configuration_done = false;
  private int fix_was_run = 60;
  private static ServerLow thisInstance;
  private long lastSendingTime;
  private string debug_msg1 = "";
  private string debug_msg2 = "";
  private bool full_cycle_done;
  private bool second_load;
  public long sent_data_count;
  public float sent_data_count_time;
  public float data_rate;
  public int framecount;
  public int framerate;
  public int framecount_phys;
  public int framerate_phys;
  public int update_framerate;
  public int update_framecount;
  private bool tm_holding_update;
  private float saved_fixedtimestep;
  private long last_flag_time;
  public long time_last_seen_players;
  private static bool killme = false;
  private Thread thread_incoming;
  public bool serverReady;
  private long tm_timer;
  private bool tournament_force_start;
  private bool game_overlay_turned_off;
  private GameObject ChampsObject;
  private long time_last_count_check = -1;
  private long time_cache_clear = -1;
  private bool cache_cleared;
  private string last_message;
  private static int message_id = 1;
  private static int message_id_tracked_players = 1;
  private static int message_id_tracked_other = 1;
  private Dictionary<string, string> serverFlags = new Dictionary<string, string>();
  private Dictionary<string, string> serverInterrupts = new Dictionary<string, string>();
  private int items_cached;
  private string msg_test_case = "";
  private long registration_time = -1;
  private List<ServerLow.Message> allReceivedData = new List<ServerLow.Message>();
  private List<ServerLow.Message> serverInfoRequests = new List<ServerLow.Message>();
  private System.Threading.Semaphore allReceivedDataSemaphore = new System.Threading.Semaphore(1, 1);
  private Dictionary<int, ServerLow.Client> allClients = new Dictionary<int, ServerLow.Client>();
  private int clientIDcounter;
  private Dictionary<int, float> pos_reset_tracker = new Dictionary<int, float>();
  public ApplicationManager top_application_manager;
  private int datareceivederrors;
  private System.Threading.Semaphore udpSemaphore = new System.Threading.Semaphore(1, 1);
  public UdpClient m_udpClient;
  public IPEndPoint remoteEP;
  public long datacounter_old;
  public long datacounter;
  public long udp_sleep_lastime;
  public float UDP_SLEEP = GLOBALS.UDP_DELAY_TIME_MS;
  private int time_day_count;
  private Dictionary<string, long> stats_outgoing = new Dictionary<string, long>();
  private long STATS_PERIOD = 60000;
  private long STATS_LAST_TIME;
  private static System.Random rng = new System.Random();

  private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
  {
    if (scene.name == "fieldElements")
      ServerLow.elements_load = true;
    if (scene.name == "field")
    {
      ServerLow.field_load = true;
      SceneManager.SetActiveScene(scene);
    }
    if (scene.name == "Scoring")
      ServerLow.scorer_load = true;
    if (scene.name == "server_gui")
      ServerLow.gui_load = true;
    if (!ServerLow.elements_load || !ServerLow.field_load || !ServerLow.scorer_load || !ServerLow.gui_load || ServerLow.configuration_done)
      return;
    this.messageLog = GameObject.Find("MessageLogText");
    this.allFieldElements.Clear();
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("GameElement"))
    {
      gameElement component = gameObject.GetComponent<gameElement>();
      if (this.allFieldElements.ContainsKey(component.id))
      {
        Debug.Log((object) ("Field element " + (object) component.id + " is not unique id."));
      }
      else
      {
        this.allFieldElements.Add(component.id, gameObject);
        if ((bool) (UnityEngine.Object) gameObject.GetComponent<Rigidbody>())
          gameObject.GetComponent<Rigidbody>().sleepThreshold = 0.0f;
      }
    }
    this.ConfigureElements();
    this.scorer = GameObject.Find("Scorekeeper").GetComponent<Scorekeeper>();
    this.scorer.ScorerReset();
    ServerLow.configuration_done = true;
    this.ourgamesettings = UnityEngine.Object.FindObjectOfType<GameSettings>();
    if ((bool) (UnityEngine.Object) this.ourgamesettings)
      this.ourgamesettings.Init();
    if (GLOBALS.HEADLESS_MODE)
    {
      this.ProcessHeadlessCommands();
      this.ServerStartHeadless();
    }
    foreach (Component component in UnityEngine.Resources.FindObjectsOfTypeAll<Camera>())
      MyUtils.SetCameraQualityLevel(component.gameObject);
    MyUtils.QualityLevel_DisableObjects();
  }

  public void ProcessCommand(string command, string value)
  {
    string[] strArray = new string[2]{ command, value };
    if (strArray[0].ToLower().Equals("tmode"))
    {
      if (strArray.Length < 2)
      {
        MyUtils.LogMessageToFile("TMODE incorrectly specified. Specify as TMODE=On/Off.");
        return;
      }
      if (strArray[1].ToLower().Equals("on"))
      {
        MyUtils.LogMessageToFile("Tournament mode enabled.", false);
        this.tournament_mode = true;
      }
      else
      {
        MyUtils.LogMessageToFile("Tournament mode disabled.", false);
        this.tournament_mode = false;
      }
    }
    else if (strArray[0].ToLower().Equals("startwhenready"))
    {
      if (strArray.Length < 2)
      {
        MyUtils.LogMessageToFile("STARTWHENREADY incorrectly specified. Specify as STARTWHENREADY=On/Off");
        return;
      }
      if (strArray[1].ToLower().Equals("on"))
      {
        MyUtils.LogMessageToFile("Start-when-ready mode enabled.", false);
        this.start_when_ready = true;
      }
      else
      {
        MyUtils.LogMessageToFile("Start-when-ready mode disabled.", false);
        this.start_when_ready = false;
      }
    }
    else if (strArray[0].ToLower().Equals("holdingmode"))
    {
      if (strArray.Length < 2)
      {
        MyUtils.LogMessageToFile("HOLDINGMODE incorrectly specified. Specify as HOLDINGMODE=On/Off");
        return;
      }
      if (strArray[1].ToLower().Equals("on"))
      {
        MyUtils.LogMessageToFile("Holding mode enabled.", false);
        this.holding_mode = true;
      }
      else
      {
        MyUtils.LogMessageToFile("Holding mode disabled.", false);
        this.holding_mode = false;
      }
    }
    else if (strArray[0].ToLower().Equals("gameoption"))
    {
      if (strArray.Length < 2)
      {
        MyUtils.LogMessageToFile("GAMEOPTION incorrectly specified. Specify as GAMEOPTION=#");
        return;
      }
      int result = 0;
      if (!int.TryParse(strArray[1], out result))
      {
        MyUtils.LogMessageToFile("GAMEOPTION incorrectly specified - unable to parse integer after GAMEOPTION= command line. Specify as GAMEOPTION=# where # starts from 1 to 5.");
        return;
      }
      if (result < 1 || result >= 5)
      {
        MyUtils.LogMessageToFile("GAMEOPTION number out of range. Starting game # = 1 and last game # is 5. You specified " + (object) result + ".");
        return;
      }
      GLOBALS.game_option = result;
      MyUtils.LogMessageToFile("GAMEOPTION set to " + (object) result, false);
    }
    else if (strArray[0].ToLower().Equals("password"))
    {
      if (strArray.Length < 2)
      {
        MyUtils.LogMessageToFile("PASSWORD incorrectly specified. Specify as PASWORD=XXXXX, where X is an aplhanumeric with no spaces or = signs.");
        return;
      }
      this.PASSWORD = strArray[1];
      MyUtils.LogMessageToFile("PASSWORD set to " + this.PASSWORD, false);
    }
    if (strArray[0].ToLower().Equals("admin"))
    {
      if (strArray.Length < 2)
      {
        MyUtils.LogMessageToFile("ADMIN incorrectly specified. Specify as ADMIN=XXXXX, where X is an aplhanumeric with no spaces or = signs.");
        return;
      }
      this.ADMIN = strArray[1];
      MyUtils.LogMessageToFile("ADMIN set to " + this.ADMIN, false);
    }
    if (strArray[0].ToLower().Equals("spectators"))
    {
      if (strArray.Length < 2)
      {
        MyUtils.LogMessageToFile("SPECTATORS incorrectly specified. Specify as SPECTATORS=#");
        return;
      }
      int result = 0;
      if (!int.TryParse(strArray[1], out result))
      {
        MyUtils.LogMessageToFile("SPECTATORS incorrectly specified - unable to parse integer after SPECTATORS= command line. Specify as SPECTATORS=# where # starts from 0.");
        return;
      }
      if (result < 0)
      {
        MyUtils.LogMessageToFile("SPECTATORS number out of range, must be >0. You specified " + (object) result + ".");
        return;
      }
      this.max_spectators = result;
      MyUtils.LogMessageToFile("Max spectators set to " + (object) result, false);
    }
    else if (strArray[0].ToLower().Equals("routerport"))
    {
      if (strArray.Length < 2)
      {
        MyUtils.LogMessageToFile("ROUTERPORT incorrectly specified. Specify as ROUTERPORT=#####");
        return;
      }
      int result = 0;
      if (!int.TryParse(strArray[1], out result))
      {
        MyUtils.LogMessageToFile("ROUTERPORT incorrectly specified - unable to parse integer after ROUTERPORT= command line. Specify as ROUTERPORT=# where # starts from 1.");
        return;
      }
      if (result < 1 || result >= (int) ushort.MaxValue)
      {
        MyUtils.LogMessageToFile("ROUTERPORT number out of range. Starting port # = 1 and last port # is 65535. You specified " + (object) result + ".");
        return;
      }
      this.ROUTER_PORT = result;
      MyUtils.LogMessageToFile("Router port set to " + (object) result, false);
    }
    else if (strArray[0].ToLower().Equals("port"))
    {
      if (strArray.Length < 2)
      {
        MyUtils.LogMessageToFile("PORT incorrectly specified. Specify as PORT=#####");
        return;
      }
      int result = 0;
      if (!int.TryParse(strArray[1], out result))
      {
        MyUtils.LogMessageToFile("PORT incorrectly specified - unable to parse integer after PORT= command line. Specify as PORT=# where # starts from 1.");
        return;
      }
      if (result < 1 || result >= (int) ushort.MaxValue)
      {
        MyUtils.LogMessageToFile("PORT number out of range. Starting port # = 1 and last port # is 65535. You specified " + (object) result + ".");
        return;
      }
      this.PORT = result;
      MyUtils.LogMessageToFile("Port set to " + (object) result, false);
    }
    else if (strArray[0].ToLower().Equals("register"))
    {
      if (strArray.Length < 2)
      {
        MyUtils.LogMessageToFile("Register incorrectly specified. Specify as REGISTER=On/Off");
        return;
      }
      if (strArray[1].ToLower().Equals("on"))
      {
        MyUtils.LogMessageToFile("Register server enabled.", false);
        this.REGISTER_SERVER = true;
      }
      else
      {
        MyUtils.LogMessageToFile("Register server disabled.", false);
        this.REGISTER_SERVER = false;
      }
    }
    else if (strArray[0].ToLower().Equals("comment"))
    {
      if (strArray.Length < 2)
      {
        MyUtils.LogMessageToFile("Comment incorrectly specified. Specify as Comment=xxxxx");
        return;
      }
      MyUtils.LogMessageToFile("Server comment field set to " + strArray[1], false);
      this.SERVER_COMMENT = strArray[1];
    }
    else if (strArray[0].ToLower().Equals("updatetime"))
    {
      if (strArray.Length < 2)
      {
        MyUtils.LogMessageToFile("UPDATETIME incorrectly specified. Specify as UPDATETIME=##");
        return;
      }
      int result = 0;
      if (!int.TryParse(strArray[1], out result))
      {
        MyUtils.LogMessageToFile("UPDATETIME incorrectly specified - unable to parse integer after UPDATETIME= command line. Specify as UPDATETIME=# where # starts from 1.");
        return;
      }
      if (result < 1 || result >= 1000)
      {
        MyUtils.LogMessageToFile("UPDATEIME number out of range. Min time # = 1 and max port # is 1000 ms. You specified " + (object) result + ".");
        return;
      }
      this.UPDATE_DELAY = (long) result;
      MyUtils.LogMessageToFile("Update time set to " + (object) result, false);
    }
    else if (strArray[0].ToLower().Equals("maxdata"))
    {
      if (strArray.Length < 2)
      {
        MyUtils.LogMessageToFile("MAXDATA incorrectly specified. Specify as MAXDATA=####");
        return;
      }
      int result = 0;
      if (!int.TryParse(strArray[1], out result))
      {
        MyUtils.LogMessageToFile("MAXDATA incorrectly specified - unable to parse integer after MAXDATA= command line. Specify as MAXDATA=#### where # starts from 1.");
        return;
      }
      if (result < 1)
      {
        MyUtils.LogMessageToFile("MAXDATA number out of range. Min time # = 1. You specified " + (object) result + ".");
        return;
      }
      this.MAX_BYTES = (long) result;
      MyUtils.LogMessageToFile("Max data set to " + (object) result, false);
    }
    else if (strArray[0].ToLower().Equals("netstats"))
    {
      if (strArray.Length < 2)
      {
        MyUtils.LogMessageToFile("Netstats incorrectly specified. Specify as REGISTER=On/Off");
        return;
      }
      if (strArray[1].ToLower().Equals("on"))
      {
        MyUtils.LogMessageToFile("Netstats enabled.", false);
        GLOBALS.NETSTATS = 1;
      }
      else if (strArray[1].ToLower().Equals("admin"))
      {
        MyUtils.LogMessageToFile("Netstats in admin mode.", false);
        GLOBALS.NETSTATS = 2;
      }
      else
      {
        MyUtils.LogMessageToFile("Netstats disabled.", false);
        GLOBALS.NETSTATS = 0;
      }
    }
    if (strArray[0].ToLower().Equals("champs"))
    {
      if (strArray.Length < 2)
      {
        MyUtils.LogMessageToFile("Championship mode requires one argument to set the field text.");
        return;
      }
      this.CHAMPS_MODE = true;
      this.CHAMPS_TEXT = strArray[1];
      this.ToggleChampsMode();
      MyUtils.LogMessageToFile("Champs mode enabled with text = " + this.CHAMPS_TEXT, false);
    }
    else if (strArray[0].ToLower().Equals("output_score_files"))
    {
      if (strArray.Length < 2)
      {
        MyUtils.LogMessageToFile("OUTPUT_SCORE_FILES needs a directory destination.");
        return;
      }
      GLOBALS.OUTPUT_SCORING_FILES = true;
      MyUtils.status_file_dir = strArray[1];
      MyUtils.LogMessageToFile("Score files output set to directory " + strArray[1], false);
    }
    if (!strArray[0].ToLower().Equals("gamesettings"))
      return;
    if (strArray.Length < 2)
    {
      MyUtils.LogMessageToFile("GAMESETTINGS requires one argument to set the options.");
    }
    else
    {
      if ((bool) (UnityEngine.Object) this.ourgamesettings)
        this.ourgamesettings.SetString(strArray[1]);
      MyUtils.LogMessageToFile("Game settings set to " + strArray[1], false);
    }
  }

  public void ProcessHeadlessCommands()
  {
    foreach (string commandLineArg in Environment.GetCommandLineArgs())
    {
      char[] chArray = new char[1]{ '=' };
      string[] strArray = commandLineArg.Split(chArray);
      this.ProcessCommand(strArray[0], strArray.Length > 1 ? strArray[1] : "");
    }
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

  private void TurnOffInterpolationInObject(GameObject inobject)
  {
    for (int index = 0; index < inobject.transform.childCount; ++index)
    {
      interpolation component = inobject.transform.GetChild(index).GetComponent<interpolation>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.enabled = false;
    }
    interpolation component1 = inobject.transform.GetComponent<interpolation>();
    if (!((UnityEngine.Object) component1 != (UnityEngine.Object) null))
      return;
    component1.enabled = false;
  }

  private void CorrectFieldElements()
  {
    foreach (GameObject currobj in this.allFieldElements.Values)
      this.scorer.CorrectFieldElement(currobj);
  }

  private void CorrectPlayers()
  {
    foreach (int key in this.allClients.Keys)
    {
      if (!((UnityEngine.Object) this.allClients[key].avatar == (UnityEngine.Object) null) && !((UnityEngine.Object) this.allClients[key].robot == (UnityEngine.Object) null) && !((UnityEngine.Object) this.allClients[key].robot.rb_body == (UnityEngine.Object) null))
      {
        if (this.scorer.IsTransformOffField(this.allClients[key].robot.rb_body.transform))
          this.ResetAvatarPosition(key);
        if (this.allClients[key].robot.GetNeedsReset())
        {
          this.allClients[key].reset_release = Time.time + this.allClients[key].robot.GetResetDuration();
          this.ResetAvatarPosition(key);
        }
        if ((double) this.allClients[key].reset_release > (double) Time.time)
          this.allClients[key].robot.HoldRobot();
        else if ((double) this.allClients[key].reset_release > 0.0)
        {
          this.allClients[key].robot.HoldRobot(false);
          this.allClients[key].reset_release = 0.0f;
        }
      }
    }
  }

  private void ResetAvatarPosition(int playerID)
  {
    if (!this.allClients.ContainsKey(playerID) || (UnityEngine.Object) this.allClients[playerID].avatar == (UnityEngine.Object) null)
      return;
    RobotID_Data refin = new RobotID_Data();
    refin.Copy(this.allClients[playerID].robot.myRobotID);
    if ((bool) (UnityEngine.Object) this.allClients[playerID].robot)
      this.allClients[playerID].robot.deleted = true;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.allClients[playerID].avatar);
    this.CreatePlayerAvatar(this.allClients[playerID]);
    this.allClients[playerID].robot.myRobotID.Copy(refin);
    this.scorer.FieldChanged();
  }

  private void RestartLevel()
  {
    this.scorer.ScorerReset();
    this.scorer.Restart();
    foreach (int key in this.allClients.Keys)
      this.ResetAvatarPosition(key);
    foreach (int key in this.allFieldElements.Keys)
    {
      gameElement component = this.allFieldElements[key].GetComponent<gameElement>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.ResetPosition(GLOBALS.game_option);
    }
    this.scorer.OnTimerReset();
    this.scorer.DoFieldChanged();
    this.scorer.OnTimerClick();
    UnityEngine.Resources.UnloadUnusedAssets();
  }

  public void UpdateScore()
  {
    if ((UnityEngine.Object) this.scorer == (UnityEngine.Object) null)
      return;
    this.scorer.SendServerData(this.serverFlags);
    if ((UnityEngine.Object) this.redtextobj == (UnityEngine.Object) null)
    {
      this.redtextobj = GameObject.Find("REDSCORE");
      if ((UnityEngine.Object) this.redtextobj == (UnityEngine.Object) null)
        return;
    }
    if ((UnityEngine.Object) this.bluetextobj == (UnityEngine.Object) null)
    {
      this.bluetextobj = GameObject.Find("BLUESCORE");
      if ((UnityEngine.Object) this.bluetextobj == (UnityEngine.Object) null)
        return;
    }
    this.redtextobj.GetComponent<UnityEngine.UI.Text>().text = this.scorer.GetRedScore().ToString();
    this.bluetextobj.GetComponent<UnityEngine.UI.Text>().text = this.scorer.GetBlueScore().ToString();
  }

  public void ShowMessage(string message, int starting_line = 0, bool clear_all = true)
  {
    if ((UnityEngine.Object) this.messageBox == (UnityEngine.Object) null)
    {
      this.messageBox = GameObject.Find("MessageBox");
      if ((UnityEngine.Object) this.messageBox == (UnityEngine.Object) null)
        return;
    }
    UnityEngine.UI.Text component = this.messageBox.GetComponent<UnityEngine.UI.Text>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    if (clear_all)
    {
      for (int index = 0; index < this.msg_lines.Length; ++index)
      {
        if (this.msg_lines[index] == null)
          this.msg_lines[index] = new StringBuilder();
        else
          this.msg_lines[index].Clear();
      }
    }
    string[] strArray = message.Split('\n');
    for (int index = 0; index < strArray.Length && starting_line + index < this.msg_lines.Length; ++index)
      this.msg_lines[index + starting_line].Clear().Append(strArray[index]);
    this.textmsg.Clear();
    int num = 0;
    for (int index1 = 0; index1 < this.msg_lines.Length; ++index1)
    {
      if (this.msg_lines[index1].Length < 1)
      {
        ++num;
      }
      else
      {
        for (int index2 = 0; index2 < num; ++index2)
          this.textmsg.Append("\n");
        num = 0;
        this.textmsg.AppendLine(this.msg_lines[index1].ToString());
      }
    }
    component.text = this.textmsg.ToString();
    component.enabled = true;
    this.time_of_message = ServerLow.GetTimeMillis();
  }

  public void ClearMessage()
  {
    if ((UnityEngine.Object) this.messageBox == (UnityEngine.Object) null)
    {
      this.messageBox = GameObject.Find("MessageBox");
      if ((UnityEngine.Object) this.messageBox == (UnityEngine.Object) null)
        return;
    }
    UnityEngine.UI.Text component = this.messageBox.GetComponent<UnityEngine.UI.Text>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    component.text = "";
    component.enabled = false;
  }

  private void UpdateMessage()
  {
    long num = ServerLow.GetTimeMillis() - this.time_of_message;
    if (num > GLOBALS.MESSAGE_DISPLAY_PERIOD)
    {
      this.ClearMessage();
    }
    else
    {
      UnityEngine.UI.Text component = this.messageBox.GetComponent<UnityEngine.UI.Text>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        return;
      float a = 1f;
      if ((double) num > 0.9 * (double) GLOBALS.MESSAGE_DISPLAY_PERIOD)
      {
        a = (float) (GLOBALS.MESSAGE_DISPLAY_PERIOD - num) / (0.1f * (float) GLOBALS.MESSAGE_DISPLAY_PERIOD);
        if ((double) a < 0.0)
          a = 0.0f;
      }
      Color color = new Color(component.color.r, component.color.g, component.color.b, a);
      component.color = color;
    }
  }

  public void ToggleMessage()
  {
    if ((UnityEngine.Object) this.messageBox == (UnityEngine.Object) null)
      return;
    this.messageBox.transform.parent.gameObject.SetActive(!this.messageBox.transform.parent.gameObject.activeSelf);
  }

  public void FlagRequest(Dropdown menu)
  {
    switch (menu.value)
    {
      case 0:
        menu.value = 0;
        menu.itemText.text = menu.options[0].text;
        break;
      case 1:
        this.RestartLevel();
        this.serverInterrupts["RESTARTALL"] = "Server";
        goto case 0;
      default:
        GLOBALS.game_option = menu.value - 1;
        this.RestartLevel();
        this.serverInterrupts["RESTARTALL"] = "Server";
        goto case 0;
    }
  }

  public void ServerMenu_RestartLevel()
  {
    if (this.tournament_mode)
    {
      this.tournament_sm = ServerLow.TOURNAMENT_STATES.WAITING;
      this.tournament_force_start = true;
    }
    else
    {
      this.RestartLevel();
      this.AddChat("Field reset by Server.");
    }
  }

  public void ShowLogMessage(string message, long add_time_s = 0)
  {
    MyUtils.LogMessageToFile(message, false);
    if (!(bool) (UnityEngine.Object) this.messageLog)
      return;
    ServerLow.LogLine logLine = new ServerLow.LogLine();
    logLine.TextLine = UnityEngine.Object.Instantiate<GameObject>(this.messageLog, this.messageLog.transform.parent, false);
    logLine.time_of_message = ServerLow.GetTimeMillis() + add_time_s * 1000L;
    UnityEngine.UI.Text component = logLine.TextLine.GetComponent<UnityEngine.UI.Text>();
    component.text = message;
    component.enabled = true;
    Canvas.ForceUpdateCanvases();
    float num = (float) component.cachedTextGenerator.lineCount * 30f;
    foreach (ServerLow.LogLine allmessage in this.allmessages)
    {
      Vector2 anchoredPosition = allmessage.TextLine.GetComponent<RectTransform>().anchoredPosition;
      anchoredPosition.y += num;
      allmessage.TextLine.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
    }
    this.allmessages.Add(logLine);
  }

  private void UpdateLogMessage()
  {
    for (int index = this.allmessages.Count - 1; index >= 0; --index)
    {
      ServerLow.LogLine allmessage = this.allmessages[index];
      long num = ServerLow.GetTimeMillis() - allmessage.time_of_message;
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
            a = 0.0f;
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
        this.allmessages[index].time_of_message = ServerLow.GetTimeMillis();
    }
  }

  private void OnEnable()
  {
    GLOBALS.LOGS_PATH = (Application.isEditor ? "." : Application.persistentDataPath) + Path.DirectorySeparatorChar.ToString() + "logs";
    ServerLow.field_load = false;
    ServerLow.elements_load = false;
    ServerLow.configuration_done = false;
    ServerLow.scorer_load = false;
    ServerLow.gui_load = false;
    GLOBALS.SERVER_MODE = true;
    GLOBALS.topserver = this;
    GLOBALS.TIMER_TOTAL = 120;
    GLOBALS.TIMER_AUTO = 0;
    GLOBALS.game_option = 1;
    this.UPDATE_DELAY = GLOBALS.SERVER_SEND_UPDATE_DELAY;
    this.MAX_BYTES = GLOBALS.UDP_MAX_BYTES_IN_MS;
    Physics.autoSimulation = true;
    this.ShowMessage("");
    SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.OnLevelFinishedLoading);
    SceneManager.LoadScene("Scenes/server_gui", LoadSceneMode.Additive);
    SceneManager.LoadScene("Scenes/" + GLOBALS.GAME + "/field", LoadSceneMode.Additive);
    SceneManager.LoadScene("Scenes/" + GLOBALS.GAME + "/fieldElements", LoadSceneMode.Additive);
    ServerLow.killme = false;
    ServerLow.thisInstance = this;
    this.FixAWSProblems();
  }

  private void FixAWSProblems()
  {
    if (!GLOBALS.HEADLESS_MODE)
      return;
    foreach (Component component in UnityEngine.Object.FindObjectsOfType<InputField>())
      component.gameObject.SetActive(false);
  }

  private void LateUpdate()
  {
    if (!GLOBALS.HEADLESS_MODE || !this.serverReady || this.fix_was_run <= 0)
      return;
    this.FixAWSProblems();
    --this.fix_was_run;
  }

  private void OnDisable()
  {
    SceneManager.sceneLoaded -= new UnityAction<Scene, LoadSceneMode>(this.OnLevelFinishedLoading);
    GLOBALS.SERVER_MODE = false;
    GLOBALS.topserver = (ServerLow) null;
    ServerLow.configuration_done = false;
    ServerLow.killme = true;
    if (this.thread_incoming != null)
      this.thread_incoming.Abort();
    if (this.m_udpClient != null)
    {
      this.m_udpClient.Close();
      this.m_udpClient.Dispose();
      this.m_udpClient = (UdpClient) null;
    }
    MyUtils.CloseScorefiles();
    GLOBALS.client_names.Clear();
    GLOBALS.client_ids.Clear();
  }

  private void Start()
  {
    GLOBALS.client_names.Clear();
    GLOBALS.client_ids.Clear();
    this.clientIDcounter = 1;
  }

  private void FixedUpdate() => ++this.framecount_phys;

  private void DoAllUpdates()
  {
    this.allReceivedDataSemaphore.WaitOne();
    try
    {
      if (ServerLow.killme)
      {
        this.allReceivedData.Clear();
        return;
      }
      if (this.allReceivedData.Count > 0)
        this.onReceivedData(ref this.allReceivedData);
    }
    catch (Exception ex)
    {
      MyUtils.LogMessageToFile("Update catch exception: " + (object) ex);
    }
    finally
    {
      this.allReceivedData.Clear();
      this.allReceivedDataSemaphore.Release();
    }
    long timeMillis = ServerLow.GetTimeMillis();
    long num1 = timeMillis - this.lastSendingTime;
    float num2 = Time.time - this.sent_data_count_time;
    ++this.framecount;
    this.SendRegistrationInfo();
    int num3 = this.GetPlayerCount() + this.GetSpectatorCount();
    if (num3 > 0)
      this.time_last_seen_players = timeMillis;
    if (num3 < 1)
    {
      if (GLOBALS.HEADLESS_MODE)
      {
        Physics.autoSimulation = false;
        if (GLOBALS.framerate > 10)
          Application.targetFrameRate = 10;
        this.ProcessServerUpdateRequest();
        return;
      }
    }
    else if (GLOBALS.HEADLESS_MODE)
    {
      Application.targetFrameRate = GLOBALS.framerate;
      Physics.autoSimulation = true;
      if ((double) this.saved_fixedtimestep == 0.0)
        this.saved_fixedtimestep = Time.fixedDeltaTime;
      Time.fixedDeltaTime = !this.tournament_mode || !this.holding_mode || this.tournament_sm != ServerLow.TOURNAMENT_STATES.WAITING ? this.saved_fixedtimestep : 1f / (float) GLOBALS.framerate;
    }
    if ((double) num2 >= 0.25)
    {
      this.data_rate = (float) ((double) this.sent_data_count / (double) num2 / 1024.0);
      this.sent_data_count_time = Time.time;
      this.sent_data_count = 0L;
      this.framerate = (int) ((double) this.framecount / (double) num2);
      this.framerate_phys = (int) ((double) this.framecount_phys / (double) num2);
      this.update_framerate = (int) ((double) this.update_framecount / (double) num2);
      this.framecount = 0;
      this.framecount_phys = 0;
      this.update_framecount = 0;
      this.tm_holding_update = true;
    }
    this.ShowMessage("Sent Data Rate (kBytes/s) = " + this.data_rate.ToString("F0"));
    this.ShowMessage("Frame Rate (Frames/s) = " + string.Format("{0,3}", (object) this.framerate) + ", Network Update Rate (Frames/s) = " + this.update_framerate.ToString(), 1, false);
    this.MonitorConnections();
    if (num1 > this.UPDATE_DELAY)
    {
      ++this.update_framecount;
      this.UpdateTournamentMode();
      this.lastSendingTime = timeMillis;
      this.full_cycle_done = true;
      this.CorrectFieldElements();
      this.CorrectPlayers();
      if (MyUtils.GetTimeMillis() - this.last_flag_time > 100L)
      {
        this.UpdateScore();
        this.SendFlags();
        this.last_flag_time = MyUtils.GetTimeMillis();
      }
      if (this.tournament_mode && this.holding_mode && this.tournament_sm == ServerLow.TOURNAMENT_STATES.WAITING)
      {
        if (this.tm_holding_update)
        {
          this.SendFieldElements();
          this.SendPlayerLocations();
          this.tm_holding_update = false;
        }
      }
      else
      {
        this.SendFieldElements();
        this.SendPlayerLocations();
      }
      this.UpdateMessage();
      this.UpdateLogMessage();
    }
    if ((double) num1 <= (double) this.UPDATE_DELAY / 2.0 || !this.full_cycle_done)
      return;
    this.full_cycle_done = false;
    this.sendCurrentPlayers();
    this.ProcessServerUpdateRequest();
    if (this.GetSpectatorCount() <= 0)
      return;
    this.SendFieldElements(SendDataFlags.SPECTATORS);
    this.SendPlayerLocations(SendDataFlags.SPECTATORS);
  }

  private void Update()
  {
    if (!this.second_load && ServerLow.gui_load)
    {
      this.second_load = true;
      SceneManager.LoadScene("Scenes/" + GLOBALS.GAME + "/Scoring", LoadSceneMode.Additive);
    }
    if (!GLOBALS.keyboard_inuse && Input.GetKeyDown(KeyCode.I))
      this.ToggleMessage();
    if (GLOBALS.UDP_ALGORITHM == 2)
      this.DataReceiveSerial();
    this.DoAllUpdates();
    MyUtils.DoScoringFiles(this.serverFlags);
  }

  private void OnApplicationQuit() => ServerLow.killme = true;

  public void ServerStart(
    int port = 1446,
    string password = "",
    int spectators = 4,
    string comment = "",
    int routerport = 1446,
    bool register = false,
    bool tournamentmode = false)
  {
    MyUtils.LogMessageToFile("setting up udp");
    this.max_spectators = spectators;
    this.tournament_mode = tournamentmode;
    this.SERVER_COMMENT = comment.Length <= 25 ? comment : comment.Substring(0, 25);
    this.PASSWORD = password;
    this.ROUTER_PORT = routerport;
    this.REGISTER_SERVER = register;
    this.SetupUdp(port);
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
    this.serverReady = true;
  }

  public void ServerStartHeadless()
  {
    MyUtils.LogMessageToFile("setting up udp");
    this.SetupUdp(this.PORT);
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
    this.serverReady = true;
    this.FixAWSProblems();
  }

  public void UpdateTournamentMode()
  {
    if (!this.tournament_force_start && !this.tournament_mode)
    {
      this.serverFlags["TM_STATE"] = "NO";
    }
    else
    {
      if (this.holding_mode && this.tournament_sm != ServerLow.TOURNAMENT_STATES.RUNNING)
        this.serverFlags["HOLDING"] = "1";
      else if (this.serverFlags.ContainsKey("HOLDING"))
        this.serverFlags.Remove("HOLDING");
      switch (this.tournament_sm)
      {
        case ServerLow.TOURNAMENT_STATES.WAITING:
          if (!this.tournament_force_start && this.GetPlayerCount() < GLOBALS.PlayerCount)
          {
            this.serverFlags["TM_STATE"] = "WAITING";
            this.serverFlags["TM_MSG"] = "Waiting for " + (object) GLOBALS.PlayerCount + " players...";
            break;
          }
          if (this.tournament_force_start || this.GetReadyPlayers() == this.GetPlayerCount() && this.start_when_ready)
          {
            this.serverFlags["TM_STATE"] = "COUNTDOWN";
            this.serverFlags["TM_MSG"] = "";
            if (this.scorer.UseGameStartOverlay())
            {
              this.serverFlags.Remove("TM_ANIM1");
              this.serverFlags.Remove("TM_ANIM2");
              this.serverInterrupts["TM_ANIM1"] = "1";
              this.scorer.ShowGameStartOverlay();
            }
            this.tournament_sm = ServerLow.TOURNAMENT_STATES.HYPE1;
            this.tm_timer = ServerLow.GetTimeMillis();
            break;
          }
          this.serverFlags["TM_STATE"] = "WAITING";
          this.serverFlags["TM_MSG"] = "Waiting for all players to be ready...";
          break;
        case ServerLow.TOURNAMENT_STATES.HYPE1:
          if (this.scorer.UseGameStartOverlay() && ServerLow.GetTimeMillis() - this.tm_timer <= 4000L)
            break;
          this.serverInterrupts["COUNTDOWN"] = "1";
          this.scorer.ScorerReset();
          this.scorer.StartCountdown();
          this.tournament_sm = ServerLow.TOURNAMENT_STATES.COUNTDOWN;
          this.tm_timer = ServerLow.GetTimeMillis();
          break;
        case ServerLow.TOURNAMENT_STATES.COUNTDOWN:
          if (ServerLow.GetTimeMillis() - this.tm_timer > 2000L)
          {
            this.serverInterrupts["TM_ANIM1"] = "0";
            this.serverFlags["TM_ANIM2"] = "0";
            if (!this.game_overlay_turned_off)
            {
              this.scorer.ShowGameStartOverlay(false);
              this.game_overlay_turned_off = true;
            }
          }
          else
            this.game_overlay_turned_off = false;
          if (ServerLow.GetTimeMillis() - this.tm_timer > 3000L)
          {
            this.serverFlags["TM_STATE"] = "RUNNING";
            this.serverFlags["TM_MSG"] = "";
            this.tournament_sm = ServerLow.TOURNAMENT_STATES.RUNNING;
            this.RestartLevel();
            break;
          }
          this.serverFlags["TM_STATE"] = "COUNTDOWN";
          break;
        case ServerLow.TOURNAMENT_STATES.RUNNING:
          this.serverFlags["TM_STATE"] = "RUNNING";
          this.serverFlags["TM_MSG"] = "";
          if (!(this.scorer.GetTimerState() == "FINISHED"))
            break;
          this.serverFlags["TM_STATE"] = "END";
          this.serverFlags["TM_MSG"] = "";
          this.tournament_sm = ServerLow.TOURNAMENT_STATES.END;
          break;
        case ServerLow.TOURNAMENT_STATES.END:
          this.serverFlags["TM_STATE"] = "END";
          if (this.scorer.GetBlueScore() > this.scorer.GetRedScore())
          {
            this.serverFlags["TM_MSG"] = "GAME FINISHED: BLUE WINS! ";
            this.serverInterrupts["FIREWORKS"] = "BLUE";
            this.scorer.StartFireworks(false);
          }
          else if (this.scorer.GetBlueScore() < this.scorer.GetRedScore())
          {
            this.serverFlags["TM_MSG"] = "GAME FINISHED: RED WINS!";
            this.serverInterrupts["FIREWORKS"] = "RED";
            this.scorer.StartFireworks(true);
          }
          else
            this.serverFlags["TM_MSG"] = "GAME FINISHED: TIE!";
          this.tournament_sm = ServerLow.TOURNAMENT_STATES.WAITING;
          this.tournament_force_start = false;
          break;
      }
    }
  }

  public void ToggleChampsMode()
  {
    ChampsInit[] objectsOfTypeAll = UnityEngine.Resources.FindObjectsOfTypeAll<ChampsInit>();
    if (objectsOfTypeAll.Length < 1)
      return;
    this.ChampsObject = objectsOfTypeAll[0].gameObject;
    this.ChampsObject.SetActive(!this.ChampsObject.activeSelf);
    foreach (TextMeshPro componentsInChild in this.ChampsObject.GetComponentsInChildren<TextMeshPro>())
    {
      if (componentsInChild.name.StartsWith("UserText"))
        componentsInChild.text = this.CHAMPS_TEXT;
    }
    if (this.ChampsObject.activeSelf)
    {
      this.serverFlags["CHAMPS"] = this.CHAMPS_TEXT;
    }
    else
    {
      if (!this.serverFlags.ContainsKey("CHAMPS"))
        return;
      this.serverFlags.Remove("CHAMPS");
    }
  }

  public void MonitorConnections()
  {
    long timeMillis = ServerLow.GetTimeMillis();
    if (timeMillis - this.time_last_count_check > GLOBALS.SERVER_MESSAGE_COUNT_TIME)
    {
      string message = "<b> List of Connections on Port " + (this.remoteEP != null ? this.remoteEP.Port.ToString() : "") + " (Press i to toggle): </b> ";
      int num1 = 0;
      foreach (ServerLow.Client client in this.allClients.Values)
      {
        if (!((UnityEngine.Object) client.status_line == (UnityEngine.Object) null))
        {
          int num2 = (int) ((double) client.message_count / (double) (timeMillis - this.time_last_count_check) * 1000.0);
          client.status_line.transform.Find("DATA").GetComponent<UnityEngine.UI.Text>().text = num2.ToString();
          client.status_line.transform.Find("READY").GetComponent<UnityEngine.UI.Text>().text = "";
          if (client.flags.ContainsKey("PL"))
            client.status_line.transform.Find("PL").GetComponent<UnityEngine.UI.Text>().text = client.flags["PL"];
          if (this.tournament_sm == ServerLow.TOURNAMENT_STATES.WAITING)
          {
            string str = "";
            client.flags.TryGetValue("READY", out str);
            client.status_line.transform.Find("READY").GetComponent<UnityEngine.UI.Text>().text = str;
          }
          client.message_count = 0;
          if ((bool) (UnityEngine.Object) this.messageBox)
          {
            client.status_line.transform.SetParent(this.messageBox.transform.parent.transform);
            client.status_line.transform.localPosition = new Vector3(24f, (float) (-100.0 - (double) num1 * 30.0), -1f);
            client.status_line.transform.localScale = new Vector3(1f, 1f, 1f);
          }
          ++num1;
        }
      }
      if (GLOBALS.ENABLE_UDP_STATS && this.netmonitor != null)
      {
        foreach (KeyValuePair<string, int> keyValuePair in this.netmonitor)
          message = message + "\n" + keyValuePair.Key + "=" + (object) (float) ((double) keyValuePair.Value / (double) (timeMillis - this.time_last_count_check) * 1000.0);
        this.netmonitor.Clear();
        message = message + "\nCached Count = " + this.items_cached.ToString();
        this.items_cached = 0;
      }
      this.time_last_count_check = timeMillis;
      this.ShowMessage(message, 2, false);
      this.last_message = message;
    }
    else
      this.ShowMessage(this.last_message, 2, false);
    List<int> intList = new List<int>();
    foreach (ServerLow.Client client in this.allClients.Values)
    {
      if (timeMillis - client.time_last_message >= GLOBALS.SERVER_DISCONNECT_TIMEOUT)
      {
        intList.Add(client.id);
        MyUtils.LogMessageToFile("Lost communication with " + client.name, false);
      }
      if (!(this.ADMIN == client.name) && !client.isAdmin && (bool) (UnityEngine.Object) client.robot && MyUtils.GetTimeMillis() - client.robot.time_last_button_activitiy >= (long) (1000 * GLOBALS.CLIENT_ROBOT_INACTIVITY_TIMEOUT) && (!client.robot.isSpectator || MyUtils.GetTimeMillis() - this.time_last_seen_players > (long) (1000 * GLOBALS.CLIENT_ROBOT_INACTIVITY_TIMEOUT)))
      {
        intList.Add(client.id);
        MyUtils.LogMessageToFile("Innactivity timeout for " + client.name, false);
      }
    }
    foreach (int cnndId in intList)
      this.RemoveClient(cnndId);
    if (timeMillis - this.time_cache_clear > GLOBALS.SERVER_CACHE_REFRESH)
    {
      this.time_cache_clear = timeMillis;
      foreach (ServerLow.CacheString cacheString in this.netcache.Values)
        cacheString.Clear();
      foreach (ServerLow.CacheString cacheString in this.netcache_spectators.Values)
        cacheString.Clear();
      this.cache_cleared = true;
    }
    else
      this.cache_cleared = false;
  }

  public static long GetTimeMillis() => MyUtils.GetTimeMillis();

  public int GetPlayerCount()
  {
    int playerCount = 0;
    foreach (int key in this.allClients.Keys)
    {
      if (this.allClients[key].starting_position != "Spectator")
        ++playerCount;
    }
    return playerCount;
  }

  public int GetSpectatorCount()
  {
    int spectatorCount = 0;
    foreach (int key in this.allClients.Keys)
    {
      if (this.allClients[key].starting_position == "Spectator")
        ++spectatorCount;
    }
    return spectatorCount;
  }

  private int GetAdminSpecCount()
  {
    int adminSpecCount = 0;
    foreach (int key in this.allClients.Keys)
    {
      if (this.allClients[key].starting_position == "Spectator" && (this.allClients[key].name == this.ADMIN && this.ADMIN.Length > 0 || this.allClients[key].isAdmin))
        ++adminSpecCount;
    }
    return adminSpecCount;
  }

  private int GetReadyPlayers()
  {
    int readyPlayers = 0;
    string str = "FALSE";
    foreach (int key in this.allClients.Keys)
    {
      if (this.allClients[key].starting_position != "Spectator" && this.allClients[key].flags.TryGetValue("READY", out str) && (str == "YES" || str == "READY"))
        ++readyPlayers;
    }
    return readyPlayers;
  }

  private void SendPlayerLocations(SendDataFlags who = SendDataFlags.PLAYERS)
  {
    if (who == SendDataFlags.EVERYONE)
      who = SendDataFlags.PLAYERS;
    ref int local = ref ServerLow.message_id_tracked_players;
    Dictionary<string, ServerLow.CacheString> dictionary1;
    if (who == SendDataFlags.PLAYERS)
    {
      dictionary1 = this.netcache;
    }
    else
    {
      dictionary1 = this.netcache_spectators;
      local = ref ServerLow.message_id_tracked_other;
    }
    this.netmonitor_header = "4.5";
    foreach (ServerLow.Client client in this.allClients.Values)
    {
      if (!((UnityEngine.Object) client.avatar == (UnityEngine.Object) null) && !(client.starting_position == "Spectator") && client.avatar.scene.name != null && client.avatar.activeInHierarchy)
      {
        string messageraw = "4.5\u0011" + local.ToString() + "\u0011" + local.ToString() + "\u0011" + client.id.ToString() + "\u0012" + client.robotmodel.Length.ToString();
        int num1 = 0;
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        stringBuilder1.Append("SPL").Append((int) who).Append(client.id.ToString() + "\u0011" + -1.ToString());
        StringBuilder stringBuilder3 = stringBuilder2;
        string[] strArray1 = new string[11];
        Vector3 vector3 = client.avatar.transform.position;
        strArray1[0] = vector3.x.ToString("0.####");
        strArray1[1] = "\u0012";
        vector3 = client.avatar.transform.position;
        strArray1[2] = vector3.y.ToString("0.####");
        strArray1[3] = "\u0012";
        vector3 = client.avatar.transform.position;
        strArray1[4] = vector3.z.ToString("0.####");
        strArray1[5] = "\u0012";
        vector3 = client.avatar.transform.eulerAngles;
        strArray1[6] = vector3.x.ToString("0.####");
        strArray1[7] = "\u0012";
        vector3 = client.avatar.transform.eulerAngles;
        strArray1[8] = vector3.y.ToString("0.####");
        strArray1[9] = "\u0012";
        vector3 = client.avatar.transform.eulerAngles;
        strArray1[10] = vector3.z.ToString("0.####");
        string str1 = string.Concat(strArray1);
        stringBuilder3.Append(str1);
        bool flag1 = false;
        if (dictionary1.ContainsKey(stringBuilder1.ToString()))
        {
          if (dictionary1[stringBuilder1.ToString()].msg.ToString() == stringBuilder2.ToString())
          {
            ++this.items_cached;
            if (++dictionary1[stringBuilder1.ToString()].count > 2)
            {
              flag1 = true;
              dictionary1[stringBuilder1.ToString()].count = 0;
            }
          }
          else
          {
            dictionary1[stringBuilder1.ToString()].msg.Clear().Append(stringBuilder2.ToString());
            dictionary1[stringBuilder1.ToString()].count = 0;
          }
        }
        else
          dictionary1[stringBuilder1.ToString()] = new ServerLow.CacheString(stringBuilder2.ToString());
        if (!flag1)
          messageraw = messageraw + "\u0012" + -1.ToString() + "\u0012" + stringBuilder2.ToString();
        bool flag2 = false;
        bool flag3 = false;
        int num2 = 1;
        Dictionary<int, bool> dictionary2 = new Dictionary<int, bool>();
        for (int index = 0; index <= client.avatar.transform.childCount; ++index)
        {
          if (index == client.avatar.transform.childCount)
          {
            if (flag3)
            {
              index = 0;
              flag3 = false;
              ++num2;
            }
            else
              break;
          }
          if (!dictionary2.ContainsKey(index) && !(client.avatar.transform.GetChild(index).name == "Body" & flag2) && (!(client.avatar.transform.GetChild(index).name != "Body") || flag2))
          {
            stringBuilder1.Clear();
            stringBuilder1.Append("SPL").Append(client.id.ToString() + "\u0011" + index.ToString());
            stringBuilder2.Clear();
            if ((bool) (UnityEngine.Object) client.avatar.transform.GetChild(index).GetComponent<BandwidthHelper>())
            {
              if (client.avatar.transform.GetChild(index).GetComponent<BandwidthHelper>().priority == num2)
              {
                stringBuilder2.Append(client.avatar.transform.GetChild(index).GetComponent<BandwidthHelper>().Get((int) who));
                flag3 = true;
                dictionary2[index] = true;
              }
              else
                continue;
            }
            else
            {
              StringBuilder stringBuilder4 = stringBuilder2;
              string[] strArray2 = new string[11];
              vector3 = client.avatar.transform.GetChild(index).transform.localPosition;
              strArray2[0] = vector3.x.ToString("0.####");
              strArray2[1] = "\u0012";
              vector3 = client.avatar.transform.GetChild(index).transform.localPosition;
              strArray2[2] = vector3.y.ToString("0.####");
              strArray2[3] = "\u0012";
              vector3 = client.avatar.transform.GetChild(index).transform.localPosition;
              strArray2[4] = vector3.z.ToString("0.####");
              strArray2[5] = "\u0012";
              vector3 = client.avatar.transform.GetChild(index).transform.localEulerAngles;
              strArray2[6] = vector3.x.ToString("0.####");
              strArray2[7] = "\u0012";
              vector3 = client.avatar.transform.GetChild(index).transform.localEulerAngles;
              strArray2[8] = vector3.y.ToString("0.####");
              strArray2[9] = "\u0012";
              vector3 = client.avatar.transform.GetChild(index).transform.localEulerAngles;
              strArray2[10] = vector3.z.ToString("0.####");
              string str2 = string.Concat(strArray2);
              stringBuilder4.Append(str2);
              dictionary2[index] = true;
            }
            if (dictionary1.ContainsKey(stringBuilder1.ToString()))
            {
              if (dictionary1[stringBuilder1.ToString()].ToString() == stringBuilder2.ToString())
              {
                if (++dictionary1[stringBuilder1.ToString()].count > 2)
                {
                  ++this.items_cached;
                  continue;
                }
              }
              else
              {
                dictionary1[stringBuilder1.ToString()].msg.Clear().Append(stringBuilder2.ToString());
                dictionary1[stringBuilder1.ToString()].count = 0;
              }
            }
            else
              dictionary1[stringBuilder1.ToString()] = new ServerLow.CacheString(stringBuilder2.ToString());
            messageraw = messageraw + "\u0012" + index.ToString() + "\u0012" + stringBuilder2.ToString();
            ++num1;
            if (!flag2)
            {
              flag2 = true;
              index = -1;
            }
          }
        }
        if (num1 != 0)
        {
          this.SendUdp(messageraw, Compression: 3, who: who);
          ++local;
        }
      }
    }
    this.netmonitor_header = (string) null;
  }

  public void sendCurrentPlayers()
  {
    ++ServerLow.message_id;
    string messageraw = "PLYRS\u0011" + ServerLow.message_id.ToString() + "\u0011" + ServerLow.message_id.ToString() + "\u0011";
    bool flag = false;
    foreach (ServerLow.Client client in this.allClients.Values)
    {
      if ((UnityEngine.Object) client.robot == (UnityEngine.Object) null || client.starting_position == "Spectator")
        messageraw = messageraw + client.name + "\u0012" + (object) client.id + "\u0012" + client.robotmodel + "\u0012" + client.starting_position + "\u00120\u0011";
      else
        messageraw = messageraw + client.name + "\u0012" + (object) client.id + "\u0012" + client.robotmodel + "\u0012" + client.starting_position + "\u0012" + client.robot.GetStates() + "\u0012" + client.skins + "\u0012" + client.robotskins + "\u0011";
      flag = true;
    }
    if (!flag)
      return;
    this.SendUdp(messageraw);
  }

  public void AddFlag(string key, string value, int id = -1)
  {
    if (id > 0)
    {
      if (!this.allClients.ContainsKey(id))
        return;
      this.allClients[id].serverFlags[key] = value;
    }
    else
      this.serverFlags[key] = value;
  }

  public void AddInterrupt(string key, string value) => this.serverInterrupts[key] = value;

  public void ClearFlag(string key, int id = -1)
  {
    if (id > 0)
    {
      if (!this.allClients.ContainsKey(id))
        return;
      this.allClients[id].serverFlags.Remove(key);
    }
    else
      this.serverFlags.Remove(key);
  }

  private void SendFlags()
  {
    this.serverFlags.Remove("ADMIN");
    if (this.ADMIN.Length > 0)
      this.serverFlags["ADMIN"] = !GLOBALS.client_ids.ContainsKey(this.ADMIN) ? "-9" : GLOBALS.client_ids[this.ADMIN].ToString();
    if (this.serverFlags.Count < 1 && this.serverInterrupts.Count < 1)
      this.serverFlags["NotUsed"] = "1";
    string str = "";
    foreach (string key in this.serverFlags.Keys)
    {
      if (str.Length > 1)
        str += "\u0012";
      str = str + key + "\u0012" + this.serverFlags[key];
    }
    foreach (string key in this.serverInterrupts.Keys)
    {
      if (str.Length > 1)
        str += "\u0012";
      str = str + key + "\u0012" + this.serverInterrupts[key];
    }
    foreach (int key1 in this.allClients.Keys)
    {
      string messageraw = str;
      foreach (string key2 in this.allClients[key1].serverFlags.Keys)
      {
        if (messageraw.Length > 1)
          messageraw += "\u0012";
        messageraw = messageraw + key2 + "\u0012" + this.allClients[key1].serverFlags[key2];
      }
      foreach (string key3 in this.allClients[key1].serverInterrupts.Keys)
      {
        if (messageraw.Length > 1)
          messageraw += "\u0012";
        messageraw = messageraw + key3 + "\u0012" + this.allClients[key1].serverInterrupts[key3];
      }
      this.allClients[key1].serverInterrupts.Clear();
      this.SendFlagsUDPTracked(messageraw, who: SendDataFlags.TARGETID, receiver_id: key1);
    }
    this.serverInterrupts.Clear();
  }

  private void SendFieldElements(SendDataFlags who = SendDataFlags.PLAYERS)
  {
    this.msg_test_case = "";
    SortedDictionary<int, GameObject>.ValueCollection values = this.allFieldElements.Values;
    if (who == SendDataFlags.EVERYONE)
      who = SendDataFlags.PLAYERS;
    ref int local = ref ServerLow.message_id_tracked_players;
    Dictionary<string, ServerLow.CacheString> dictionary;
    if (who == SendDataFlags.PLAYERS)
    {
      dictionary = this.netcache;
    }
    else
    {
      dictionary = this.netcache_spectators;
      local = ref ServerLow.message_id_tracked_other;
    }
    string messageraw1 = "3.2\u0011" + local.ToString() + "\u0011" + local.ToString() + "\u0011";
    int num = 0;
    StringBuilder stringBuilder1 = new StringBuilder();
    StringBuilder stringBuilder2 = new StringBuilder();
    foreach (GameObject gameObject in values)
    {
      Transform component1 = gameObject.GetComponent<Transform>();
      gameElement component2 = gameObject.GetComponent<gameElement>();
      if (component2.type != ElementType.Off)
      {
        stringBuilder1.Clear();
        stringBuilder1.Append("SFE").Append((int) who).Append(component2.id.ToString());
        stringBuilder2.Clear();
        stringBuilder2.Append(((int) component2.type).ToString() + "\u0012");
        if ((bool) (UnityEngine.Object) component1.GetComponent<BandwidthHelper>())
        {
          stringBuilder2.Append(component1.GetComponent<BandwidthHelper>().Get((int) who));
        }
        else
        {
          StringBuilder stringBuilder3 = stringBuilder2;
          string[] strArray = new string[11];
          Vector3 vector3 = component1.position;
          strArray[0] = vector3.x.ToString("0.####");
          strArray[1] = "\u0012";
          vector3 = component1.position;
          strArray[2] = vector3.y.ToString("0.####");
          strArray[3] = "\u0012";
          vector3 = component1.position;
          strArray[4] = vector3.z.ToString("0.####");
          strArray[5] = "\u0012";
          Quaternion rotation = component1.rotation;
          vector3 = rotation.eulerAngles;
          strArray[6] = vector3.x.ToString("0.####");
          strArray[7] = "\u0012";
          rotation = component1.rotation;
          vector3 = rotation.eulerAngles;
          strArray[8] = vector3.y.ToString("0.####");
          strArray[9] = "\u0012";
          rotation = component1.rotation;
          vector3 = rotation.eulerAngles;
          strArray[10] = vector3.z.ToString("0.####");
          string str = string.Concat(strArray);
          stringBuilder3.Append(str);
        }
        if (dictionary.ContainsKey(stringBuilder1.ToString()))
        {
          if (dictionary[stringBuilder1.ToString()].ToString() == stringBuilder2.ToString() && ++dictionary[stringBuilder1.ToString()].count > 2)
          {
            ++this.items_cached;
            continue;
          }
          dictionary[stringBuilder1.ToString()].msg.Clear().Append(stringBuilder2.ToString());
          dictionary[stringBuilder1.ToString()].count = 0;
        }
        else
          dictionary[stringBuilder1.ToString()] = new ServerLow.CacheString(stringBuilder2.ToString());
        messageraw1 = messageraw1 + component2.id.ToString() + "\u0012" + stringBuilder2.ToString() + "\u0011";
        if (num >= 15)
        {
          num = 0;
          messageraw1 += "\u0011";
          this.SendUdp(messageraw1, Compression: 3, who: who);
          ++local;
          this.msg_test_case += messageraw1;
          messageraw1 = "3.2\u0011" + local.ToString() + "\u0011" + local.ToString() + "\u0011";
        }
        else
          ++num;
      }
    }
    string messageraw2 = messageraw1 + "\u0011";
    if (num == 0)
      return;
    this.SendUdp(messageraw2, Compression: 3, who: who);
    ++local;
    this.msg_test_case += messageraw2;
  }

  private void ProcessServerUpdateRequest()
  {
    for (int index = 0; index < this.serverInfoRequests.Count; ++index)
    {
      ServerLow.Message serverInfoRequest = this.serverInfoRequests[index];
      byte[] data = serverInfoRequest.data;
      List<byte[]> numArrayList = new List<byte[]>();
      List<byte[]> extracted_data = numArrayList;
      MyUtils.ExtractMessageHeader(data, extracted_data);
      this.SendServerInfoUdp("GAME\u0012" + GLOBALS.GAME + "\u0012PLAYERS\u0012" + this.GetPlayerCount().ToString() + "\u0012MAXPLAYERS\u0012" + (object) GLOBALS.PlayerCount + "\u0012VERSION\u0012" + Application.version + "\u0012COMMENT\u0012" + this.SERVER_COMMENT + "\u0012PASSWORD\u0012" + (this.PASSWORD.Length > 0 ? (object) "1" : (object) "0") + "\u0011" + Encoding.UTF8.GetString(numArrayList[1]) + "\u0011", serverInfoRequest.endpoint);
    }
    this.serverInfoRequests.Clear();
  }

  private void SendRegistrationInfo()
  {
    if (!this.REGISTER_SERVER || MyUtils.GetTimeMillis() - this.registration_time <= 30000L)
      return;
    this.registration_time = MyUtils.GetTimeMillis() + (long) ServerLow.rng.Next(-1000, 1000);
    this.StopAllCoroutines();
    this.StartCoroutine(this.SendRegistrationCoroutine());
  }

  private IEnumerator SendRegistrationCoroutine()
  {
    using (UnityWebRequest www = UnityWebRequest.Put("http://xrcsimulator.org/game/registerserver.php", "PORT\u0011" + (object) this.ROUTER_PORT + "\u0012VERSION\u0011" + Application.version + "\u0012GAME\u0011" + GLOBALS.GAME + "\u0012PLAYERS\u0011" + (object) this.GetPlayerCount() + "\u0012MAXPLAYERS\u0011" + (object) GLOBALS.PlayerCount + "\u0012SPECTATORS\u0011" + (object) this.GetSpectatorCount() + "\u0012MAXSPECTATORS\u0011" + (object) this.max_spectators + "\u0012PASSWORD\u0011" + (object) (this.PASSWORD.Length > 0 ? 1 : 0) + "\u0012COMMENT\u0011" + this.SERVER_COMMENT))
      yield return (object) www.SendWebRequest();
  }

  public void SendChat(string msg)
  {
    if (msg.StartsWith("/SET "))
    {
      string[] strArray1 = msg.Substring(5).Split('=');
      if (strArray1[0] == "OUTPUT_SCORE_FILES")
      {
        GLOBALS.OUTPUT_SCORING_FILES = true;
        if (strArray1.Length > 1)
          MyUtils.status_file_dir = strArray1[1];
        Console.Out.WriteLine("/SET: OUTPUT Score files turned on to dir " + MyUtils.status_file_dir);
      }
      else if (strArray1[0] == "PASSWORD")
      {
        this.PASSWORD = strArray1.Length <= 1 ? "" : strArray1[1];
        Console.Out.WriteLine("/SET: PASSWORD = " + this.PASSWORD);
      }
      else if (strArray1[0] == "TOURNAMENT")
      {
        this.tournament_force_start = true;
        Console.Out.WriteLine("/SET: Tournament mode enabled.");
      }
      else if (strArray1[0] == "CODE")
      {
        string[] strArray2 = strArray1[1].Split(',');
        if (strArray2.Length < 4)
          return;
        string md5 = MyUtils.CreateMD5(int.Parse(strArray2[0]), int.Parse(strArray2[1]), strArray2[2], int.Parse(strArray2[3]));
        this.ShowLogMessage("RED=" + (object) int.Parse(strArray2[0]) + ",BLUE=" + (object) int.Parse(strArray2[1]) + ",Position=" + strArray2[2] + ", Code=" + (object) int.Parse(strArray2[3]) + ",CODE=" + md5, 10L);
      }
      else
      {
        if (!(strArray1[0] == "FIND") || strArray1.Length < 3)
          return;
        int red_score;
        int blue_score;
        string position;
        string md5 = MyUtils.FindMD5(strArray1[1], int.Parse(strArray1[2]), 500, out red_score, out blue_score, out position);
        this.ShowLogMessage("Red=" + (object) red_score + ", Blue=" + (object) blue_score + ", POS=" + position + ", CODE=" + md5, 10L);
      }
    }
    else
    {
      string str = "SERVER: " + msg;
      this.ShowLogMessage(str);
      this.AddChat(str);
    }
  }

  public void AddChat(string msg, int id = -1)
  {
    Dictionary<string, string> dictionary1 = id > 0 ? this.allClients[id].serverInterrupts : this.serverInterrupts;
    if (dictionary1.ContainsKey("CHAT"))
    {
      Dictionary<string, string> dictionary2 = dictionary1;
      dictionary2["CHAT"] = dictionary2["CHAT"] + "\n" + msg;
    }
    else
      dictionary1["CHAT"] = msg;
  }

  public void ChangeGameSettings(string newsettings) => this.serverFlags["GameSettings"] = newsettings;

  public void onReceivedData(ref List<ServerLow.Message> messages)
  {
    for (int index = 0; index < messages.Count; ++index)
    {
      ServerLow.Message message = messages[index];
      byte[] data = message.data;
      List<byte[]> extracted_data = new List<byte[]>();
      MyUtils.ExtractMessageHeader(data, extracted_data);
      if (!Encoding.UTF8.GetString(extracted_data[0]).Equals("7p0"))
      {
        if (Encoding.UTF8.GetString(extracted_data[0]).Equals("11115") && extracted_data.Count >= 2)
          this.serverInfoRequests.Add(message);
        else
          MyUtils.LogMessageToFile("OnReceiveData passcode failed. " + (object) data);
      }
      else if (extracted_data.Count < 5)
      {
        MyUtils.LogMessageToFile("OnReceiveData split failed on _. " + (object) data);
      }
      else
      {
        int result1 = 0;
        if (!int.TryParse(Encoding.UTF8.GetString(extracted_data[3]), out result1))
          MyUtils.LogMessageToFile("OnReceiveData extraction of length failed. " + (object) data);
        else if (result1 != extracted_data[4].Length)
        {
          MyUtils.LogMessageToFile("OnReceiveData lengths did not equal. Length =" + result1.ToString() + " Actual=" + extracted_data[3].Length.ToString());
        }
        else
        {
          int result2 = 0;
          if (!int.TryParse(Encoding.UTF8.GetString(extracted_data[1]), out result2))
          {
            MyUtils.LogMessageToFile("OnReceiveData extraction of compression failed. " + (object) data);
          }
          else
          {
            int result3 = 0;
            if (!int.TryParse(Encoding.UTF8.GetString(extracted_data[2]), out result3))
            {
              MyUtils.LogMessageToFile("OnReceiveData extraction of clientId failed. " + (object) data);
            }
            else
            {
              string[] strArray = MyUtils.DecompressMessage(extracted_data[4], result2).Split('\u0011');
              if (strArray.Length < 2)
                MyUtils.LogMessageToFile("onReceivedData unable to split on |. Client IP = " + message.endpoint.Address.ToString());
              else if (strArray[0] == "NAMEIS")
                this.AddNewClient(strArray, message.endpoint);
              else if (this.allClients.TryGetValue(result3, out ServerLow.Client _))
              {
                string str = strArray[0];
                if (!(str == "MYINPUTS"))
                {
                  if (str == "FLG")
                    this.OnClientFlags(result3, strArray);
                  else
                    MyUtils.LogMessageToFile("OnReceiveData invalid HEADER:" + strArray[0]);
                }
                else
                  this.OnClientInputReceive(result3, strArray);
                if (this.allClients.ContainsKey(result3))
                {
                  this.allClients[result3].time_last_message = ServerLow.GetTimeMillis();
                  ++this.allClients[result3].message_count;
                }
              }
            }
          }
        }
      }
    }
  }

  private bool OnClientInputReceive(int cnnId, string[] rawData)
  {
    try
    {
      if ((UnityEngine.Object) this.allClients[cnnId].avatar == (UnityEngine.Object) null)
        return true;
      RobotInterface3D robot = this.allClients[cnnId].robot;
      if ((UnityEngine.Object) robot == (UnityEngine.Object) null)
        return false;
      string[] strArray1 = rawData[1].Split('\u0012');
      int num1 = 0;
      RobotInterface3D robotInterface3D1 = robot;
      string[] strArray2 = strArray1;
      int index1 = num1;
      int num2 = index1 + 1;
      int num3 = MyUtils.StringToBool(strArray2[index1]) ? 1 : 0;
      robotInterface3D1.gamepad1_a = num3 != 0;
      RobotInterface3D robotInterface3D2 = robot;
      string[] strArray3 = strArray1;
      int index2 = num2;
      int num4 = index2 + 1;
      int num5 = MyUtils.StringToBool(strArray3[index2]) ? 1 : 0;
      robotInterface3D2.gamepad1_b = num5 != 0;
      RobotInterface3D robotInterface3D3 = robot;
      string[] strArray4 = strArray1;
      int index3 = num4;
      int num6 = index3 + 1;
      int num7 = MyUtils.StringToBool(strArray4[index3]) ? 1 : 0;
      robotInterface3D3.gamepad1_x = num7 != 0;
      RobotInterface3D robotInterface3D4 = robot;
      string[] strArray5 = strArray1;
      int index4 = num6;
      int num8 = index4 + 1;
      int num9 = MyUtils.StringToBool(strArray5[index4]) ? 1 : 0;
      robotInterface3D4.gamepad1_y = num9 != 0;
      RobotInterface3D robotInterface3D5 = robot;
      string[] strArray6 = strArray1;
      int index5 = num8;
      int num10 = index5 + 1;
      double num11 = (double) float.Parse(strArray6[index5]);
      robotInterface3D5.gamepad1_right_stick_y = (float) num11;
      RobotInterface3D robotInterface3D6 = robot;
      string[] strArray7 = strArray1;
      int index6 = num10;
      int num12 = index6 + 1;
      double num13 = (double) float.Parse(strArray7[index6]);
      robotInterface3D6.gamepad1_right_stick_x = (float) num13;
      RobotInterface3D robotInterface3D7 = robot;
      string[] strArray8 = strArray1;
      int index7 = num12;
      int num14 = index7 + 1;
      double num15 = (double) float.Parse(strArray8[index7]);
      robotInterface3D7.gamepad1_left_stick_x = (float) num15;
      RobotInterface3D robotInterface3D8 = robot;
      string[] strArray9 = strArray1;
      int index8 = num14;
      int num16 = index8 + 1;
      double num17 = (double) float.Parse(strArray9[index8]);
      robotInterface3D8.gamepad1_left_stick_y = (float) num17;
      RobotInterface3D robotInterface3D9 = robot;
      string[] strArray10 = strArray1;
      int index9 = num16;
      int num18 = index9 + 1;
      int num19 = MyUtils.StringToBool(strArray10[index9]) ? 1 : 0;
      robotInterface3D9.gamepad1_dpad_down = num19 != 0;
      RobotInterface3D robotInterface3D10 = robot;
      string[] strArray11 = strArray1;
      int index10 = num18;
      int num20 = index10 + 1;
      int num21 = MyUtils.StringToBool(strArray11[index10]) ? 1 : 0;
      robotInterface3D10.gamepad1_dpad_up = num21 != 0;
      RobotInterface3D robotInterface3D11 = robot;
      string[] strArray12 = strArray1;
      int index11 = num20;
      int num22 = index11 + 1;
      int num23 = MyUtils.StringToBool(strArray12[index11]) ? 1 : 0;
      robotInterface3D11.gamepad1_dpad_left = num23 != 0;
      RobotInterface3D robotInterface3D12 = robot;
      string[] strArray13 = strArray1;
      int index12 = num22;
      int num24 = index12 + 1;
      int num25 = MyUtils.StringToBool(strArray13[index12]) ? 1 : 0;
      robotInterface3D12.gamepad1_dpad_right = num25 != 0;
      RobotInterface3D robotInterface3D13 = robot;
      string[] strArray14 = strArray1;
      int index13 = num24;
      int num26 = index13 + 1;
      int num27 = MyUtils.StringToBool(strArray14[index13]) ? 1 : 0;
      robotInterface3D13.gamepad1_right_bumper = num27 != 0;
      RobotInterface3D robotInterface3D14 = robot;
      string[] strArray15 = strArray1;
      int index14 = num26;
      int num28 = index14 + 1;
      int num29 = MyUtils.StringToBool(strArray15[index14]) ? 1 : 0;
      robotInterface3D14.gamepad1_left_bumper = num29 != 0;
      RobotInterface3D robotInterface3D15 = robot;
      string[] strArray16 = strArray1;
      int index15 = num28;
      int num30 = index15 + 1;
      double num31 = (double) float.Parse(strArray16[index15]);
      robotInterface3D15.gamepad1_left_trigger = (float) num31;
      RobotInterface3D robotInterface3D16 = robot;
      string[] strArray17 = strArray1;
      int index16 = num30;
      int num32 = index16 + 1;
      double num33 = (double) float.Parse(strArray17[index16]);
      robotInterface3D16.gamepad1_right_trigger = (float) num33;
      RobotInterface3D robotInterface3D17 = robot;
      string[] strArray18 = strArray1;
      int index17 = num32;
      int num34 = index17 + 1;
      int num35 = MyUtils.StringToBool(strArray18[index17]) ? 1 : 0;
      robotInterface3D17.gamepad1_stop = num35 != 0;
      RobotInterface3D robotInterface3D18 = robot;
      string[] strArray19 = strArray1;
      int index18 = num34;
      int num36 = index18 + 1;
      int num37 = MyUtils.StringToBool(strArray19[index18]) ? 1 : 0;
      robotInterface3D18.gamepad1_restart = num37 != 0;
    }
    catch (Exception ex)
    {
      MyUtils.LogMessageToFile("OnClientInputReceive error: " + (object) ex);
      return false;
    }
    return true;
  }

  private void OnClientFlags(int cnnId, string[] rawData)
  {
    if (GLOBALS.UDP_LOGGING)
    {
      MyUtils.LogMessageToFile(DateTime.Now.ToString() + " UDP LOG: OnClientFlags with cnnId = " + (object) cnnId);
      for (int index = 0; index < rawData.Length - 1; ++index)
        MyUtils.LogMessageToFile("    " + (object) index + "=" + rawData[index]);
    }
    if (!this.allClients.TryGetValue(cnnId, out ServerLow.Client _))
      return;
    this.allClients[cnnId].flags.Clear();
    string str1 = rawData[1];
    char[] chArray1 = new char[1]{ '\u0013' };
    foreach (string str2 in str1.Split(chArray1))
    {
      char[] chArray2 = new char[1]{ '\u0012' };
      string[] strArray = str2.Split(chArray2);
      if (strArray.Length < 4)
        break;
      int num1 = int.Parse(strArray[0]);
      int num2 = int.Parse(strArray[1]);
      if (num1 == num2)
      {
        if (num1 <= this.allClients[cnnId].server_confirmed_this_msg_id)
        {
          if (num1 + 60 < this.allClients[cnnId].server_confirmed_this_msg_id)
            this.allClients[cnnId].server_confirmed_this_msg_id = num1;
        }
        else
        {
          this.allClients[cnnId].server_confirmed_this_msg_id = num1;
          for (int index = 2; index < strArray.Length - 1; index += 2)
            this.allClients[cnnId].flags[strArray[index]] = strArray[index + 1];
          if (this.allClients[cnnId].flags.ContainsKey("TRACKED_FLAG_ID"))
          {
            this.allClients[cnnId].client_confirmed_this_msg_id = int.Parse(this.allClients[cnnId].flags["TRACKED_FLAG_ID"]);
            if (this.allClients[cnnId].client_confirmed_this_msg_id > this.allClients[cnnId].curr_tracked_packet)
              this.allClients[cnnId].client_confirmed_this_msg_id = this.allClients[cnnId].curr_tracked_packet;
          }
          this.ProcessClientFlags();
        }
      }
    }
  }

  public void AddNewClient(string[] data, IPEndPoint endpoint)
  {
    if (data.Length < 14)
    {
      this.SendUdp("ERR\u0011" + ServerLow.message_id.ToString() + "\u0011INVALID\u0011Invalid server request! ", endpoint);
    }
    else
    {
      string key = data[1];
      string str1 = data[2];
      string requested_position = data[3];
      string requested_robot = data[4];
      string str2 = data[5];
      string str3 = data[6];
      string str4 = data[7];
      string s1 = data[8];
      string s2 = data[9];
      string s3 = data[10];
      string s4 = data[11];
      string str5 = data[12];
      string str6 = data[13];
      string str7 = data[14];
      string str8 = "0";
      bool flag = false;
      if (data.Length >= 16)
        str8 = data[15];
      string str9 = "";
      if (data.Length >= 17)
        str9 = data[16];
      if (this.PASSWORD.Length > 0 && this.PASSWORD != str1 && requested_position != "Admin")
        this.SendUdp("ERR\u0011" + ServerLow.message_id.ToString() + "\u0011" + key + "\u0011Invalid Password! ", endpoint);
      else if (str2 != GLOBALS.GAME)
        this.SendUdp("ERR\u0011" + ServerLow.message_id.ToString() + "\u0011" + key + "\u0011Wrong game: server is running  " + GLOBALS.GAME + "!", endpoint);
      else if (str3 != Application.version)
        this.SendUdp("ERR\u0011" + ServerLow.message_id.ToString() + "\u0011" + key + "\u0011Version mis-match: server is running " + Application.version + "!", endpoint);
      else if (requested_position == "Spectator" && this.GetSpectatorCount() >= this.max_spectators)
        this.SendUdp("ERR\u0011" + ServerLow.message_id.ToString() + "\u0011" + key + "\u0011Maximum spectator count reached! ", endpoint);
      else if (requested_position == "Admin" && (this.ADMIN.Length < 1 || str1 != this.ADMIN))
        this.SendUdp("ERR\u0011" + ServerLow.message_id.ToString() + "\u0011" + key + "\u0011Incorrect admin credentials! ", endpoint);
      else if (requested_position == "Admin" && this.GetAdminSpecCount() >= 2 && this.GetSpectatorCount() >= this.max_spectators)
      {
        this.SendUdp("ERR\u0011" + ServerLow.message_id.ToString() + "\u0011" + key + "\u0011Maximum admin count reached! ", endpoint);
      }
      else
      {
        if (requested_position == "Admin")
        {
          requested_position = "Spectator";
          flag = true;
        }
        if (this.ADMIN.Length >= 1 && key == this.ADMIN)
          flag = true;
        Transform transform1 = (Transform) null;
        if (requested_position != "Spectator")
        {
          List<string> used_positions = new List<string>();
          foreach (ServerLow.Client client in this.allClients.Values)
            used_positions.Add(client.starting_position);
          transform1 = this.scorer.CorrectRobotPosition(requested_position, used_positions);
          if ((bool) (UnityEngine.Object) transform1)
            requested_position = transform1.name;
        }
        if (requested_position != "Spectator" && (UnityEngine.Object) transform1 == (UnityEngine.Object) null)
        {
          this.SendUdp("ERR\u0011" + ServerLow.message_id.ToString() + "\u0011" + key + "\u0011Position " + requested_position + " already taken!", endpoint);
        }
        else
        {
          foreach (ServerLow.Client client in this.allClients.Values)
          {
            if (client.name == key)
            {
              if (client.endpoint == endpoint)
                return;
              this.SendUdp("ERR\u0011" + ServerLow.message_id.ToString() + "\u0011" + key + "\u0011Name " + key + " already taken!", endpoint);
              return;
            }
          }
          int clientIdcounter = this.clientIDcounter;
          GLOBALS.client_names[clientIdcounter] = key;
          GLOBALS.client_ids[key] = clientIdcounter;
          ++this.clientIDcounter;
          ServerLow.Client newclient = new ServerLow.Client();
          newclient.name = key;
          this.allClients.Add(clientIdcounter, newclient);
          if (requested_position != "Spectator")
            requested_robot = this.scorer.CorrectRobotChoice(requested_robot);
          newclient.id = clientIdcounter;
          newclient.starting_position = requested_position;
          newclient.endpoint = endpoint;
          newclient.robotmodel = requested_robot;
          newclient.DriveTrain = str4;
          newclient.skins = str8;
          newclient.robotskins = str9;
          float.TryParse(s1, out newclient.speed);
          float.TryParse(s2, out newclient.acceleration);
          float.TryParse(s3, out newclient.weight);
          float.TryParse(s4, out newclient.turning_scaler);
          bool.TryParse(str5, out newclient.fieldcentric);
          bool.TryParse(str6, out newclient.activebreaking);
          bool.TryParse(str7, out newclient.tankcontrol);
          newclient.isAdmin = flag;
          if ((double) newclient.weight < 15.0)
            newclient.weight = 15f;
          if ((double) newclient.weight > 42.0)
            newclient.weight = 42f;
          if ((double) newclient.speed * (double) newclient.acceleration * (double) newclient.weight > 2600.0)
          {
            newclient.speed = 6.28f;
            newclient.acceleration = 25.1f;
            newclient.weight = 15f;
          }
          newclient.flags = new Dictionary<string, string>();
          newclient.server_sent_packets = new Dictionary<int, string>();
          newclient.serverFlags = new Dictionary<string, string>();
          newclient.serverInterrupts = new Dictionary<string, string>();
          newclient.time_last_message = ServerLow.GetTimeMillis();
          Transform transform2 = this.transform;
          if ((UnityEngine.Object) transform1 != (UnityEngine.Object) null)
            newclient.starting_pos = transform1;
          if (requested_position != "Spectator" && (UnityEngine.Object) this.CreatePlayerAvatar(newclient) == (UnityEngine.Object) null)
          {
            this.SendUdp("ERR\u0011" + ServerLow.message_id.ToString() + "\u0011" + key + "\u0011Robot model " + requested_robot + " doesn't exist!", endpoint);
          }
          else
          {
            newclient.status_line = UnityEngine.Object.Instantiate<GameObject>(this.clientlineprefab);
            newclient.status_line.transform.Find("USER").GetComponent<UnityEngine.UI.Text>().text = newclient.name;
            newclient.status_line.transform.Find("Location").GetComponent<UnityEngine.UI.Text>().text = newclient.starting_position;
            newclient.status_line.transform.Find("IP").GetComponent<UnityEngine.UI.Text>().text = newclient.endpoint.Address.ToString();
            newclient.status_line.transform.Find("DATA").GetComponent<UnityEngine.UI.Text>().text = "1";
            newclient.status_line.transform.Find("Kick").GetComponent<UnityEngine.UI.Button>().onClick.AddListener((UnityAction) (() => this.RemoveClient(newclient.id)));
            if ((bool) (UnityEngine.Object) newclient.robot)
              this.scorer.AddPlayer(newclient.robot);
            MyUtils.LogMessageToFile("Player " + newclient.name + " joined on position " + newclient.starting_position + " from IP=" + newclient.endpoint.Address.ToString() + ". Total Players = " + (object) this.GetPlayerCount() + ", Specs = " + (object) this.GetSpectatorCount(), false);
            if (!flag || !(bool) (UnityEngine.Object) this.ourgamesettings)
              return;
            this.ourgamesettings.UpdateServer();
          }
        }
      }
    }
  }

  private GameObject CreatePlayerAvatar(ServerLow.Client client)
  {
    Vector3 position = client.starting_pos.position;
    Quaternion rotation = client.starting_pos.rotation;
    GameObject inobject = MyUtils.InstantiateRobot(client.robotmodel, position, rotation, client.skins, client.robotskins);
    inobject.name = "PLAYER" + client.id.ToString();
    client.avatar = inobject;
    RobotID robotId = inobject.AddComponent<RobotID>();
    robotId.starting_pos = client.starting_position;
    robotId.id = client.id;
    this.TurnOffInterpolationInObject(inobject);
    client.robot = inobject.GetComponent<RobotInterface3D>();
    client.robot.SetUserParameters(client.speed, client.acceleration, client.weight, client.DriveTrain, client.turning_scaler, client.fieldcentric ? 1 : 0, client.activebreaking ? 1 : 0, client.tankcontrol ? 1 : 0);
    client.robot.SetName(client.name);
    client.robot.SetColorFromPosition(client.starting_position);
    GameObject gameObject = GameObject.Find(client.starting_position + " Cam");
    if (!(bool) (UnityEngine.Object) gameObject)
      gameObject = GameObject.Find("Spectator Cam");
    client.camera_rotation = gameObject.transform.rotation;
    client.robot.fieldcentric_rotation = client.camera_rotation;
    client.robot.Initialize();
    this.scorer.FieldChanged();
    return inobject;
  }

  private void RemoveClient(int cnndId)
  {
    if (!this.allClients.ContainsKey(cnndId))
      return;
    MyUtils.LogMessageToFile("Removing " + this.allClients[cnndId].name, false);
    if ((UnityEngine.Object) this.allClients[cnndId].avatar != (UnityEngine.Object) null)
    {
      if ((bool) (UnityEngine.Object) this.allClients[cnndId].robot)
        this.allClients[cnndId].robot.deleted = true;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.allClients[cnndId].avatar);
      this.allClients[cnndId].avatar = (GameObject) null;
    }
    if ((UnityEngine.Object) this.allClients[cnndId].status_line != (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.allClients[cnndId].status_line);
      this.allClients[cnndId].status_line = (GameObject) null;
    }
    this.allClients.Remove(cnndId);
  }

  private void ResetPlayerPosition(int playerid, bool referee = false)
  {
    if (!this.scorer.ALLOW_RESET_POS || !referee && this.pos_reset_tracker.ContainsKey(playerid) && (double) Time.time - (double) this.pos_reset_tracker[playerid] < 1.0)
      return;
    this.pos_reset_tracker[playerid] = Time.time;
    this.scorer.PlayerReset(playerid, referee);
    this.ResetAvatarPosition(playerid);
  }

  private void ProcessClientFlags()
  {
    foreach (int key1 in this.allClients.Keys.ToArray<int>())
    {
      if (this.allClients.ContainsKey(key1))
      {
        ServerLow.Client allClient = this.allClients[key1];
        this.serverFlags.Remove(key1.ToString());
        foreach (string key2 in allClient.flags.Keys)
        {
          if (!(key2 == "RESTARTALL"))
          {
            if (!(key2 == "RESETMYPOS"))
            {
              if (!(key2 == "CHAT"))
              {
                if (key2 == "GameSettings" && (this.ADMIN.Length > 0 && allClient.name == this.ADMIN || allClient.isAdmin) && (bool) (UnityEngine.Object) this.ourgamesettings)
                  this.ourgamesettings.SetString(allClient.flags[key2]);
              }
              else if (!this.ProcessClientCommands(allClient.name, allClient.flags[key2], allClient.isAdmin))
              {
                this.ShowLogMessage(allClient.name + ": " + allClient.flags[key2]);
                this.AddChat(allClient.name + ": " + allClient.flags[key2]);
              }
            }
            else
              this.ResetPlayerPosition(allClient.id);
          }
          else if (ServerLow.GetTimeMillis() - allClient.lastrestart >= 155000L || this.scorer.timerstate != Scorekeeper.TimerState.RUNNING)
          {
            if (this.tournament_mode)
            {
              if (this.tournament_sm != ServerLow.TOURNAMENT_STATES.RUNNING && !this.holding_mode)
              {
                this.tournament_sm = ServerLow.TOURNAMENT_STATES.WAITING;
                allClient.lastrestart = ServerLow.GetTimeMillis();
                this.RestartLevel();
              }
              else
              {
                this.ShowLogMessage("Can't reset field while running tournament game (" + allClient.name + ")");
                this.AddChat("Can't reset field while running tournament game (" + allClient.name + ")");
                continue;
              }
            }
            else
            {
              allClient.lastrestart = ServerLow.GetTimeMillis();
              this.RestartLevel();
            }
            this.ShowLogMessage("Field reset by " + allClient.name);
            this.AddChat("Field reset by " + allClient.name);
          }
        }
        allClient.flags.Remove("RESTARTALL");
        allClient.flags.Remove("RESETMYPOS");
        allClient.flags.Remove("CHAT");
        allClient.flags.Remove("GameSettings");
      }
    }
  }

  private bool ProcessClientCommands(string name, string msg, bool isAdmin = false)
  {
    if (name != this.ADMIN && !isAdmin)
      return false;
    if (msg.StartsWith("/SERVER "))
    {
      string[] strArray = msg.Substring(8).Split('=');
      strArray[0] = strArray[0].TrimEnd();
      if (strArray[0] == "RESTART")
      {
        this.ServerMenu_RestartLevel();
        MyUtils.LogMessageToFile("/SERVER: Restarted game.", false);
      }
      else if (strArray[0] == "PASSWORD")
      {
        this.PASSWORD = strArray[1];
        MyUtils.LogMessageToFile("/SERVER: PASSWORD = " + this.PASSWORD, false);
      }
      else if (strArray[0] == "KICKALL")
      {
        int[] array = new int[this.allClients.Keys.Count];
        this.allClients.Keys.CopyTo(array, 0);
        foreach (int num in array)
        {
          if (this.allClients[num].name != this.ADMIN && !this.allClients[num].isAdmin)
            this.RemoveClient(num);
        }
        MyUtils.LogMessageToFile("/SERVER: Kicked all players.", false);
      }
      else if (strArray[0] == "KICK")
      {
        string str = strArray[1];
        bool flag = false;
        foreach (int key in this.allClients.Keys)
        {
          if (this.allClients[key].name == str)
          {
            this.RemoveClient(key);
            flag = true;
            break;
          }
        }
        if (!flag)
          MyUtils.LogMessageToFile("/SERVER: Kicked player " + str + " not found!", false);
        else
          MyUtils.LogMessageToFile("/SERVER: Kicked player " + str, false);
      }
      else if (strArray[0] == "KICKID")
      {
        int result = -1;
        if (!int.TryParse(strArray[1], out result))
        {
          MyUtils.LogMessageToFile("/SERVER: KICKID failed to extract id number from: " + strArray[1], false);
          return false;
        }
        if (!this.allClients.ContainsKey(result))
        {
          MyUtils.LogMessageToFile("/SERVER: KICKID player with id not found: " + strArray[1], false);
          return false;
        }
        string name1 = this.allClients[result].name;
        this.RemoveClient(result);
        MyUtils.LogMessageToFile("/SERVER: Kicked player " + name1, false);
      }
      else if (strArray[0] == "STOP")
      {
        this.scorer.SetTimerState(Scorekeeper.TimerState.STOPPED);
        MyUtils.LogMessageToFile("/SERVER: Timer stopped.", false);
      }
      else if (strArray[0] == "BLUEADJ")
      {
        int result = 0;
        if (int.TryParse(strArray[1], out result))
          this.scorer.score_blueadj = result;
        MyUtils.LogMessageToFile("/SERVER: Blue score adj = " + (object) result, false);
      }
      else if (strArray[0] == "REDADJ")
      {
        int result = 0;
        if (int.TryParse(strArray[1], out result))
          this.scorer.score_redadj = result;
        MyUtils.LogMessageToFile("/SERVER: Red score adj = " + (object) result, false);
      }
      else if (strArray[0] == "MESSAGE")
      {
        if (strArray.Length < 3)
        {
          MyUtils.LogMessageToFile("/SERVER: MESSAGE missing 2 '=' signs. ", false);
          return false;
        }
        int result = -1;
        if (!int.TryParse(strArray[1], out result))
        {
          MyUtils.LogMessageToFile("/SERVER: MESSAGE failed to extract id number from: " + strArray[1], false);
          return false;
        }
        if (!this.allClients.ContainsKey(result))
        {
          MyUtils.LogMessageToFile("/SERVER: MESSAGE player with id not found: " + strArray[1], false);
          return false;
        }
        string msg1 = string.Join("=", strArray, 2, strArray.Length - 2);
        this.AddChat(msg1, result);
        MyUtils.LogMessageToFile("/SERVER: Message sent to player " + this.allClients[result].name + " = " + msg1, false);
      }
      else if (strArray[0] == "RESET")
      {
        int result = -1;
        if (!int.TryParse(strArray[1], out result))
        {
          MyUtils.LogMessageToFile("/SERVER: RESET failed to extract id number from: " + strArray[1], false);
          return false;
        }
        if (!this.allClients.ContainsKey(result))
        {
          MyUtils.LogMessageToFile("/SERVER: RESET player with id not found: " + strArray[1], false);
          return false;
        }
        string name2 = this.allClients[result].name;
        this.ResetPlayerPosition(result, true);
        MyUtils.LogMessageToFile("/SERVER: Ref reset player " + name2, false);
      }
      else if (strArray[0] == "ROBOTCOUNTER")
      {
        if (strArray.Length < 3)
        {
          MyUtils.LogMessageToFile("/SERVER: ROBOTCOUNTER missing 2 '=' signs. ", false);
          return false;
        }
        int result1 = -1;
        if (!int.TryParse(strArray[1], out result1))
        {
          MyUtils.LogMessageToFile("/SERVER: ROBOTCOUNTER failed to extract id number from: " + strArray[1], false);
          return false;
        }
        if (!this.allClients.ContainsKey(result1))
        {
          MyUtils.LogMessageToFile("/SERVER: ROBOTCOUNTER player with id not found: " + strArray[1], false);
          return false;
        }
        float result2 = -1f;
        if (!float.TryParse(strArray[2], out result2))
        {
          MyUtils.LogMessageToFile("/SERVER: ROBOTCOUNTER failed to extract duration from: " + strArray[2], false);
          return false;
        }
        this.scorer.SetRobotCounter(result1, result2);
        MyUtils.LogMessageToFile("/SERVER: Robot counter set on " + this.allClients[result1].name + " = " + strArray[2], false);
      }
      else if (strArray[0] == "ROBOTCOUNTERRESET")
      {
        if (strArray.Length < 2)
        {
          MyUtils.LogMessageToFile("/SERVER: ROBOTCOUNTERRESET missing 1 '=' signs. ", false);
          return false;
        }
        int result = -1;
        if (!int.TryParse(strArray[1], out result))
        {
          MyUtils.LogMessageToFile("/SERVER: ROBOTCOUNTERRESET failed to extract id number from: " + strArray[1], false);
          return false;
        }
        if (!this.allClients.ContainsKey(result))
        {
          MyUtils.LogMessageToFile("/SERVER: ROBOTCOUNTERRESET player with id not found: " + strArray[1], false);
          return false;
        }
        this.scorer.ResetRobotCounter(result);
        MyUtils.LogMessageToFile("/SERVER: Robot counter reset on " + this.allClients[result].name, false);
      }
      else
        this.ProcessCommand(strArray[0], strArray.Length > 1 ? strArray[1] : "");
      if ((bool) (UnityEngine.Object) this.top_application_manager)
        this.top_application_manager.UpdateMenuesToServerSettings();
    }
    else
    {
      this.ShowLogMessage("<ADMIN>: " + msg);
      this.AddChat("<ADMIN>: " + msg);
    }
    return true;
  }

  public void playSound(int id, string soundname, float crossFadeTime, float volume, float pitch = -1f)
  {
    string str = id.ToString() + "\u0015" + soundname + "\u0015" + crossFadeTime.ToString("0.#") + "\u0015" + volume.ToString("0.#") + "\u0015" + pitch.ToString("0.#");
    if (this.serverInterrupts.ContainsKey("SOUND_PLAY"))
    {
      Dictionary<string, string> serverInterrupts = this.serverInterrupts;
      serverInterrupts["SOUND_PLAY"] = serverInterrupts["SOUND_PLAY"] + "\u0014" + str;
    }
    else
      this.serverInterrupts["SOUND_PLAY"] = str;
  }

  public void stopSound(int id, string soundname, float crossFadeTime)
  {
    string str = id.ToString() + "\u0015" + soundname + "\u0015" + crossFadeTime.ToString("0.#");
    if (this.serverInterrupts.ContainsKey("SOUND_STOP"))
    {
      Dictionary<string, string> serverInterrupts = this.serverInterrupts;
      serverInterrupts["SOUND_STOP"] = serverInterrupts["SOUND_STOP"] + "\u0014" + str;
    }
    else
      this.serverInterrupts["SOUND_STOP"] = str;
  }

  private void ClearSoundFlags()
  {
    this.serverFlags.Remove("SOUND_STOP");
    this.serverFlags.Remove("SOUND_PLAY");
  }

  private void DataReceived(IAsyncResult ar)
  {
    if (ServerLow.killme)
      return;
    UdpClient asyncState = (UdpClient) ar.AsyncState;
    ServerLow.Message message = new ServerLow.Message();
    message.endpoint = new IPEndPoint(IPAddress.Any, 0);
    try
    {
      message.data = asyncState.EndReceive(ar, ref message.endpoint);
      if (this.allReceivedDataSemaphore.WaitOne())
      {
        this.allReceivedData.Add(message);
        this.allReceivedDataSemaphore.Release();
      }
    }
    catch (Exception ex)
    {
      if (this.datareceivederrors < 10)
        MyUtils.LogMessageToFile("DR Exception: " + ex.ToString(), false);
      ++this.datareceivederrors;
    }
    if (ServerLow.killme)
      return;
    asyncState.BeginReceive(new AsyncCallback(this.DataReceived), ar.AsyncState);
  }

  private void DataReceive()
  {
    while (!ServerLow.killme)
    {
      int millisecondsTimeout = 0;
      IPEndPoint remoteEP;
      byte[] numArray;
      try
      {
        this.udpSemaphore.WaitOne();
        if (this.m_udpClient.Available < 1)
        {
          millisecondsTimeout = 1;
          continue;
        }
        remoteEP = new IPEndPoint(IPAddress.Any, 0);
        numArray = this.m_udpClient.Receive(ref remoteEP);
      }
      catch (Exception ex)
      {
        MyUtils.LogMessageToFile("DR GLOBAL udpClient Exception: " + ex.ToString(), false);
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
        ServerLow.Message message = new ServerLow.Message();
        message.endpoint = remoteEP;
        message.data = numArray;
        this.allReceivedDataSemaphore.WaitOne();
        this.allReceivedData.Add(message);
        this.allReceivedDataSemaphore.Release();
        Thread.Sleep(0);
      }
    }
  }

  private void DataReceiveSerial()
  {
    if (ServerLow.killme || this.m_udpClient == null)
      return;
    while (this.m_udpClient.Available > 10)
    {
      IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
      try
      {
        byte[] numArray = this.m_udpClient.Receive(ref remoteEP);
        this.allReceivedData.Add(new ServerLow.Message()
        {
          endpoint = remoteEP,
          data = numArray
        });
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

  public void SetupUdp(int port = 14446)
  {
    this.remoteEP = new IPEndPoint(IPAddress.Any, port);
    this.m_udpClient = new UdpClient(this.remoteEP);
    this.m_udpClient.DontFragment = false;
    MyUtils.LogMessageToFile("done setting up udp", false);
  }

  private void ResetDataCounter(bool force = false)
  {
    if (!force && (double) (ServerLow.GetTimeMillis() - this.datacounter_old) <= (double) GLOBALS.UDP_DELAY_TIME_MS)
      return;
    this.datacounter = 0L;
    this.datacounter_old = ServerLow.GetTimeMillis();
  }

  private void UDPSendSleep(float durations_ms)
  {
    long timeMillis = ServerLow.GetTimeMillis();
    while ((double) (ServerLow.GetTimeMillis() - timeMillis) < (double) durations_ms)
    {
      this.udp_sleep_lastime = ServerLow.GetTimeMillis();
      if (GLOBALS.UDP_ALGORITHM == 2)
        this.DataReceiveSerial();
    }
  }

  public bool SendUdp(
    string messageraw,
    IPEndPoint destination = null,
    int Compression = 1,
    SendDataFlags who = SendDataFlags.EVERYONE)
  {
    string key1 = "";
    if (GLOBALS.NETSTATS > 0)
    {
      long num = MyUtils.GetTimeMillis() - this.STATS_LAST_TIME;
      if (GLOBALS.NETSTATS == 1)
        key1 = "DATA OUT";
      else
        key1 = messageraw.Split('\u0011')[0];
      if (num > this.STATS_PERIOD)
      {
        this.STATS_LAST_TIME = MyUtils.GetTimeMillis();
        if (GLOBALS.NETSTATS == 2)
          MyUtils.LogMessageToFile("NETSTAT (ms): Period=" + (object) num, false);
        foreach (string key2 in this.stats_outgoing.Keys)
          MyUtils.LogMessageToFile("NETSTAT (B/s): " + key2 + "=" + (this.stats_outgoing[key2] * 1000L / num).ToString("0"), false);
        this.stats_outgoing.Clear();
      }
      if (!this.stats_outgoing.ContainsKey(key1))
        this.stats_outgoing[key1] = 0L;
    }
    int num1 = (int) (1000.0 * ((double) Time.fixedTime - 86400.0 * (double) this.time_day_count));
    if (num1 / 1000 > 86400)
      ++this.time_day_count;
    if (this.datacounter > this.MAX_BYTES)
    {
      this.UDPSendSleep(this.UDP_SLEEP);
      this.ResetDataCounter(true);
    }
    else
      this.ResetDataCounter();
    byte[] second = MyUtils.CompressMessage(messageraw, Compression);
    if (second[0] == (byte) 52 && second[1] == (byte) 46 && second[2] == (byte) 53 && second[3] == (byte) 17 && second[4] == (byte) 52)
    {
      int num2 = (int) second[5];
    }
    byte[] dgram = MyUtils.CombineByteArrays(Encoding.UTF8.GetBytes("7p0\u0011" + Compression.ToString() + "\u0011" + num1.ToString() + "\u0011" + second.Length.ToString() + "\u0011"), second);
    if (destination != null)
    {
      int num3;
      try
      {
        this.udpSemaphore.WaitOne();
        num3 = this.m_udpClient.Send(dgram, dgram.Length, destination);
        this.sent_data_count += (long) num3;
        this.datacounter += (long) num3;
      }
      catch (Exception ex)
      {
        MyUtils.LogMessageToFile("exeption when sending to player  : " + ex.ToString());
        return false;
      }
      finally
      {
        this.udpSemaphore.Release();
      }
      if (GLOBALS.NETSTATS > 0)
        this.stats_outgoing[key1] += (long) num3;
      return true;
    }
    List<int> list = this.allClients.Keys.ToList<int>();
    ServerLow.Shuffle((IList<int>) list);
    foreach (int key3 in list)
    {
      if ((who != SendDataFlags.PLAYERS || !(this.allClients[key3].starting_position == "Spectator")) && (who != SendDataFlags.SPECTATORS || !(this.allClients[key3].starting_position != "Spectator")))
      {
        if (GLOBALS.ENABLE_UDP_STATS && this.netmonitor_header != null)
        {
          string key4 = this.netmonitor_header + "_" + (object) key3 + "_in";
          if (this.netmonitor.ContainsKey(key4))
            ++this.netmonitor[key4];
          else
            this.netmonitor.Add(key4, 1);
        }
        try
        {
          this.udpSemaphore.WaitOne();
          int num4 = this.m_udpClient.Send(dgram, dgram.Length, this.allClients[key3].endpoint);
          this.sent_data_count += (long) num4;
          this.datacounter += (long) num4;
          if (GLOBALS.NETSTATS > 0)
            this.stats_outgoing[key1] += (long) num4;
          if (GLOBALS.ENABLE_UDP_STATS)
          {
            if (this.netmonitor_header != null)
            {
              string key5 = this.netmonitor_header + "_" + (object) key3 + "_out";
              if (this.netmonitor.ContainsKey(key5))
                ++this.netmonitor[key5];
              else
                this.netmonitor.Add(key5, 1);
            }
          }
        }
        catch (Exception ex)
        {
          MyUtils.LogMessageToFile("exeption when sending to player" + this.allClients[key3].name + " : " + ex.ToString());
          this.allClients.Remove(key3);
        }
        finally
        {
          this.udpSemaphore.Release();
        }
        if (this.datacounter > this.MAX_BYTES)
        {
          this.UDPSendSleep(this.UDP_SLEEP);
          this.ResetDataCounter(true);
        }
        else
          this.ResetDataCounter();
      }
    }
    return true;
  }

  public bool SendFlagsUDPTracked(
    string messageraw,
    int Compression = 1,
    SendDataFlags who = SendDataFlags.EVERYONE,
    int receiver_id = -1)
  {
    List<int> list;
    if (who == SendDataFlags.TARGETID)
    {
      list = new List<int>();
      list.Add(receiver_id);
    }
    else
    {
      list = this.allClients.Keys.ToList<int>();
      ServerLow.Shuffle((IList<int>) list);
    }
    foreach (int key1 in list)
    {
      if ((who != SendDataFlags.PLAYERS || !(this.allClients[key1].starting_position == "Spectator")) && (who != SendDataFlags.SPECTATORS || !(this.allClients[key1].starting_position != "Spectator")))
      {
        ServerLow.Client allClient = this.allClients[key1];
        ++allClient.curr_tracked_packet;
        allClient.server_sent_packets[allClient.curr_tracked_packet] = allClient.curr_tracked_packet.ToString() + "\u0012" + allClient.curr_tracked_packet.ToString() + "\u0012" + messageraw;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("FLG\u0011");
        bool flag = false;
        int key2 = allClient.client_confirmed_this_msg_id + 1;
        if (allClient.curr_tracked_packet - key2 > 30)
          key2 = allClient.curr_tracked_packet - 30 + 1;
        if (key2 > allClient.curr_tracked_packet)
          key2 = allClient.curr_tracked_packet;
        for (; key2 <= allClient.curr_tracked_packet; ++key2)
        {
          if (allClient.server_sent_packets.ContainsKey(key2))
          {
            if (flag)
              stringBuilder.Append('\u0013');
            else
              flag = true;
            stringBuilder.Append(allClient.server_sent_packets[key2] + "\u0012TRACKED_FLAG_ID\u0012" + (object) allClient.server_confirmed_this_msg_id);
          }
        }
        this.SendUdp(stringBuilder.ToString(), allClient.endpoint, Compression);
        int num = allClient.curr_tracked_packet - 30 + 1;
        foreach (int key3 in allClient.server_sent_packets.Keys.ToList<int>())
        {
          if (key3 < num)
            allClient.server_sent_packets.Remove(key3);
        }
      }
    }
    return true;
  }

  private void DEBUG_print_flags_udp(int currclient, string message)
  {
    MyUtils.LogMessageToFile("SC: Client last received: client_confirmed_this_msg_id = " + (object) this.allClients[currclient].client_confirmed_this_msg_id);
    MyUtils.LogMessageToFile("SC: Server current id: curr_tracked_packet = " + (object) this.allClients[currclient].curr_tracked_packet);
    MyUtils.LogMessageToFile("CS: Server last received: server_confirmed_this_msg_id = " + (object) this.allClients[currclient].server_confirmed_this_msg_id);
    MyUtils.LogMessageToFile("Fifo buffer size: server_sent_packets = " + (object) this.allClients[currclient].server_sent_packets.Count);
    MyUtils.LogMessageToFile("  curr_tracked_packet = " + (object) this.allClients[currclient].curr_tracked_packet);
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

  public static void Shuffle(IList<int> list)
  {
    int count = list.Count;
    while (count > 1)
    {
      --count;
      int index = ServerLow.rng.Next(count + 1);
      int num = list[index];
      list[index] = list[count];
      list[count] = num;
    }
  }

  public bool SendServerInfoUdp(string messageraw, IPEndPoint destination)
  {
    byte[] bytes = Encoding.UTF8.GetBytes("11115\u0011" + messageraw);
    try
    {
      this.udpSemaphore.WaitOne();
      this.m_udpClient.Send(bytes, bytes.Length, destination);
    }
    catch (Exception ex)
    {
      MyUtils.LogMessageToFile("exeption when sending server info!");
      return false;
    }
    finally
    {
      this.udpSemaphore.Release();
    }
    return true;
  }

  public class CacheString
  {
    public StringBuilder msg = new StringBuilder();
    public int count;

    public CacheString(string inmsg = "") => this.msg.Append(inmsg);

    public void Clear()
    {
      this.msg.Clear();
      this.count = 0;
    }
  }

  private enum TOURNAMENT_STATES
  {
    WAITING,
    HYPE1,
    COUNTDOWN,
    RUNNING,
    END,
    ALLDONE,
  }

  private class LogLine
  {
    public GameObject TextLine;
    public long time_of_message;
  }

  public class Message
  {
    public byte[] data;
    public IPEndPoint endpoint;
  }

  public class Client
  {
    public IPEndPoint endpoint;
    public int id = -1;
    public string name = "";
    public long time_last_message = -1;
    public int message_count;
    public int client_confirmed_this_msg_id = -1;
    public int server_confirmed_this_msg_id = 1;
    public GameObject avatar;
    public RobotInterface3D robot;
    public string starting_position = "";
    public bool isAdmin;
    public float reset_release;
    public Transform starting_pos;
    public Quaternion camera_rotation;
    public string robotmodel = "";
    public string robotskins = "";
    public string skins = "0";
    public string DriveTrain = "Tank";
    public float speed;
    public float acceleration;
    public float weight;
    public float turning_scaler;
    public bool fieldcentric;
    public bool activebreaking;
    public bool tankcontrol;
    public Dictionary<string, string> flags;
    public Dictionary<int, string> server_sent_packets;
    public int curr_tracked_packet = 1;
    public GameObject status_line;
    public long lastrestart;
    public Dictionary<string, string> serverFlags;
    public Dictionary<string, string> serverInterrupts;
  }
}
