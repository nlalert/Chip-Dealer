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
        Setting,
        PassingLevel,
        GameOver,
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
    public static Rectangle GetRectangleFromSpriteSheet(String name){
        Dictionary<string, Rectangle> _spriteRects = new Dictionary<string, Rectangle>();
        // Chips (Top row)
        _spriteRects.Add("ChipRed",new Rectangle( 0, 0, 32, 32));
        _spriteRects.Add("ChipBlue",new Rectangle( 32, 0, 32, 32));
        _spriteRects.Add("ChipGreen",new Rectangle( 64, 0, 32, 32));
        _spriteRects.Add("ChipPurple",new Rectangle( 96, 0, 32, 32));
        _spriteRects.Add("ChipGray",new Rectangle( 128, 0, 32, 32));
        _spriteRects.Add("ChipOrange",new Rectangle( 160, 0, 32, 32));
        _spriteRects.Add("BombChip",new Rectangle( 0, 40, 32, 32));
        _spriteRects.Add("SpikeChip",new Rectangle( 32, 40, 32, 32));
        _spriteRects.Add("IronChip",new Rectangle( 64, 40, 32, 32));
        _spriteRects.Add("GoldenChip",new Rectangle( 96, 40, 32, 32));

        _spriteRects.Add("PlayArea", new Rectangle(0,224,288,488));
        _spriteRects.Add("Button",new Rectangle( 640, 912, 176, 48));
        _spriteRects.Add("StartButton",new Rectangle( 640, 912, 176, 48));
        _spriteRects.Add("StartButtonHighlight",new Rectangle( 816, 912, 176, 48));
        _spriteRects.Add("GameName",new Rectangle( 640, 720, 352, 80));//TODO
        _spriteRects.Add("ScoreBoardButton",new Rectangle( 608, 1360, 336, 48));
        _spriteRects.Add("ScoreBoardButtonHighlight",new Rectangle( 608, 1424, 336, 48));
        _spriteRects.Add("BackButton",new Rectangle( 608, 1264, 160, 32));
        _spriteRects.Add("BackButtonHighlight",new Rectangle( 608, 1312, 160, 32));
        _spriteRects.Add("NextChipBox",new Rectangle( 112, 144, 64, 64));
        _spriteRects.Add("NextChipText",new Rectangle( 176, 144, 64, 16));
        _spriteRects.Add("GameOver",new Rectangle( 0, 720, 320, 256));
        _spriteRects.Add("Player",new Rectangle( 0, 144, 96, 80));
        _spriteRects.Add("Shop",new Rectangle( 640, 912, 176, 48));//TODO


        return _spriteRects[name];
    }
}

