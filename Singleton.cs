using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;


class Singleton
{
    public const int SCREEN_WIDTH = 640;
    public const int SCREEN_HEIGHT = 480;

    public const int CHIP_GRID_WIDTH = 8;
    public const int CHIP_GRID_HEIGHT = 12;
    public const int CHIP_SHOOTING_HEIGHT = 430;
    public const int CHIP_SIZE = 32;
    public const int CHIP_SHADOW_HEIGHT = 3;
    public const int PLAY_AREA_START_X = (SCREEN_WIDTH - (CHIP_GRID_WIDTH * CHIP_SIZE)) / 2;
    public const int PLAY_AREA_END_X = PLAY_AREA_START_X + (CHIP_GRID_WIDTH * CHIP_SIZE);
    public const float MAX_PLAYER_ROTATION = (float)(80 * (Math.PI / 180)); //80 Degree

    public const int CHIP_BREAK_AMOUNT = 3;
    public const int CEILING_WAITING_TURN = 8;

    public int CeilingPosition;
    public int ChipShotAmount;
    public int Stage;
    public int Score;

    public GameBoard GameBoard;

    public ChipType CurrentChip;
    public ChipType NextChip;
    
    public Random Random;

    public enum GameState
    {
        MainMenu,
        StartingGame,
        SetLevel,
        Playing,
        CheckChipAndCeiling,
        Pause,
        PassingLevel,
        GameOver,
        Exit,
    }

    public GameState CurrentGameState;

    public KeyboardState PreviousKey, CurrentKey;
    
    public MouseState PreviousMouseState,CurrentMouseState;
    private static Singleton instance;
    public float Volume = 1.0f;
    
    private Singleton() { 
        Random = new Random();
    }

    public static Singleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Singleton();
            }
            return instance;
        }
    }

    public static Rectangle GetChipViewPort(ChipType chipType)
    {
        switch (chipType)
        {
            case ChipType.Explosive:
                return new Rectangle(0, 48, CHIP_SIZE, CHIP_SIZE + CHIP_SHADOW_HEIGHT);
            default:
                return new Rectangle(((int)chipType - 1) * CHIP_SIZE, 0, CHIP_SIZE, CHIP_SIZE + CHIP_SHADOW_HEIGHT);
        }
    }

    //score
    public class ScoreEntry
    {
        public string Timestamp { get; set; }
        public int Score { get; set; }
    }
    private static string ScoreFilePath => "scores.json";

    public static void SaveScore()
    {
        List<ScoreEntry> scores = LoadScores();
        Console.WriteLine("Saving score ");
        scores.Add(new ScoreEntry
        {
            Score = Instance.Score,
            Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        });
        string json = JsonSerializer.Serialize(scores, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(ScoreFilePath, json);
    }

    public static List<ScoreEntry> LoadScores()
    {
        if (!File.Exists(ScoreFilePath)) 
            return new List<ScoreEntry>();
        else{
            Console.WriteLine("Found Scores");
        }
        string json = File.ReadAllText(ScoreFilePath);
        return JsonSerializer.Deserialize<List<ScoreEntry>>(json) ?? new List<ScoreEntry>();
    }

    public static Rectangle GetViewPortFromSpriteSheet(String name){

        Dictionary<string, Rectangle> _spriteRects = new Dictionary<string, Rectangle>();

        // Normal Chips
        _spriteRects.Add("Red_Chip",new Rectangle( 0, 0, 32, 35));
        _spriteRects.Add("Blue_Chip",new Rectangle( 32, 0, 32, 35));
        _spriteRects.Add("Green_Chip",new Rectangle( 64, 0, 32, 35));
        _spriteRects.Add("Yellow_Chip",new Rectangle( 96, 0, 32, 35));
        _spriteRects.Add("Purple_Chip",new Rectangle( 128, 0, 32, 35));
        _spriteRects.Add("White_Chip",new Rectangle( 160, 0, 32, 35));
        _spriteRects.Add("Black_Chip",new Rectangle( 192, 0, 32, 35));
        _spriteRects.Add("Orange_Chip",new Rectangle( 224, 0, 32, 35));

        // Specaial Chips
        _spriteRects.Add("Explosive_Chip0",new Rectangle( 0, 48, 32, 35));
        _spriteRects.Add("Explosive_Chip1",new Rectangle( 32, 48, 32, 35));
        _spriteRects.Add("Unknown_Chip",new Rectangle( 64, 48, 32, 35));

        // Background
        _spriteRects.Add("PlayArea", new Rectangle(288,224,384,488));
        _spriteRects.Add("PlayArea_Ingame", new Rectangle(0,224,288,488));

        // Menu UI
        _spriteRects.Add("Game_Title",new Rectangle( 640, 720, 352, 80));//TODO

        _spriteRects.Add("Menu_Button",new Rectangle( 640, 720, 352, 80));
        _spriteRects.Add("Menu_Button_Highlighted",new Rectangle( 640, 816, 352, 80));

        _spriteRects.Add("Start_Label",new Rectangle( 640, 912, 176, 48));
        _spriteRects.Add("Start_Label_Highlighted",new Rectangle( 816, 912, 176, 48));

        _spriteRects.Add("ScoreBoard_Label",new Rectangle( 608, 1360, 336, 48));
        _spriteRects.Add("ScoreBoard_Label_Highlighted",new Rectangle( 608, 1424, 336, 48));

        _spriteRects.Add("Exit_Label",new Rectangle( 0, 1488, 304, 48));
        _spriteRects.Add("Exit_Label_Highlighted",new Rectangle( 304, 1488, 304, 48));


        _spriteRects.Add("Back_Button",new Rectangle(608, 1136, 160 ,48));
        _spriteRects.Add("Back_Button_Highlighted",new Rectangle(608, 1200, 160 ,48));

        _spriteRects.Add("Back_Label",new Rectangle( 608, 1264, 160, 32));
        _spriteRects.Add("Back_Label_Highlighted",new Rectangle( 608, 1312, 160, 32));

        // In Game UI
        _spriteRects.Add("Next_Chip_Box",new Rectangle( 112, 144, 64, 64));
        _spriteRects.Add("Next_Chip_Label",new Rectangle( 176, 144, 64, 16));

        _spriteRects.Add("Player_Hand",new Rectangle( 0, 144, 96, 80));

        _spriteRects.Add("Shop",new Rectangle( 640, 912, 176, 48));//TODO

        // Slot-Mechine UI

        _spriteRects.Add("Slot_Label",new Rectangle(672, 224, 160, 160));

        _spriteRects.Add("Slot_Machine",new Rectangle(672, 400, 160, 160));
        _spriteRects.Add("Slot_Handle",new Rectangle(672, 576, 48, 16));
        _spriteRects.Add("Slot_Background",new Rectangle(832, 400, 48, 147));

        _spriteRects.Add("Slot_Drawing",new Rectangle(672, 608, 112, 96));

        // GameOver UI
        _spriteRects.Add("GameOver_Title",new Rectangle( 0, 720, 320, 256));

        // Pause UI
        _spriteRects.Add("Pause_Title0",new Rectangle(0, 992, 384, 128));
        _spriteRects.Add("Pause_Title1",new Rectangle(384, 992, 384, 128));

        _spriteRects.Add("Pause_Button",new Rectangle(0, 1136, 304, 80));
        _spriteRects.Add("Pause_Button_Highlighted",new Rectangle(304, 1136, 304, 80));

        _spriteRects.Add("Resume_Label",new Rectangle(0, 1296, 304, 48));
        _spriteRects.Add("Resume_Label_Highlighted",new Rectangle(304, 1296, 304, 48));

        _spriteRects.Add("Restart_Label",new Rectangle(0, 1360, 304, 48));
        _spriteRects.Add("Restart_Label_Highlighted",new Rectangle(304, 1360, 304, 48));

        _spriteRects.Add("Settings_Label",new Rectangle(0, 1232, 304, 48));
        _spriteRects.Add("Settings_Label_Highlighted",new Rectangle(304, 1232, 304, 48));

        _spriteRects.Add("Mainmenu_Label",new Rectangle(0, 1424, 304, 48));
        _spriteRects.Add("Mainmenu_Label_Highlighted",new Rectangle(304, 1424, 304, 48));

        // Settings UI
        _spriteRects.Add("Settings_Box0",new Rectangle(0, 1552, 448, 352));
        _spriteRects.Add("Settings_Box1",new Rectangle(448, 1552, 448, 352));

        _spriteRects.Add("Music_Label",new Rectangle(448, 1984, 176, 32));
        _spriteRects.Add("SFX_Label",new Rectangle(624, 1984, 112, 32));

        _spriteRects.Add("Slide_Bar",new Rectangle(0, 1920, 448, 80));

        _spriteRects.Add("Slide_Chip0",new Rectangle(448, 1920, 32, 35));
        _spriteRects.Add("Slide_Chip1",new Rectangle(480, 1920, 32, 35));
        _spriteRects.Add("Slide_Chip2",new Rectangle(512, 1920, 32, 35));
        _spriteRects.Add("Slide_Chip3",new Rectangle(544, 1920, 32, 35));
        
        return _spriteRects[name];
    }
}

