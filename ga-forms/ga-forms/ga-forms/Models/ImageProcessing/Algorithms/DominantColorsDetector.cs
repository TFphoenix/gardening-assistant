using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ga_forms.Common;
using SkiaSharp;

namespace ga_forms.Models.ImageProcessing.Algorithms
{
    class DominantColorsDetector : IAlgorithm
    {
        public enum FilteringMethod
        {
            None,
            BlackWhite,
            Black,
            White
        }

        public SKBitmap ProcessingImage { get; set; }
        public SKBitmap ProcessedImage
        {
            get => throw new Exception(Constants.NO_PROCESSED_IMAGE);
            set => throw new Exception(Constants.NO_PROCESSED_IMAGE);
        }

        // IN: Filtering
        public FilteringMethod Filtering { get; set; }

        // IN: Patameters
        private const int ResizeWidth = 100;
        private const int ResizeHeight = 100;

        // OUT: Dominant colors
        public List<SKColor> DominantColors { get; private set; }

        public DominantColorsDetector()
        {
            Filtering = FilteringMethod.BlackWhite;
        }

        public void Execute()
        {
            // Resize initial image
            ResizeImage();

            // K-menas clustering
            KMeansClustering clusteringAlgorithm = new KMeansClustering();
            clusteringAlgorithm.ColorsIn = FilterColors(GetColorsFromImage());
            clusteringAlgorithm.Execute();

            DominantColors = clusteringAlgorithm.ColorsOut;
        }

        private void ResizeImage()
        {
            ProcessingImage = ProcessingImage.Resize(new SKSizeI(ResizeWidth, ResizeHeight), SKFilterQuality.Medium);
        }

        private List<SKColor> GetColorsFromImage()
        {
            return ProcessingImage.Pixels
                .Where(c => c.Alpha != 0)
                .ToList();
        }

        // Filter colors (Removes shades of gray)
        private List<SKColor> FilterColors(List<SKColor> colors)
        {
            switch (Filtering)
            {
                case FilteringMethod.None:
                    return colors;
                case FilteringMethod.BlackWhite:
                    return colors
                        .Where(c => (BitmapExtensions.EuclideanDistance(c, SKColors.Black) >= 200) &&
                                    (BitmapExtensions.EuclideanDistance(c, SKColors.White) >= 200))
                        .ToList();
                case FilteringMethod.Black:
                    return colors
                        .Where(c => (BitmapExtensions.EuclideanDistance(c, SKColors.Black) >= 200))
                        .ToList();
                case FilteringMethod.White:
                    return colors
                        .Where(c => (BitmapExtensions.EuclideanDistance(c, SKColors.White) >= 200))
                        .ToList();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
