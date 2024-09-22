using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public static class IllegalWindowController
{
    private const int SC_CLOSE = 0xF060;
    private const int MF_GRAYED = 0x0001;
    private const int MF_BYCOMMAND = 0x0000;
    private const int MF_ENABLED = 0x0000; // Butonu etkinleştirmek için

    [DllImport("user32.dll")]
    private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("user32.dll")]
    private static extern bool EnableMenuItem(IntPtr hMenu, int uIDEnableItem, int uEnable);

    // Kapatma (çarpı) tuşunu devre dışı bırakma fonksiyonu
    public static void DisableCloseButton(IntPtr windowHandle)
    {
        IntPtr hMenu = GetSystemMenu(windowHandle, false);
        EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED | MF_BYCOMMAND);
    }

    // Kapatma (çarpı) tuşunu aktif hale getirme fonksiyonu
    public static void EnableCloseButton(IntPtr windowHandle)
    {
        IntPtr hMenu = GetSystemMenu(windowHandle, false);
        EnableMenuItem(hMenu, SC_CLOSE, MF_ENABLED | MF_BYCOMMAND);
    }
}