using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Shop : GameObject
{   
    public SpriteFont font;
    private ShopRelic[] _items;
    private Relics.RelicType[] _itemsType;
    private Button _nextButton;

    private int _itemSpacing = 80;
    private int _itemSize = 32;
    private int _reRollPrice = 1;

    public Shop(Texture2D texture) : base(texture)
    {
        Reset();
    }

    public override void Reset()
    {
        _reRollPrice = 1;

        _items = new ShopRelic[3];
        _itemsType = new Relics.RelicType[3];

        // Randomize items for sale
        for (int i = 0; i < _items.Length; i++)
        {
            _itemsType[i] = Relics.GetRandomRelic();

            while (Singleton.Instance.OwnedRelics.Contains(_itemsType[i]) || _itemsType.Count(itemType => itemType == _itemsType[i]) > 1)
            {
                _itemsType[i] = Relics.GetRandomRelic();
            }

            _items[i] = new ShopRelic(_texture)
            {
                Name = string.Concat(_itemsType[i].ToString().Select((x, i) => i > 0 && char.IsUpper(x) ? " " + x : x.ToString())),
                relicType = _itemsType[i],
                Position = new Vector2(Position.X - ViewportManager.Get("Shop_Box").Width/2 + _itemSize, (_itemSpacing + _itemSize) * (i+1)),
                Rarity = Relics.GetRelicRarity(_itemsType[i]),
                Price = Relics.GetRelicPrice(_itemsType[i]),
                Descriptions = Relics.GetRelicDescriptions(_itemsType[i]),
                showDescriptions = true,
                font = font,
            };
            _items[i].Reset();
        }

        // Crate next button
        _nextButton = new Button(_texture)
        {
            Name = "NextButton",
            Viewport = ViewportManager.Get("Small_Button"),
            HighlightedViewPort = ViewportManager.Get("Small_Button_Highlighted"),
            Position = new Vector2(Position.X - ViewportManager.Get("Small_Button").Width/2,
                    Position.Y - ViewportManager.Get("Small_Button").Height / 2 + 16*12 + 8),
            LabelViewPort = ViewportManager.Get("Next_Label"),
            HighlightedLabelViewPort = ViewportManager.Get("Next_Label_Highlighted"),
            LabelPosition = new Vector2(Position.X - ViewportManager.Get("Next_Label").Width/2,
                    Position.Y - ViewportManager.Get("Next_Label").Height / 2 + 16*12 + 8),
            IsActive = true
        };

    }


    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        _nextButton.Update(gameTime, gameObjects);

        if(_nextButton.IsClicked()){
            Singleton.Instance.Stage++;
            Singleton.Instance.CurrentGameState = Singleton.GameState.InitializingStage;
        }
        
        for (int i = 0; i < _items.Length; i++)
        {
             _items[i].Update(gameTime, gameObjects);

            // Sale Item when left clicked
             if(_items[i].IsLeftClicked() && !_items[i].isSold && Singleton.Instance.Money > _items[i].Price){

                if (Singleton.Instance.OwnedRelics.Contains(Relics.RelicType.None))
                {
                    int index = Singleton.Instance.OwnedRelics.IndexOf(Relics.RelicType.None);
                    if (index != -1) Singleton.Instance.OwnedRelics[index] = _itemsType[i];
                    _items[i].isSold = true;
                    Singleton.Instance.Money -= _items[i].Price;
                }

             }
        }

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        // Draw shop border
        spriteBatch.Draw(_texture ,new Vector2(Position.X - ViewportManager.Get("Shop_Box").Width/2, Position.Y - ViewportManager.Get("Shop_Box").Height/2),
        ViewportManager.Get("Shop_Box"), Color.White);

        // Draw shop sign
        spriteBatch.Draw(_texture ,new Vector2(Position.X - ViewportManager.Get("Shop_Label").Width/2, Position.Y - ViewportManager.Get("Shop_Box").Height/2 + 16),
        ViewportManager.Get("Shop_Label"), Color.White);

        // Draw next button
        _nextButton.Draw(spriteBatch);

        // Draw sale items
        for (int i = 0; i < _itemsType.Length; i++)
        {
            _items[i].Draw(spriteBatch);
        }
    }
}
