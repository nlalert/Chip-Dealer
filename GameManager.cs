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
    private Rectangle _startButtonRectangle;
    private MouseState _previousMouseState;

    // Game Scene (MainScene)
    private MainScene _mainScene;

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
        int buttonWidth = _startButtonTexture.Width;
        int buttonHeight = _startButtonTexture.Height;
        int buttonX = (Singleton.SCREEN_WIDTH - buttonWidth) / 2;
        int buttonY = (Singleton.SCREEN_HEIGHT - buttonHeight) / 2;
        _startButtonRectangle = new Rectangle(buttonX, buttonY, buttonWidth, buttonHeight);

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
        _spriteBatch.Draw(_startButtonTexture, _startButtonRectangle, Color.White);
    }
}
