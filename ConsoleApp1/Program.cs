﻿using System.Drawing;
using System.Security.Cryptography;

class Program
{
    static void Main(string[] args)
    {
        string masterDir = "MasterShots";
        string testDir = "TestShots";
        string outputDir = "DiffResults";

        if (!System.IO.Directory.Exists(outputDir))
        {
            System.IO.Directory.CreateDirectory(outputDir);
        }

        string[] masterFiles = System.IO.Directory.GetFiles(masterDir);
        foreach (string masterFile in masterFiles)
        {
            string fileName = System.IO.Path.GetFileName(masterFile);
            string testFile = System.IO.Path.Combine(testDir, fileName);

            if (System.IO.File.Exists(testFile))
            {

                string masterHash = CalculateImageHash(masterFile);
                string testHash = CalculateImageHash(testFile);

                if (masterHash == testHash)
                {
                    Console.WriteLine("Bilder sind gleich!");
                }
                else
                {
                    Bitmap masterImage = new Bitmap(masterFile);
                    Bitmap testImage = new Bitmap(testFile);
                    SaveDifferenceImage(masterImage, testImage, System.IO.Path.Combine(outputDir, fileName));
                    Console.WriteLine($"Diff-Bild wird im {outputDir}-Ordner gespeichert!");
                }
            }
        }
    }

    static string CalculateImageHash(string imagePath)
    {
        using (var md5 = MD5.Create())
        {
            using (var stream = File.OpenRead(imagePath))
            {
                byte[] hashBytes = md5.ComputeHash(stream);
                return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            }
        }
    }


    static void SaveDifferenceImage(Bitmap image1, Bitmap image2, string outputFilePath)
    {
        Bitmap differenceImage = new Bitmap(image1.Width, image1.Height);

        for (int x = 0; x < image1.Width; x++)
        {
            for (int y = 0; y < image1.Height; y++)
            {
                if (image1.GetPixel(x, y) != image2.GetPixel(x, y))
                {
                    Color masterPixel = image1.GetPixel(x, y);
                    differenceImage.SetPixel(x, y, masterPixel);
                }
                else
                {
                    differenceImage.SetPixel(x, y, Color.White);
                }
            }
        }

        differenceImage.Save(outputFilePath);
    }
}