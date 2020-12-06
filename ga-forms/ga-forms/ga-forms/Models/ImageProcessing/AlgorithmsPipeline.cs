using System;
using ga_forms.Models.ImageProcessing.Algorithms;
using SkiaSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ga_forms.Models.ImageProcessing
{
    class AlgorithmsPipeline
    {
        public SKBitmap InitialImage { get; set; }
        public static SKBitmap ResultImage { get; set; }
        public List<IAlgorithm> Algorithms { get; set; }
        public float Progress { get; set; }

        //ctor
        public AlgorithmsPipeline(List<IAlgorithm> algorithms)
        {
            Algorithms = algorithms;
        }

        public void ExecutePipeline()
        {
            if (InitialImage == null) throw new NullReferenceException("InitialImage cannot be null");

            Progress = 0.0f;
            SKBitmap currentImage = new SKBitmap();
            InitialImage.CopyTo(currentImage);

            foreach (IAlgorithm algorithm in Algorithms)
            {
                algorithm.ProcessingImage = currentImage;
                //await Task.Run(algorithm.Execute);
                algorithm.Execute();
                currentImage = algorithm.ProcessedImage;
                Progress += 1.0f / Algorithms.Count;
            }

            ResultImage = currentImage;
        }
    }
}
