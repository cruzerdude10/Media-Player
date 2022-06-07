using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;

namespace WpfTutorialSamples.Audio_and_Video
{
	public partial class MediaPlayerWPF : Window {
		private bool mediaPlayerIsPlaying = false;
		private bool userIsDraggingSlider = false;

		public MediaPlayerWPF() {
			InitializeComponent();

			//START TIMER ON MEDIA
			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += timer_Tick;
			timer.Start();
		}

		//PROGRESS BAR/TIMER
		private void timer_Tick(object sender, EventArgs e) {
			if((mediaBox.Source != null) && (mediaBox.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
			{
				sliProgress.Minimum = 0;
				sliProgress.Maximum = mediaBox.NaturalDuration.TimeSpan.TotalSeconds;
				sliProgress.Value = mediaBox.Position.TotalSeconds;
			}
		}
		private void sliProgress_DragStarted(object sender, DragStartedEventArgs e) {
			userIsDraggingSlider = true;
		}
		private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e) {
			userIsDraggingSlider = false;
			mediaBox.Position = TimeSpan.FromSeconds(sliProgress.Value);
		}
		private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
			lblProgressBar.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
		}

		//OPEN MEDIA
		private void Open_Executed(object sender, ExecutedRoutedEventArgs e) {
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*";
			if(openFileDialog.ShowDialog() == true)
				mediaBox.Source = new Uri(openFileDialog.FileName);
		}
		private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}
		
		//PLAY 
		private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = (mediaBox != null) && (mediaBox.Source != null);
		}
		private void Play_Executed(object sender, ExecutedRoutedEventArgs e) {
			mediaBox.Play();
			mediaPlayerIsPlaying = true;
		}

		//PAUSE
		private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = mediaPlayerIsPlaying;
		}
		private void Pause_Executed(object sender, ExecutedRoutedEventArgs e) {
			mediaBox.Pause();
		}

		//STOP
		private void Stop_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = mediaPlayerIsPlaying;
		}
		private void Stop_Executed(object sender, ExecutedRoutedEventArgs e) {
			mediaBox.Stop();
			mediaPlayerIsPlaying = false;
		}

		//VOLUME
		private void Grid_MouseWheel(object sender, MouseWheelEventArgs e) {
			mediaBox.Volume += (e.Delta > 0) ? 0.1 : -0.1;
		}

	}//END CLASS
}//END NAMESPACE