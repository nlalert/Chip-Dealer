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
    private Texture2D _SpriteTexture;
    private List<Vector3> _ChipPos;// z is type of Chips
    private const float CHIP_FALL_SPEED = 1.5f;
    private SpriteBatch _spriteBatch;
    private bool IsShowScore;
    private SpriteFont _font;
    private List<ScoreEntry> _scores;
    private Button _backButton;
    private Button _StartButton;
    private Button _ScoreBoardButton;
    public void Initialize()
    {
        Console.WriteLine("MainMenu Init");
    }

    public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        Console.WriteLine("Loading Content");
        IsShowScore = false;
        _spriteBatch = spriteBatch;
        _font = content.Load<SpriteFont>("GameFont");
        _scores = Singleton.LoadScores();
        _SpriteTexture= content.Load<Texture2D>("Sprite_Sheet");

        //chis falling
        _ChipPos = new List<Vector3>();
        int ChipCount = 100;
        for (int i = 0; i < ChipCount; i++)
        {
            _ChipPos.Add(new Vector3(Singleton.Instance.Random.Next(0, Singleton.SCREEN_WIDTH-(Singleton.GetRectangleFromSpriteSheet("ChipRed").Width/8))
                                    ,Singleton.Instance.Random.Next(0-Singleton.GetRectangleFromSpriteSheet("ChipRed").Height, Singleton.SCREEN_HEIGHT+Singleton.GetRectangleFromSpriteSheet("ChipRed").Height),
                                    new Random().Next(1,5)));
        }

        _StartButton = new Button(_SpriteTexture){
            Name = "StartButton",
            Viewport = Singleton.GetRectangleFromSpriteSheet("StartButton"),
            HighlightViewPort = Singleton.GetRectangleFromSpriteSheet("StartButtonHighlight"),
            Position = new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetRectangleFromSpriteSheet("Button").Width) / 2,
                    Singleton.SCREEN_HEIGHT *8/16 - Singleton.GetRectangleFromSpriteSheet("Button").Height),
            IsActive = true
        };

        _ScoreBoardButton = new Button(_SpriteTexture){
            Name = "ScoreBoardButton",
            Viewport = Singleton.GetRectangleFromSpriteSheet("ScoreBoardButton"),
            HighlightViewPort = Singleton.GetRectangleFromSpriteSheet("ScoreBoardButtonHighlight"),
            Position = new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetRectangleFromSpriteSheet("ScoreBoardButton").Width) / 2,
                    Singleton.SCREEN_HEIGHT *11/16 - Singleton.GetRectangleFromSpriteSheet("ScoreBoardButton").Height),
            IsActive = true
        };

        _backButton = new Button(_SpriteTexture){
            Name = "BackButton",
            Viewport = Singleton.GetRectangleFromSpriteSheet("BackButton"),
            HighlightViewPort = Singleton.GetRectangleFromSpriteSheet("BackButtonHighlight"),
            Position = new Vector2(Singleton.SCREEN_WIDTH /2 - Singleton.GetRectangleFromSpriteSheet("BackButton").Width/2,
                         Singleton.SCREEN_HEIGHT - Singleton.GetRectangleFromSpriteSheet("BackButton").Height - 50),
            IsActive = true
            
        };
        
        Reset();
        Console.WriteLine("Content Loaded");
    }

    public void Update(GameTime gameTime)
    {
        // Console.WriteLine("Update Mainmenu");
        Singleton.Instance.CurrentKey = Keyboard.GetState();
        Singleton.Instance.CurrentMouseState = Mouse.GetState();

        if (_StartButton.IsClicked() && !IsShowScore)
        {
            // Load Main scene
            Singleton.Instance.CurrentGameState = GameState.SetLevel;
            // Transition to the game scene
            Singleton.Instance.CurrentGameState = GameState.StartingGame;
        }
        if (_ScoreBoardButton.IsClicked()&& !IsShowScore)
        {
            IsShowScore = true;
            _scores = Singleton.LoadScores();
        }
        if(IsShowScore){
            if((Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && Singleton.Instance.PreviousKey.IsKeyUp(Keys.Escape))||
                _backButton.IsClicked())
            IsShowScore= false;
        }   
        // Update chip positions
        for (int i = 0; i < _ChipPos.Count; i++)
        {
            Vector3 chip = _ChipPos[i];
            chip.Y += CHIP_FALL_SPEED; // Move chip downwards
            // Reset position if it goes off-screen
            if (chip.Y > Singleton.SCREEN_HEIGHT+Singleton.GetRectangleFromSpriteSheet("ChipRed").Height)
            {
                chip.Y = -Singleton.GetRectangleFromSpriteSheet("ChipRed").Height; // Reset Y to top
                if(IsShowScore){// will not block the view
                    do
                    {
                        chip.X = Singleton.Instance.Random.Next(0, Singleton.SCREEN_WIDTH - Singleton.GetRectangleFromSpriteSheet("ChipRed").Width / 8);
                    } while (chip.X  >= Singleton.SCREEN_WIDTH / 4 
                            && chip.X  <= Singleton.SCREEN_WIDTH * 3 / 4 - Singleton.GetRectangleFromSpriteSheet("ChipRed").Width / 8); 
                    //this is cool shit WWW
                }else
                chip.X = Singleton.Instance.Random.Next(0, Singleton.SCREEN_WIDTH - Singleton.GetRectangleFromSpriteSheet("ChipRed").Width/8); // Random X position
                chip.Z = new Random().Next(1, Enum.GetValues(typeof(ChipType)).Length-1); // Random Chip type
            }
            _ChipPos[i] = chip; 
        }
        Singleton.Instance.PreviousMouseState = Singleton.Instance.CurrentMouseState;
        Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;

    }   

    public void Draw(GameTime gameTime)
    {
        //draw Table
        _spriteBatch.Draw(_SpriteTexture,
        new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetRectangleFromSpriteSheet("PlayArea").Width)/2 ,0),
            Singleton.GetRectangleFromSpriteSheet("PlayArea"), Color.White);
        // draw falling chips
        foreach (Vector3 ChipPos in _ChipPos)
        {
            // _spriteBatch.Draw(_SpriteTexture, new Vector2(ChipPos.X,ChipPos.Y),Singleton.GetChipViewPort((ChipType)ChipPos.Z),Color.White); 
        }

        
        if(!IsShowScore){

            //Game Name
            Vector2 GameNamePos = new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetRectangleFromSpriteSheet("GameName").Width)/2  ,Singleton.SCREEN_HEIGHT*5/16 - Singleton.GetRectangleFromSpriteSheet("GameName").Height );
            _spriteBatch.Draw(_SpriteTexture,GameNamePos,Singleton.GetRectangleFromSpriteSheet("GameName"), Color.White);
            
            //start button
            _StartButton.Draw(_spriteBatch);
            
            //scoreboard
            _ScoreBoardButton.Draw(_spriteBatch);
            
        }
        else{
            //pull data from file
            Vector2 position = new Vector2(Singleton.SCREEN_WIDTH /3 , 50); 
            foreach (var entry in _scores.OrderByDescending(s => s.Score).Take(10)) // Show top 10 scores
            {
                string text = $"{entry.Score} pts ({entry.Timestamp:MM/dd HH:mm})";
                _spriteBatch.DrawString(_font, text, position, Color.White);
                position.Y += 30; //gap 
            }
            //draw back button
            _backButton.Draw(_spriteBatch);
        }        
    }

    protected void Reset()
    {
        Singleton.Instance.CurrentGameState = Singleton.GameState.MainMenu;
    }
}