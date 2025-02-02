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
            _scoreLabelType = ViewportManager.Get("Score_Label0");
            _scoreBoxType = ViewportManager.Get("Score_Box0");
        }
        else if(Singleton.Instance.Score < 10000)
        {
            _scoreLabelType = ViewportManager.Get("Score_Label1");
            _scoreBoxType = ViewportManager.Get("Score_Box1");
        }
        else
        {
            _scoreLabelType = ViewportManager.Get("Score_Label2");
            _scoreBoxType = ViewportManager.Get("Score_Box2");
        }
            
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, new Vector2(Position.X - ViewportManager.Get("Score_Label0").Width/2, Position.Y), _scoreLabelType, Color.White);
        spriteBatch.Draw(_texture, new Vector2(Position.X - ViewportManager.Get("Score_Box0").Width/2, Position.Y + 16*2), _scoreBoxType, Color.White);

        fontSize = font.MeasureString(Singleton.Instance.Score.ToString());
        spriteBatch.DrawString(font,Singleton.Instance.Score.ToString(),new Vector2(Position.X - fontSize.X/2, Position.Y + 16*2 + 10),Color.White);

        spriteBatch.Draw(_texture, new Vector2(Position.X - (ViewportManager.Get("Stage_Label").Width + ViewportManager.Get("Stage_Box").Width + 16)/2 , Position.Y + 16*5 + 8), 
        ViewportManager.Get("Stage_Label"), Color.White);

        spriteBatch.Draw(_texture, new Vector2(Position.X + (ViewportManager.Get("Stage_Label").Width - ViewportManager.Get("Stage_Box").Width + 16)/2 , Position.Y + 16*5),  
        ViewportManager.Get("Stage_Box"), Color.White);

        fontSize = font.MeasureString(Singleton.Instance.Stage.ToString());
        spriteBatch.DrawString(font,Singleton.Instance.Stage.ToString(),
        new Vector2(Position.X + (ViewportManager.Get("Stage_Label").Width - ViewportManager.Get("Stage_Box").Width + 16)/2 + 16 - fontSize.X/2, Position.Y + 16*5 + 10),Color.White);

        spriteBatch.Draw(_texture, new Vector2(Position.X - (ViewportManager.Get("Money_Label").Width + ViewportManager.Get("Ceiling_Turn_Label").Width + 16)/2 , Position.Y + 16*8), 
        ViewportManager.Get("Money_Label"), Color.White);

        spriteBatch.Draw(_texture, new Vector2(Position.X - (ViewportManager.Get("Money_Box").Width + ViewportManager.Get("Ceiling_Turn_Box").Width + 16)/2 , Position.Y + 16*10), 
        ViewportManager.Get("Money_Box"), Color.White);

        fontSize = font.MeasureString((Singleton.Instance.Score/100).ToString());
        spriteBatch.DrawString(font,(Singleton.Instance.Score/100).ToString(),
        new Vector2(Position.X - (ViewportManager.Get("Money_Box").Width + ViewportManager.Get("Ceiling_Turn_Box").Width + 16)/2 + 16*3 - fontSize.X/2, Position.Y + 16*10 + 8),Color.White);

        spriteBatch.Draw(_texture, new Vector2(Position.X + (ViewportManager.Get("Money_Label").Width - ViewportManager.Get("Ceiling_Turn_Label").Width + 16)/2 , Position.Y + 16*8),  
        ViewportManager.Get("Ceiling_Turn_Label"), Color.White);

        spriteBatch.Draw(_texture, new Vector2(Position.X + (ViewportManager.Get("Money_Box").Width - ViewportManager.Get("Ceiling_Turn_Box").Width + 16)/2 , Position.Y + 16*10),  
        ViewportManager.Get("Ceiling_Turn_Box"), Color.White);

        fontSize = font.MeasureString((Singleton.CEILING_WAITING_TURN - (Singleton.Instance.ChipShotAmount % Singleton.CEILING_WAITING_TURN)).ToString());
        spriteBatch.DrawString(font,(Singleton.CEILING_WAITING_TURN - (Singleton.Instance.ChipShotAmount % Singleton.CEILING_WAITING_TURN)).ToString(),
        new Vector2(Position.X + (ViewportManager.Get("Money_Box").Width - ViewportManager.Get("Ceiling_Turn_Box").Width + 16)/2 + 16 - fontSize.X/2, Position.Y + 16*10 + 10),Color.White);

        spriteBatch.Draw(_texture, new Vector2(Position.X - ViewportManager.Get("Relic_Box").Width/2, Position.Y + 16*13), ViewportManager.Get("Relic_Box"), Color.White);

        spriteBatch.Draw(_texture, new Vector2(Position.X  - ViewportManager.Get("Next_Chip_Label").Width/2 , Position.Y + 16*23),ViewportManager.Get("Next_Chip_Label"),Color.White);
        spriteBatch.Draw(_texture, new Vector2(Position.X  - ViewportManager.Get("Next_Chip_Box").Width/2 , Position.Y + 16*25),ViewportManager.Get("Next_Chip_Box"),Color.White);
    }
}
