using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public int Score;

    public GameBoard GameBoard;

    public ChipType CurrentChip;
    public ChipType NextChip;
    
    public Random Random;

    public enum GameState
    {
        MainMenu,
        SetLevel,
        Playing,
        CheckChipAndCeiling,
        Pause,
        GameOver,
    }

    public GameState CurrentGameState;

    public KeyboardState PreviousKey, CurrentKey;

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
    
    public static Color GetChipColor(ChipType chipType){
        switch (chipType)
        {
            case ChipType.Red:
                return Color.Red;
            case ChipType.Green:
                return Color.LimeGreen;
            case ChipType.Blue:
                return Color.Blue;
            case ChipType.Yellow:
                return Color.Yellow;
        }
        // if somehow not have color 
        return Color.White;
    }
    public static Rectangle GetChipViewPort(ChipType chipType){

        int chipIndex = 0;

        switch (chipType)
        {
            case ChipType.Red: 
                chipIndex = 0;
                break;
            case ChipType.Blue: 
                chipIndex = 1;
                break;
            case ChipType.Green: 
                chipIndex = 2;
                break;
            case ChipType.Yellow: 
                chipIndex = 3;
                break;
            case ChipType.Explosive:
                chipIndex = 4;
                break;
            default:
                break;
        }

        return new Rectangle((chipIndex%4) * CHIP_SIZE, (chipIndex/4)*48, CHIP_SIZE, CHIP_SIZE + CHIP_SHADOW_HEIGHT);
    }
}

