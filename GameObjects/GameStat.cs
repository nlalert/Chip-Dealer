using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class GameStat : GameObject
{
    public SpriteFont font;
    public Vector2 fontSize;
    public Vector2 fontColor;
    public Rectangle _scoreLabelType;
    public Rectangle _scoreBoxType;

    public GameStat(Texture2D texture) : base(texture)
    {
        
    }
    public override void Reset()
    {
    }

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        
        if(Singleton.Instance.Score < 5000)
        {
            _scoreLabelType = Singleton.GetViewPortFromSpriteSheet("Score_Label0");
            _scoreBoxType = Singleton.GetViewPortFromSpriteSheet("Score_Box0");
        }
        else if(Singleton.Instance.Score < 10000)
        {
            _scoreLabelType = Singleton.GetViewPortFromSpriteSheet("Score_Label1");
            _scoreBoxType = Singleton.GetViewPortFromSpriteSheet("Score_Box1");
        }
        else
        {
            _scoreLabelType = Singleton.GetViewPortFromSpriteSheet("Score_Label2");
            _scoreBoxType = Singleton.GetViewPortFromSpriteSheet("Score_Box2");
        }
            
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, new Vector2(Position.X - Singleton.GetViewPortFromSpriteSheet("Score_Label0").Width/2, Position.Y), _scoreLabelType, Color.White);
        spriteBatch.Draw(_texture, new Vector2(Position.X - Singleton.GetViewPortFromSpriteSheet("Score_Box0").Width/2, Position.Y + 16*2), _scoreBoxType, Color.White);

        fontSize = font.MeasureString(Singleton.Instance.Score.ToString());
        spriteBatch.DrawString(font,Singleton.Instance.Score.ToString(),new Vector2(Position.X - fontSize.X/2, Position.Y + 16*2 + 10),Color.White);

        spriteBatch.Draw(_texture, new Vector2(Position.X - (Singleton.GetViewPortFromSpriteSheet("Stage_Label").Width + Singleton.GetViewPortFromSpriteSheet("Stage_Box").Width + 16)/2 , Position.Y + 16*5 + 8), 
        Singleton.GetViewPortFromSpriteSheet("Stage_Label"), Color.White);

        spriteBatch.Draw(_texture, new Vector2(Position.X + (Singleton.GetViewPortFromSpriteSheet("Stage_Label").Width - Singleton.GetViewPortFromSpriteSheet("Stage_Box").Width + 16)/2 , Position.Y + 16*5),  
        Singleton.GetViewPortFromSpriteSheet("Stage_Box"), Color.White);

        fontSize = font.MeasureString(Singleton.Instance.Stage.ToString());
        spriteBatch.DrawString(font,Singleton.Instance.Stage.ToString(),
        new Vector2(Position.X + (Singleton.GetViewPortFromSpriteSheet("Stage_Label").Width - Singleton.GetViewPortFromSpriteSheet("Stage_Box").Width + 16)/2 + 16 - fontSize.X/2, Position.Y + 16*5 + 10),Color.White);

        spriteBatch.Draw(_texture, new Vector2(Position.X - (Singleton.GetViewPortFromSpriteSheet("Money_Label").Width + Singleton.GetViewPortFromSpriteSheet("Ceiling_Turn_Label").Width + 16)/2 , Position.Y + 16*8), 
        Singleton.GetViewPortFromSpriteSheet("Money_Label"), Color.White);

        spriteBatch.Draw(_texture, new Vector2(Position.X - (Singleton.GetViewPortFromSpriteSheet("Money_Box").Width + Singleton.GetViewPortFromSpriteSheet("Ceiling_Turn_Box").Width + 16)/2 , Position.Y + 16*10), 
        Singleton.GetViewPortFromSpriteSheet("Money_Box"), Color.White);

        fontSize = font.MeasureString(Singleton.Instance.Money.ToString());
        spriteBatch.DrawString(font,Singleton.Instance.Money.ToString(),
        new Vector2(Position.X - (Singleton.GetViewPortFromSpriteSheet("Money_Box").Width + Singleton.GetViewPortFromSpriteSheet("Ceiling_Turn_Box").Width + 16)/2 + 16*3 - fontSize.X/2, Position.Y + 16*10 + 10),Color.White);

        spriteBatch.Draw(_texture, new Vector2(Position.X + (Singleton.GetViewPortFromSpriteSheet("Money_Label").Width - Singleton.GetViewPortFromSpriteSheet("Ceiling_Turn_Label").Width + 16)/2 , Position.Y + 16*8),  
        Singleton.GetViewPortFromSpriteSheet("Ceiling_Turn_Label"), Color.White);

        spriteBatch.Draw(_texture, new Vector2(Position.X + (Singleton.GetViewPortFromSpriteSheet("Money_Box").Width - Singleton.GetViewPortFromSpriteSheet("Ceiling_Turn_Box").Width + 16)/2 , Position.Y + 16*10),  
        Singleton.GetViewPortFromSpriteSheet("Ceiling_Turn_Box"), Color.White);

        fontSize = font.MeasureString((Singleton.CEILING_WAITING_TURN - (Singleton.Instance.ChipShotAmount % Singleton.CEILING_WAITING_TURN)).ToString());
        spriteBatch.DrawString(font,(Singleton.CEILING_WAITING_TURN - (Singleton.Instance.ChipShotAmount % Singleton.CEILING_WAITING_TURN)).ToString(),
        new Vector2(Position.X + (Singleton.GetViewPortFromSpriteSheet("Money_Box").Width - Singleton.GetViewPortFromSpriteSheet("Ceiling_Turn_Box").Width + 16)/2 + 16 - fontSize.X/2, Position.Y + 16*10 + 10),Color.White);

        spriteBatch.Draw(_texture, new Vector2(Position.X - Singleton.GetViewPortFromSpriteSheet("Relic_Box").Width/2, Position.Y + 16*13), Singleton.GetViewPortFromSpriteSheet("Relic_Box"), Color.White);

        spriteBatch.Draw(_texture, new Vector2(Position.X  - Singleton.GetViewPortFromSpriteSheet("Next_Chip_Label").Width/2 , Position.Y + 16*23),Singleton.GetViewPortFromSpriteSheet("Next_Chip_Label"),Color.White);
        spriteBatch.Draw(_texture, new Vector2(Position.X  - Singleton.GetViewPortFromSpriteSheet("Next_Chip_Box").Width/2 , Position.Y + 16*25),Singleton.GetViewPortFromSpriteSheet("Next_Chip_Box"),Color.White);
    }
}
