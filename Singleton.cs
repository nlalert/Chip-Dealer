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

    public const int CHIP_BREAK_AMOUNT = 3;
    public const int CEILING_WAITING_TURN = 8;

    public int CeilingPosition;
    public int ChipShotAmount;
    public int Stage;
    public int Score;
    public int Money;
    public bool waitForPlayer;

    public GameBoard GameBoard;

    public List<Relics.RelicType> OwnedRelics;
    public ChipType CurrentChip;
    public ChipType NextChip;
    
    public Random Random;

    public enum GameState
    {
        MainMenu,
        StartingGame,
        SetLevel,
        Playing,
        CheckCeiling,
        CheckGameBoard,
        Pause,
        PassingLevel,
        GameOver,
        GameWon,
        Exit,
    }

    public GameState CurrentGameState;
    public GameState PreviousGameState;

    public KeyboardState PreviousKey, CurrentKey;
    
    public MouseState PreviousMouseState,CurrentMouseState;
    private static Singleton instance;
    public float SFXVolume = 1.0f;
    public float MusicVolume =1.0f;
    
    private Singleton() { 
        Random = new Random();
        OwnedRelics = Relics.GetEmptyRelicList();
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

}

