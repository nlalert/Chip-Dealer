using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Singleton
{
    public const int SCREEN_WIDTH = 640;
    public const int SCREEN_HEIGHT = 480;

    public const int PLAY_AREA_WIDTH = 8;
    public const int PLAY_AREA_HEIGHT = 10;
    public const int BUBBLE_SIZE = 32;
    public const int PLAY_AREA_START_X = (SCREEN_WIDTH - (PLAY_AREA_WIDTH * BUBBLE_SIZE)) / 2;
    public const int PLAY_AREA_END_X = PLAY_AREA_START_X + (PLAY_AREA_WIDTH * BUBBLE_SIZE);
    public const int PlayAreaEndY = 0;
    public const float MAX_PLAYER_ROTATION = (float)(80 * (Math.PI / 180)); //80 Degree

    public enum BubbleType
    {
        None, Red, Green, Blue
    }

    public BubbleType[,] GameBoard;

    public Random Random;

    public KeyboardState PreviousKey, CurrentKey;

    private static Singleton instance;

    private Singleton() { }

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
}

