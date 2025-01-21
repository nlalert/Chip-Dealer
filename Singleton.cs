using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Singleton
{
    public const int SCREENWIDTH = 640;
    public const int SCREENHEIGHT = 480;

    public const int PLAYAREAWIDTH = 8;
    public const int PLAYAREAHEIGHT = 10;
    public const int BUBBLESIZE = 32;
    public const int PlayAreaStartX = (SCREENWIDTH - (PLAYAREAWIDTH * BUBBLESIZE)) / 2;
    public const int PlayAreaEndX = PlayAreaStartX + (PLAYAREAWIDTH * BUBBLESIZE);
    public const int PlayAreaEndY = 0;

    public enum BubbleType
    {
        Red, Green, Blue
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

