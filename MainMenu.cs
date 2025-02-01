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
    private List<double> _ChipFallSpeed;
    private const float MIN_CHIP_FALL_SPEED = 1.25f;
    private const float MAX_CHIP_FALL_SPEED = 1.75f;
    private SpriteBatch _spriteBatch;
    private bool IsShowScore;
    private SpriteFont _font;
    private List<ScoreEntry> _scores;
    private Button _BackButton;
    private Button _StartButton;
    private Button _ScoreBoardButton;
    private Button _ExitButton;

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
        _ChipFallSpeed = new List<double>();
        int ChipCount = 75;
        for (int i = 0; i < ChipCount; i++)
        {
            _ChipPos.Add(new Vector3(Singleton.Instance.Random.Next(0, Singleton.SCREEN_WIDTH-(Singleton.GetViewPortFromSpriteSheet("Red_Chip").Width/8))
                                    ,Singleton.Instance.Random.Next(0-Singleton.GetViewPortFromSpriteSheet("Red_Chip").Height, Singleton.SCREEN_HEIGHT+Singleton.GetViewPortFromSpriteSheet("Red_Chip").Height),
                                    new Random().Next(1,9)));
            
            _ChipFallSpeed.Add(MIN_CHIP_FALL_SPEED + Singleton.Instance.Random.NextDouble() * (MAX_CHIP_FALL_SPEED - MIN_CHIP_FALL_SPEED));
        }

        _StartButton = new Button(_SpriteTexture)
        {
            Name = "StartButton",
            Viewport = Singleton.GetViewPortFromSpriteSheet("Menu_Button"),
            HighlightedViewPort = Singleton.GetViewPortFromSpriteSheet("Menu_Button_Highlighted"),
            Position = new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("Menu_Button").Width) / 2,
                    Singleton.SCREEN_HEIGHT *8/16 - Singleton.GetViewPortFromSpriteSheet("Menu_Button").Height / 2),
            LabelViewPort = Singleton.GetViewPortFromSpriteSheet("Start_Label"),
            HighlightedLabelViewPort = Singleton.GetViewPortFromSpriteSheet("Start_Label_Highlighted"),
            LabelPosition = new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("Start_Label").Width) / 2,
                    Singleton.SCREEN_HEIGHT *8/16 - Singleton.GetViewPortFromSpriteSheet("Start_Label").Height / 2),
            IsActive = true
        };

        _ScoreBoardButton = new Button(_SpriteTexture)
        {
            Name = "ScoreBoardButton",
            Viewport = Singleton.GetViewPortFromSpriteSheet("Menu_Button"),
            HighlightedViewPort = Singleton.GetViewPortFromSpriteSheet("Menu_Button_Highlighted"),
            Position = new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("Menu_Button").Width) / 2,
                    Singleton.SCREEN_HEIGHT *11/16 - Singleton.GetViewPortFromSpriteSheet("Menu_Button").Height / 2),
            LabelViewPort = Singleton.GetViewPortFromSpriteSheet("ScoreBoard_Label"),
            HighlightedLabelViewPort = Singleton.GetViewPortFromSpriteSheet("ScoreBoard_Label_Highlighted"),
            LabelPosition = new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("ScoreBoard_Label").Width) / 2,
                    Singleton.SCREEN_HEIGHT *11/16 - Singleton.GetViewPortFromSpriteSheet("ScoreBoard_Label").Height / 2),
            IsActive = true
        };

        _ExitButton = new Button(_SpriteTexture)
        {
            Name = "ScoreBoardButton",
            Viewport = Singleton.GetViewPortFromSpriteSheet("Menu_Button"),
            HighlightedViewPort = Singleton.GetViewPortFromSpriteSheet("Menu_Button_Highlighted"),
            Position = new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("Menu_Button").Width) / 2,
                    Singleton.SCREEN_HEIGHT *14/16 - Singleton.GetViewPortFromSpriteSheet("Menu_Button").Height / 2),
            LabelViewPort = Singleton.GetViewPortFromSpriteSheet("Exit_Label"),
            HighlightedLabelViewPort = Singleton.GetViewPortFromSpriteSheet("Exit_Label_Highlighted"),
            LabelPosition = new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("Exit_Label").Width) / 2,
                    Singleton.SCREEN_HEIGHT *14/16 - Singleton.GetViewPortFromSpriteSheet("Exit_Label").Height / 2),
            IsActive = true
        };

        _BackButton = new Button(_SpriteTexture)
        {
            Name = "BackButton",
            Viewport = Singleton.GetViewPortFromSpriteSheet("Back_Button"),
            HighlightedViewPort = Singleton.GetViewPortFromSpriteSheet("Back_Button_Highlighted"),
            Position = new Vector2(Singleton.SCREEN_WIDTH /2 - Singleton.GetViewPortFromSpriteSheet("Back_Button").Width/2,
                    Singleton.SCREEN_HEIGHT - Singleton.GetViewPortFromSpriteSheet("Back_Button").Height / 2 - 50),
            LabelViewPort = Singleton.GetViewPortFromSpriteSheet("Back_Label"),
            HighlightedLabelViewPort = Singleton.GetViewPortFromSpriteSheet("Back_Label_Highlighted"),
            LabelPosition = new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("Back_Label").Width) / 2,
                    Singleton.SCREEN_HEIGHT - Singleton.GetViewPortFromSpriteSheet("Back_Label").Height / 2 - 50),
            IsActive = true
            
        };
        
        Reset();
        Console.WriteLine("Content Loaded");
    }

    public void Update(GameTime gameTime)
    {
        if(!IsShowScore){
            _StartButton.ButtonUpdate();
            _ScoreBoardButton.ButtonUpdate();
            _BackButton.ButtonUpdate();
        }
        else  _ExitButton.ButtonUpdate();
           
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

        if (_ExitButton.IsClicked()  && !IsShowScore){
            Singleton.Instance.CurrentGameState = GameState.Exit;
        }

        if(IsShowScore){
            if((Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && Singleton.Instance.PreviousKey.IsKeyUp(Keys.Escape))||
                _BackButton.IsClicked())
            IsShowScore= false;
        }
        // Update chip positions
        for (int i = 0; i < _ChipPos.Count; i++)
        {
            Vector3 chip = _ChipPos[i];
            chip.Y += (float)_ChipFallSpeed[i]; // Move chip downwards

            // Reset position if it goes off-screen
            if (chip.Y > Singleton.SCREEN_HEIGHT+Singleton.GetViewPortFromSpriteSheet("Red_Chip").Height)
            {
                chip.Y = -Singleton.GetViewPortFromSpriteSheet("Red_Chip").Height; // Reset Y to top
                if(IsShowScore){// will not block the view
                    do
                    {
                        chip.X = Singleton.Instance.Random.Next(0, Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("Red_Chip").Width / 8);
                    } while (chip.X  >= Singleton.SCREEN_WIDTH / 4 
                            && chip.X  <= Singleton.SCREEN_WIDTH * 3 / 4 - Singleton.GetViewPortFromSpriteSheet("Red_Chip").Width / 8); 
                    //this is cool shit WWW
                }else
                chip.X = Singleton.Instance.Random.Next(0, Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("Red_Chip").Width/8); // Random X position
                chip.Z = new Random().Next(1, Enum.GetValues(typeof(ChipType)).Length-1); // Random Chip type
                _ChipFallSpeed[i] = MIN_CHIP_FALL_SPEED + Singleton.Instance.Random.NextDouble() * (MAX_CHIP_FALL_SPEED - MIN_CHIP_FALL_SPEED);
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
        new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("PlayArea").Width)/2 ,0),
            Singleton.GetViewPortFromSpriteSheet("PlayArea"), Color.White);

        // draw falling chips
        foreach (Vector3 ChipPos in _ChipPos)
        {
            _spriteBatch.Draw(_SpriteTexture, new Vector2(ChipPos.X,ChipPos.Y),Singleton.GetChipViewPort((ChipType)ChipPos.Z),Color.White); 
        }

        
        if(!IsShowScore){

            //Game Name
            Vector2 GameNamePos = new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("Game_Title").Width)/2  ,Singleton.SCREEN_HEIGHT*5/16 - Singleton.GetViewPortFromSpriteSheet("Game_Title").Height );
            _spriteBatch.Draw(_SpriteTexture,GameNamePos,Singleton.GetViewPortFromSpriteSheet("Game_Title"), Color.White);
            
            //start button
            _StartButton.Draw(_spriteBatch);
            
            //scoreboard
            _ScoreBoardButton.Draw(_spriteBatch);

            //exit
            _ExitButton.Draw(_spriteBatch);    
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
            _BackButton.Draw(_spriteBatch);
        }        
    }

    protected void Reset()
    {
        Singleton.Instance.CurrentGameState = Singleton.GameState.MainMenu;
    }
}