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
    Texture2D _SpriteTexture;
    Texture2D _rectTexture;
    Texture2D _LevelPassTexture;
    SoundEffect _ceilingPushingSound;
    SoundEffect _chipHitSound; 
    SoundEffect _handSlidingSound; 
    SoundEffect _LosingBetSound;
    SoundEffect _WinningBetSound;

    Song _gameMusic;

    SlotMachine _slotMachine;
    GameStat _gameStat;

    private int _slotMachinePositionX = 470;
    private Vector2 _statPosition = new Vector2(90, 16);
    private double _levelPassTimer = 0;
    private bool _showLevelPass = false;
    public void Initialize()
    {
        _gameObjects = new List<GameObject>();
        Singleton.Instance.GameBoard = new GameBoard(Singleton.CHIP_GRID_HEIGHT, Singleton.CHIP_GRID_WIDTH);
    }

    public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        _spriteBatch = spriteBatch;
        _font = content.Load<SpriteFont>("GameFont");

        //TODO REMOVE THIS AFTER ADD NEW TEXTURE
        _LevelPassTexture = content.Load<Texture2D>("Pause1");

        _SpriteTexture= content.Load<Texture2D>("Sprite_Sheet");

        _rectTexture = new Texture2D(graphicsDevice, 3, 640);
        Color[] data = new Color[3 * 640];
        for (int i = 0; i < data.Length; i++) data[i] = Color.White;
        _rectTexture.SetData(data);

        _ceilingPushingSound = content.Load<SoundEffect>("Ceilingpushing");
        _chipHitSound = content.Load<SoundEffect>("ChipHit");
        _handSlidingSound = content.Load<SoundEffect>("Sliding");
        _gameMusic = content.Load<Song>("A Night Alone - TrackTribe");
        _LosingBetSound = content.Load<SoundEffect>("aw-dangit");
        _WinningBetSound = content.Load<SoundEffect>("winningSFX");

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

                UpdateGameObject(gameTime);

                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && Singleton.Instance.PreviousKey.IsKeyUp(Keys.Escape))
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.Pause;
                }
                break;
            case Singleton.GameState.CheckCeiling:
                CheckAndPushDownCeiling();
                UpdateGameObject(gameTime);
                Singleton.Instance.CurrentGameState = Singleton.GameState.CheckGameBoard;
                break;
            case Singleton.GameState.CheckGameBoard:
                CheckAndDestroyHangingChips();
                Singleton.Instance.NextChip = Singleton.Instance.GameBoard.GetRandomChipColor();
                UpdateGameObject(gameTime);
                CheckGameOver();
                CheckLevelClear();
                break;
            case Singleton.GameState.Pause:    
                // Adjust volume with Up/Down arrow keys
                Singleton.Instance.SFXVolume = MathHelper.Clamp(Singleton.Instance.SFXVolume, 0.0f, 1.0f);
                Singleton.Instance.MusicVolume = MathHelper.Clamp(Singleton.Instance.MusicVolume, 0.0f, 1.0f);
                
                //TODO check this please
                MediaPlayer.Volume = Singleton.Instance.MusicVolume; 
                SoundEffect.MasterVolume = Singleton.Instance.SFXVolume;
                break;
                
            case Singleton.GameState.PassingLevel:
                _levelPassTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (_levelPassTimer <= 0)
                {
                    _showLevelPass = false;
                    Singleton.Instance.Stage++;
                    Singleton.Instance.CurrentGameState = Singleton.GameState.SetLevel;
                    _levelPassTimer = 3.0f;
                }else{
                    _showLevelPass = true;
                }
                break;
            case Singleton.GameState.GameOver:
                if (MediaPlayer.State == MediaState.Playing)
                {
                    MediaPlayer.Stop();
                }
                if(Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape)){
                    Singleton.SaveScore();
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

    protected void UpdateGameObject(GameTime gameTime)
    {
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
    }

    public void Draw(GameTime gameTime)
    {
        _numObject = _gameObjects.Count;

        //draw background
        _spriteBatch.Draw(_SpriteTexture, new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("Ingame_Background").Width)/2 ,0),
            Singleton.GetViewPortFromSpriteSheet("Ingame_Background"), Color.White);

            
        _spriteBatch.Draw(_SpriteTexture, new Vector2(Singleton.PLAY_AREA_START_X, - Singleton.GetViewPortFromSpriteSheet("Chip_Stick").Height + Singleton.Instance.CeilingPosition),
        Singleton.GetViewPortFromSpriteSheet("Chip_Stick"), Color.White);

        for (int i = 0; i < _numObject; i++)
        {
            _gameObjects[i].Draw(_spriteBatch);
        }

        //draw Next Chip Display
        _spriteBatch.Draw(_SpriteTexture, new Vector2(_statPosition.X - Singleton.CHIP_SIZE/2 , _statPosition.Y + 16*25 + 8), 
            new Rectangle(((int)Singleton.Instance.NextChip - 1) * Singleton.CHIP_SIZE, 0, Singleton.CHIP_SIZE, Singleton.CHIP_SIZE + Singleton.CHIP_SHADOW_HEIGHT),Color.White); 

        if (Singleton.Instance.CurrentGameState == Singleton.GameState.GameOver)
        {
            _spriteBatch.Draw(_rectTexture, Vector2.Zero, new Rectangle(0, 0, Singleton.SCREEN_WIDTH, Singleton.SCREEN_HEIGHT), new Color(0, 0, 0, 100));
            _spriteBatch.Draw(_SpriteTexture,
                new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("GameOver_Title").Width) / 2, (Singleton.SCREEN_HEIGHT - Singleton.GetViewPortFromSpriteSheet("GameOver_Title").Height) / 2),
                Singleton.GetViewPortFromSpriteSheet("GameOver_Title"), Color.White);
            return;
        }
        if (Singleton.Instance.CurrentGameState == Singleton.GameState.Pause)
        {
            //idk what to put in here
            //hehe 
            return;
        }
        if (Singleton.Instance.CurrentGameState == Singleton.GameState.PassingLevel && _showLevelPass)
        {
            Vector2 position = new Vector2(
                (Singleton.SCREEN_WIDTH - _LevelPassTexture.Width) / 2,
                (Singleton.SCREEN_HEIGHT - _LevelPassTexture.Height) / 2
            );
            _spriteBatch.Draw(_LevelPassTexture, position, Color.White);
        }
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
        Singleton.Instance.Stage = 10;
        Singleton.Instance.CurrentGameState = Singleton.GameState.SetLevel;
        _levelPassTimer = 3.0f;

        _gameObjects.Add(new Player(_SpriteTexture)
        {
            Name = "Player",
            Viewport = Singleton.GetViewPortFromSpriteSheet("Player_Hand"),
            Position = new Vector2(Singleton.SCREEN_WIDTH / 2, Singleton.CHIP_SHOOTING_HEIGHT),
            Left = Keys.Left,
            Right = Keys.Right,
            Fire = Keys.Space,
            Chip = new Chip(_SpriteTexture)
            {
                Name = "Chip",
                IsShot = false,
                Viewport = new Rectangle(0, 0, Singleton.CHIP_SIZE, Singleton.CHIP_SIZE + Singleton.CHIP_SHADOW_HEIGHT), 
                ChipHitSound = _chipHitSound,
                Speed = 0,
                Score = 10
            }
        });


        AddExtraGameAsset();

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

        _gameObjects.Add(new Player(_SpriteTexture)
        {
            Name = "Player",
            Viewport = Singleton.GetViewPortFromSpriteSheet("Player_Hand"),
            Position = new Vector2(Singleton.SCREEN_WIDTH / 2, Singleton.CHIP_SHOOTING_HEIGHT),
            SlidingSound = _handSlidingSound,
            Left = Keys.Left,
            Right = Keys.Right,
            Fire = Keys.Space,
            Chip = new Chip(_SpriteTexture)
            {
                Name = "Chip",
                IsShot = false,
                Viewport = new Rectangle(0, 0, Singleton.CHIP_SIZE, Singleton.CHIP_SIZE + Singleton.CHIP_SHADOW_HEIGHT), 
                ChipHitSound = _chipHitSound,
                Speed = 0,
                Score = 10
            }
        });

        AddExtraGameAsset();

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

    protected void AddExtraGameAsset(){

        _slotMachine = new SlotMachine(_SpriteTexture)
        {
            Name = "SlotMachine",
            Viewport = Singleton.GetViewPortFromSpriteSheet("Slot_Machine"),
            Position = new Vector2(_slotMachinePositionX, Singleton.SCREEN_HEIGHT/2 - Singleton.GetViewPortFromSpriteSheet("Slot_Machine").Height/2 + 32),
            LosingBetSound = _LosingBetSound,
            WinningBetSound = _WinningBetSound
        };

        _gameStat = new GameStat(_SpriteTexture){
            Name = "GameStat",
            font = _font,
            Position = _statPosition,
        };

        _gameObjects.Add(_slotMachine);
        _gameObjects.Add(_gameStat);
    }

    protected void SetUpInitalChipsPattern()
    {
        Stage.SetUpBoard();

        for (int j = 0; j < Singleton.CHIP_GRID_HEIGHT; j++)
        {
            for (int i = 0; i < Singleton.CHIP_GRID_WIDTH; i++)
            {
                if(Singleton.Instance.GameBoard.HaveChip(j, i))
                    AddChipToBoard(i, j);
            }
        }
    }

    protected void AddChipToBoard(int i, int j)
    {
        int offSetX = (j % 2 == 1) ? Singleton.CHIP_SIZE / 2 : 0;
        Chip newChip = new Chip(_SpriteTexture)
        {
            Name = "Chip",
            IsShot = true,
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
                if(!Singleton.Instance.GameBoard.HaveChip(j, i))
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
                    Singleton.Instance.Score += (int)(10 * Math.Pow(2, ConnectedChips.Count));
                    StartChipFalling(ConnectedChips,_gameObjects);
                }
            }
        }
    }

    protected void StartChipFalling(List<Vector2> connectedChips, List<GameObject> gameObjects)
    {
        foreach (Vector2 chipPosition in connectedChips)
        {
            foreach (GameObject s in gameObjects)
            {
                if(s is Chip && (s as Chip).BoardCoord == chipPosition)
                {
                    (s as Chip).IsFalling = true;
                    break;
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
