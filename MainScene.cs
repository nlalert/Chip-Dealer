using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MidtermComGame;

public class MainScene : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    SpriteFont _font;

    List<GameObject> _gameObjects;
    int _numObject;

    public MainScene()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = Singleton.SCREENWIDTH;
        _graphics.PreferredBackBufferHeight = Singleton.SCREENHEIGHT;
        _graphics.ApplyChanges();
        
        _gameObjects = new List<GameObject>();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _font = Content.Load<SpriteFont>("GameFont");

        Reset();
    }

    protected override void Update(GameTime gameTime)
    {
        Singleton.Instance.CurrentKey = Keyboard.GetState();

        _numObject = _gameObjects.Count;

        // TODO: Do this when only in playing game state
        for (int i = 0; i < _numObject; i++)
            {
                if(_gameObjects[i].IsActive)
                    _gameObjects[i].Update(gameTime, _gameObjects);
            }
            for (int i = 0; i < _numObject; i++)
            {
                if(!_gameObjects[i].IsActive)
                {
                _gameObjects.RemoveAt(i);
                i--;
                _numObject--;
                }
            }

        Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        _numObject = _gameObjects.Count;

        for (int i = 0; i < _numObject; i++)
        {
            _gameObjects[i].Draw(_spriteBatch);
        }

        _spriteBatch.End();

        _graphics.BeginDraw();

        base.Draw(gameTime);
    }

    protected void Reset()
    {
        Singleton.Instance.Random = new System.Random();

        Texture2D spaceInvaderTexture = Content.Load<Texture2D>("SpaceInvaderSheet");
        Texture2D bubbleTexture = Content.Load<Texture2D>("TestBubble");

        _gameObjects.Clear();
        _gameObjects.Add(new Player(spaceInvaderTexture)
        {
            Name = "Player",
            Viewport = new Rectangle(51, 30, 54, 30),
            Position = new Vector2(Singleton.SCREENWIDTH / 2, 400),
            Left = Keys.Left,
            Right = Keys.Right,
            Fire = Keys.Space,
            Bubble = new Bubble(bubbleTexture)
            {
                Name = "Bubble",
                Viewport = new Rectangle(0, 0, 32, 32),
                Velocity = new Vector2(0, -600f)
            }
        });
        foreach (GameObject s in _gameObjects)
        {
            s.Reset();
        }
    }
}
