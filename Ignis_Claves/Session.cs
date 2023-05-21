using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IgnisClaves;

public abstract class Session
{
    public IgnisGame SessionIgnisGame;
    public SpriteBatch SessionSpriteBatch;
    public abstract void Start();
    public abstract void Draw(GameTime gameTime);
    public abstract void Update(GameTime gameTime);
}