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


    // Main Menu Variables
    

    // Game Scene (MainScene)
    private MainScene _mainScene;
    private MainMenu _mainMenu;
    private PauseMenu _pauseMenu;

    public GameManager()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = Singleton.SCREEN_WIDTH;
        _graphics.PreferredBackBufferHeight = Singleton.SCREEN_HEIGHT;

        Singleton.Instance.CurrentGameState = Singleton.GameState.MainMenu;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
     
        _mainMenu = new MainMenu();
        _mainMenu.Initialize();
        _mainMenu.LoadContent(Content, GraphicsDevice, _spriteBatch);
        
        // Initialize the main game scene
        _mainScene = new MainScene();
        _mainScene.Initialize(); // Ensure MainScene has a proper Initialize method
        _mainScene.LoadContent(Content, GraphicsDevice, _spriteBatch);
        Singleton.Instance.CurrentGameState = Singleton.GameState.MainMenu;

        _pauseMenu = new PauseMenu();
        _pauseMenu.Initialize(); // Ensure MainScene has a proper Initialize method
        _pauseMenu.LoadContent(Content, GraphicsDevice, _spriteBatch);
    }

    protected override void Update(GameTime gameTime)
    {
        Singleton.Instance.CurrentKey = Keyboard.GetState();
        Singleton.Instance.CurrentMouseState = Mouse.GetState();

        switch (Singleton.Instance.CurrentGameState)
        {
            case Singleton.GameState.MainMenu:
                _mainScene.Update(gameTime);
                _mainMenu.Update(gameTime);
                break;
            case Singleton.GameState.Pause:
                _mainScene.Update(gameTime);
                _pauseMenu.Update(gameTime);
                break;
            default:
                _mainScene.Update(gameTime);
                break;
        }

        Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;
        Singleton.Instance.PreviousMouseState = Singleton.Instance.CurrentMouseState;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        switch (Singleton.Instance.CurrentGameState)
        {
            case Singleton.GameState.MainMenu:
                _mainMenu.Draw(gameTime);
                break;
            case Singleton.GameState.Pause:
                _mainScene.Draw(gameTime);
                _pauseMenu.Draw(gameTime);
                break;
            default:
                _mainScene.Draw(gameTime);
                break;
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
