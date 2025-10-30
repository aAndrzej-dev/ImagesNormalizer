using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImagesNormalizer;
internal static class Program
{
    private static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Image Normalizer v1.0");
        Console.WriteLine("Copyright (c) aAndrzej-dev 2025");
        Console.ResetColor();

        Rect margin = new Rect();
        Rect border = new Rect();
        Color borderColor = Color.Black;
        int standardWidth = -1;
        int standardHeight = -1;
        bool autoDetectSize = false;
        string? sourceDir;
        string? targetDir;

        //Setup form agrs
        if (args.Length >= 2)
        {
            sourceDir = args[0];
            targetDir = args[1];

            for (int i = 2; i < args.Length; i++)
            {
                string element = args[i];
                if (element is "-m" or "-margin")
                {
                    if (args.Length <= i + 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Not enough parameters for margin");
                        return;
                    }
                    if (args.Length > i + 4)
                    {
                        if (int.TryParse(args[i + 2], out margin.top))
                        {
                            if (int.TryParse(args[i + 1], out margin.left) &&
                           int.TryParse(args[i + 3], out margin.right) &&
                           int.TryParse(args[i + 4], out margin.bottom))
                            {
                                i += 4;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid margin parameters");
                                return;
                            }
                        }
                        else
                        {
                            if (int.TryParse(args[i + 1], out int all))
                            {
                                margin = new Rect(all);
                                i++;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid margin parameters");
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (int.TryParse(args[i + 1], out int all))
                        {
                            margin = new Rect(all);
                            i++;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid margin parameters");
                            return;
                        }
                    }
                    continue;
                }
                if (element is "-b" or "-border")
                {
                    if (args.Length <= i + 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Not enough parameters for border");
                        return;
                    }

                    borderColor = ColorTranslator.FromHtml(args[i + 1]);
                    i++;

                    if (args.Length > i + 4)
                    {
                        if (int.TryParse(args[i + 2], out border.top))
                        {
                            if (int.TryParse(args[i + 1], out border.left) &&
                           int.TryParse(args[i + 3], out border.right) &&
                           int.TryParse(args[i + 4], out border.bottom))
                            {
                                i += 4;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid border parameters");
                                return;
                            }
                        }
                        else
                        {
                            if (int.TryParse(args[i + 1], out int all))
                            {
                                border = new Rect(all);
                                i++;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid border parameters");
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (int.TryParse(args[i + 1], out int all))
                        {
                            border = new Rect(all);
                            i++;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid border parameters");
                            return;
                        }
                    }
                    continue;
                }
                if (element == "-s")
                {
                    if (args.Length > i + 1 && args[i + 1] == "auto")
                    {
                        autoDetectSize = true;
                        i++;
                        continue;
                    }

                    if (args.Length <= i + 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Not enough parameters for standard size");
                        return;
                    }
                    if (!int.TryParse(args[i + 1], out standardWidth))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid standard width");
                        return;
                    }
                    if (!int.TryParse(args[i + 2], out standardHeight))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid standard height");
                        return;
                    }
                    i += 2;
                    continue;
                }
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Source directory: ");
            Console.ResetColor();
            sourceDir = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Target directory: ");
            Console.ResetColor();
            targetDir = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Standard size (auto, manual, none): ");
            Console.ResetColor();
            string? sizeInput = Console.ReadLine();

            if (sizeInput == "auto")
            {
                autoDetectSize = true;
            }
            else if (sizeInput == "manual")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Width: ");
                Console.ResetColor();
                string? wInput = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Height: ");
                Console.ResetColor();
                string? hInput = Console.ReadLine();
                if (!int.TryParse(wInput, out standardWidth) || !int.TryParse(hInput, out standardHeight))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid standard size input");
                    return;
                }
            }
            else if (sizeInput != "none" && !string.IsNullOrWhiteSpace(sizeInput))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid size input");
                return;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Margin (single value or left top right bottom): ");
            Console.ResetColor();
            string? marginInput = Console.ReadLine();
            string[] marginParts = marginInput?.Split(' ') ?? [];
            if (marginParts.Length == 1 && !string.IsNullOrWhiteSpace(marginParts[0]))
            {
                if (int.TryParse(marginParts[0], out int all))
                {
                    margin = new Rect(all);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid margin input");
                    return;
                }
            }
            else if (marginParts.Length == 4)
            {
                if (!int.TryParse(marginParts[0], out margin.left) ||
                    !int.TryParse(marginParts[1], out margin.top) ||
                    !int.TryParse(marginParts[2], out margin.right) ||
                    !int.TryParse(marginParts[3], out margin.bottom))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid margin input");
                    return;
                }
            }
            else if (marginParts.Length != 0 && !string.IsNullOrWhiteSpace(marginParts[0]))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid margin input");
                return;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Border color (HTML format, e.g. #ff0000): ");
            Console.ResetColor();
            string? borderColorInput = Console.ReadLine();
            borderColor = ColorTranslator.FromHtml(borderColorInput);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Border size (single value or left top right bottom): ");
            Console.ResetColor();
            string? borderInput = Console.ReadLine();
            string[] borderParts = borderInput?.Split(' ') ?? [];
            if (borderParts.Length == 1 && !string.IsNullOrWhiteSpace(borderParts[0]))
            {
                if (int.TryParse(borderParts[0], out int all))
                {
                    border = new Rect(all);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid border input");
                    return;
                }
            }
            else if (borderParts.Length == 4)
            {
                if (!int.TryParse(borderParts[0], out border.left) ||
                    !int.TryParse(borderParts[1], out border.top) ||
                    !int.TryParse(borderParts[2], out border.right) ||
                    !int.TryParse(borderParts[3], out border.bottom))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid border input");
                    return;
                }
            }
            else if (borderParts.Length != 0 && !string.IsNullOrWhiteSpace(borderParts[0]))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid border input");
                return;
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Setup complete!");
            Console.ResetColor();
        }
        if (string.IsNullOrWhiteSpace(sourceDir) || string.IsNullOrWhiteSpace(targetDir))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Source or target directory is not set");
            return;
        }
        DirectoryInfo sourceDI = new DirectoryInfo(sourceDir);
        if (!sourceDI.Exists)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Source directory does not exist");
            Console.WriteLine(sourceDI.FullName);
            return;
        }
        DirectoryInfo targetDI = new DirectoryInfo(targetDir);
        if (targetDI.Exists)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Target directory already exist");
            return;
        }
        targetDI.Create();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Operation setup: ");
        Console.WriteLine($"Source directory: {sourceDI.FullName}");
        Console.WriteLine($"Target directory: {targetDI.FullName}");



        if (autoDetectSize)
        {
            DetectTargetSize(sourceDI.GetFiles(), out standardWidth, out standardHeight);

            if (!margin.IsZero)
            {
                standardWidth += margin.left + margin.right;
                standardHeight += margin.top + margin.bottom;
                Console.WriteLine($"Margin: ({margin.left}, {margin.top}, {margin.right}, {margin.bottom})");
            }

            Console.WriteLine($"Auto-detected standard size (with margin): {standardWidth} x {standardHeight}");
        }
        else if (standardWidth > 0 && standardHeight > 0)
        {
            Console.WriteLine($"Standard size: {standardWidth} x {standardHeight}");
        }
        else
        {
            if (!margin.IsZero)
            {
                Console.WriteLine($"Margin: ({margin.left}, {margin.top}, {margin.right}, {margin.bottom})");
            }
        }
        if (!border.IsZero)
        {
            Console.WriteLine($"Border (#{borderColor.ToArgb():x}): ({border.left}, {border.top}, {border.right}, {border.bottom})");
        }
        Console.WriteLine();
        Console.ResetColor();
        FileInfo[] sourceFiles = sourceDI.GetFiles();
        Stopwatch sw = new Stopwatch();
        sw.Start();
        for (int i = 0; i < sourceFiles.Length; i++)
        {
            using FileStream fileStream = sourceFiles[i].OpenRead();
            using Bitmap sourceBitmap = new Bitmap(fileStream);

            int w = sourceBitmap.Width;
            int h = sourceBitmap.Height;
            int tW = standardWidth < 0 ? w + margin.left + margin.right : standardWidth;
            int tH = standardHeight < 0 ? h + margin.top + margin.bottom : standardHeight;

            if (tW < w || tH < h)
            {
                Console.WriteLine($"Source image {sourceFiles[i].Name} is larger than target size, skipping");
                continue;
            }

            uint[] pixels = new uint[tH * tW];

            int xOffset = standardWidth < 0 ? margin.left : (tW - w) / 2;
            int yOffset = standardHeight < 0 ? margin.top : (tH - h) / 2;

            // Copy image
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    pixels[((yOffset + y) * tW) + xOffset + x] = (uint)sourceBitmap.GetPixel(x, y).ToArgb();
                }
            }
            fileStream.Close();

            // Draw border

            if (xOffset - border.left < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Border left size is too big for image {sourceFiles[i].Name}, skipping");
                continue;
            }
            if (yOffset - border.top < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Border top size is too big for image {sourceFiles[i].Name}, skipping");
                continue;
            }
            if (xOffset + w + border.right > tW)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Border right size is too big for image {sourceFiles[i].Name}, skipping");
                continue;
            }
            if (yOffset + h + border.bottom > tH)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Border bottom size is too big for image {sourceFiles[i].Name}, skipping");
                continue;
            }


            for (int x = 0; x < w + border.left + border.right; x++)
            {
                for (int y = 0; y < border.top; y++)
                {
                    pixels[((yOffset - y - 1) * tW) + x - border.left + xOffset] = (uint)borderColor.ToArgb();
                }
                for (int y = 0; y < border.bottom; y++)
                {
                    pixels[((y + yOffset + h) * tW) + x - border.left + xOffset] = (uint)borderColor.ToArgb();
                }
            }
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < border.left; x++)
                {
                    pixels[((yOffset + y) * tW) + x - border.left + xOffset] = (uint)borderColor.ToArgb();
                }
                for (int x = 0; x < border.right; x++)
                {
                    pixels[((yOffset + y) * tW) + x + xOffset + w] = (uint)borderColor.ToArgb();
                }
            }

            // Convert to bitmap and save
            GCHandle gcHandle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            using Bitmap targetBitmap = new Bitmap(tW, tH, tW * sizeof(uint), PixelFormat.Format32bppArgb, gcHandle.AddrOfPinnedObject());

            targetBitmap.Save(Path.Combine(targetDI.FullName, sourceFiles[i].Name), ImageFormat.Png);
            Console.WriteLine($"File {sourceFiles[i].Name} has been converted");


            targetBitmap.Dispose();
            gcHandle.Free();
        }
        sw.Stop();
        Console.WriteLine($"Operation complete in {Math.Round(sw.Elapsed.TotalSeconds)}s!");
    }



    private static void DetectTargetSize(FileInfo[] sourceFiles, out int width, out int height)
    {
        int w = -1;
        int h = -1;
        for (int i = 0; i < sourceFiles.Length; i++)
        {
            using FileStream fileStream = sourceFiles[i].OpenRead();
            using Bitmap bmp = new Bitmap(fileStream);

            int cw = bmp.Width;
            int ch = bmp.Height;

            if (cw > w)
                w = cw;
            if (ch > h)
                h = ch;

            fileStream.Close();
        }
        width = w;
        height = h;
    }
}