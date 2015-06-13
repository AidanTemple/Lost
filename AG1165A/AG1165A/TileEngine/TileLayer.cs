#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace AG1165A
{
    public class TileLayer
    {
        #region Private Members

        private GraphicsDevice m_Graphics;

        private Tile[,] m_Tiles;

        private Level m_Level;

        private Vector2 m_CameraPosition;

        private List<Rectangle> m_BoundingBoxes;

        #endregion

        #region Properties

        public int Width
        {
            get { return m_Tiles.GetLength(0); }
        }

        public int Height
        {
            get { return m_Tiles.GetLength(1); }
        }

        public Tile[,] Tiles
        {
            get { return m_Tiles; }
            set { m_Tiles = value; }
        }

        public List<Rectangle> BoundingBoxes
        {
            get { return m_BoundingBoxes; }
            set { m_BoundingBoxes = value; }
        }

        #endregion

        #region Initialisation

        public TileLayer(Stream stream, GraphicsDevice graphics, Level level, Vector2 cameraPosition)
        {
            m_Graphics = graphics;
            m_Level = level;
            m_CameraPosition = cameraPosition;

            m_BoundingBoxes = new List<Rectangle>();

            LoadLayer(stream);
        }

        private void LoadLayer(Stream stream)
        {
            List<string> lines = new List<string>();
            int width;

            using (StreamReader reader = new StreamReader(stream))
            {
                string line = reader.ReadLine();
                width = line.Length / 2;

                while (line != null)
                {
                    lines.Add(line);

                    if (line.Length / 2 != width)
                        Console.WriteLine(string.Format("Line {0} is not the correct length", lines.Count));

                    line = reader.ReadLine();
                }
            }

            m_Tiles = new Tile[width, lines.Count];

            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    string type = lines[y].Substring(x * 2, 2);
                    m_Tiles[x, y] = LoadTile(type, x, y);
                }
            }
        }

        private Tile LoadTile(string type, int x, int y)
        {
            var tiles = ContentManager.Tiles;
            var objects = ContentManager.Objects;

            switch (type)
            {
                case "..":
                    return new Tile();

                case "01":
                    return LoadPlayer(PlayerIndex.One, 0, x, y);

                case "02":
                    return LoadPlayer(PlayerIndex.Two, 1, x, y);

                case "03":
                    return LoadBoostCollectible(x, y);

                #region Wall Tiles

                case "11":
                    return new Tile(ref tiles, ContentManager.Stairs_Bottom, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Null, -1);

                case "12":
                    return new Tile(ref tiles, ContentManager.Stairs_Middle, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Null, -1);

                case "13":
                    return new Tile(ref tiles, ContentManager.Stairs_Top, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Null, -1);

                case "W0":
                    return new Tile(ref tiles, ContentManager.Wall_Left, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Null, -1);

                case "W1":
                    return new Tile(ref tiles, ContentManager.Wall_Left, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.FlipHorizontally, Collision.Impassable, Type.Null, -1);

                case "W2":
                    return new Tile(ref tiles, ContentManager.Wall_Left, 90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Null, -1);

                case "W3":
                    return new Tile(ref tiles, ContentManager.Wall_Left, -90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Null, -1);

                case "W4":
                    return new Tile(ref tiles, ContentManager.Wall_Cavity, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Null, -1);

                case "W5":
                    return new Tile(ref tiles, ContentManager.Wall_Cavity, 90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Null, -1);

                case "W6": // Left
                        return new Tile(ref tiles, ContentManager.Wall_Partition, 0f,
                            new Rectangle((x * Tile.Width), (y * Tile.Height), 30, Tile.Height),
                            SpriteEffects.None, Collision.Impassable, Type.Wall_Partition, -1);

                case "W7": // Right
                    return new Tile(ref tiles, ContentManager.Wall_Partition, 0f,
                        new Rectangle((x * Tile.Width) + 98, (y * Tile.Height), 30, Tile.Height),
                        SpriteEffects.FlipHorizontally, Collision.Impassable, Type.Wall_Partition, -1);

                case "W8": // Top
                    return new Tile(ref tiles, ContentManager.Wall_Partition, 90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, 30),
                        SpriteEffects.None, Collision.Impassable, Type.Wall_Partition, -1);

                case "W9": // Bottom
                    return new Tile(ref tiles, ContentManager.Wall_Partition, -90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height) + 98, Tile.Width, 30),
                        SpriteEffects.None, Collision.Impassable, Type.Wall_Partition, -1);

                case "10":
                    return new Tile(ref tiles, ContentManager.Wall_Corner, 180f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Null, -1);

                case "|0": // Left Top
                    return new Tile(ref tiles, ContentManager.Window_Frame_Top, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Null, -1);

                case "|1": // Left Bottom
                    return new Tile(ref tiles, ContentManager.Window_Frame_Top, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.FlipVertically, Collision.Impassable, Type.Null, -1);

                case "|2": // Right Top
                    return new Tile(ref tiles, ContentManager.Window_Frame_Top, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.FlipHorizontally, Collision.Impassable, Type.Null, -1);

                case "|3": // Right Bottom
                    return new Tile(ref tiles, ContentManager.Window_Frame_Top, 180f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Null, -1);

                case "|4": // Top Right
                    return new Tile(ref tiles, ContentManager.Window_Frame_Top, 90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Null, -1);

                case "|5": // Top Left
                    return new Tile(ref tiles, ContentManager.Window_Frame_Top, 90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.FlipHorizontally, Collision.Impassable, Type.Null, -1);

                case "|6": // Bottom Right
                    return new Tile(ref tiles, ContentManager.Window_Frame_Top, 90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.FlipHorizontally, Collision.Impassable, Type.Null, -1);

                case "|7": // Bottom Left
                    return new Tile(ref tiles, ContentManager.Window_Frame_Top, -90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Null, -1);

                case "D0":
                    return new Tile(ref tiles, ContentManager.Door, 180f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Null, -1);

                case "D1":
                    return new Tile(ref tiles, ContentManager.Door, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Null, -1);

                case "D2":
                    return new Tile(ref tiles, ContentManager.Door, 90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Null, -1);

                case "D3":
                    return new Tile(ref tiles, ContentManager.Door, -90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Null, -1);

                case "D4":
                    return new Tile(ref tiles, ContentManager.Door_Left, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Null, -1);
                    
                case "D5":
                    return new Tile(ref tiles, ContentManager.Door_Right, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Null, -1);

                #endregion

                #region Objects

                case "H0": // Left
                    return new Tile(ref objects, ContentManager.Hob, 0f,
                        new Rectangle(x * Tile.Width, y * Tile.Width, Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Hob, 1);

                case "H1": // Right
                    return new Tile(ref objects, ContentManager.Hob, 180f,
                        new Rectangle(x * Tile.Width, y * Tile.Width, Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Hob, 1);

                case "H2": // Top
                    return new Tile(ref objects, ContentManager.Hob, 90f,
                        new Rectangle(x * Tile.Width, y * Tile.Width, Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Hob, 1);

                case "H3": // Bottom
                    return new Tile(ref objects, ContentManager.Hob, -90f,
                        new Rectangle(x * Tile.Width, y * Tile.Width, Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Hob, 1);

                case "K0": // Left
                    return new Tile(ref objects, ContentManager.Kitchen_Sink, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Kitchen_Sink, 1);

                case "K1": // Right
                    return new Tile(ref objects, ContentManager.Kitchen_Sink, 180f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Kitchen_Sink, 1);

                case "K2": // Top
                    return new Tile(ref objects, ContentManager.Kitchen_Sink, 90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Kitchen_Sink, 1);

                case "K3": // Bottom
                    return new Tile(ref objects, ContentManager.Kitchen_Sink, -90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Kitchen_Sink, 1);

                case "C0": // Left
                    return new Tile(ref objects, ContentManager.Counter_Top_Utensils, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Counter_Top_Utensils, 1);

                case "C1": // Right
                    return new Tile(ref objects, ContentManager.Counter_Top_Utensils, 180f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Counter_Top_Utensils, 1);

                case "C2": // Top
                    return new Tile(ref objects, ContentManager.Counter_Top_Utensils, 90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Counter_Top_Utensils, 1);

                case "C3": // Bottom
                    return new Tile(ref objects, ContentManager.Counter_Top_Utensils, -90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Counter_Top_Utensils, 1);

                case "C4": // Left
                    return new Tile(ref objects, ContentManager.Counter_Top_Corner, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Counter_Top_Corner, 1);

                case "C5": // Right
                    return new Tile(ref objects, ContentManager.Counter_Top_Corner, 180f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Counter_Top_Corner, 1);

                case "C6": // Top
                    return new Tile(ref objects, ContentManager.Counter_Top_Corner, 90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Counter_Top_Corner, 1);

                case "C7": // Bottom
                    return new Tile(ref objects, ContentManager.Counter_Top_Corner, -90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Counter_Top_Corner, 1);

                case "M0": // Left
                    return new Tile(ref objects, ContentManager.Microwave, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Microwave, 1);

                case "M1": // Right
                    return new Tile(ref objects, ContentManager.Microwave, 180f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Microwave, 1);

                case "M2": // Top
                    return new Tile(ref objects, ContentManager.Microwave, 90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Microwave, 1);

                case "M3": // Bottom
                    return new Tile(ref objects, ContentManager.Microwave, -90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Microwave, 1);

                case "A0": // Left
                    return new Tile(ref objects, ContentManager.Armchair, 0f,
                        new Rectangle((x * Tile.Width) + 6, (y * Tile.Height) + 6, 116, 116),
                        SpriteEffects.None, Collision.Impassable, Type.Armchair, 1);

                case "A1": // Right
                    return new Tile(ref objects, ContentManager.Armchair, 180f,
                        new Rectangle((x * Tile.Width) + 6, (y * Tile.Height) + 6, 116, 116),
                        SpriteEffects.None, Collision.Impassable, Type.Armchair, 1);

                case "A2": // Top
                    return new Tile(ref objects, ContentManager.Armchair, 90f,
                        new Rectangle((x * Tile.Width) + 6, (y * Tile.Height) + 6, 116, 116),
                        SpriteEffects.None, Collision.Impassable, Type.Armchair, 1);

                case "A3": // Bottom
                    return new Tile(ref objects, ContentManager.Armchair, -90f,
                        new Rectangle((x * Tile.Width) + 6, (y * Tile.Height) + 6, 116, 116),
                        SpriteEffects.None, Collision.Impassable, Type.Armchair, 1);

                case "A4":
                    return new Tile(ref objects, ContentManager.Armchair_Left, 180f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Armchair_Left, 1);

                case "A5":
                    return new Tile(ref objects, ContentManager.Armchair_Right, 180f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Armchair_Right, 1);

                case "T0": // Left
                    return new Tile(ref objects, ContentManager.Television, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Television, 1);

                case "T1": // Right
                    return new Tile(ref objects, ContentManager.Television, 180f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Television, 1);

                case "T2": // Top
                    return new Tile(ref objects, ContentManager.Television, 90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Television, 1);

                case "T3": // Bottom
                    return new Tile(ref objects, ContentManager.Television, -90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Television, 1);

                case "PP":
                    return new Tile(ref objects, ContentManager.Plant, 0f,
                        new Rectangle((x * Tile.Width) + 16, (y * Tile.Height) + 18, 87, 92),
                        SpriteEffects.None, Collision.Impassable, Type.Plant, 1);

                case "T4": // Left
                    return new Tile(ref objects, ContentManager.Toilet, 0f,
                        new Rectangle((x * Tile.Width) + 19, (y * Tile.Height) + 20, 106, 85),
                        SpriteEffects.None, Collision.Impassable, Type.Toilet, 1);

                case "T5": // Right
                    return new Tile(ref objects, ContentManager.Toilet, 180f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Toilet, 1);

                case "T6": // Top
                    return new Tile(ref objects, ContentManager.Toilet, 90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Toilet, 1);

                case "T7": // Bottom
                    return new Tile(ref objects, ContentManager.Toilet, -90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Toilet, 1);

                case "S0": // Left
                    return new Tile(ref objects, ContentManager.Bathroom_Sink, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bathroom_Sink, 1);

                case "S1": // Right
                    return new Tile(ref objects, ContentManager.Bathroom_Sink, 180f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bathroom_Sink, 1);

                case "S2": // Top
                    return new Tile(ref objects, ContentManager.Bathroom_Sink, 90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bathroom_Sink, 1);

                case "S3": // Bottom
                    return new Tile(ref objects, ContentManager.Bathroom_Sink, -90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bathroom_Sink, 1);

                case "S4":
                    return new Tile(ref objects, ContentManager.Bookshelf, 0f,
                        new Rectangle((x * Tile.Width) + 26, (y * Tile.Height) + 1, 41, 126),
                        SpriteEffects.None, Collision.Impassable, Type.Bookshelf, 1);

                case "B0": // Left
                    return new Tile(ref objects, ContentManager.Bath_Top, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bath_Top, 1);

                case "B1": // Top
                    return new Tile(ref objects, ContentManager.Bath_Top, 90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bath_Top, 1);

                case "B2": // Bottom
                    return new Tile(ref objects, ContentManager.Bath_Top, -90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bath_Top, 1);

                case "B3": // Left
                    return new Tile(ref objects, ContentManager.Bath_Bottom, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bath_Bottom, 1);

                case "B4": // Top
                    return new Tile(ref objects, ContentManager.Bath_Bottom, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.FlipVertically, Collision.Impassable, Type.Bath_Bottom, 1);

                case "B5": // Bottom
                    return new Tile(ref objects, ContentManager.Bath_Bottom, -90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bath_Bottom, 1);

                case "B6": // Left
                    return new Tile(ref objects, ContentManager.Bathroom_Shelf, 180f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Null, 1);

                case "B7": // Right
                    return new Tile(ref objects, ContentManager.Bathroom_Shelf, 0f,
                        new Rectangle((x * Tile.Width) + 18, (y * Tile.Height) + 5, 57, 117),
                        SpriteEffects.None, Collision.Impassable, Type.Bathroom_Shelf, 1);

                case "B8": // Top
                    return new Tile(ref objects, ContentManager.Bathroom_Shelf, -90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Null, 1);

                case "B9": // Bottom
                    return new Tile(ref objects, ContentManager.Bathroom_Shelf, 90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Null, 1);

                case "R0":
                    return new Tile(ref objects, ContentManager.Bed_Top_Left, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bed_Top_Left, 1);

                case "R1":
                    return new Tile(ref objects, ContentManager.Bed_Top_Left, 180f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bed_Top_Left, 1);

                case "R2":
                    return new Tile(ref objects, ContentManager.Bed_Top_Left, 90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bed_Top_Left, 1);

                case "R3": // Bottom
                    return new Tile(ref objects, ContentManager.Bed_Top_Left, -90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bed_Top_Left, 1);

                case "R4":
                    return new Tile(ref objects, ContentManager.Bed_Top_Right, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bed_Top_Right, 1);

                case "R5":
                    return new Tile(ref objects, ContentManager.Bed_Top_Right, 180f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bed_Top_Right, 1);

                case "R6":
                    return new Tile(ref objects, ContentManager.Bed_Top_Right, 90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bed_Top_Right, 1);

                case "R7": // Bottom
                    return new Tile(ref objects, ContentManager.Bed_Top_Right, -90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bed_Top_Right, 1);

                case "E0":
                    return new Tile(ref objects, ContentManager.Bed_Bottom_Left, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bed_Bottom_Left, 1);

                case "E1":
                    return new Tile(ref objects, ContentManager.Bed_Bottom_Left, 180f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bed_Bottom_Left, 1);

                case "E2":
                    return new Tile(ref objects, ContentManager.Bed_Bottom_Left, 90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bed_Bottom_Left, 1);

                case "E3":
                    return new Tile(ref objects, ContentManager.Bed_Bottom_Left, -90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bed_Bottom_Left, 1);

                case "E4":
                    return new Tile(ref objects, ContentManager.Bed_Bottom_Right, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bed_Bottom_Right, 1);

                case "E5":
                    return new Tile(ref objects, ContentManager.Bed_Bottom_Right, 180f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bed_Bottom_Right, 1);

                case "E6":
                    return new Tile(ref objects, ContentManager.Bed_Bottom_Right, 90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bed_Bottom_Right, 1);

                case "E7":
                    return new Tile(ref objects, ContentManager.Bed_Bottom_Right, -90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bed_Bottom_Right, 1);

                case "F0":
                    return new Tile(ref objects, ContentManager.Fridge, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Fridge, 1);

                case "F1":
                    return new Tile(ref objects, ContentManager.Fridge, 180f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Fridge, 1);

                case "F2":
                    return new Tile(ref objects, ContentManager.Fridge, 90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Fridge, 1);

                case "F3":
                    return new Tile(ref objects, ContentManager.Fridge, -90f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Fridge, 1);

                case "P0":
                    return new Tile(ref objects, ContentManager.Table_Top_Left, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Table_Top_Left, 1);

                case "P1":
                    return new Tile(ref objects, ContentManager.Table_Top_Center, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Table_Top_Center, 1);

                case "P2":
                    return new Tile(ref objects, ContentManager.Table_Top_Right, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Table_Top_Right, 1);

                case "P3":
                    return new Tile(ref objects, ContentManager.Table_Bottom_Left, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Table_Bottom_Left, 1);

                case "P4":
                    return new Tile(ref objects, ContentManager.Table_Bottom_Center, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Table_Bottom_Center, 1);

                case "P5":
                    return new Tile(ref objects, ContentManager.Table_Bottom_Right, 0f,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Table_Bottom_Right, 1);

                #endregion

                default:
                    throw new NotSupportedException();
            }
        }

        private Tile LoadPlayer(PlayerIndex index, int i, int x, int y)
        {
            Point position = GetTileBounds(x, y).Center;
            m_Level.Players[i] = new Player(m_Graphics, m_Level, index, new Vector2(position.X, position.Y));

            return new Tile();
        }

        private Tile LoadBoostCollectible(int x, int y)
        {
            Point position = GetTileBounds(x, y).Center;
            m_Level.BoostCollectibles.Add(new Boost(m_Level, new Vector2(position.X, position.Y)));

            return new Tile();
        }

        #endregion

        #region Helper Methods

        public Rectangle GetTileBounds(int x, int y)
        {
            return new Rectangle(x * Tile.Width, y * Tile.Height, Tile.Width, Tile.Height);
        }

        public Tile SwitchTile(Type tileType, int x, int y, float rotation)
        {
            var tiles = ContentManager.Tiles;
            var objects = ContentManager.Objects;

            rotation = MathHelper.ToDegrees(rotation);

            switch (tileType)
            {
                //case Type.Wall_Partition:
                //    return new Tile(ref tiles, ContentManager.Wall_Partition_Broken, rotation,
                //        SpriteEffects.None, Collision.Passable, Type.Wall_Partition);

                case Type.Hob:
                    return new Tile(ref objects, ContentManager.Hob_Broken, rotation,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Hob, 0);

                case Type.Kitchen_Sink:
                    return new Tile(ref objects, ContentManager.Kitchen_Sink_Broken, rotation,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Kitchen_Sink, 0);

                case Type.Counter_Top_Utensils:
                    return new Tile(ref objects, ContentManager.Counter_Top_Utensils_Broken, rotation,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Counter_Top_Utensils, 0);

                case Type.Microwave:
                    return new Tile(ref objects, ContentManager.Microwave_Broken, rotation,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Microwave, 0);

                case Type.Fridge:
                    return new Tile(ref objects, ContentManager.Fridge_Broken, rotation,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Fridge, 0);

                case Type.Armchair:
                    return new Tile(ref objects, ContentManager.Armchair_Broken, rotation,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Armchair, 0);

                case Type.Television:
                    return new Tile(ref objects, ContentManager.Television_Broken, rotation,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Television, 0);

                case Type.Plant:
                    return new Tile(ref objects, ContentManager.Plant_Broken, rotation,
                        new Rectangle((x * Tile.Width) + 26, (y * Tile.Height) + 40, 63, 70),
                        SpriteEffects.None, Collision.Impassable, Type.Plant, 0);

                case Type.Bathroom_Sink:
                    return new Tile(ref objects, ContentManager.Bathroom_Sink_Broken, rotation,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bathroom_Sink, 0);

                case Type.Toilet:
                    return new Tile(ref objects, ContentManager.Toilet_Broken, rotation,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Toilet, 0);

                case Type.Bath_Top:
                    return new Tile(ref objects, ContentManager.Bath_Top_Broken, rotation,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bath_Top, 0);

                case Type.Bath_Bottom:
                    return new Tile(ref objects, ContentManager.Bath_Bottom_Broken, rotation,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.FlipVertically, Collision.Impassable, Type.Bath_Bottom, 0);

                case Type.Bed_Top_Left:
                    return new Tile(ref objects, ContentManager.Bed_Top_Left_Broken, rotation,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bed_Top_Left, 0);

                case Type.Bed_Top_Right:
                    return new Tile(ref objects, ContentManager.Bed_Top_Right_Broken, rotation,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bed_Top_Right, 0);

                case Type.Bed_Bottom_Left:
                    return new Tile(ref objects, ContentManager.Bed_Bottom_Left_Broken, rotation,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bed_Bottom_Left, 0);

                case Type.Bed_Bottom_Right:
                    return new Tile(ref objects, ContentManager.Bed_Bottom_Right_Broken, rotation,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Bed_Bottom_Right, 0);

                case Type.Table_Top_Left:
                    return new Tile(ref objects, ContentManager.Table_Top_Left_Broken, rotation,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Table_Top_Left, 0);

                case Type.Table_Top_Center:
                    return new Tile(ref objects, ContentManager.Table_Top_Center_Broken, rotation,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Table_Top_Center, 0);

                case Type.Table_Top_Right:
                    return new Tile(ref objects, ContentManager.Table_Top_Right_Broken, rotation,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Table_Top_Right, 0);

                case Type.Table_Bottom_Left:
                    return new Tile(ref objects, ContentManager.Table_Bottom_Left_Broken, rotation,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Table_Bottom_Left, 0);

                case Type.Table_Bottom_Center:
                    return new Tile(ref objects, ContentManager.Table_Bottom_Center_Broken, rotation,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Table_Bottom_Center, 0);

                case Type.Table_Bottom_Right:
                    return new Tile(ref objects, ContentManager.Table_Bottom_Right_Broken, rotation,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Table_Bottom_Right, 0);

                case Type.Armchair_Left:
                    return new Tile(ref objects, ContentManager.Armchair_Left_Broken, rotation,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Armchair_Left, 0);

                case Type.Armchair_Right:
                    return new Tile(ref objects, ContentManager.Armchair_Right_Broken, rotation,
                        new Rectangle((x * Tile.Width), (y * Tile.Height), Tile.Width, Tile.Height),
                        SpriteEffects.None, Collision.Impassable, Type.Armchair_Right, 0);

                default:
                    return this.m_Tiles[x, y];
            }
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch, float cameraPosX, float cameraPosY)
        {
            // Introduce culling to help prevent any slow-down by limitimg
            // the number of tiles drawn to only those within the field of view.
            int top = (int)Math.Floor(cameraPosY / Tile.Height);
            int bottom = top + m_Graphics.Viewport.Height / Tile.Height;
            bottom = Math.Max(bottom, Height - 1);

            int left = (int)Math.Floor(cameraPosX / Tile.Width);
            int right = left + m_Graphics.Viewport.Width / Tile.Width;
            right = Math.Min(right, Width - 1);

            for (int y = top; y <= bottom; ++y)
            {
                for (int x = left; x <= right; ++x)
                {
                    try
                    {
                        Texture2D texture = m_Tiles[x, y].Texture;

                        if (texture != null)
                        {
                            Vector2 position = new Vector2(x, y) * Tile.Size;
                            Rectangle source = m_Tiles[x, y].Source;
                            Vector2 origin = new Vector2(source.Width / 2, source.Height / 2);

                            float rotation = m_Tiles[x, y].Rotation;
                            int offset = 64;

                            Rectangle destination = new Rectangle((int)position.X + offset, (int)position.Y + offset,
                                source.Width, source.Height);

                            SpriteEffects effect = m_Tiles[x, y].Effect;

                            spriteBatch.Draw(texture, destination, source, Color.White, rotation,
                                origin, effect, 0f);
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }

        #endregion
    }
}