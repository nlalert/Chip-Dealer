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
    private Rectangle _slideChip0BoundingBox;
    private Rectangle _slideChip1BoundingBox;

    private int _pauseSignHeight;
    private int _resumeButtonHeight;
    private int _settingsButtonHeight;
    private int _restartButtonHeight;
    private int _mainmenuButtonHeight;
    private float _slideChip0Position;
    private float _slideChip1Position;


    private int _slideBarMinValue;
    private int _slideBarMaxValue;
    private int buttonGap;
    private int labelGap;

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

        _slide_bar_viewport = new Rectangle(0, 1920, 448, 80);

        _slide_chip_0_viewport = new Rectangle(448, 1920, Singleton.CHIP_SIZE, Singleton.CHIP_SIZE + Singleton.CHIP_SHADOW_HEIGHT);
        _slide_chip_1_viewport = new Rectangle(480, 1920, Singleton.CHIP_SIZE, Singleton.CHIP_SIZE + Singleton.CHIP_SHADOW_HEIGHT);
        _slide_chip_2_viewport = new Rectangle(512, 1920, Singleton.CHIP_SIZE, Singleton.CHIP_SIZE + Singleton.CHIP_SHADOW_HEIGHT);
        _slide_chip_3_viewport = new Rectangle(544, 1920, Singleton.CHIP_SIZE, Singleton.CHIP_SIZE + Singleton.CHIP_SHADOW_HEIGHT);

        _slideBarMinValue = 0;
        _slideBarMaxValue = 320;

        _slideChip0Position = Singleton.Instance.Volume * _slideBarMaxValue;

        _slideChip0BoundingBox = new Rectangle((Singleton.SCREEN_WIDTH / 2) - (Singleton.CHIP_SIZE/ 2) + _slideBarMaxValue - (_slideBarMaxValue/2), 
            (Singleton.SCREEN_HEIGHT / 2) - (Singleton.CHIP_SIZE / 2) - (_settings_box_0_viewport.Height/5), _slide_chip_0_viewport.Width, _slide_chip_0_viewport.Height);

        // Y positon of the pause sign
        _pauseSignHeight = 70;

        // Y positon of the resume button
        _resumeButtonHeight = 180;

        // a gap between each button
        buttonGap = 5;
        // a gap between label and button border
        labelGap = 16;

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

        // Create bounding box for eachh button
        _resumeBoundingBox = new Rectangle((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _resumeButtonHeight - (_button_viewport.Height / 2), _button_viewport.Width, _button_viewport.Height);
        _settingsBoundingBox = new Rectangle((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _settingsButtonHeight - (_button_viewport.Height / 2), _button_viewport.Width, _button_viewport.Height);
        _restartBoundingBox = new Rectangle((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _restartButtonHeight - (_button_viewport.Height / 2), _button_viewport.Width, _button_viewport.Height);
        _mainmenuBoundingBox = new Rectangle((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _mainmenuButtonHeight - (_button_viewport.Height / 2), _button_viewport.Width, _button_viewport.Height);
        
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
            // Unpause when clicked & released "left mouse button" on resume button or pressed & released "Escape key"
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


            if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)
            {
                _settings = false;
            }

            if ((_previousmousestate.LeftButton == ButtonState.Pressed && _currentmousestate.LeftButton != ButtonState.Released && IsMouseHovering(_slideChip0BoundingBox)) || _slideChip0Dragging){

                _slideChip0Dragging = true;

                int newX = Math.Clamp(_currentmousestate.X - (_slideChip0BoundingBox.Width / 2), (Singleton.SCREEN_WIDTH / 2) - (Singleton.CHIP_SIZE/ 2) - _slideBarMaxValue/2, (Singleton.SCREEN_WIDTH / 2) - (Singleton.CHIP_SIZE/ 2) + _slideBarMaxValue/2);

                _slideChip0BoundingBox.X = newX;

                float _slideBarMinPossition = (Singleton.SCREEN_WIDTH / 2) - (Singleton.CHIP_SIZE/ 2) - (_slideBarMaxValue/2);

                _slideChip0Position = _slideChip0BoundingBox.X - _slideBarMinPossition;

                Singleton.Instance.Volume = _slideChip0Position / _slideBarMaxValue;

                Console.WriteLine(Singleton.Instance.Volume);

                if(_previousmousestate.LeftButton == ButtonState.Pressed && _currentmousestate.LeftButton == ButtonState.Released) _slideChip0Dragging = false;
            }
        }
    }

    public void Draw(GameTime gameTime)
    {
        // Draw tranparent background
        _spriteBatch.Draw(_rectTexture, Vector2.Zero, new Rectangle(0, 0, Singleton.SCREEN_WIDTH, Singleton.SCREEN_HEIGHT), new Color(0, 0, 0, 150));

        if (!_settings)
        {         
            // Draw pause sign
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_pause_0_viewport.Width / 2), _pauseSignHeight - (_pause_0_viewport.Height / 2)), _pause_0_viewport, Color.White);

            // Draw resume button
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _resumeButtonHeight - (_button_viewport.Height / 2)), _button_viewport, Color.White);
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _resumeButtonHeight + labelGap - (_button_viewport.Height / 2)), _resume_viewport, Color.White);

            // Draw restart button
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _restartButtonHeight - (_button_viewport.Height / 2)), _button_viewport, Color.White);
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _restartButtonHeight + labelGap - (_button_viewport.Height / 2)), _restart_viewport, Color.White);
            
            // Draw settings button
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _settingsButtonHeight - (_button_viewport.Height / 2)), _button_viewport, Color.White);
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _settingsButtonHeight + labelGap - (_button_viewport.Height / 2)), _settings_viewport, Color.White);
            
            //Draw main-menu button
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _mainmenuButtonHeight - (_button_viewport.Height / 2)), _button_viewport, Color.White);
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _mainmenuButtonHeight + labelGap - (_button_viewport.Height / 2)), _mainmenu_viewport, Color.White);

            //Draw highlighted resume button when hovered
            if (IsMouseHovering(_resumeBoundingBox))
            {
                _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _resumeButtonHeight - (_button_viewport.Height / 2)), _button_hovered_viewport, Color.White);
                _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _resumeButtonHeight + labelGap - (_button_viewport.Height / 2)), _resume_hovered_viewport, Color.White);
            }

            //Draw highlighted restart button when hovered
            else if (IsMouseHovering(_restartBoundingBox))
            {
                _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _restartButtonHeight - (_button_viewport.Height / 2)), _button_hovered_viewport, Color.White);
                _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _restartButtonHeight + labelGap - (_button_viewport.Height / 2)), _restart_hovered_viewport, Color.White);
            }

            //Draw highlighted settings button when hovered
            else if (IsMouseHovering(_settingsBoundingBox))
            {
                _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _settingsButtonHeight - (_button_viewport.Height / 2)), _button_hovered_viewport, Color.White);
                _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _settingsButtonHeight + labelGap - (_button_viewport.Height / 2)), _settings_hovered_viewport, Color.White);
            }

            //Draw highlighted main-menu button when hovered
            else if (IsMouseHovering(_mainmenuBoundingBox))
            {   
                _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _mainmenuButtonHeight - (_button_viewport.Height / 2)), _button_hovered_viewport, Color.White);
                _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _mainmenuButtonHeight + labelGap - (_button_viewport.Height / 2)), _mainmenu_hovered_viewport, Color.White);
            }
        }

        else {
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_settings_box_0_viewport.Width/ 2), (Singleton.SCREEN_HEIGHT / 2) - (_settings_box_0_viewport.Height/ 2)), _settings_box_0_viewport, Color.White);
            
            // Music
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_slide_bar_viewport.Width/ 2), (Singleton.SCREEN_HEIGHT / 2) - (_slide_bar_viewport.Height / 2) - (_settings_box_0_viewport.Height/5)), _slide_bar_viewport, Color.White);
            
            // SFX
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_slide_bar_viewport.Width/ 2), (Singleton.SCREEN_HEIGHT / 2) - (_slide_bar_viewport.Height / 2) + (_settings_box_0_viewport.Height/5)), _slide_bar_viewport, Color.White);
            
            Rectangle slide_chip_viewport;

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


            _spriteBatch.Draw(_texture, new Vector2(_slideChip0BoundingBox.X, (Singleton.SCREEN_HEIGHT / 2) - (Singleton.CHIP_SIZE / 2) - (_settings_box_0_viewport.Height/5)), slide_chip_viewport, Color.White);

            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (Singleton.CHIP_SIZE/ 2) + (Singleton.Instance.Volume * _slideBarMaxValue) - (_slideBarMaxValue/2), 
            (Singleton.SCREEN_HEIGHT / 2) - (Singleton.CHIP_SIZE / 2) + (_settings_box_0_viewport.Height/5)), slide_chip_viewport, Color.White);
        }
    }

    public bool IsMouseHovering(Rectangle boundingbox)
    {
        return boundingbox.Contains(_mousePosition);
    }
}
