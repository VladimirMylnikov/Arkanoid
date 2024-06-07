using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace ARKANOID
{
    /// <summary>
    /// Логика взаимодействия для GAME.xaml
    /// </summary>
    public partial class GAME : Page
    {
        private int dx = -1;
        private int dy = -1;

        private int score = 0;

        private bool pause = false;

        private bool IsStarted = false;
        private bool IsOver = false;

        private System.Windows.Threading.DispatcherTimer dispatcherTimer;
        private List<Rectangle> rectangles = new List<Rectangle>();

        public GAME()
        {
            InitializeComponent();
            for (int j = 2; j < 7; j = j + 2)
            {
                for (int i = 3; i < 24; i = i + 3)
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.Stroke = new SolidColorBrush(System.Windows.Media.Colors.Magenta);
                    rectangle.StrokeThickness = 2.5;
                    rectangle.RadiusX = 5;
                    rectangle.RadiusY = 5;
                    Grid.SetColumn(rectangle, i);
                    Grid.SetColumnSpan(rectangle, 1);
                    Grid.SetRow(rectangle, j);
                    Grid.SetRowSpan(rectangle, 1);
                    MyGrid.Children.Add(rectangle);
                    rectangles.Add(rectangle);
                }
            }
        }


        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Visibility == Visibility.Visible)
            {
                this.Focusable = true;
                this.Focus();
            }
        }

        private void MENU_Button_Click(object sender, RoutedEventArgs e)
        {
            IsOver = true;
            NavigationService.Navigate(new PAGE_MENU());
        }

        private void RESTART_Button_Click(object sender, RoutedEventArgs e)
        {
            IsOver = true;
            NavigationService.Navigate(new GAME());
        }

        private void PAUSE_Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsStarted)
            {
                if (pause == false)
                {
                    pause = true;
                    InfoForPlayer.Text = "PAUSE";
                    dispatcherTimer.Stop();
                }
                else if (pause == true)
                {
                    pause = false;
                    InfoForPlayer.Text = "";
                    dispatcherTimer.Start();
                }
            }
        }


        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D)
            {
                int count = Grid.GetColumn(Raketka);
                if (count < 21)
                {
                    Grid.SetColumn(Raketka, count + 1);
                    if (!IsStarted)
                    {
                        int ballColumn = Grid.GetColumn(Ball);
                        Grid.SetColumn(Ball, ballColumn + 1);
                    }

                }
            }

            else if (e.Key == Key.A)
            {
                int count = Grid.GetColumn(Raketka);
                if (count > 0)
                {
                    Grid.SetColumn(Raketka, count - 1);
                    if (!IsStarted)
                    {
                        int ballColumn = Grid.GetColumn(Ball);
                        Grid.SetColumn(Ball, ballColumn - 1);
                    }
                }
            }

            else if (!IsStarted && e.Key == Key.Space)
            {
                IsStarted = true;
                dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0,  180);
                dispatcherTimer.Start();
            }

            else if (IsStarted && e.Key == Key.Space)
            {
                if (pause == false)
                {
                    pause = true;
                    InfoForPlayer.Text = "PAUSE";
                    dispatcherTimer.Stop();
                }
                else if (pause == true)
                {
                    pause = false;
                    InfoForPlayer.Text = "";
                    dispatcherTimer.Start();
                }
            }

            else if (IsStarted && e.Key == Key.R)
            {
                IsOver = true;
                NavigationService.Navigate(new GAME());
            }

            else if (e.Key == Key.Escape)
            {
                IsOver = true;
                NavigationService.Navigate(new PAGE_MENU());
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (IsOver)
            {
                dispatcherTimer.Stop();
                return;
            }

            CollisionsCheck();
            int ballColumn = Grid.GetColumn(Ball);
            Grid.SetColumn(Ball, ballColumn + dx);
            int ballRow = Grid.GetRow(Ball);
            Grid.SetRow(Ball, ballRow + dy);
            dispatcherTimer.Start();
        }


        private void CollisionsCheck()
        {
            int RaketkaColumn = Grid.GetColumn(Raketka);
            int ballColumn = Grid.GetColumn(Ball);
            int ballRow = Grid.GetRow(Ball);

            if (ballRow == 13 && dy > 0)
            {
                if (RaketkaColumn >= (ballColumn - 2) && RaketkaColumn <= ballColumn)
                {
                    dy = -dy;
                }
                else if ((RaketkaColumn == (ballColumn - 1) && dx < 0) || (RaketkaColumn == (ballColumn + 1) && dx > 0))
                {
                    dy = -dy;
                    dx = -dx;
                }
                else if ((ballColumn == 0 && dy > 0 && dx < 0)|| (ballColumn == 23 && dy > 0 && dx > 0))
                {
                    dx = -dx;
                }
            }

            if (ballRow == 15)
            {
                InfoForPlayer.Text = "GAME OVER! SCORE:" + score;
                IsOver = true;
                return;
            }

            if ((ballColumn == 0 && dx < 0) || (ballColumn == 23 && dx > 0))
            {
                dx = -dx;
            }

            if (ballRow == 0 && dy < 0)
            {
                dy = -dy;
            }


            UIElement UgolBlock = MyGrid.Children.OfType<Rectangle>().Where(i => (Grid.GetColumn(i) == ballColumn + dx) && (Grid.GetRow(i) == ballRow + dy)).FirstOrDefault();
            if (UgolBlock != null)
            {
                score += 50;
                ScoreTablo.Text = score.ToString();
                dx = -dx;
                dy = -dy;
                MyGrid.Children.Remove(UgolBlock);
            }

            UIElement LeftOrRightBlock = MyGrid.Children.OfType<Rectangle>().Where(i => (Grid.GetColumn(i) == ballColumn + dx) && (Grid.GetRow(i) == ballRow)).FirstOrDefault();
            if (LeftOrRightBlock != null)
            {
                score += 50;
                ScoreTablo.Text = score.ToString();
                dx = -dx;
                MyGrid.Children.Remove(LeftOrRightBlock);
            }

            UIElement UpOrDownBlock = MyGrid.Children.OfType<Rectangle>().Where(i => (Grid.GetRow(i) == ballRow + dy) && (Grid.GetColumn(i) == ballColumn)).FirstOrDefault();
            if (UpOrDownBlock != null)
            {
                score += 50;
                ScoreTablo.Text = score.ToString();
                dy = -dy;
                MyGrid.Children.Remove(UpOrDownBlock);
            }

            if(score==1050)
            {
                InfoForPlayer.Text = "YOU WIN! SCORE:" + score;
                IsOver = true;
                return;
            }

        }
    }
}

   

