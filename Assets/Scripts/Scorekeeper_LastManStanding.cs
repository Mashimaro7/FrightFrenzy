// Decompiled with JetBrains decompiler
// Type: Scorekeeper_LastManStanding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scorekeeper_LastManStanding : Scorekeeper
{
  public LastManStanding_Settings lms_settings;
  private LSM_BallCollision[] allBalls;
  private Transform floor;
  public float floor_max_scale = 3f;
  public int TIME_AUTO = 30;
  private List<Transform> Flames = new List<Transform>();
  public int starting_players_blue;
  public int starting_players_red;
  public int curr_red_count;
  public int curr_blue_count;
  public int red_kills;
  public int blue_kills;
  public TextMeshProUGUI textScores;
  public GameObject textScores_panel;
  private string gameovercode = "";
  private float center_offset = 0.75f;
  public int mystate;
  public TextMeshProUGUI gameover_text;
  public GameObject gameoverpanel;
  private List<int> start_players = new List<int>();
  private List<int> dead_players = new List<int>();
  private List<int> live_players = new List<int>();
  public GameObject redtextobj;
  public GameObject bluetextobj;
  private Scorekeeper.TimerState timerstate_old = Scorekeeper.TimerState.READY;
  private bool escape_was_pressed;
  private int score_red;
  private int score_blue;
  private int mystate_old = -1;

  private void Awake()
  {
    GLOBALS.TIMER_TOTAL = 120;
    GLOBALS.TIMER_AUTO = this.TIME_AUTO;
    GLOBALS.TIMER_ENDGAME = -1;
    this.ALLOW_RESET_POS = false;
    if ((UnityEngine.Object) this.redtextobj == (UnityEngine.Object) null)
      this.redtextobj = GameObject.Find("REDSCORE");
    if ((UnityEngine.Object) this.bluetextobj == (UnityEngine.Object) null)
      this.bluetextobj = GameObject.Find("BLUESCORE");
    if (!(bool) (UnityEngine.Object) this.textScores_panel)
      return;
    this.textScores_panel.SetActive(false);
  }

  private bool ConfigureField()
  {
    if ((UnityEngine.Object) this.lms_settings == (UnityEngine.Object) null)
      return false;
    if (GLOBALS.SERVER_MODE && (bool) (UnityEngine.Object) GLOBALS.topserver)
      GLOBALS.topserver.AddInterrupt("SCORER", "CF");
    foreach (GameObject gameObject in new List<GameObject>()
    {
      GameObject.Find("CenterField/G1"),
      GameObject.Find("CenterField/G2"),
      GameObject.Find("CenterField/G3"),
      GameObject.Find("CenterField/G4")
    })
    {
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
        return false;
      for (int index = 0; index < gameObject.transform.childCount; ++index)
      {
        Transform child = gameObject.transform.GetChild(index);
        if (child.name.StartsWith("row"))
        {
          float num1 = float.Parse(child.name.Substring(3, 1));
          int num2 = int.Parse(child.name.Substring(5, 1));
          float num3 = num1 - (1f - this.center_offset);
          if (num2 > 5)
            num2 = 5 - num2;
          Vector3 localPosition = child.localPosition with
          {
            z = this.lms_settings.ROW_SPACING * num3,
            x = this.lms_settings.ROW_SPACING * (float) num2
          };
          child.localPosition = localPosition;
          if ((double) num3 >= (double) this.lms_settings.ROW_COUNT)
            child.gameObject.SetActive(false);
          else
            child.gameObject.SetActive(true);
        }
        else if (child.name.StartsWith("Protection"))
        {
          float num = float.Parse(child.name.Substring(10, 1));
          if ((double) num > 5.0)
            num = 5f - num;
          Vector3 localPosition = child.localPosition with
          {
            z = this.lms_settings.ROW_SPACING * ((float) this.lms_settings.ROW_COUNT - (1f - this.center_offset)),
            x = (float) ((double) this.lms_settings.ROW_SPACING * (double) (this.lms_settings.ROW_COUNT - 1) * (double) num / 2.0)
          };
          child.localPosition = localPosition;
          if (this.lms_settings.SPAWN_WALLS)
            child.gameObject.SetActive(true);
          else
            child.gameObject.SetActive(false);
        }
        else if (child.name.StartsWith("spawn"))
        {
          float num = float.Parse(child.name.Substring(5, 1));
          if ((double) num > 5.0)
            num = 5f - num;
          Vector3 localPosition = child.localPosition with
          {
            z = this.lms_settings.ROW_SPACING * ((float) this.lms_settings.ROW_COUNT - (1f - this.center_offset)),
            x = this.lms_settings.ROW_SPACING * (float) (this.lms_settings.ROW_COUNT - 1) * num
          };
          child.localPosition = localPosition;
          Transform transform = child.Find("Protection");
          if ((UnityEngine.Object) transform != (UnityEngine.Object) null)
            transform.gameObject.SetActive(this.lms_settings.SPAWN_WALLS);
        }
      }
    }
    this.floor_max_scale = (float) ((double) this.lms_settings.ROW_SPACING * ((double) this.lms_settings.ROW_COUNT - (1.0 - (double) this.center_offset)) / 5.0 + 0.75);
    List<Transform> transformList1 = new List<Transform>();
    List<Transform> transformList2 = new List<Transform>();
    foreach (Transform componentsInChild in GameObject.Find("CenterField/G2").transform.GetComponentsInChildren<Transform>(true))
    {
      if (componentsInChild.name.StartsWith("SpawnPoint"))
        transformList2.Add(componentsInChild);
      if (componentsInChild.name.StartsWith("spawnball"))
        transformList1.Add(componentsInChild);
    }
    foreach (Transform componentsInChild in GameObject.Find("CenterField/G1").transform.GetComponentsInChildren<Transform>(true))
    {
      if (componentsInChild.name.StartsWith("SpawnPoint"))
        transformList2.Add(componentsInChild);
      if (componentsInChild.name.StartsWith("spawnball"))
        transformList1.Add(componentsInChild);
    }
    List<Transform> transformList3 = new List<Transform>();
    foreach (Transform componentsInChild in GameObject.Find("CenterField/G4").transform.GetComponentsInChildren<Transform>(true))
    {
      if (componentsInChild.name.StartsWith("SpawnPoint"))
        transformList3.Add(componentsInChild);
      if (componentsInChild.name.StartsWith("spawnball"))
        transformList1.Add(componentsInChild);
    }
    foreach (Transform componentsInChild in GameObject.Find("CenterField/G3").transform.GetComponentsInChildren<Transform>(true))
    {
      if (componentsInChild.name.StartsWith("SpawnPoint"))
        transformList3.Add(componentsInChild);
      if (componentsInChild.name.StartsWith("spawnball"))
        transformList1.Add(componentsInChild);
    }
    GameObject gameObject1 = GameObject.Find("Positions");
    if (!(bool) (UnityEngine.Object) gameObject1)
      return false;
    List<Transform> transformList4 = new List<Transform>();
    List<Transform> transformList5 = new List<Transform>();
    Transform transform1 = gameObject1.transform;
    for (int index = transform1.childCount - 1; index >= 0; --index)
    {
      if (transform1.GetChild(index).name.StartsWith("Red"))
        transformList4.Add(transform1.GetChild(index));
      else
        transformList5.Add(transform1.GetChild(index));
    }
    for (int index = 0; index < transformList4.Count && index < transformList2.Count; ++index)
    {
      transformList4[index].position = transformList2[index].position;
      transformList4[index].rotation = transformList2[index].rotation;
    }
    for (int index = 0; index < transformList5.Count && index < transformList3.Count; ++index)
    {
      transformList5[index].position = transformList3[index].position;
      transformList5[index].rotation = transformList3[index].rotation;
    }
    GameObject gameObject2 = GameObject.Find("Balls");
    if (!(bool) (UnityEngine.Object) gameObject2)
      return false;
    Transform transform2 = gameObject2.transform;
    for (int index = transform2.childCount - 1; index >= 0; --index)
    {
      if (index < transformList1.Count)
      {
        transform2.GetChild(index).position = transformList1[index].position;
        Rigidbody component = transform2.GetChild(index).GetComponent<Rigidbody>();
        if ((bool) (UnityEngine.Object) component)
        {
          component.velocity = Vector3.zero;
          component.angularVelocity = Vector3.zero;
          component.Sleep();
        }
      }
    }
    GameObject gameObject3 = GameObject.Find("Spectator Cam");
    if (!(bool) (UnityEngine.Object) gameObject3)
      return true;
    Transform transform3 = gameObject3.transform;
    Vector3 localPosition1 = transform3.localPosition;
    Quaternion quaternion = transform3.localRotation;
    Vector3 eulerAngles = quaternion.eulerAngles;
    localPosition1.y = (float) ((double) this.floor_max_scale * 12.25 - 4.0);
    eulerAngles.x = (float) ((double) this.floor_max_scale * 3.72000002861023 + 64.4000015258789);
    transform3.localPosition = localPosition1;
    quaternion = new Quaternion();
    quaternion.eulerAngles = eulerAngles;
    transform3.localRotation = quaternion;
    return true;
  }

  public override void ScorerInit()
  {
    GameObject gameObject1 = GameObject.Find("GameSettings");
    if ((bool) (UnityEngine.Object) gameObject1)
      this.lms_settings = gameObject1.GetComponent<LastManStanding_Settings>();
    this.allBalls = UnityEngine.Object.FindObjectsOfType<LSM_BallCollision>();
    GameObject gameObject2 = GameObject.Find("3d field/floor");
    if ((bool) (UnityEngine.Object) gameObject2)
    {
      this.floor = gameObject2.transform;
      for (int index = 0; index < this.floor.childCount; ++index)
      {
        this.Flames.Add(this.floor.GetChild(index));
        if (QualitySettings.GetQualityLevel() == 0)
        {
          ParticleSystem component = this.floor.GetChild(index).GetComponent<ParticleSystem>();
          if ((bool) (UnityEngine.Object) component)
            component.Pause();
        }
      }
    }
    this.ScorerReset();
    if ((UnityEngine.Object) this.redtextobj == (UnityEngine.Object) null)
      this.redtextobj = GameObject.Find("REDSCORE");
    if (!((UnityEngine.Object) this.bluetextobj == (UnityEngine.Object) null))
      return;
    this.bluetextobj = GameObject.Find("BLUESCORE");
  }

  public override void OnTimerStart()
  {
    base.OnTimerStart();
    this.ScorerReset();
    this.start_players.Clear();
    this.dead_players.Clear();
    this.live_players.Clear();
    foreach (RobotID robotId in this.allRobotID.Values)
    {
      if (robotId.is_red)
        ++this.starting_players_red;
      else
        ++this.starting_players_blue;
      this.start_players.Add(robotId.id);
      this.live_players.Add(robotId.id);
    }
    this.SetRobotColors();
    if ((UnityEngine.Object) this.redtextobj == (UnityEngine.Object) null)
      this.redtextobj = GameObject.Find("REDSCORE");
    if (!((UnityEngine.Object) this.bluetextobj == (UnityEngine.Object) null))
      return;
    this.bluetextobj = GameObject.Find("BLUESCORE");
  }

  private void SetRobotColors()
  {
    foreach (RobotID robotId in this.allRobotID.Values)
    {
      RobotInterface3D robotInterface3D = this.allrobots_byid[robotId.id];
      if ((bool) (UnityEngine.Object) robotInterface3D)
      {
        if (this.lms_settings.FREE_FOR_ALL)
          robotInterface3D.SetRobotColor(3);
        else
          robotInterface3D.SetColorFromPosition(robotId.starting_pos);
      }
    }
  }

  public override void ScorerReset()
  {
    this.ConfigureField();
    base.ScorerReset();
    bool flag = false;
    foreach (RobotID robotId in this.allRobotID.Values)
    {
      if (!(bool) (UnityEngine.Object) robotId)
      {
        flag = true;
      }
      else
      {
        robotId.SetUserFloat("KILLS", 0.0f);
        robotId.SetUserInt("HITS", 0);
        robotId.SetUserBool("DEAD", false);
        if (GLOBALS.SERVER_MODE && (bool) (UnityEngine.Object) GLOBALS.topserver)
          GLOBALS.topserver.ClearFlag("DEAD", robotId.id);
        this.allrobots_byid[robotId.id].EnableTopObjects();
        this.allrobots_byid[robotId.id].SetKinematic(false);
      }
    }
    if (flag)
      this.DoFieldChanged();
    this.red_kills = 0;
    this.blue_kills = 0;
    this.starting_players_blue = 0;
    this.starting_players_red = 0;
    this.ResetFloor();
  }

  private void ResetFloor()
  {
    if (!(bool) (UnityEngine.Object) this.floor)
      return;
    this.floor.localScale = this.floor.localScale with
    {
      x = this.floor_max_scale,
      z = this.floor_max_scale
    };
    this.SetFlamesToFloor();
  }

  private void SetFlamesToFloor()
  {
    if (!(bool) (UnityEngine.Object) this.floor)
      return;
    foreach (Transform flame in this.Flames)
      flame.localScale = new Vector3(this.floor.localScale.x / 2f, 2f, 1f);
  }

  public override void Restart() => base.Restart();

  public override void ScorerUpdate()
  {
    if (GLOBALS.CLIENT_MODE && (bool) (UnityEngine.Object) GLOBALS.topclient && GLOBALS.topclient.connection_state == ClientLow.ConnectionStates.CONNECTED)
      this.textScores_panel.SetActive(true);
    else if (GLOBALS.SERVER_MODE && (bool) (UnityEngine.Object) GLOBALS.topserver && GLOBALS.topserver.serverReady)
      this.textScores_panel.SetActive(true);
    else
      this.textScores_panel.SetActive(false);
    if (this.gameoverpanel.activeSelf && Input.GetKey(KeyCode.Escape))
    {
      this.gameoverpanel.SetActive(false);
      this.gameoverpanel.GetComponent<Animator>().enabled = false;
      this.escape_was_pressed = true;
    }
    if (GLOBALS.SERVER_MODE)
    {
      if (this.timerstate == Scorekeeper.TimerState.FINISHED && this.timerstate_old == Scorekeeper.TimerState.RUNNING)
      {
        this.gameoverpanel.SetActive(true);
        this.gameoverpanel.GetComponent<Animator>().enabled = true;
        this.CreateGameOverText(this.gameovercode);
      }
      this.timerstate_old = this.timerstate;
      if (this.timerstate != Scorekeeper.TimerState.FINISHED && this.gameoverpanel.activeSelf)
      {
        this.gameoverpanel.GetComponent<Animator>().enabled = false;
        this.gameoverpanel.SetActive(false);
      }
    }
    if (this.timerstate != Scorekeeper.TimerState.RUNNING)
    {
      this.ResetFloor();
    }
    else
    {
      this.CheckPlayers();
      this.CalculateScores();
    }
  }

  public void CheckPlayers()
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    bool flag = false;
    foreach (RobotID robotId in this.allRobotID.Values)
    {
      if (!(bool) (UnityEngine.Object) robotId)
      {
        flag = true;
      }
      else
      {
        RobotInterface3D robotInterface3D = this.allrobots_byid[robotId.id];
        if (!(bool) (UnityEngine.Object) robotInterface3D)
          flag = true;
        else if (!robotId.GetUserBool("DEAD"))
        {
          if ((double) robotInterface3D.rb_body.transform.position.y < -2.0)
          {
            Robot_LastManStanding robotLastManStanding = robotInterface3D as Robot_LastManStanding;
            robotId.SetUserBool("DEAD");
            if (GLOBALS.SERVER_MODE && (bool) (UnityEngine.Object) GLOBALS.topserver)
              GLOBALS.topserver.AddFlag("DEAD", "1", robotId.id);
            this.dead_players.Add(robotId.id);
            this.live_players.Remove(robotId.id);
            if ((bool) (UnityEngine.Object) robotLastManStanding)
            {
              robotLastManStanding.PlayDeathAnimation();
            }
            else
            {
              robotInterface3D.SetKinematic();
              robotInterface3D.DisableTopObjects();
            }
          }
          else
          {
            if (this.lms_settings.HIT_COUNT <= 1)
              robotInterface3D.SetProgressBar(0.0f);
            else
              robotInterface3D.SetProgressBar((float) (this.lms_settings.HIT_COUNT - robotId.GetUserInt("HITS")) / (float) this.lms_settings.HIT_COUNT);
            if (robotId.GetUserInt("HITS") >= this.lms_settings.HIT_COUNT)
            {
              if (robotId.is_red)
                ++this.blue_kills;
              else
                ++this.red_kills;
              robotId.SetUserBool("DEAD");
              if (GLOBALS.SERVER_MODE && (bool) (UnityEngine.Object) GLOBALS.topserver)
                GLOBALS.topserver.AddFlag("DEAD", "1", robotId.id);
              this.dead_players.Add(robotId.id);
              this.live_players.Remove(robotId.id);
              int userInt = robotId.GetUserInt("LAST_HIT_ID");
              if (userInt > 0 && GLOBALS.SERVER_MODE && (bool) (UnityEngine.Object) GLOBALS.topserver)
              {
                int index = new System.Random().Next(0, GLOBALS.killingWords.Count - 1);
                GLOBALS.topserver.AddChat(GLOBALS.client_names[userInt] + " " + GLOBALS.killingWords[index] + " " + GLOBALS.client_names[robotId.id]);
              }
              Robot_LastManStanding robotLastManStanding = robotInterface3D as Robot_LastManStanding;
              if ((bool) (UnityEngine.Object) robotLastManStanding)
              {
                robotLastManStanding.PlayDeathAnimation();
              }
              else
              {
                robotInterface3D.SetKinematic();
                robotInterface3D.HoldRobot();
              }
            }
          }
        }
      }
    }
    if (flag)
      this.DoFieldChanged();
    this.curr_red_count = 0;
    this.curr_blue_count = 0;
    foreach (RobotID robotId in this.allRobotID.Values)
    {
      if (!robotId.GetUserBool("DEAD"))
      {
        if (robotId.is_red)
          ++this.curr_red_count;
        else
          ++this.curr_blue_count;
      }
    }
  }

  public override void GetScoreDetails(Dictionary<string, string> data)
  {
    base.GetScoreDetails(data);
    data["ScoreR"] = (this.score_red + this.score_redadj).ToString();
    data["ScoreB"] = (this.score_blue + this.score_blueadj).ToString();
  }

  private int SortByOPR(int p1, int p2)
  {
    if (!this.player_opr.ContainsKey(GLOBALS.client_names[p1]))
      this.player_opr[GLOBALS.client_names[p1]] = 0.0f;
    if (!this.player_opr.ContainsKey(GLOBALS.client_names[p2]))
      this.player_opr[GLOBALS.client_names[p2]] = 0.0f;
    return -1 * this.player_opr[GLOBALS.client_names[p1]].CompareTo(this.player_opr[GLOBALS.client_names[p2]]);
  }

  private void CalculateScores()
  {
    if (this.timerstate != Scorekeeper.TimerState.RUNNING || !(bool) (UnityEngine.Object) this.lms_settings || !(bool) (UnityEngine.Object) this.floor)
      return;
    Vector3 localScale = this.floor.localScale;
    if (this.time_total.TotalSeconds >= (double) (GLOBALS.TIMER_TOTAL - GLOBALS.TIMER_AUTO))
    {
      localScale.x = this.floor_max_scale;
      localScale.z = this.floor_max_scale;
    }
    else
    {
      float num = (float) this.time_total.TotalSeconds / (float) (GLOBALS.TIMER_TOTAL - GLOBALS.TIMER_AUTO) * this.floor_max_scale;
      localScale.x = num;
      localScale.z = num;
    }
    this.floor.localScale = localScale;
    this.SetFlamesToFloor();
    if (GLOBALS.CLIENT_MODE)
      return;
    int num1 = this.starting_players_blue - this.curr_blue_count;
    int num2 = this.starting_players_red - this.curr_red_count;
    int num3 = num1 + num2;
    foreach (RobotID robotId in this.allRobotID.Values)
    {
      if (!((UnityEngine.Object) robotId == (UnityEngine.Object) null) && !robotId.GetUserBool("DEAD") && GLOBALS.client_names.ContainsKey(robotId.id))
      {
        if (this.lms_settings.FREE_FOR_ALL)
          this.player_opr[GLOBALS.client_names[robotId.id]] = (float) num3 + robotId.GetUserFloat("KILLS");
        else if (robotId.is_red)
          this.player_opr[GLOBALS.client_names[robotId.id]] = (float) num1 + robotId.GetUserFloat("KILLS");
        else
          this.player_opr[GLOBALS.client_names[robotId.id]] = (float) num2 + robotId.GetUserFloat("KILLS");
      }
    }
    this.start_players.Sort(new Comparison<int>(this.SortByOPR));
    this.textScores.text = "<b>Rank:</b>";
    for (int index = 0; index < this.start_players.Count; ++index)
    {
      int startPlayer = this.start_players[index];
      if (this.allRobotID.ContainsKey(startPlayer) && GLOBALS.client_names.ContainsKey(startPlayer))
      {
        string str = "<color=white>";
        if (!this.lms_settings.FREE_FOR_ALL)
          str = !this.allRobotID[this.start_players[index]].is_red ? (!this.allRobotID[startPlayer].GetUserBool("DEAD") ? "<color=blue>" : "<color=#505090>") : (!this.allRobotID[startPlayer].GetUserBool("DEAD") ? "<color=red>" : "<color=#905050>");
        TextMeshProUGUI textScores = this.textScores;
        textScores.text = textScores.text + "\n<b>" + (object) (index + 1) + "</b>) " + str + GLOBALS.client_names[startPlayer] + "</color>=" + (object) this.player_opr[GLOBALS.client_names[startPlayer]];
      }
    }
    this.gameovercode = this.GetGameOverCode();
    if (this.lms_settings.FREE_FOR_ALL)
    {
      if ((bool) (UnityEngine.Object) this.redtextobj)
        this.redtextobj.SetActive(false);
      if ((bool) (UnityEngine.Object) this.bluetextobj)
        this.bluetextobj.SetActive(false);
      this.score_red = 0;
      this.score_blue = 0;
      if (this.curr_red_count + this.curr_blue_count > 1 || GLOBALS.SINGLEPLAYER_MODE)
        return;
      this.force_game_end = true;
    }
    else
    {
      if ((bool) (UnityEngine.Object) this.redtextobj)
        this.redtextobj.SetActive(true);
      if ((bool) (UnityEngine.Object) this.bluetextobj)
        this.bluetextobj.SetActive(true);
      if (this.curr_red_count > 0 && this.curr_blue_count > 0 || GLOBALS.SINGLEPLAYER_MODE)
        return;
      this.force_game_end = true;
    }
  }

  public override void FieldChangedTrigger()
  {
    this.SetRobotColors();
    for (int index = this.live_players.Count - 1; index >= 0; --index)
    {
      int livePlayer = this.live_players[index];
      if (!this.allRobotID.ContainsKey(livePlayer))
      {
        this.dead_players.Add(livePlayer);
        this.live_players.RemoveAt(index);
      }
    }
  }

  public override int GetRedScore()
  {
    if (this.timerstate == Scorekeeper.TimerState.FINISHED)
      return this.score_redfinal;
    this.score_red = this.red_kills + (this.starting_players_blue - this.curr_blue_count);
    return this.score_red + this.score_redadj;
  }

  public override int GetBlueScore()
  {
    if (this.timerstate == Scorekeeper.TimerState.FINISHED)
      return this.score_bluefinal;
    this.score_blue = this.blue_kills + (this.starting_players_red - this.curr_red_count);
    return this.score_blue + this.score_blueadj;
  }

  public override void SendServerData(Dictionary<string, string> serverFlags)
  {
    base.SendServerData(serverFlags);
    serverFlags["SCOREText"] = this.textScores.text;
    string str = (!(bool) (UnityEngine.Object) this.redtextobj || !this.redtextobj.activeSelf ? "0" : "1") + ":" + (!(bool) (UnityEngine.Object) this.bluetextobj || !this.bluetextobj.activeSelf ? "0" : "1") + ":" + (!(bool) (UnityEngine.Object) this.gameoverpanel || !this.gameoverpanel.activeSelf ? "0:" : "1:" + this.gameovercode);
    serverFlags["LMSState"] = str;
  }

  public override void ReceiveServerData(Dictionary<string, string> serverFlags)
  {
    base.ReceiveServerData(serverFlags);
    if (serverFlags.ContainsKey("SCOREText"))
      this.textScores.text = serverFlags["SCOREText"];
    if (serverFlags.ContainsKey("LMSState"))
    {
      string[] strArray = serverFlags["LMSState"].Split(':');
      if (strArray.Length < 4)
        return;
      if ((bool) (UnityEngine.Object) this.redtextobj)
        this.redtextobj.SetActive(strArray[0][0] == '1');
      if ((bool) (UnityEngine.Object) this.bluetextobj)
        this.bluetextobj.SetActive(strArray[1][0] == '1');
      bool flag = strArray[2][0] == '1';
      if (!flag)
        this.escape_was_pressed = false;
      if (this.gameoverpanel.activeSelf && !flag)
      {
        this.gameoverpanel.GetComponent<Animator>().enabled = false;
        this.gameoverpanel.SetActive(false);
      }
      if (((this.escape_was_pressed ? 0 : (!this.gameoverpanel.activeSelf ? 1 : 0)) & (flag ? 1 : 0)) != 0)
      {
        this.gameoverpanel.SetActive(true);
        this.gameoverpanel.GetComponent<Animator>().enabled = true;
        this.gameoverpanel.GetComponent<Animator>().Play("GameOver");
        this.CreateGameOverText(strArray[3]);
      }
    }
    if (!(GLOBALS.CLIENT_MODE & (bool) (UnityEngine.Object) GLOBALS.topclient))
      return;
    this.mystate = GLOBALS.topclient.serverFlags.ContainsKey("DEAD") ? int.Parse(GLOBALS.topclient.serverFlags["DEAD"]) : 0;
    if (this.mystate == this.mystate_old)
      return;
    this.mystate_old = this.mystate;
    this.OnCameraViewChanged();
    GLOBALS.topclient.DoCameraViewChanged();
  }

  public override string CorrectRobotChoice(string requested_robot) => "Bot Royale";

  public override Transform CorrectRobotPosition(
    string requested_position,
    List<string> used_positions)
  {
    if ((UnityEngine.Object) this.lms_settings == (UnityEngine.Object) null)
      this.Start();
    if (this.lms_settings.FREE_FOR_ALL && used_positions.Count >= this.lms_settings.PLAYER_COUNT)
      return (Transform) null;
    if (!this.lms_settings.FREE_FOR_ALL)
    {
      int num1 = this.lms_settings.PLAYER_COUNT / 2;
      string str = requested_position.Contains("Red") ? "Red" : "Blue";
      int num2 = 0;
      foreach (string usedPosition in used_positions)
      {
        if (usedPosition.Contains(str))
          ++num2;
      }
      if (num2 >= num1)
        return (Transform) null;
    }
    if (this.lms_settings.FREE_FOR_ALL)
      requested_position = "ALL";
    return base.CorrectRobotPosition(requested_position, used_positions);
  }

  public override void AddPlayer(RobotInterface3D currbot, bool robot_is_new = true)
  {
    base.AddPlayer(currbot, robot_is_new);
    if (!robot_is_new || this.timerstate != Scorekeeper.TimerState.RUNNING)
      return;
    currbot.SetKinematic();
    currbot.DisableTopObjects();
    currbot.myRobotID.SetUserBool("DEAD");
  }

  public string GetGameOverCode() => "" + (this.lms_settings.FREE_FOR_ALL ? "1" : "0") + "." + (this.live_players.Count == 1 ? GLOBALS.client_names[this.live_players[0]] : "") + "." + (this.dead_players.Count > 0 ? GLOBALS.client_names[this.dead_players[this.dead_players.Count - 1]] : "") + "." + (this.dead_players.Count > 1 ? GLOBALS.client_names[this.dead_players[this.dead_players.Count - 2]] : "") + "." + (this.start_players.Count > 0 ? GLOBALS.client_names[this.start_players[0]] : "") + "." + (this.start_players.Count > 1 ? GLOBALS.client_names[this.start_players[1]] : "") + "." + (this.start_players.Count > 2 ? GLOBALS.client_names[this.start_players[2]] : "");

  public void CreateGameOverText(string gameovercode)
  {
    this.gameover_text.text = "";
    string[] strArray = gameovercode.Split('.');
    this.gameover_text.text = "";
    if (strArray.Length < 7)
      return;
    if (strArray[0][0] == '1')
    {
      this.gameover_text.text = "<b><size=+10>Last Robot Standing</size></b>\n";
      TextMeshProUGUI gameoverText1 = this.gameover_text;
      gameoverText1.text = gameoverText1.text + "<size=+10> 1st Place: " + (strArray[1].Length > 0 ? strArray[1] : "No Survivors?!") + "</size>\n";
      if (strArray[2].Length > 0)
      {
        TextMeshProUGUI gameoverText2 = this.gameover_text;
        gameoverText2.text = gameoverText2.text + "  2nd Place: " + strArray[2] + "\n";
      }
      if (strArray[3].Length > 0)
      {
        TextMeshProUGUI gameoverText3 = this.gameover_text;
        gameoverText3.text = gameoverText3.text + "  3rd Place: " + strArray[3] + "\n";
      }
      this.gameover_text.text += "\n\n<b><size=+10>Overall Score</size></b>\n";
      TextMeshProUGUI gameoverText4 = this.gameover_text;
      gameoverText4.text = gameoverText4.text + "<size=+10> 1st Place: " + (strArray[4].Length > 0 ? strArray[4] : "No Survivors?!") + "</size>\n";
      if (strArray[5].Length > 0)
      {
        TextMeshProUGUI gameoverText5 = this.gameover_text;
        gameoverText5.text = gameoverText5.text + "  2nd Place: " + strArray[5] + "\n";
      }
      if (strArray[6].Length <= 0)
        return;
      TextMeshProUGUI gameoverText6 = this.gameover_text;
      gameoverText6.text = gameoverText6.text + "  3rd Place: " + strArray[6] + "\n";
    }
    else
    {
      this.gameover_text.text = "<b><size=+10>Top Individual Scores</size></b>\n";
      TextMeshProUGUI gameoverText7 = this.gameover_text;
      gameoverText7.text = gameoverText7.text + "<size=+10> 1st Place: " + (strArray[4].Length > 0 ? strArray[4] : "No Survivors?!") + "</size>\n";
      if (strArray[5].Length > 0)
      {
        TextMeshProUGUI gameoverText8 = this.gameover_text;
        gameoverText8.text = gameoverText8.text + "  2nd Place: " + strArray[5] + "\n";
      }
      if (strArray[6].Length <= 0)
        return;
      TextMeshProUGUI gameoverText9 = this.gameover_text;
      gameoverText9.text = gameoverText9.text + "  3rd Place: " + strArray[6] + "\n";
    }
  }

  public override bool CorrectFieldElement(GameObject currobj)
  {
    if (!base.CorrectFieldElement(currobj))
      return false;
    LSM_BallCollision component = currobj.GetComponent<LSM_BallCollision>();
    if (!(bool) (UnityEngine.Object) component)
      return true;
    component.DeactiveBall();
    return true;
  }

  public override void OnCameraViewChanged()
  {
    if (GLOBALS.SINGLEPLAYER_MODE)
      return;
    GLOBALS.camera_follows = this.mystate == 0;
  }

  public override void OnScorerInterrupt(string msg)
  {
    if (!(msg == "CF"))
      return;
    this.ConfigureField();
  }

  public override bool UseGameStartOverlay() => false;
}
