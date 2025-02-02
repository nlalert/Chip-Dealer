using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class ShopRelic : GameObject
{

    public SpriteFont font;
    public int Price;
    public int Rarity;
    public string Descriptions;
    public Vector2 DescriptionsPosition;

    public Relics.RelicType relicType;
    public bool isSold = false;
    public bool isOwned = false;

    public bool showDescriptions;

    private Color priceColor;

    public ShopRelic(Texture2D texture) : base(texture)
    {

    }

    public override void Reset(){
        DescriptionsPosition = new Vector2(Position.X + 16*4, Position.Y - 16*2);
        priceColor = Color.White;
        if(Singleton.Instance.Money < Price+1) priceColor = Color.Red;

        Viewport = ViewportManager.Get(Name.Replace(" ", "_"));
    } 

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        priceColor = Color.White;
        if(IsMouseHoveringForInfo()) priceColor = Color.Yellow;
        if(Singleton.Instance.Money < Price && !Singleton.Instance.OwnedRelics.Contains(relicType)) priceColor = Color.Red;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {

        if(isSold)
        {
            spriteBatch.Draw(_texture, Position, ViewportManager.Get("Placeholder"), Color.White);

            if(showDescriptions){
                spriteBatch.Draw(_texture, DescriptionsPosition, ViewportManager.Get("Relic_Box" + Rarity), Color.White);

                spriteBatch.DrawString(font, Name,new Vector2(DescriptionsPosition.X+8, DescriptionsPosition.Y+8),Color.White, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font,"\nPurchased",new Vector2(DescriptionsPosition.X+8, DescriptionsPosition.Y+8),Color.Gray, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font,"\n\n\n" + Descriptions,new Vector2(DescriptionsPosition.X+8, DescriptionsPosition.Y+8),Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0);

                spriteBatch.Draw(_texture, DescriptionsPosition, ViewportManager.Get("Relic_Box_Sold"), Color.White);
            }
        }

        else
        {
            
            spriteBatch.Draw(_texture, Position, Viewport, Color.White);

            if(showDescriptions){
                spriteBatch.Draw(_texture, DescriptionsPosition, ViewportManager.Get("Relic_Box" + Rarity), Color.White);

                spriteBatch.DrawString(font, Name,new Vector2(DescriptionsPosition.X+8, DescriptionsPosition.Y+8),Color.White, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);

                if (IsMouseHoveringForInfo())
                {
                    spriteBatch.DrawString(font,"\nSell for " + Price/4 + " $",new Vector2(DescriptionsPosition.X+8, DescriptionsPosition.Y+8),priceColor, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);

                    if (Singleton.Instance.OwnedRelics.Contains(relicType))
                        spriteBatch.DrawString(font,"(right click to sell)",new Vector2(DescriptionsPosition.X+8, DescriptionsPosition.Y + ViewportManager.Get("Relic_Box" + Rarity).Height-16),
                        Color.LightGray, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0);
                }

                else
                {       
                    spriteBatch.DrawString(font,"\n" + Price + " $",new Vector2(DescriptionsPosition.X+8, DescriptionsPosition.Y+8),priceColor, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
                }

                spriteBatch.DrawString(font,"\n\n\n" + Descriptions,new Vector2(DescriptionsPosition.X+8, DescriptionsPosition.Y+8),Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0);

                if (IsMouseHovering() && !IsMouseHoveringForInfo()) 
                {
                    spriteBatch.Draw(_texture, DescriptionsPosition, ViewportManager.Get("Relic_Box_Highlighted" + Rarity), Color.White);
                    spriteBatch.DrawString(font,"(click to purchase)",new Vector2(DescriptionsPosition.X+8, DescriptionsPosition.Y + ViewportManager.Get("Relic_Box" + Rarity).Height-16),
                        Color.LightGray, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0);
                }
            }
        }
        
        base.Draw(spriteBatch);
    }

    public bool IsLeftClicked()
    {
        Rectangle buttonBounds = new Rectangle((int)DescriptionsPosition.X, (int)DescriptionsPosition.Y, 
        ViewportManager.Get("Relic_Box" + Rarity).Width,ViewportManager.Get("Relic_Box" + Rarity).Height);
        return buttonBounds.Contains(Singleton.Instance.CurrentMouseState.Position) && Singleton.Instance.PreviousMouseState.LeftButton == ButtonState.Pressed 
        && Singleton.Instance.CurrentMouseState.LeftButton == ButtonState.Released;
    }

    public bool IsRightClicked()
    {
        Rectangle buttonBounds = new Rectangle((int)Position.X, (int)Position.Y, 
        ViewportManager.Get("Red_Chip").Width,ViewportManager.Get("Red_Chip").Height);
        return buttonBounds.Contains(Singleton.Instance.CurrentMouseState.Position) && Singleton.Instance.PreviousMouseState.RightButton == ButtonState.Pressed 
        && Singleton.Instance.CurrentMouseState.RightButton == ButtonState.Released;
    }

    public bool IsMouseHoveringForInfo()
    {
        Rectangle buttonBounds = new Rectangle((int)Position.X, (int)Position.Y, 
        ViewportManager.Get("Red_Chip").Width,ViewportManager.Get("Red_Chip").Height);
        return buttonBounds.Contains(Singleton.Instance.CurrentMouseState.Position);
    }
    
    public bool IsMouseHovering()
    {
        Rectangle buttonBounds = new Rectangle((int)DescriptionsPosition.X, (int)DescriptionsPosition.Y, 
        ViewportManager.Get("Relic_Box" + Rarity).Width,ViewportManager.Get("Relic_Box" + Rarity).Height);
        return buttonBounds.Contains(Singleton.Instance.CurrentMouseState.Position);
    }
}