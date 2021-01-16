using System;
using System.Collections.Generic;
using System.Linq;
using ga_forms.Common;
using SkiaSharp;

namespace ga_forms.Models.ImageProcessing.Algorithms
{
    class KMeansClustering : IAlgorithm
    {
        public SKBitmap ProcessingImage
        {
            get => throw new Exception(Constants.NO_PROCESSING_IMAGE);
            set => throw new Exception(Constants.NO_PROCESSING_IMAGE);
        }
        public SKBitmap ProcessedImage
        {
            get => throw new Exception(Constants.NO_PROCESSED_IMAGE);
            set => throw new Exception(Constants.NO_PROCESSED_IMAGE);
        }

        // IN: All colors
        public List<SKColor> ColorsIn { get; set; }

        // OUT: Clusters colors
        public List<SKColor> ColorsOut { get; set; }

        // IN: Parameters
        private const int K = 3;
        private const double Threshold = 5.0d;

        public void Execute()
        {
            List<KCluster> clusters = new List<KCluster>();

            // 1. Initialization
            Random random = new Random();
            List<int> usedIndexes = new List<int>();
            while (clusters.Count < K)
            {
                int index = random.Next(0, ColorsIn.Count);
                if (usedIndexes.Contains(index) == true)
                {
                    continue;
                }

                usedIndexes.Add(index);
                KCluster cluster = new KCluster(ColorsIn[index]);
                clusters.Add(cluster);
            }

            bool updated;
            do
            {
                updated = false;

                // 2. Find closest cluster
                foreach (SKColor color in ColorsIn)
                {
                    double shortestDistance = double.MaxValue;
                    KCluster closestCluster = null;

                    foreach (KCluster cluster in clusters)
                    {
                        double distance = cluster.DistanceFromCenter(color);
                        if (distance < shortestDistance)
                        {
                            shortestDistance = distance;
                            closestCluster = cluster;
                        }
                    }

                    closestCluster.Colors.Add(color);
                }

                // 3. Recalculate clusters center
                foreach (KCluster cluster in clusters)
                {
                    if (cluster.RecalculateCenter(Threshold) == true)
                    {
                        updated = true;
                    }
                }

                // 4. If updated, reiterate
            } while (updated);

            // Set clusters colors
            ColorsOut = clusters.OrderByDescending(c => c.PriorCount).Select(c => c.Center).ToList();
        }
    }
}
