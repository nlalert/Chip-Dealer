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

    public ChipType ChipType;
    public bool isSold = false;

    private Color priceColor;
    private Vector2 DescriptionsPosition;

    public ShopRelic(Texture2D texture) : base(texture)
    {

    }

    public override void Reset(){
        DescriptionsPosition = new Vector2(Position.X + 16*4, Position.Y - 16*2);
        priceColor = Color.White;
        if(Singleton.Instance.Money < Price+1) priceColor = Color.Red;
    } 

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {

    }

    public override void Draw(SpriteBatch spriteBatch)
    {

        if(isSold)
        {
            spriteBatch.Draw(_texture, Position, Singleton.GetViewPortFromSpriteSheet("Placeholder"), Color.White);
            spriteBatch.Draw(_texture, DescriptionsPosition, Singleton.GetViewPortFromSpriteSheet("Relic_Box" + Rarity), Color.White);

            spriteBatch.DrawString(font, Name,new Vector2(DescriptionsPosition.X+8, DescriptionsPosition.Y+8),Color.White, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
            spriteBatch.DrawString(font,"\nPurchased",new Vector2(DescriptionsPosition.X+8, DescriptionsPosition.Y+8),Color.Gray, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
            spriteBatch.DrawString(font,"\n\n\n" + Descriptions,new Vector2(DescriptionsPosition.X+8, DescriptionsPosition.Y+8),Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0);

            spriteBatch.Draw(_texture, DescriptionsPosition, Singleton.GetViewPortFromSpriteSheet("Relic_Box_Sold"), Color.White);
        }

        else
        {
            spriteBatch.Draw(_texture, Position, Singleton.GetViewPortFromSpriteSheet(Name.Replace(" ", "_")), Color.White);
            spriteBatch.Draw(_texture, DescriptionsPosition, Singleton.GetViewPortFromSpriteSheet("Relic_Box" + Rarity), Color.White);

            spriteBatch.DrawString(font, Name,new Vector2(DescriptionsPosition.X+8, DescriptionsPosition.Y+8),Color.White, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
            spriteBatch.DrawString(font,"\n" + Price + " $",new Vector2(DescriptionsPosition.X+8, DescriptionsPosition.Y+8),priceColor, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
            spriteBatch.DrawString(font,"\n\n\n" + Descriptions,new Vector2(DescriptionsPosition.X+8, DescriptionsPosition.Y+8),Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0);
            if (IsMouseHovering()) 
            {
                spriteBatch.Draw(_texture, DescriptionsPosition, Singleton.GetViewPortFromSpriteSheet("Relic_Box_Highlighted" + Rarity), Color.White);
            }
        }
        
        base.Draw(spriteBatch);
    }

    public bool IsClicked()
    {
        Rectangle buttonBounds = new Rectangle((int)DescriptionsPosition.X, (int)DescriptionsPosition.Y, 
        Singleton.GetViewPortFromSpriteSheet("Relic_Box" + Rarity).Width,Singleton.GetViewPortFromSpriteSheet("Relic_Box" + Rarity).Height);
        return buttonBounds.Contains(Singleton.Instance.CurrentMouseState.Position) && Singleton.Instance.PreviousMouseState.LeftButton == ButtonState.Pressed 
        && Singleton.Instance.CurrentMouseState.LeftButton == ButtonState.Released;
    }
    
    public bool IsMouseHovering()
    {
        Rectangle buttonBounds = new Rectangle((int)DescriptionsPosition.X, (int)DescriptionsPosition.Y, 
        Singleton.GetViewPortFromSpriteSheet("Relic_Box" + Rarity).Width,Singleton.GetViewPortFromSpriteSheet("Relic_Box" + Rarity).Height);
        return buttonBounds.Contains(Singleton.Instance.CurrentMouseState.Position);
    }
}