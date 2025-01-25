using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

class Item : Chip{
    
    public Item(Texture2D texture) : base(texture)
    {

    }
    public virtual void OnClickBuy(){
        
    }
    public override void Reset()
    {
        ResetChipTexture();
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
    }
}