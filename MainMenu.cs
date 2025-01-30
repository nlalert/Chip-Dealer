using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using static Singleton;
using System.Linq;
namespace MidtermComGame;

public class MainMenu
{
    private Texture2D _startButtonTexture;
    private Texture2D _ScoreboardTexture;
    private Texture2D _BackGroundTexture;
    private Texture2D _GameNameTexture;
    private Texture2D _chipTexture;
    private Rectangle _startButtonRectangle;
    private Rectangle _ScoreBoardButtonRectangle;
    private List<Vector3> _ChipPos;// z is type of Chips
    private const float CHIP_FALL_SPEED = 1.5f;
    private SpriteBatch _spriteBatch;
    private bool IsShowScore;
    private SpriteFont _font;
    private List<ScoreEntry> _scores;

    public void Initialize()
    {
        Console.WriteLine("MainMenu Init");
    }

    public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        Console.WriteLine("Loading Content");
        IsShowScore = false;
        _spriteBatch = spriteBatch;
        // Load main menu assets
        _startButtonTexture = content.Load<Texture2D>("Start"); // Add Start.png to content
        _BackGroundTexture = content.Load<Texture2D>("Background"); 
        _GameNameTexture = content.Load<Texture2D>("GameName"); 
        _chipTexture = content.Load<Texture2D>("Chips");
        _ScoreboardTexture = content.Load<Texture2D>("ScoreboardButton");
        _font = content.Load<SpriteFont>("GameFont");
        _scores = Singleton.LoadScores();

        int buttonWidth = _startButtonTexture.Width;
        int buttonHeight = _startButtonTexture.Height;
        int StartbuttonX = (Singleton.SCREEN_WIDTH - buttonWidth) / 2;
        int StartbuttonY = (Singleton.SCREEN_HEIGHT *8/16 - buttonHeight) ;
        _startButtonRectangle = new Rectangle(StartbuttonX, StartbuttonY, buttonWidth, buttonHeight);

        int buttonX = (Singleton.SCREEN_WIDTH - _ScoreboardTexture.Width) / 2;
        int buttonY = (Singleton.SCREEN_HEIGHT *11/16 - _ScoreboardTexture.Height) ;
        _ScoreBoardButtonRectangle = new Rectangle(buttonX,buttonY,_ScoreboardTexture.Width,_ScoreboardTexture.Height);

        //chis falling
        _ChipPos = new List<Vector3>();
        int ChipCount = 100;
        for (int i = 0; i < ChipCount; i++)
        {
            _ChipPos.Add(new Vector3(Singleton.Instance.Random.Next(0, Singleton.SCREEN_WIDTH-(_chipTexture.Width/8))
                                    ,Singleton.Instance.Random.Next(0-_chipTexture.Height, Singleton.SCREEN_HEIGHT+_chipTexture.Height),
                                    new Random().Next(1,5)));
        }
        Reset();
        Console.WriteLine("Content Loaded");

    }

    public void Update(GameTime gameTime)
    {
        // Console.WriteLine("Update Mainmenu");
        Singleton.Instance.CurrentKey = Keyboard.GetState();
        Singleton.Instance.CurrentMouseState = Mouse.GetState();

        if (Singleton.Instance.CurrentMouseState.LeftButton == ButtonState.Pressed &&
            Singleton.Instance.PreviousMouseState.LeftButton == ButtonState.Released &&
            _startButtonRectangle.Contains(Singleton.Instance.CurrentMouseState.Position)
            && !IsShowScore)
        {
            // Load Main scene
            Singleton.Instance.CurrentGameState = GameState.SetLevel;
            // Transition to the game scene
            Singleton.Instance.CurrentGameState = GameState.StartingGame;
        }
        if (Singleton.Instance.CurrentMouseState.LeftButton == ButtonState.Pressed &&
            Singleton.Instance.PreviousMouseState.LeftButton == ButtonState.Released &&
            _ScoreBoardButtonRectangle.Contains(Singleton.Instance.CurrentMouseState.Position)
            && !IsShowScore)
        {
            IsShowScore = true;
        }
        if(IsShowScore){
            if(Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && Singleton.Instance.PreviousKey.IsKeyUp(Keys.Escape))
            IsShowScore= false;
        }   
        // Update chip positions
        for (int i = 0; i < _ChipPos.Count; i++)
        {
            Vector3 chip = _ChipPos[i];
            chip.Y += CHIP_FALL_SPEED; // Move chip downwards
            // Reset position if it goes off-screen
            if (chip.Y > Singleton.SCREEN_HEIGHT+_chipTexture.Height)
            {
                chip.Y = -_chipTexture.Height; // Reset Y to top
                if(IsShowScore){// will not block the view
                    do
                    {
                        chip.X = Singleton.Instance.Random.Next(0, Singleton.SCREEN_WIDTH - _chipTexture.Width / 8);
                    } while (chip.X  >= Singleton.SCREEN_WIDTH / 4 
                            && chip.X  <= Singleton.SCREEN_WIDTH * 3 / 4 - _chipTexture.Width / 8); 
                    //this is cool shit WWW
                }else
                chip.X = Singleton.Instance.Random.Next(0, Singleton.SCREEN_WIDTH - _chipTexture.Width/8); // Random X position
                chip.Z = new Random().Next(1, Enum.GetValues(typeof(ChipType)).Length-1); // Random Chip type
            }
            _ChipPos[i] = chip; 
        }
        Singleton.Instance.PreviousMouseState = Singleton.Instance.CurrentMouseState;
        Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;

    }   

    public void Draw(GameTime gameTime)
    {
        
        _spriteBatch.Draw(_BackGroundTexture,new Vector2(0,0), Color.White);
        //draw falling chips
        foreach (Vector3 ChipPos in _ChipPos)
        {
            _spriteBatch.Draw(_chipTexture, new Vector2(ChipPos.X,ChipPos.Y),Singleton.GetChipViewPort((ChipType)ChipPos.Z),Color.White); 
        }
        if(!IsShowScore){
            _spriteBatch.Draw(_startButtonTexture, _startButtonRectangle, Color.White);
            _spriteBatch.Draw(_GameNameTexture, new Vector2((Singleton.SCREEN_WIDTH - _GameNameTexture.Width)/2  ,(Singleton.SCREEN_HEIGHT*5/16 - _GameNameTexture.Height) ), Color.White);
            _spriteBatch.Draw(_ScoreboardTexture,_ScoreBoardButtonRectangle,Color.White);
        }
        else{
            Vector2 position = new Vector2(Singleton.SCREEN_WIDTH /4 + _chipTexture.Width / 4 , 100); 
            foreach (var entry in _scores.OrderByDescending(s => s.Score).Take(10)) // Show top 10 scores
            {
                string text = $"{entry.Score} pts ({entry.Timestamp:MM/dd HH:mm})";
                _spriteBatch.DrawString(_font, text, position, Color.White);
                position.Y += 30; //gap 
            }
        }        
    }

    protected void Reset()
    {
        Singleton.Instance.CurrentGameState = Singleton.GameState.MainMenu;
    }
    private void DisplayScore(){

    }
    
}