
using System;
using System.IO;
using StbImageSharp;

namespace TundraAssessment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Check to see if the input is valid and if the file exists, if not it will return an error message.

                // The console must take exactly one argument
                if (args.Length != 1)
                {
                    Console.WriteLine(
                        "Error: Please provide exactly one image path.");
                    return;
                }

                string imagePath = args[0];

                // The file path must be absolute
                if (!Path.IsPathFullyQualified(imagePath))
                {
                    Console.WriteLine(
                        "Error: Please provide an absolute image path.");
                    return;
                }

                // The file must exist
                if (!File.Exists(imagePath))
                {
                    Console.WriteLine("Error: Image file not found.");
                    return;
                }

                // The file must be a JPG file
                if (!string.Equals(Path.GetExtension(imagePath),".jpg", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Error: Image must be a JPG file.");
                    return;
                }

                int lineCount = CountVerticalLines(imagePath);
                Console.WriteLine(lineCount); //output the number of vertical lines in the image
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        static int CountVerticalLines(string imagePath)
        {
            using FileStream stream = File.OpenRead(imagePath);
            ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlue);

            int lineCount = 0;
            bool isPreviousPixelBlack = false;
            const int BytesPerPixel = 3; // Each RGB pixel occupies three consecutive bytes. (This is how StbImageSharp represents RGB images.)
            int middleY = image.Height / 2;
            int rowStart = middleY * image.Width * BytesPerPixel; // This is the starting position of the middle row.
            const byte BlackThreshold = 128; // Since the pictures only contain black and white, we can use a threshold of 128 (255 / 2) to determine if the pixel is coloured (black) whilst accounting for JPEG compression.

            // Scan horizontally through the middle row.
            for (int x = 0; x < image.Width; x++)
            {
                int pixelIndex = rowStart + x * BytesPerPixel;

                //StbImageSharp stores pixel data in a byte array, where each pixel is represented by three consecutive bytes (RGB). The red component is at the current index, green is at the next index, and blue is at the index after that.
                byte red = image.Data[pixelIndex]; 
                byte green = image.Data[pixelIndex + 1];
                byte blue = image.Data[pixelIndex + 2];


                // Identify dark pixels
                bool isCurrentPixelBlack =
                    red < BlackThreshold &&
                    green < BlackThreshold &&
                    blue < BlackThreshold;

                // Current line is black but previous line is white, so we have found a new vertical line.
                if (isCurrentPixelBlack && !isPreviousPixelBlack)
                {
                    lineCount++;
                }

                isPreviousPixelBlack = isCurrentPixelBlack;
            }

            return lineCount;
        }
    }
}
