using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using SkiaSharp;
using Xamarin.Forms;

namespace ga_forms.Common
{
    static class BitmapExtensions
    {
        // Bitmap pipeline
        public static SKBitmap LoadBitmapResource(Type type, string resourceID)
        {
            Assembly assembly = type.GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                return SKBitmap.Decode(stream);
            }
        }

        public static async Task<SKBitmap> LoadBitmapFromGallery()
        {
            IPhotoLibrary photoLibrary = DependencyService.Get<IPhotoLibrary>();
            using (Stream stream = await photoLibrary.PickPhotoAsync())
            {
                if (stream != null)
                {
                    return SKBitmap.Decode(stream);
                }
            }

            throw new IOException("Failed to load bitmap from library");
        }

        public static Image GetImageFromBitmap(SKBitmap bitmap)
        {
            Image img = new Image();

            SKImage image = SKImage.FromPixels(bitmap.PeekPixels());
            SKData encoded = image.Encode();
            Stream stream = encoded.AsStream();
            img.Source = ImageSource.FromStream(() => stream);

            return img;
        }


        // Bitmap drawing
        public static void DrawBitmap(this SKCanvas canvas, SKBitmap bitmap, SKRect dest,
                                      BitmapStretch stretch,
                                      BitmapAlignment horizontal = BitmapAlignment.Center,
                                      BitmapAlignment vertical = BitmapAlignment.Center,
                                      SKPaint paint = null)
        {
            if (stretch == BitmapStretch.Fill)
            {
                canvas.DrawBitmap(bitmap, dest, paint);
            }
            else
            {
                float scale = 1;

                switch (stretch)
                {
                    case BitmapStretch.None:
                        break;

                    case BitmapStretch.Uniform:
                        scale = Math.Min(dest.Width / bitmap.Width, dest.Height / bitmap.Height);
                        break;

                    case BitmapStretch.UniformToFill:
                        scale = Math.Max(dest.Width / bitmap.Width, dest.Height / bitmap.Height);
                        break;
                }

                SKRect display = CalculateDisplayRect(dest, scale * bitmap.Width, scale * bitmap.Height,
                                                      horizontal, vertical);

                canvas.DrawBitmap(bitmap, display, paint);
            }
        }

        public static void DrawBitmap(this SKCanvas canvas, SKBitmap bitmap, SKRect source, SKRect dest,
                                      BitmapStretch stretch,
                                      BitmapAlignment horizontal = BitmapAlignment.Center,
                                      BitmapAlignment vertical = BitmapAlignment.Center,
                                      SKPaint paint = null)
        {
            if (stretch == BitmapStretch.Fill)
            {
                canvas.DrawBitmap(bitmap, source, dest, paint);
            }
            else
            {
                float scale = 1;

                switch (stretch)
                {
                    case BitmapStretch.None:
                        break;

                    case BitmapStretch.Uniform:
                        scale = Math.Min(dest.Width / source.Width, dest.Height / source.Height);
                        break;

                    case BitmapStretch.UniformToFill:
                        scale = Math.Max(dest.Width / source.Width, dest.Height / source.Height);
                        break;
                }

                SKRect display = CalculateDisplayRect(dest, scale * source.Width, scale * source.Height,
                                                      horizontal, vertical);

                canvas.DrawBitmap(bitmap, source, display, paint);
            }
        }


        // Bitmap transformations
        public static SKBitmap RotateBitmap(string path, int degrees)
        {
            SKBitmap rotatedBitmap;

            using (var bitmap = SKBitmap.Decode(path))
            {
                rotatedBitmap = new SKBitmap(bitmap.Height, bitmap.Width);

                using (var surface = new SKCanvas(rotatedBitmap))
                {
                    surface.Translate(rotatedBitmap.Width, 0);
                    surface.RotateDegrees(degrees);
                    surface.DrawBitmap(bitmap, 0, 0);
                }
            }

            return rotatedBitmap;
        }

        public static SKBitmap RotateBitmap(SKBitmap bitmap, int degrees)
        {
            var rotatedBitmap = new SKBitmap(bitmap.Height, bitmap.Width);

            using (var surface = new SKCanvas(rotatedBitmap))
            {
                surface.Translate(rotatedBitmap.Width, 0);
                surface.RotateDegrees(degrees);
                surface.DrawBitmap(bitmap, 0, 0);
            }

            return rotatedBitmap;
        }


        // Color converters
        public static uint RgbaMakePixel(byte red, byte green, byte blue, byte alpha = 255)
        {
            return (uint)((alpha << 24) | (blue << 16) | (green << 8) | red);
        }

        public static void RgbaGetBytes(this uint pixel, out byte red, out byte green, out byte blue, out byte alpha)
        {
            red = (byte)pixel;
            green = (byte)(pixel >> 8);
            blue = (byte)(pixel >> 16);
            alpha = (byte)(pixel >> 24);
        }

        public static uint BgraMakePixel(byte blue, byte green, byte red, byte alpha = 255)
        {
            return (uint)((alpha << 24) | (red << 16) | (green << 8) | blue);
        }

        public static void BgraGetBytes(this uint pixel, out byte blue, out byte green, out byte red, out byte alpha)
        {
            blue = (byte)pixel;
            green = (byte)(pixel >> 8);
            red = (byte)(pixel >> 16);
            alpha = (byte)(pixel >> 24);
        }

        public static HsvColor RgbToHsv(SKColor rgb)
        {
            // Get RGB
            double r = rgb.Red;
            double g = rgb.Green;
            double b = rgb.Blue;

            // Scale RGB
            r = r / 255.0;
            g = g / 255.0;
            b = b / 255.0;

            // Auxiliary variables
            double cmin = Math.Min(r, Math.Min(g, b));
            double cmax = Math.Max(r, Math.Max(g, b));
            double delta = cmax - cmin;

            // h
            int h = 0;
            if (delta == 0)
                h = 0;
            else if (cmax == r)
                h = (int)(60 * (Math.Abs(g - b) / delta % 6));
            else if (cmax == g)
                h = (int)(60 * (Math.Abs(b - r) / delta) + 2);
            else if (cmax == b)
                h = (int)(60 * (Math.Abs(r - g) / delta) + 4);

            // s
            double s;
            if (cmax == 0)
                s = 0;
            else
                s = delta / cmax;

            // v
            double v = cmax;

            // Return HSV
            return new HsvColor(h, s, v);
        }

        public static SKColor HsvToRgb(HsvColor hsv)
        {
            // Get HSV
            int h = hsv.H;
            double s = hsv.S;
            double v = hsv.V;

            // Auxiliary variables
            double c = v * s;
            double x = c * (1 - Math.Abs(h / 60) % 2 - 1);
            double m = v - c;

            // r', g', b'
            double r = 0, g = 0, b = 0;
            if (0 <= h && h < 60)
            {
                r = c; g = x; b = 0;
            }
            else if (60 <= h && h < 120)
            {
                r = x; g = c; b = 0;
            }
            else if (120 <= h && h < 180)
            {
                r = 0; g = c; b = x;
            }
            else if (180 <= h && h < 240)
            {
                r = 0; g = x; b = c;
            }
            else if (240 <= h && h < 300)
            {
                r = x; g = 0; b = c;
            }
            else if (300 <= h && h < 360)
            {
                r = c; g = 0; b = x;
            }

            // Return RGB
            return new SKColor((byte)((r + m) * 255), (byte)((g + m) * 255), (byte)((b + m) * 255));
        }


        // Private methods
        static SKRect CalculateDisplayRect(SKRect dest, float bmpWidth, float bmpHeight,
                                       BitmapAlignment horizontal, BitmapAlignment vertical)
        {
            float x = 0;
            float y = 0;

            switch (horizontal)
            {
                case BitmapAlignment.Center:
                    x = (dest.Width - bmpWidth) / 2;
                    break;

                case BitmapAlignment.Start:
                    break;

                case BitmapAlignment.End:
                    x = dest.Width - bmpWidth;
                    break;
            }

            switch (vertical)
            {
                case BitmapAlignment.Center:
                    y = (dest.Height - bmpHeight) / 2;
                    break;

                case BitmapAlignment.Start:
                    break;

                case BitmapAlignment.End:
                    y = dest.Height - bmpHeight;
                    break;
            }

            x += dest.Left;
            y += dest.Top;

            return new SKRect(x, y, x + bmpWidth, y + bmpHeight);
        }
    }


    public struct HsvColor
    {
        public readonly int H;
        public readonly double S;
        public readonly double V;

        public HsvColor(int h, double s, double v)
        {
            H = h;
            S = s;
            V = v;
        }
    }

    // Enums
    public enum BitmapStretch
    {
        None,
        Fill,
        Uniform,
        UniformToFill,
        AspectFit = Uniform,
        AspectFill = UniformToFill
    }

    public enum BitmapAlignment
    {
        Start,
        Center,
        End
    }
}
