using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Item : Chip{
    public int Price;
    private bool _wasMousePressed = false;
    public Item(Texture2D texture) : base(texture)
    {
        
    }
    public virtual void OnClickBuy(){
        Console.WriteLine($"Item purchased for {Price}!");
    }
    public override void Reset()
    {
        ResetChipTexture();
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, Position, Viewport, Color.White);
        base.Draw(spriteBatch);
    }
    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
    }
}