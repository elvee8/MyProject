using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_ForAutoItTest.SendKeyMessage
{
    public class VirtualKeyCode
    {
        //
        // Summary:
        //     Left mouse button
        public const uint LBUTTON = 1;
        //
        // Summary:
        //     Right mouse button
        public const uint RBUTTON = 2;
        //
        // Summary:
        //     Control-break processing
        public const uint CANCEL = 3;
        //
        // Summary:
        //     Middle mouse button (three-button mouse) - NOT contiguous with LBUTTON and RBUTTON
        public const uint MBUTTON = 4;
        //
        // Summary:
        //     Windows 2000/XP: X1 mouse button - NOT contiguous with LBUTTON and RBUTTON
        public const uint XBUTTON1 = 5;
        //
        // Summary:
        //     Windows 2000/XP: X2 mouse button - NOT contiguous with LBUTTON and RBUTTON
        public const uint XBUTTON2 = 6;
        //
        // Summary:
        //     BACKSPACE key
        public const uint BACK = 8;
        //
        // Summary:
        //     TAB key
        public const uint TAB = 9;
        //
        // Summary:
        //     CLEAR key
        public const uint CLEAR = 12;
        //
        // Summary:
        //     ENTER key
        public const uint RETURN = 13;
        //
        // Summary:
        //     SHIFT key
        public const uint SHIFT = 16;
        //
        // Summary:
        //     CTRL key
        public const uint CONTROL = 17;
        //
        // Summary:
        //     ALT key
        public const uint MENU = 18;
        //
        // Summary:
        //     PAUSE key
        public const uint PAUSE = 19;
        //
        // Summary:
        //     CAPS LOCK key
        public const uint CAPITAL = 20;
        //
        // Summary:
        //     Input Method Editor (IME) Kana mode
        public const uint KANA = 21;
        //
        // Summary:
        //     IME Hanguel mode (maintained for compatibility; use HANGUL)
        public const uint HANGEUL = 21;
        //
        // Summary:
        //     IME Hangul mode
        public const uint HANGUL = 21;
        //
        // Summary:
        //     IME Junja mode
        public const uint JUNJA = 23;
        //
        // Summary:
        //     IME final mode
        public const uint FINAL = 24;
        //
        // Summary:
        //     IME Hanja mode
        public const uint HANJA = 25;
        //
        // Summary:
        //     IME Kanji mode
        public const uint KANJI = 25;
        //
        // Summary:
        //     ESC key
        public const uint ESCAPE = 27;
        //
        // Summary:
        //     IME convert
        public const uint CONVERT = 28;
        //
        // Summary:
        //     IME nonconvert
        public const uint NONCONVERT = 29;
        //
        // Summary:
        //     IME accept
        public const uint ACCEPT = 30;
        //
        // Summary:
        //     IME mode change request
        public const uint MODECHANGE = 31;
        //
        // Summary:
        //     SPACEBAR
        public const uint SPACE = 32;
        //
        // Summary:
        //     PAGE UP key
        public const uint PRIOR = 33;
        //
        // Summary:
        //     PAGE DOWN key
        public const uint NEXT = 34;
        //
        // Summary:
        //     END key
        public const uint END = 35;
        //
        // Summary:
        //     HOME key
        public const uint HOME = 36;
        //
        // Summary:
        //     LEFT ARROW key
        public const uint LEFT = 37;
        //
        // Summary:
        //     UP ARROW key
        public const uint UP = 38;
        //
        // Summary:
        //     RIGHT ARROW key
        public const uint RIGHT = 39;
        //
        // Summary:
        //     DOWN ARROW key
        public const uint DOWN = 40;
        //
        // Summary:
        //     SELECT key
        public const uint SELECT = 41;
        //
        // Summary:
        //     PRINT key
        public const uint PRINT = 42;
        //
        // Summary:
        //     EXECUTE key
        public const uint EXECUTE = 43;
        //
        // Summary:
        //     PRINT SCREEN key
        public const uint SNAPSHOT = 44;
        //
        // Summary:
        //     INS key
        public const uint INSERT = 45;
        //
        // Summary:
        //     DEL key
        public const uint DELETE = 46;
        //
        // Summary:
        //     HELP key
        public const uint HELP = 47;
        //
        // Summary:
        //     0 key
        public const uint VK_0 = 48;
        //
        // Summary:
        //     1 key
        public const uint VK_1 = 49;
        //
        // Summary:
        //     2 key
        public const uint VK_2 = 50;
        //
        // Summary:
        //     3 key
        public const uint VK_3 = 51;
        //
        // Summary:
        //     4 key
        public const uint VK_4 = 52;
        //
        // Summary:
        //     5 key
        public const uint VK_5 = 53;
        //
        // Summary:
        //     6 key
        public const uint VK_6 = 54;
        //
        // Summary:
        //     7 key
        public const uint VK_7 = 55;
        //
        // Summary:
        //     8 key
        public const uint VK_8 = 56;
        //
        // Summary:
        //     9 key
        public const uint VK_9 = 57;
        //
        // Summary:
        //     A key
        public const uint VK_A = 65;
        //
        // Summary:
        //     B key
        public const uint VK_B = 66;
        //
        // Summary:
        //     C key
        public const uint VK_C = 67;
        //
        // Summary:
        //     D key
        public const uint VK_D = 68;
        //
        // Summary:
        //     E key
        public const uint VK_E = 69;
        //
        // Summary:
        //     F key
        public const uint VK_F = 70;
        //
        // Summary:
        //     G key
        public const uint VK_G = 71;
        //
        // Summary:
        //     H key
        public const uint VK_H = 72;
        //
        // Summary:
        //     I key
        public const uint VK_I = 73;
        //
        // Summary:
        //     J key
        public const uint VK_J = 74;
        //
        // Summary:
        //     K key
        public const uint VK_K = 75;
        //
        // Summary:
        //     L key
        public const uint VK_L = 76;
        //
        // Summary:
        //     M key
        public const uint VK_M = 77;
        //
        // Summary:
        //     N key
        public const uint VK_N = 78;
        //
        // Summary:
        //     O key
        public const uint VK_O = 79;
        //
        // Summary:
        //     P key
        public const uint VK_P = 80;
        //
        // Summary:
        //     Q key
        public const uint VK_Q = 81;
        //
        // Summary:
        //     R key
        public const uint VK_R = 82;
        //
        // Summary:
        //     S key
        public const uint VK_S = 83;
        //
        // Summary:
        //     T key
        public const uint VK_T = 84;
        //
        // Summary:
        //     U key
        public const uint VK_U = 85;
        //
        // Summary:
        //     V key
        public const uint VK_V = 86;
        //
        // Summary:
        //     W key
        public const uint VK_W = 87;
        //
        // Summary:
        //     X key
        public const uint VK_X = 88;
        //
        // Summary:
        //     Y key
        public const uint VK_Y = 89;
        //
        // Summary:
        //     Z key
        public const uint VK_Z = 90;
        //
        // Summary:
        //     Left Windows key (Microsoft Natural keyboard)
        public const uint LWIN = 91;
        //
        // Summary:
        //     Right Windows key (Natural keyboard)
        public const uint RWIN = 92;
        //
        // Summary:
        //     Applications key (Natural keyboard)
        public const uint APPS = 93;
        //
        // Summary:
        //     Computer Sleep key
        public const uint SLEEP = 95;
        //
        // Summary:
        //     Numeric keypad 0 key
        public const uint NUMPAD0 = 96;
        //
        // Summary:
        //     Numeric keypad 1 key
        public const uint NUMPAD1 = 97;
        //
        // Summary:
        //     Numeric keypad 2 key
        public const uint NUMPAD2 = 98;
        //
        // Summary:
        //     Numeric keypad 3 key
        public const uint NUMPAD3 = 99;
        //
        // Summary:
        //     Numeric keypad 4 key
        public const uint NUMPAD4 = 100;
        //
        // Summary:
        //     Numeric keypad 5 key
        public const uint NUMPAD5 = 101;
        //
        // Summary:
        //     Numeric keypad 6 key
        public const uint NUMPAD6 = 102;
        //
        // Summary:
        //     Numeric keypad 7 key
        public const uint NUMPAD7 = 103;
        //
        // Summary:
        //     Numeric keypad 8 key
        public const uint NUMPAD8 = 104;
        //
        // Summary:
        //     Numeric keypad 9 key
        public const uint NUMPAD9 = 105;
        //
        // Summary:
        //     Multiply key
        public const uint MULTIPLY = 106;
        //
        // Summary:
        //     Add key
        public const uint ADD = 107;
        //
        // Summary:
        //     Separator key
        public const uint SEPARATOR = 108;
        //
        // Summary:
        //     Subtract key
        public const uint SUBTRACT = 109;
        //
        // Summary:
        //     Decimal key
        public const uint DECIMAL = 110;
        //
        // Summary:
        //     Divide key
        public const uint DIVIDE = 111;
        //
        // Summary:
        //     F1 key
        public const uint F1 = 112;
        //
        // Summary:
        //     F2 key
        public const uint F2 = 113;
        //
        // Summary:
        //     F3 key
        public const uint F3 = 114;
        //
        // Summary:
        //     F4 key
        public const uint F4 = 115;
        //
        // Summary:
        //     F5 key
        public const uint F5 = 116;
        //
        // Summary:
        //     F6 key
        public const uint F6 = 117;
        //
        // Summary:
        //     F7 key
        public const uint F7 = 118;
        //
        // Summary:
        //     F8 key
        public const uint F8 = 119;
        //
        // Summary:
        //     F9 key
        public const uint F9 = 120;
        //
        // Summary:
        //     F10 key
        public const uint F10 = 121;
        //
        // Summary:
        //     F11 key
        public const uint F11 = 122;
        //
        // Summary:
        //     F12 key
        public const uint F12 = 123;
        //
        // Summary:
        //     F13 key
        public const uint F13 = 124;
        //
        // Summary:
        //     F14 key
        public const uint F14 = 125;
        //
        // Summary:
        //     F15 key
        public const uint F15 = 126;
        //
        // Summary:
        //     F16 key
        public const uint F16 = 127;
        //
        // Summary:
        //     F17 key
        public const uint F17 = 128;
        //
        // Summary:
        //     F18 key
        public const uint F18 = 129;
        //
        // Summary:
        //     F19 key
        public const uint F19 = 130;
        //
        // Summary:
        //     F20 key
        public const uint F20 = 131;
        //
        // Summary:
        //     F21 key
        public const uint F21 = 132;
        //
        // Summary:
        //     F22 key
        public const uint F22 = 133;
        //
        // Summary:
        //     F23 key
        public const uint F23 = 134;
        //
        // Summary:
        //     F24 key
        public const uint F24 = 135;
        //
        // Summary:
        //     NUM LOCK key
        public const uint NUMLOCK = 144;
        //
        // Summary:
        //     SCROLL LOCK key
        public const uint SCROLL = 145;
        //
        // Summary:
        //     Left SHIFT key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        public const uint LSHIFT = 160;
        //
        // Summary:
        //     Right SHIFT key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        public const uint RSHIFT = 161;
        //
        // Summary:
        //     Left CONTROL key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        public const uint LCONTROL = 162;
        //
        // Summary:
        //     Right CONTROL key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        public const uint RCONTROL = 163;
        //
        // Summary:
        //     Left MENU key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        public const uint LMENU = 164;
        //
        // Summary:
        //     Right MENU key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        public const uint RMENU = 165;
        //
        // Summary:
        //     Windows 2000/XP: Browser Back key
        public const uint BROWSER_BACK = 166;
        //
        // Summary:
        //     Windows 2000/XP: Browser Forward key
        public const uint BROWSER_FORWARD = 167;
        //
        // Summary:
        //     Windows 2000/XP: Browser Refresh key
        public const uint BROWSER_REFRESH = 168;
        //
        // Summary:
        //     Windows 2000/XP: Browser Stop key
        public const uint BROWSER_STOP = 169;
        //
        // Summary:
        //     Windows 2000/XP: Browser Search key
        public const uint BROWSER_SEARCH = 170;
        //
        // Summary:
        //     Windows 2000/XP: Browser Favorites key
        public const uint BROWSER_FAVORITES = 171;
        //
        // Summary:
        //     Windows 2000/XP: Browser Start and Home key
        public const uint BROWSER_HOME = 172;
        //
        // Summary:
        //     Windows 2000/XP: Volume Mute key
        public const uint VOLUME_MUTE = 173;
        //
        // Summary:
        //     Windows 2000/XP: Volume Down key
        public const uint VOLUME_DOWN = 174;
        //
        // Summary:
        //     Windows 2000/XP: Volume Up key
        public const uint VOLUME_UP = 175;
        //
        // Summary:
        //     Windows 2000/XP: Next Track key
        public const uint MEDIA_NEXT_TRACK = 176;
        //
        // Summary:
        //     Windows 2000/XP: Previous Track key
        public const uint MEDIA_PREV_TRACK = 177;
        //
        // Summary:
        //     Windows 2000/XP: Stop Media key
        public const uint MEDIA_STOP = 178;
        //
        // Summary:
        //     Windows 2000/XP: Play/Pause Media key
        public const uint MEDIA_PLAY_PAUSE = 179;
        //
        // Summary:
        //     Windows 2000/XP: Start Mail key
        public const uint LAUNCH_MAIL = 180;
        //
        // Summary:
        //     Windows 2000/XP: Select Media key
        public const uint LAUNCH_MEDIA_SELECT = 181;
        //
        // Summary:
        //     Windows 2000/XP: Start Application 1 key
        public const uint LAUNCH_APP1 = 182;
        //
        // Summary:
        //     Windows 2000/XP: Start Application 2 key
        public const uint LAUNCH_APP2 = 183;
        //
        // Summary:
        //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
        //     For the US standard keyboard; the ';:' key
        public const uint OEM_1 = 186;
        //
        // Summary:
        //     Windows 2000/XP: For any country/region; the '+' key
        public const uint OEM_PLUS = 187;
        //
        // Summary:
        //     Windows 2000/XP: For any country/region; the ';' key
        public const uint OEM_COMMA = 188;
        //
        // Summary:
        //     Windows 2000/XP: For any country/region; the '-' key
        public const uint OEM_MINUS = 189;
        //
        // Summary:
        //     Windows 2000/XP: For any country/region; the '.' key
        public const uint OEM_PERIOD = 190;
        //
        // Summary:
        //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
        //     For the US standard keyboard; the '/?' key
        public const uint OEM_2 = 191;
        //
        // Summary:
        //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
        //     For the US standard keyboard; the '`~' key
        public const uint OEM_3 = 192;
        //
        // Summary:
        //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
        //     For the US standard keyboard; the '[{' key
        public const uint OEM_4 = 219;
        //
        // Summary:
        //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
        //     For the US standard keyboard; the '\|' key
        public const uint OEM_5 = 220;
        //
        // Summary:
        //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
        //     For the US standard keyboard; the ']}' key
        public const uint OEM_6 = 221;
        //
        // Summary:
        //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
        //     For the US standard keyboard; the 'single-quote/double-quote' key
        public const uint OEM_7 = 222;
        //
        // Summary:
        //     Used for miscellaneous characters; it can vary by keyboard.
        public const uint OEM_8 = 223;
        //
        // Summary:
        //     Windows 2000/XP: Either the angle bracket key or the backslash key on the RT
        //     102-key keyboard
        public const uint OEM_102 = 226;
        //
        // Summary:
        //     Windows 95/98/Me; Windows NT 4.0; Windows 2000/XP: IME PROCESS key
        public const uint PROCESSKEY = 229;
        //
        // Summary:
        //     Windows 2000/XP: Used to pass Unicode characters as if they were keystrokes.
        //     The PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard
        //     input methods. For more information; see Remark in KEYBDINPUT; SendInput; WM_KEYDOWN;
        //     and WM_KEYUP
        public const uint PACKET = 231;
        //
        // Summary:
        //     Attn key
        public const uint ATTN = 246;
        //
        // Summary:
        //     CrSel key
        public const uint CRSEL = 247;
        //
        // Summary:
        //     ExSel key
        public const uint EXSEL = 248;
        //
        // Summary:
        //     Erase EOF key
        public const uint EREOF = 249;
        //
        // Summary:
        //     Play key
        public const uint PLAY = 250;
        //
        // Summary:
        //     Zoom key
        public const uint ZOOM = 251;
        //
        // Summary:
        //     Reserved
        public const uint NONAME = 252;
        //
        // Summary:
        //     PA1 key
        public const uint PA1 = 253;
        //
        // Summary:
        //     Clear key
        public const uint OEM_CLEAR = 254;
    }

}