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
    Texture2D _rectTexture;

    public MainScene()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = Singleton.SCREEN_WIDTH;
        _graphics.PreferredBackBufferHeight = Singleton.SCREEN_HEIGHT;
        _graphics.ApplyChanges();
        
        _gameObjects = new List<GameObject>();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _font = Content.Load<SpriteFont>("GameFont");
        _bubbleTexture = Content.Load<Texture2D>("Bubble");

        _rectTexture = new Texture2D(_graphics.GraphicsDevice, 3, 640);
        Color[] data = new Color[3*640];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = Color.White;
        }
        _rectTexture.SetData(data);

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

        CheckAndDestroyHangingBubbles();

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

        //Play Border
        _spriteBatch.Draw(_rectTexture, new Vector2(Singleton.PLAY_AREA_END_X, 0), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
        _spriteBatch.Draw(_rectTexture, new Vector2(Singleton.PLAY_AREA_START_X, 0), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
        
        //Next Bubble Display
        _spriteBatch.Draw(_bubbleTexture,new Vector2(Singleton.SCREEN_WIDTH / 8, 400),Singleton.GetBubbleColor(Singleton.Instance.NextBubble));

        _spriteBatch.End();

        _graphics.BeginDraw();

        base.Draw(gameTime);
    }

    protected void Reset()
    {
        Singleton.Instance.GameBoard = new BubbleType[Singleton.BUBBLE_GRID_HEIGHT, Singleton.BUBBLE_GRID_WIDTH];

        Singleton.Instance.Random = new System.Random();

        Texture2D spaceInvaderTexture = Content.Load<Texture2D>("SpaceInvaderSheet");

        _gameObjects.Clear();
        _gameObjects.Add(new Player(spaceInvaderTexture)
        {
            Name = "Player",
            Viewport = new Rectangle(51, 30, 54, 30),
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

    private void CheckAndDestroyHangingBubbles()
    {
        for (int j = 1; j < Singleton.BUBBLE_GRID_HEIGHT; j++)
        {
            for (int i = 0; i < Singleton.BUBBLE_GRID_WIDTH; i++)
            {
                //skip last column
                if (j % 2 == 1 && i == Singleton.BUBBLE_GRID_WIDTH - 1)
                    continue;

                if(!HaveBubble(i-1, j) && !HaveBubble(i+1, j) && !HaveBubble(i, j-1))
                {  
                    if (j % 2 == 1)
                    {
                        if(!HaveBubble(i+1, j-1))
                        {
                            foreach (GameObject s in _gameObjects)
                            {
                                if(s is Bubble && (s as Bubble).BoardCoord == new Vector2(i, j))
                                {
                                    s.IsActive = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        if(!HaveBubble(i-1, j-1))
                        {
                            foreach (GameObject s in _gameObjects)
                            {
                                if(s is Bubble && (s as Bubble).BoardCoord == new Vector2(i, j))
                                {
                                    s.IsActive = false;
                                }
                            }
                        }
                    }
                }


            }
        }
    }

    protected bool HaveBubble(int x, int y)
    {
        if (x >= 0 && x < Singleton.BUBBLE_GRID_WIDTH && 
            y >= 0 && y < Singleton.BUBBLE_GRID_HEIGHT)
        {
            return Singleton.Instance.GameBoard[y, x] != BubbleType.None;
        }
        return false;
    }
}
