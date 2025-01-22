using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MidtermComGame;


public class MainScene
{
    private SpriteBatch _spriteBatch;
    private Texture2D _bubbleTexture, _rectTexture;
    private SpriteFont _font;
    private List<GameObject> _gameObjects;

    public void Initialize()
    {
        Singleton.Instance.GameBoard = new BubbleType[Singleton.BUBBLE_GRID_HEIGHT, Singleton.BUBBLE_GRID_WIDTH];
        Singleton.Instance.Random = new System.Random();
        Singleton.Instance.BubbleShotAmount = 0;
        Singleton.Instance.PlayAreaStartY = 0;
        Singleton.Instance.CurrentGameState = Singleton.GameState.Playing;

        _gameObjects = new List<GameObject>();
    }

    public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        _spriteBatch = spriteBatch;

        _font = content.Load<SpriteFont>("GameFont");
        _bubbleTexture = content.Load<Texture2D>("Bubble");

        _rectTexture = new Texture2D(graphicsDevice, 3, 640);
        Color[] data = new Color[3 * 640];
        for (int i = 0; i < data.Length; i++) data[i] = Color.White;
        _rectTexture.SetData(data);

        Texture2D cannonTexture = content.Load<Texture2D>("Cannon");

        _gameObjects.Clear();
        _gameObjects.Add(new Player(cannonTexture)
        {
            Name = "Player",
            Viewport = new Rectangle(0, 0, 72, 72),
            Position = new Vector2(Singleton.SCREEN_WIDTH / 2, 400),
            Left = Keys.Left,
            Right = Keys.Right,
            Fire = Keys.Space,
            Bubble = new Bubble(_bubbleTexture)
            {
                Name = "Bubble",
                Viewport = new Rectangle(0, 0, 32, 32),
                Velocity = new Vector2(0, -600f),
                Speed = 0
            }
        });

        foreach (GameObject s in _gameObjects)
        {
            s.Reset();
        }
    }

    public void Update(GameTime gameTime)
    {
        Singleton.Instance.CurrentKey = Keyboard.GetState();
        int numObject = _gameObjects.Count;

        switch (Singleton.Instance.CurrentGameState)
        {
            case Singleton.GameState.Playing:
                for (int i = 0; i < numObject; i++)
                {
                    if (_gameObjects[i].IsActive) _gameObjects[i].Update(gameTime, _gameObjects);
                }
                _gameObjects.RemoveAll(obj => !obj.IsActive);
                break;

            case Singleton.GameState.CheckBubbleAndCeiling:
                // Add specific logic here
                Singleton.Instance.CurrentGameState = Singleton.GameState.Playing;
                break;
        }

        Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;
    }

    public void Draw(GameTime gameTime)
    {
        int numObject = _gameObjects.Count;

        for (int i = 0; i < numObject; i++)
        {
            _gameObjects[i].Draw(_spriteBatch);
        }

        _spriteBatch.Draw(_rectTexture, new Vector2(Singleton.PLAY_AREA_END_X, 0), Color.White);
        _spriteBatch.Draw(_rectTexture, new Vector2(Singleton.PLAY_AREA_START_X, 0), Color.White);

        _spriteBatch.Draw(_bubbleTexture, new Vector2(Singleton.SCREEN_WIDTH / 8, 400),
            Singleton.GetBubbleColor(Singleton.Instance.NextBubble));
    }
}

