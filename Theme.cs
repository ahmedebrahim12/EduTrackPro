using System.Drawing;
using System.Drawing.Drawing2D;

namespace EduTrackPro
{
    public static class Theme
    {
        public static Color PrimaryBlue = Color.FromArgb(0, 82, 204);
        public static Color SecondaryBlue = Color.FromArgb(230, 240, 255);
        public static Color SidebarBg = Color.White;
        public static Color ContentBg = Color.FromArgb(244, 247, 252);
        public static Color TextDark = Color.FromArgb(30, 39, 46);
        public static Color TextLight = Color.FromArgb(128, 142, 155);
        public static Color BackgroundGray = Color.FromArgb(240, 242, 245);
        public static Color BorderGray = Color.FromArgb(220, 220, 220);
        
        // Additional Colors for Stats
        public static Color SecondaryGreen = Color.FromArgb(46, 213, 115);
        public static Color AccentOrange = Color.FromArgb(255, 165, 2);
        public static Color StatusPurple = Color.FromArgb(165, 94, 234);

        public static Font HeaderFont = new Font("Segoe UI", 18, FontStyle.Bold);
        public static Font SubHeaderFont = new Font("Segoe UI", 14, FontStyle.Bold);
        public static Font BodyFont = new Font("Segoe UI", 10);
        public static Font SmallFont = new Font("Segoe UI", 9);

        public static GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float r2 = radius * 2f;
            path.AddArc(rect.X, rect.Y, r2, r2, 180, 90);
            path.AddArc(rect.Right - r2, rect.Y, r2, r2, 270, 90);
            path.AddArc(rect.Right - r2, rect.Bottom - r2, r2, r2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - r2, r2, r2, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
