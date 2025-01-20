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
        GraphicsDevice.Clear(Color.CornflowerBlue);

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

        _gameObjects.Clear();

        foreach (GameObject s in _gameObjects)
        {
            s.Reset();
        }
    }
}
