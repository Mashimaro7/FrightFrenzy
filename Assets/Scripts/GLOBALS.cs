// Decompiled with JetBrains decompiler
// Type: GLOBALS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Windows;

public static class GLOBALS
{
  public static bool FORCE_REAL = false;
  public static bool FORCE_OLD_BHELP = false;
  public static string VERSION = "v7.0a";
  public static bool HEADLESS_MODE = false;
  public static bool CLIENT_MODE = false;
  public static ClientLow topclient = (ClientLow) null;
  public static bool SINGLEPLAYER_MODE = false;
  public static SinglePlayer topsingleplayer = (SinglePlayer) null;
  public static bool SERVER_MODE = false;
  public static ServerLow topserver = (ServerLow) null;
  public static bool SERVER_DEBUG = false;
  public static int NETSTATS = 0;
  public static bool AUDIO = true;
  public static bool ROBOTAUDIO = true;
  public static float VOLUME = 0.5f;
  public static bool ENABLE_LOGS = true;
  public static string LOGS_PATH = "." + Path.DirectorySeparatorChar.ToString() + "logs";
  public static bool USE_STANDARD_LOG = true;
  public static bool ENABLE_UDP_STATS = false;
  public static bool UDP_LOGGING = false;
  public static bool OUTPUT_SCORING_FILES = false;
  public static ConsoleWindow win_console = (ConsoleWindow) null;
  public static int UDP_PORT = 1446;
  public const string PASSCODE = "7p0";
  public const string GETSERVERINFO = "11115";
  public const string PACKETTYPE_ZIP = "Z";
  public const string PACKETTYPE_TXT = "T";
  public const string HEADER_PLAYERS = "PLYRS";
  public const string HEADER_NEWPLAYER = "NEW";
  public const string HEADER_PLAYER_POS = "4.5";
  public const string HEADER_FIELDELEMENTS = "3.2";
  public const string HEADER_ERROR = "ERR";
  public const string HEADER_FLAGS = "FLG";
  public const int PACKET_COMPRESSION = 1;
  public const bool SHOW_COMPRESSION_TESTCASE = false;
  public const int MAX_TRACKED_PACKETS = 30;
  public const long PACKET_LOSS_UPDATE_TIME = 500;
  public const long FLAG_UPDATE_TIME = 100;
  public static bool now_recording = false;
  public static bool now_playing = false;
  public static bool now_paused = false;
  public static float playback_speed = 1f;
  public static int PB_BUFFER_DURATION = 5;
  public static bool autosave_recordings = false;
  public static string autosave_filename = "";
  public const char SEPARATOR1 = '\u0011';
  public const char SEPARATOR2 = '\u0012';
  public const char SEPARATOR3 = '\u0013';
  public const char SEPARATOR4 = '\u0014';
  public const char SEPARATOR5 = '\u0015';
  public const char DATAFILE_SEPARATOR1 = '\u0016';
  public const char DATAFILE_SEPARATOR2 = '\u0017';
  public const char DATAFILE_SEPARATOR3 = '\u0018';
  public const string HEADER_IN_NEWPLAYER = "NAMEIS";
  public const string HEADER_IN_INPUTS = "MYINPUTS";
  public static float time_after_data_received = 0.0f;
  public const string HTTP_ADDRESS = "http://xrcsimulator.org/game/getserverlist.php";
  public const string HTTP_REGISTRATION = "http://xrcsimulator.org/game/registerserver.php";
  public const long registration_update_rate = 30000;
  public const string HTTP_LICENSE_ACTIVATE = "https://xrcsimulator.org/wp-json/lmfwc/v2/licenses/activate/";
  public const string HTTP_LICENSE_VALIDATE = "https://xrcsimulator.org/wp-json/lmfwc/v2/licenses/validate/";
  public const string HTTP_LICENSE_KEY = "ck_9baaa4b34b6149c8b576fd7ddbdd5f971f6a21b8";
  public const string HTTP_LICENSE_SECRET = "cs_ca7dbf4b6b9c215f9276f1f746dbe568a9e71e59";
  public static List<LicenseData> myLicenses = new List<LicenseData>();
  public static long SERVER_SEND_UPDATE_DELAY = 50;
  public static long CLIENT_SEND_UPDATE_DELAY = 20;
  public static long CLIENT_CONNECT_RETRY_TIME = 2000;
  public static long CLIENT_DISCONNECT_TIMEOUT = 3000;
  public static long SERVER_DISCONNECT_TIMEOUT = 3000;
  public static long SERVER_MESSAGE_COUNT_TIME = 1000;
  public static int MESSAGES_TO_KEEP = 6;
  public static long SERVER_CACHE_REFRESH = 500;
  public static bool INTERPOLATE = true;
  public static int CAMERA_AVERAGING = 1;
  public static long STATUS_FILES_UPDATE_PERIOD = 250;
  public static int CLIENT_ROBOT_INACTIVITY_TIMEOUT = 600;
  public static long UDP_MAX_BYTES_IN_MS = 3000;
  public static float UDP_DELAY_TIME_MS = 0.5f;
  public static long SERVER_MAX_UPDATE_DELAY = 250;
  public static string XRC_PRIVATEPUBLIC_KEY_XML = "<RSAKeyValue><Modulus>fe8zZC1JIoxuSexi7cQKZA/yA9NKCoP9Gt+OWO6WiYc33iHeoMSdSuD7oCqjT3VEkmMYfKTUJEXblqLMU4hDOw==</Modulus><Exponent>AQAB</Exponent><P>9fucrbEoJIIyzeB15ZE47lNA2MjVrHO9HieZ6DRoUzc=</P><Q>gxATPkPdf+5uEjf0pOqhtpvFFqo9C2qAQhQYJhr22h0=</Q><DP>uctTu4ntFS5Wa1SYGE7JXpH5kASaCAjflpA42sAC8J8=</DP><DQ>BVh1gHeiJCKkaKfRmZxcRidqTXdaEAoi+w74wS0eXl0=</DQ><InverseQ>r/vRnW84CE8vC95CCLDB+45wBlNchp78EgOXZgXUHpQ=</InverseQ><D>BdKHV7xYQ0am2rgZItELgfDSyaZ9J9tOWm23kRkG0LjkR6qJA7PCoaSRIUeaXj4p302xVgXxVLOTsHKs9lhH8Q==</D></RSAKeyValue>";
  public static string XRC_PUBLIC_KEY_XML = "<RSAKeyValue><Modulus>fe8zZC1JIoxuSexi7cQKZA/yA9NKCoP9Gt+OWO6WiYc33iHeoMSdSuD7oCqjT3VEkmMYfKTUJEXblqLMU4hDOw==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
  public static byte[] RANDOMIZER_SEED = Encoding.UTF8.GetBytes("BpczAbhmxBqfSidUqvdSncnuAvhmcfqt");
  public static string GAME = "Rapid React";
  public static int GAME_INDEX = 11;
  public static int framerate = 60;
  public static float worldscale = 2f;
  public static int PlayerCount = 4;
  public static int TIMER_TOTAL = 120;
  public static int TIMER_AUTO = 0;
  public static int TIMER_ENDGAME = 30;
  public static int game_option = 1;
  public static float OVERLAY_NAME_Y_OFFSET = 100f;
  public static float OVERLAY_NAME_Y_INCREMENT = 7f;
  public static string RobotModel = "RR_MiniDrone";
  public static string DriveTrain = "Mecanum";
  public static int DriveTrainIndex = 1;
  public static int RobotModelCategory = 10;
  public static int RobotModelIndex = 0;
  public static long MESSAGE_DISPLAY_PERIOD = 4000;
  public static float StrafeSpeedScale = 1f;
  public static float friction = 2f;
  public static float strafing_friction = 1.5f;
  public static float regen_breaking = 0.25f;
  public static int motortypeindex = 0;
  public static float gear_ratio = 20f;
  public static float wheel_diameter = 4f;
  public static float weight = 15f;
  public static float motor_count = 4f;
  public static float turning_scaler = 1f;
  public static bool fieldcentric = false;
  public static bool activebreaking = true;
  public static bool tankcontrol = false;
  public static float speed = 1f;
  public static float acceleration = 1f;
  public static bool I_AM_RED = false;
  public static bool I_AM_SPECTATOR = false;
  public static bool camera_follows = true;
  public static bool CAMERA_COUNTDOWN_CONTROL = false;
  public static string video_quality = "Beautiful";
  public static string video_resolution = "";
  public static bool video_fullscreen = false;
  public static Dictionary<string, string> GENERIC_DATA = new Dictionary<string, string>();
  public static IDictionary<string, keyinfo> KeyboardMap = (IDictionary<string, keyinfo>) new Dictionary<string, keyinfo>();
  public static bool keyboard_inuse = false;
  public static Dictionary<string, string> keyboard_states = new Dictionary<string, string>();
  public static IDictionary<string, JoystickRawInfo> JoystickMap = (IDictionary<string, JoystickRawInfo>) new Dictionary<string, JoystickRawInfo>();
  public static Dictionary<string, string> joystick_states = new Dictionary<string, string>();
  public static string default_player_name = "";
  public static bool settings_loaded = false;
  public static float SOUND_MIN_DISTANCE = 0.3f;
  public static float SOUND_MAX_DISTANCE = 10f;
  public static Dictionary<int, GameObject> skins_eyes = new Dictionary<int, GameObject>();
  public static Dictionary<int, GameObject> skins_hats = new Dictionary<int, GameObject>();
  public static Dictionary<int, GameObject> skins_spoilers = new Dictionary<int, GameObject>();
  public static Dictionary<int, GameObject> skins_other = new Dictionary<int, GameObject>();
  public const char SKIN_SEPERATOR = ':';
  public static string skins = "0";
  public static string robotskins = "";
  public static List<string> robotskinslist = new List<string>()
  {
    ""
  };
  public static List<string> all_robot_capital_names = new List<string>();
  public static int UDP_ALGORITHM = 2;
  public static int LAYER_RobotBoundry = 23;
  public static Dictionary<int, string> client_names = new Dictionary<int, string>();
  public static Dictionary<string, int> client_ids = new Dictionary<string, int>();
  public static IDictionary<string, string> ScorefilesDescription = (IDictionary<string, string>) new Dictionary<string, string>()
  {
    {
      "Timer",
      "Timer"
    },
    {
      "NetFPS",
      "Server Network FPS"
    },
    {
      "GameState",
      "Game State"
    },
    {
      "RedADJ",
      "Red Score Adjustment"
    },
    {
      "BlueADJ",
      "Blue Score Adjustment"
    },
    {
      "OPR",
      "Player Score Contribution"
    },
    {
      "ScoreR",
      "Red Score"
    },
    {
      "ScoreB",
      "Blue Score"
    },
    {
      "RBalls",
      "Red Balls Scored"
    },
    {
      "BBalls",
      "Blue Balls Scored"
    },
    {
      "AutoR",
      "Red Auto Score"
    },
    {
      "AutoB",
      "Blue Auto Score"
    },
    {
      "RRows",
      "Red Rows"
    },
    {
      "BRows",
      "Blue Rows"
    },
    {
      "PC_R",
      "Red Power Cells"
    },
    {
      "PC_B",
      "Blue Power Cells"
    },
    {
      "TeleR",
      "Red Teleop Score"
    },
    {
      "TeleB",
      "Blue Teleop Score"
    },
    {
      "EndR",
      "Red Endgame Score"
    },
    {
      "EndB",
      "Blue Endgame Score"
    },
    {
      "PenR",
      "Penalties Red"
    },
    {
      "PenB",
      "Penalties Blue"
    },
    {
      "BlueWP",
      "Blue Win Point"
    },
    {
      "RedWP",
      "Red Win Point"
    }
  };
  public static List<string> killingWords = new List<string>()
  {
    "kills",
    "executes",
    "slaughters",
    "murders",
    "finishes",
    "snuffs",
    "neutralizes",
    "massacres",
    "dispatches",
    "liquidates",
    "obliterates",
    "annihilates",
    "exterminates",
    "knocks out",
    "wipes out",
    "defeats",
    "destroys",
    "crushes",
    "eradicates",
    "shatters",
    "wrecks",
    "ends",
    "butchers",
    "creams",
    "decimates",
    "eliminates",
    "puts an end to",
    "rubs out",
    "dismembers"
  };
}
