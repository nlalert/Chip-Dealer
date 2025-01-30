using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace MidtermComGame;

public class PauseMenu
{
    private SpriteBatch _spriteBatch;
    private Rectangle _pause_0_viewport;
    private Rectangle _pause_1_viewport;
    private Rectangle _button_viewport;
    private Rectangle _button_hovered_viewport;

    private Rectangle _resume_viewport;
    private Rectangle _resume_hovered_viewport;
    private Rectangle _settings_viewport;
    private Rectangle _settings_hovered_viewport;
    private Rectangle _restart_viewport;
    private Rectangle _restart_hovered_viewport;
    private Rectangle _mainmenu_viewport;
    private Rectangle _mainmenu_hovered_viewport;

    private Rectangle _music_label_viewport;
    private Rectangle _sfx_label_viewport;

    private Rectangle _back_label_viewport;
    private Rectangle _back_label_hovered_viewport;
    private Rectangle _back_button_viewport;
    private Rectangle _back_button_hovered_viewport;

    private Rectangle _settings_box_0_viewport;
    private Rectangle _settings_box_1_viewport;
    private Rectangle _slide_bar_viewport;
    private Rectangle _slide_chip_0_viewport;
    private Rectangle _slide_chip_1_viewport;
    private Rectangle _slide_chip_2_viewport;
    private Rectangle _slide_chip_3_viewport;

    private Texture2D _texture;
    private Texture2D _rectTexture;
    private Point _mousePosition;

    private Rectangle _resumeBoundingBox;
    private Rectangle _settingsBoundingBox;
    private Rectangle _restartBoundingBox;
    private Rectangle _mainmenuBoundingBox;

    private Rectangle _backButtonBoundingBox;
    private Rectangle _slideChip0BoundingBox;
    private Rectangle _slideChip1BoundingBox;

    private int _pauseSignHeight;
    private int _resumeButtonHeight;
    private int _settingsButtonHeight;
    private int _restartButtonHeight;
    private int _mainmenuButtonHeight;

    private int _musicLabelHeight;
    private int _sfxLabelHeight;
    private int _backButtonHeight;

    private float _slideChip0Position;
    private float _slideChip1Position;

    private int _slideBarMaxValue;
    private int buttonGap;
    private int labelGap;
    private int backlabelGap;

    private bool _settings;
    private bool _slideChip0Dragging;
    private bool _slideChip1Dragging;

    public void Initialize()
    {
        Console.WriteLine("Paused");

        _settings = false;

        _pause_0_viewport = new Rectangle(0, 992, 384, 128);
        _pause_1_viewport = new Rectangle(384, 992, 384, 128);

        _button_viewport = new Rectangle(0, 1136, 304, 80);
        _button_hovered_viewport = new Rectangle(304, 1136, 304, 80);

        _resume_viewport = new Rectangle(0, 1296, 304, 48);
        _resume_hovered_viewport = new Rectangle(304, 1296, 304, 48);

        _settings_viewport = new Rectangle(0, 1232, 304, 48);
        _settings_hovered_viewport = new Rectangle(304, 1232, 304, 48);

        _restart_viewport = new Rectangle(0, 1360, 304, 48); 
        _restart_hovered_viewport = new Rectangle(304, 1360, 304, 48);

        _mainmenu_viewport = new Rectangle(0, 1424, 304, 48); 
        _mainmenu_hovered_viewport = new Rectangle(304, 1424, 304, 48);

        _settings_box_0_viewport = new Rectangle(0, 1552, 448, 352);
        _settings_box_1_viewport = new Rectangle(448, 1552, 448, 352);

        _music_label_viewport = new Rectangle(448, 1984, 176, 32);
        _sfx_label_viewport = new Rectangle(624, 1984, 112, 32);

        _back_label_viewport = new Rectangle(608, 1264, 160 ,32);
        _back_label_hovered_viewport = new Rectangle(608, 1312, 160 ,32);

        _back_button_viewport = new Rectangle(608, 1136, 160 ,48);
        _back_button_hovered_viewport = new Rectangle(608, 1200, 160 ,48);

        _slide_bar_viewport = new Rectangle(0, 1920, 448, 80);

        _slide_chip_0_viewport = new Rectangle(448, 1920, Singleton.CHIP_SIZE, Singleton.CHIP_SIZE + Singleton.CHIP_SHADOW_HEIGHT);
        _slide_chip_1_viewport = new Rectangle(480, 1920, Singleton.CHIP_SIZE, Singleton.CHIP_SIZE + Singleton.CHIP_SHADOW_HEIGHT);
        _slide_chip_2_viewport = new Rectangle(512, 1920, Singleton.CHIP_SIZE, Singleton.CHIP_SIZE + Singleton.CHIP_SHADOW_HEIGHT);
        _slide_chip_3_viewport = new Rectangle(544, 1920, Singleton.CHIP_SIZE, Singleton.CHIP_SIZE + Singleton.CHIP_SHADOW_HEIGHT);

        _slideBarMaxValue = 320;

        // Y positon of the pause sign
        _pauseSignHeight = 70;

        // Y positon of the resume button
        _resumeButtonHeight = 180;

        // a gap between each button
        buttonGap = 5;

        // a gap between labels and button border
        labelGap = 16;
        backlabelGap = 8;

        // Calculating Y position of other buttons
        for (int i = 0; i < 4; i++){
            switch (i)
            {
                case 1:
                    _restartButtonHeight = _resumeButtonHeight + (buttonGap + _button_viewport.Height)*i;
                    break;
                case 2:
                    _settingsButtonHeight = _resumeButtonHeight + (buttonGap + _button_viewport.Height)*i;                
                    break;
                case 3:
                    _mainmenuButtonHeight = _resumeButtonHeight + (buttonGap + _button_viewport.Height)*i;
                    break;    
                default:
                    break;
            }
        }

        _musicLabelHeight = (Singleton.SCREEN_HEIGHT / 2) - (Singleton.CHIP_SIZE / 2) - (_settings_box_0_viewport.Height/4) - (_slide_bar_viewport.Height/2) - buttonGap;
        _sfxLabelHeight = (Singleton.SCREEN_HEIGHT / 2) - (Singleton.CHIP_SIZE / 2) + (_settings_box_0_viewport.Height/12) - (_slide_bar_viewport.Height/2) - buttonGap;
        _backButtonHeight = (Singleton.SCREEN_HEIGHT / 2) - (Singleton.CHIP_SIZE / 2) + (_settings_box_0_viewport.Height*2/5);

        // Create bounding box for each button
        _resumeBoundingBox = new Rectangle((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _resumeButtonHeight - (_button_viewport.Height / 2), _button_viewport.Width, _button_viewport.Height);
        _settingsBoundingBox = new Rectangle((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _settingsButtonHeight - (_button_viewport.Height / 2), _button_viewport.Width, _button_viewport.Height);
        _restartBoundingBox = new Rectangle((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _restartButtonHeight - (_button_viewport.Height / 2), _button_viewport.Width, _button_viewport.Height);
        _mainmenuBoundingBox = new Rectangle((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _mainmenuButtonHeight - (_button_viewport.Height / 2), _button_viewport.Width, _button_viewport.Height);

        _backButtonBoundingBox = new Rectangle((Singleton.SCREEN_WIDTH / 2) - (_back_button_viewport.Width/ 2), _backButtonHeight - (_back_button_viewport.Height / 2), _back_button_viewport.Width, _back_button_viewport.Height);
        
        _slideChip0BoundingBox = new Rectangle((Singleton.SCREEN_WIDTH / 2) - (Singleton.CHIP_SIZE/ 2) + (int)(Singleton.Instance.Volume*_slideBarMaxValue) - (_slideBarMaxValue/2), 
            (Singleton.SCREEN_HEIGHT / 2) - (Singleton.CHIP_SIZE / 2) - (_settings_box_0_viewport.Height/4) + buttonGap, _slide_chip_0_viewport.Width, _slide_chip_0_viewport.Height);

        _slideChip1BoundingBox = new Rectangle((Singleton.SCREEN_WIDTH / 2) - (Singleton.CHIP_SIZE/ 2) + (int)(Singleton.Instance.Volume*_slideBarMaxValue) - (_slideBarMaxValue/2), 
            (Singleton.SCREEN_HEIGHT / 2) - (Singleton.CHIP_SIZE / 2) + (_settings_box_1_viewport.Height/12) + buttonGap, _slide_chip_1_viewport.Width, _slide_chip_1_viewport.Height);
    }

    public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {

        _spriteBatch = spriteBatch;

        _texture = content.Load<Texture2D>("Sprite_Sheet");

        _rectTexture = new Texture2D(graphicsDevice, 1, 1);
        Color[] data = new Color[1 * 1];
        for (int i = 0; i < data.Length; i++) data[i] = Color.White;
        _rectTexture.SetData(data);

    }

    public void Update(GameTime gameTime)
    {
        
        MouseState _currentmousestate = Singleton.Instance.CurrentMouseState;
        MouseState _previousmousestate = Singleton.Instance.PreviousMouseState;
        _mousePosition = new Point(_currentmousestate.X, _currentmousestate.Y);

        if (!_settings)
        {
            // Unpause when left clicked & released on resume button or pressed & released "Escape key"
            if ((_previousmousestate.LeftButton == ButtonState.Pressed && _currentmousestate.LeftButton == ButtonState.Released && IsMouseHovering(_resumeBoundingBox)) 
            || (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey))
            {
                Singleton.Instance.CurrentGameState = Singleton.GameState.Playing;
            }

            // Restart to stage 1 when left clicked & released on restart button
            if (_previousmousestate.LeftButton == ButtonState.Pressed && _currentmousestate.LeftButton == ButtonState.Released && IsMouseHovering(_restartBoundingBox))
            {
                Singleton.Instance.CurrentGameState = Singleton.GameState.StartingGame;
            }

            // Show settings when left clicked & released on settings button
            if (_previousmousestate.LeftButton == ButtonState.Pressed && _currentmousestate.LeftButton == ButtonState.Released && IsMouseHovering(_settingsBoundingBox))
            {
                _settings = true;
            }

            // Exit to main-menu when left clicked & released on main-menu button
            if (_previousmousestate.LeftButton == ButtonState.Pressed && _currentmousestate.LeftButton == ButtonState.Released && IsMouseHovering(_mainmenuBoundingBox))
            {
                Singleton.Instance.CurrentGameState = Singleton.GameState.MainMenu;
            }
        }

        else
        {

            // Exit to pause menu when left clicked & released on back cutton or pressed & released "Escape key"
            if (_previousmousestate.LeftButton == ButtonState.Pressed && _currentmousestate.LeftButton == ButtonState.Released && IsMouseHovering(_backButtonBoundingBox)
            || Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)
            {
                _settings = false;
            }

            // Adjust music volume by left click an drag the music slide chip
            if ((_previousmousestate.LeftButton == ButtonState.Pressed && _currentmousestate.LeftButton != ButtonState.Released && IsMouseHovering(_slideChip0BoundingBox)) || _slideChip0Dragging){

                _slideChip0Dragging = true;

                int newX = Math.Clamp(_currentmousestate.X - (_slideChip0BoundingBox.Width / 2), (Singleton.SCREEN_WIDTH / 2) - (Singleton.CHIP_SIZE/ 2) - _slideBarMaxValue/2, (Singleton.SCREEN_WIDTH / 2) - (Singleton.CHIP_SIZE/ 2) + _slideBarMaxValue/2);
                _slideChip0BoundingBox.X = newX;

                float _slideBarMinPossition = (Singleton.SCREEN_WIDTH / 2) - (Singleton.CHIP_SIZE/ 2) - (_slideBarMaxValue/2);
                _slideChip0Position = _slideChip0BoundingBox.X - _slideBarMinPossition;
                Singleton.Instance.Volume = _slideChip0Position / _slideBarMaxValue;

                if(_previousmousestate.LeftButton == ButtonState.Pressed && _currentmousestate.LeftButton == ButtonState.Released)  
                    _slideChip0Dragging = false;
            }

            // Adjust sfx volume by left click an drag the sfx slide chip
            if ((_previousmousestate.LeftButton == ButtonState.Pressed && _currentmousestate.LeftButton != ButtonState.Released && IsMouseHovering(_slideChip1BoundingBox)) || _slideChip1Dragging){

                _slideChip1Dragging = true;

                int newX = Math.Clamp(_currentmousestate.X - (_slideChip1BoundingBox.Width / 2), (Singleton.SCREEN_WIDTH / 2) - (Singleton.CHIP_SIZE/ 2) - _slideBarMaxValue/2, (Singleton.SCREEN_WIDTH / 2) - (Singleton.CHIP_SIZE/ 2) + _slideBarMaxValue/2);
                _slideChip1BoundingBox.X = newX;

                if(_previousmousestate.LeftButton == ButtonState.Pressed && _currentmousestate.LeftButton == ButtonState.Released)  
                    _slideChip1Dragging = false;
            }
        }
    }

    public void Draw(GameTime gameTime)
    {
        // Tranparent background
        _spriteBatch.Draw(_rectTexture, Vector2.Zero, new Rectangle(0, 0, Singleton.SCREEN_WIDTH, Singleton.SCREEN_HEIGHT), new Color(0, 0, 0, 150));

        if (!_settings)
        {         
            // Pause sign
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_pause_0_viewport.Width / 2), _pauseSignHeight - (_pause_0_viewport.Height / 2)), _pause_0_viewport, Color.White);

            // Resume button
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _resumeButtonHeight - (_button_viewport.Height / 2)), _button_viewport, Color.White);
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _resumeButtonHeight + labelGap - (_button_viewport.Height / 2)), _resume_viewport, Color.White);

            // Restart button
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _restartButtonHeight - (_button_viewport.Height / 2)), _button_viewport, Color.White);
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _restartButtonHeight + labelGap - (_button_viewport.Height / 2)), _restart_viewport, Color.White);
            
            // Settings button
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _settingsButtonHeight - (_button_viewport.Height / 2)), _button_viewport, Color.White);
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _settingsButtonHeight + labelGap - (_button_viewport.Height / 2)), _settings_viewport, Color.White);
            
            // Main-menu button
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _mainmenuButtonHeight - (_button_viewport.Height / 2)), _button_viewport, Color.White);
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _mainmenuButtonHeight + labelGap - (_button_viewport.Height / 2)), _mainmenu_viewport, Color.White);

            // Highlighted resume button when hovered
            if (IsMouseHovering(_resumeBoundingBox))
            {
                _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _resumeButtonHeight - (_button_viewport.Height / 2)), _button_hovered_viewport, Color.White);
                _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _resumeButtonHeight + labelGap - (_button_viewport.Height / 2)), _resume_hovered_viewport, Color.White);
            }

            // Highlighted restart button when hovered
            else if (IsMouseHovering(_restartBoundingBox))
            {
                _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _restartButtonHeight - (_button_viewport.Height / 2)), _button_hovered_viewport, Color.White);
                _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _restartButtonHeight + labelGap - (_button_viewport.Height / 2)), _restart_hovered_viewport, Color.White);
            }

            // Highlighted settings button when hovered
            else if (IsMouseHovering(_settingsBoundingBox))
            {
                _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _settingsButtonHeight - (_button_viewport.Height / 2)), _button_hovered_viewport, Color.White);
                _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _settingsButtonHeight + labelGap - (_button_viewport.Height / 2)), _settings_hovered_viewport, Color.White);
            }

            // Highlighted main-menu button when hovered
            else if (IsMouseHovering(_mainmenuBoundingBox))
            {   
                _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _mainmenuButtonHeight - (_button_viewport.Height / 2)), _button_hovered_viewport, Color.White);
                _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _mainmenuButtonHeight + labelGap - (_button_viewport.Height / 2)), _mainmenu_hovered_viewport, Color.White);
            }
        }

        else {
            // Setting Border
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_settings_box_0_viewport.Width/ 2), (Singleton.SCREEN_HEIGHT / 2) - (_settings_box_0_viewport.Height/ 2)), _settings_box_0_viewport, Color.White);
            
            // Music
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_music_label_viewport.Width/ 2), _musicLabelHeight), _music_label_viewport, Color.White);
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_slide_bar_viewport.Width/ 2), (Singleton.SCREEN_HEIGHT / 2) - (_slide_bar_viewport.Height / 2) - (_settings_box_0_viewport.Height/4) + buttonGap), _slide_bar_viewport, Color.White);
            
            // SFX
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_sfx_label_viewport.Width/ 2), _sfxLabelHeight), _sfx_label_viewport, Color.White);
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_slide_bar_viewport.Width/ 2), (Singleton.SCREEN_HEIGHT / 2) - (_slide_bar_viewport.Height / 2) + (_settings_box_0_viewport.Height/12) + buttonGap), _slide_bar_viewport, Color.White);
            
            // Back button
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_back_button_viewport.Width/ 2), _backButtonHeight - (_back_button_viewport.Height / 2)), _back_button_viewport, Color.White);
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_back_label_viewport.Width/ 2), _backButtonHeight - (_back_button_viewport.Height / 2) + backlabelGap), _back_label_viewport, Color.White);

            if (IsMouseHovering(_backButtonBoundingBox))
            {
                _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_back_button_viewport.Width/ 2), _backButtonHeight - (_back_button_viewport.Height / 2)), _back_button_hovered_viewport, Color.White);
                _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_back_label_viewport.Width/ 2), _backButtonHeight - (_back_button_viewport.Height / 2) + backlabelGap), _back_label_hovered_viewport, Color.White);
            }
            
            Rectangle slide_chip_viewport;

            // Slide chip baso on music volume
            if (Singleton.Instance.Volume <= 0){
                slide_chip_viewport = _slide_chip_0_viewport;
            }
            else if (Singleton.Instance.Volume <= 0.33){
                slide_chip_viewport = _slide_chip_1_viewport;
            }
            else if (Singleton.Instance.Volume <= 0.66){
                slide_chip_viewport = _slide_chip_2_viewport;
            }
            else
            {
                slide_chip_viewport = _slide_chip_3_viewport;                
            }

            // Slide Chip
            _spriteBatch.Draw(_texture, new Vector2(_slideChip0BoundingBox.X, (Singleton.SCREEN_HEIGHT / 2) - (Singleton.CHIP_SIZE / 2) - (_settings_box_0_viewport.Height/4) + buttonGap), slide_chip_viewport, Color.White);
            _spriteBatch.Draw(_texture, new Vector2(_slideChip1BoundingBox.X, (Singleton.SCREEN_HEIGHT / 2) - (Singleton.CHIP_SIZE / 2) + (_settings_box_1_viewport.Height/12) + buttonGap), slide_chip_viewport, Color.White);
        }
    }

    public bool IsMouseHovering(Rectangle boundingbox)
    {
        return boundingbox.Contains(_mousePosition);
    }
}
