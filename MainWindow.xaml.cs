using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;

namespace Robot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private BitmapImage _imageSource;
        public BitmapImage ImageSourceProperty
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged("ImageSourceProperty");
            }
        }

        private RotateTransform _imageRotation;
        public RotateTransform ImageRotation
        {
            get { return _imageRotation; }
            set
            {
                _imageRotation = value;
                OnPropertyChanged("ImageRotation");
            }
        }
        
        private string direction;
        private bool placed;

        private int _imageRow;
        public int ImageRow 
        { 
            get { return _imageRow; } 
            set 
            { 
                _imageRow = value; 
                OnPropertyChanged("ImageRow"); 
            } 
        }

        private int _imageColumn;
        public int ImageColumn
        {
            get { return _imageColumn; }
            set
            {
                _imageColumn = value;
                OnPropertyChanged("ImageColumn");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            ImageSourceProperty = new BitmapImage(new Uri(@"/Arrow.png", UriKind.Relative));
            txtCommand.Text = "";
            txtCommand.Focus();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void GetCommand(string command)
        {
            string[] split = command.Split(' ');
            switch (split[0].ToUpper())
            {
                case "PLACE":
                    int x, y;
                    try
                    {
                        x = int.Parse(split[1].Split(',')[0]);
                        y = int.Parse(split[1].Split(',')[1]);
                        string direction = split[1].Split(',')[2];
                        Place(x, y, direction);
                    }
                    catch 
                    {
                        txtStatus.Text = "Input Format Incorrect";
                    }
                    break;
                case "MOVE":
                    Move();
                    break;
                case "LEFT":
                    Left();
                    break;
                case "RIGHT":
                    Right();
                    break;
                case "REPORT":
                    Report();
                    break;
            }
        }

        public void Place(int x, int y, string direction)
        {
            if (placed) return;

            int angle = 90;
            switch (direction.ToUpper())
            {
                case "NORTH":
                    angle = 90;
                    break;
                case "EAST":
                    angle = 180;
                    break;
                case "SOUTH":
                    angle = 270;
                    break;
                case "WEST":
                    angle = 0;
                    break;
            }

            if (x >= 0 && x <= 4 && y >= 0 && y <= 4)
            {
                this.direction = direction;
                this.placed = true;
                robotImage.Visibility = Visibility.Visible;
                this.ImageRow = x;
                this.ImageColumn = y;
                ImageRotation = new RotateTransform(angle);
            }
        }

        public void Move()
        {
            if (!placed) return;

            switch (direction.ToUpper())
            {
                case "NORTH":
                    if (ImageRow <= 4 && ImageRow > 0) ImageRow--;
                    break;
                case "SOUTH":
                    if (ImageRow >= 0 && ImageRow < 4 ) ImageRow++;
                    break;
                case "EAST":
                    if (ImageColumn < 4 && ImageColumn >= 0) ImageColumn++;
                    break;
                case "WEST":
                    if (ImageColumn > 0 && ImageColumn <= 4) ImageColumn--;
                    break;
            }
            txtStatus.Text = "";
        }

        public void Left()
        {
            if (!placed) return;

            switch (direction.ToUpper())
            {
                case "NORTH":
                    direction = "WEST";
                    ImageRotation = new RotateTransform(0);
                    break;
                case "SOUTH":
                    direction = "EAST";
                    ImageRotation = new RotateTransform(180);
                    break;
                case "EAST":
                    direction = "NORTH";
                    ImageRotation = new RotateTransform(90);
                    break;
                case "WEST":
                    direction = "SOUTH";
                    ImageRotation = new RotateTransform(270);
                    break;
            }
            txtStatus.Text = "";
        }

        public void Right()
        {
            if (!placed) return;

            switch (direction.ToUpper())
            {
                case "NORTH":
                    direction = "EAST";
                    ImageRotation = new RotateTransform(180);
                    break;
                case "SOUTH":
                    direction = "WEST";
                    ImageRotation = new RotateTransform(0);
                    break;
                case "EAST":
                    direction = "SOUTH";
                    ImageRotation = new RotateTransform(270);
                    break;
                case "WEST":
                    direction = "NORTH";
                    ImageRotation = new RotateTransform(90);
                    break;
            }
            txtStatus.Text = "";
        }

        public void Report()
        {
            if (!placed) return;

            txtStatus.Text = "Robot is curently at X: " + this.ImageRow.ToString() + " - Y: " + this.ImageColumn.ToString() + " - Facing: " + direction;
        }


        private void txtCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                GetCommand(txtCommand.Text);
                LastCommand.Items.Add(txtCommand.Text);
                txtCommand.Text = "";
            }
        }
    }
}
