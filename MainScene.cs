using System;
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

    Texture2D _bubbleTexture;

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
        _bubbleTexture = Content.Load<Texture2D>("TestBubble");

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

        DrawSetBubble(_spriteBatch);

        _spriteBatch.End();

        _graphics.BeginDraw();

        base.Draw(gameTime);
    }

    protected void DrawSetBubble(SpriteBatch _spriteBatch)
    {
        for (int j = 0; j < Singleton.PLAYAREAHEIGHT; j++)
        {
            int Xoffset = (j % 2 == 0) ? 0 : (Singleton.BUBBLESIZE / 2);

            for (int i = 0; i < Singleton.PLAYAREAWIDTH; i++)
            {
                if(Xoffset != 0 && i == Singleton.PLAYAREAWIDTH - 1)
                    continue;

                //draw corresponding to each color (now only red)
                if (Singleton.Instance.GameBoard[j, i] == Singleton.BubbleType.Red)
                    _spriteBatch.Draw(_bubbleTexture, new Vector2(i * Singleton.BUBBLESIZE + Xoffset + Singleton.PlayAreaStartX, j * Singleton.BUBBLESIZE), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
        }
    }

    protected void Reset()
    {
        Singleton.Instance.GameBoard = new Singleton.BubbleType[Singleton.PLAYAREAHEIGHT, Singleton.PLAYAREAWIDTH];

        Singleton.Instance.Random = new System.Random();

        Texture2D spaceInvaderTexture = Content.Load<Texture2D>("SpaceInvaderSheet");

        _gameObjects.Clear();
        _gameObjects.Add(new Player(spaceInvaderTexture)
        {
            Name = "Player",
            Viewport = new Rectangle(51, 30, 54, 30),
            Position = new Vector2(Singleton.SCREENWIDTH / 2, 400),
            Left = Keys.Left,
            Right = Keys.Right,
            Fire = Keys.Space,
            Bubble = new Bubble(_bubbleTexture)
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
