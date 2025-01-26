using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
class ExplosiveChip : Chip{

    public ExplosiveChip(Texture2D texture) : base(texture)
    {  
        ChipType = ChipType.Explosive;
    }
    public override void CheckAndDestroySameTypeChip(List<GameObject> gameObjects)
    {
        //find 6 surround chips
        Console.WriteLine("Bob do something");
        //get current pos on board
        //BoardCoord cur pos on board
        int X = (int)BoardCoord.X;
        int Y = (int)BoardCoord.Y;
        

    }
}