using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;


namespace iTVOD_WindowPhone7.TVOD.TVODClass
{
    public class ImageCache
    {
        //URL for Image File in Internet
        private string ImageFileName = null;
        private string folder = "TVODImageCache";

        //HTTP Client is used to request
        WebClient webClient;


        public string getImage_2(string category_id)
        {
            if (category_id == "13")
            {
                return "Images/tron_nhac.png";
            }
            else if (category_id == "14")
            {
                return "Images/tron_phim.png";
            }
            else if (category_id == "15")
            {
                return "Images/tron_thethao.png";
            }
            else if (category_id == "16")
            {
                return "Images/tron_truyenhinh.png";
            }
            else if (category_id == "552")
            {
                return "Images/tron_radio.png";
            }
            else if (category_id == "685")
            {
                return "Images/tintuc.png";
            }
            else if (category_id == "644")
            {
                return "Images/Phim/Phim1-6.png";
            }
            else if (category_id == "424")
            {
                return "Images/Phim/phim 2012.png";
            }
            else if (category_id == "423")
            {
                return "Images/Phim/phim 2011.png";
            }
            else if (category_id == "422")
            {
                return "Images/Phim/phim 2010.png";
            }
            else if (category_id == "1")
            {
                return "Images/Phim/phim hanh dong.png";
            }
            else if (category_id == "8")
            {
                return "Images/Phim/Phim hai huoc.png";
            }
            else if (category_id == "10")
            {
                return "Images/Phim/Phim vien tuong.png";
            }
            else if (category_id == "9")
            {
                return "Images/Phim/phim tam ly.png";
            }
            else if (category_id == "5")
            {
                return "Images/Phim/phim kinh di.png";
            }
            else if (category_id == "3")
            {
                return "Images/Phim/phim hoat hinh.png";
            }
            else if (category_id == "12")
            {
                return "Images/Phim/phim tai lieu_2.png";
            }
            else if (category_id == "6")
            {
                return "Images/Phim/Phim kinh dien.png";
            }
            else if (category_id == "4")
            {
                return "Images/Phim/phim bo.png";
            }
            else if (category_id == "710")
            {
                return "Images/live/135958660.png";
            }
            else if (category_id == "739")
            {
                return "Images/live/hdicon.png";
            }
            else
            {
                return "Images/tron_phim.png";
            }
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

        public void SaveData(string category_image)
        {
            webClient = new WebClient();
            webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadCompleted);
            webClient.OpenReadAsync(new Uri(category_image));
        }

        void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                try
                {
                    bool isSpaceAvailable = IsSpaceIsAvailable(e.Result.Length);
                    if (isSpaceAvailable)
                    {
                        //Save File To Isolated Storage
                        using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                        {
                            bool isDirectoryExists = myIsolatedStorage.DirectoryExists(folder);
                            if (!isDirectoryExists)
                            {
                                myIsolatedStorage.CreateDirectory(folder);
                            }

                            using (IsolatedStorageFileStream isfs = new IsolatedStorageFileStream(folder + "\\" + ImageFileName, FileMode.Create, FileAccess.Write, myIsolatedStorage))
                            {
                                long imgLen = e.Result.Length;
                                byte[] b = new byte[imgLen];
                                e.Result.Read(b, 0, b.Length);
                                isfs.Write(b, 0, b.Length);
                                isfs.Flush();
                                isfs.Close();
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        public string getImage(string category_image)
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                Uri uri = new Uri(category_image.Trim());
                ImageFileName = uri.AbsolutePath.Replace("/", "_");
                ImageFileName = ImageFileName.Replace("%20", "_");
                ImageFileName = ImageFileName.Replace(" ", "_");
                ImageFileName = ImageFileName.Substring(0, 50);

                string fullFileName = folder + "\\" + ImageFileName;
                bool fileExist = isf.FileExists(fullFileName);
                if (fileExist)
                {
                    //return fullFileName;
                    return category_image;
                }
                else
                {
                    if (!string.IsNullOrEmpty(category_image))
                    {
                        //SaveData(category_image);
                        //return category_image;
                        return category_image;
                    }
                    else
                    {
                        return "http";
                    }
                }
            }
        }

    }
}
