namespace ga_forms.Models.ImageProcessing.Algorithms
{
    class DiseaseSeverity
    {
        public static double DiseasePercentage()
        {
            int totalNumberOfPixels = 0;
            int numberOfBlackPixels = 0;

            for (int x = 0; x < AlgorithmsPipeline.ResultImage.Width; ++x)
            {
                for (int y = 0; y < AlgorithmsPipeline.ResultImage.Height; ++y)
                {
                    if (AlgorithmsPipeline.ResultImage.GetPixel(x, y).Alpha != 0)
                    {
                        totalNumberOfPixels++;
                        if (AlgorithmsPipeline.ResultImage.GetPixel(x, y).Red == 0)
                        {
                            numberOfBlackPixels++;
                        }
                    }
                }
            }
            return numberOfBlackPixels * 100 / totalNumberOfPixels;
        }
    }
}
