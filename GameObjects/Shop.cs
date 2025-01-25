using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

class Shop : GameObject
{   
    Dictionary<Item, int> _shopItems;

    public Shop(Texture2D texture) : base(texture)
    {
        _shopItems = new Dictionary<Item, int>();
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        //Display Shop
        spriteBatch.Draw(_texture,new Vector2(Singleton.SCREEN_WIDTH *3/4 ,30),Color.White);
        base.Draw(spriteBatch);
    }

    public override void Reset()
    {
        Console.WriteLine("Shop Reset");
        _shopItems.Add(new RedChip(_texture),50);
        base.Reset();
    }


    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {

        base.Update(gameTime, gameObjects);
    }

    

}
