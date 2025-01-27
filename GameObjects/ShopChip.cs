using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

public class ShopChip : Item
{
    public ChipType ChipType;

    public ShopChip(Texture2D texture) : base(texture)
    {
    }

    public override void OnBuy(List<GameObject> gameObjects)
    {
        Singleton.Instance.CurrentChip = ChipType;
        base.OnBuy(gameObjects);
    }
}