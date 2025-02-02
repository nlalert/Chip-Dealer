using System.Collections.Generic;
using Microsoft.Xna.Framework;

public class ViewportManager
{
    private static readonly Dictionary<string, Rectangle> _spriteRects = new Dictionary<string, Rectangle>
    {
        // Normal Chips
        {"Red_Chip",new Rectangle( 0, 0, 32, 35)},
        {"Blue_Chip",new Rectangle( 32, 0, 32, 35)},
        {"Green_Chip",new Rectangle( 64, 0, 32, 35)},
        {"Yellow_Chip",new Rectangle( 96, 0, 32, 35)},
        {"Purple_Chip",new Rectangle( 128, 0, 32, 35)},
        {"White_Chip",new Rectangle( 160, 0, 32, 35)},
        {"Black_Chip",new Rectangle( 192, 0, 32, 35)},
        {"Orange_Chip",new Rectangle( 224, 0, 32, 35)},

        // Specaial Chips
        {"Explosive_Chip",new Rectangle( 32, 48, 32, 35)},
        {"Unknown_Chip",new Rectangle( 64, 48, 32, 35)},

        // Relics
        {"Placeholder",new Rectangle( 176, 176, 32, 32)},
        {"Potato_Chip",new Rectangle( 0, 96, 32, 35)},
        {"Processing_Chip",new Rectangle( 160, 96, 32, 35)},
        {"Chipped_Chip",new Rectangle( 192, 96, 32, 35)},

        {"Chipy_Chip",new Rectangle( 0, 0, 32, 35)},
        {"Chipy_Chip1",new Rectangle( 0, 0, 32, 35)},
        {"Chipy_Chip2",new Rectangle( 0, 0, 32, 35)},
        {"Chipy_Chip3",new Rectangle( 0, 0, 32, 35)},
        {"Chipy_Chip4",new Rectangle( 0, 0, 32, 35)},
        {"Chipy_Chip5",new Rectangle( 0, 0, 32, 35)},
        {"Chipy_Chip6",new Rectangle( 0, 0, 32, 35)},
        {"Chipy_Chip7",new Rectangle( 0, 0, 32, 35)},
        {"Chipy_Chip8",new Rectangle( 0, 0, 32, 35)},

        // Background
        {"Mainmenu_Background", new Rectangle(288,224,384,488)},
        {"Ingame_Background", new Rectangle(0,224,288,488)},

        // Menu UI
        {"Game_Title",new Rectangle(0, 2032, 480, 160)},//TODO

        {"Menu_Button",new Rectangle( 640, 720, 352, 80)},
        {"Menu_Button_Highlighted",new Rectangle( 640, 816, 352, 80)},

        {"Start_Label",new Rectangle( 640, 912, 176, 48)},
        {"Start_Label_Highlighted",new Rectangle( 816, 912, 176, 48)},

        {"ScoreBoard_Label",new Rectangle( 608, 1360, 336, 48)},
        {"ScoreBoard_Label_Highlighted",new Rectangle( 608, 1424, 336, 48)},

        {"Exit_Label",new Rectangle( 0, 1488, 304, 48)},
        {"Exit_Label_Highlighted",new Rectangle( 304, 1488, 304, 48)},


        {"Small_Button",new Rectangle(608, 1136, 160 ,48)},
        {"Small_Button_Highlighted",new Rectangle(608, 1200, 160 ,48)},

        {"Back_Label",new Rectangle( 608, 1264, 160, 32)},
        {"Back_Label_Highlighted",new Rectangle( 608, 1312, 160, 32)},

        // In Game UI

        {"Player_Hand",new Rectangle( 0, 144, 96, 80)},

        {"Chip_Stick",new Rectangle( 896, 1504, 256, 400)},

        // In Game-Stat UI
        {"Score_Label0",new Rectangle( 240, 144, 80, 16)},
        {"Score_Label1",new Rectangle( 320, 144, 80, 16)},
        {"Score_Label2",new Rectangle( 400, 144, 80, 16)},

        {"Score_Box0",new Rectangle( 208, 176, 144, 32)},
        {"Score_Box1",new Rectangle( 352, 176, 144, 32)},
        {"Score_Box2",new Rectangle( 496, 176, 144, 32)},

        {"Stage_Label",new Rectangle(480 , 144 ,80, 16)},
        {"Stage_Box",new Rectangle(768, 176 ,48, 32)},

        {"Money_Label",new Rectangle(640, 144 ,80, 16)},
        {"Money_Box",new Rectangle(640, 176 ,80, 32)},

        {"Ceiling_Turn_Label",new Rectangle(720, 144 ,48, 16)},
        {"Ceiling_Turn_Box",new Rectangle(720, 176 ,48, 32)},

        {"Owned_Relic_Box",new Rectangle(768, 992 ,144, 144)},

        {"Next_Chip_Box",new Rectangle( 112, 160, 48, 48)},
        {"Next_Chip_Label",new Rectangle( 176, 144, 64, 16)},

        // Slot-Mechine UI
        {"Tutorial_Drawing",new Rectangle(672, 224, 160, 96)},
        {"Slot_Label",new Rectangle(672, 336, 160, 48)},

        {"Slot_Machine",new Rectangle(672, 400, 160, 160)},
        {"Slot_Handle",new Rectangle(672, 576, 48, 16)},
        {"Slot_Background",new Rectangle(832, 400, 48, 147)},

        {"Slot_Drawing",new Rectangle(672, 608, 112, 96)},

        // Shop UI
        {"Shop_Label",new Rectangle(880, 160, 144, 48)},
        {"Shop_Box",new Rectangle(880, 224, 256, 480)},

        {"Relic_Box0",new Rectangle(0, 880, 128, 96)},
        {"Relic_Box1",new Rectangle(128, 880, 128, 96)},
        {"Relic_Box2",new Rectangle(256, 880, 128, 96)},

        {"Relic_Box_Highlighted0",new Rectangle(768, 1152, 128, 96)},
        {"Relic_Box_Highlighted1",new Rectangle(896, 1152, 128, 96)},
        {"Relic_Box_Highlighted2",new Rectangle(1024, 1152, 128, 96)},

        {"Relic_Box_Sold",new Rectangle(384, 880, 128, 96)},

        {"Next_Label",new Rectangle( 768, 1264, 160, 32)},
        {"Next_Label_Highlighted",new Rectangle( 768, 1312, 160, 32)},

        // GameOver UI
        {"GameOver_Label",new Rectangle( 352, 720, 240, 144)},
        {"GameWin_Label",new Rectangle( 0, 720, 336, 64)},
        {"NewGame_Label",new Rectangle( 0, 800, 256, 32)},

        // Pause UI
        {"Pause_Title0",new Rectangle(0, 992, 384, 128)},
        {"Pause_Title1",new Rectangle(384, 992, 384, 128)},

        {"Pause_Button",new Rectangle(0, 1136, 304, 80)},
        {"Pause_Button_Highlighted",new Rectangle(304, 1136, 304, 80)},

        {"Resume_Label",new Rectangle(0, 1296, 304, 48)},
        {"Resume_Label_Highlighted",new Rectangle(304, 1296, 304, 48)},

        {"Restart_Label",new Rectangle(0, 1360, 304, 48)},
        {"Restart_Label_Highlighted",new Rectangle(304, 1360, 304, 48)},

        {"Settings_Label",new Rectangle(0, 1232, 304, 48)},
        {"Settings_Label_Highlighted",new Rectangle(304, 1232, 304, 48)},

        {"Mainmenu_Label",new Rectangle(0, 1424, 304, 48)},
        {"Mainmenu_Label_Highlighted",new Rectangle(304, 1424, 304, 48)},

        // Settings UI
        {"Big_Box0",new Rectangle(0, 1552, 448, 352)},
        {"Big_Box1",new Rectangle(448, 1552, 448, 352)},

        {"Music_Label",new Rectangle(448, 1984, 176, 32)},
        {"SFX_Label",new Rectangle(624, 1984, 112, 32)},

        {"Slide_Bar",new Rectangle(0, 1920, 448, 80)},

        {"Slide_Chip0",new Rectangle(448, 1920, 32, 35)},
        {"Slide_Chip1",new Rectangle(480, 1920, 32, 35)},
        {"Slide_Chip2",new Rectangle(512, 1920, 32, 35)},
        {"Slide_Chip3",new Rectangle(544, 1920, 32, 35)},
    };
    
    public static Rectangle Get(string name){
        return _spriteRects[name];
    }
}