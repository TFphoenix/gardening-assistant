using SkiaSharp;

namespace ga_forms.Models.ImageProcessing.Algorithms
{
    class Otsu:IAlgorithm
    {
        public SKBitmap ProcessingImage { get; set; }
        public SKBitmap ProcessedImage { get; set; }
        public void Execute()
        {
            ProcessedImage = new SKBitmap(ProcessingImage.Width, ProcessingImage.Height);

            double[] GreyLevels = new double[256];
            double GlSum = 0;

            for (int x = 0; x < ProcessingImage.Width; x++)
            {
                for (int y = 0; y < ProcessingImage.Height; y++)
                {
                    GreyLevels[ProcessingImage.GetPixel(y, x).Red] += 1;
                }
            }

            for (int index = 0; index < 256; index++)
            {
                GreyLevels[index] /= (ProcessingImage.Width * ProcessingImage.Height);
                GlSum += GreyLevels[index];
            }

            double FirstClassProb, SecondClassProb;

            double FirstClassMean, SecondClassMean, MeanGrey;

            double FirstSigma, SecondSigma, MaxSigma = 0;

            double zwSigma, imSigma;

            int BestThreshold = 0;

            for (int Threshold = 1; Threshold < 255; Threshold++)
            {
                FirstClassMean = 0;
                SecondClassMean = 0;

                FirstClassProb = 0;
                SecondClassProb = 0;

                FirstSigma = 0;
                SecondSigma = 0;

                for (int index = 0; index <= Threshold; index++)
                {
                    FirstClassProb += GreyLevels[index];
                }

                SecondClassProb = GlSum - FirstClassProb;

                for (int index = 0; index < 256; index++)
                {
                    if (index <= Threshold)
                    {
                        FirstClassMean += index * GreyLevels[index];
                    }
                    else
                    {
                        SecondClassMean += index * GreyLevels[index];
                    }
                }

                FirstClassMean = ((double)1 / FirstClassProb) * FirstClassMean;
                SecondClassMean = ((double)1 / SecondClassProb) * SecondClassMean;

                MeanGrey = FirstClassMean * FirstClassProb + SecondClassProb * SecondClassMean;

                for (int index = 0; index < 256; index++)
                {
                    if (index <= Threshold)
                    {
                        FirstSigma += GreyLevels[index] * System.Math.Pow(index - FirstClassMean, 2);
                    }
                    else
                    {
                        SecondSigma += GreyLevels[index] * System.Math.Pow(index - SecondClassMean, 2);
                    }
                }

                zwSigma = FirstClassProb * System.Math.Pow(FirstClassMean - MeanGrey, 2) + SecondClassProb * System.Math.Pow(SecondClassMean - MeanGrey, 2);
                imSigma = FirstClassProb * FirstSigma + SecondClassProb * SecondSigma;

                double val = zwSigma / imSigma;

                if (val > MaxSigma)
                {
                    MaxSigma = val;
                    BestThreshold = Threshold;
                }
            }

            for (int y = 0; y < ProcessingImage.Height; y++)
            {
                for (int x = 0; x < ProcessingImage.Width; x++)
                {
                    if (ProcessingImage.GetPixel(x, y).Red >= BestThreshold)
                    {
                        ProcessedImage.SetPixel(x, y, new SKColor(255,255,255));
                    }
                    else
                    {
                        ProcessedImage.SetPixel(x,y, new SKColor(0, 0, 0));
                    }
                }
            }
        }
    }
}
