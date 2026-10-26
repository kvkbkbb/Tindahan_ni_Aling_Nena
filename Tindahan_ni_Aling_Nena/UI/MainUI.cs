using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Tindahan_ni_Aling_Nena
{
    public partial class Form1 : Form
    {
        private decimal _selectedChangeTotal;
        private Label _totalChangeLabel;

        public Form1()
        {
            InitializeComponent();
            BuildScreen();
        }

        private void BuildScreen()
        {
            Text = "Tindahan ni Aling Nena";
            ClientSize = new Size(950, 700);
            MinimumSize = new Size(760, 560);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(105, 105, 105);
            Font = new Font("Segoe UI", 10F);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.FromArgb(105, 105, 105),
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 63F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 37F));
            layout.Controls.Add(BuildTopSection(), 0, 0);
            layout.Controls.Add(BuildCashbox(), 0, 1);
            Controls.Add(layout);
        }

        private Control BuildTopSection()
        {
            var top = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.FromArgb(105, 105, 105),
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
            top.Controls.Add(new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(105, 105, 105),
                Margin = Padding.Empty
            }, 0, 0);
            top.Controls.Add(BuildReceipt(), 1, 0);
            return top;
        }

        private Control BuildReceipt()
        {
            var receiptImage = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                BackgroundImage = LoadImage(Path.Combine("Assets", "Receipt.png")),
                BackgroundImageLayout = ImageLayout.Zoom,
                Margin = new Padding(8, 0, 8, 0)
            };
            var receiptContent = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                BackColor = Color.Transparent,
                Padding = new Padding(36, 42, 32, 16),
                Margin = Padding.Empty
            };
            receiptContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            receiptContent.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            receiptContent.RowStyles.Add(new RowStyle(SizeType.Percent, 32F));
            receiptContent.RowStyles.Add(new RowStyle(SizeType.Percent, 32F));
            receiptContent.RowStyles.Add(new RowStyle(SizeType.Percent, 36F));
            receiptContent.Controls.Add(MakeLabel("Order Receipt", 13F, true), 0, 0);
            receiptContent.Controls.Add(MakeLabel("1x Piattos\r\n5x Coca Cola", 11F, false), 0, 1);
            receiptContent.Controls.Add(MakeLabel("Customer Paid:\r\n3× 100 Peso Bills\r\n10× 20 Peso Bills", 11F, false), 0, 2);
            var changeSummary = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = Padding.Empty
            };
            var summaryLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            summaryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 56F));
            summaryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44F));
            summaryLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _totalChangeLabel = MakeLabel("Total change: ₱0.00", 11F, true);
            _totalChangeLabel.TextAlign = ContentAlignment.MiddleLeft;
            summaryLayout.Controls.Add(_totalChangeLabel, 0, 0);
            var actions = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.Transparent,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            var giveChangeButton = MakeAction("GIVE CHANGE", Color.FromArgb(104, 174, 87), 116);
            giveChangeButton.Height = 26;
            giveChangeButton.Margin = new Padding(0, 0, 0, 3);
            var underpaidButton = MakeAction("KULANG PO", Color.FromArgb(203, 133, 83), 116);
            underpaidButton.Height = 26;
            underpaidButton.Margin = new Padding(0, 0, 0, 3);
            var resetButton = MakeAction("RESET", Color.FromArgb(100, 100, 100), 116);
            resetButton.Height = 26;
            resetButton.Margin = Padding.Empty;
            resetButton.Click += (sender, e) => ResetSelectedChange();
            actions.Controls.Add(giveChangeButton);
            actions.Controls.Add(underpaidButton);
            actions.Controls.Add(resetButton);
            summaryLayout.Controls.Add(actions, 1, 0);
            changeSummary.Controls.Add(summaryLayout);
            receiptContent.Controls.Add(changeSummary, 0, 3);
            receiptImage.Controls.Add(receiptContent);

            return receiptImage;
        }

        private Control BuildCashbox()
        {
            var cashbox = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.FromArgb(48, 50, 49),
                Padding = new Padding(6, 8, 6, 8),
                Margin = Padding.Empty
            };
            cashbox.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72F));
            cashbox.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
            cashbox.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var tiles = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 6,
                RowCount = 1,
                BackColor = Color.FromArgb(48, 50, 49),
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            for (int i = 0; i < 6; i++)
                tiles.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / 6));

            tiles.Controls.Add(MakeCashTile("PHP_20.jpg", 20m), 0, 0);
            tiles.Controls.Add(MakeCashTile("PHP_50.jpg", 50m), 1, 0);
            tiles.Controls.Add(MakeCashTile("PHP_100.jpg", 100m), 2, 0);
            tiles.Controls.Add(MakeCashTile("PHP_200.jpg", 200m), 3, 0);
            tiles.Controls.Add(MakeCashTile("PHP_500.jpg", 500m), 4, 0);
            tiles.Controls.Add(MakeCashTile("PHP_1000.jpg", 1000m), 5, 0);
            cashbox.Controls.Add(tiles, 0, 0);

            var coins = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                BackColor = Color.FromArgb(48, 50, 49),
                Padding = new Padding(10, 0, 4, 0),
                Margin = Padding.Empty
            };
            coins.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            coins.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            coins.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            coins.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            coins.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            var coinsHeading = MakeLabel("COINS", 10F, true);
            coinsHeading.ForeColor = Color.White;
            coins.Controls.Add(coinsHeading, 0, 0);
            coins.SetColumnSpan(coinsHeading, 2);
            coins.Controls.Add(MakeCoinTile("PHP_20_Coin.png", 20m), 0, 1);
            coins.Controls.Add(MakeCoinTile("PHP_10.png", 10m), 1, 1);
            coins.Controls.Add(MakeCoinTile("PHP_5.png", 5m), 0, 2);
            coins.Controls.Add(MakeCoinTile("PHP_1.png", 1m), 1, 2);
            cashbox.Controls.Add(coins, 1, 0);
            return cashbox;
        }

        private static Image LoadImage(string relativePath)
        {
            var imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            if (!File.Exists(imagePath))
                throw new FileNotFoundException("UI image was not found.", imagePath);

            using (var source = Image.FromFile(imagePath))
                return new Bitmap(source);
        }

        private Control MakeCashTile(string imageName, decimal value)
        {
            var imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Bills", imageName);
            if (!File.Exists(imagePath))
                throw new FileNotFoundException("Cashbox bill image was not found.", imagePath);

            Image billImage;
            using (var source = Image.FromFile(imagePath))
                billImage = new Bitmap(source);
            billImage.RotateFlip(RotateFlipType.Rotate90FlipNone);

            var tile = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(48, 50, 49),
                Image = billImage,
                SizeMode = PictureBoxSizeMode.Zoom,
                Margin = new Padding(0, 0, 8, 0)
            };
            tile.Click += (sender, e) => AddSelectedChange(value);
            tile.Cursor = Cursors.Hand;
            return tile;
        }

        private Control MakeCoinTile(string imageName, decimal value)
        {
            var imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Coins", imageName);
            if (!File.Exists(imagePath))
                throw new FileNotFoundException("Cashbox coin image was not found.", imagePath);

            return MakeImageTile(
                LoadImage(Path.Combine("Assets", "Coins", imageName)),
                value,
                new Padding(3));
        }

        private Control MakeImageTile(Image image, decimal value, Padding margin)
        {
            var tile = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(48, 50, 49),
                Margin = margin
            };
            var imageView = new PictureBox
            {
                Size = new Size(96, 96),
                BackColor = Color.Transparent,
                Image = image,
                SizeMode = PictureBoxSizeMode.Zoom,
                Cursor = Cursors.Hand
            };
            imageView.Click += (sender, e) => AddSelectedChange(value);
            tile.Controls.Add(imageView);
            tile.Resize += (sender, e) =>
            {
                imageView.Location = new Point(
                    (tile.ClientSize.Width - imageView.Width) / 2,
                    (tile.ClientSize.Height - imageView.Height) / 2);
            };
            return tile;
        }

        private void AddSelectedChange(decimal value)
        {
            _selectedChangeTotal += value;
            _totalChangeLabel.Text = $"Total change: ₱{_selectedChangeTotal:N2}";
        }

        private void ResetSelectedChange()
        {
            _selectedChangeTotal = 0m;
            _totalChangeLabel.Text = "Total change: ₱0.00";
        }

        private static Label MakeLabel(string text, float size, bool bold)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                AutoSize = false,
                Font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = Color.FromArgb(35, 35, 35),
                TextAlign = ContentAlignment.TopLeft,
                Margin = Padding.Empty
            };
        }

        private static Button MakeAction(string text, Color background, int width)
        {
            return new Button
            {
                Text = text,
                Width = width,
                Height = 40,
                Margin = new Padding(0, 0, 16, 0),
                BackColor = background,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                UseVisualStyleBackColor = false
            };
        }
    }
}
