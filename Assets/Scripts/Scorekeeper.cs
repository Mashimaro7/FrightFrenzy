// Decompiled with JetBrains decompiler
// Type: Scorekeeper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Scorekeeper : MonoBehaviour
{
  public static Scorekeeper instance;
  private Text timer;
  private TextMesh field_timer;
  private Text timer_status;
  private GameObject timer_reset;
  public TimeSpan timer_elapsed;
  private GameObject countdown;
  private GameStartAnimation gamestartanimation;
  private GameObject fireworks;
  private long time_last_raw;
  public TimeSpan time_total;
  public bool ALLOW_RESET_POS = true;
  public bool clean_run;
  public string cleancode = "";
  public long OPR_UPDATE_TIME = 500;
  public List<RobotInterface3D> allrobots = new List<RobotInterface3D>();
  public List<float> allrobots_timecodes = new List<float>();
  public Dictionary<int, RobotInterface3D> allrobots_byid = new Dictionary<int, RobotInterface3D>();
  public Dictionary<string, RobotInterface3D> allrobots_byname = new Dictionary<string, RobotInterface3D>();
  public Dictionary<int, RobotID> allRobotID = new Dictionary<int, RobotID>();
  public Dictionary<string, float> player_opr = new Dictionary<string, float>();
  public Scorekeeper.TimerState timerstate;
  public Scorekeeper.FirstGameState firstgamestate = Scorekeeper.FirstGameState.READY;
  public int score_redfinal;
  public int score_bluefinal;
  public int score_redadj;
  public int score_blueadj;
  public bool shadowmode;
  protected Vector3 floor_max = new Vector3(1f, 1f, 1f);
  protected Vector3 floor_min = new Vector3(-1f, -1f, -1f);
  public bool floor_was_found;
  private int dofieldchanged = 1;
  public bool force_game_end;
  private SortedDictionary<string, float> player_opr_red = new SortedDictionary<string, float>();
  private SortedDictionary<string, float> player_opr_blue = new SortedDictionary<string, float>();
  private bool animation_override;
  private Dictionary<string, string> match_details = new Dictionary<string, string>();
  private Dictionary<string, Transform> allPositions = new Dictionary<string, Transform>();

  public void Start()
  {
    Scorekeeper.instance = this;
    this.ScorerInit();
    GameObject gameObject1 = GameObject.Find("ScoreBoard");
    GameObject gameObject2 = GameObject.Find("TIMER");
    if (!(bool) (UnityEngine.Object) gameObject2)
      return;
    CountdownFinder objectOfType = UnityEngine.Object.FindObjectOfType<CountdownFinder>();
    if ((bool) (UnityEngine.Object) objectOfType)
    {
      this.countdown = objectOfType.countdown;
      this.countdown.SetActive(false);
    }
    else
      this.countdown = (GameObject) null;
    GameStartAnimation[] objectsOfTypeAll = UnityEngine.Resources.FindObjectsOfTypeAll(typeof (GameStartAnimation)) as GameStartAnimation[];
    if (objectsOfTypeAll.Length >= 1)
    {
      this.gamestartanimation = objectsOfTypeAll[0];
      this.gamestartanimation.gameObject.SetActive(false);
    }
    else
      this.gamestartanimation = (GameStartAnimation) null;
    this.fireworks = GameObject.Find("Fireworks");
    Transform transform1 = gameObject2.transform.Find("Status");
    Transform transform2 = gameObject2.transform.Find("RESET");
    GameObject gameObject3 = GameObject.Find("FIELD_TIME");
    if ((bool) (UnityEngine.Object) gameObject3)
      this.field_timer = gameObject3.GetComponent<TextMesh>();
    if (!XRDevice.isPresent || (UnityEngine.Object) gameObject1 == (UnityEngine.Object) null)
    {
      if ((bool) (UnityEngine.Object) gameObject1)
        gameObject1.SetActive(false);
    }
    if ((UnityEngine.Object) transform1 != (UnityEngine.Object) null)
    {
      this.timer = gameObject2.GetComponent<Text>();
      this.timer_status = transform1.GetComponent<Text>();
      this.timer_reset = transform2.gameObject;
      this.SetTimerState(Scorekeeper.TimerState.READY);
    }
    this.score_redfinal = -1;
    this.score_bluefinal = -1;
    this.player_opr.Clear();
    GameObject gameObject4 = GameObject.Find("3d field/matts");
    if ((UnityEngine.Object) gameObject4 == (UnityEngine.Object) null)
      gameObject4 = GameObject.Find("3d field/floor");
    if (!((UnityEngine.Object) gameObject4 != (UnityEngine.Object) null))
      return;
    this.floor_max = gameObject4.GetComponent<Renderer>().bounds.max;
    this.floor_min = gameObject4.GetComponent<Renderer>().bounds.min;
    this.floor_was_found = true;
  }

  private void OnEnable()
  {
    if (this.floor_was_found)
      return;
    this.Start();
  }

  public virtual void ScorerInit()
  {
  }

  public void FieldChanged() => this.dofieldchanged = 1;

  public void DoFieldChanged()
  {
    this.allrobots.Clear();
    this.allrobots_timecodes.Clear();
    this.allrobots_byid.Clear();
    this.allrobots_byname.Clear();
    this.allRobotID.Clear();
    foreach (RobotInterface3D currbot in UnityEngine.Object.FindObjectsOfType<RobotInterface3D>())
    {
      if (!currbot.deleted && (bool) (UnityEngine.Object) currbot.GetComponent<RobotID>())
        this.AddPlayer(currbot, false);
    }
    this.FieldChangedTrigger();
    if (!(bool) (UnityEngine.Object) this.gamestartanimation)
      return;
    this.gamestartanimation.OnEnable();
  }

  public virtual void AddPlayer(RobotInterface3D currbot, bool robot_is_new = true)
  {
    if (!((UnityEngine.Object) currbot != (UnityEngine.Object) null) || !((UnityEngine.Object) currbot.myRobotID != (UnityEngine.Object) null))
      return;
    GameObject gameObject = currbot.gameObject;
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null || !gameObject.activeSelf || !gameObject.activeInHierarchy)
      return;
    if (!this.allrobots.Contains(currbot))
    {
      this.allrobots.Add(currbot);
      this.allrobots_timecodes.Add(-1f);
    }
    this.allrobots_byid[currbot.myRobotID.id] = currbot;
    this.allRobotID[currbot.myRobotID.id] = currbot.myRobotID;
    if (!GLOBALS.client_names.ContainsKey(currbot.myRobotID.id))
      return;
    this.allrobots_byname[GLOBALS.client_names[currbot.myRobotID.id]] = currbot;
  }

  public virtual void FieldChangedTrigger()
  {
  }

  public void Update()
  {
    if ((bool) (UnityEngine.Object) this.field_timer)
      this.field_timer.text = this.timer.text;
    if (!GLOBALS.now_paused && this.timerstate == Scorekeeper.TimerState.RUNNING)
    {
      long ticks = DateTime.Now.Ticks;
      this.timer_elapsed = TimeSpan.FromTicks(ticks - this.time_last_raw);
      this.time_last_raw = ticks;
      if (!GLOBALS.now_playing)
        this.time_total = this.time_total.Subtract(this.timer_elapsed);
      if (this.force_game_end || this.time_total.TotalMilliseconds <= 0.0)
      {
        this.force_game_end = false;
        if (this.time_total.TotalMilliseconds < 0.0)
          this.time_total = TimeSpan.FromSeconds(0.0);
        this.timer.color = Color.white;
        this.SetTimerState(Scorekeeper.TimerState.FINISHED);
        if (this.firstgamestate == Scorekeeper.FirstGameState.ENDGAME)
        {
          GameObject.Find("AudioManagerTop").GetComponent<AudioManager>().Play("GameEnd");
          this.firstgamestate = Scorekeeper.FirstGameState.FINISHED;
        }
      }
      else if (this.time_total.TotalSeconds <= (double) GLOBALS.TIMER_ENDGAME)
      {
        this.timer.color = Color.magenta;
        if (this.firstgamestate == Scorekeeper.FirstGameState.TELEOP)
        {
          GameObject.Find("AudioManagerTop").GetComponent<AudioManager>().Play("EndGame");
          this.firstgamestate = Scorekeeper.FirstGameState.ENDGAME;
        }
      }
      else if (this.time_total.TotalSeconds <= (double) (GLOBALS.TIMER_TOTAL - GLOBALS.TIMER_AUTO))
      {
        this.timer.color = Color.white;
        if (this.firstgamestate == Scorekeeper.FirstGameState.AUTO)
        {
          GameObject.Find("AudioManagerTop").GetComponent<AudioManager>().Play("EndAuto");
          this.firstgamestate = Scorekeeper.FirstGameState.TELEOP;
        }
      }
      else
      {
        this.timer.color = Color.green;
        this.firstgamestate = Scorekeeper.FirstGameState.AUTO;
      }
      if (GLOBALS.SINGLEPLAYER_MODE)
        this.timer.text = this.time_total.Minutes.ToString() + ":" + this.time_total.Seconds.ToString("D2") + "." + (this.time_total.Milliseconds / 100).ToString("D1");
      else
        this.timer.text = this.time_total.Minutes.ToString() + ":" + this.time_total.Seconds.ToString("D2");
    }
    else
      this.timer_elapsed = TimeSpan.Zero;
    this.ScorerUpdate();
    this.UpdateRobotCounter(true);
  }

  public void LateUpdate()
  {
    if (this.dofieldchanged <= 0)
      return;
    --this.dofieldchanged;
    if (this.dofieldchanged != 0)
      return;
    this.DoFieldChanged();
  }

  public virtual void ScorerUpdate()
  {
  }

  public virtual void ScorerReset()
  {
    this.player_opr.Clear();
    this.player_opr_red.Clear();
    this.player_opr_blue.Clear();
    this.DoFieldChanged();
    foreach (RobotInterface3D allrobot in this.allrobots)
    {
      if (!((UnityEngine.Object) allrobot.myRobotID == (UnityEngine.Object) null) && GLOBALS.client_names.ContainsKey(allrobot.myRobotID.id))
        this.player_opr[GLOBALS.client_names[allrobot.myRobotID.id]] = 0.0f;
    }
    this.score_redadj = 0;
    this.score_blueadj = 0;
    this.force_game_end = false;
  }

  public virtual void Restart()
  {
  }

  public virtual void PlayerReset(int id, bool referee = false)
  {
  }

  public virtual void GetScoreDetails(Dictionary<string, string> data)
  {
    data.Clear();
    if ((bool) (UnityEngine.Object) GLOBALS.topserver)
      data["NetFPS"] = GLOBALS.topserver.update_framerate.ToString();
    data["Timer"] = this.timer.text;
    data["GameState"] = this.firstgamestate.ToString();
    string str = "";
    foreach (string key in this.player_opr.Keys)
    {
      if (!this.allrobots_byname.ContainsKey(key) || (UnityEngine.Object) this.allrobots_byname[key] == (UnityEngine.Object) null || (UnityEngine.Object) this.allrobots_byname[key].myRobotID == (UnityEngine.Object) null)
        this.DoFieldChanged();
      if (this.allrobots_byname.ContainsKey(key) && !((UnityEngine.Object) this.allrobots_byname[key] == (UnityEngine.Object) null) && !((UnityEngine.Object) this.allrobots_byname[key].myRobotID == (UnityEngine.Object) null))
      {
        if (this.allrobots_byname[key].myRobotID.is_red)
          this.player_opr_red[key] = this.player_opr[key];
        else
          this.player_opr_blue[key] = this.player_opr[key];
      }
    }
    foreach (string key in this.player_opr_red.Keys)
    {
      str += key;
      if ((double) this.player_opr_red[key] != 0.0)
        str = str + ": " + this.player_opr_red[key].ToString("0");
      str += "\n";
    }
    for (int index = 3 - this.player_opr_red.Count; index >= 1; --index)
      str += "\n";
    foreach (string key in this.player_opr_blue.Keys)
    {
      str += key;
      if ((double) this.player_opr_blue[key] != 0.0)
        str = str + ": " + this.player_opr_blue[key].ToString("0");
      str += "\n";
    }
    data["OPR"] = str;
    data["RedADJ"] = this.score_redadj.ToString();
    data["BlueADJ"] = this.score_blueadj.ToString();
  }

  public virtual void SetScoreDetails(Dictionary<string, string> data)
  {
    if (!data.ContainsKey("OPR"))
      return;
    this.player_opr.Clear();
    string str1 = data["OPR"];
    char[] chArray1 = new char[1]{ '\n' };
    foreach (string str2 in str1.Split(chArray1))
    {
      char[] chArray2 = new char[1]{ ':' };
      string[] strArray = str2.Split(chArray2);
      if (strArray.Length == 2 && strArray[0].Length >= 1)
      {
        float result = 0.0f;
        if (float.TryParse(strArray[1], out result))
          this.player_opr[strArray[0]] = result;
      }
    }
    data.Remove("OPR");
  }

  public void OnTimerReset() => this.SetTimerState(Scorekeeper.TimerState.READY);

  public virtual void OnTimerStart()
  {
  }

  public void OnTimerClick()
  {
    if (this.shadowmode || (UnityEngine.Object) this.timer_status == (UnityEngine.Object) null || (UnityEngine.Object) this.timer == (UnityEngine.Object) null)
      return;
    switch (this.timerstate)
    {
      case Scorekeeper.TimerState.READY:
        this.SetTimerState(Scorekeeper.TimerState.RUNNING);
        GameObject.Find("AudioManagerTop").GetComponent<AudioManager>().Play("Start");
        this.OnTimerStart();
        break;
      case Scorekeeper.TimerState.STOPPED:
        this.SetTimerState(Scorekeeper.TimerState.READY);
        this.clean_run = false;
        this.cleancode = "";
        break;
      case Scorekeeper.TimerState.RUNNING:
        this.SetTimerState(Scorekeeper.TimerState.PAUSED);
        this.clean_run = false;
        this.cleancode = "";
        break;
      case Scorekeeper.TimerState.PAUSED:
        this.SetTimerState(Scorekeeper.TimerState.RUNNING);
        this.clean_run = false;
        this.cleancode = "";
        break;
      case Scorekeeper.TimerState.FINISHED:
        this.SetTimerState(Scorekeeper.TimerState.READY);
        this.clean_run = false;
        break;
    }
  }

  public void SetTimerState(Scorekeeper.TimerState state, bool server_request = false)
  {
    if (this.shadowmode && !server_request)
      return;
    switch (state)
    {
      case Scorekeeper.TimerState.READY:
        this.DoTimerReady();
        break;
      case Scorekeeper.TimerState.STOPPED:
        this.timer_status.text = "STOPPED";
        this.timer_status.color = Color.red;
        this.timer_status.gameObject.SetActive(true);
        this.timer_reset.SetActive(true);
        this.timerstate = Scorekeeper.TimerState.STOPPED;
        break;
      case Scorekeeper.TimerState.RUNNING:
        this.timer_status.text = "RUNNING";
        this.timer_status.color = Color.gray;
        this.timer_status.gameObject.SetActive(false);
        this.timer_reset.SetActive(false);
        this.time_last_raw = DateTime.Now.Ticks;
        this.timerstate = Scorekeeper.TimerState.RUNNING;
        break;
      case Scorekeeper.TimerState.PAUSED:
        this.timer_status.text = "PAUSED";
        this.timer_status.color = Color.yellow;
        this.timer_status.gameObject.SetActive(true);
        this.timer_reset.SetActive(true);
        this.timerstate = Scorekeeper.TimerState.PAUSED;
        break;
      case Scorekeeper.TimerState.FINISHED:
        this.DoTimerFinished();
        break;
    }
  }

  public virtual void DoTimerReady()
  {
    this.score_redfinal = 0;
    this.score_bluefinal = 0;
    this.time_total = TimeSpan.FromSeconds((double) GLOBALS.TIMER_TOTAL);
    Text timer = this.timer;
    int num = this.time_total.Minutes;
    string str1 = num.ToString();
    num = this.time_total.Seconds;
    string str2 = num.ToString("D2");
    string str3 = str1 + ":" + str2;
    timer.text = str3;
    this.timer_status.text = "READY";
    this.timer_status.color = Color.green;
    this.timer_status.gameObject.SetActive(true);
    this.timer_reset.SetActive(false);
    this.timerstate = Scorekeeper.TimerState.READY;
    this.ScorerReset();
    this.firstgamestate = Scorekeeper.FirstGameState.READY;
  }

  public virtual void DoTimerFinished()
  {
    this.timer_status.text = "FINISHED";
    this.timer_status.color = Color.green;
    this.timer_status.gameObject.SetActive(true);
    this.timer_reset.SetActive(false);
    this.score_redfinal = this.GetRedScore();
    this.score_bluefinal = this.GetBlueScore();
    this.timerstate = Scorekeeper.TimerState.FINISHED;
  }

  public string GetTimerText() => this.timer.text;

  public void SetTimerText(string text) => this.timer.text = text;

  public string GetTimerState() => this.timer_status.text;

  public void SetTimerState(string text)
  {
    Scorekeeper.TimerState state = (Scorekeeper.TimerState) Enum.Parse(typeof (Scorekeeper.TimerState), text);
    if (state == this.timerstate)
      return;
    this.SetTimerState(state, true);
  }

  public bool IsTimerFinished() => this.timerstate == Scorekeeper.TimerState.FINISHED || this.timerstate == Scorekeeper.TimerState.STOPPED;

  public virtual int GetRedScore()
  {
    int num = 0;
    return this.timerstate == Scorekeeper.TimerState.FINISHED ? this.score_redfinal : num;
  }

  public virtual int GetBlueScore()
  {
    int num = 0;
    return this.timerstate == Scorekeeper.TimerState.FINISHED ? this.score_bluefinal : num;
  }

  public void StartCountdown()
  {
    if ((bool) (UnityEngine.Object) this.countdown)
      this.countdown.SetActive(true);
    GameObject.Find("AudioManagerTop").GetComponent<AudioManager>().Play("Countdown");
  }

  public void ShowGameStartOverlay(bool enable = true, bool passivemode = false)
  {
    if ((bool) (UnityEngine.Object) this.gamestartanimation)
    {
      if (enable && this.gamestartanimation.gameObject.activeSelf || !enable && !this.gamestartanimation.gameObject.activeSelf)
        return;
      if (enable)
        this.gamestartanimation.gameObject.SetActive(true);
      else if (!passivemode || !this.animation_override)
      {
        this.gamestartanimation.EndAnimation();
        this.animation_override = false;
      }
    }
    if (enable)
      GameObject.Find("AudioManagerTop").GetComponent<AudioManager>().Play("Hype1");
    else
      GameObject.Find("AudioManagerTop").GetComponent<AudioManager>().Stop("Hype1", 0.0f);
  }

  public virtual void UpdateRobotCounter(bool only_if_smaller = false)
  {
    foreach (RobotID bot in this.allRobotID.Values)
    {
      if (bot.is_counting)
      {
        RobotInterface3D robotInterface3D = this.allrobots_byid[bot.id];
        if ((bool) (UnityEngine.Object) robotInterface3D)
        {
          float num = (bot.count_duration - bot.count_start + (float) this.time_total.TotalSeconds) / bot.count_duration;
          if (!only_if_smaller || (double) robotInterface3D.GetProgressBar() <= 0.0 || (double) num < (double) robotInterface3D.GetProgressBar())
            robotInterface3D.SetProgressBar(num);
          if ((double) num <= 0.0)
            this.RobotCounterExpired(bot);
        }
      }
    }
  }

  public virtual void RobotCounterExpired(RobotID bot)
  {
    bot.is_counting = false;
    bot.count_start = 0.0f;
    this.PlayerReset(bot.id, true);
    if (!(bool) (UnityEngine.Object) this.allrobots_byid[bot.id] || !(bool) (UnityEngine.Object) this.allrobots_byid[bot.id])
      return;
    this.allrobots_byid[bot.id].MarkForReset(1f);
  }

  public virtual void ResetRobotCounter(int id)
  {
    RobotID robotId = this.allRobotID[id];
    RobotInterface3D robotInterface3D = this.allrobots_byid[robotId.id];
    if ((bool) (UnityEngine.Object) robotInterface3D)
      robotInterface3D.SetProgressBar(0.0f);
    robotId.is_counting = false;
    robotId.count_start = 0.0f;
  }

  public virtual void SetRobotCounter(int id, float duration)
  {
    RobotID robotId = this.allRobotID[id];
    if ((double) duration > 0.0)
    {
      robotId.is_counting = true;
      robotId.count_start = (float) this.time_total.TotalSeconds;
      robotId.count_duration = duration;
    }
    else
      this.ResetRobotCounter(id);
  }

  public virtual bool UseGameStartOverlay() => true;

  public void ShowGameStartOverlayNoAnimation(bool enable = true)
  {
    if (!(bool) (UnityEngine.Object) this.gamestartanimation)
      return;
    if (enable)
    {
      this.animation_override = true;
      this.gamestartanimation.ShowNoAnimation(true);
      GameObject.Find("AudioManagerTop").GetComponent<AudioManager>().Play("Hype1", server_request: true);
    }
    else
    {
      this.animation_override = false;
      this.gamestartanimation.ShowNoAnimation(false);
      GameObject.Find("AudioManagerTop").GetComponent<AudioManager>().Stop("Hype1", 0.0f, true);
    }
  }

  public void StartFireworks(bool red)
  {
    if ((bool) (UnityEngine.Object) this.fireworks && !GLOBALS.HEADLESS_MODE)
    {
      if (red)
        this.fireworks.GetComponent<FireworksFinder>().fireworksRed.SetActive(true);
      else
        this.fireworks.GetComponent<FireworksFinder>().fireworksBlue.SetActive(true);
    }
    GameObject.Find("AudioManagerTop").GetComponent<AudioManager>().Play("fireworks");
  }

  public virtual void SendServerData(Dictionary<string, string> serverFlags)
  {
    serverFlags["REDSCORE"] = this.GetRedScore().ToString();
    serverFlags["BLUESCORE"] = this.GetBlueScore().ToString();
    serverFlags["TIMER"] = this.GetTimerText();
    serverFlags["TIMERSTATE"] = this.GetTimerState();
    serverFlags["TIMERAW"] = ((int) this.time_total.TotalMilliseconds).ToString();
    this.GetScoreDetails(this.match_details);
    serverFlags["SCORE"] = string.Join(";", this.match_details.Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (x => x.Key + "=" + x.Value)));
  }

  public virtual void ProcessScoreKey(Dictionary<string, string> serverFlags)
  {
    if (!serverFlags.ContainsKey("SCORE"))
      return;
    string[] strArray1 = serverFlags["SCORE"].Split(';');
    if (strArray1.Length < 1)
      return;
    for (int index = 0; index < strArray1.Length; ++index)
    {
      string[] strArray2 = strArray1[index].Split('=');
      if (strArray2.Length == 2)
        this.match_details[strArray2[0]] = strArray2[1];
    }
    this.SetScoreDetails(this.match_details);
  }

  public virtual void ReceiveServerData(Dictionary<string, string> serverFlags)
  {
    if (serverFlags.TryGetValue("TIMERAW", out string _))
    {
      this.time_total = TimeSpan.FromMilliseconds((double) int.Parse(serverFlags["TIMERAW"]));
      this.time_last_raw = DateTime.Now.Ticks;
      serverFlags.Remove("TIMERAW");
    }
    string text;
    if (serverFlags.TryGetValue("TIMERSTATE", out text))
    {
      this.SetTimerState(text);
      serverFlags.Remove("TIMERSTATE");
      if (this.timerstate == Scorekeeper.TimerState.RUNNING && (bool) (UnityEngine.Object) this.gamestartanimation && this.gamestartanimation.enabled)
        this.ShowGameStartOverlayNoAnimation(false);
    }
    this.ProcessScoreKey(serverFlags);
  }

  public virtual string CorrectRobotChoice(string requested_robot) => requested_robot;

  public virtual Transform CorrectRobotPosition(
    string requested_position,
    List<string> used_positions)
  {
    if (this.allPositions.Count < 1)
    {
      GameObject gameObject = GameObject.Find("Positions");
      if (!(bool) (UnityEngine.Object) gameObject)
        return (Transform) null;
      Transform transform = gameObject.transform;
      for (int index = transform.childCount - 1; index >= 0; --index)
        this.allPositions.Add(transform.GetChild(index).name, transform.GetChild(index));
      if (this.allPositions.Count < 1)
        return (Transform) null;
    }
    if (used_positions.Contains(requested_position))
      return (Transform) null;
    if (this.allPositions.ContainsKey(requested_position))
      return this.allPositions[requested_position];
    bool flag1 = requested_position.Contains("Blue");
    bool flag2 = requested_position.Contains("Red");
    foreach (string key in this.allPositions.Keys)
    {
      if ((!flag1 || key.Contains("Blue")) && (!flag2 || key.Contains("Red")) && !used_positions.Contains(key))
        return this.allPositions[key];
    }
    return (Transform) null;
  }

  public virtual bool CorrectFieldElement(GameObject currobj)
  {
    if (!this.floor_was_found)
    {
      GameObject gameObject = GameObject.Find("3d field/matts");
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
        gameObject = GameObject.Find("3d field/floor");
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      {
        Bounds bounds = gameObject.GetComponent<Renderer>().bounds;
        this.floor_max = bounds.max;
        bounds = gameObject.GetComponent<Renderer>().bounds;
        this.floor_min = bounds.min;
        this.floor_was_found = true;
      }
    }
    if ((double) currobj.transform.position.y >= (double) this.floor_min.y - 5.0)
      return false;
    currobj.transform.position = new Vector3(UnityEngine.Random.Range(this.floor_min.x, this.floor_max.x), this.floor_max.y + 20f, UnityEngine.Random.Range(this.floor_min.z, this.floor_max.z));
    Rigidbody component = currobj.GetComponent<Rigidbody>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      component.velocity = new Vector3(0.0f, 0.0f, 0.0f);
      component.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    }
    return true;
  }

  public virtual bool IsTransformOffField(Transform currobj)
  {
    if (!this.floor_was_found)
    {
      GameObject gameObject = GameObject.Find("3d field/matts");
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
        gameObject = GameObject.Find("3d field/floor");
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      {
        Bounds bounds = gameObject.GetComponent<Renderer>().bounds;
        this.floor_max = bounds.max;
        bounds = gameObject.GetComponent<Renderer>().bounds;
        this.floor_min = bounds.min;
        this.floor_was_found = true;
      }
    }
    return (double) currobj.position.y < (double) this.floor_min.y - 5.0;
  }

  public virtual void OnCameraViewChanged()
  {
  }

  public virtual void OnScorerInterrupt(string msg)
  {
  }

  public virtual Transform GetOverlays(int id, Transform parent = null) => (Transform) null;

  public virtual string GetOverlaysString(int id) => "";

  public virtual bool PU_CheckIfClearToAssign(PowerUpScript thepu, RobotInterface3D robot) => true;

  public enum TimerState
  {
    READY = 1,
    STOPPED = 2,
    RUNNING = 3,
    PAUSED = 4,
    FINISHED = 5,
  }

  public enum FirstGameState
  {
    READY = 1,
    AUTO = 2,
    TELEOP = 3,
    ENDGAME = 4,
    FINISHED = 5,
  }
}
