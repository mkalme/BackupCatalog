using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace GUI {
    public static class IconManager {
        public enum IconSize {
            Large = 0,
            Small = 1
        }
        public static class IconReader {
            public static Icon GetFileIcon(string name, IconSize size){
                var shfi = new Shell32.Shfileinfo();
                var flags = Shell32.ShgfiIcon | Shell32.ShgfiUsefileattributes;

                /* Check the size specified for return. */
                if (IconSize.Small == size)
                    flags += Shell32.ShgfiSmallicon;
                else
                    flags += Shell32.ShgfiLargeicon;

                Shell32.SHGetFileInfo(name,
                    Shell32.FileAttributeNormal,
                    ref shfi,
                    (uint)Marshal.SizeOf(shfi),
                    flags);

                // Copy (clone) the returned icon to a new object, thus allowing us to clean-up properly
                var icon = (Icon)Icon.FromHandle(shfi.hIcon).Clone();
                User32.DestroyIcon(shfi.hIcon);     // Cleanup

                return icon;
            }
        }
        static class Shell32 {
            private const int MaxPath = 256;
            [StructLayout(LayoutKind.Sequential)]
            public struct Shfileinfo {
                private const int Namesize = 80;
                public readonly IntPtr hIcon;
                private readonly int iIcon;
                private readonly uint dwAttributes;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MaxPath)]
                private readonly string szDisplayName;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Namesize)]
                private readonly string szTypeName;
            };
            public const uint ShgfiIcon = 0x000000100;     // get icon
            public const uint ShgfiLinkoverlay = 0x000008000;     // put a link overlay on icon
            public const uint ShgfiLargeicon = 0x000000000;     // get large icon
            public const uint ShgfiSmallicon = 0x000000001;     // get small icon
            public const uint ShgfiUsefileattributes = 0x000000010;     // use passed dwFileAttribute
            public const uint FileAttributeNormal = 0x00000080;
            [DllImport("Shell32.dll")]
            public static extern IntPtr SHGetFileInfo(
                string pszPath,
                uint dwFileAttributes,
                ref Shfileinfo psfi,
                uint cbFileInfo,
                uint uFlags
                );
        }
        static class User32 {
            [DllImport("User32.dll")]
            public static extern int DestroyIcon(IntPtr hIcon);
        }
    }
}
