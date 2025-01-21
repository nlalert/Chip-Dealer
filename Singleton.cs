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

    public const int PLAYAREAWIDTH =  32*8;
    public static int PlayAreaStartX => (SCREENWIDTH - PLAYAREAWIDTH) / 2;
    public static int PlayAreaEndX => PlayAreaStartX + PLAYAREAWIDTH;
    public static int PlayAreaEndY => 0;

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

