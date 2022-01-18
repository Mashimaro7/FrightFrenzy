// Decompiled with JetBrains decompiler
// Type: Settings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
  public GameObject robotOptionsPanel;
  public GameObject skinsOptionsPanel;
  public GameObject controllerOptionsPanel;
  public GameObject drivetrainOptionsPanel;
  public GameObject otherOptionsPanel;
  public GameObject videoOptionsPanel;
  public GameObject keyboardGrid;
  public GameObject keyboardlineentry;
  public GameObject joysticklineentry;
  public GameObject joystickGrid;
  public GameObject joystickAdvancedMenu;
  public GameObject themeup;
  public GameObject themedown;
  public GameObject theme_graphics;
  public TMP_Dropdown game_dropdown;
  public TextMeshPro version_text;
  public GameObject ApplyVideoButton;
  public int curr_robotskins;
  public TextMeshProUGUI status_robot;
  private bool init_done;
  private List<string> themes = new List<string>((IEnumerable<string>) new string[11]
  {
    "Splish Splash",
    "Relic Recovery",
    "Rover Ruckus",
    "Skystone",
    "Infinite Recharge",
    "Change Up",
    "Bot Royale",
    "Ultimate Goal",
    "Tipping Point",
    "Freight Frenzy",
    "Rapid React"
  });
  public int current_theme = 11;
  private List<Dropdown.OptionData> resolutionList;
  private List<Dropdown.OptionData> qualityList;
  private GameObject robot_on_display;
  public GameObject robot_showcase;
  public GameObject license_locked;
  public Text license_daysleft;
  private string old_model = "";
  public Dropdown joystick_sate_dropdown;
  public GameObject savestring_go;
  public TMP_InputField save_name;
  public Text save_name_status;
  public bool savestate_joystick = true;
  public Dropdown joystick_state_dropdown;
  public Dropdown keyboard_state_dropdown;

  private void Awake() => CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

  private void Start()
  {
    if ((bool) (UnityEngine.Object) this.version_text)
      this.version_text.text = GLOBALS.VERSION;
    this.SetKeyboardToDefaults();
    this.SetJoystickToDefaults();
    this.LoadSkins("Eyes", "Eyes_", GLOBALS.skins_eyes);
    this.LoadSkins("Hats", "Hat_", GLOBALS.skins_hats);
    this.LoadSkins("Spoilers", "Spoiler_", GLOBALS.skins_spoilers);
    this.LoadSkins("Other", "Other_", GLOBALS.skins_other);
    GLOBALS.myLicenses.Clear();
    for (int index = 0; index < 99; ++index)
    {
      if (PlayerPrefs.HasKey("license" + (object) index))
        LicenseData.AddLicense(PlayerPrefs.GetString("license" + (object) index));
    }
    if (PlayerPrefs.HasKey("UDP_PORT"))
      GLOBALS.UDP_PORT = PlayerPrefs.GetInt("UDP_PORT");
    if (PlayerPrefs.HasKey("RobotModel"))
      GLOBALS.RobotModel = PlayerPrefs.GetString("RobotModel");
    if (PlayerPrefs.HasKey("RobotModelIndex"))
      GLOBALS.RobotModelIndex = PlayerPrefs.GetInt("RobotModelIndex");
    if (PlayerPrefs.HasKey("RobotModelCategory"))
      GLOBALS.RobotModelCategory = PlayerPrefs.GetInt("RobotModelCategory");
    if (PlayerPrefs.HasKey("DriveTrain"))
      GLOBALS.DriveTrain = PlayerPrefs.GetString("DriveTrain");
    if (PlayerPrefs.HasKey("DriveTrainIndex"))
      GLOBALS.DriveTrainIndex = PlayerPrefs.GetInt("DriveTrainIndex");
    if (PlayerPrefs.HasKey("MotorTypeIndex"))
      GLOBALS.motortypeindex = PlayerPrefs.GetInt("MotorTypeIndex");
    if (PlayerPrefs.HasKey("GearRatio"))
      GLOBALS.gear_ratio = PlayerPrefs.GetFloat("GearRatio");
    if (PlayerPrefs.HasKey("WheelDiameter"))
      GLOBALS.wheel_diameter = PlayerPrefs.GetFloat("WheelDiameter");
    if (PlayerPrefs.HasKey("Weight"))
      GLOBALS.weight = PlayerPrefs.GetFloat("Weight");
    if (PlayerPrefs.HasKey("turningScaler"))
      GLOBALS.turning_scaler = PlayerPrefs.GetFloat("turningScaler");
    if (PlayerPrefs.HasKey("fieldcentric"))
      GLOBALS.fieldcentric = PlayerPrefs.GetInt("fieldcentric") == 1;
    if (PlayerPrefs.HasKey("activebreaking"))
      GLOBALS.activebreaking = PlayerPrefs.GetInt("activebreaking") == 1;
    if (PlayerPrefs.HasKey("tankcontrol"))
      GLOBALS.tankcontrol = PlayerPrefs.GetInt("tankcontrol") == 1;
    if (PlayerPrefs.HasKey("FrameRate"))
      GLOBALS.framerate = PlayerPrefs.GetInt("FrameRate");
    if (PlayerPrefs.HasKey("interpolate"))
      GLOBALS.INTERPOLATE = PlayerPrefs.GetInt("interpolate") == 1;
    if (PlayerPrefs.HasKey("camera_averaging"))
      GLOBALS.CAMERA_AVERAGING = PlayerPrefs.GetInt("camera_averaging");
    if (PlayerPrefs.HasKey("audio"))
      GLOBALS.AUDIO = PlayerPrefs.GetInt("audio") == 1;
    if (PlayerPrefs.HasKey("robotaudio"))
      GLOBALS.ROBOTAUDIO = PlayerPrefs.GetInt("robotaudio") == 1;
    if (PlayerPrefs.HasKey("volume"))
      GLOBALS.VOLUME = PlayerPrefs.GetFloat("volume");
    if (PlayerPrefs.HasKey("WorldScale"))
      GLOBALS.worldscale = PlayerPrefs.GetFloat("WorldScale");
    if (PlayerPrefs.HasKey("video_fullscreen"))
      GLOBALS.video_fullscreen = PlayerPrefs.GetInt("video_fullscreen") == 1;
    if (PlayerPrefs.HasKey("video_quality"))
      GLOBALS.video_quality = PlayerPrefs.GetString("video_quality");
    if (PlayerPrefs.HasKey("playername"))
      GLOBALS.default_player_name = PlayerPrefs.GetString("playername");
    if (PlayerPrefs.HasKey("video_resolution"))
      GLOBALS.video_resolution = PlayerPrefs.GetString("video_resolution");
    if (PlayerPrefs.HasKey("skins"))
      GLOBALS.skins = PlayerPrefs.GetString("skins");
    if (PlayerPrefs.HasKey("robotskins"))
      GLOBALS.robotskins = PlayerPrefs.GetString("robotskins");
    if (PlayerPrefs.HasKey("game"))
      GLOBALS.GAME = PlayerPrefs.GetString("game");
    GLOBALS.GAME_INDEX = !PlayerPrefs.HasKey("gameindex") ? this.themes.Count - 1 : PlayerPrefs.GetInt("gameindex");
    if (PlayerPrefs.HasKey("replay_buffer"))
      GLOBALS.PB_BUFFER_DURATION = PlayerPrefs.GetInt("replay_buffer");
    Application.targetFrameRate = GLOBALS.framerate;
    foreach (string key in (IEnumerable<string>) GLOBALS.KeyboardMap.Keys)
    {
      if (PlayerPrefs.HasKey(key))
        GLOBALS.KeyboardMap[key].key = PlayerPrefs.GetString(key);
    }
    foreach (string key in (IEnumerable<string>) GLOBALS.JoystickMap.Keys)
    {
      if (PlayerPrefs.HasKey(key))
        GLOBALS.JoystickMap[key].FromString(PlayerPrefs.GetString(key));
    }
    this.LoadJoystickStates();
    this.LoadKeyboardStates();
    GLOBALS.GENERIC_DATA.Clear();
    for (int index = 1; index < 1000 && PlayerPrefs.HasKey("GD_" + (object) index); ++index)
    {
      string[] strArray = PlayerPrefs.GetString("GD_" + (object) index).Split('|');
      if (strArray.Length == 2)
        GLOBALS.GENERIC_DATA.Add(strArray[0], strArray[1]);
    }
    if ((UnityEngine.Object) this.robotOptionsPanel != (UnityEngine.Object) null)
    {
      if (this.robotOptionsPanel.transform.Find("Panel/Settings Robot/Category/Dropdown").GetComponent<Dropdown>().options.Count - 1 < GLOBALS.RobotModelCategory)
        GLOBALS.RobotModelCategory = 0;
      string text = this.robotOptionsPanel.transform.Find("Panel/Settings Robot/Category/Dropdown").GetComponent<Dropdown>().options[GLOBALS.RobotModelCategory].text;
      if (this.robotOptionsPanel.transform.Find("Panel/Settings Robot/Model " + text + "/Dropdown").GetComponent<Dropdown>().options.Count - 1 < GLOBALS.RobotModelIndex)
        GLOBALS.RobotModelIndex = 0;
      this.robotOptionsPanel.transform.Find("Panel/Settings Robot/Category/Dropdown").GetComponent<Dropdown>().value = GLOBALS.RobotModelCategory;
      this.robotOptionsPanel.transform.Find("Panel/Settings Robot/Model " + text + "/Dropdown").GetComponent<Dropdown>().value = GLOBALS.RobotModelIndex;
      GLOBALS.RobotModel = this.robotOptionsPanel.transform.Find("Panel/Settings Robot/Model " + text + "/Dropdown").GetComponent<Dropdown>().options[GLOBALS.RobotModelIndex].text;
      this.old_model = GLOBALS.RobotModel;
      Transform transform1 = this.robotOptionsPanel.transform.Find("Panel/Settings Robot");
      GLOBALS.all_robot_capital_names.Clear();
      foreach (Transform transform2 in transform1)
      {
        if (transform2.name.StartsWith("Model"))
        {
          foreach (Dropdown.OptionData option in transform2.GetComponentInChildren<Dropdown>().options)
            GLOBALS.all_robot_capital_names.Add(option.text.ToUpper());
        }
      }
    }
    if ((UnityEngine.Object) this.otherOptionsPanel != (UnityEngine.Object) null)
    {
      this.otherOptionsPanel.transform.Find("Panel/Settings/FrameRate").GetComponent<InputField>().text = GLOBALS.framerate.ToString();
      this.otherOptionsPanel.transform.Find("Panel/Settings/Interpolate").GetComponent<Toggle>().isOn = GLOBALS.INTERPOLATE;
      this.otherOptionsPanel.transform.Find("Panel/Settings/CameraSmoothing").GetComponent<InputField>().text = GLOBALS.CAMERA_AVERAGING.ToString();
      this.otherOptionsPanel.transform.Find("Panel/Settings/Audio").GetComponent<Toggle>().isOn = GLOBALS.AUDIO;
      this.otherOptionsPanel.transform.Find("Panel/Settings/RobotAudio").GetComponent<Toggle>().isOn = GLOBALS.ROBOTAUDIO;
      this.otherOptionsPanel.transform.Find("Panel/Settings/Volume").GetComponent<Slider>().value = GLOBALS.VOLUME;
      this.otherOptionsPanel.transform.Find("Panel/Settings/RecordingTime").GetComponent<Slider>().value = (float) GLOBALS.PB_BUFFER_DURATION;
      this.otherOptionsPanel.transform.Find("Panel/Settings/RecordingTime/Text").GetComponent<Text>().text = GLOBALS.PB_BUFFER_DURATION.ToString();
      this.otherOptionsPanel.transform.Find("Panel/Settings/WorldScale").GetComponent<InputField>().text = GLOBALS.worldscale.ToString();
    }
    if ((UnityEngine.Object) this.videoOptionsPanel != (UnityEngine.Object) null)
    {
      this.resolutionList = new List<Dropdown.OptionData>();
      foreach (Resolution resolution in Screen.resolutions)
        this.resolutionList.Add(new Dropdown.OptionData(resolution.ToString()));
      this.videoOptionsPanel.transform.Find("Panel/Settings/Resolution/Dropdown").GetComponent<Dropdown>().ClearOptions();
      this.videoOptionsPanel.transform.Find("Panel/Settings/Resolution/Dropdown").GetComponent<Dropdown>().AddOptions(this.resolutionList);
      int index1 = 0;
      while (index1 < this.resolutionList.Count && !(this.resolutionList[index1].text == GLOBALS.video_resolution))
        ++index1;
      this.videoOptionsPanel.transform.Find("Panel/Settings/Resolution/Dropdown").GetComponent<Dropdown>().value = index1;
      this.qualityList = new List<Dropdown.OptionData>();
      foreach (object name in QualitySettings.names)
        this.qualityList.Add(new Dropdown.OptionData(name.ToString()));
      this.videoOptionsPanel.transform.Find("Panel/Settings/Quality/Dropdown").GetComponent<Dropdown>().ClearOptions();
      this.videoOptionsPanel.transform.Find("Panel/Settings/Quality/Dropdown").GetComponent<Dropdown>().AddOptions(this.qualityList);
      int index2 = 0;
      while (index2 < this.qualityList.Count && !(this.qualityList[index2].text == GLOBALS.video_quality))
        ++index2;
      this.videoOptionsPanel.transform.Find("Panel/Settings/Quality/Dropdown").GetComponent<Dropdown>().value = index2;
      this.videoOptionsPanel.transform.Find("Panel/Settings/Full Screen").GetComponent<Toggle>().isOn = GLOBALS.video_fullscreen;
    }
    if ((UnityEngine.Object) this.drivetrainOptionsPanel != (UnityEngine.Object) null)
    {
      DriveTrainCalcs.CalcDriveTrain(GLOBALS.wheel_diameter, GLOBALS.gear_ratio, GLOBALS.motortypeindex, GLOBALS.weight, out GLOBALS.speed, out GLOBALS.acceleration);
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/Drivetrain/Dropdown").GetComponent<Dropdown>().value = GLOBALS.DriveTrainIndex;
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/MotorType/Dropdown").GetComponent<Dropdown>().value = GLOBALS.motortypeindex;
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/GearRatio").GetComponent<InputField>().text = GLOBALS.gear_ratio.ToString();
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/wheelDiameter").GetComponent<InputField>().text = GLOBALS.wheel_diameter.ToString();
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/Weight").GetComponent<InputField>().text = GLOBALS.weight.ToString();
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/TurningSpeed").GetComponent<Slider>().value = GLOBALS.turning_scaler;
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/TurningSpeed2").GetComponent<InputField>().text = (GLOBALS.turning_scaler * 100f).ToString();
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/FieldCentric").GetComponent<Toggle>().isOn = GLOBALS.fieldcentric;
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/ActiveBreaking").GetComponent<Toggle>().isOn = GLOBALS.activebreaking;
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/TankControl").GetComponent<Toggle>().isOn = GLOBALS.tankcontrol;
    }
    this.LoadJoystickStates();
    this.LoadKeyboardStates();
    this.PopulateKeyboardGrid();
    this.PopulateJoystickGrid();
    if ((UnityEngine.Object) this.skinsOptionsPanel != (UnityEngine.Object) null)
    {
      string[] strArray = GLOBALS.skins.Split(':');
      if (strArray.Length != 0)
        this.skinsOptionsPanel.transform.Find("Panel/Settings/Eyes/InputField").GetComponent<InputField>().text = strArray[0];
      if (strArray.Length > 1)
        this.skinsOptionsPanel.transform.Find("Panel/Settings/Hat/InputField").GetComponent<InputField>().text = strArray[1];
      if (strArray.Length > 2)
        this.skinsOptionsPanel.transform.Find("Panel/Settings/Spoiler/InputField").GetComponent<InputField>().text = strArray[2];
      if (strArray.Length > 3)
        this.skinsOptionsPanel.transform.Find("Panel/Settings/Other/InputField").GetComponent<InputField>().text = strArray[3];
      this.curr_robotskins = this.UpdateRobotSkins();
    }
    if ((UnityEngine.Object) this.game_dropdown != (UnityEngine.Object) null)
    {
      this.game_dropdown.ClearOptions();
      this.game_dropdown.AddOptions(this.themes);
    }
    this.current_theme = this.themes.Count;
    this.MoveThemeToTarget(GLOBALS.GAME_INDEX);
    this.init_done = true;
    GLOBALS.settings_loaded = true;
    if (Application.isBatchMode)
    {
      GLOBALS.HEADLESS_MODE = true;
      Console.Out.WriteLine("****** xRC Simulator *********** ");
      Console.Out.WriteLine("****** Version " + GLOBALS.VERSION + " ************* ");
      Console.Out.WriteLine("Time started: " + DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
      Console.Out.WriteLine("\n\n");
      this.ProcessHeadlessCommands();
    }
    else
      this.OnMenuChanged();
  }

  public void MoveThemeToTarget(int index = 0)
  {
    if (index == this.current_theme)
      return;
    if (index > this.current_theme)
      this.MoveThemeUp(index - this.current_theme);
    else
      this.MoveThemeDown(this.current_theme - index);
  }

  public void ProcessHeadlessCommands()
  {
    foreach (string commandLineArg in Environment.GetCommandLineArgs())
    {
      char[] chArray = new char[1]{ '=' };
      string[] strArray = commandLineArg.Split(chArray);
      if (strArray[0].ToLower().Equals("game"))
      {
        if (strArray.Length < 2)
        {
          Console.Out.WriteLine("Game incorrectly specified. Specify as GAME=# where # starts from 0. 0 = Splish Splash.");
        }
        else
        {
          int result = 0;
          if (!int.TryParse(strArray[1], out result))
            Console.Out.WriteLine("Game incorrectly specified - unable to parse integer after GAME= command line. Specify as GAME=# where # starts from 0. 0 = Splish Splash.");
          else if (result < 0 || result >= this.themes.Count)
          {
            Console.Out.WriteLine("Game number out of range. Starting game # = 0 and last game # is " + (object) (this.themes.Count - 1) + ". You specified " + (object) result + ".");
          }
          else
          {
            GLOBALS.GAME = this.themes[result];
            GLOBALS.GAME_INDEX = result;
            Console.Out.WriteLine("Game = " + this.themes[result]);
          }
        }
      }
      else if (strArray[0].ToLower().Equals("framerate"))
      {
        if (strArray.Length < 2)
        {
          Console.Out.WriteLine("Framerate incorrectly specified. Specify as FRAMERATE=##.");
        }
        else
        {
          int result = 0;
          if (!int.TryParse(strArray[1], out result))
            Console.Out.WriteLine("Framerate incorrectly specified - unable to parse integer after FRAMERATE= command line. Specify as FRAMERATE=##.");
          else if (result < 0 || result >= 120)
          {
            Console.Out.WriteLine("Framerate specified invalid range. Needs to be between 1 and 120.");
          }
          else
          {
            GLOBALS.framerate = result;
            Application.targetFrameRate = result;
          }
        }
      }
    }
  }

  public static void SavePrefs()
  {
    if (!GLOBALS.settings_loaded)
      return;
    PlayerPrefs.DeleteAll();
    PlayerPrefs.SetInt("UDP_PORT", GLOBALS.UDP_PORT);
    PlayerPrefs.SetString("RobotModel", GLOBALS.RobotModel);
    PlayerPrefs.SetString("skins", GLOBALS.skins);
    PlayerPrefs.SetString("robotskins", GLOBALS.robotskins);
    PlayerPrefs.SetInt("RobotModelIndex", GLOBALS.RobotModelIndex);
    PlayerPrefs.SetInt("RobotModelCategory", GLOBALS.RobotModelCategory);
    PlayerPrefs.SetString("DriveTrain", GLOBALS.DriveTrain);
    PlayerPrefs.SetInt("DriveTrainIndex", GLOBALS.DriveTrainIndex);
    PlayerPrefs.SetInt("MotorTypeIndex", GLOBALS.motortypeindex);
    PlayerPrefs.SetFloat("GearRatio", GLOBALS.gear_ratio);
    PlayerPrefs.SetFloat("WheelDiameter", GLOBALS.wheel_diameter);
    PlayerPrefs.SetFloat("Weight", GLOBALS.weight);
    PlayerPrefs.SetFloat("turningScaler", GLOBALS.turning_scaler);
    PlayerPrefs.SetInt("fieldcentric", GLOBALS.fieldcentric ? 1 : 0);
    PlayerPrefs.SetInt("activebreaking", GLOBALS.activebreaking ? 1 : 0);
    PlayerPrefs.SetInt("tankcontrol", GLOBALS.tankcontrol ? 1 : 0);
    PlayerPrefs.SetInt("FrameRate", GLOBALS.framerate);
    PlayerPrefs.SetInt("interpolate", GLOBALS.INTERPOLATE ? 1 : 0);
    PlayerPrefs.SetInt("camera_averaging", GLOBALS.CAMERA_AVERAGING);
    PlayerPrefs.SetInt("audio", GLOBALS.AUDIO ? 1 : 0);
    PlayerPrefs.SetInt("robotaudio", GLOBALS.ROBOTAUDIO ? 1 : 0);
    PlayerPrefs.SetFloat("volume", GLOBALS.VOLUME);
    PlayerPrefs.SetFloat("WorldScale", GLOBALS.worldscale);
    PlayerPrefs.SetInt("video_fullscreen", GLOBALS.video_fullscreen ? 1 : 0);
    PlayerPrefs.SetString("video_quality", GLOBALS.video_quality);
    PlayerPrefs.SetString("playername", GLOBALS.default_player_name);
    PlayerPrefs.SetString("video_resolution", GLOBALS.video_resolution);
    PlayerPrefs.SetString("game", GLOBALS.GAME);
    PlayerPrefs.SetInt("gameindex", GLOBALS.GAME_INDEX);
    PlayerPrefs.SetInt("replay_buffer", GLOBALS.PB_BUFFER_DURATION);
    foreach (string key in (IEnumerable<string>) GLOBALS.KeyboardMap.Keys)
      PlayerPrefs.SetString(key, GLOBALS.KeyboardMap[key].key);
    foreach (string key in (IEnumerable<string>) GLOBALS.JoystickMap.Keys)
      PlayerPrefs.SetString(key, GLOBALS.JoystickMap[key].GetString());
    Settings.SaveJoystickStates();
    Settings.SaveKeyboardStates();
    int num1 = 0;
    foreach (LicenseData license in GLOBALS.myLicenses)
    {
      if (license.DaysLeft() >= 0)
        PlayerPrefs.SetString("license" + (object) num1++, license.ToString());
    }
    int num2 = 1;
    foreach (string key in GLOBALS.GENERIC_DATA.Keys)
      PlayerPrefs.SetString("GD_" + (object) num2++, key + "|" + GLOBALS.GENERIC_DATA[key]);
    PlayerPrefs.Save();
  }

  private void LoadSkins(string dir_name, string file_header, Dictionary<int, GameObject> prefabs)
  {
    prefabs.Clear();
    for (int key = 1; key < 100; ++key)
    {
      GameObject gameObject = UnityEngine.Resources.Load("Skins/" + dir_name + "/" + file_header + key.ToString()) as GameObject;
      if (!(bool) (UnityEngine.Object) gameObject)
        break;
      prefabs.Add(key, gameObject);
    }
  }

  public void OnMenuChanged()
  {
    if (!this.init_done)
      return;
    if ((UnityEngine.Object) this.robotOptionsPanel != (UnityEngine.Object) null)
    {
      Dropdown component = this.robotOptionsPanel.transform.Find("Panel/Settings Robot/Category/Dropdown").GetComponent<Dropdown>();
      GLOBALS.RobotModelCategory = component.value;
      string text = component.options[GLOBALS.RobotModelCategory].text;
      foreach (Dropdown.OptionData option in component.options)
        this.robotOptionsPanel.transform.Find("Panel/Settings Robot/Model " + option.text).gameObject.SetActive(false);
      this.robotOptionsPanel.transform.Find("Panel/Settings Robot/Model " + text).gameObject.SetActive(true);
      GLOBALS.RobotModelIndex = this.robotOptionsPanel.transform.Find("Panel/Settings Robot/Model " + text + "/Dropdown").GetComponent<Dropdown>().value;
      GLOBALS.RobotModel = this.robotOptionsPanel.transform.Find("Panel/Settings Robot/Model " + text + "/Dropdown").GetComponent<Dropdown>().options[GLOBALS.RobotModelIndex].text;
      this.status_robot.text = GLOBALS.RobotModel;
      if (this.old_model != GLOBALS.RobotModel)
      {
        GLOBALS.robotskins = "Real";
        this.curr_robotskins = this.UpdateRobotSkins();
        this.old_model = GLOBALS.RobotModel;
        this.LoadJoystickState(GLOBALS.RobotModel);
      }
    }
    if ((UnityEngine.Object) this.otherOptionsPanel != (UnityEngine.Object) null)
    {
      int result1 = 60;
      if (!int.TryParse(this.otherOptionsPanel.transform.Find("Panel/Settings/FrameRate").GetComponent<InputField>().text, out result1))
        result1 = 60;
      if (result1 < 15)
        result1 = 15;
      this.otherOptionsPanel.transform.Find("Panel/Settings/FrameRate").GetComponent<InputField>().text = result1.ToString();
      GLOBALS.framerate = result1;
      Application.targetFrameRate = result1;
      GLOBALS.INTERPOLATE = this.otherOptionsPanel.transform.Find("Panel/Settings/Interpolate").GetComponent<Toggle>().isOn;
      int result2 = 1;
      if (!int.TryParse(this.otherOptionsPanel.transform.Find("Panel/Settings/CameraSmoothing").GetComponent<InputField>().text, out result2))
        result2 = 1;
      if (result2 < 1)
        result2 = 1;
      if (result2 > 20)
        result2 = 20;
      this.otherOptionsPanel.transform.Find("Panel/Settings/CameraSmoothing").GetComponent<InputField>().text = result2.ToString();
      GLOBALS.CAMERA_AVERAGING = result2;
      float result3 = 2f;
      if (!float.TryParse(this.otherOptionsPanel.transform.Find("Panel/Settings/WorldScale").GetComponent<InputField>().text, out result3))
        result3 = 2f;
      if ((double) result3 > 4.0)
        result3 = 4f;
      if ((double) result3 < 0.100000001490116)
        result3 = 0.1f;
      this.otherOptionsPanel.transform.Find("Panel/Settings/WorldScale").GetComponent<InputField>().text = result3.ToString();
      GLOBALS.worldscale = result3;
      GLOBALS.AUDIO = this.otherOptionsPanel.transform.Find("Panel/Settings/Audio").GetComponent<Toggle>().isOn;
      GLOBALS.ROBOTAUDIO = this.otherOptionsPanel.transform.Find("Panel/Settings/RobotAudio").GetComponent<Toggle>().isOn;
      GLOBALS.VOLUME = this.otherOptionsPanel.transform.Find("Panel/Settings/Volume").GetComponent<Slider>().value;
      GLOBALS.PB_BUFFER_DURATION = (int) this.otherOptionsPanel.transform.Find("Panel/Settings/RecordingTime").GetComponent<Slider>().value;
      this.otherOptionsPanel.transform.Find("Panel/Settings/RecordingTime/Text").GetComponent<Text>().text = GLOBALS.PB_BUFFER_DURATION.ToString();
      AudioListener.volume = GLOBALS.VOLUME;
    }
    if ((UnityEngine.Object) this.videoOptionsPanel != (UnityEngine.Object) null)
    {
      if (this.videoOptionsPanel.transform.Find("Panel/Settings/Resolution/Dropdown").GetComponent<Dropdown>().options.Count > 0)
      {
        try
        {
          int index1 = this.videoOptionsPanel.transform.Find("Panel/Settings/Resolution/Dropdown").GetComponent<Dropdown>().value;
          GLOBALS.video_resolution = this.videoOptionsPanel.transform.Find("Panel/Settings/Resolution/Dropdown").GetComponent<Dropdown>().options[index1].text;
          int index2 = this.videoOptionsPanel.transform.Find("Panel/Settings/Quality/Dropdown").GetComponent<Dropdown>().value;
          GLOBALS.video_quality = this.videoOptionsPanel.transform.Find("Panel/Settings/Quality/Dropdown").GetComponent<Dropdown>().options[index2].text;
          GLOBALS.video_fullscreen = this.videoOptionsPanel.transform.Find("Panel/Settings/Full Screen").GetComponent<Toggle>().isOn;
          if (GLOBALS.HEADLESS_MODE)
            index2 = 0;
          QualitySettings.SetQualityLevel(index2, true);
          if (GLOBALS.video_fullscreen)
            Screen.SetResolution(Screen.resolutions[index1].width, Screen.resolutions[index1].height, GLOBALS.video_fullscreen, Screen.resolutions[index1].refreshRate);
          else
            Screen.SetResolution(Screen.width, Screen.height, GLOBALS.video_fullscreen);
        }
        catch (Exception ex)
        {
        }
      }
    }
    if ((UnityEngine.Object) this.drivetrainOptionsPanel != (UnityEngine.Object) null)
    {
      GLOBALS.DriveTrainIndex = this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/Drivetrain/Dropdown").GetComponent<Dropdown>().value;
      GLOBALS.DriveTrain = this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/Drivetrain/Dropdown").GetComponent<Dropdown>().options[GLOBALS.DriveTrainIndex].text;
      GLOBALS.motortypeindex = this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/MotorType/Dropdown").GetComponent<Dropdown>().value;
      GLOBALS.gear_ratio = float.Parse(this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/GearRatio").GetComponent<InputField>().text);
      GLOBALS.wheel_diameter = float.Parse(this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/wheelDiameter").GetComponent<InputField>().text);
      GLOBALS.weight = float.Parse(this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/Weight").GetComponent<InputField>().text);
      GLOBALS.turning_scaler = this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/TurningSpeed").GetComponent<Slider>().value;
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/TurningSpeed2").GetComponent<InputField>().text = (GLOBALS.turning_scaler * 100f).ToString();
      GLOBALS.fieldcentric = this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/FieldCentric").GetComponent<Toggle>().isOn;
      GLOBALS.activebreaking = this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/ActiveBreaking").GetComponent<Toggle>().isOn;
      GLOBALS.tankcontrol = this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/TankControl").GetComponent<Toggle>().isOn;
    }
    if ((UnityEngine.Object) this.skinsOptionsPanel != (UnityEngine.Object) null)
    {
      int num1 = int.Parse(this.skinsOptionsPanel.transform.Find("Panel/Settings/Eyes/InputField").GetComponent<InputField>().text);
      if (num1 < 0)
        this.skinsOptionsPanel.transform.Find("Panel/Settings/Eyes/InputField").GetComponent<InputField>().text = "0";
      int count;
      if (num1 > GLOBALS.skins_eyes.Count)
      {
        InputField component = this.skinsOptionsPanel.transform.Find("Panel/Settings/Eyes/InputField").GetComponent<InputField>();
        count = GLOBALS.skins_eyes.Count;
        string str = count.ToString();
        component.text = str;
      }
      int num2 = int.Parse(this.skinsOptionsPanel.transform.Find("Panel/Settings/Hat/InputField").GetComponent<InputField>().text);
      if (num2 < 0)
        this.skinsOptionsPanel.transform.Find("Panel/Settings/Hat/InputField").GetComponent<InputField>().text = "0";
      if (num2 > GLOBALS.skins_hats.Count)
      {
        InputField component = this.skinsOptionsPanel.transform.Find("Panel/Settings/Hat/InputField").GetComponent<InputField>();
        count = GLOBALS.skins_hats.Count;
        string str = count.ToString();
        component.text = str;
      }
      int num3 = int.Parse(this.skinsOptionsPanel.transform.Find("Panel/Settings/Spoiler/InputField").GetComponent<InputField>().text);
      if (num3 < 0)
        this.skinsOptionsPanel.transform.Find("Panel/Settings/Spoiler/InputField").GetComponent<InputField>().text = "0";
      if (num3 > GLOBALS.skins_spoilers.Count)
      {
        InputField component = this.skinsOptionsPanel.transform.Find("Panel/Settings/Spoiler/InputField").GetComponent<InputField>();
        count = GLOBALS.skins_spoilers.Count;
        string str = count.ToString();
        component.text = str;
      }
      int num4 = int.Parse(this.skinsOptionsPanel.transform.Find("Panel/Settings/Other/InputField").GetComponent<InputField>().text);
      if (num4 < 0)
        this.skinsOptionsPanel.transform.Find("Panel/Settings/Other/InputField").GetComponent<InputField>().text = "0";
      if (num4 > GLOBALS.skins_other.Count)
      {
        InputField component = this.skinsOptionsPanel.transform.Find("Panel/Settings/Other/InputField").GetComponent<InputField>();
        count = GLOBALS.skins_other.Count;
        string str = count.ToString();
        component.text = str;
      }
      GLOBALS.skins = this.skinsOptionsPanel.transform.Find("Panel/Settings/Eyes/InputField").GetComponent<InputField>().text;
      GLOBALS.skins += ":";
      GLOBALS.skins += this.skinsOptionsPanel.transform.Find("Panel/Settings/Hat/InputField").GetComponent<InputField>().text;
      GLOBALS.skins += ":";
      GLOBALS.skins += this.skinsOptionsPanel.transform.Find("Panel/Settings/Spoiler/InputField").GetComponent<InputField>().text;
      GLOBALS.skins += ":";
      GLOBALS.skins += this.skinsOptionsPanel.transform.Find("Panel/Settings/Other/InputField").GetComponent<InputField>().text;
      if (this.curr_robotskins < 0)
        this.curr_robotskins = GLOBALS.robotskinslist.Count - 1;
      if (this.curr_robotskins >= GLOBALS.robotskinslist.Count)
        this.curr_robotskins = 0;
      GLOBALS.robotskins = GLOBALS.robotskinslist.Count >= 1 ? GLOBALS.robotskinslist[this.curr_robotskins] : "";
    }
    foreach (string key in (IEnumerable<string>) GLOBALS.KeyboardMap.Keys)
    {
      Transform transform = this.keyboardGrid.transform.Find(key);
      if ((UnityEngine.Object) transform != (UnityEngine.Object) null)
        GLOBALS.KeyboardMap[key].key = transform.Find("InputField").GetComponent<InputField>().text;
    }
    Settings.SavePrefs();
    if (!GLOBALS.AUDIO)
    {
      foreach (AudioSource audioSource in UnityEngine.Object.FindObjectsOfType<AudioSource>())
        audioSource.Stop();
    }
    if ((bool) (UnityEngine.Object) this.robot_on_display)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.robot_on_display);
    this.robot_on_display = MyUtils.InstantiateRobot(GLOBALS.RobotModel, Vector3.zero, Quaternion.identity, GLOBALS.skins, GLOBALS.robotskins);
    this.robot_on_display.transform.SetParent(this.robot_showcase.transform);
    this.robot_on_display.transform.localPosition = Vector3.zero;
    this.robot_on_display.transform.localRotation = Quaternion.identity;
    RobotInterface3D componentInChildren = this.robot_on_display.GetComponentInChildren<RobotInterface3D>();
    if ((bool) (UnityEngine.Object) componentInChildren && (bool) (UnityEngine.Object) this.robotOptionsPanel)
    {
      MyUtils.FindHierarchy(this.robotOptionsPanel.transform, "RobotInfo").GetComponent<Text>().text = componentInChildren.info;
      componentInChildren.SetUserParameters();
      this.LockRobotParameters(componentInChildren);
      foreach (Behaviour componentsInChild in componentInChildren.GetComponentsInChildren<interpolation>())
        componentsInChild.enabled = false;
    }
    this.license_daysleft.text = "";
    if (LicenseData.CheckRobotIsUnlocked(GLOBALS.RobotModel, GLOBALS.robotskins))
    {
      this.license_locked.SetActive(false);
      int featureDaysLeft = LicenseData.GetFeatureDaysLeft(GLOBALS.RobotModel, GLOBALS.robotskins);
      if (featureDaysLeft < 9999)
        this.license_daysleft.text = featureDaysLeft.ToString() + " Days";
    }
    else
      this.license_locked.SetActive(true);
    UnityEngine.Resources.UnloadUnusedAssets();
    this.ApplyVideoButton.SetActive(false);
  }

  private void LockRobotParameters(RobotInterface3D ri3d)
  {
    if (ri3d.total_weight_lock)
    {
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/Weight").GetComponent<InputField>().interactable = false;
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/Weight").GetComponent<InputField>().text = ri3d.total_weight.ToString();
    }
    else
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/Weight").GetComponent<InputField>().interactable = true;
    if (ri3d.max_speed_lock || ri3d.max_acceleration_lock || ri3d.lock_all_parameters)
    {
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/GearRatio").GetComponent<InputField>().interactable = false;
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/wheelDiameter").GetComponent<InputField>().interactable = false;
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/Weight").GetComponent<InputField>().interactable = false;
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/MotorType/Dropdown").GetComponent<Dropdown>().interactable = false;
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/Graph/notused").GetComponent<Image>().enabled = true;
    }
    else
    {
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/GearRatio").GetComponent<InputField>().interactable = true;
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/wheelDiameter").GetComponent<InputField>().interactable = true;
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/MotorType/Dropdown").GetComponent<Dropdown>().interactable = true;
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/Graph/notused").GetComponent<Image>().enabled = false;
    }
    if (ri3d.DriveTrain_lock)
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/Drivetrain/Dropdown").GetComponent<Dropdown>().interactable = false;
    else
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/Drivetrain/Dropdown").GetComponent<Dropdown>().interactable = true;
    if (ri3d.turn_scale_lock)
    {
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/TurningSpeed").GetComponent<Slider>().interactable = false;
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/TurningSpeed2").GetComponent<InputField>().interactable = false;
    }
    else
    {
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/TurningSpeed").GetComponent<Slider>().interactable = true;
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/TurningSpeed2").GetComponent<InputField>().interactable = true;
    }
    if (ri3d.fieldcentric_lock)
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/FieldCentric").GetComponent<Toggle>().interactable = false;
    else
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/FieldCentric").GetComponent<Toggle>().interactable = true;
    if (ri3d.activebreaking_lock)
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/ActiveBreaking").GetComponent<Toggle>().interactable = false;
    else
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/ActiveBreaking").GetComponent<Toggle>().interactable = true;
    if (ri3d.tankcontrol_lock)
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/TankControl").GetComponent<Toggle>().interactable = false;
    else
      this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/TankControl").GetComponent<Toggle>().interactable = true;
    Dropdown component = this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/Drivetrain/Dropdown").GetComponent<Dropdown>();
    if (!ri3d.valid_DriveTrains.Contains(GLOBALS.DriveTrain))
    {
      component.value = 0;
      GLOBALS.DriveTrainIndex = 0;
      GLOBALS.DriveTrain = component.options[GLOBALS.DriveTrainIndex].text;
    }
    foreach (Dropdown.OptionData option in component.options)
    {
      if (option.text.StartsWith("[X] "))
        option.text = option.text.Substring(4);
      if (!ri3d.valid_DriveTrains.Contains(option.text))
        option.text = "[X] " + option.text;
    }
  }

  public void OnTurningScaler2Changed()
  {
    GLOBALS.turning_scaler = float.Parse(this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/TurningSpeed2").GetComponent<InputField>().text) / 100f;
    if ((double) GLOBALS.turning_scaler < 0.100000001490116)
      GLOBALS.turning_scaler = 0.1f;
    if ((double) GLOBALS.turning_scaler > 1.0)
      GLOBALS.turning_scaler = 1f;
    this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/TurningSpeed2").GetComponent<InputField>().text = (GLOBALS.turning_scaler * 100f).ToString();
    this.drivetrainOptionsPanel.transform.Find("Panel/Settings Drivetrain/TurningSpeed").GetComponent<Slider>().value = GLOBALS.turning_scaler;
  }

  public int UpdateRobotSkins()
  {
    GLOBALS.robotskinslist.Clear();
    UnityEngine.Object[] objectArray = UnityEngine.Resources.LoadAll("Robots/Skins/" + GLOBALS.RobotModel, typeof (GameObject));
    if (objectArray != null && objectArray.Length != 0)
    {
      foreach (UnityEngine.Object @object in objectArray)
        GLOBALS.robotskinslist.Add(@object.name);
      if (GLOBALS.robotskinslist.Contains(GLOBALS.robotskins))
        return GLOBALS.robotskinslist.IndexOf(GLOBALS.robotskins);
      if (GLOBALS.robotskinslist.Contains("Default"))
      {
        GLOBALS.robotskins = "Default";
        return GLOBALS.robotskinslist.IndexOf(GLOBALS.robotskins);
      }
    }
    GLOBALS.robotskins = "";
    return -1;
  }

  public void MoveThemeUp(int steps = 1)
  {
    if ((UnityEngine.Object) this.theme_graphics == (UnityEngine.Object) null || this.theme_graphics.GetComponent<ThemeAnimation>().animation_started)
      return;
    int currentTheme = this.current_theme;
    this.current_theme += steps;
    if (this.current_theme > this.themes.Count - 1)
    {
      this.current_theme = this.themes.Count - 1;
      steps = this.current_theme - currentTheme;
    }
    this.theme_graphics.GetComponent<ThemeAnimation>().MoveUp((float) steps);
    if (this.current_theme >= this.themes.Count - 1)
    {
      GLOBALS.GAME = this.themes[this.themes.Count - 1];
      this.themeup.SetActive(false);
      this.themedown.SetActive(true);
    }
    else
    {
      this.themeup.SetActive(true);
      this.themedown.SetActive(true);
    }
    GLOBALS.GAME = this.themes[this.current_theme];
    GLOBALS.GAME_INDEX = this.current_theme;
    Settings.SavePrefs();
    this.game_dropdown.value = this.current_theme;
  }

  public void MoveThemeDown(int steps = 1)
  {
    if ((UnityEngine.Object) this.theme_graphics == (UnityEngine.Object) null || this.theme_graphics.GetComponent<ThemeAnimation>().animation_started)
      return;
    int currentTheme = this.current_theme;
    this.current_theme -= steps;
    if (this.current_theme < 0)
    {
      this.current_theme = 0;
      steps = currentTheme - this.current_theme;
    }
    this.theme_graphics.GetComponent<ThemeAnimation>().MoveDown((float) steps);
    if (this.current_theme < this.themes.Count - 1)
      this.themeup.SetActive(true);
    else
      this.themeup.SetActive(false);
    if (this.current_theme <= 0)
      this.themedown.SetActive(false);
    else
      this.themedown.SetActive(true);
    GLOBALS.GAME = this.themes[this.current_theme];
    GLOBALS.GAME_INDEX = this.current_theme;
    Settings.SavePrefs();
    this.game_dropdown.value = this.current_theme;
  }

  public void ResetKeyboardToDefaults()
  {
    this.SetKeyboardToDefaults();
    Settings.SavePrefs();
    this.PopulateKeyboardGrid();
    GLOBALS.keyboard_inuse = false;
  }

  public void ResetJoystickToDefaults()
  {
    this.SetJoystickToDefaults();
    Settings.SavePrefs();
    this.PopulateJoystickGrid();
    GLOBALS.keyboard_inuse = false;
  }

  public void SetKeyboardToDefaults()
  {
    GLOBALS.KeyboardMap.Clear();
    GLOBALS.KeyboardMap = (IDictionary<string, keyinfo>) new Dictionary<string, keyinfo>()
    {
      {
        "controlls_A",
        new keyinfo("u", "Button A")
      },
      {
        "controlls_B",
        new keyinfo("o", "Button B")
      },
      {
        "controlls_X",
        new keyinfo("i", "Button X")
      },
      {
        "controlls_Y",
        new keyinfo("k", "Button Y")
      },
      {
        "controlls_LD",
        new keyinfo("q", "Left Trigger")
      },
      {
        "controlls_RD",
        new keyinfo("z", "Right Trigger")
      },
      {
        "controlls_LT",
        new keyinfo("e", "Slow Turn L")
      },
      {
        "controlls_RT",
        new keyinfo("c", "Slow Turn R")
      },
      {
        "controlls_turn_l",
        new keyinfo("j", "Turn Left")
      },
      {
        "controlls_turn_r",
        new keyinfo("l", "Turn Right")
      },
      {
        "controlls_move_u",
        new keyinfo("w", "Move Up")
      },
      {
        "controlls_move_d",
        new keyinfo("s", "Move Down")
      },
      {
        "controlls_move_l",
        new keyinfo("a", "Move Left")
      },
      {
        "controlls_move_r",
        new keyinfo("d", "Move Right")
      },
      {
        "controlls_dpad_u",
        new keyinfo("t", "D-Pad Up")
      },
      {
        "controlls_dpad_l",
        new keyinfo("f", "D-Pad Left")
      },
      {
        "controlls_dpad_d",
        new keyinfo("g", "D-Pad Down")
      },
      {
        "controlls_dpad_r",
        new keyinfo("h", "D-Pad Right")
      },
      {
        "controlls_stop",
        new keyinfo("[", "Stop Timer")
      },
      {
        "controlls_restart",
        new keyinfo("]", "Start Timer")
      },
      {
        "controlls_reset",
        new keyinfo("\\", "Reset Position")
      },
      {
        "controlls_rightstick_up",
        new keyinfo("p", "Right-Stick Up")
      },
      {
        "controlls_rightstick_down",
        new keyinfo(";", "Right-Stick Down")
      }
    };
  }

  public void PopulateKeyboardGrid()
  {
    if (!((UnityEngine.Object) this.keyboardlineentry != (UnityEngine.Object) null) || !((UnityEngine.Object) this.keyboardGrid != (UnityEngine.Object) null))
      return;
    foreach (Component component in this.keyboardGrid.transform)
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
    this.keyboardGrid.GetComponent<RectTransform>().ForceUpdateRectTransforms();
    GameObject gameObject1 = UnityEngine.Object.Instantiate<GameObject>(this.keyboardlineentry, this.keyboardGrid.transform);
    gameObject1.transform.Find("InputField/Label").GetComponent<Text>().text = "Slow Turning";
    gameObject1.transform.Find("InputField/Label").GetComponent<Text>().fontSize = gameObject1.transform.Find("InputField/Label").GetComponent<Text>().fontSize - 5;
    gameObject1.transform.Find("InputField/Text").GetComponent<Text>().fontSize = gameObject1.transform.Find("InputField/Text").GetComponent<Text>().fontSize - 10;
    gameObject1.transform.Find("InputField").GetComponent<InputField>().characterLimit = 10;
    gameObject1.transform.Find("InputField").GetComponent<InputField>().text = "SHIFT";
    gameObject1.transform.Find("InputField").GetComponent<InputField>().interactable = false;
    gameObject1.name = "SHIFT";
    GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.keyboardlineentry, this.keyboardGrid.transform);
    gameObject2.transform.Find("InputField/Label").GetComponent<Text>().text = "Switch Camera";
    gameObject2.transform.Find("InputField/Label").GetComponent<Text>().fontSize = gameObject2.transform.Find("InputField/Label").GetComponent<Text>().fontSize - 5;
    gameObject2.transform.Find("InputField/Text").GetComponent<Text>().fontSize = gameObject2.transform.Find("InputField/Text").GetComponent<Text>().fontSize - 10;
    gameObject2.transform.Find("InputField").GetComponent<InputField>().characterLimit = 10;
    gameObject2.transform.Find("InputField").GetComponent<InputField>().text = "SPACE";
    gameObject2.transform.Find("InputField").GetComponent<InputField>().interactable = false;
    gameObject2.name = "SPACE";
    GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.keyboardlineentry, this.keyboardGrid.transform);
    gameObject3.transform.Find("InputField/Label").GetComponent<Text>().text = "Toggle Names";
    gameObject3.transform.Find("InputField/Label").GetComponent<Text>().fontSize = gameObject3.transform.Find("InputField/Label").GetComponent<Text>().fontSize - 5;
    gameObject3.transform.Find("InputField/Text").GetComponent<Text>().fontSize = gameObject3.transform.Find("InputField/Text").GetComponent<Text>().fontSize - 10;
    gameObject3.transform.Find("InputField").GetComponent<InputField>().characterLimit = 10;
    gameObject3.transform.Find("InputField").GetComponent<InputField>().text = "!";
    gameObject3.transform.Find("InputField").GetComponent<InputField>().interactable = false;
    gameObject3.name = "EXCLAMATION";
    GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(this.keyboardlineentry, this.keyboardGrid.transform);
    gameObject4.transform.Find("InputField/Label").GetComponent<Text>().text = "Toggle Details";
    gameObject4.transform.Find("InputField/Label").GetComponent<Text>().fontSize = gameObject4.transform.Find("InputField/Label").GetComponent<Text>().fontSize - 5;
    gameObject4.transform.Find("InputField/Text").GetComponent<Text>().fontSize = gameObject4.transform.Find("InputField/Text").GetComponent<Text>().fontSize - 10;
    gameObject4.transform.Find("InputField").GetComponent<InputField>().characterLimit = 10;
    gameObject4.transform.Find("InputField").GetComponent<InputField>().text = "@";
    gameObject4.transform.Find("InputField").GetComponent<InputField>().interactable = false;
    gameObject4.name = "AT";
    foreach (string key in (IEnumerable<string>) GLOBALS.KeyboardMap.Keys)
    {
      keyinfo keyboard = GLOBALS.KeyboardMap[key];
      GameObject gameObject5 = UnityEngine.Object.Instantiate<GameObject>(this.keyboardlineentry, this.keyboardGrid.transform);
      gameObject5.transform.Find("InputField/Label").GetComponent<Text>().text = keyboard.details;
      gameObject5.transform.Find("InputField").GetComponent<InputField>().text = keyboard.key;
      gameObject5.name = key;
    }
    this.keyboardGrid.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0.0f, -500f, 0.0f);
  }

  public void PopulateJoystickGrid()
  {
    if (!((UnityEngine.Object) this.joysticklineentry != (UnityEngine.Object) null) || !((UnityEngine.Object) this.joystickGrid != (UnityEngine.Object) null))
      return;
    foreach (Component component in this.joystickGrid.transform)
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
    this.joystickGrid.GetComponent<RectTransform>().ForceUpdateRectTransforms();
    foreach (string key in (IEnumerable<string>) GLOBALS.JoystickMap.Keys)
    {
      JoystickRawInfo joystick = GLOBALS.JoystickMap[key];
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.joysticklineentry, this.joystickGrid.transform);
      gameObject.GetComponent<JoystickInfo>().joydata = joystick;
      gameObject.GetComponent<JoystickInfo>().UpdateGUI();
      gameObject.name = key;
    }
    this.joystickGrid.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0.0f, -500f, 0.0f);
  }

  public void SetJoystickToDefaults()
  {
    GLOBALS.JoystickMap.Clear();
    GLOBALS.JoystickMap = (IDictionary<string, JoystickRawInfo>) new Dictionary<string, JoystickRawInfo>()
    {
      {
        "Jcontrolls_turn",
        new JoystickRawInfo("Turn", axis_in: 4)
      },
      {
        "Jcontrolls_move_lr",
        new JoystickRawInfo("Strafe")
      },
      {
        "Jcontrolls_move_ud",
        new JoystickRawInfo("Forward/Back", axis_in: 2)
      },
      {
        "Jcontrolls_right_y",
        new JoystickRawInfo("Right Stick Y", axis_in: 5)
      },
      {
        "Jcontrolls_dpad_lr",
        new JoystickRawInfo("D-Pad Left/Right", axis_in: 6)
      },
      {
        "Jcontrolls_dpad_ud",
        new JoystickRawInfo("D-Pad Up/Down", axis_in: 7, button_rd_num_in: 0, leftup_value_in: 1f, rightdown_value_in: -1f)
      },
      {
        "Jcontrolls_A",
        new JoystickRawInfo("Button A", isButton_in: true, axis_in: 0)
      },
      {
        "Jcontrolls_B",
        new JoystickRawInfo("Button B", isButton_in: true, axis_in: 0, button_lu_num_in: 1)
      },
      {
        "Jcontrolls_X",
        new JoystickRawInfo("Button X", isButton_in: true, axis_in: 0, button_lu_num_in: 2)
      },
      {
        "Jcontrolls_Y",
        new JoystickRawInfo("Button Y", isButton_in: true, axis_in: 0, button_lu_num_in: 3)
      },
      {
        "Jcontrolls_LB",
        new JoystickRawInfo("Slow Turn L", isButton_in: true, axis_in: 0, button_lu_num_in: 4)
      },
      {
        "Jcontrolls_RB",
        new JoystickRawInfo("Slow Turn R", isButton_in: true, axis_in: 0, button_lu_num_in: 5)
      },
      {
        "Jcontrolls_LTR",
        new JoystickRawInfo("Left Trigger", axis_in: 3, button_rd_num_in: 0, leftup_value_in: 1f, rightdown_value_in: -1f)
      },
      {
        "Jcontrolls_RTR",
        new JoystickRawInfo("Right Trigger", axis_in: 3)
      },
      {
        "Jcontrolls_stop",
        new JoystickRawInfo("Stop Timer", isButton_in: true, axis_in: 0, button_lu_num_in: 7)
      },
      {
        "Jcontrolls_restart",
        new JoystickRawInfo("Start Timer", isButton_in: true, axis_in: 0, button_lu_num_in: 6)
      },
      {
        "Jcontrolls_camera",
        new JoystickRawInfo("Switch Camera", isButton_in: true, axis_in: 0, button_lu_num_in: 8)
      },
      {
        "Jcontrolls_reset",
        new JoystickRawInfo("Restart Position", isButton_in: true, axis_in: 0, button_lu_num_in: 9)
      }
    };
  }

  public void OnRobotSkinsUp()
  {
    ++this.curr_robotskins;
    this.OnMenuChanged();
  }

  public void OnRobotSkinsDown()
  {
    --this.curr_robotskins;
    this.OnMenuChanged();
  }

  public bool SaveStateValidator()
  {
    string text = this.save_name.text;
    if (text.Length < 1)
    {
      this.save_name_status.text = "Enter name...";
      return false;
    }
    string upper = text.ToUpper();
    if (upper == "DEFAULT")
    {
      this.save_name_status.text = "Can't override default state!";
      return false;
    }
    this.save_name_status.text = !GLOBALS.all_robot_capital_names.Contains(upper) ? "State name Ok." : "Set " + upper + " Robot default state.";
    return true;
  }

  public void OnSaveStateButton() => this.SaveStateValidator();

  public void OnSaveButton(bool joystick)
  {
    this.savestring_go.SetActive(true);
    this.savestate_joystick = joystick;
  }

  public void OnDeleteState(bool joystick)
  {
    Dropdown dropdown = joystick ? this.joystick_state_dropdown : this.keyboard_state_dropdown;
    Dictionary<string, string> dictionary = joystick ? GLOBALS.joystick_states : GLOBALS.keyboard_states;
    string text = dropdown.options[dropdown.value].text;
    if (text == "Default")
      return;
    dictionary.Remove(text);
    dropdown.value = 1;
    if (joystick)
    {
      this.UpdateJoystickList();
      Settings.SaveJoystickStates();
    }
    else
    {
      this.UpdateKeyboardList();
      Settings.SaveKeyboardStates();
    }
    PlayerPrefs.Save();
  }

  public void OnSaveState()
  {
    if (!this.SaveStateValidator())
      return;
    if (this.savestate_joystick)
      this.SaveJoystickState(this.save_name.text);
    else
      this.SaveKeyboardState(this.save_name.text);
    this.savestring_go.SetActive(false);
  }

  public void OnLoadState(bool joystick)
  {
    if (joystick)
    {
      if (this.joystick_state_dropdown.value >= this.joystick_state_dropdown.options.Count)
        this.joystick_state_dropdown.value = this.joystick_state_dropdown.options.Count - 1;
      string text = this.joystick_state_dropdown.options[this.joystick_state_dropdown.value].text;
      if (text == "Default")
        this.ResetJoystickToDefaults();
      else
        this.LoadJoystickState(text);
    }
    else
    {
      if (this.keyboard_state_dropdown.value >= this.keyboard_state_dropdown.options.Count)
        this.keyboard_state_dropdown.value = this.keyboard_state_dropdown.options.Count - 1;
      string text = this.keyboard_state_dropdown.options[this.keyboard_state_dropdown.value].text;
      if (text == "Default")
        this.ResetKeyboardToDefaults();
      else
        this.LoadKeyboardState(text);
    }
  }

  public void SaveJoystickState(string name)
  {
    string str = "";
    name = name.ToUpper();
    foreach (string key in (IEnumerable<string>) GLOBALS.JoystickMap.Keys)
      str = str + "||" + key + "||" + GLOBALS.JoystickMap[key].GetString();
    GLOBALS.joystick_states[name] = str;
    Settings.SaveJoystickStates();
    this.UpdateJoystickList();
    PlayerPrefs.Save();
    this.joystick_state_dropdown.value = this.GetOptionsIndex(name, true);
  }

  public int GetOptionsIndex(string value, bool joystick)
  {
    Dropdown dropdown = joystick ? this.joystick_state_dropdown : this.keyboard_state_dropdown;
    for (int index = 0; index < dropdown.options.Count; ++index)
    {
      if (dropdown.options[index].text == value)
        return index;
    }
    return 0;
  }

  public void SaveKeyboardState(string name)
  {
    string str = "";
    name = name.ToUpper();
    foreach (string key in (IEnumerable<string>) GLOBALS.KeyboardMap.Keys)
      str = str + "||" + key + "||" + GLOBALS.KeyboardMap[key].GetString();
    GLOBALS.keyboard_states[name] = str;
    Settings.SaveKeyboardStates();
    this.UpdateKeyboardList();
    PlayerPrefs.Save();
    this.keyboard_state_dropdown.value = this.GetOptionsIndex(name, false);
  }

  public static void SaveJoystickStates()
  {
    string str = "";
    foreach (string key in GLOBALS.joystick_states.Keys)
      str = str + "~~" + key + "~~" + GLOBALS.joystick_states[key];
    PlayerPrefs.SetString("JOY_STATES", str);
  }

  public static void SaveKeyboardStates()
  {
    string str = "";
    foreach (string key in GLOBALS.keyboard_states.Keys)
      str = str + "~~" + key + "~~" + GLOBALS.keyboard_states[key];
    PlayerPrefs.SetString("KEY_STATES", str);
  }

  public bool LoadJoystickState(string key)
  {
    key = key.ToUpper();
    if (!GLOBALS.joystick_states.ContainsKey(key))
      return false;
    string[] separator = new string[1]{ "||" };
    string[] strArray = GLOBALS.joystick_states[key].Split(separator, StringSplitOptions.None);
    if (strArray.Length < 3)
      return false;
    for (int index = 1; index < strArray.Length - 1; index += 2)
    {
      if (GLOBALS.JoystickMap.ContainsKey(strArray[index]))
        GLOBALS.JoystickMap[strArray[index]].FromString(strArray[index + 1]);
      else
        Debug.Log((object) ("Joystick Map did not have key " + strArray[index]));
    }
    this.PopulateJoystickGrid();
    return true;
  }

  public bool LoadKeyboardState(string key)
  {
    key = key.ToUpper();
    if (!GLOBALS.keyboard_states.ContainsKey(key))
      return false;
    string[] separator = new string[1]{ "||" };
    string[] strArray = GLOBALS.keyboard_states[key].Split(separator, StringSplitOptions.None);
    if (strArray.Length < 3)
      return false;
    for (int index = 1; index < strArray.Length - 1; index += 2)
    {
      if (GLOBALS.KeyboardMap.ContainsKey(strArray[index]))
        GLOBALS.KeyboardMap[strArray[index]].FromString(strArray[index + 1]);
      else
        Debug.Log((object) ("Keyboard Map did not have key " + strArray[index]));
    }
    this.PopulateKeyboardGrid();
    return true;
  }

  public bool LoadJoystickStates()
  {
    if (!PlayerPrefs.HasKey("JOY_STATES"))
      return false;
    string[] separator = new string[1]{ "~~" };
    string[] strArray = PlayerPrefs.GetString("JOY_STATES").Split(separator, StringSplitOptions.None);
    if (strArray.Length < 2)
      return false;
    GLOBALS.joystick_states.Clear();
    for (int index = 1; index < strArray.Length - 1; index += 2)
      GLOBALS.joystick_states[strArray[index]] = strArray[index + 1];
    this.UpdateJoystickList();
    return true;
  }

  public bool LoadKeyboardStates()
  {
    if (!PlayerPrefs.HasKey("KEY_STATES"))
      return false;
    string[] separator = new string[1]{ "~~" };
    string[] strArray = PlayerPrefs.GetString("KEY_STATES").Split(separator, StringSplitOptions.None);
    if (strArray.Length < 2)
      return false;
    GLOBALS.keyboard_states.Clear();
    for (int index = 1; index < strArray.Length - 1; index += 2)
      GLOBALS.keyboard_states[strArray[index]] = strArray[index + 1];
    this.UpdateKeyboardList();
    return true;
  }

  public void UpdateJoystickList()
  {
    this.joystick_state_dropdown.ClearOptions();
    List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
    options.Add(new Dropdown.OptionData("Default"));
    foreach (string key in GLOBALS.joystick_states.Keys)
      options.Add(new Dropdown.OptionData(key));
    this.joystick_state_dropdown.AddOptions(options);
    if (this.joystick_state_dropdown.value >= this.joystick_state_dropdown.options.Count)
      this.joystick_state_dropdown.value = this.joystick_state_dropdown.options.Count - 1;
    this.joystick_state_dropdown.RefreshShownValue();
  }

  public void UpdateKeyboardList()
  {
    this.keyboard_state_dropdown.ClearOptions();
    List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
    options.Add(new Dropdown.OptionData("Default"));
    foreach (string key in GLOBALS.keyboard_states.Keys)
      options.Add(new Dropdown.OptionData(key));
    this.keyboard_state_dropdown.AddOptions(options);
    if (this.keyboard_state_dropdown.value >= this.keyboard_state_dropdown.options.Count)
      this.keyboard_state_dropdown.value = this.keyboard_state_dropdown.options.Count - 1;
    this.keyboard_state_dropdown.RefreshShownValue();
  }
}
