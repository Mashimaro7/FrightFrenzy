// Decompiled with JetBrains decompiler
// Type: ApplicationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using SimpleFileBrowser;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using Windows;

public class ApplicationManager : MonoBehaviour
{
  private GameObject gui_panel;
  private Animator gui_animator;
  public GameObject MainCamera;
  public GameObject VRCamera;
  public Camera VRtracking;
  public GameObject EventSystem;
  public GameObject ServerOptions;
  public GameObject chat_text;
  public Toggle toggle_tmode;
  public Toggle toggle_startwhenready;
  public Toggle toggle_holding;
  public Dropdown dropdown_gameoption;
  public InputField inputfield_password;
  public InputField inputfield_updaterate;
  public InputField inputfield_maxdata;
  public InputField inputfield_admin;
  public TextMeshProUGUI scoreadj_red_placeholder;
  public TextMeshProUGUI scoreadj_blue_placeholder;
  public GameObject admin_menu;
  public GameObject admin_popup;
  public GameObject admin_button;
  public GameObject options_button;
  public Dropdown admin_kickplayer;
  public Toggle playback_rec;
  public Toggle playback_pause;
  public Toggle playback_play;
  public Toggle playback_save;
  public Toggle playback_stop;
  public Slider playback_slider;
  public TMP_Dropdown playback_speed;
  public Text playback_autosave;
  private bool close_client_menu;
  private bool initialized;
  public int pointer_over_active;
  private bool old_admin_enable;
  public GameObject settings_panel;
  private bool filebrowser_initialized;
  public GameObject RequestsDropdown;
  public GameObject PositionDropdown;
  private SinglePlayer singleplayer;
  private bool slider_changed_by_script;

  private void Awake()
  {
    if (!GLOBALS.HEADLESS_MODE || GLOBALS.win_console != null)
      return;
    Console.WriteLine("**** STARTING CONSOLE FOR WINDOWS *****");
    GLOBALS.win_console = new ConsoleWindow();
    GLOBALS.win_console.Initialize();
    Console.WriteLine("**** CONSOLE FOR WINDOWS STARTED *****");
  }

  private void Start()
  {
    if ((bool) (UnityEngine.Object) this.settings_panel)
    {
      Transform transform = this.settings_panel.transform.Find("NameInput");
      if ((bool) (UnityEngine.Object) transform)
        transform.GetComponent<InputField>().text = GLOBALS.default_player_name;
    }
    this.PB_StopPressed(true);
    GLOBALS.now_playing = false;
    GLOBALS.now_paused = false;
    GLOBALS.now_recording = false;
    MyUtils.PB_ChangeSpeed(1f);
  }

  public void Update()
  {
    if (this.close_client_menu)
    {
      if ((UnityEngine.Object) GLOBALS.topclient == (UnityEngine.Object) null)
        return;
      if (!GLOBALS.topclient.playbackmode && GLOBALS.topclient.GetConnectionStatus() == -1)
      {
        this.close_client_menu = false;
        PanelManager component = GameObject.Find("MenuManager").GetComponent<PanelManager>();
        if ((UnityEngine.Object) component == (UnityEngine.Object) null)
          return;
        component.OnEnable();
        return;
      }
      if (GLOBALS.topclient.IsAdmin)
      {
        this.admin_button.SetActive(true);
        this.options_button.SetActive(true);
      }
      else
      {
        this.admin_button.SetActive(false);
        this.admin_menu.SetActive(false);
        this.options_button.SetActive(false);
      }
      this.scoreadj_red_placeholder.text = MyUtils.GetRedAdj();
      this.scoreadj_blue_placeholder.text = MyUtils.GetBlueAdj();
    }
    if ((bool) (UnityEngine.Object) this.chat_text)
      this.UpdateChat();
    if ((bool) (UnityEngine.Object) this.admin_menu)
    {
      if (this.admin_menu.activeSelf && (GLOBALS.topclient.players_changed || !this.old_admin_enable))
      {
        this.admin_kickplayer.ClearOptions();
        List<string> options = new List<string>();
        options.Add("Kick Player");
        this.admin_kickplayer.AddOptions(options);
        options.Clear();
        foreach (int key in GLOBALS.topclient.players.Keys)
          options.Add(key.ToString() + ":" + GLOBALS.topclient.players[key].playerName);
        this.admin_kickplayer.AddOptions(options);
      }
      this.old_admin_enable = this.admin_menu.activeSelf;
    }
    if (GLOBALS.now_recording)
    {
      this.playback_slider.minValue = (float) MyUtils.PB_GetStartTime();
      this.playback_slider.maxValue = (float) MyUtils.PB_GetEndTime();
      this.playback_slider.value = this.playback_slider.minValue;
      if (GLOBALS.autosave_recordings)
      {
        if (!this.playback_autosave.gameObject.activeSelf)
          this.playback_autosave.gameObject.SetActive(true);
        this.playback_autosave.color = this.playback_autosave.color with
        {
          a = MyUtils.PB_AutoSavePercentage()
        };
      }
      else if (this.playback_autosave.gameObject.activeSelf)
        this.playback_autosave.gameObject.SetActive(false);
    }
    if (GLOBALS.now_playing && !GLOBALS.now_paused)
    {
      this.PB_ChangeSlider((float) MyUtils.PB_GetCurrentTime());
      if (MyUtils.PB_ReachedEnd())
        this.PB_StopPressed(true);
    }
    if (!(bool) (UnityEngine.Object) this.playback_save)
      return;
    if (MyUtils.recorded_data.Count > 0)
    {
      if (this.playback_save.gameObject.activeSelf)
        return;
      this.playback_save.gameObject.SetActive(true);
      this.playback_save.interactable = true;
    }
    else
    {
      if (!this.playback_save.gameObject.activeSelf)
        return;
      this.playback_save.gameObject.SetActive(false);
      this.playback_save.interactable = false;
    }
  }

  public void LateUpdate()
  {
    if (this.initialized)
      return;
    if ((bool) (UnityEngine.Object) this.VRCamera && !XRDevice.isPresent)
      this.VRCamera.SetActive(false);
    this.initialized = true;
    if (!GLOBALS.HEADLESS_MODE || (bool) (UnityEngine.Object) GLOBALS.topserver)
      return;
    Console.Out.WriteLine("Loading Server....");
    this.LoadSceneDelayed("Scenes/server");
  }

  public void OnEnable() => UnityEngine.Resources.UnloadUnusedAssets();

  public void Quit() => Application.Quit();

  public void ShowGameOptions()
  {
    GameMenu[] objectsOfTypeAll = UnityEngine.Resources.FindObjectsOfTypeAll<GameMenu>();
    if (objectsOfTypeAll.Length == 0)
      return;
    foreach (Component component in objectsOfTypeAll)
      component.gameObject.SetActive(true);
  }

  public void LoadScene(string name) => SceneManager.LoadScene(name);

  public IEnumerator DelaySeconds(float seconds, string name)
  {
    yield return (object) new WaitForSecondsRealtime(seconds);
    this.LoadScene(name);
  }

  public void LoadSceneDelayed(string name) => this.StartCoroutine(this.DelaySeconds(0.65f, name));

  public void EnableWindow(string name)
  {
    this.gui_panel = GameObject.Find(name);
    if ((UnityEngine.Object) this.gui_panel == (UnityEngine.Object) null)
      return;
    this.gui_animator = this.gui_panel.GetComponent<Animator>();
    this.gui_animator.SetBool("Open", true);
  }

  public void ConnectButtonPressed()
  {
    if ((UnityEngine.Object) GLOBALS.topclient == (UnityEngine.Object) null)
      return;
    InputField component1 = this.settings_panel.transform.Find("IPInput").GetComponent<InputField>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
      return;
    InputField component2 = this.settings_panel.transform.Find("PORTInput").GetComponent<InputField>();
    if ((UnityEngine.Object) component2 == (UnityEngine.Object) null)
      return;
    InputField component3 = this.settings_panel.transform.Find("NameInput").GetComponent<InputField>();
    if ((UnityEngine.Object) component3 == (UnityEngine.Object) null)
      return;
    GLOBALS.default_player_name = component3.text;
    Settings.SavePrefs();
    InputField component4 = this.settings_panel.transform.Find("PasswordInput").GetComponent<InputField>();
    if ((UnityEngine.Object) component4 == (UnityEngine.Object) null)
      return;
    Dropdown component5 = this.settings_panel.transform.Find("StartPosInput").GetComponent<Dropdown>();
    if ((UnityEngine.Object) component5 == (UnityEngine.Object) null || !GLOBALS.topclient.Connect(component1.text, component2.text, component3.text, component4.text, component5.options[component5.value].text))
      return;
    PanelManager component6 = GameObject.Find("MenuManager").GetComponent<PanelManager>();
    if ((UnityEngine.Object) component6 == (UnityEngine.Object) null)
      return;
    component6.CloseCurrent();
    this.close_client_menu = true;
    this.chat_text.GetComponent<InputField>().readOnly = false;
    this.chat_text.GetComponent<InputField>().text = "";
    this.chat_text.SetActive(false);
  }

  public void LoadButtonPressed()
  {
    if (FileBrowser.IsOpen)
      return;
    this.StartCoroutine(this.ShowLoadDialogCoroutine());
  }

  private IEnumerator ShowLoadDialogCoroutine()
  {
    FileBrowser.SetFilters(true, new FileBrowser.Filter("xRC Data-Set", ".xrc"));
    FileBrowser.SetDefaultFilter(".xrc");
    yield return (object) FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, title: "Load Files and Folders", loadButtonText: "Load xRC Data");
    Debug.Log((object) FileBrowser.Success);
    if (FileBrowser.Success && !((UnityEngine.Object) GLOBALS.topclient == (UnityEngine.Object) null))
    {
      if (FileBrowser.Result.Length != 1)
        GLOBALS.topclient.ShowMessage("Did not chose 1 file...");
      else if (!MyUtils.PB_LoadFromFile(FileBrowser.Result[0]))
      {
        GLOBALS.topclient.ShowMessage("Failed to load " + FileBrowser.Result[0]);
      }
      else
      {
        PanelManager component = GameObject.Find("MenuManager").GetComponent<PanelManager>();
        if (!((UnityEngine.Object) component == (UnityEngine.Object) null))
        {
          component.CloseCurrent();
          this.close_client_menu = true;
          this.chat_text.GetComponent<InputField>().readOnly = false;
          this.chat_text.GetComponent<InputField>().text = "";
          this.chat_text.SetActive(false);
          GLOBALS.topclient.InitPlaybackMode();
          GLOBALS.now_playing = false;
          GLOBALS.now_paused = false;
          GLOBALS.now_recording = false;
          if ((bool) (UnityEngine.Object) this.playback_stop)
          {
            this.playback_stop.gameObject.SetActive(true);
            this.playback_stop.interactable = true;
            this.playback_stop.isOn = true;
            this.PB_StopPressed(true);
          }
        }
      }
    }
  }

  private IEnumerator ShowSaveDialogCoroutine()
  {
    FileBrowser.SetFilters(true, new FileBrowser.Filter("xRC Data-Set", ".xrc"));
    FileBrowser.SetDefaultFilter(".xrc");
    yield return (object) FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Files, title: "Save xRC Data");
    Debug.Log((object) FileBrowser.Success);
    if (FileBrowser.Success && !((UnityEngine.Object) GLOBALS.topclient == (UnityEngine.Object) null))
    {
      if (FileBrowser.Result.Length != 1)
      {
        GLOBALS.topclient.ShowMessage("Did not chose 1 file...");
      }
      else
      {
        GLOBALS.autosave_recordings = FileBrowser.AutoSave;
        if (MyUtils.PB_SaveToFile(FileBrowser.Result[0]))
        {
          string message = "File save =  " + FileBrowser.Result[0];
          if (GLOBALS.autosave_recordings)
          {
            GLOBALS.autosave_filename = FileBrowser.Result[0];
            message += ", Auto-Save started.";
          }
          GLOBALS.topclient.ShowMessage(message);
        }
        else
        {
          string message = "Failed to save to file " + FileBrowser.Result[0];
          if (GLOBALS.autosave_recordings)
          {
            message += ", Auto-Save disabled.";
            GLOBALS.autosave_recordings = false;
          }
          GLOBALS.topclient.ShowMessage(message);
        }
      }
    }
  }

  public void StartButtonPressed()
  {
    if ((UnityEngine.Object) GLOBALS.topserver == (UnityEngine.Object) null)
      return;
    GLOBALS.topserver.top_application_manager = this;
    InputField component1 = this.settings_panel.transform.Find("PORTInput").GetComponent<InputField>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
      return;
    InputField component2 = this.settings_panel.transform.Find("PasswordInput").GetComponent<InputField>();
    if ((UnityEngine.Object) component2 == (UnityEngine.Object) null)
      return;
    InputField component3 = this.settings_panel.transform.Find("SPECTATORSInput").GetComponent<InputField>();
    if ((UnityEngine.Object) component3 == (UnityEngine.Object) null)
      return;
    InputField component4 = this.settings_panel.transform.Find("CommentInput").GetComponent<InputField>();
    if ((UnityEngine.Object) component4 == (UnityEngine.Object) null)
      return;
    Toggle component5 = this.settings_panel.transform.Find("RegisterToggle").GetComponent<Toggle>();
    if ((UnityEngine.Object) component5 == (UnityEngine.Object) null)
      return;
    InputField component6 = this.settings_panel.transform.Find("REGPORTInput").GetComponent<InputField>();
    if ((UnityEngine.Object) component6 == (UnityEngine.Object) null)
      return;
    if (component6.text.Length < 1)
      component6.text = GLOBALS.UDP_PORT.ToString();
    if (component1.text.Length < 1)
      component1.text = GLOBALS.UDP_PORT.ToString();
    if (component3.text.Length < 1)
      component3.text = "4";
    GLOBALS.topserver.ServerStart(int.Parse(component1.text), component2.text, int.Parse(component3.text), component4.text, int.Parse(component6.text), component5.isOn);
    this.chat_text.GetComponent<InputField>().readOnly = false;
    this.chat_text.GetComponent<InputField>().text = "";
    this.chat_text.SetActive(false);
    if ((UnityEngine.Object) this.ServerOptions == (UnityEngine.Object) null)
      return;
    this.toggle_tmode.isOn = false;
    this.toggle_startwhenready.isOn = true;
    this.toggle_holding.isOn = false;
    this.inputfield_password.text = component2.text;
    this.inputfield_updaterate.text = GLOBALS.SERVER_SEND_UPDATE_DELAY.ToString();
    this.inputfield_maxdata.text = GLOBALS.UDP_MAX_BYTES_IN_MS.ToString();
    PanelManager component7 = GameObject.Find("MenuManager").GetComponent<PanelManager>();
    if ((UnityEngine.Object) component7 == (UnityEngine.Object) null)
      return;
    component7.CloseCurrent();
  }

  public void MultiPlayerRequestCB()
  {
    if ((UnityEngine.Object) GLOBALS.topclient == (UnityEngine.Object) null)
      return;
    Dropdown component = this.RequestsDropdown.GetComponent<Dropdown>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    GLOBALS.topclient.FlagRequest(component);
  }

  public void SinglePlayerRequestCB()
  {
    if ((UnityEngine.Object) this.singleplayer == (UnityEngine.Object) null)
      this.singleplayer = GameObject.Find("SinglePlayerScript").GetComponent<SinglePlayer>();
    if ((UnityEngine.Object) this.singleplayer == (UnityEngine.Object) null)
      return;
    Dropdown component = this.RequestsDropdown.GetComponent<Dropdown>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    this.singleplayer.FlagRequest(component);
  }

  public void SinglePlayerSetPos()
  {
    if ((UnityEngine.Object) this.singleplayer == (UnityEngine.Object) null)
      this.singleplayer = GameObject.Find("SinglePlayerScript").GetComponent<SinglePlayer>();
    if ((UnityEngine.Object) this.singleplayer == (UnityEngine.Object) null)
      return;
    Dropdown component = this.PositionDropdown.GetComponent<Dropdown>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    this.singleplayer.SetPosition(component.options[component.value].text);
  }

  public void TimerButtonPressed() => GameObject.Find("Scorekeeper").GetComponent<Scorekeeper>().OnTimerClick();

  public void TimerReset() => GameObject.Find("Scorekeeper").GetComponent<Scorekeeper>().OnTimerReset();

  public void SendChat()
  {
    GLOBALS.keyboard_inuse = false;
    if (Input.GetKeyDown(KeyCode.Escape))
      return;
    if ((bool) (UnityEngine.Object) GLOBALS.topclient)
      GLOBALS.topclient.SendChat(this.chat_text.GetComponent<InputField>().text);
    else if ((bool) (UnityEngine.Object) GLOBALS.topserver)
      GLOBALS.topserver.SendChat(this.chat_text.GetComponent<InputField>().text);
    this.chat_text.GetComponent<InputField>().text = "";
    this.chat_text.SetActive(false);
  }

  private void UpdateChat()
  {
    if (Input.GetKeyDown(KeyCode.BackQuote))
    {
      if (!this.chat_text.activeSelf)
      {
        this.chat_text.SetActive(true);
        if ((bool) (UnityEngine.Object) this.chat_text.transform.parent)
        {
          this.chat_text.transform.parent.gameObject.SetActive(true);
          GameObject gameObject = GameObject.Find("ServerInfoWindow");
          if ((bool) (UnityEngine.Object) gameObject)
            gameObject.SetActive(false);
        }
        this.chat_text.GetComponent<InputField>().ActivateInputField();
        this.chat_text.GetComponent<InputField>().Select();
        GLOBALS.keyboard_inuse = true;
      }
      else
        GLOBALS.keyboard_inuse = false;
    }
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      this.chat_text.GetComponent<InputField>().text = "";
      this.chat_text.GetComponent<InputField>().DeactivateInputField();
      this.chat_text.SetActive(false);
      GLOBALS.keyboard_inuse = false;
    }
    if (!this.chat_text.activeSelf)
      return;
    if ((bool) (UnityEngine.Object) GLOBALS.topclient)
    {
      GLOBALS.topclient.ResetChatCounter();
    }
    else
    {
      if (!(bool) (UnityEngine.Object) GLOBALS.topserver)
        return;
      GLOBALS.topserver.ResetChatCounter();
    }
  }

  public void ServerOptionsChange()
  {
    if ((UnityEngine.Object) GLOBALS.topserver == (UnityEngine.Object) null)
      return;
    GLOBALS.topserver.top_application_manager = this;
    GLOBALS.topserver.tournament_mode = this.toggle_tmode.isOn;
    GLOBALS.topserver.start_when_ready = this.toggle_startwhenready.isOn;
    GLOBALS.topserver.holding_mode = this.toggle_holding.isOn;
    GLOBALS.game_option = this.dropdown_gameoption.value + 1;
    GLOBALS.topserver.PASSWORD = this.inputfield_password.text;
    GLOBALS.topserver.ADMIN = this.inputfield_admin.text;
    long result;
    if (long.TryParse(this.inputfield_updaterate.text, out result))
      GLOBALS.topserver.UPDATE_DELAY = result;
    if (!long.TryParse(this.inputfield_maxdata.text, out result))
      return;
    GLOBALS.topserver.MAX_BYTES = result;
  }

  public void UpdateMenuesToServerSettings()
  {
    this.toggle_tmode.isOn = GLOBALS.topserver.tournament_mode;
    this.toggle_startwhenready.isOn = GLOBALS.topserver.start_when_ready;
    this.toggle_holding.isOn = GLOBALS.topserver.holding_mode;
    this.dropdown_gameoption.value = GLOBALS.game_option - 1;
    this.inputfield_password.text = GLOBALS.topserver.PASSWORD;
    this.inputfield_admin.text = GLOBALS.topserver.ADMIN;
  }

  public void RestartGame() => GLOBALS.topserver.ServerMenu_RestartLevel();

  public void DebugChange(string number_in)
  {
  }

  public void ADMIN_KickAll()
  {
    if (!(bool) (UnityEngine.Object) GLOBALS.topclient)
      return;
    GLOBALS.topclient.SendChat("/SERVER KICKALL");
  }

  public void ADMIN_KickPlayer(int index)
  {
    if (!(bool) (UnityEngine.Object) GLOBALS.topclient || index == 0)
      return;
    string[] strArray = this.admin_kickplayer.options[index].text.Split(':');
    if (strArray.Length < 2)
      return;
    GLOBALS.topclient.SendChat("/SERVER KICKID=" + strArray[0]);
  }

  public void ADMIN_StartGame()
  {
    if (!(bool) (UnityEngine.Object) GLOBALS.topclient)
      return;
    GLOBALS.topclient.SendChat("/SERVER RESTART");
  }

  public void ADMIN_StopGame()
  {
    if (!(bool) (UnityEngine.Object) GLOBALS.topclient)
      return;
    GLOBALS.topclient.SendChat("/SERVER STOP");
  }

  public void ADMIN_SetPassword(Text newpassword)
  {
    if (!(bool) (UnityEngine.Object) GLOBALS.topclient)
      return;
    GLOBALS.topclient.SendChat("/SERVER PASSWORD=" + newpassword.text);
  }

  public void ADMIN_ExitAdmin()
  {
    if (!(bool) (UnityEngine.Object) GLOBALS.topclient || !(bool) (UnityEngine.Object) this.admin_menu)
      return;
    this.admin_menu.SetActive(false);
  }

  public void ADMIN_ToggleHUD()
  {
    if (!(bool) (UnityEngine.Object) GLOBALS.topclient)
      return;
    GLOBALS.topclient.Toggle_HUD();
  }

  public void ADMIN_EnterAdmin()
  {
    if (!(bool) (UnityEngine.Object) GLOBALS.topclient || !(bool) (UnityEngine.Object) this.admin_menu)
      return;
    this.admin_menu.SetActive(true);
  }

  public void ADMIN_AdjRED(TMP_InputField newvalue)
  {
    if (!(bool) (UnityEngine.Object) GLOBALS.topclient)
      return;
    GLOBALS.topclient.SendChat("/SERVER REDADJ=" + newvalue.text);
    newvalue.text = "";
  }

  public void ADMIN_AdjBLUE(TMP_InputField newvalue)
  {
    if (!(bool) (UnityEngine.Object) GLOBALS.topclient)
      return;
    GLOBALS.topclient.SendChat("/SERVER BLUEADJ=" + newvalue.text);
    newvalue.text = "";
  }

  public void ADMIN_LockKeyboard(bool lockstate) => GLOBALS.keyboard_inuse = lockstate;

  public void ADMIN_SetOutputScoreFiles(TMP_InputField location)
  {
    GLOBALS.OUTPUT_SCORING_FILES = true;
    MyUtils.status_file_dir = location.text;
  }

  public void PB_SavePressed(bool state)
  {
    if (state)
      this.playback_save.isOn = false;
    if (!state || FileBrowser.IsOpen)
      return;
    this.StartCoroutine(this.ShowSaveDialogCoroutine());
  }

  public void PB_RecordPressed(bool state)
  {
    if (GLOBALS.now_recording || !state)
      return;
    MyUtils.PB_ClearRecording();
    this.playback_rec.interactable = false;
    this.playback_rec.isOn = true;
    this.playback_stop.gameObject.SetActive(true);
    this.playback_stop.interactable = true;
    this.playback_stop.isOn = false;
    this.playback_play.gameObject.SetActive(false);
    this.playback_play.interactable = false;
    this.playback_play.isOn = false;
    this.playback_pause.gameObject.SetActive(false);
    this.playback_pause.interactable = false;
    this.playback_pause.isOn = false;
    this.playback_slider.gameObject.SetActive(false);
    this.playback_speed.gameObject.SetActive(false);
    this.playback_speed.interactable = false;
    GLOBALS.now_recording = true;
  }

  public void PB_StopPressed(bool state)
  {
    if (!(bool) (UnityEngine.Object) this.playback_stop || !state)
      return;
    this.playback_stop.gameObject.SetActive(true);
    this.playback_stop.interactable = false;
    this.playback_stop.isOn = false;
    this.playback_rec.gameObject.SetActive(true);
    this.playback_rec.interactable = true;
    this.playback_rec.isOn = false;
    this.playback_pause.gameObject.SetActive(false);
    this.playback_pause.interactable = false;
    this.playback_pause.isOn = false;
    this.playback_speed.gameObject.SetActive(false);
    this.playback_speed.interactable = false;
    if (MyUtils.PB_GetStartTime() > 0L)
    {
      this.playback_play.gameObject.SetActive(true);
      this.playback_play.interactable = true;
      this.playback_play.isOn = false;
      this.playback_slider.gameObject.SetActive(true);
      this.playback_slider.interactable = true;
      this.playback_slider.minValue = (float) MyUtils.PB_GetStartTime();
      this.playback_slider.maxValue = (float) MyUtils.PB_GetEndTime();
      if ((double) this.playback_slider.value < (double) this.playback_slider.minValue || (double) this.playback_slider.value > (double) this.playback_slider.maxValue)
        this.PB_ChangeSlider(this.playback_slider.minValue);
    }
    else
    {
      this.playback_play.gameObject.SetActive(false);
      this.playback_play.interactable = false;
      this.playback_play.isOn = false;
      this.playback_slider.gameObject.SetActive(false);
    }
    GLOBALS.now_recording = false;
    if (!GLOBALS.now_playing)
      return;
    GLOBALS.now_playing = false;
    GLOBALS.now_paused = false;
    if (!(bool) (UnityEngine.Object) GLOBALS.topclient)
      return;
    GLOBALS.topclient.Playback_Stopped();
  }

  public void PB_PlayPressed(bool state)
  {
    if (!state)
      return;
    float start_time = this.playback_slider.value;
    if ((long) ((double) start_time + 1.0) >= MyUtils.PB_GetEndTime())
      this.PB_StopPressed(true);
    else if (!MyUtils.PB_StartPlayback((long) start_time))
    {
      this.PB_StopPressed(true);
    }
    else
    {
      GLOBALS.now_recording = false;
      GLOBALS.now_playing = true;
      GLOBALS.now_paused = false;
      this.playback_play.gameObject.SetActive(true);
      this.playback_play.interactable = false;
      this.playback_play.isOn = true;
      this.playback_stop.gameObject.SetActive(true);
      this.playback_stop.interactable = true;
      this.playback_stop.isOn = false;
      this.playback_rec.gameObject.SetActive(false);
      this.playback_rec.interactable = false;
      this.playback_rec.isOn = false;
      this.playback_pause.gameObject.SetActive(true);
      this.playback_pause.interactable = true;
      this.playback_pause.isOn = false;
      this.playback_speed.gameObject.SetActive(true);
      this.playback_speed.interactable = true;
    }
  }

  public void PB_PausedPressed(bool state)
  {
    if (!GLOBALS.now_playing)
    {
      this.playback_pause.gameObject.SetActive(false);
      this.playback_pause.interactable = false;
      this.playback_pause.isOn = false;
    }
    else if (state)
      GLOBALS.now_paused = true;
    else
      GLOBALS.now_paused = false;
  }

  public void PB_SliderChanged(float value)
  {
    if (GLOBALS.now_recording || this.slider_changed_by_script)
      return;
    if (!GLOBALS.now_playing)
      this.PB_PlayPressed(true);
    MyUtils.PB_ChangeTime((long) value);
  }

  public void PB_ChangeSlider(float new_value)
  {
    this.slider_changed_by_script = true;
    this.playback_slider.value = new_value;
    this.slider_changed_by_script = false;
  }

  public void PB_ChangeSpeed(int value)
  {
    float speed;
    switch (value)
    {
      case 0:
        speed = 0.25f;
        break;
      case 1:
        speed = 0.5f;
        break;
      case 2:
        speed = 1f;
        break;
      case 3:
        speed = 2f;
        break;
      case 4:
        speed = 4f;
        break;
      default:
        speed = 1f;
        break;
    }
    MyUtils.PB_ChangeSpeed(speed);
  }

  public void CB_Referee(string msg)
  {
    if (!(bool) (UnityEngine.Object) GLOBALS.topclient)
      return;
    GLOBALS.topclient.CB_Referee(msg);
  }

  public void PointerEnteredActive() => ++this.pointer_over_active;

  public void PointerExitedActive() => --this.pointer_over_active;

  public bool IsPointerOverActive() => this.pointer_over_active > 0;
}
