using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
class BlueChip : Item{

    public BlueChip(Texture2D texture) : base(texture)
    {
        ChipType = ChipType.Blue;
    }
    public override void OnBuy(List<GameObject> gameObject)
    {
        base.OnBuy(gameObject);
    }
}