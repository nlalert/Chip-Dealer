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

        Texture2D cannonTexture = Content.Load<Texture2D>("Cannon");

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

    protected void CheckAndDestroyHangingBubbles()
    {
        for (int j = 1; j < Singleton.BUBBLE_GRID_HEIGHT; j++)
        {
            for (int i = 0; i < Singleton.BUBBLE_GRID_WIDTH; i++)
            {
                //skip last column
                if (j % 2 == 1 && i == Singleton.BUBBLE_GRID_WIDTH - 1)
                    continue;

                List<Vector2> AdjacentBubbles = new List<Vector2>();

                CheckHighestHangingBubbles(new Vector2(i, j), AdjacentBubbles);

                int highestRow = Singleton.BUBBLE_GRID_HEIGHT;

                foreach (Vector2 b in AdjacentBubbles)
                {
                    if(b.Y < highestRow) highestRow = (int) b.Y;
                }

                if(highestRow != 0)
                    DestroyBubbles(AdjacentBubbles);
                    
            }
        }
    }

    private void CheckHighestHangingBubbles(Vector2 boardCoord, List<Vector2> AdjacentBubbles)
    {
        if(AdjacentBubbles.Contains(boardCoord))
            return;

        int X = (int)boardCoord.X;
        int Y = (int)boardCoord.Y;

        AdjacentBubbles.Add(new Vector2(X, Y));

        if(HaveBubble(X-1, Y)) CheckHighestHangingBubbles(new Vector2(X-1, Y), AdjacentBubbles);
        if(HaveBubble(X+1, Y)) CheckHighestHangingBubbles(new Vector2(X+1, Y), AdjacentBubbles);
        if(HaveBubble(X, Y-1)) CheckHighestHangingBubbles(new Vector2(X, Y-1), AdjacentBubbles);

        bool isOddRow = (Y % 2 == 1);
        
        if (isOddRow)
        {
            if(HaveBubble( X+1, Y-1)) CheckHighestHangingBubbles(new Vector2(X+1, Y-1), AdjacentBubbles);
        }
        else
        {
            if(HaveBubble(X-1, Y-1)) CheckHighestHangingBubbles(new Vector2(X-1, Y-1), AdjacentBubbles);
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

    protected void DestroyBubbles(List<Vector2> AdjacentBubbles)
    {
        for (int i = 0; i < AdjacentBubbles.Count; i++)
        {
            Singleton.Instance.GameBoard[(int)AdjacentBubbles[i].Y, (int)AdjacentBubbles[i].X] = BubbleType.None;
            for (int j = 0; j < _numObject; j++)
            {
                if(_gameObjects[j] is Bubble && (_gameObjects[j] as Bubble).BoardCoord == AdjacentBubbles[i])
                {
                    _gameObjects[j].IsActive = false;
                }
            }
        }
    }
}
