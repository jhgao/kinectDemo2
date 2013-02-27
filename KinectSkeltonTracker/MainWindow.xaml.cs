// -----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft Limited">
//  Copyright (c) Microsoft Limited, Microsoft Consulting Services, UK. All rights reserved.
// All rights reserved.
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// <summary>Interaction logic for MainWindow.xaml</summary>
//-----------------------------------------------------------------------
namespace KinectSkeltonTracker
{
    #region using...

    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Kinect;

    #endregion

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        /// 
        byte[] pixelData;

        public MainWindow()
        {
            InitializeComponent();
            MainViewModel model = new MainViewModel();
            this.DataContext = model;
            this.SkeletonControl.ItemsSource = model.Skeletons;
            model.Kinect.ImageFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(this.Kinect_ImageFrameReady);
        }

        /// <summary>
        /// Handles the ImageFrameReady event of the kinect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Microsoft.Kinect.ColorImageFrameReadyEventArgs"/> instance containing the event data.</param>
        private void Kinect_ImageFrameReady(object sender, Microsoft.Kinect.ColorImageFrameReadyEventArgs e)
        {
            //PlanarImage image = e.ImageFrame.Image;

            bool receivedData = false;
            using (ColorImageFrame colorImageFrame = e.OpenColorImageFrame())
            {
                if (colorImageFrame != null)
                {
                    if (pixelData == null)
                    //allocate the first time
                    {
                        pixelData = new byte[colorImageFrame.PixelDataLength];
                    }
                    colorImageFrame.CopyPixelDataTo(pixelData);
                    receivedData = true;
                }
                else
                {
                    // apps processing of image data is taking too long, it got more than 2 frames behind.
                    // the data is no longer avabilable.
                }
            if (receivedData)
            {
                cameraFeed.Source = BitmapSource.Create(colorImageFrame.Width, colorImageFrame.Height, 96, 96, PixelFormats.Bgr32, null, pixelData, colorImageFrame.Width * colorImageFrame.BytesPerPixel);
            }
            }
        }
    }
}
