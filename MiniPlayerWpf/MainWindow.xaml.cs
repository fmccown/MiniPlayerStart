using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MiniPlayerWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MusicRepo? musicRepo;
        private readonly MediaPlayer mediaPlayer;
        private readonly ObservableCollection<int>? songIds;

        public MainWindow()
        {
            InitializeComponent();

            mediaPlayer = new MediaPlayer();

            try
            {
                musicRepo = new MusicRepo();

                // Put the ids into an ObservableCollection, which has methods to add and remove items.
                // The UI will update itself automatically if any changes are made to this collection.
                songIds = new ObservableCollection<int>(musicRepo.SongIds);

                // Bind the song IDs to the combo box
                songIdComboBox.ItemsSource = songIds;

                // Select the first item
                if (songIdComboBox.Items.Count > 0)
                {
                    songIdComboBox.SelectedIndex = 0;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error loading file: " + e.Message, "MiniPlayer",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        private void songIdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Display the selected song
            if (songIdComboBox.SelectedItem != null)
            {
                int songId = Convert.ToInt32(songIdComboBox.SelectedItem);
                Song? song = musicRepo?.GetSong(songId);
                if (song != null)
                {
                    songTitle.Content = song.Title;
                    if (song.Filename is not null)
                    {
                        mediaPlayer.Open(new Uri(song.Filename));
                    }
                }
            }
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
        }
    }
}