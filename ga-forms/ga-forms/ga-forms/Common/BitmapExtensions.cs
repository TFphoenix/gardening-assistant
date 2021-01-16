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

            // hue (scaled)
            double h = 0;
            if (cmax == r)
                h = (g - b) / delta;
            else if (cmax == g)
                h = 2.0d + (b - r) / delta;
            else if (cmax == b)
                h = 4.0 + (r - g) / delta;

            // hue (degrees)
            h *= 60;
            if (h < 0) h += 360;

            // saturation
            double s;
            if (cmax == 0)
                s = 0;
            else
                s = delta / cmax;

            // value
            double v = cmax;

            // Return HSV
            return new HsvColor((int)h, s, v);
        }

        public static SKColor HsvToRgb(HsvColor hsv)
        {
            // Get HSV
            int h = hsv.Hue;
            double s = hsv.Saturation;
            double v = hsv.Value;

            // r', g', b'
            double r, g, b;

            if (h == 0 && s == 0)
            {
                r = v * 255.0d;
                g = v * 255.0d;
                b = v * 255.0d;
            }
            else
            {
                // aux1, aux2
                double aux1, aux2;
                if (v < 0.5d)
                {
                    aux1 = v * (1.0d + s);
                }
                else
                {
                    aux1 = v + s - v * s;
                }
                aux2 = 2.0d * v - aux1;

                // aux: h
                double aux_h = h / 360.0d;

                // aux: r, g, b
                var aux_r = aux_h + 0.333d;
                var aux_g = aux_h;
                var aux_b = aux_h - 0.333d;

                if (aux_r > 1) aux_r -= 1.0d;
                else if (aux_r < 0) aux_r += 1.0d;
                if (aux_g > 1) aux_g -= 1.0d;
                else if (aux_g < 0) aux_g += 1.0d;
                if (aux_b > 1) aux_b -= 1.0d;
                else if (aux_b < 0) aux_b += 1.0d;

                // red
                if (6.0d * aux_r < 1) r = aux2 + (aux1 - aux2) * 6.0d * aux_r;
                else if (2.0d * aux_r < 1) r = aux1;
                else if (3.0d * aux_r < 2) r = aux2 + (aux1 - aux2) * (0.666d - aux_r) * 6.0d;
                else r = aux2;

                // green
                if (6.0d * aux_g < 1) g = aux2 + (aux1 - aux2) * 6.0d * aux_g;
                else if (2.0d * aux_g < 1) g = aux1;
                else if (3.0d * aux_g < 2) g = aux2 + (aux1 - aux2) * (0.666d - aux_g) * 6.0d;
                else g = aux2;

                // blue
                if (6.0d * aux_b < 1) b = aux2 + (aux1 - aux2) * 6.0d * aux_b;
                else if (2.0d * aux_b < 1) b = aux1;
                else if (3.0d * aux_b < 2) b = aux2 + (aux1 - aux2) * (0.666d - aux_b) * 6.0d;
                else b = aux2;
            }

            // Return RGB
            return new SKColor((byte)(255 * r), (byte)(255 * g), (byte)(255 * b));
        }

        // Color distances
        public static double EuclideanDistance(SKColor c1, SKColor c2)
        {
            return Math.Sqrt(Math.Pow(c1.Red - c2.Red, 2) + Math.Pow(c1.Green - c2.Green, 2) + Math.Pow(c1.Blue - c2.Blue, 2));
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
        public readonly int Hue;
        public readonly double Saturation;
        public readonly double Value;

        public HsvColor(int hue, double saturation, double value)
        {
            Hue = hue;
            Saturation = saturation;
            Value = value;
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
