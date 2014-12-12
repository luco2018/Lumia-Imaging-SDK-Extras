﻿using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Lumia.Imaging.Transforms;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Streams;

namespace Lumia.Imaging.Extras.Extensions
{
    public static class ImageProviderExtensions
    {
        /// <summary>
        /// Renders the IImageProvider into a WriteableBitmap. If the passed WriteableBitmap is non-null, and it matches the passed size, it will be reused. Otherwise this method creates a new WriteableBitmap. Note that this method must be called on the UI thread.
        /// </summary>
        /// <param name="imageProvider">The image provider to render the image content of.</param>
        /// <param name="writeableBitmap">The WriteableBitmap to reuse, if possible. If null is passed, a new WriteableBitmap is created.</param>
        /// <param name="size">The desired size of the rendered image.</param>
        /// <param name="outputOption">Specifies how to fit the image into the rendered rectangle, if the aspect ratio differs.</param>
        /// <returns>A task resulting in either the reused WriteableBitmap, or a new one if necessary.</returns>
        public static Task<WriteableBitmap> GetBitmapAsync(this IImageProvider imageProvider, WriteableBitmap writeableBitmap, Size size, OutputOption outputOption)
        {
            if (writeableBitmap == null || writeableBitmap.PixelWidth != (int)size.Width || writeableBitmap.PixelHeight != (int)size.Height)
            {
                writeableBitmap = new WriteableBitmap((int)size.Width, (int)size.Height);
            }

            return imageProvider.GetBitmapAsync(writeableBitmap, outputOption).AsTask();
        }

        /// <summary>
        /// Renders the IImageProvider into a Bitmap. If the passed Bitmap is non-null, and it matches the passed size and color mode, it will be reused. Otherwise this method creates a new Bitmap.
        /// </summary>
        /// <param name="imageProvider">The image provider to render the image content of.</param>
        /// <param name="bitmap">The Bitmap to reuse, if possible. If null is passed, a new Bitmap is created.</param>
        /// <param name="size">The desired size of the rendered image.</param>
        /// <param name="outputOption">Specifies how to fit the image into the rendered rectangle, if the aspect ratio differs.</param>
        /// <returns>A task resulting in either the reused Bitmap, or a new one if necessary.</returns>
        public static Task<Bitmap> GetBitmapAsync(this IImageProvider imageProvider, Bitmap bitmap, Size size, ColorMode colorMode, OutputOption outputOption)
        {
            if (bitmap == null || bitmap.Dimensions != size || bitmap.ColorMode != colorMode)
            {
                bitmap = new Bitmap(size, colorMode);
            }

            return imageProvider.GetBitmapAsync(bitmap, outputOption).AsTask();
        }

        /// <summary>
        /// Rotate the image provider.
        /// </summary>
        /// <param name="imageProvider">The source image provider.</param>
        /// <param name="rotationAngle">The angle of rotation, in degrees clockwise.</param>
        /// <returns>A rotated image provider.</returns>
        /// <remarks>
        /// Note that no intelligence is built in, this method simply applies a RotationFilter. 
        /// For instance, calling this method several times with the angles summing to an even 360 degrees will not be very efficient.
        /// </remarks>
        public static IImageProvider Rotate(this IImageProvider imageProvider, double rotationAngle)
        {
            return new FilterEffect(imageProvider) { Filters = new[] { new RotationFilter(rotationAngle) } };
        }
    }
}
