using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using static Singleton;
namespace MidtermComGame;

public class MainMenu
{
    private Texture2D _startButtonTexture;
    private Texture2D _BackGroundTexture;
    private Texture2D _GameNameTexture;
    private Texture2D _chipTexture;
    private MouseState _previousMouseState;
    private Rectangle _startButtonRectangle;
    private List<Vector3> _ChipPos;// z is type of Chips
    private const float CHIP_FALL_SPEED = 1.5f;
    private SpriteBatch _spriteBatch;

    public void Initialize()
    {
        Console.WriteLine("MainMenu Init");
    }

    public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        Console.WriteLine("Loading Content");
        
        _spriteBatch = spriteBatch;
        // Load main menu assets
        _startButtonTexture = content.Load<Texture2D>("Start"); // Add Start.png to content
        _BackGroundTexture = content.Load<Texture2D>("Background"); 
        _GameNameTexture = content.Load<Texture2D>("GameName"); 
        _chipTexture = content.Load<Texture2D>("Chips");

        int buttonWidth = _startButtonTexture.Width;
        int buttonHeight = _startButtonTexture.Height;
        int buttonX = (Singleton.SCREEN_WIDTH - buttonWidth) / 2;
        int buttonY = (Singleton.SCREEN_HEIGHT - buttonHeight) / 2;
        _startButtonRectangle = new Rectangle(buttonX, buttonY, buttonWidth, buttonHeight);
        //chis falling
        _ChipPos = new List<Vector3>();
        for (int i = 0; i < 50; i++)
        {
            _ChipPos.Add(new Vector3(Singleton.Instance.Random.Next(0, Singleton.SCREEN_WIDTH-(_chipTexture.Width/4))
                                    ,Singleton.Instance.Random.Next(0-_chipTexture.Height, Singleton.SCREEN_HEIGHT+_chipTexture.Height),
                                    new Random().Next(1,5)));
        }
        Reset();
        Console.WriteLine("Content Loaded");

    }

    public void Update(GameTime gameTime)
    {
        // Console.WriteLine("Update Mainmenu");
        
        var currentMouseState = Mouse.GetState();

        // Check for button click
        if (currentMouseState.LeftButton == ButtonState.Pressed &&
            _previousMouseState.LeftButton == ButtonState.Released &&
            _startButtonRectangle.Contains(currentMouseState.Position))
        {
            // Transition to the game scene
            Singleton.Instance.CurrentGameState = GameState.SetLevel;
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
                chip.X = new Random().Next(0, Singleton.SCREEN_WIDTH - _chipTexture.Width/4); // Random X position
                // Optionally change the Z value to switch chip type randomly
                chip.Z = new Random().Next(1, 5);
            }
            _ChipPos[i] = chip; 
        }
        _previousMouseState = currentMouseState;
    }

    public void Draw(GameTime gameTime)
    {
        
        _spriteBatch.Draw(_BackGroundTexture,new Vector2(0,0), Color.White);
        //draw falling chips
        foreach (Vector3 ChipPos in _ChipPos)
        {
            _spriteBatch.Draw(_chipTexture, new Vector2(ChipPos.X,ChipPos.Y),Singleton.GetChipViewPort((ChipType)ChipPos.Z),Color.White); 
        }
        _spriteBatch.Draw(_startButtonTexture, _startButtonRectangle, Color.White);
        _spriteBatch.Draw(_GameNameTexture, new Vector2((Singleton.SCREEN_WIDTH - _GameNameTexture.Width)/2  ,(Singleton.SCREEN_HEIGHT - _GameNameTexture.Height)/4 ), Color.White);
        
    }

    protected void Reset()
    {
        Singleton.Instance.CurrentGameState = Singleton.GameState.MainMenu;
    }

}