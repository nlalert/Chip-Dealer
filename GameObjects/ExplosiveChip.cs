using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
class ExplosiveChip : Chip
{
    public ExplosiveChip(Texture2D texture) : base(texture)
    {  
        ChipType = ChipType.Explosive;
    }
}
