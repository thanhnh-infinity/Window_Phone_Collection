using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Example_5_MediaPlayerDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        private bool _updatingMediaTimeLine;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            _updatingMediaTimeLine = false;

            mediaPlayer.Position = System.TimeSpan.FromSeconds(0);

            mediaPlayer.DownloadProgressChanged += (s, e) =>
                {
                    lblDowloading.Text = string.Format("Dowloading {0:0.0%}", mediaPlayer.DownloadProgress);
                };

            mediaPlayer.BufferingTime = TimeSpan.FromSeconds(Convert.ToDouble(txtBufferingTime.Text));

            mediaPlayer.BufferingProgressChanged += (s, e) =>
                {
                    lblBuffering.Text = string.Format("Buffering {0:0.0%}", mediaPlayer.BufferingProgress);
                };

            CompositionTarget.Rendering += (s, e) =>
                {
                    _updatingMediaTimeLine = true;
                    TimeSpan duration = mediaPlayer.NaturalDuration.TimeSpan;
                    if (duration.TotalSeconds != 0)
                    {
                        double percentComplete = mediaPlayer.Position.TotalSeconds / duration.TotalSeconds;
                        mediaTimeLine.Value = percentComplete;
                        TimeSpan mediaTime = mediaPlayer.Position;
                        string text = string.Format("{0:0.0}:{1:0.0}", (mediaTime.Hours * 60) + mediaTime.Minutes, mediaTime.Seconds);

                        if (!lblStatus.Text.Equals(text))
                        {
                            lblStatus.Text = text;
                            _updatingMediaTimeLine = false;
                        }
                    }
                };

        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if (mediaPlayer.CanPause)
            {
                mediaPlayer.Pause();
                lblStatus.Text = "Paused";
            }
            else
            {
                lblStatus.Text = "Cannot pause! Please, try again";
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            mediaPlayer.Position = System.TimeSpan.FromSeconds(0);
            lblStatus.Text = "Stopped";
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            //Uri uri = new Uri(txtURL.Text.Trim());
            //Uri uri = new Uri("http://203.162.16.22:8081/movies/Video/disk1/video-raw-cp-10095/62f2546474adf9645bbfe1754db87398.mp4?sessionID=wWkenMtA&user_id=146&payment_value=0&payment_item_id=17710&des_user_id=null&ipaddress=113.190.240.238");
            //mediaPlayer.Source = uri;
            mediaPlayer.Play();
        }

        private void btnMute_Click(object sender, RoutedEventArgs e)
        {
            if (lblSoundStatus.Text.Equals("Sound On", StringComparison.CurrentCultureIgnoreCase))
            {
                lblSoundStatus.Text = "Sound Off";
                mediaPlayer.IsMuted = true;
            }
            else
            {
                lblSoundStatus.Text = "Sound On";
                mediaPlayer.IsMuted = false;
            }
        }

        private void mediaTimeLine_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!_updatingMediaTimeLine && mediaPlayer.CanSeek)
            {
                TimeSpan duration = mediaPlayer.NaturalDuration.TimeSpan;
                int newPosition = (int)(duration.TotalSeconds * mediaTimeLine.Value);
                mediaPlayer.Position = new TimeSpan(0,0,newPosition);

            }
        }

        private void btnMediaPlayerLauncher_Click(object sender, RoutedEventArgs e)
        {
            MediaPlayerLauncher player = new MediaPlayerLauncher();        
            player.Media = new Uri("http://203.162.16.22:8081/movies/Video/disk1/video-raw-cp-10095/62f2546474adf9645bbfe1754db87398.mp4?sessionID=wWkenMtA&user_id=146&payment_value=0&payment_item_id=17710&des_user_id=null&ipaddress=113.190.240.238");
            player.Show();
            
        }
    }
}