using ga_forms.Models.ImageProcessing.Algorithms;
using SkiaSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ga_forms.Models.ImageProcessing
{
    class AlgorithmsPipeline
    {
        //ctor
        public AlgorithmsPipeline(SKBitmap initialImage, List<IAlgorithm> algorithms)
        {
            InitialImage = initialImage;
            Algorithms = algorithms;
        }

        public SKBitmap InitialImage { get; set; }
        public SKBitmap ResultImage { get; set; }
        public List<IAlgorithm> Algorithms { get; set; }
        public float Progress { get; set; }

        public async Task ExecutePipeline()
        {
            Progress = 0.0f;
            SKBitmap currentImage = InitialImage;
            foreach (IAlgorithm algorithm in Algorithms)
            {
                algorithm.ProcessingImage = currentImage;
                await Task.Run(algorithm.Execute);
                currentImage = algorithm.ProcessedImage;
                Progress += 1.0f / Algorithms.Count;
            }
        }
    }
}
