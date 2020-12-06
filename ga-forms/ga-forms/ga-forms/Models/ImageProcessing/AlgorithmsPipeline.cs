using ga_forms.Models.ImageProcessing.Algorithms;
using ga_forms.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ga_forms.Models.ImageProcessing
{
    class AlgorithmsPipeline
    {
        private readonly IImageManagerService _imageManagerService;

        //ctor
        public AlgorithmsPipeline(SKBitmap initialImage, List<IAlgorithm> algorithms, IImageManagerService imageManagerService)
        {
            InitialImage = initialImage;
            Algorithms = algorithms;
            _imageManagerService = imageManagerService;
        }

        public SKBitmap InitialImage { get; set; }
        public SKBitmap ResultImage { get; set; }
        public List<IAlgorithm> Algorithms { get; set; }

        public void Execute()
        {
            InitialImage = _imageManagerService.HealthInitialImageBitmap;
            foreach (IAlgorithm algorithm in Algorithms)
            {
                algorithm.ProcessingImage = InitialImage;
                algorithm.Execute();
                InitialImage = algorithm.ProcessedImage;
            }
        }
    }
}
