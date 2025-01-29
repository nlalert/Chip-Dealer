using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
namespace MidtermComGame;

public class MainScene 
{
    // private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    SpriteFont _font;

    List<GameObject> _gameObjects;
    int _numObject;
    Texture2D _backgroundTexture;
    Texture2D _chipTexture;
    Texture2D _chipStickTexture;
    Texture2D _handTexture;
    Texture2D _rectTexture;
    Texture2D _ShopTexture;
    Texture2D _GameOverTexture;
    Texture2D _PauseTexture;
    Texture2D _ButtonTexture;
    SoundEffect _ceilingPushingSound;
    SoundEffect _chipHitSound;
    Song _gameMusic;
    Shop _shop;
    private Button _volumeUpButton;
    private Button _volumeDownButton;

    public void Initialize()
    {
        _gameObjects = new List<GameObject>();
        Singleton.Instance.GameBoard = new GameBoard(Singleton.CHIP_GRID_HEIGHT, Singleton.CHIP_GRID_WIDTH);
    }

    public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        _spriteBatch = spriteBatch;
        _font = content.Load<SpriteFont>("GameFont");

        _backgroundTexture = content.Load<Texture2D>("Background");
        _chipTexture = content.Load<Texture2D>("Chips");
        _chipStickTexture = content.Load<Texture2D>("ChipStick");
        _handTexture = content.Load<Texture2D>("Hand");
        _ShopTexture = content.Load<Texture2D>("Shop");
        _GameOverTexture = content.Load<Texture2D>("GameOver1");
        _PauseTexture = content.Load<Texture2D>("Pause1");
        _ButtonTexture = content.Load<Texture2D>("Hand");

        _rectTexture = new Texture2D(graphicsDevice, 3, 640);
        Color[] data = new Color[3 * 640];
        for (int i = 0; i < data.Length; i++) data[i] = Color.White;
        _rectTexture.SetData(data);

        _ceilingPushingSound = content.Load<SoundEffect>("Ceilingpushing");
        _chipHitSound = content.Load<SoundEffect>("ChipHit");
        _gameMusic = content.Load<Song>("A Night Alone - TrackTribe");

        ResetGame();
    }

    public void Update(GameTime gameTime)
    {
        _numObject = _gameObjects.Count;
        switch (Singleton.Instance.CurrentGameState)
        {
            case Singleton.GameState.StartingGame:
                if (MediaPlayer.State == MediaState.Playing)
                {
                    MediaPlayer.Stop();
                }
                ResetGame();
                Singleton.Instance.CurrentGameState = Singleton.GameState.SetLevel;
                break;
            case Singleton.GameState.SetLevel:
                ResetLevel();
                SetUpInitalChipsPattern();
                Singleton.Instance.CurrentChip = Singleton.Instance.GameBoard.GetRandomChipColor();
                Singleton.Instance.NextChip = Singleton.Instance.GameBoard.GetRandomChipColor();

                Singleton.Instance.CurrentGameState = Singleton.GameState.Playing;
                break;
            case Singleton.GameState.Playing:
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Play(_gameMusic);
                }
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
                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && Singleton.Instance.PreviousKey.IsKeyUp(Keys.Escape))
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.Pause;
                }
                break;
            case Singleton.GameState.CheckChipAndCeiling:
                CheckAndDestroyHangingChips();
                Singleton.Instance.NextChip = Singleton.Instance.GameBoard.GetRandomChipColor();
                CheckAndPushDownCeiling();
                CheckGameOver();
                CheckLevelClear();
                break;
            case Singleton.GameState.Pause:    
                // Adjust volume with Up/Down arrow keys
                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Up) && Singleton.Instance.Volume < 1.0f)
                {
                    Singleton.Instance.Volume += 0.01f; 
                }
                else if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Down) && Singleton.Instance.Volume > 0.0f)
                {
                    Singleton.Instance.Volume -= 0.01f; 
                }
                if(_volumeUpButton.IsClicked(Singleton.Instance.CurrentMouseState)
                && Singleton.Instance.CurrentMouseState.LeftButton != Singleton.Instance.PreviousMouseState.LeftButton
                && Singleton.Instance.Volume < 1.0f){
                    Singleton.Instance.Volume += 0.1f; 
                    Console.WriteLine("increase volume" + Singleton.Instance.Volume);
                }else if(_volumeDownButton.IsClicked(Singleton.Instance.CurrentMouseState)
                && Singleton.Instance.CurrentMouseState.LeftButton != Singleton.Instance.PreviousMouseState.LeftButton
                && Singleton.Instance.Volume > 0.0f){
                    Singleton.Instance.Volume -= 0.1f;
                    Console.WriteLine("reduce volume" + Singleton.Instance.Volume);
                }

                Singleton.Instance.Volume = MathHelper.Clamp(Singleton.Instance.Volume, 0.0f, 1.0f);
                
                //TODO check this please
                MediaPlayer.Volume = Singleton.Instance.Volume; 
                SoundEffect.MasterVolume = Singleton.Instance.Volume;
                break;
                
            case Singleton.GameState.PassingLevel:
                Singleton.Instance.Stage++;
                Singleton.Instance.CurrentGameState = Singleton.GameState.SetLevel;
                break;
            case Singleton.GameState.GameOver:
                if (MediaPlayer.State == MediaState.Playing)
                {
                    MediaPlayer.Stop();
                }
                if(Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape)){
                    ResetGame();
                }
                break;
            case Singleton.GameState.MainMenu:
                if (MediaPlayer.State == MediaState.Playing)
                {
                    MediaPlayer.Stop();
                }
                break;
        }
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
        
        //Next Chip Display 
        // Red blue green Yellow
        // 0 1 2 3
        // _spriteBatch.Draw(_chipTexture,new Vector2(Singleton.SCREEN_WIDTH / 8, 400),Singleton.GetChipColor(Singleton.Instance.NextChip));
        
        // Draw the chip using the sourceRectangle
        _spriteBatch.Draw(_chipTexture, new Vector2(Singleton.SCREEN_WIDTH / 8, 400), 
            new Rectangle(((int)Singleton.Instance.NextChip - 1) * Singleton.CHIP_SIZE, 0, Singleton.CHIP_SIZE, Singleton.CHIP_SIZE + Singleton.CHIP_SHADOW_HEIGHT),Color.White); 

        // //Game Over Line
        // _spriteBatch.Draw(_rectTexture, new Vector2(0, (Singleton.CHIP_GRID_HEIGHT - 1) * Singleton.CHIP_SIZE), null, Color.White, (float) (3*Math.PI/2), Vector2.Zero, 1, SpriteEffects.None, 0f);

        Vector2 fontSize = _font.MeasureString("Score : " + Singleton.Instance.Score.ToString());
        _spriteBatch.DrawString(_font,
            "Score : " + Singleton.Instance.Score.ToString(),
            new Vector2((Singleton.SCREEN_WIDTH / 4 - fontSize.X) / 2, 30),
            Color.White);

        if (Singleton.Instance.CurrentGameState == Singleton.GameState.GameOver)
        {
            _spriteBatch.Draw(_rectTexture, Vector2.Zero, new Rectangle(0, 0, Singleton.SCREEN_WIDTH, Singleton.SCREEN_HEIGHT), new Color(0, 0, 0, 100));
            _spriteBatch.Draw(_GameOverTexture, new Vector2((Singleton.SCREEN_WIDTH - _GameOverTexture.Width) / 2, (Singleton.SCREEN_HEIGHT - _GameOverTexture.Height) / 2), Color.White);
            return;
        }
        // if (Singleton.Instance.CurrentGameState == Singleton.GameState.Pause)
        // {
        //     _spriteBatch.Draw(_rectTexture, Vector2.Zero, new Rectangle(0, 0, Singleton.SCREEN_WIDTH, Singleton.SCREEN_HEIGHT), new Color(0, 0, 0, 100));
        //     _spriteBatch.Draw(_PauseTexture, new Vector2((Singleton.SCREEN_WIDTH - _PauseTexture.Width) / 2, (Singleton.SCREEN_HEIGHT - _PauseTexture.Height) / 2), Color.White);
        //     // Display the volume percentage
        //     string volumeText = $"Volume: {Math.Round((decimal)(Singleton.Instance.Volume * 100))}%";
        //     Vector2 textSize = _font.MeasureString(volumeText);
        //     _spriteBatch.DrawString(_font, volumeText, new Vector2((Singleton.SCREEN_WIDTH - textSize.X) / 2, Singleton.SCREEN_HEIGHT / 2), Color.White);
        //     return;
        // }
        
    }

    protected void ResetGame()

    {
        // _gameObjects = new List<GameObject>();
        _gameObjects.Clear();
        Singleton.Instance.GameBoard.ClearBoard();

        Singleton.Instance.Random = new System.Random();

        Singleton.Instance.CeilingPosition = 0;
        Singleton.Instance.ChipShotAmount = 0;
        Singleton.Instance.Score = 0;
        Singleton.Instance.Stage = 1;

        Singleton.Instance.CurrentGameState = Singleton.GameState.SetLevel;

        _gameObjects.Add(new Player(_handTexture)
        {
            Name = "Player",
            Viewport = new Rectangle(0, 0, _handTexture.Width, _handTexture.Height),
            Position = new Vector2(Singleton.SCREEN_WIDTH / 2, Singleton.CHIP_SHOOTING_HEIGHT),
            Left = Keys.Left,
            Right = Keys.Right,
            Fire = Keys.Space,
            Chip = new Chip(_chipTexture)
            {
                Name = "Chip",
                _isShot = false,
                Viewport = new Rectangle(0, 0, Singleton.CHIP_SIZE, Singleton.CHIP_SIZE + Singleton.CHIP_SHADOW_HEIGHT), 
                ChipHitSound = _chipHitSound,
                Speed = 0,
                Score = 10
            }
        });
        _volumeDownButton = new Button(_ButtonTexture){
            Name = "DownButton",
            Viewport = new Rectangle(0, 0, _handTexture.Width, _handTexture.Height),
            Position = new Vector2(Singleton.SCREEN_WIDTH / 2 + 100, Singleton.CHIP_SHOOTING_HEIGHT-50),
            IsActive = true,
        };
        _volumeUpButton = new Button(_ButtonTexture){
            Name = "UpButton",
            Viewport = new Rectangle(0, 0, _handTexture.Width, _handTexture.Height),
            Position = new Vector2(Singleton.SCREEN_WIDTH / 2 +100, Singleton.CHIP_SHOOTING_HEIGHT-150),
            IsActive = true,
        };
        _gameObjects.Add(_volumeDownButton);
        _gameObjects.Add(_volumeUpButton);
        //add shop content
        SetUpShop();

        foreach (GameObject s in _gameObjects)
        {
            s.Reset();
        }
    }

    protected void ResetLevel()
    {

        Singleton.Instance.Random = new System.Random();

        Singleton.Instance.CeilingPosition = 0;
        Singleton.Instance.ChipShotAmount = 0;

        DestroyAllChips();

        _gameObjects.Clear();

        _gameObjects.Add(new Player(_handTexture)
        {
            Name = "Player",
            Viewport = new Rectangle(0, 0, _handTexture.Width, _handTexture.Height),
            Position = new Vector2(Singleton.SCREEN_WIDTH / 2, Singleton.CHIP_SHOOTING_HEIGHT),
            Left = Keys.Left,
            Right = Keys.Right,
            Fire = Keys.Space,
            Chip = new Chip(_chipTexture)
            {
                Name = "Chip",
                _isShot = false,
                Viewport = new Rectangle(0, 0, Singleton.CHIP_SIZE, Singleton.CHIP_SIZE + Singleton.CHIP_SHADOW_HEIGHT), 
                ChipHitSound = _chipHitSound,
                Speed = 0,
                Score = 10
            }
        });

        SetUpShop();

        foreach (GameObject s in _gameObjects)
        {
            s.Reset();
        }
    }

    protected void DestroyAllChips()
    {
        for (int j = 0; j < Singleton.CHIP_GRID_HEIGHT; j++)
        {
            for (int i = 0; i < Singleton.CHIP_GRID_WIDTH; i++)
            {
                Singleton.Instance.GameBoard.DestroySingleChip(j, i, _gameObjects);
            }
        }
    }

    protected void SetUpShop(){
        
        _shop = new Shop(_ShopTexture){
            Name = "Shop",
            Position = new Vector2(Singleton.SCREEN_WIDTH *3/4 ,30)
        };
        // _shop.AddItems

        foreach (ChipType chipType in Enum.GetValues(typeof(ChipType)))
        {
            if(chipType == ChipType.None)
                continue;

            // Map keys for each ChipType
            Keys buyKey = chipType switch
            {
                ChipType.Red => Keys.A,
                ChipType.Blue => Keys.S,
                ChipType.Green => Keys.D,
                ChipType.Yellow => Keys.F,
                ChipType.Purple => Keys.G,
                ChipType.White => Keys.H,
                ChipType.Black => Keys.J,
                ChipType.Orange => Keys.K,
                ChipType.Explosive => Keys.Q,
                _ => Keys.None // Default case for safety
            };

            // Create and configure ShopChip
            ShopChip shopChip = new ShopChip(_chipTexture)
            {
                ChipType = chipType,
                Viewport = Singleton.GetChipViewPort(chipType),
                Price = 0,
                BuyKey = buyKey
            };

            // Add to shop
            _shop.AddShopItem(shopChip);
        }

        // Add the shop to the game objects
        _gameObjects.Add(_shop);
    }
    protected void SetUpInitalChipsPattern()
    {
        Stage.SetUpBoard();

        for (int j = 0; j < Singleton.CHIP_GRID_HEIGHT; j++)
        {
            for (int i = 0; i < Singleton.CHIP_GRID_WIDTH; i++)
            {
                if(Singleton.Instance.GameBoard[j, i] != ChipType.None)
                    AddChipToBoard(i, j);
            }
        }
    }

    protected void AddChipToBoard(int i, int j)
    {
        int offSetX = (j % 2 == 1) ? Singleton.CHIP_SIZE / 2 : 0;
        Chip newChip = new Chip(_chipTexture)
        {
            Name = "Chip",
            _isShot = true,
            Position = new Vector2(Singleton.PLAY_AREA_START_X + i * Singleton.CHIP_SIZE + offSetX, j * Singleton.CHIP_SIZE),
            ChipHitSound = _chipHitSound,
            ChipType = Singleton.Instance.GameBoard[j, i],
            Score = 10
        };

        newChip.Reset();
        newChip.BoardCoord =  new Vector2(i, j);

        _gameObjects.Add(newChip);
    }

    protected void CheckAndPushDownCeiling()
    {
        if(Singleton.Instance.ChipShotAmount % Singleton.CEILING_WAITING_TURN == 0){

            _numObject = _gameObjects.Count;

            for (int i = 0; i < _numObject; i++)
            {
                if(_gameObjects[i].Name.Contains("Chip"))
                {
                    _gameObjects[i].Position.Y += Singleton.CHIP_SIZE;
                }
            }

            Singleton.Instance.CeilingPosition += Singleton.CHIP_SIZE;
            _ceilingPushingSound.Play();
        }
    }

    protected void CheckAndDestroyHangingChips()
    {
        for (int j = 1; j < Singleton.CHIP_GRID_HEIGHT; j++)
        {
            for (int i = 0; i < Singleton.CHIP_GRID_WIDTH; i++)
            {
                //skip last column
                if(Singleton.Instance.GameBoard.IsUnUseSpot(j, i))
                    continue;
                if(Singleton.Instance.GameBoard[j, i] == ChipType.None)
                    continue;

                List<Vector2> ConnectedChips = new List<Vector2>();

                Singleton.Instance.GameBoard.GetAllConnectedChips(new Vector2(i, j), ConnectedChips);
                int highestRow = Singleton.CHIP_GRID_HEIGHT;

                foreach (Vector2 b in ConnectedChips)
                {
                    if(b.Y < highestRow) highestRow = (int) b.Y;
                }

                if(highestRow != 0)
                {
                    Singleton.Instance.GameBoard.DestroyChips(ConnectedChips, _gameObjects);
                    Singleton.Instance.Score += (int)(10 * Math.Pow(2, ConnectedChips.Count));
                }
            }
        }
    }

    protected void CheckGameOver()
    {
        Singleton.Instance.CurrentGameState = Singleton.GameState.Playing;

        int ceilingPushedAmount = Singleton.Instance.ChipShotAmount / Singleton.CEILING_WAITING_TURN;

        for (int i = 0; i < Singleton.CHIP_GRID_WIDTH; i++)
        {
            for (int j = Singleton.CHIP_GRID_HEIGHT - ceilingPushedAmount - 1; j < Singleton.CHIP_GRID_HEIGHT; j++)
            {
                if(Singleton.Instance.GameBoard.HaveChip(j, i))
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GameOver;
                    return;
                }
            }
        }
    }

    protected void CheckLevelClear()
    {
        for (int i = 0; i < Singleton.CHIP_GRID_WIDTH; i++)
        {
            for (int j = 0; j < Singleton.CHIP_GRID_HEIGHT; j++)
            {
                if(Singleton.Instance.GameBoard.HaveChip(j, i))
                {
                    return;
                }
            }
        }
        Singleton.Instance.CurrentGameState = Singleton.GameState.PassingLevel;
    }
}
