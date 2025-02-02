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
    private List<GameObject> _gameObjects;

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
        _gameObjects = new List<GameObject>();

        //chis falling
        _ChipPos = new List<Vector3>();
        _ChipFallSpeed = new List<double>();
        int ChipCount = 75;
        for (int i = 0; i < ChipCount; i++)
        {
            _ChipPos.Add(new Vector3(Singleton.Instance.Random.Next(0, Singleton.SCREEN_WIDTH-(ViewportManager.Get("Red_Chip").Width/8))
                                    ,Singleton.Instance.Random.Next(0-ViewportManager.Get("Red_Chip").Height, Singleton.SCREEN_HEIGHT+ViewportManager.Get("Red_Chip").Height),
                                    new Random().Next(1,9)));
            
            _ChipFallSpeed.Add(MIN_CHIP_FALL_SPEED + Singleton.Instance.Random.NextDouble() * (MAX_CHIP_FALL_SPEED - MIN_CHIP_FALL_SPEED));
        }

        _StartButton = new Button(_SpriteTexture)
        {
            Name = "StartButton",
            Viewport = ViewportManager.Get("Menu_Button"),
            HighlightedViewPort = ViewportManager.Get("Menu_Button_Highlighted"),
            Position = new Vector2((Singleton.SCREEN_WIDTH - ViewportManager.Get("Menu_Button").Width) / 2,
                    Singleton.SCREEN_HEIGHT *8/16 - ViewportManager.Get("Menu_Button").Height / 2),
            LabelViewPort = ViewportManager.Get("Start_Label"),
            HighlightedLabelViewPort = ViewportManager.Get("Start_Label_Highlighted"),
            LabelPosition = new Vector2((Singleton.SCREEN_WIDTH - ViewportManager.Get("Start_Label").Width) / 2,
                    Singleton.SCREEN_HEIGHT *8/16 - ViewportManager.Get("Start_Label").Height / 2),
            IsActive = true
        };

        _ScoreBoardButton = new Button(_SpriteTexture)
        {
            Name = "ScoreBoardButton",
            Viewport = ViewportManager.Get("Menu_Button"),
            HighlightedViewPort = ViewportManager.Get("Menu_Button_Highlighted"),
            Position = new Vector2((Singleton.SCREEN_WIDTH - ViewportManager.Get("Menu_Button").Width) / 2,
                    Singleton.SCREEN_HEIGHT *11/16 - ViewportManager.Get("Menu_Button").Height / 2),
            LabelViewPort = ViewportManager.Get("ScoreBoard_Label"),
            HighlightedLabelViewPort = ViewportManager.Get("ScoreBoard_Label_Highlighted"),
            LabelPosition = new Vector2((Singleton.SCREEN_WIDTH - ViewportManager.Get("ScoreBoard_Label").Width) / 2,
                    Singleton.SCREEN_HEIGHT *11/16 - ViewportManager.Get("ScoreBoard_Label").Height / 2),
            IsActive = true
        };

        _ExitButton = new Button(_SpriteTexture)
        {
            Name = "ScoreBoardButton",
            Viewport = ViewportManager.Get("Menu_Button"),
            HighlightedViewPort = ViewportManager.Get("Menu_Button_Highlighted"),
            Position = new Vector2((Singleton.SCREEN_WIDTH - ViewportManager.Get("Menu_Button").Width) / 2,
                    Singleton.SCREEN_HEIGHT *14/16 - ViewportManager.Get("Menu_Button").Height / 2),
            LabelViewPort = ViewportManager.Get("Exit_Label"),
            HighlightedLabelViewPort = ViewportManager.Get("Exit_Label_Highlighted"),
            LabelPosition = new Vector2((Singleton.SCREEN_WIDTH - ViewportManager.Get("Exit_Label").Width) / 2,
                    Singleton.SCREEN_HEIGHT *14/16 - ViewportManager.Get("Exit_Label").Height / 2),
            IsActive = true
        };

        _BackButton = new Button(_SpriteTexture)
        {
            Name = "BackButton",
            Viewport = ViewportManager.Get("Small_Button"),
            HighlightedViewPort = ViewportManager.Get("Small_Button_Highlighted"),
            Position = new Vector2(Singleton.SCREEN_WIDTH /2 - ViewportManager.Get("Small_Button").Width/2,
                    Singleton.SCREEN_HEIGHT - ViewportManager.Get("Small_Button").Height / 2 - 50),
            LabelViewPort = ViewportManager.Get("Back_Label"),
            HighlightedLabelViewPort = ViewportManager.Get("Back_Label_Highlighted"),
            LabelPosition = new Vector2((Singleton.SCREEN_WIDTH - ViewportManager.Get("Back_Label").Width) / 2,
                    Singleton.SCREEN_HEIGHT - ViewportManager.Get("Back_Label").Height / 2 - 50),
            IsActive = true
            
        };
        
        Reset();
        Console.WriteLine("Content Loaded");
    }

    public void Update(GameTime gameTime)
    {
        if(!IsShowScore){
            _StartButton.Update(gameTime , _gameObjects);
            _ScoreBoardButton.Update(gameTime , _gameObjects);
            _BackButton.Update(gameTime , _gameObjects);
        }
        else  _ExitButton.Update(gameTime , _gameObjects);;
           
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
            if (chip.Y > Singleton.SCREEN_HEIGHT+ViewportManager.Get("Red_Chip").Height)
            {
                chip.Y = -ViewportManager.Get("Red_Chip").Height; // Reset Y to top
                if(IsShowScore){// will not block the view
                    do
                    {
                        chip.X = Singleton.Instance.Random.Next(0, Singleton.SCREEN_WIDTH - ViewportManager.Get("Red_Chip").Width / 8);
                    } while (chip.X  >= Singleton.SCREEN_WIDTH / 5 
                            && chip.X  <= Singleton.SCREEN_WIDTH * 3 / 4 - ViewportManager.Get("Red_Chip").Width / 8); 
                    //this is cool shit WWW
                }else
                chip.X = Singleton.Instance.Random.Next(0, Singleton.SCREEN_WIDTH - ViewportManager.Get("Red_Chip").Width/8); // Random X position
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
        new Vector2((Singleton.SCREEN_WIDTH - ViewportManager.Get("Mainmenu_Background").Width)/2 ,0),
            ViewportManager.Get("Mainmenu_Background"), Color.White);

        // draw falling chips
        foreach (Vector3 ChipPos in _ChipPos)
        {
            _spriteBatch.Draw(_SpriteTexture, new Vector2(ChipPos.X,ChipPos.Y),ViewportManager.Get(((ChipType)ChipPos.Z).ToString() + "_Chip"),Color.White); 
        }

        
        if(!IsShowScore){

            //Game Name
            Vector2 GameNamePos = new Vector2((Singleton.SCREEN_WIDTH - ViewportManager.Get("Game_Title").Width)/2  , 16);
            _spriteBatch.Draw(_SpriteTexture,GameNamePos,ViewportManager.Get("Game_Title"), Color.White);
            
            //start button
            _StartButton.Draw(_spriteBatch);
            
            //scoreboard
            _ScoreBoardButton.Draw(_spriteBatch);

            //exit
            _ExitButton.Draw(_spriteBatch);    
        }

        else{

            //pull data from file
            float currentGap =50;
            foreach (var entry in _scores.OrderByDescending(s => s.Score).Take(10)) // Show top 10 scores
            {   
                
                string text = $"{entry.Score} pts ({entry.Timestamp:MM/dd HH:mm})";
                Vector2 position = new Vector2(Singleton.SCREEN_WIDTH /2 -_font.MeasureString(text).X/2, currentGap); 
                _spriteBatch.DrawString(_font, text, position, Color.White);
                currentGap += 30; //gap 
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