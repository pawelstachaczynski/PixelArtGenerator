//using System;
//using System.Drawing;
//using System.IO;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace PixelArtGeneratorBackend.Controllers
//{
//    [Route("api/pixel")]
//    public class PixelController : ControllerBase
//    {
//        [HttpPost]
//        public IActionResult GeneratePixelArt(IFormFile imageFile)
//        {
//            try
//            {
//                if (imageFile == null || imageFile.Length == 0)
//                    return BadRequest("Nieprawidłowe dane wejściowe.");

//                // Przekształć obraz na piksel art
//                var pixelArt = GeneratePixelArtFromImage(imageFile.OpenReadStream());

//                // Zapisz pixel art do strumienia
//                var memoryStream = new MemoryStream();
//                pixelArt.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
//                memoryStream.Position = 0;

//                // Wyślij odpowiedź z pixel artem
//                return File(memoryStream, "image/png");
//            }
//            catch (Exception ex)
//            {
//                // Obsłuż wyjątek
//                return StatusCode(500, ex.Message);
//            }
//        }

//        private Bitmap GeneratePixelArtFromImage(Stream inputStream)
//        {
//            // Przekształć obraz na piksel art
//            var originalImage = new Bitmap(inputStream);
//            var pixelArt = new Bitmap(originalImage.Width, originalImage.Height);

//            // Ustaw wielkość piksela w pixel arcie (tutaj 8x8 pikseli)
//            const int pixelSize = 8;

//            for (int x = 0; x < originalImage.Width; x += pixelSize)
//            {
//                for (int y = 0; y < originalImage.Height; y += pixelSize)
//                {
//                    // Pobierz kolor piksela na podstawie średnich wartości kolorów w bloku
//                    var color = GetAverageColor(originalImage, x, y, pixelSize);

//                    // Przypisz kolor piksela w pixel arcie
//                    for (int i = x; i < x + pixelSize; i++)
//                    {
//                        for (int j = y; j < y + pixelSize; j++)
//                        {
//                            if (i < pixelArt.Width && j < pixelArt.Height)
//                                pixelArt.SetPixel(i, j, color);
//                        }
//                    }
//                }
//            }

//            return pixelArt;
//        }

//        private Color GetAverageColor(Bitmap image, int startX, int startY, int blockSize)
//        {
//            var red = 0;
//            var green = 0;
//            var blue = 0;
//            var count = 0;

//            for (int x = startX; x < startX + blockSize; x++)
//            {
//                for (int y = startY; y < startY + blockSize; y++)
//                {
//                    if (x < image.Width && y < image.Height)
//                    {
//                        var pixelColor = image.GetPixel(x, y);
//                        red += pixelColor.R;
//                        green += pixelColor.G;
//                        blue += pixelColor.B;
//                        count++;
//                    }
//                }
//            }

//            if (count > 0)
//            {
//                red /= count;
//                green /= count;
//                blue /= count;
//            }

//            return Color.FromArgb(red, green, blue);
//        }
//    }
//}


// Wersja 2
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.IO;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.PixelFormats;
//using SixLabors.ImageSharp.Processing;
//using SixLabors.ImageSharp.Processing.Processors.Quantization;

//namespace PixelArtGeneratorBackend.Controllers
//{
//    [Route("api/pixel")]
//    public class PixelController : ControllerBase
//    {
//        [HttpPost]
//        public IActionResult GeneratePixelArt(IFormFile imageFile, int pixelSize, PixelColorPalette colorPalette)
//        {
//            try
//            {
//                if (imageFile == null || imageFile.Length == 0)
//                    return BadRequest("Nieprawidłowe dane wejściowe.");

//                // Przekształć obraz na piksel art
//                var pixelArt = GeneratePixelArtFromImage(imageFile.OpenReadStream(), pixelSize, colorPalette);

//                // Zapisz pixel art do strumienia
//                var memoryStream = new MemoryStream();
//                pixelArt.Save(memoryStream, SixLabors.ImageSharp.Formats.Png.PngFormat.Instance);
//                memoryStream.Position = 0;

//                // Wyślij odpowiedź z pixel artem
//                return File(memoryStream, "image/png");
//            }
//            catch (Exception ex)
//            {
//                // Obsłuż wyjątek
//                return StatusCode(500, ex.Message);
//            }
//        }

//        private Image<Rgba32> GeneratePixelArtFromImage(Stream inputStream, int pixelSize, PixelColorPalette colorPalette)
//        {
//            // Przekształć obraz na piksel art
//            using (var image = SixLabors.ImageSharp.Image.Load<Rgba32>(inputStream))
//            {
//                var pixelArt = new Image<Rgba32>(image.Width * pixelSize, image.Height * pixelSize);

//                for (int x = 0; x < image.Width; x++)
//                {
//                    for (int y = 0; y < image.Height; y++)
//                    {
//                        var pixelColor = image[x, y];

//                        // Przypisz kolor piksela w pixel arcie na podstawie pixelColor
//                        for (int i = 0; i < pixelSize; i++)
//                        {
//                            for (int j = 0; j < pixelSize; j++)
//                            {
//                                var targetX = x * pixelSize + i;
//                                var targetY = y * pixelSize + j;

//                                pixelArt[targetX, targetY] = GetClosestColor(pixelColor, colorPalette);
//                            }
//                        }
//                    }
//                }

//                return pixelArt;
//            }
//        }

//        private Rgba32 GetClosestColor(Rgba32 pixelColor, PixelColorPalette colorPalette)
//        {
//            var closestColor = colorPalette.Colors()[0];
//            var minDistance = CalculateColorDistance(pixelColor, closestColor);

//            foreach (var color in colorPalette.Colors())
//            {
//                var distance = CalculateColorDistance(pixelColor, color);
//                if (distance < minDistance)
//                {
//                    minDistance = distance;
//                    closestColor = color;
//                }
//            }

//            return closestColor;
//        }

//        private float CalculateColorDistance(Rgba32 color1, Rgba32 color2)
//        {
//            var rDiff = color1.R - color2.R;
//            var gDiff = color1.G - color2.G;
//            var bDiff = color1.B - color2.B;

//            return (rDiff * rDiff) + (gDiff * gDiff) + (bDiff * bDiff);
//        }
//    }

//    public enum PixelColorPalette
//    {
//        Grayscale,
//        RGB
//    }

//    public static class PixelColorPaletteExtensions
//    {
//        public static List<Rgba32> Colors(this PixelColorPalette colorPalette)
//        {
//            switch (colorPalette)
//            {
//                case PixelColorPalette.Grayscale:
//                    return new List<Rgba32>
//                    {
//                        new Rgba32(0, 0, 0),
//                        new Rgba32(255, 255, 255)
//                    };

//                case PixelColorPalette.RGB:
//                    return new List<Rgba32>
//                    {
//                        new Rgba32(255, 0, 0),
//                        new Rgba32(0, 255, 0),
//                        new Rgba32(0, 0, 255)
//                    };

//                default:
//                    throw new NotSupportedException("Nieobsługiwana paleta kolorów.");
//            }
//        }
//    }
//}

//Tu już całkiem spoko

//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.IO;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.PixelFormats;
//using SixLabors.ImageSharp.Processing;
//using SixLabors.ImageSharp.Processing.Processors.Quantization;

//namespace PixelArtGeneratorBackend.Controllers
//{
//    [Route("api/pixel")]
//    public class PixelController : ControllerBase
//    {
//        [HttpPost]
//        public IActionResult GeneratePixelArt(IFormFile imageFile, int width, int height, PixelColorPalette colorPalette)
//        {
//            try
//            {
//                if (imageFile == null || imageFile.Length == 0)
//                    return BadRequest("Nieprawidłowe dane wejściowe.");

//                // Przekształć obraz na piksel art
//                var pixelArt = GeneratePixelArtFromImage(imageFile.OpenReadStream(), width, height, colorPalette);

//                // Zapisz pixel art do strumienia
//                var memoryStream = new MemoryStream();
//                pixelArt.Save(memoryStream, SixLabors.ImageSharp.Formats.Png.PngFormat.Instance);
//                memoryStream.Position = 0;

//                // Wyślij odpowiedź z pixel artem
//                return File(memoryStream, "image/png");
//            }
//            catch (Exception ex)
//            {
//                // Obsłuż wyjątek
//                return StatusCode(500, ex.Message);
//            }
//        }

//        private Image<Rgba32> GeneratePixelArtFromImage(Stream inputStream, int width, int height, PixelColorPalette colorPalette)
//        {
//            // Przekształć obraz na piksel art
//            using (var image = SixLabors.ImageSharp.Image.Load<Rgba32>(inputStream))
//            {
//                image.Mutate(x => x.Resize(width, height));

//                var pixelArt = new Image<Rgba32>(image.Width, image.Height);

//                for (int x = 0; x < image.Width; x++)
//                {
//                    for (int y = 0; y < image.Height; y++)
//                    {
//                        var pixelColor = image[x, y];

//                        // Przypisz kolor piksela w pixel arcie na podstawie pixelColor
//                        pixelArt[x, y] = GetClosestColor(pixelColor, colorPalette);
//                    }
//                }

//                return pixelArt;
//            }
//        }

//        private Rgba32 GetClosestColor(Rgba32 pixelColor, PixelColorPalette colorPalette)
//        {
//            var closestColor = colorPalette.Colors()[0];
//            var minDistance = CalculateColorDistance(pixelColor, closestColor);

//            foreach (var color in colorPalette.Colors())
//            {
//                var distance = CalculateColorDistance(pixelColor, color);
//                if (distance < minDistance)
//                {
//                    minDistance = distance;
//                    closestColor = color;
//                }
//            }

//            return closestColor;
//        }

//        private float CalculateColorDistance(Rgba32 color1, Rgba32 color2)
//        {
//            var rDiff = color1.R - color2.R;
//            var gDiff = color1.G - color2.G;
//            var bDiff = color1.B - color2.B;

//            return (rDiff * rDiff) + (gDiff * gDiff) + (bDiff * bDiff);
//        }
//    }

//    public enum PixelColorPalette
//    {
//        Grayscale,
//        RGB
//    }

//    public static class PixelColorPaletteExtensions
//    {
//        public static List<Rgba32> Colors(this PixelColorPalette colorPalette)
//        {
//            switch (colorPalette)
//            {
//                case PixelColorPalette.Grayscale:
//                    return new List<Rgba32>
//                    {
//                        new Rgba32(0, 0, 0),
//                        new Rgba32(255, 255, 255)
//                    };

//                case PixelColorPalette.RGB:
//                    return new List<Rgba32>
//                    {
//                        new Rgba32(255, 0, 0),
//                        new Rgba32(0, 255, 0),
//                        new Rgba32(0, 0, 255)
//                    };

//                default:
//                    throw new NotSupportedException("Nieobsługiwana paleta kolorów.");
//            }
//        }
//    }
//}

//Super !

//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.IO;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.PixelFormats;
//using SixLabors.ImageSharp.Processing;
//using SixLabors.ImageSharp.Processing.Processors.Quantization;

//namespace PixelArtGeneratorBackend.Controllers
//{
//    [Route("api/pixel")]
//    public class PixelController : ControllerBase
//    {
//        [HttpPost]
//        public IActionResult GeneratePixelArt(IFormFile imageFile, int width, int height, PixelColorPalette colorPalette)
//        {
//            try
//            {
//                if (imageFile == null || imageFile.Length == 0)
//                    return BadRequest("Nieprawidłowe dane wejściowe.");

//                // Przekształć obraz na piksel art
//                var pixelArt = GeneratePixelArtFromImage(imageFile.OpenReadStream(), width, height, colorPalette);

//                // Zapisz pixel art do strumienia
//                var memoryStream = new MemoryStream();
//                pixelArt.Save(memoryStream, SixLabors.ImageSharp.Formats.Png.PngFormat.Instance);
//                memoryStream.Position = 0;

//                // Wyślij odpowiedź z pixel artem
//                return File(memoryStream, "image/png");
//            }
//            catch (Exception ex)
//            {
//                // Obsłuż wyjątek
//                return StatusCode(500, ex.Message);
//            }
//        }

//        private Image<Rgba32> GeneratePixelArtFromImage(Stream inputStream, int width, int height, PixelColorPalette colorPalette)
//        {
//            // Przekształć obraz na piksel art
//            using (var image = SixLabors.ImageSharp.Image.Load<Rgba32>(inputStream))
//            {
//                image.Mutate(x => x.Resize(width, height));

//                var pixelArt = new Image<Rgba32>(image.Width, image.Height);

//                for (int x = 0; x < image.Width; x++)
//                {
//                    for (int y = 0; y < image.Height; y++)
//                    {
//                        var pixelColor = image[x, y];

//                        // Przypisz kolor piksela w pixel arcie na podstawie pixelColor
//                        pixelArt[x, y] = GetClosestColor(pixelColor, colorPalette);
//                    }
//                }

//                return pixelArt;
//            }
//        }

//        private Rgba32 GetClosestColor(Rgba32 pixelColor, PixelColorPalette colorPalette)
//        {
//            var closestColor = colorPalette.Colors()[0];
//            var minDistance = CalculateColorDistance(pixelColor, closestColor);

//            foreach (var color in colorPalette.Colors())
//            {
//                var distance = CalculateColorDistance(pixelColor, color);
//                if (distance < minDistance)
//                {
//                    minDistance = distance;
//                    closestColor = color;
//                }
//            }

//            return closestColor;
//        }

//        private float CalculateColorDistance(Rgba32 color1, Rgba32 color2)
//        {
//            var rDiff = color1.R - color2.R;
//            var gDiff = color1.G - color2.G;
//            var bDiff = color1.B - color2.B;

//            return (rDiff * rDiff) + (gDiff * gDiff) + (bDiff * bDiff);
//        }
//    }

//    public enum PixelColorPalette
//    {
//        Grayscale,
//        RGB,
//        Sepia,
//        Pastel,
//        Neon
//    }

//    public static class PixelColorPaletteExtensions
//    {
//        public static List<Rgba32> Colors(this PixelColorPalette colorPalette)
//        {
//            switch (colorPalette)
//            {
//                case PixelColorPalette.Grayscale:
//                    return new List<Rgba32>
//                    {
//                        new Rgba32(0, 0, 0),
//                        new Rgba32(255, 255, 255)
//                    };

//                case PixelColorPalette.RGB:
//                    return new List<Rgba32>
//                    {
//                        new Rgba32(255, 0, 0),
//                        new Rgba32(0, 255, 0),
//                        new Rgba32(0, 0, 255)
//                    };

//                case PixelColorPalette.Sepia:
//                    return new List<Rgba32>
//                    {
//                        new Rgba32(112, 66, 20),
//                        new Rgba32(219, 173, 109)
//                    };

//                case PixelColorPalette.Pastel:
//                    return new List<Rgba32>
//                    {
//                        new Rgba32(252, 228, 236),
//                        new Rgba32(204, 255, 204)
//                    };

//                case PixelColorPalette.Neon:
//                    return new List<Rgba32>
//                    {
//                        new Rgba32(255, 0, 0),
//                        new Rgba32(0, 255, 0),
//                        new Rgba32(0, 0, 255),
//                        new Rgba32(255, 255, 0)
//                    };

//                default:
//                    throw new NotSupportedException("Nieobsługiwana paleta kolorów.");
//            }
//        }
//    }
//}