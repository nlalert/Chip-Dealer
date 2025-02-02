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
    SoundEffect _ceilingPushingSound;
    SoundEffect _chipHitSound; 
    SoundEffect _handSlidingSound; 
    SoundEffect _LosingBetSound;
    SoundEffect _WinningBetSound;

    Song _gameMusic;

    GameStat _gameStat;
    SlotMachine _slotMachine;
    Shop _shop;

    private int _slotMachinePositionX = 470;

    private bool HasChipFell = false;
    private Vector2 _statPosition = new Vector2(90, 16);
    public void Initialize()
    {
        _gameObjects = new List<GameObject>();
        Singleton.Instance.GameBoard = new GameBoard(Singleton.CHIP_GRID_HEIGHT, Singleton.CHIP_GRID_WIDTH);
    }

    public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        _spriteBatch = spriteBatch;
        _font = content.Load<SpriteFont>("GameFont");

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
                Singleton.Instance.CurrentGameState = Singleton.GameState.InitializingStage;
                break;
            case Singleton.GameState.InitializingStage:
                ResetStage();
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
                    Singleton.Instance.PreviousGameState = Singleton.Instance.CurrentGameState;
                    Singleton.Instance.CurrentGameState = Singleton.GameState.Pause;
                }

                if (Singleton.Instance.Money <= 0 && Singleton.Instance.waitForPlayer) 
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GameOver;

                break;
            case Singleton.GameState.CheckGameBoard:
                CheckAndPushDownCeiling();
                CheckAndDestroyHangingChips();
                Singleton.Instance.NextChip = Singleton.Instance.GameBoard.GetRandomChipColor();
                Singleton.Instance.waitForPlayer = true;
                UpdateGameObject(gameTime);
                CheckGameOver();
                CheckStageClear();
                break;
            case Singleton.GameState.CheckFalling:
                GetBetterNextChip();
                UpdateGameObject(gameTime);
                CheckStageClear();
                break;

            case Singleton.GameState.Pause:    
                // Adjust volume with Up/Down arrow keys
                Singleton.Instance.SFXVolume = MathHelper.Clamp(Singleton.Instance.SFXVolume, 0.0f, 1.0f);
                Singleton.Instance.MusicVolume = MathHelper.Clamp(Singleton.Instance.MusicVolume, 0.0f, 1.0f);
                
                //TODO check this please
                MediaPlayer.Volume = Singleton.Instance.MusicVolume; 
                SoundEffect.MasterVolume = Singleton.Instance.SFXVolume;
                break;
                
            case Singleton.GameState.StageCompleted:
                if (Singleton.Instance.Stage == 30){
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GameWon;
                    break;
                }
                if (!_gameObjects.Contains(_shop))
                {
                    _shop = new Shop(_SpriteTexture){
                        Name = "Shop",
                        Position = new Vector2(Singleton.SCREEN_WIDTH/2, Singleton.SCREEN_HEIGHT/2),
                        font = _font,
                    };

                    _shop.Reset();
                    _gameObjects.Add(_shop);
                    _gameObjects.Remove(_gameStat);
                    _gameObjects.Add(_gameStat);
                    
                }

                _gameStat.Update(gameTime, _gameObjects);
                _slotMachine.Update(gameTime, _gameObjects);
                _shop.Update(gameTime, _gameObjects);

                if (Singleton.Instance.Money <= 0)
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GameOver;

                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && Singleton.Instance.PreviousKey.IsKeyUp(Keys.Escape))
                {
                    Singleton.Instance.PreviousGameState = Singleton.Instance.CurrentGameState;
                    Singleton.Instance.CurrentGameState = Singleton.GameState.Pause;
                }

                break;

            case Singleton.GameState.GameOver:
            case Singleton.GameState.GameWon:
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

    protected void GetBetterNextChip()
    {
        //(For Better Player Experience)
        //Get new next chip after falling if there are no current chip in board with same type as next chip 
        if(Singleton.Instance.GameBoard.Contains(Singleton.Instance.NextChip))
            return;
        Singleton.Instance.NextChip = Singleton.Instance.GameBoard.GetRandomChipColor();
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
        _spriteBatch.Draw(_SpriteTexture, new Vector2((Singleton.SCREEN_WIDTH - ViewportManager.Get("Ingame_Background").Width)/2 ,0),
            ViewportManager.Get("Ingame_Background"), Color.White);

        //draw objects
        _spriteBatch.Draw(_SpriteTexture, new Vector2(Singleton.PLAY_AREA_START_X, - ViewportManager.Get("Chip_Stick").Height + Singleton.Instance.CeilingPosition),
        ViewportManager.Get("Chip_Stick"), Color.White);

        for (int i = 0; i < _numObject; i++)
        {
            _gameObjects[i].Draw(_spriteBatch);
        }

        //draw game over
        if (Singleton.Instance.CurrentGameState == Singleton.GameState.GameOver)
        {
            _spriteBatch.Draw(_rectTexture, Vector2.Zero, new Rectangle(0, 0, Singleton.SCREEN_WIDTH, Singleton.SCREEN_HEIGHT), new Color(0, 0, 0, 100));
            
            _spriteBatch.Draw(_SpriteTexture,
                new Vector2((Singleton.SCREEN_WIDTH -ViewportManager.Get("Big_Box0").Width) / 2, (Singleton.SCREEN_HEIGHT -ViewportManager.Get("Big_Box0").Height) / 2),
               ViewportManager.Get("Big_Box0"), Color.White);
            _spriteBatch.Draw(_SpriteTexture,

                new Vector2((Singleton.SCREEN_WIDTH -ViewportManager.Get("GameOver_Label").Width) / 2, (Singleton.SCREEN_HEIGHT -ViewportManager.Get("Big_Box0").Height) / 2 + 16*3),
               ViewportManager.Get("GameOver_Label"), Color.White);
            _spriteBatch.Draw(_SpriteTexture,
                new Vector2((Singleton.SCREEN_WIDTH -ViewportManager.Get("NewGame_Label").Width) / 2, (Singleton.SCREEN_HEIGHT -ViewportManager.Get("Big_Box0").Height) / 2 + 16*18),
               ViewportManager.Get("NewGame_Label"), Color.White);  

            _spriteBatch.Draw(_SpriteTexture,
                new Vector2((Singleton.SCREEN_WIDTH -ViewportManager.Get("Score_Label2").Width -ViewportManager.Get("Stage_Box").Width) / 2 - 16
                , (Singleton.SCREEN_HEIGHT -ViewportManager.Get("Big_Box0").Height) / 2 + 16*13),
               ViewportManager.Get("Score_Label2"), Color.White);  
            _spriteBatch.Draw(_SpriteTexture,
                new Vector2((Singleton.SCREEN_WIDTH -ViewportManager.Get("Score_Box2").Width -ViewportManager.Get("Stage_Box").Width) / 2 - 16
                , (Singleton.SCREEN_HEIGHT -ViewportManager.Get("Big_Box0").Height) / 2 + 16*15),
               ViewportManager.Get("Score_Box2"), Color.White);  
            
            Vector2 fontSize = _font.MeasureString(Singleton.Instance.Score.ToString());

            _spriteBatch.DrawString(_font,Singleton.Instance.Score.ToString(),
                new Vector2((Singleton.SCREEN_WIDTH - fontSize.X -ViewportManager.Get("Stage_Box").Width) / 2 - 16,
                (Singleton.SCREEN_HEIGHT -ViewportManager.Get("Big_Box0").Height) / 2 + 16*15 + 10),Color.White);

            _spriteBatch.Draw(_SpriteTexture,
                new Vector2((Singleton.SCREEN_WIDTH -ViewportManager.Get("Stage_Label").Width +ViewportManager.Get("Score_Box2").Width) / 2,
                (Singleton.SCREEN_HEIGHT -ViewportManager.Get("Big_Box0").Height) / 2 + 16*13),
               ViewportManager.Get("Stage_Label"), Color.White);  
            _spriteBatch.Draw(_SpriteTexture,
                new Vector2((Singleton.SCREEN_WIDTH -ViewportManager.Get("Stage_Box").Width +ViewportManager.Get("Score_Box2").Width) / 2,
                (Singleton.SCREEN_HEIGHT -ViewportManager.Get("Big_Box0").Height) / 2 + 16*15),
               ViewportManager.Get("Stage_Box"), Color.White);  
            
            fontSize = _font.MeasureString(Singleton.Instance.Stage.ToString());
            
            _spriteBatch.DrawString(_font,Singleton.Instance.Stage.ToString(),
            new Vector2((Singleton.SCREEN_WIDTH - fontSize.X +ViewportManager.Get("Score_Box2").Width) / 2 - 8,
            (Singleton.SCREEN_HEIGHT -ViewportManager.Get("Big_Box0").Height) / 2 + 16*15 + 10),Color.White);

        }

        //draw game win
        if (Singleton.Instance.CurrentGameState == Singleton.GameState.GameWon)
        {

            _spriteBatch.Draw(_rectTexture, Vector2.Zero, new Rectangle(0, 0, Singleton.SCREEN_WIDTH, Singleton.SCREEN_HEIGHT), new Color(0, 0, 0, 100));
            
            _spriteBatch.Draw(_SpriteTexture,
                new Vector2((Singleton.SCREEN_WIDTH -ViewportManager.Get("Big_Box0").Width) / 2, (Singleton.SCREEN_HEIGHT -ViewportManager.Get("Big_Box0").Height) / 2),
               ViewportManager.Get("Big_Box0"), Color.White);
            _spriteBatch.Draw(_SpriteTexture,
                new Vector2((Singleton.SCREEN_WIDTH -ViewportManager.Get("GameWin_Label").Width) / 2, (Singleton.SCREEN_HEIGHT -ViewportManager.Get("Big_Box0").Height) / 2 + 16*5),
               ViewportManager.Get("GameWin_Label"), Color.White);
            _spriteBatch.Draw(_SpriteTexture,
                new Vector2((Singleton.SCREEN_WIDTH -ViewportManager.Get("NewGame_Label").Width) / 2, (Singleton.SCREEN_HEIGHT -ViewportManager.Get("Big_Box0").Height) / 2 + 16*18),
               ViewportManager.Get("NewGame_Label"), Color.White);

            _spriteBatch.Draw(_SpriteTexture,
                new Vector2((Singleton.SCREEN_WIDTH -ViewportManager.Get("Score_Label2").Width -ViewportManager.Get("Stage_Box").Width) / 2 - 16
                , (Singleton.SCREEN_HEIGHT -ViewportManager.Get("Big_Box0").Height) / 2 + 16*11),
               ViewportManager.Get("Score_Label2"), Color.White); 
            _spriteBatch.Draw(_SpriteTexture,
                new Vector2((Singleton.SCREEN_WIDTH -ViewportManager.Get("Score_Box2").Width -ViewportManager.Get("Stage_Box").Width) / 2 - 16
                , (Singleton.SCREEN_HEIGHT -ViewportManager.Get("Big_Box0").Height) / 2 + 16*13),
               ViewportManager.Get("Score_Box2"), Color.White);       
            
            Vector2 fontSize = _font.MeasureString(Singleton.Instance.Score.ToString());

            _spriteBatch.DrawString(_font,Singleton.Instance.Score.ToString(),
                new Vector2((Singleton.SCREEN_WIDTH - fontSize.X -ViewportManager.Get("Stage_Box").Width) / 2 - 16,
                (Singleton.SCREEN_HEIGHT -ViewportManager.Get("Big_Box0").Height) / 2 + 16*13 + 10),Color.White);

            _spriteBatch.Draw(_SpriteTexture,
                new Vector2((Singleton.SCREEN_WIDTH -ViewportManager.Get("Stage_Label").Width +ViewportManager.Get("Score_Box2").Width) / 2,
                (Singleton.SCREEN_HEIGHT -ViewportManager.Get("Big_Box0").Height) / 2 + 16*11),
               ViewportManager.Get("Stage_Label"), Color.White);  
            _spriteBatch.Draw(_SpriteTexture,
                new Vector2((Singleton.SCREEN_WIDTH -ViewportManager.Get("Stage_Box").Width +ViewportManager.Get("Score_Box2").Width) / 2,
                (Singleton.SCREEN_HEIGHT -ViewportManager.Get("Big_Box0").Height) / 2 + 16*13),
               ViewportManager.Get("Stage_Box"), Color.White);  
            
            fontSize = _font.MeasureString(Singleton.Instance.Stage.ToString());
            
            _spriteBatch.DrawString(_font,Singleton.Instance.Stage.ToString(),
            new Vector2((Singleton.SCREEN_WIDTH - fontSize.X +ViewportManager.Get("Score_Box2").Width) / 2 - 8,
            (Singleton.SCREEN_HEIGHT -ViewportManager.Get("Big_Box0").Height) / 2 + 16*13 + 10),Color.White);  

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
        Singleton.Instance.Stage = 1;
        Singleton.Instance.Money = 5;
        Singleton.Instance.waitForPlayer = true;
        Singleton.Instance.OwnedRelics = Relics.GetEmptyRelicList();
        Singleton.Instance.CurrentGameState = Singleton.GameState.InitializingStage;

        _gameObjects.Add(new Player(_SpriteTexture)
        {
            Name = "Player",
            Viewport = ViewportManager.Get("Player_Hand"),
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

    protected void ResetStage()
    {

        Singleton.Instance.Random = new System.Random();

        Singleton.Instance.CeilingPosition = 0;
        Singleton.Instance.ChipShotAmount = 0;
        Singleton.Instance.waitForPlayer = true;

        DestroyAllChips();

        _gameObjects.Clear();

        _gameObjects.Add(new Player(_SpriteTexture)
        {
            Name = "Player",
            Viewport = ViewportManager.Get("Player_Hand"),
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
            Viewport = ViewportManager.Get("Slot_Machine"),
            Position = new Vector2(_slotMachinePositionX, Singleton.SCREEN_HEIGHT/2 - ViewportManager.Get("Slot_Machine").Height/2 + 32),
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
        Stage.SetUpStage();

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
                    if (!HasChipFell)
                    {         
                        Singleton.Instance.Money += ConnectedChips.Count;
                        HasChipFell = true;
                    }
                    Console.WriteLine(ConnectedChips.Count);
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

    protected void CheckStageClear()
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
        Singleton.Instance.CurrentGameState = Singleton.GameState.StageCompleted;
    }
}
