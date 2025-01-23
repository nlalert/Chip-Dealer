using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MidtermComGame;

public class GameManager : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private enum GameState
    {
        MainMenu,
        Game
    }

    private GameState _currentGameState;

    // Main Menu Variables
    private Texture2D _startButtonTexture;
    private Texture2D _BackGroundTexture;
    private Texture2D _GameNameTexture;
    private Texture2D _chipTexture;
    private Rectangle _startButtonRectangle;
    private MouseState _previousMouseState;
    private List<Vector3> _ChipPos;// z is type of Chips

    // Game Scene (MainScene)
    private MainScene _mainScene;
    private const float CHIP_FALL_SPEED = 1.5f;
    public GameManager()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = Singleton.SCREEN_WIDTH;
        _graphics.PreferredBackBufferHeight = Singleton.SCREEN_HEIGHT;

        _currentGameState = GameState.MainMenu;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load main menu assets
        _startButtonTexture = Content.Load<Texture2D>("Start"); // Add Start.png to Content
        _BackGroundTexture = Content.Load<Texture2D>("Background"); 
        _GameNameTexture = Content.Load<Texture2D>("GameName"); 
        _chipTexture = Content.Load<Texture2D>("Chips");

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

        // Initialize the main game scene
        _mainScene = new MainScene();
        _mainScene.Initialize(); // Ensure MainScene has a proper Initialize method
        _mainScene.LoadContent(Content, GraphicsDevice, _spriteBatch);

    }

    protected override void Update(GameTime gameTime)
    {
        switch (_currentGameState)
        {
            case GameState.MainMenu:
                UpdateMainMenu();
                break;

            case GameState.Game:
                _mainScene.Update(gameTime);
                break;
        }

        base.Update(gameTime);
    }

    private void UpdateMainMenu()
    {
        var currentMouseState = Mouse.GetState();

        // Check for button click
        if (currentMouseState.LeftButton == ButtonState.Pressed &&
            _previousMouseState.LeftButton == ButtonState.Released &&
            _startButtonRectangle.Contains(currentMouseState.Position))
        {
            // Transition to the game scene
            _currentGameState = GameState.Game;
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
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        switch (_currentGameState)
        {
            case GameState.MainMenu:
                DrawMainMenu();
                break;

            case GameState.Game:
                _mainScene.Draw(gameTime);
                break;
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawMainMenu()
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
}
