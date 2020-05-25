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
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.IO;

namespace IsolateStorageStoreImageDemo_CacheImage
{
    public partial class MainPage : PhoneApplicationPage
    {
        //URL for Image File in Internet
        private string ImageFileName = null;
        private string folder = "TVODImageCache";

        //HTTP Client is used to request
        WebClient webClient;


        // Constructor
        public MainPage()
        {
            InitializeComponent();

            Uri uri = new Uri(txtImageURL.Text.Trim());
            ImageFileName = uri.AbsolutePath.Replace("/", "_");
            ImageFileName = ImageFileName.Replace("%20", "_");
            ImageFileName = ImageFileName.Replace(" ", "_");

            SupportedOrientations = SupportedPageOrientation.Landscape | SupportedPageOrientation.Portrait;

            webClient = new WebClient();

            //Request to server to get Image Material
            webClient.OpenReadCompleted += (s1, e1) =>
                {
                    if (e1.Error == null)
                    {
                        try
                        {
                            bool isSpaceAvailable = IsSpaceIsAvailable(e1.Result.Length);
                            if (isSpaceAvailable)
                            {
                                //Save File To Isolated Storage
                                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication()) {
                                    if (!myIsolatedStorage.DirectoryExists(folder))
                                    {
                                        myIsolatedStorage.CreateDirectory(folder);
                                    }

                                    using (IsolatedStorageFileStream isfs = new IsolatedStorageFileStream( folder + "\\" + ImageFileName, FileMode.Create, FileAccess.Write, myIsolatedStorage))
                                    {
                                   
                                        long imgLen = e1.Result.Length;
                                        byte[] b = new byte[imgLen];
                                        e1.Result.Read(b, 0, b.Length);
                                        isfs.Write(b, 0, b.Length);
                                        isfs.Flush();
                                        isfs.Close();
                                        
                                           
                                    }
                                }

                                LoadImageFromIsolatedStorage(folder + "\\" + ImageFileName);

                            }
                            else
                            {
                                BitmapImage bmpImg = new BitmapImage();
                                bmpImg.SetSource(e1.Result);
                                image1.Source = bmpImg;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }

                };
        }

        private bool IsSpaceIsAvailable(long spaceSeq)
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                long spaceAvail = store.AvailableFreeSpace;
                if (spaceAvail < spaceSeq)
                {
                    return false;
                }
                return true;
            }
        }

        private void LoadImageFromIsolatedStorage(string imageFileName)
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream isoStream = isf.OpenFile(imageFileName, FileMode.Open))
                {
                    BitmapImage bmpImg = new BitmapImage();
                    bmpImg.SetSource(isoStream);
                    image1.Source = bmpImg;
                }
            }
        }

        private void btnGetImage_Click(object sender, RoutedEventArgs e)
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                string fullFileName = folder + "\\" + ImageFileName;
                bool fileExist = isf.FileExists(fullFileName);
                if (fileExist)
                {
                    LoadImageFromIsolatedStorage(fullFileName);
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtImageURL.Text))
                    {
                        Uri uri = new Uri(txtImageURL.Text.Trim());
                        ImageFileName = uri.AbsolutePath.Replace("/","_");
                        ImageFileName = ImageFileName.Replace("%20", "_");
                        ImageFileName = ImageFileName.Replace(" ", "_");
                        webClient.OpenReadAsync(new Uri(txtImageURL.Text));
                    }
                }
            }
        }

    }
}