using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EduTrackPro.Components
{
    public class CustomButton : Button
    {
        public int BorderRadius { get; set; } = 8;
        public Color NormalColor { get; set; } = Theme.PrimaryBlue;
        public Color HoverColor { get; set; } = ColorTranslator.FromHtml("#0747A6");
        public Color PressedColor { get; set; } = ColorTranslator.FromHtml("#00388D");
        
        private bool isHovered = false;
        private bool isPressed = false;

        public CustomButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = NormalColor;
            ForeColor = Color.White;
            Font = Theme.BodyFont;
            Size = new Size(120, 40);
            Cursor = Cursors.Hand;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            isHovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            isHovered = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            isPressed = true;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            isPressed = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            pevent.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Color currentColor = isPressed ? PressedColor : (isHovered ? HoverColor : NormalColor);
            
            using (var path = Theme.GetRoundedPath(ClientRectangle, BorderRadius))
            using (var brush = new SolidBrush(currentColor))
            {
                pevent.Graphics.FillPath(brush, path);
                
                TextRenderer.DrawText(pevent.Graphics, Text, Font, ClientRectangle, ForeColor, 
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }
    }

    public class CustomCard : Panel
    {
        public int BorderRadius { get; set; } = 12;
        public Color CardColor { get; set; } = Color.White;
        public bool ShowShadow { get; set; } = true;

        public CustomCard()
        {
            BackColor = Color.Transparent;
            Padding = new Padding(20);
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw Shadow
            if (ShowShadow)
            {
                using (var shadowPath = Theme.GetRoundedPath(new Rectangle(2, 2, Width - 4, Height - 4), BorderRadius))
                using (var shadowBrush = new SolidBrush(Color.FromArgb(20, Color.Black)))
                {
                    e.Graphics.FillPath(shadowBrush, shadowPath);
                }
            }

            Rectangle rect = new Rectangle(0, 0, Width - 3, Height - 3);
            using (var path = Theme.GetRoundedPath(rect, BorderRadius))
            using (var brush = new SolidBrush(CardColor))
            {
                e.Graphics.FillPath(brush, path);
                using (var pen = new Pen(Theme.BorderGray, 1))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }
    }
}
