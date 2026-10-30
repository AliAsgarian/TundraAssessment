Tundra Assessment
=========================================

Overview
--------
This application counts vertical black lines in a black and white JPG image. 
It accepts one command line argument: the absolute path to the image.
On success, it prints only the number of vertical lines.

Requirements
------------
- .NET 10 SDK
- StbImageSharp NuGet package (referenced by the project)
- Windows or macOS for running the console application

Build
-----
From the folder containing TundraAssessment.csproj, run:

    dotnet restore
    dotnet build -c Release

Run
---
Pass exactly one argument containing the absolute path to a .jpg image.

Windows (PowerShell example):
    dotnet run -- "C:\full\path\to\img_1.jpg"

macOS (Terminal, when the image is in the current project folder):
    dotnet run -- "$(pwd)/img_1.jpg"

Expected results for the supplied test images:
    img_1.jpg    1
    img_2.jpg    3
    img_3.jpg    1
    img_4.jpg    7

How It Works
------------
1. Validate that exactly one argument was provided, the path is absolute, the file exists, and its extension is .jpg (case insensitive).
2. Decode the image into RGB pixel data using StbImageSharp.
3. Scan the horizontal row in the middle row of the image. The assignment states that each vertical line is straight and continuous through both halves, so each line crosses this row.
4. Treat a pixel as black when its red, green, and blue values are all below 128. This allows minor deviations caused by JPG compression.
5. Count each transition from a non black pixel to a black pixel. Adjacent black pixels belong to the same line and are counted only once.

Errors and Limitations
----------------------
The program prints an error message for missing or extra arguments, relative paths, missing files, files without a .jpg extension, or exceptions while reading/decoding the image.

The approach assumes the assignment's specified images: 
black vertical lines on a white background, with each line crossing the middle row. It does not attempt to identify arbitrary shapes or disconnected line segments.