using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
                Song? s = musicRepo?.GetSong(songId);
                if (s != null)
                {
                    songTitle.Content = s.Title;
                    if (s.Filename is not null)
                    {
                        mediaPlayer.Open(new Uri(s.Filename));
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
