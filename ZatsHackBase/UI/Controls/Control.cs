using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core.Timing;
using ZatsHackBase.Input;
using ZatsHackBase.Maths;
using ZatsHackBase.UI.Controls.Events;
using ZatsHackBase.UI.Controls.Layouts;
using ZatsHackBase.Drawing;

namespace ZatsHackBase.UI.Controls
{
    public abstract class Control
    {
        private bool mouseOver, visible, drawText, drawBackground, drawBorder, enabled;
        private string text, name;
        private Font font;
        private Vector2 size, position;
        private Control parent;
        private Color backColor, foreColor, borderColor;
        private List<Control> children;
        private TextAlignment textAlignment;
        private Distance padding, margin;
        private ILayout layout;

        public event EventHandler<Vector2DiffArgs> PositionChanged;
        protected virtual void OnPositionChanged(Vector2DiffArgs e) { PositionChanged?.Invoke(this, e); }

        public event EventHandler<Vector2DiffArgs> SizeChanged;
        protected virtual void OnSizeChanged(Vector2DiffArgs e) { SizeChanged?.Invoke(this, e); }

        public event EventHandler<ControlDiffArgs> ParentChanged;
        protected virtual void OnParentChanged(ControlDiffArgs e) { ParentChanged?.Invoke(this, e); }

        public event EventHandler FontChanged;
        protected virtual void OnFontChanged(EventArgs e) { FontChanged?.Invoke(this, e); }

        public event EventHandler TextChanged;
        protected virtual void OnTextChanged(EventArgs e) { TextChanged?.Invoke(this, e); }

        public event EventHandler BackColorChanged;
        protected virtual void OnBackColorChanged(EventArgs e) { BackColorChanged?.Invoke(this, e); }

        public event EventHandler ForeColorChanged;
        protected virtual void OnForeColorChanged(EventArgs e) { ForeColorChanged?.Invoke(this, e); }

        public event EventHandler BorderColorChanged;
        protected virtual void OnBorderColorChanged(EventArgs e) { BorderColorChanged?.Invoke(this, e); }

        public event EventHandler MouseEntered;
        protected virtual void OnMouseEntered(EventArgs e) { MouseEntered?.Invoke(this, e); }

        public event EventHandler MouseLeft;
        protected virtual void OnMouseLeft(EventArgs e) { MouseLeft?.Invoke(this, e); }

        public event EventHandler VisibleChanged;
        protected virtual void OnVisibleChanged(EventArgs e) { VisibleChanged?.Invoke(this, e); }

        public event EventHandler DrawTextChanged;
        protected virtual void OnDrawTextChanged(EventArgs e) { DrawTextChanged?.Invoke(this, e); }

        public event EventHandler MarginChanged;
        protected virtual void OnMarginChanged(EventArgs e) { MarginChanged?.Invoke(this, e); }

        public event EventHandler PaddingChanged;
        protected virtual void OnPaddingChanged(EventArgs e) { PaddingChanged?.Invoke(this, e); }

        public event EventHandler LayoutChanged;
        protected virtual void OnLayoutChanged(EventArgs e) { LayoutChanged?.Invoke(this, e); }

        public event EventHandler DrawBackgroundChanged;
        protected virtual void OnDrawBackgroundChanged(EventArgs e) { DrawBackgroundChanged?.Invoke(this, e); }

        public event EventHandler DrawBorderChanged;
        protected virtual void OnDrawBorderChanged(EventArgs e) { DrawBorderChanged?.Invoke(this, e); }

        public event EventHandler TextAlignmentChanged;
        protected virtual void OnTextAlignmentChanged(EventArgs e) { TextAlignmentChanged?.Invoke(this, e); }

        public event EventHandler EnabledChanged;
        protected virtual void OnEnabledChanged(EventArgs e) { EnabledChanged?.Invoke(this, e); }

        public event EventHandler<ControlArgs> ChildAdded;
        protected virtual void OnChildAdded(ControlArgs e) { ChildAdded?.Invoke(this, e); }

        public event EventHandler<ControlArgs> ChildRemoved;
        protected virtual void OnChildRemoved(ControlArgs e) { ChildRemoved?.Invoke(this, e); }

        #region PROPERTIES
        public string Name { get { return name; } set { name = value; } }
        public float Left { get { return position.X; } }
        public float Top { get { return position.Y; } }
        public float Bottom { get { return position.Y + size.Y; } }
        public float Right { get { return position.X + size.X; } }
        public Vector2 Center { get { return position + size * 0.5f; } }
        public Rectangle Bounds { get { return new Rectangle(position, size); } }
        public Object Tag { get; set; }

        public Control Parent
        {
            get { return parent; }
            set
            {
                if(value != parent)
                {

                    var old = parent;
                    parent = value;

                    if (old != null)
                        old.RemoveChild(this);

                    if (parent != null)
                        parent.AddChild(this);

                    OnParentChanged(new ControlDiffArgs(old, value));
                }
            }
        }
        public IEnumerable<Control> Children { get { return children; } }

        public bool IsVisible
        {
            get
            {
                if (parent == null)
                    return visible;
                return visible & parent.IsVisible;
            }
        }
        public Vector2 AbsolutePosition
        {
            get
            {
                if (parent == null)
                    return position;
                return parent.AbsolutePosition + position;
            }
        }
        public Rectangle AbsoluteBounds
        {
            get
            {
                if (parent == null)
                    return Bounds;
                return new Rectangle(AbsolutePosition, size);
            }
        }

        public bool Visible
        {
            get { return visible; }
            set
            {
                if (visible != value)
                {
                    visible = value;
                    OnVisibleChanged(null);
                }
            }
        }

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (enabled != value)
                {
                    enabled = value;
                    OnEnabledChanged(null);
                }
            }
        }

        public ILayout Layout
        {
            get { return layout; }
            protected set
            {
                if (layout != value)
                {
                    layout = value;
                    OnLayoutChanged(null);
                }
            }
        }

        public Distance Padding
        {
            get { return padding; }
            set
            {
                if (padding != value)
                {
                    padding = value;
                    OnPaddingChanged(null);
                }
            }
        }

        public Distance Margin
        {
            get { return margin; }
            set
            {
                if (margin != value)
                {
                    margin = value;
                    OnMarginChanged(null);
                }
            }
        }

        public TextAlignment TextAlignment
        {
            get { return textAlignment; }
            set
            {
                if (textAlignment != value)
                {
                    textAlignment = value;
                    OnTextAlignmentChanged(null);
                }
            }
        }

        public bool DrawBorder
        {
            get { return drawBorder; }
            set
            {
                if (drawBorder != value)
                {
                    drawBorder = value;
                    OnDrawBorderChanged(null);
                }
            }
        }

        public bool DrawBackground
        {
            get { return drawBackground; }
            set
            {
                if (drawBackground != value)
                {
                    drawBackground = value;
                    OnDrawBackgroundChanged(null);
                }
            }
        }

        public bool DrawText
        {
            get { return drawText; }
            protected set
            {
                if (drawText != value)
                {
                    drawText = value;
                    OnDrawTextChanged(null);
                }
            }
        }
        public bool MouseOver
        {
            get { return mouseOver; }
            protected set
            {
                if(mouseOver != value)
                {
                    mouseOver = value;
                    if (value)
                        OnMouseEntered(null);
                    else
                        OnMouseLeft(null);
                }
            }
        }
        public Color ForeColor
        {
            get { return foreColor; }
            set
            {
                if (foreColor != value)
                {
                    foreColor = value;
                    OnForeColorChanged(null);
                }
            }
        }
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                if (borderColor != value)
                {
                    borderColor = value;
                    OnBorderColorChanged(null);
                }
            }
        }
        public Color BackColor
        {
            get { return backColor; }
            set
            {
                if (backColor != value)
                {
                    backColor = value;
                    OnBackColorChanged(null);
                }
            }
        }
        public Font Font
        {
            get { return font; }
            set
            {
                if(font != value)
                {
                    font = value;
                    OnFontChanged(null);
                }
            }
        }
        public String Text
        {
            get { return text; }
            set
            {
                if (text != value)
                {
                    text = value;
                    OnTextChanged(null);
                }
            }
        }

        public Vector2 Position
        {
            get { return position; }
            set
            {
                if (value != position)
                {
                    var old = position;
                    position = value;
                    OnPositionChanged(new Vector2DiffArgs(old, value));
                }
            }
        }
        public float X
        {
            get { return position.X; }
            set
            {
                Position = new Vector2(value, position.Y);
            }
        }
        public float Y
        {
            get { return position.Y; }
            set
            {
                Position = new Vector2(position.X, value);
            }
        }

        public Vector2 Size
        {
            get { return size; }
            set
            {
                if (value != size)
                {
                    var old = size;
                    size = value;
                    OnSizeChanged(new Vector2DiffArgs(old, value));
                }
            }
        }
        public float Width
        {
            get { return size.X; }
            set
            {
                Size = new Vector2(value, size.Y);
            }
        }
        public float Height
        {
            get { return size.Y; }
            set
            {
                Size = new Vector2(size.X, value);
            }
        }
        #endregion

        public Control()
        {
            drawBackground = drawText = drawBorder = true;
            foreColor = Color.Black;
            backColor = Color.White;
            borderColor = new Color(0.5f, 0.5f, 0.5f);
            children = new List<Control>();
            position = Vector2.Zero;
            size = Vector2.Zero;
            padding = new Distance(2f);
            margin = new Distance(2f);
            visible = true;
            enabled = true;
        }

        #region METHODS
        public void AddChildren(IEnumerable<Control> children)
        {
            foreach (var c in children)
                AddChild(c, false);

            if (layout != null)
                layout.ApplyLayout(this);
        }
        public void AddChild(Control child, bool updateLayout = true)
        {
            if (children.Contains(child))
                return;

            if (child.Parent != null)
                child.Parent.RemoveChild(child);

            children.Add(child);
            child.Parent = this;
            if (updateLayout && layout != null)
                layout.ApplyLayout(this);
            OnChildAdded(new ControlArgs(child));
        }

        public void RemoveChildren(IEnumerable<Control> children)
        {
            foreach (var c in children)
                RemoveChild(c, false);

            if (layout != null)
                layout.ApplyLayout(this);
        }
        public void RemoveChild(Control child, bool updateLayout = true)
        {
            if (!children.Contains(child))
                return;

            children.Remove(child);
            child.Parent = null;
            if (updateLayout && layout != null)
                layout.ApplyLayout(this);
            OnChildRemoved(new ControlArgs(child));
        }

        public virtual void Draw(Graphics g)
        {
            if (font != null)
                font = g.FindFont(font);

            foreach (var c in children)
                if (c.Visible)
                    c.Draw(g);
        }

        public virtual void Update(Time time, HackInput input, Vector2 cursorPos)
        {
            if (!IsVisible)
                return;

            foreach (var c in children)
                c.Update(time, input, cursorPos);
        }
        #endregion
    }
}
