using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
class GameStat : GameObject
{
    public GameStat(Texture2D texture) : base(texture)
    {
        
    }
    public override void Reset()
    {
        
    }

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, new Vector2(Position.X - Singleton.GetViewPortFromSpriteSheet("Score_Label0").Width/2, Position.Y), Singleton.GetViewPortFromSpriteSheet("Score_Label0"), Color.White);
        spriteBatch.Draw(_texture, new Vector2(Position.X - Singleton.GetViewPortFromSpriteSheet("Score_Box0").Width/2, Position.Y + 16*2), Singleton.GetViewPortFromSpriteSheet("Score_Box0"), Color.White);

        spriteBatch.Draw(_texture, new Vector2(Position.X - (Singleton.GetViewPortFromSpriteSheet("Money_Box").Width + Singleton.GetViewPortFromSpriteSheet("Ceiling_Timer_Box").Width + 16)/2 , Position.Y + 16*5), 
        Singleton.GetViewPortFromSpriteSheet("Money_Box"), Color.White);
        spriteBatch.Draw(_texture, new Vector2(Position.X + (Singleton.GetViewPortFromSpriteSheet("Money_Box").Width - Singleton.GetViewPortFromSpriteSheet("Ceiling_Timer_Box").Width + 16)/2 , Position.Y + 16*5),  
        Singleton.GetViewPortFromSpriteSheet("Ceiling_Timer_Box"), Color.White);

        spriteBatch.Draw(_texture, new Vector2(Position.X - Singleton.GetViewPortFromSpriteSheet("Relic_Box").Width/2, Position.Y + 16*8), Singleton.GetViewPortFromSpriteSheet("Relic_Box"), Color.White);

        spriteBatch.Draw(_texture, new Vector2(Position.X  - Singleton.GetViewPortFromSpriteSheet("Next_Chip_Label").Width/2 , Position.Y + 16*18),Singleton.GetViewPortFromSpriteSheet("Next_Chip_Label"),Color.White);
        spriteBatch.Draw(_texture, new Vector2(Position.X  - Singleton.GetViewPortFromSpriteSheet("Next_Chip_Box").Width/2 , Position.Y + 16*20),Singleton.GetViewPortFromSpriteSheet("Next_Chip_Box"),Color.White);
    }
}
