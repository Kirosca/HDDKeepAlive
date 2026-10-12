using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public class IcoMaker
{
    public static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: IcoMaker.exe <input_image_path> <output_ico_path> [tray_image_path]");
            return;
        }

        string inputPath = args[0];
        string outputPath = args[1];
        string trayPath = args.Length >= 3 ? args[2] : null;

        int[] sizes = new int[] { 256, 128, 64, 48, 32, 16 };

        using (Bitmap mainBmp = new Bitmap(inputPath))
        using (Bitmap trayBmp = !string.IsNullOrEmpty(trayPath) && File.Exists(trayPath) ? new Bitmap(trayPath) : null)
        {
            byte[][] iconImages = new byte[sizes.Length][];

            for (int i = 0; i < sizes.Length; i++)
            {
                int s = sizes[i];
                Bitmap src = (s <= 32 && trayBmp != null) ? trayBmp : mainBmp;
                using (Bitmap resized = ResizeImage(src, s, s))
                {
                    if (s == 256)
                    {
                        // 256x256 使用 PNG 压缩以控制 ICO 体积
                        using (MemoryStream ms = new MemoryStream())
                        {
                            resized.Save(ms, ImageFormat.Png);
                            iconImages[i] = ms.ToArray();
                        }
                    }
                    else
                    {
                        // 16~128 使用标准 DIB (BITMAPINFOHEADER + BGRA + AND Mask)，保证与 csc.exe 及老旧 Win32 API 绝对兼容
                        iconImages[i] = CreateDibData(resized);
                    }
                }
            }

            using (FileStream fs = new FileStream(outputPath, FileMode.Create))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                // Header (ICONDIR)
                bw.Write((ushort)0); // reserved
                bw.Write((ushort)1); // type 1 = icon
                bw.Write((ushort)sizes.Length); // count

                int offset = 6 + (16 * sizes.Length);

                for (int i = 0; i < sizes.Length; i++)
                {
                    int s = sizes[i];
                    bw.Write((byte)(s >= 256 ? 0 : s)); // bWidth
                    bw.Write((byte)(s >= 256 ? 0 : s)); // bHeight
                    bw.Write((byte)0); // bColorCount
                    bw.Write((byte)0); // bReserved
                    bw.Write((ushort)1); // wPlanes
                    bw.Write((ushort)32); // wBitCount
                    bw.Write((uint)iconImages[i].Length); // dwBytesInRes
                    bw.Write((uint)offset); // dwImageOffset
                    offset += iconImages[i].Length;
                }

                for (int i = 0; i < sizes.Length; i++)
                {
                    bw.Write(iconImages[i]);
                }
            }
        }

        Console.WriteLine("Successfully created: " + outputPath);
    }

    private static Bitmap ResizeImage(Image image, int width, int height)
    {
        Bitmap dest = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        using (Graphics g = Graphics.FromImage(dest))
        {
            g.CompositingMode = CompositingMode.SourceCopy;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            g.DrawImage(image, new Rectangle(0, 0, width, height));
        }
        return dest;
    }

    private static byte[] CreateDibData(Bitmap bmp)
    {
        int width = bmp.Width;
        int height = bmp.Height;

        int andRowBytes = ((width + 31) / 32) * 4;
        int andMaskSize = andRowBytes * height;
        int xorSize = width * height * 4;
        int dibHeaderSize = 40;
        int totalSize = dibHeaderSize + xorSize + andMaskSize;

        byte[] dib = new byte[totalSize];
        using (MemoryStream ms = new MemoryStream(dib))
        using (BinaryWriter bw = new BinaryWriter(ms))
        {
            // BITMAPINFOHEADER
            bw.Write(40); // biSize
            bw.Write(width); // biWidth
            bw.Write(height * 2); // biHeight (XOR + AND)
            bw.Write((ushort)1); // biPlanes
            bw.Write((ushort)32); // biBitCount
            bw.Write(0); // biCompression (BI_RGB)
            bw.Write(xorSize + andMaskSize); // biSizeImage
            bw.Write(0); // biXPelsPerMeter
            bw.Write(0); // biYPelsPerMeter
            bw.Write(0); // biClrUsed
            bw.Write(0); // biClrImportant

            // XOR mask (BGRA, bottom-up)
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            try
            {
                byte[] row = new byte[width * 4];
                for (int y = height - 1; y >= 0; y--)
                {
                    IntPtr rowPtr = new IntPtr(data.Scan0.ToInt64() + y * data.Stride);
                    System.Runtime.InteropServices.Marshal.Copy(rowPtr, row, 0, row.Length);
                    bw.Write(row);
                }
            }
            finally
            {
                bmp.UnlockBits(data);
            }

            // AND mask (bottom-up, 1 bit per pixel: 0 = visible, 1 = transparent)
            // 32-bit RGBA already has alpha in XOR mask, so AND mask can be all zeros
            byte[] andMask = new byte[andMaskSize];
            bw.Write(andMask);
        }

        return dib;
    }
}
