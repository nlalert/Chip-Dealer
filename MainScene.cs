using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
namespace MidtermComGame;

public class MainScene 
{
    // private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    SpriteFont _font;

    List<GameObject> _gameObjects;
    int _numObject;
    Texture2D _backgroundTexture;
    Texture2D _bubbleTexture;
    Texture2D _chipTexture;
    Texture2D _chipStickTexture;
    Texture2D _rectTexture;
    Texture2D _cannonTexture;

    public void Initialize()
    {
        _gameObjects = new List<GameObject>();
    }

    public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        _spriteBatch = spriteBatch;
        _font = content.Load<SpriteFont>("GameFont");

        _backgroundTexture = content.Load<Texture2D>("Background");

        _bubbleTexture = content.Load<Texture2D>("Bubble");
        _chipTexture = content.Load<Texture2D>("Chips");
        _chipStickTexture = content.Load<Texture2D>("ChipStick");

        _cannonTexture = content.Load<Texture2D>("Cannon");
        _rectTexture = new Texture2D(graphicsDevice, 3, 640);
        Color[] data = new Color[3 * 640];
        for (int i = 0; i < data.Length; i++) data[i] = Color.White;
        _rectTexture.SetData(data);

        Reset();
    }

    public void Update(GameTime gameTime)
    {
        Singleton.Instance.CurrentKey = Keyboard.GetState();

        _numObject = _gameObjects.Count;

        switch (Singleton.Instance.CurrentGameState)
        {
            case Singleton.GameState.Playing:
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
                break;
            case Singleton.GameState.CheckBubbleAndCeiling:
                CheckAndDestroyHangingBubbles();
                CheckAndPushDownCeiling();
                CheckGameOver();
                break;
            case Singleton.GameState.GameOver:
                // TODO: Later Lazy
                break;
        }

        Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;

    }

    public void Draw(GameTime gameTime)
    {

        _numObject = _gameObjects.Count;

        _spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);

        _spriteBatch.Draw(_chipStickTexture, new Vector2(Singleton.PLAY_AREA_START_X, -_chipStickTexture.Height + Singleton.Instance.CeilingPosition),
        null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);

        for (int i = 0; i < _numObject; i++)
        {
            _gameObjects[i].Draw(_spriteBatch);
        }
        
        //Next Bubble Display
        _spriteBatch.Draw(_bubbleTexture,new Vector2(Singleton.SCREEN_WIDTH / 8, 400),Singleton.GetBubbleColor(Singleton.Instance.NextBubble));

        //Game Over Line
        //_spriteBatch.Draw(_rectTexture, new Vector2(0, Singleton.BUBBLE_GRID_HEIGHT * Singleton.BUBBLE_SIZE), null, Color.White, (float) (3*Math.PI/2), Vector2.Zero, 1, SpriteEffects.None, 0f);

    }

    protected void Reset()
    {
        Singleton.Instance.GameBoard = new BubbleType[Singleton.BUBBLE_GRID_HEIGHT, Singleton.BUBBLE_GRID_WIDTH];

        Singleton.Instance.Random = new System.Random();

        Singleton.Instance.BubbleShotAmount = 0;
        Singleton.Instance.PlayAreaStartY = 0;
        Singleton.Instance.PlayAreaStartY = 0;
        Singleton.Instance.CeilingPosition = 0;

        Singleton.Instance.CurrentGameState = Singleton.GameState.Playing;

        // Texture2D cannonTexture = content.Load<Texture2D>("Cannon");

        _gameObjects.Add(new Player(_cannonTexture)
        {
            Name = "Player",
            Viewport = new Rectangle(0, 0, 72, 72),
            Position = new Vector2(Singleton.SCREEN_WIDTH / 2, 400),
            Left = Keys.Left,
            Right = Keys.Right,
            Fire = Keys.Space,
            Bubble = new Bubble(_chipTexture)
            {
                Name = "Bubble",
                Viewport = new Rectangle(0, 0, 32, 32),
                Speed = 0
            }
        });
        
        foreach (GameObject s in _gameObjects)
        {
            s.Reset();
        }
    }

    protected void CheckAndPushDownCeiling()
    {
        if(Singleton.Instance.BubbleShotAmount % Singleton.CEILING_WAITING_TURN == 0){
            Singleton.Instance.PlayAreaStartY += Singleton.BUBBLE_SIZE;
            Singleton.Instance.CeilingPosition += Singleton.BUBBLE_SIZE;

            _numObject = _gameObjects.Count;

            for (int i = 0; i < _numObject; i++)
            {
                if(_gameObjects[i].Name.Contains("Bubble"))
                {
                    _gameObjects[i].Position.Y += Singleton.BUBBLE_SIZE;
                }
            }

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
            _numObject = _gameObjects.Count;
            for (int j = 0; j < _numObject; j++)
            {
                if(_gameObjects[j] is Bubble && (_gameObjects[j] as Bubble).BoardCoord == AdjacentBubbles[i])
                {
                    _gameObjects[j].IsActive = false;
                }
            }
        }
    }

    protected void CheckGameOver()
    {
        Singleton.Instance.CurrentGameState = Singleton.GameState.Playing;

        int ceilingPushedAmount = Singleton.Instance.BubbleShotAmount / Singleton.CEILING_WAITING_TURN;

        for (int i = 0; i < Singleton.BUBBLE_GRID_WIDTH; i++)
        {
            for (int j = Singleton.BUBBLE_GRID_HEIGHT - ceilingPushedAmount; j < Singleton.BUBBLE_GRID_HEIGHT; j++)
            {
                if(Singleton.Instance.GameBoard[j, i] != BubbleType.None)
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GameOver;
                    return;
                }
            }
        }
    }
}
