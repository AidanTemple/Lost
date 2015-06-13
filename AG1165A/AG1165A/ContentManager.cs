#region Using Statements
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
#endregion

namespace AG1165A
{
    static class ContentManager
    {
        #region Public Members

        #region Audio

        public static List<SoundEffectInstance> BreakSounds { get; private set; }

        public static SoundEffect Theme { get; private set; }
        public static SoundEffect PunchSound { get; private set; }
        public static SoundEffect WalkSound { get; private set; }
        public static SoundEffect StunSound { get; private set; }

        public static SoundEffect BreakSound_01 { get; private set; }
        public static SoundEffect BreakSound_02 { get; private set; }

        public static SoundEffect SmashSound { get; private set; }
        public static SoundEffect MenuBackSound { get; private set; }
        public static SoundEffect MenuSelectSound { get; private set; }
        public static SoundEffect MenuUpSound { get; private set; }
        public static SoundEffect MenuDownSound { get; private set; }
        public static SoundEffect CountdownShort { get; private set; }
        public static SoundEffect CountdownLong { get; private set; }
        public static SoundEffect KeyDropSound { get; private set; }
        public static SoundEffect KeyPickupSound { get; private set; }

        public static SoundEffectInstance CountdownLongInstance { get; private set; }
        public static SoundEffectInstance ThemeInstance { get; private set; }
        public static SoundEffectInstance PunchInstance { get; private set; }
        public static SoundEffectInstance WalkInstance { get; private set; }
        public static SoundEffectInstance StunInstance { get; private set; }
        public static SoundEffectInstance BreakInstance_01 { get; private set; }
        public static SoundEffectInstance BreakInstance_02 { get; private set; }
        public static SoundEffectInstance SmashInstance { get; private set; }

        #endregion

        #region Animations

        public static Texture2D Player_Idle_A { get; private set; }
        public static Texture2D Player_Walk_A { get; private set; }
        public static Texture2D Player_Stun_A { get; private set; }
        public static Texture2D Player_Punch_A { get; private set; }

        public static Texture2D Player_Idle_B { get; private set; }
        public static Texture2D Player_Walk_B { get; private set; }
        public static Texture2D Player_Stun_B { get; private set; }
        public static Texture2D Player_Punch_B { get; private set; }

        public static Texture2D PlayerADefault { get; private set; }
        public static Texture2D PlayerAStun { get; private set; }
        public static Texture2D PlayerAWin { get; private set; }
        public static Texture2D PlayerALose { get; private set; }

        public static Texture2D PlayerBDefault { get; private set; }
        public static Texture2D PlayerBStun { get; private set; }
        public static Texture2D PlayerBWin { get; private set; }
        public static Texture2D PlayerBLose { get; private set; }

        public static Texture2D Identifier { get; private set; }
        public static Texture2D Identifier_Blue { get; private set; }
        public static Texture2D Identifier_Yellow { get; private set; }

        public static Texture2D Identifier_Arrow { get; private set; }
        public static Texture2D Identifier_Arrow_Blue { get; private set; }
        public static Texture2D Identifier_Arrow_Yellow { get; private set; }

        public static Texture2D Key_Spawn { get; private set; }
        public static Texture2D Key_Idle { get; private set; }

        public static Texture2D Smoke_Cloud { get; private set; }

        #endregion

        #region Backgrounds

        public static Texture2D SplashScreen { get; private set; }
        public static Texture2D KeyboardBackScreen { get; private set; }
        public static Texture2D ControllerBackScreen { get; private set; }
        public static Texture2D KeyboardSelectScreen { get; private set; }
        public static Texture2D ControllerSelectScreen { get; private set; }
        public static Texture2D ControllerControls { get; private set; }
        public static Texture2D KeyboardControls { get; private set; }
        public static Texture2D LevelBackground { get; private set; }
        public static Texture2D LevelInteriorWalls { get; private set; }
        public static Texture2D Statistics { get; private set; }
        public static Texture2D KeyboardPauseOverlay{ get; private set; }
        public static Texture2D ControllerPauseOverlay { get; private set; }
        public static Texture2D Credits { get; private set; }

        #endregion

        #region Fonts

        public static SpriteFont DebugFont { get; private set; }
        public static SpriteFont UnselectedFont { get; private set; }
        public static SpriteFont SelectedFont { get; private set; }
        public static SpriteFont TimerFont { get; private set; }
        public static SpriteFont ScoreFont { get; private set; }
        public static SpriteFont CountdownFont { get; private set; }
        public static SpriteFont IdentifierFont { get; private set; }

        #endregion

        #region Interface

        public static Texture2D Controller_Buttons { get; set; }
        public static Texture2D Viewport_Edge { get; private set; }
        public static Texture2D HUD_Yellow { get; private set; }
        public static Texture2D HUD_Blue { get; private set; }
        public static Texture2D HUD_Key { get; private set; }

        public static Rectangle Start_Button { get; private set; }

        #endregion

        #region Levels

        public static string Layer01 { get; private set; }
        public static string Layer02 { get; private set; }

        #endregion

        #region Tiles

        public static Texture2D Tiles { get; set; }
        public static Texture2D Objects { get; set; }

        public static Rectangle WoodenFloor { get; private set; }
        public static Rectangle WoodenFloorBroken { get; private set; }
        public static Rectangle KitchenFloor { get; private set; }
        public static Rectangle BathroomFloor { get; private set; }
        public static Rectangle Carpet_Red { get; private set; }
        public static Rectangle Carpet_Blue { get; private set; }
        public static Rectangle Grass { get; private set; }
        public static Rectangle Door { get; private set; }
        public static Rectangle Door_Left { get; private set; }
        public static Rectangle Door_Right { get; private set; }
        public static Rectangle Door_Broken { get; private set; }
        public static Rectangle Stairs_Bottom { get; private set; }
        public static Rectangle Stairs_Middle { get; private set; }
        public static Rectangle Stairs_Top { get; private set; }

        public static Rectangle Wall_Corner_Stone { get; private set; }
        public static Rectangle Wall_Corner { get; private set; }
        public static Rectangle Wall_Door_Left { get; private set; }
        public static Rectangle Wall_Left { get; private set; }
        public static Rectangle Wall_Cavity { get; private set; }
        public static Rectangle Wall_Partition { get; private set; }
        public static Rectangle Wall_Partition_Broken { get; private set; }
        public static Rectangle Window_Frame_Top { get; private set; }
        public static Rectangle Window { get; private set; }

        public static Rectangle IncreaseSpeedCollectible { get; private set; }
        public static Rectangle KeyCollectible { get; private set; }

        public static Rectangle Bath_Bottom { get; private set; }
        public static Rectangle Bath_Bottom_Broken { get; private set; }
        public static Rectangle Bath_Top { get; private set; }
        public static Rectangle Bath_Top_Broken { get; private set; }
        public static Rectangle ClothesHamper { get; private set; }
        public static Rectangle ClothesHamperBroken { get; private set; }
        public static Rectangle Counter_Top_Corner { get; private set; }
        public static Rectangle Counter_Top_Utensils { get; private set; }
        public static Rectangle Counter_Top_Utensils_Broken { get; private set; }
        public static Rectangle Drawer { get; private set; }
        public static Rectangle DrawerBroken { get; private set; }
        public static Rectangle Fridge { get; private set; }
        public static Rectangle Fridge_Broken { get; private set; }
        public static Rectangle Hob { get; private set; }
        public static Rectangle Hob_Broken { get; private set; }
        public static Rectangle Shower { get; private set; }
        public static Rectangle ShowerBroken { get; private set; }
        public static Rectangle Kitchen_Sink { get; private set; }
        public static Rectangle Kitchen_Sink_Broken { get; private set; }
        public static Rectangle WashingMachine { get; private set; }
        public static Rectangle WashingMachineBroken { get; private set; }
        public static Rectangle Microwave { get; private set; }
        public static Rectangle Microwave_Broken { get; private set; }
        public static Rectangle Armchair { get; private set; }
        public static Rectangle Armchair_Broken { get; private set; }
        public static Rectangle Armchair_Left { get; private set; }
        public static Rectangle Armchair_Left_Broken { get; private set; }
        public static Rectangle Armchair_Right { get; private set; }
        public static Rectangle Armchair_Right_Broken { get; private set; }
        public static Rectangle Television { get; private set; }
        public static Rectangle Television_Broken { get; private set; }
        public static Rectangle Plant { get; private set; }
        public static Rectangle Plant_Broken { get; private set; }
        public static Rectangle Toilet { get; private set; }
        public static Rectangle Toilet_Broken { get; private set; }
        public static Rectangle Bathroom_Sink { get; private set; }
        public static Rectangle Bathroom_Sink_Broken { get; private set; }
        public static Rectangle Bed_Top_Left { get; private set; }
        public static Rectangle Bed_Top_Left_Broken { get; private set; }
        public static Rectangle Bed_Top_Right { get; private set; }
        public static Rectangle Bed_Top_Right_Broken { get; private set; }
        public static Rectangle Bed_Bottom_Left { get; private set; }
        public static Rectangle Bed_Bottom_Left_Broken { get; private set; }
        public static Rectangle Bed_Bottom_Right { get; private set; }
        public static Rectangle Bed_Bottom_Right_Broken { get; private set; }
        public static Rectangle Bathroom_Shelf { get; private set; }
        public static Rectangle Bathroom_Shelf_Broken { get; private set; }
        public static Rectangle Bookshelf { get; private set; }
        public static Rectangle Bookshelf_Broken { get; private set; }
        public static Rectangle Table_Top_Left { get; private set; }
        public static Rectangle Table_Top_Left_Broken { get; private set; }
        public static Rectangle Table_Top_Center { get; private set; }
        public static Rectangle Table_Top_Center_Broken { get; private set; }
        public static Rectangle Table_Top_Right { get; private set; }
        public static Rectangle Table_Top_Right_Broken { get; private set; }
        public static Rectangle Table_Bottom_Left { get; private set; }
        public static Rectangle Table_Bottom_Left_Broken { get; private set; }
        public static Rectangle Table_Bottom_Center { get; private set; }
        public static Rectangle Table_Bottom_Center_Broken { get; private set; }
        public static Rectangle Table_Bottom_Right { get; private set; }
        public static Rectangle Table_Bottom_Right_Broken { get; private set; }

        #endregion

        #endregion

        #region Initialisation

        /// <summary>
        /// Used to load all game content
        /// </summary>
        /// <param name="Content">Run-time component which loads managed objects from binary files</param>
        public static void Load(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            #region Audio

            Theme                   = Content.Load<SoundEffect>("Audio/Theme");
            PunchSound              = Content.Load<SoundEffect>("Audio/Punch");
            WalkSound               = Content.Load<SoundEffect>("Audio/Walk");
            StunSound               = Content.Load<SoundEffect>("Audio/Stun");
            BreakSound_01           = Content.Load<SoundEffect>("Audio/Break_01");
            BreakSound_02           = Content.Load<SoundEffect>("Audio/Break_02");
            SmashSound              = Content.Load<SoundEffect>("Audio/Smash");
            MenuBackSound           = Content.Load<SoundEffect>("Audio/Menu");
            MenuSelectSound         = Content.Load<SoundEffect>("Audio/MenuSelect");
            MenuUpSound             = Content.Load<SoundEffect>("Audio/MenuUp");
            MenuDownSound           = Content.Load<SoundEffect>("Audio/MenuDown");
            CountdownShort          = Content.Load<SoundEffect>("Audio/CountdownShort");
            CountdownLong           = Content.Load<SoundEffect>("Audio/CountdownLong");
            KeyDropSound            = Content.Load<SoundEffect>("Audio/KeyDrop");
            KeyPickupSound          = Content.Load<SoundEffect>("Audio/KeyPickup");

            CountdownLongInstance   = AudioHandler.CreateInstance(CountdownLong, CountdownLongInstance);
            ThemeInstance           = AudioHandler.CreateInstance(Theme, ThemeInstance);
            PunchInstance           = AudioHandler.CreateInstance(PunchSound, PunchInstance);
            WalkInstance            = AudioHandler.CreateInstance(WalkSound, WalkInstance);
            StunInstance            = AudioHandler.CreateInstance(StunSound, StunInstance);
            BreakInstance_01        = AudioHandler.CreateInstance(BreakSound_01, BreakInstance_01);
            BreakInstance_02        = AudioHandler.CreateInstance(BreakSound_02, BreakInstance_02);
            SmashInstance           = AudioHandler.CreateInstance(SmashSound, SmashInstance);

            ThemeInstance.IsLooped = true;

            BreakSounds = new List<SoundEffectInstance>();
            BreakSounds.Insert(0, BreakInstance_01);
            BreakSounds.Insert(1, BreakInstance_02);

            #endregion

            #region Animations

            Player_Idle_A = Content.Load<Texture2D>("Textures/Animations/Player/Idle_Animation_A");
            Player_Walk_A = Content.Load<Texture2D>("Textures/Animations/Player/Walk_Animation_A");
            Player_Stun_A = Content.Load<Texture2D>("Textures/Animations/Player/Stun_Animation_A");
            Player_Punch_A = Content.Load<Texture2D>("Textures/Animations/Player/Punch_Animation_A");

            PlayerADefault = Content.Load<Texture2D>("Textures/Animations/Player/PlayerADefault");
            PlayerAStun = Content.Load<Texture2D>("Textures/Animations/Player/PlayerAStun");
            PlayerAWin = Content.Load<Texture2D>("Textures/Animations/Player/PlayerAWin");
            PlayerALose = Content.Load<Texture2D>("Textures/Animations/Player/PlayerALose");

            Player_Idle_B = Content.Load<Texture2D>("Textures/Animations/Opponent/Idle_Animation_B");
            Player_Walk_B = Content.Load<Texture2D>("Textures/Animations/Opponent/Walk_Animation_B");
            Player_Stun_B = Content.Load<Texture2D>("Textures/Animations/Opponent/Stun_Animation_B");
            Player_Punch_B = Content.Load<Texture2D>("Textures/Animations/Opponent/Punch_Animation_B");

            PlayerBDefault = Content.Load<Texture2D>("Textures/Animations/Opponent/PlayerBDefault");
            PlayerBStun = Content.Load<Texture2D>("Textures/Animations/Opponent/PlayerBStun");
            PlayerBWin = Content.Load<Texture2D>("Textures/Animations/Opponent/PlayerBWin");
            PlayerBLose = Content.Load<Texture2D>("Textures/Animations/Opponent/PlayerBLose");

            Identifier_Blue = Content.Load<Texture2D>("Textures/Identifier_Blue");
            Identifier_Yellow = Content.Load<Texture2D>("Textures/Identifier_Yellow");

            Identifier_Arrow_Blue = Content.Load<Texture2D>("Textures/Identifier_Arrow_Blue");
            Identifier_Arrow_Yellow = Content.Load<Texture2D>("Textures/Identifier_Arrow_Yellow");

            //Key_Spawn = Content.Load<Texture2D>("Textures/Animations/Key/Key_Spawn");
            Key_Idle = Content.Load<Texture2D>("Textures/Animations/Key/Key_Idle");

            Smoke_Cloud = Content.Load<Texture2D>("Textures/Animations/Smoke_Cloud");

            #endregion

            #region Backgrounds

            SplashScreen            = Content.Load<Texture2D>("Textures/backgrounds/SplashScreen");
            KeyboardBackScreen      = Content.Load<Texture2D>("Textures/backgrounds/KeyboardBackScreen");
            ControllerBackScreen    = Content.Load<Texture2D>("Textures/backgrounds/ControllerBackScreen");
            KeyboardSelectScreen    = Content.Load<Texture2D>("Textures/backgrounds/KeyboardSelectScreen");
            ControllerSelectScreen  = Content.Load<Texture2D>("Textures/backgrounds/ControllerSelectScreen");
            ControllerControls      = Content.Load<Texture2D>("Textures/Backgrounds/ControllerControls");
            KeyboardControls        = Content.Load<Texture2D>("Textures/Backgrounds/KeyboardControls");
            LevelBackground         = Content.Load<Texture2D>("Textures/Backgrounds/Level");
            LevelInteriorWalls      = Content.Load<Texture2D>("Textures/Backgrounds/Interior_Walls");
            Statistics              = Content.Load<Texture2D>("Textures/Backgrounds/Statistics");
            KeyboardPauseOverlay    = Content.Load<Texture2D>("Textures/Backgrounds/KeyboardPauseOverlay");
            ControllerPauseOverlay  = Content.Load<Texture2D>("Textures/Backgrounds/ControllerPauseOverlay");
            Credits                 = Content.Load<Texture2D>("Textures/Backgrounds/Credits");

            #endregion

            #region Fonts

            DebugFont               = Content.Load<SpriteFont>("Fonts/Debug");
            UnselectedFont          = Content.Load<SpriteFont>("Fonts/Unselected");
            SelectedFont            = Content.Load<SpriteFont>("Fonts/Selected");
            TimerFont               = Content.Load<SpriteFont>("Fonts/Timer");
            ScoreFont               = Content.Load<SpriteFont>("Fonts/Score");
            CountdownFont           = Content.Load<SpriteFont>("Fonts/Countdown");
            IdentifierFont          = Content.Load<SpriteFont>("Fonts/IdentifierFont");

            #endregion

            #region Interface

            Controller_Buttons              = Content.Load<Texture2D>("Textures/Controller_Buttons");
            Viewport_Edge                   = Content.Load<Texture2D>("Textures/ViewportEdge");
            HUD_Yellow                      = Content.Load<Texture2D>("Textures/HUD_Yellow");
            HUD_Blue                        = Content.Load<Texture2D>("Textures/HUD_Blue");
            HUD_Key = Content.Load<Texture2D>("Textures/HUD_Key");

            Start_Button                    = new Rectangle(712, 44, 83, 76);

            #endregion

            #region Levels

            Layer01 = "Content/Maps/layer_0.txt";
            Layer02 = "Content/Maps/layer_1.txt";

            #endregion

            #region Tiles

            Tiles                           = Content.Load<Texture2D>("Textures/Tiles");
            Objects                         = Content.Load<Texture2D>("Textures/Objects");

            BathroomFloor                   = new Rectangle(2, 2, 128, 128);
            Carpet_Blue                     = new Rectangle(2, 132, 128, 128);
            Carpet_Red                      = new Rectangle(2, 262, 128, 128);
            KitchenFloor                    = new Rectangle(132, 132, 128, 128);
            WoodenFloor                     = new Rectangle(132, 262, 128, 128);
            Grass                           = new Rectangle(132, 2, 128, 128);

            Wall_Corner_Stone               = new Rectangle(522, 2, 128, 128);
            Wall_Corner                     = new Rectangle(392, 132, 128, 128);
            Wall_Left                       = new Rectangle(652, 132, 128, 128);
            Wall_Cavity                     = new Rectangle(782, 2, 128, 128);
            Wall_Partition                  = new Rectangle(912, 2, 128, 128);
            Wall_Partition_Broken           = new Rectangle(782, 132, 128, 128);
            Wall_Door_Left                  = new Rectangle(262, 262, 128, 128);
            Window                          = new Rectangle(912, 132, 128, 128);
            Window_Frame_Top                = new Rectangle(782, 262, 128, 128);
            Door                            = new Rectangle(262, 262, 128, 128);
            Door_Broken                     = new Rectangle(262, 132, 128, 128);
            Door_Left                       = new Rectangle(262, 132, 128, 128);
            Door_Right                      = new Rectangle(262, 2, 128, 128);
            Stairs_Bottom                   = new Rectangle(2, 652, 128, 128);
            Stairs_Middle                   = new Rectangle(2, 782, 128, 128);
            Stairs_Top                      = new Rectangle(132, 782, 128, 128);

            Hob                             = new Rectangle(2, 2, 128, 128);
            Hob_Broken                      = new Rectangle(132, 2, 128, 128);
            Kitchen_Sink                    = new Rectangle(262, 2, 128, 128);
            Kitchen_Sink_Broken             = new Rectangle(392, 2, 128, 128);
            Counter_Top_Utensils            = new Rectangle(522, 2, 128, 128);
            Counter_Top_Utensils_Broken     = new Rectangle(652, 2, 128, 128);
            Microwave                       = new Rectangle(782, 2, 128, 128);
            Microwave_Broken                = new Rectangle(912, 2, 128, 128);
            Fridge                          = new Rectangle(2, 132, 128, 128);
            Fridge_Broken                   = new Rectangle(132, 132, 128, 128);
            Counter_Top_Corner              = new Rectangle(262, 132, 128, 128);
            Armchair                        = new Rectangle(522, 132, 128, 128);
            Armchair_Broken                 = new Rectangle(392, 132, 128, 128);
            Armchair_Left                   = new Rectangle(262, 652, 128, 128);
            Armchair_Left_Broken            = new Rectangle(2, 652, 128, 128);
            Armchair_Right                  = new Rectangle(392, 652, 128, 128);
            Armchair_Right_Broken           = new Rectangle(132, 652, 128, 128);
            Television                      = new Rectangle(652, 132, 128, 128);
            Television_Broken               = new Rectangle(782, 132, 128, 128);
            Plant                           = new Rectangle(912, 132, 128, 128);
            Plant_Broken                    = new Rectangle(2, 262, 128, 128);
            Toilet                          = new Rectangle(132, 262, 128, 128);
            Toilet_Broken                   = new Rectangle(262, 262, 128, 128);
            Bathroom_Sink                   = new Rectangle(392, 262, 128, 128);
            Bathroom_Sink_Broken            = new Rectangle(522, 262, 128, 128);
            Bath_Top                        = new Rectangle(652, 262, 128, 128);
            Bath_Top_Broken                 = new Rectangle(912, 262, 128, 128);
            Bath_Bottom                     = new Rectangle(782, 262, 128, 128);
            Bath_Bottom_Broken              = new Rectangle(2, 392, 128, 128);
            Bed_Top_Left                    = new Rectangle(132, 392, 128, 128);
            Bed_Top_Right                   = new Rectangle(262, 392, 128, 128);
            Bed_Bottom_Left                 = new Rectangle(392, 392, 128, 128);
            Bed_Bottom_Right                = new Rectangle(522, 392, 128, 128);
            Bed_Top_Left_Broken             = new Rectangle(652, 392, 128, 128);
            Bed_Top_Right_Broken            = new Rectangle(782, 392, 128, 128);
            Bed_Bottom_Left_Broken          = new Rectangle(912, 392, 128, 128);
            Bed_Bottom_Right_Broken         = new Rectangle(2, 522, 128, 128);
            Bathroom_Shelf                  = new Rectangle(132, 522, 128, 128);
            Bathroom_Shelf_Broken           = new Rectangle(262, 522, 128, 128);
            Bookshelf                       = new Rectangle(392, 522, 128, 128);
            Bookshelf_Broken                = new Rectangle(522, 522, 128, 128);
            Table_Top_Left                  = new Rectangle(2, 782, 128, 128);
            Table_Top_Left_Broken           = new Rectangle(782, 782, 128, 128);
            Table_Top_Center                = new Rectangle(132, 782, 128, 128);
            Table_Top_Center_Broken         = new Rectangle(912, 782, 128, 128);
            Table_Top_Right                 = new Rectangle(262, 782, 128, 128);
            Table_Top_Right_Broken          = new Rectangle(2, 912, 128, 128);
            Table_Bottom_Left               = new Rectangle(392, 782, 128, 128);
            Table_Bottom_Left_Broken        = new Rectangle(132, 912, 128, 128);
            Table_Bottom_Center             = new Rectangle(522, 782, 128, 128);
            Table_Bottom_Center_Broken      = new Rectangle(262, 912, 128, 128);
            Table_Bottom_Right              = new Rectangle(652, 782, 128, 128);
            Table_Bottom_Right_Broken       = new Rectangle(392, 912, 128, 128);

            KeyCollectible                  = new Rectangle(652, 522, 128, 128);
            IncreaseSpeedCollectible        = new Rectangle(782, 522, 128, 128);

            #endregion
        }

        #endregion
    }
}