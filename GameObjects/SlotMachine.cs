using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


class SlotMachine : GameObject
{  
    public ChipType[] _chipsDisplay;
    public Button _handle;
    public Button _chipReward;

    public ChipType _currentChipRewardType;
    public ChipType _nextChipRewardType;

    public Vector2 chipDisplay0Position;
    public Vector2 chipDisplay1Position;
    public Vector2 chipDisplay2Position;

    public Vector2[] chipDisplayPositions;

    public Vector2 slot0Position;
    public Vector2 slot1Position;
    public Vector2 slot2Position;

    
    public Vector2[] slotPositions;
    public Rectangle[] slotBorders;

    public float handleMinHeight;
    public float handleMaxHeight;

    public int slotSpinSpeed;
    public float handleResetSpeed;

    public float winningChances; 
    public bool onCooldown;
    public SoundEffect LosingBetSound;
    public SoundEffect WinningBetSound;

    public SlotMachine(Texture2D texture) : base(texture)
    {
        
    }

    public override void Reset(){

        handleMinHeight = Position.Y + 95;
        handleMaxHeight = handleMinHeight + 34;

        slotBorders = new Rectangle[3];
        slotPositions = new Vector2[3];
        chipDisplayPositions = new Vector2[3];
        _chipsDisplay = new ChipType[3];

        for (int i = 0; i < slotBorders.Length; i++)
        {
            slotBorders[i] = new Rectangle(Singleton.GetViewPortFromSpriteSheet("Slot_Background").X,Singleton.GetViewPortFromSpriteSheet("Slot_Background").Y,48,50);

            slotPositions[i] = new Vector2(Position.X + 8 + i*48, Position.Y + 23);

            chipDisplayPositions[i] = new Vector2(Position.X + 16 + i*48,Position.Y + 32);

            _chipsDisplay[i] = (ChipType)Singleton.Instance.Random.Next(1,9);
            while (_chipsDisplay[2] == _chipsDisplay[1] && _chipsDisplay[2] == _chipsDisplay[1])
                {
                    _chipsDisplay[2] = (ChipType)Singleton.Instance.Random.Next(1,9);
                }

        }

        _currentChipRewardType = ChipType.None;

        chipDisplay0Position = new Vector2(Position.X + 16,Position.Y + 32);
        chipDisplay1Position = chipDisplay0Position + new Vector2(48, 0);
        chipDisplay2Position = chipDisplay1Position + new Vector2(48, 0);

        slot0Position = new Vector2(Position.X + 8,Position.Y + 23);
        slot1Position = slot0Position + new Vector2(48, 0);
        slot2Position = slot1Position + new Vector2(48, 0);

        winningChances = 0.20f;
        slotSpinSpeed = 10;
        handleResetSpeed = 1.0f;

        _handle = new Button(_texture){
            Name = "SlotHandle",
            Viewport = Singleton.GetViewPortFromSpriteSheet("Slot_Handle"),
            Position = new Vector2 (Position.X + 8, handleMinHeight),
            IsActive = true

        };

        _chipReward = new Button(_texture){
            Name = "ChipReward",
            Position = new Vector2 (Position.X + 112, Position.Y + 102),
            IsActive = true
        };
        
    }

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        _handle.Update(gameTime, gameObjects);

        if (_handle.Position.Y > handleMinHeight)
        {
            _handle.Position.Y -= handleResetSpeed;
        }

        if (_handle.Dragging && !onCooldown)
        {
            int newY = Math.Clamp(Singleton.Instance.CurrentMouseState.Y - (_handle.Viewport.Height / 2), (int)handleMinHeight, (int)handleMaxHeight);
            _handle.Position.Y = newY;
        }

        if (!_handle.Dragging && _handle.Position.Y == handleMaxHeight-handleResetSpeed && !onCooldown){
            onCooldown = true;

            _chipsDisplay = new ChipType[3];

            if ((float)Singleton.Instance.Random.Next(0,101)/100 <= winningChances)
            {
            Console.WriteLine("You win!");
            WinningBetSound.Play();
            _nextChipRewardType = (ChipType)Singleton.Instance.Random.Next(1,9);
            }
            else
            {Console.WriteLine("Aw.. Dang it!");
            LosingBetSound.Play();
            _nextChipRewardType = ChipType.None;
            }

        }

        if (!_handle.Dragging && _handle.Position.Y == handleMinHeight && onCooldown){
            onCooldown = false;

            if (_nextChipRewardType != ChipType.None){

                _currentChipRewardType = _nextChipRewardType;

                if (_currentChipRewardType != ChipType.None)
                {
                    _chipReward = new Button(_texture)
                    {
                        Viewport = Singleton.GetViewPortFromSpriteSheet(_currentChipRewardType.ToString() + "_Chip"),
                        Position = new Vector2 (Position.X + 112, Position.Y + 102),
                    };
                }
            }
        }

        if (_chipReward.IsClicked() && _currentChipRewardType != ChipType.None)
        {
            Singleton.Instance.CurrentChip = _currentChipRewardType;

            _currentChipRewardType = ChipType.None;

            _chipReward = new Button(_texture){
                Name = "ChipReward",
                Position = new Vector2 (Position.X + 112, Position.Y + 102),
                IsActive = true,
            };
        }

        if(onCooldown)
        {
            if(_nextChipRewardType != ChipType.None)
            {

                if (_handle.Position.Y == handleMinHeight + 34*2/3 && _chipsDisplay[0] == ChipType.None){
                    _chipsDisplay[0] = _nextChipRewardType;
                }
                else if (_handle.Position.Y == handleMinHeight + 34*1/3 && _chipsDisplay[1] == ChipType.None){
                    _chipsDisplay[1] = _nextChipRewardType;
                }
                else if (_handle.Position.Y == handleMinHeight + 1 && _chipsDisplay[2] == ChipType.None){
                    _chipsDisplay[2] = _nextChipRewardType;
                }
            }

            else
            {
                if (_handle.Position.Y == handleMinHeight + 34*2/3 && _chipsDisplay[0] == ChipType.None){
                    _chipsDisplay[0] = (ChipType)Singleton.Instance.Random.Next(1,9);
                }
                else if (_handle.Position.Y == handleMinHeight + 34*1/3 && _chipsDisplay[1] == ChipType.None){
                    _chipsDisplay[1] = (ChipType)Singleton.Instance.Random.Next(1,9);
                }
                else if (_handle.Position.Y == handleMinHeight + 1 && _chipsDisplay[2] == ChipType.None){
                    _chipsDisplay[2] = (ChipType)Singleton.Instance.Random.Next(1,9);
                    while (_chipsDisplay[2] == _chipsDisplay[1] && _chipsDisplay[2] == _chipsDisplay[1])
                    {
                        _chipsDisplay[2] = (ChipType)Singleton.Instance.Random.Next(1,9);
                    }
                }
            }

        }

        for (int i = 0; i < slotBorders.Length; i++)
        {
            if (_chipsDisplay[i] == ChipType.None)
            {   
                slotBorders[i].Y += slotSpinSpeed;
                if (slotBorders[i].Y - Singleton.GetViewPortFromSpriteSheet("Slot_Background").Y 
                >= Singleton.GetViewPortFromSpriteSheet("Slot_Background").Height - slotBorders[i].Height)
                {
                    slotBorders[i].Y = Singleton.GetViewPortFromSpriteSheet("Slot_Background").Y;
                }
            }
            else
            {
                slotBorders[i].Y = Singleton.GetViewPortFromSpriteSheet("Slot_Background").Y;
            }
        }

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < slotPositions.Length; i++)
        {
            spriteBatch.Draw(_texture, slotPositions[i], slotBorders[i], Color.White);
        }

        spriteBatch.Draw(_texture, Position, Viewport, Color.White);

        for (int i = 0; i < chipDisplayPositions.Length; i++)
        {
            if (_chipsDisplay[i] != ChipType.None)
                spriteBatch.Draw(_texture, chipDisplayPositions[i], Singleton.GetViewPortFromSpriteSheet(_chipsDisplay[i].ToString() + "_Chip"), Color.White);
        }

        _handle.Draw(spriteBatch);
        
        _chipReward.Draw(spriteBatch);
    }


}