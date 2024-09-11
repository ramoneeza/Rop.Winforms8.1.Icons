using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.IncludeFrom.Annotations;
using Rop.Winforms8.DuotoneIcons.PartialControls;

namespace Rop.Winforms8.DuotoneIcons.Controls
{
    internal partial class Dummy{}
    [IncludeFrom(typeof(PartialIHasBank))]
    public partial class ColumnPanel:Panel,IShowHidden, IHasBank
    {
        public ColumnPanel()
        {
            InitShowHidden();
        }
        private readonly List<OrderIcon> _columns = new();
        public event EventHandler? SelectedChanged;
        public event EventHandler<ColumnPanelOrderArgs>? OrderChanged;
        public int NumberOfColumns
        {
            get => _columns.Count;
            set
            {
                if (value== _columns.Count) return;
                if (value < _columns.Count)
                {
                    while (_columns.Count > value)
                    {
                        var c = _columns.Last();
                        _columns.Remove(c);
                        Controls.Remove(c);
                        c.Dispose();
                    }
                }
                else
                {
                    while (_columns.Count<value)
                    {
                        _addcolumn();
                    }
                }
                Invalidate();
            }
        }
        private void _addcolumn()
        {
            var acolumn=_columns.LastOrDefault();
            var column = new OrderIcon();
            column.ColumnIndex = _columns.Count;
            column.SortOrder = SortOrder.None;
            column.IconColorSelected = IconColorSelected;
            column.IconColorUnSelected = IconColorUnSelected;
            column.IconUnselected = IconUnselected;
            column.IconAscending = IconAscending;
            column.IconDescending = IconDescending;
            column.AutoSize = false;
            column.TextAlign = ContentAlignment.MiddleLeft;
            column.IconScale = IconScale;
            column.Font = Font;
            column.OffsetIcon = OffsetIcon;
            column.Left= acolumn?.Right ?? 0;
            column.Top = 0;
            column.Width = 100;
            column.Height = Height;
            column.Text = $"Col {column.ColumnIndex}";
            _columns.Add(column);
            this.Controls.Add(column);
            column.MouseDown += C_Click;
            column.PostPaint += Column_Paint1;
        }

        private void Column_Paint1(object? sender, PaintEventArgs e)
        {
            if (!_interiorborder) return;
            if (sender is not OrderIcon column) return;
            e.Graphics.DrawLine(Pens.White, 0, 0, 0, column.Height);
            e.Graphics.DrawLine(new Pen(Color.Silver), column.Width - 1,0,column.Width-1, Height);
        }


        private OrderIcon? _selected = null;
        public OrderIcon? Selected
        {
            get => _selected;
            set
            {
                if (_selected == value) return;
                foreach (var c in _columns)
                    c.Selected = c == value;
                _selected = value;
                SelectedChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public int SelectedColumn
        {
            get => _selected?.ColumnIndex ?? -1;
            set => Selected = _getcolumnlabel(value);
        }
        public SortOrder SelectedOrder=> _selected?.SortOrder ?? SortOrder.None;
        public bool Ascending => _selected?.Ascending ?? true;

        private void C_Click(object? sender, EventArgs e)
        {
            if (sender is not OrderIcon { Selectable: true } oi) return;
            if (oi==Selected)
            {
                oi.Ascending = !oi.Ascending;
            }
            else
            {
                Selected = oi;
            }
            OrderChanged?.Invoke(this, new ColumnPanelOrderArgs(new ColumnPanelOrder { ColumnIndex = oi.ColumnIndex, SortOrder = oi.SortOrder }));
        }

        private OrderIcon? _getcolumnlabel(int index)
        {
            if (index < 0 || index >= _columns.Count) return null;
            return _columns[index];
        }
        public Rectangle[] ColumnsBounds => _columns.Select(c => c.Bounds).ToArray();
        
        private void _doaction(Action<OrderIcon> action)
        {
            foreach (var c in _columns)
            {
                action(c);
            }
        }

        private void _updateproperties()
        {
            _doaction(c =>
            {
                c.IconColorSelected = IconColorSelected;
                c.IconColorUnSelected = IconColorUnSelected;
                c.IconUnselected = IconUnselected;
                c.IconAscending = IconAscending;
                c.IconDescending = IconDescending;
                c.IconScale = IconScale;
                c.Font = Font;
                c.OffsetIcon = OffsetIcon;
                c.Padding = ColumnsPadding;
                c.BackColor = ColumnsBackColor;
            });
        }
        public int[] ColumnsWidth
        {
            get => _columns.Select(c => c.Width).ToArray();
            set
            {
                if (value.Length > NumberOfColumns)
                {
                    NumberOfColumns = value.Length;
                }
                var m = Math.Min(value.Length, NumberOfColumns);
                for (var i = 0; i < m; i++)
                {
                    _columns[i].Width = value[i];
                }
                _doLayout();
            }
        }
        public string[] ColumnsNames
        {
            get => _columns.Select(c => c.Text).ToArray();
            set
            {
                if (value.Length > NumberOfColumns)
                {
                    NumberOfColumns = value.Length;
                }
                var m = Math.Min(value.Length, NumberOfColumns);
                for (var i = 0; i < m; i++)
                {
                    _columns[i].Text = value[i];
                }
            }
        }
        public bool[] EnabledColumns
        {
            get => _columns.Select(c => c.Enabled).ToArray();
            set
            {
                if (value.Length > NumberOfColumns)
                {
                    NumberOfColumns = value.Length;
                }
                var m = Math.Min(value.Length, NumberOfColumns);
                for (var i = 0; i < m; i++)
                {
                    _columns[i].Enabled = value[i];
                }
            }
        }
        public bool[] SelectablesColumns
        {
            get => _columns.Select(c => c.Enabled).ToArray();
            set
            {
                if (value.Length > NumberOfColumns)
                {
                    NumberOfColumns = value.Length;
                }
                var m = Math.Min(value.Length, NumberOfColumns);
                for (var i = 0; i < m; i++)
                {
                    _columns[i].Selectable = value[i];
                }
            }
        }

        private Padding _columnsPadding = new Padding(3);
        public Padding ColumnsPadding
        {
            get => _columnsPadding;
            set
            {
                _columnsPadding = value;
                _updateproperties();
            }
        }

        private bool _interiorborder = false;
        public bool InteriorBorder
        {
            get => _interiorborder;
            set
            {
                _interiorborder = value;
                Invalidate();
            }
        }

        private Color _columnsBackColor = SystemColors.Control;

        public Color ColumnsBackColor
        {
            get => _columnsBackColor;
            set
            {
                _columnsBackColor = value;
                _updateproperties();
            }
        }



        private void _doLayout()
        {
            var ap = Point.Empty;
            foreach (var c in _columns)
            {
                c.Location = ap;
                ap.X += c.Width;
                c.Size = new Size(c.Width, this.Height);
            }
        }
        
        
        #region ColumnPanel Properties

        private DuoToneColor _iconColorSelected;

        public DuoToneColor IconColorSelected
        {
            get => _iconColorSelected;
            set
            {
                _iconColorSelected = value;
                _updateproperties();
            }
        }

        private DuoToneColor _iconColorUnSelected;

        public DuoToneColor IconColorUnSelected
        {
            get => _iconColorUnSelected;
            set
            {
                _iconColorUnSelected = value;
                _updateproperties();
            }
        }

        private string _iconUnselected = "";

        public string IconUnselected
        {
            get => _iconUnselected;
            set
            {
                _iconUnselected = value;
                _updateproperties();
            }
        }

        private string _iconAscending = "";

        public string IconAscending
        {
            get => _iconAscending;
            set
            {
                _iconAscending = value;
                _updateproperties();
            }
        }

        private string _iconDescending = "";

        public string IconDescending
        {
            get => _iconDescending;
            set
            {
                _iconDescending = value;
                _updateproperties();
            }
        }
        #endregion
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _doLayout();
        }
        protected override void Dispose(bool disposing)
        {
            NumberOfColumns = 0;
            base.Dispose(disposing);
        }
    }

    public readonly struct ColumnPanelOrder
    {
        public int ColumnIndex { get; init; }
        public SortOrder SortOrder { get; init; }
        public bool Ascending=> SortOrder != SortOrder.Descending;
    }

    public class ColumnPanelOrderArgs : EventArgs
    {
        public ColumnPanelOrder Order { get; }

        public ColumnPanelOrderArgs(ColumnPanelOrder order)
        {
            Order = order;
        }
    }
}
