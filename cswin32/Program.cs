using System.Runtime.InteropServices;
using static Windows.Win32.PInvoke;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.Graphics.Gdi;
using System.ComponentModel;
using Windows.Win32;

namespace CsWin32
{
    static unsafe class Program
    {
        [DllImport("coredll.dll")]
        public static extern IntPtr GetModuleHandle(IntPtr lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetCurrentProcess();

        static char* GetStringPointer(string str)
        {
            fixed (char* ptr = str)
            {
                return ptr;
            }
        }
        static HWND hwnd;

        static void Main(string[] args)
        {
            string CLASS_NAME = "Sample Window Class";
            string WINDOW_NAME = "Learn how to program C#/Win32";
            char* CLASS_NAME_P = GetStringPointer(CLASS_NAME);
            char* WINDOW_NAME_P = GetStringPointer(WINDOW_NAME);

            WNDCLASSEXW wcex = new();
            wcex.cbSize = (uint)sizeof(WNDCLASSEXW);
            wcex.style = 0;
            wcex.lpfnWndProc = WndProc;
            wcex.cbClsExtra = 0;
            wcex.cbWndExtra = 0;
            wcex.hInstance = new HINSTANCE(GetCurrentProcess());
            wcex.hIcon = HICON.Null;
            wcex.hCursor = HCURSOR.Null;
            wcex.hbrBackground = new HBRUSH(new IntPtr((int)SYS_COLOR_INDEX.COLOR_WINDOW + 1));
            wcex.lpszMenuName = new PCWSTR((char*)0);
            wcex.lpszClassName = new PCWSTR(CLASS_NAME_P);
            wcex.hIconSm = HICON.Null;


            if (RegisterClassEx(wcex) == 0)
            {
                throw new Win32Exception("Window Registration failed");
            }

            hwnd = CreateWindowEx((WINDOW_EX_STYLE)0, new PCWSTR(CLASS_NAME_P), new PCWSTR(WINDOW_NAME_P), WINDOW_STYLE.WS_OVERLAPPEDWINDOW, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, HWND.Null, HMENU.Null, new HINSTANCE(GetCurrentProcess()), null);

            if (hwnd == HWND.Null)
            {
                throw new Win32Exception("The window can't be created");
            }

            ShowWindow(hwnd, SHOW_WINDOW_CMD.SW_SHOWNORMAL);

            MSG msg = new MSG();

            while (GetMessage(out msg, hwnd, 0, 0))
            {
                TranslateMessage(msg);
                DispatchMessage(msg);
            }
            return;
        }
        
        public static LRESULT WndProc(HWND hwnd, uint msg, WPARAM wParam, LPARAM lParam)
        {
            switch (msg)
            {
                case WM_CREATE:
                    
                    break;
                case WM_DESTROY:
                    PostQuitMessage(0);
                    break;
                case WM_PAINT:
                    PAINTSTRUCT ps;
                    HDC hdc = BeginPaint(hwnd, out ps);
                    FillRect(hdc, &ps.rcPaint, new HBRUSH(new IntPtr((int)SYS_COLOR_INDEX.COLOR_WINDOW + 1)));
                    EndPaint(hwnd, ps);
                    break;
            }
            return DefWindowProc(hwnd, msg, wParam, lParam);
        }
    }
}
