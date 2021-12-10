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

namespace MatchGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    using System.Windows.Threading;

    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;

        //Dictionary to hold the placements of emojis
        IDictionary<string, string> textBlockLocations = new Dictionary<string, string>();

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
            }
        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>()
            {
                "🦊","🦊",
                "🐵","🐵",
                "🐮","🐮",
                "🐷","🐷",
                "🐴","🐴",
                "🐹","🐹",
                "🐼","🐼",
                "🦁","🦁",
            };

            Random random = new Random();
            int count = 0;
            //Clears Dictionary for New Game
            textBlockLocations.Clear();

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlockLocations.Add("textBlock_" + count.ToString(), nextEmoji);
                    textBlock.Text = "?";
                    animalEmoji.RemoveAt(index);
                    count++;
                }
            }
            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        string nameLastTextBlock;
        TextBlock lastTextBlockClicked;
        bool findingMatch = false;

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;

            if (findingMatch == false)
            {
                textBlock.Text = textBlockLocations[textBlock.Name];
                nameLastTextBlock = textBlock.Name;
                lastTextBlockClicked = textBlock;
                findingMatch = true;    
            }
            else if ((textBlockLocations[textBlock.Name] == textBlockLocations[nameLastTextBlock]) && (lastTextBlockClicked.Name != textBlock.Name))
            {
                matchesFound++;
                textBlock.Text = textBlockLocations[textBlock.Name];
                findingMatch = false;
            }
            else
            {
                lastTextBlockClicked.Text = "?";
                textBlock.Text = "?";
                findingMatch = false;
            }
        }

        private void timeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}
