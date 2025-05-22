using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GUI {
    public partial class TextBoxPanel : UserControl {
        [Bindable(true)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Misc")]
        public override string Text { get => TextBox.Text; set => TextBox.Text = value; }

        public override Font Font { get => TextBox.Font; }
        public Font TextFont { get => TextBox.Font; set => TextBox.Font = value; }

        public override Color BackColor => base.BackColor;
        public override Color ForeColor => TextBox.ForeColor;

        private Color _activeBackColor = Color.FromArgb(45, 45, 45);
        private Color _activeForeColor = SystemColors.HighlightText;

        private Color _disabledBackColor = Color.FromArgb(52, 52, 52);
        private Color _disabledForeColor = Color.FromArgb(145, 145, 145);

        public Color ActiveBackColor {
            get => _activeBackColor;
            set {
                _activeBackColor = value;

                SetColor(TextBox.ReadOnly);
            }
        }
        public Color ActiveForeColor {
            get => _activeForeColor;
            set {
                _activeForeColor = value;

                SetColor(TextBox.ReadOnly);
            }
        }

        public Color DisabledBackColor {
            get => _disabledBackColor;
            set {
                _disabledBackColor = value;

                SetColor(TextBox.ReadOnly);
            }
        }
        public Color DisabledForeColor {
            get => _disabledForeColor;
            set {
                _disabledForeColor = value;

                SetColor(TextBox.ReadOnly);
            }
        }

        public bool ReadOnly {
            get => TextBox.ReadOnly;
            set {
                TextBox.ReadOnly = value;

                SetColor(value);
            }
        }
        public Padding TextPadding { get; set; }

        //Event
        private event EventHandler _textChange;
        public event EventHandler TextChange {
            add {
                TextBox.TextChanged += OnTextChanged;
                _textChange += value;
            }
            remove {
                TextBox.TextChanged -= OnTextChanged;
                _textChange -= value;
            }
        }

        private void OnTextChanged(object sender, EventArgs e) {
            if (_textChange != null)
                _textChange.Invoke(this, EventArgs.Empty);
        }

        public TextBoxPanel(){
            InitializeComponent();

            SetColor(TextBox.ReadOnly);
            ResizePanel();
        }

        private void SetColor(bool readOnly) {
            if (readOnly) {
                base.BackColor = _disabledBackColor;
                TextBox.BackColor = _disabledBackColor;
                TextBox.ForeColor = _disabledForeColor;
            } else {
                base.BackColor = _activeBackColor;
                TextBox.BackColor = _activeBackColor;
                TextBox.ForeColor = _activeForeColor;
            }
        }
        private void ResizePanel(){
            Panel.Padding = new Padding(
                TextPadding.Left + 5,
                (Height - TextBox.Height) / 2 + TextPadding.Top,
                TextPadding.Right,
                TextPadding.Bottom
            );
        }

        private void TextBoxPanelNew_Resize(object sender, EventArgs e){
            ResizePanel();
        }
        private void TextBoxPanelNew_Load(object sender, EventArgs e) {
            ResizePanel();
        }
    }
}
