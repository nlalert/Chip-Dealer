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

    private Texture2D _texture;
    private Texture2D _rectTexture;
    private Point _mousePosition;

    private Rectangle _resumeBoundingBox;
    private Rectangle _settingsBoundingBox;
    private Rectangle _restartBoundingBox;
    private Rectangle _mainmenuBoundingBox;
    private int _resumeButtonHeight;
    private int _settingsButtonHeight;
    private int _restartButtonHeight;
    private int _mainmenuButtonHeight;
    private int buttonGap;
    private int labelGap;

    public void Initialize()
    {
        Console.WriteLine("Paused");
    }

    public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        _spriteBatch = spriteBatch;

        _texture = content.Load<Texture2D>("Sprite_Sheet");

        _rectTexture = new Texture2D(graphicsDevice, 1, 1);
        Color[] data = new Color[1 * 1];
        for (int i = 0; i < data.Length; i++) data[i] = Color.White;
        _rectTexture.SetData(data);

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

        _resumeButtonHeight = 180;

        buttonGap = 5;
        labelGap = 16;

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
        
        _resumeBoundingBox = new Rectangle((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _resumeButtonHeight - (_button_viewport.Height / 2), _button_viewport.Width, _button_viewport.Height);
        _settingsBoundingBox = new Rectangle((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _settingsButtonHeight - (_button_viewport.Height / 2), _button_viewport.Width, _button_viewport.Height);
        _restartBoundingBox = new Rectangle((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _restartButtonHeight - (_button_viewport.Height / 2), _button_viewport.Width, _button_viewport.Height);
        _mainmenuBoundingBox = new Rectangle((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _mainmenuButtonHeight - (_button_viewport.Height / 2), _button_viewport.Width, _button_viewport.Height);

    }
    public void Update(GameTime gameTime)
    {
        
        MouseState _mousestate = Singleton.Instance.CurrentMouseState;
        _mousePosition = new Point(_mousestate.X, _mousestate.Y);

        if ((_mousestate.LeftButton == ButtonState.Pressed && IsMouseHovering(_resumeBoundingBox)) || (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)){
            Singleton.Instance.CurrentGameState = Singleton.GameState.Playing;
        }

        if (_mousestate.LeftButton == ButtonState.Pressed && IsMouseHovering(_restartBoundingBox)){
            Singleton.Instance.CurrentGameState = Singleton.GameState.StartingGame;
        }

        if (_mousestate.LeftButton == ButtonState.Pressed && IsMouseHovering(_mainmenuBoundingBox)){
            Singleton.Instance.CurrentGameState = Singleton.GameState.MainMenu;
        }
    }

    public void Draw(GameTime gameTime)
    {
        _spriteBatch.Draw(_rectTexture, Vector2.Zero, new Rectangle(0, 0, Singleton.SCREEN_WIDTH, Singleton.SCREEN_HEIGHT), new Color(0, 0, 0, 150));
        _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_pause_0_viewport.Width / 2), 70 - (_pause_0_viewport.Height / 2)), _pause_0_viewport, Color.White);

        _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _resumeButtonHeight - (_button_viewport.Height / 2)), _button_viewport, Color.White);
        _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _resumeButtonHeight + labelGap - (_button_viewport.Height / 2)), _resume_viewport, Color.White);

        _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _restartButtonHeight - (_button_viewport.Height / 2)), _button_viewport, Color.White);
        _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _restartButtonHeight + labelGap - (_button_viewport.Height / 2)), _restart_viewport, Color.White);
        
        _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _settingsButtonHeight - (_button_viewport.Height / 2)), _button_viewport, Color.White);
        _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _settingsButtonHeight + labelGap - (_button_viewport.Height / 2)), _settings_viewport, Color.White);
        
        _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _mainmenuButtonHeight - (_button_viewport.Height / 2)), _button_viewport, Color.White);
        _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _mainmenuButtonHeight + labelGap - (_button_viewport.Height / 2)), _mainmenu_viewport, Color.White);

        if (IsMouseHovering(_resumeBoundingBox))
        {
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _resumeButtonHeight - (_button_viewport.Height / 2)), _button_hovered_viewport, Color.White);
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _resumeButtonHeight + labelGap - (_button_viewport.Height / 2)), _resume_hovered_viewport, Color.White);
        }

        else if (IsMouseHovering(_restartBoundingBox))
        {
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _restartButtonHeight - (_button_viewport.Height / 2)), _button_hovered_viewport, Color.White);
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _restartButtonHeight + labelGap - (_button_viewport.Height / 2)), _restart_hovered_viewport, Color.White);
        }

        else if (IsMouseHovering(_settingsBoundingBox))
        {
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _settingsButtonHeight - (_button_viewport.Height / 2)), _button_hovered_viewport, Color.White);
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _settingsButtonHeight + labelGap - (_button_viewport.Height / 2)), _settings_hovered_viewport, Color.White);
        }

        else if (IsMouseHovering(_mainmenuBoundingBox))
        {   
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _mainmenuButtonHeight - (_button_viewport.Height / 2)), _button_hovered_viewport, Color.White);
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (_button_viewport.Width/ 2), _mainmenuButtonHeight + labelGap - (_button_viewport.Height / 2)), _mainmenu_hovered_viewport, Color.White);
        }
    }

    public bool IsMouseHovering(Rectangle boundingbox)
    {
        return boundingbox.Contains(_mousePosition);
    }
}
