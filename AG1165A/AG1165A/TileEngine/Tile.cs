#region Using Statements
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
#endregion

namespace AG1165A
{
    #region Collision Type

    public enum Collision
    {
        Passable,
        Impassable,
        Collectible,
    }

    #endregion

    #region Tile Type

    public enum Type
    {
        Null,
        Counter_Top_Utensils,
        Counter_Top_Corner,
        Door,
        Fridge,
        Kitchen_Sink,
        Hob,
        Microwave,
        Wall_Cavity,
        Wall_Partition,
        Armchair,
        Armchair_Left,
        Armchair_Right,
        Television,
        Plant,
        Toilet,
        Bookshelf,
        Bathroom_Sink,
        Bathroom_Shelf,
        Bath_Top,
        Bath_Bottom,
        Bed_Top_Left,
        Bed_Top_Right,
        Bed_Bottom_Left,
        Bed_Bottom_Right,
        Table_Top_Left,
        Table_Top_Center,
        Table_Top_Right,
        Table_Bottom_Left,
        Table_Bottom_Center,
        Table_Bottom_Right,
    }

    #endregion

    public struct Tile
    {
        #region Public Members

        public const int Width = 128;
        public const int Height = 128;

        public static readonly Vector2 Size = new Vector2(Width, Height);

        public Texture2D Texture;
        public Rectangle Source;
        public Rectangle BoundingRectangle;
        public float Rotation;
        public SpriteEffects Effect;
        public Collision Collision;
        public Type Type;
        public int Damage;

        #endregion

        #region Initialisation

        public Tile(ref Texture2D texture, Rectangle source, float rotation, Rectangle boundary,
            SpriteEffects effect, Collision collision, Type type, int damage)
        {
            Texture = texture;
            Source = source;
            Rotation = MathHelper.ToRadians(rotation);
            BoundingRectangle = boundary;
            Effect = effect;
            Collision = collision;
            Type = type;
            Damage = damage;
        }

        #endregion
    }
}