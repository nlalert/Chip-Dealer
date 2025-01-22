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

    public const int BUBBLE_GRID_WIDTH = 8;
    public const int BUBBLE_GRID_HEIGHT = 12;
    public const int BUBBLE_SIZE = 32;
    public const int PLAY_AREA_START_X = (SCREEN_WIDTH - (BUBBLE_GRID_WIDTH * BUBBLE_SIZE)) / 2;
    public const int PLAY_AREA_END_X = PLAY_AREA_START_X + (BUBBLE_GRID_WIDTH * BUBBLE_SIZE);
    public const int PLAY_AREA_END_Y = 0;
    public const float MAX_PLAYER_ROTATION = (float)(80 * (Math.PI / 180)); //80 Degree

    public BubbleType[,] GameBoard;

    public BubbleType CurrentBubble;
    public BubbleType NextBubble;
    
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
    public static Color GetBubbleColor(BubbleType bubbleType){
        switch (bubbleType)
        {
            case BubbleType.Red:
                return Color.Red;
            case BubbleType.Green:
                return Color.LimeGreen;
            case BubbleType.Blue:
                return Color.Blue;
            case BubbleType.Yellow:
                return Color.Yellow;
        }
        // if somehow not have color 
        return Color.White;
    }
}

