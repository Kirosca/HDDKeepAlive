using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public class Build10StunningIcons
{
    private static string brainDir = @"C:\Users\Admin（无密码）\.gemini\antigravity\brain\94e48d27-bd6b-44c9-99d5-8f5642b9c151";

    public static void Main()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string outDir = Path.Combine(baseDir, "assets", "icons_top10");
        if (!Directory.Exists(outDir)) Directory.CreateDirectory(outDir);

        for (int i = 1; i <= 10; i++)
        {
            RenderVariant(i, outDir);
        }

        Console.WriteLine("All 10 refined stunning icons generated successfully!");
    }

    private static void RenderVariant(int id, string outDir)
    {
        string bgName = (id <= 5) ? "hdd_pulse_v2_1791812663658.jpg" :
                        (id <= 8) ? "hdd_pulse_v4_1791812717805.jpg" :
                                    "hdd_pulse_v1_1791812642213.jpg";

        string bgFile = Path.Combine(brainDir, bgName);
        if (!File.Exists(bgFile)) return;

        using (Bitmap bgBmp = new Bitmap(bgFile))
        using (Bitmap output = new Bitmap(1024, 1024, PixelFormat.Format32bppArgb))
        using (Graphics g = Graphics.FromImage(output))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // 1. 真实 3D 机械硬盘全景底座 (背景)
            g.DrawImage(bgBmp, 0, 0, 1024, 1024);

            // 2. 根据不同底座微调中心位置与大小 (更大、更挺拔饱满的红心)
            float cx = (id >= 9) ? 505f : (id >= 6) ? 515f : 512f;
            float cy = (id >= 9) ? 450f : (id >= 6) ? 460f : 465f;
            float hw = 330f;
            float hh = 310f;

            // 3. 盘面真实接触阴影 (Ambient Occlusion Contact Shadow)
            DrawContactShadow(g, cx, cy, hw, hh);

            // 4. 10 款不同艺术与光影风格的饱满红心
            switch (id)
            {
                case 1:
                    // 方案 1：经典 3D 鲜红活体宝石心 (多层光晕 + 冠状脉管 + 湿润弧光)
                    DrawAura(g, cx, cy, hw, hh, Color.FromArgb(255, 30, 60), 36);
                    DrawRealisticHeart(g, cx, cy, hw, hh, Color.FromArgb(255, 240, 25, 55), Color.FromArgb(255, 80, 0, 15));
                    DrawVentricles(g, cx, cy, hw, hh);
                    DrawCoronaryArteries(g, cx, cy, hw, hh, Color.FromArgb(255, 215, 60));
                    DrawSpecular(g, cx, cy, hw, hh);
                    break;

                case 2:
                    // 方案 2：赛博霓虹红心 (电光红肌体 + 荧光青蓝极光外晕 + 霓虹发光脉管)
                    DrawAura(g, cx, cy, hw, hh, Color.FromArgb(0, 230, 255), 45); // 青蓝冷晕
                    DrawRealisticHeart(g, cx, cy, hw, hh, Color.FromArgb(255, 255, 35, 75), Color.FromArgb(255, 95, 5, 20));
                    DrawVentricles(g, cx, cy, hw, hh);
                    DrawCoronaryArteries(g, cx, cy, hw, hh, Color.FromArgb(0, 255, 230)); // 赛博青绿发光脉管
                    DrawSpecular(g, cx, cy, hw, hh);
                    DrawRimGlow(g, cx, cy, hw, hh, Color.FromArgb(255, 120, 180), 5f);
                    break;

                case 3:
                    // 方案 3：红宝石水晶切面心脏 (透光晶面切线 + 内部折射光)
                    DrawAura(g, cx, cy, hw, hh, Color.FromArgb(255, 40, 80), 30);
                    DrawRealisticHeart(g, cx, cy, hw, hh, Color.FromArgb(255, 245, 20, 60), Color.FromArgb(255, 65, 0, 15));
                    DrawCrystalFacets(g, cx, cy, hw, hh);
                    DrawSpecular(g, cx, cy, hw, hh);
                    DrawRimGlow(g, cx, cy, hw, hh, Color.FromArgb(200, 255, 200, 220), 3f);
                    break;

                case 4:
                    // 方案 4：熔岩热核红心 (深暗暗红心体 + 炽烈橙金能量脉动裂纹)
                    DrawAura(g, cx, cy, hw, hh, Color.FromArgb(255, 90, 10), 40);
                    DrawRealisticHeart(g, cx, cy, hw, hh, Color.FromArgb(255, 225, 45, 10), Color.FromArgb(255, 55, 5, 5));
                    DrawLavaVeins(g, cx, cy, hw, hh);
                    DrawSpecular(g, cx, cy, hw, hh);
                    DrawRimGlow(g, cx, cy, hw, hh, Color.FromArgb(255, 200, 50), 4f);
                    break;

                case 5:
                    // 方案 5：极简高对比现代 Fluent 纯净红心 (高亮白边勾勒，极利于小尺寸与托盘识别)
                    DrawAura(g, cx, cy, hw, hh, Color.FromArgb(255, 30, 60), 25);
                    DrawRealisticHeart(g, cx, cy, hw, hh, Color.FromArgb(255, 255, 35, 65), Color.FromArgb(255, 130, 5, 25));
                    DrawSpecular(g, cx, cy, hw, hh);
                    DrawRimGlow(g, cx, cy, hw, hh, Color.FromArgb(255, 255, 255), 5.5f);
                    break;

                case 6:
                    // 方案 6：Fluent 喷砂铝合金底盘 + 纯粹血红 3D 宝石心
                    DrawAura(g, cx, cy, hw, hh, Color.FromArgb(255, 25, 55), 35);
                    DrawRealisticHeart(g, cx, cy, hw, hh, Color.FromArgb(255, 235, 20, 50), Color.FromArgb(255, 75, 0, 10));
                    DrawVentricles(g, cx, cy, hw, hh);
                    DrawCoronaryArteries(g, cx, cy, hw, hh, Color.FromArgb(255, 225, 120));
                    DrawSpecular(g, cx, cy, hw, hh);
                    break;

                case 7:
                    // 方案 7：Fluent 铝合金底盘 + 翡翠绿生命光圈红心 (绿意生机保活)
                    DrawAura(g, cx, cy, hw, hh, Color.FromArgb(0, 255, 140), 40);
                    DrawRealisticHeart(g, cx, cy, hw, hh, Color.FromArgb(255, 245, 25, 60), Color.FromArgb(255, 85, 5, 15));
                    DrawVentricles(g, cx, cy, hw, hh);
                    DrawCoronaryArteries(g, cx, cy, hw, hh, Color.FromArgb(0, 255, 150));
                    DrawSpecular(g, cx, cy, hw, hh);
                    DrawRimGlow(g, cx, cy, hw, hh, Color.FromArgb(120, 0, 255, 150), 4f);
                    break;

                case 8:
                    // 方案 8：Fluent 铝合金底盘 + 黄金脉动能量环红心
                    DrawGoldenRings(g, cx, cy, hw * 1.08f);
                    DrawRealisticHeart(g, cx, cy, hw, hh, Color.FromArgb(255, 240, 30, 65), Color.FromArgb(255, 95, 5, 20));
                    DrawVentricles(g, cx, cy, hw, hh);
                    DrawCoronaryArteries(g, cx, cy, hw, hh, Color.FromArgb(255, 215, 0));
                    DrawSpecular(g, cx, cy, hw, hh);
                    break;

                case 9:
                    // 方案 9：3D 透视盘体 + 倾斜自然俯视立体红心
                    DrawAura(g, cx, cy, hw * 0.96f, hh * 0.96f, Color.FromArgb(255, 30, 60), 35);
                    DrawRealisticHeart(g, cx, cy, hw * 0.96f, hh * 0.96f, Color.FromArgb(255, 240, 25, 55), Color.FromArgb(255, 75, 0, 15));
                    DrawVentricles(g, cx, cy, hw * 0.96f, hh * 0.96f);
                    DrawCoronaryArteries(g, cx, cy, hw * 0.96f, hh * 0.96f, Color.FromArgb(255, 200, 90));
                    DrawSpecular(g, cx, cy, hw * 0.96f, hh * 0.96f);
                    break;

                case 10:
                    // 方案 10：3D 透视盘体 + 赛博超能光晕红心
                    DrawAura(g, cx, cy, hw * 0.96f, hh * 0.96f, Color.FromArgb(255, 50, 90), 45);
                    DrawRealisticHeart(g, cx, cy, hw * 0.96f, hh * 0.96f, Color.FromArgb(255, 255, 35, 70), Color.FromArgb(255, 85, 5, 20));
                    DrawVentricles(g, cx, cy, hw * 0.96f, hh * 0.96f);
                    DrawCoronaryArteries(g, cx, cy, hw * 0.96f, hh * 0.96f, Color.FromArgb(0, 240, 255));
                    DrawSpecular(g, cx, cy, hw * 0.96f, hh * 0.96f);
                    DrawRimGlow(g, cx, cy, hw * 0.96f, hh * 0.96f, Color.FromArgb(255, 100, 140), 4.5f);
                    break;
            }

            // 保存高精图
            string jpgPath = Path.Combine(outDir, string.Format("hdd_heart_top_{0}.jpg", id));
            string pngPath = Path.Combine(outDir, string.Format("hdd_heart_top_{0}.png", id));
            output.Save(jpgPath, ImageFormat.Jpeg);
            output.Save(pngPath, ImageFormat.Png);

            // 编译为 Windows 多尺寸 ICO (256, 128, 64, 48, 32, 16)
            string icoPath = Path.Combine(outDir, string.Format("hdd_heart_top_{0}.ico", id));
            BuildIco(output, icoPath);
        }
    }

    private static GraphicsPath CreateHeartPath(float cx, float cy, float w, float h)
    {
        GraphicsPath path = new GraphicsPath();
        float topY = cy - h * 0.46f;
        float botY = cy + h * 0.54f;
        float leftX = cx - w;
        float rightX = cx + w;

        // 更加饱满、自然的双心弧曲线
        path.AddBezier(
            new PointF(cx, topY + h * 0.28f),
            new PointF(cx - w * 0.52f, topY - h * 0.18f),
            new PointF(leftX, topY + h * 0.22f),
            new PointF(cx, botY)
        );
        path.AddBezier(
            new PointF(cx, botY),
            new PointF(rightX, topY + h * 0.22f),
            new PointF(cx + w * 0.52f, topY - h * 0.18f),
            new PointF(cx, topY + h * 0.28f)
        );
        path.CloseFigure();
        return path;
    }

    private static void DrawContactShadow(Graphics g, float cx, float cy, float w, float h)
    {
        using (GraphicsPath sp = new GraphicsPath())
        {
            sp.AddEllipse(cx - w * 1.1f, cy + h * 0.42f, w * 2.2f, h * 0.70f);
            using (PathGradientBrush pgb = new PathGradientBrush(sp))
            {
                pgb.CenterColor = Color.FromArgb(230, 0, 0, 0);
                pgb.SurroundColors = new Color[] { Color.FromArgb(0, 0, 0, 0) };
                g.FillPath(pgb, sp);
            }
        }
    }

    private static void DrawAura(Graphics g, float cx, float cy, float w, float h, Color c, int maxR)
    {
        for (int r = maxR; r >= 10; r -= 8)
        {
            using (GraphicsPath ap = CreateHeartPath(cx, cy, w + r, h + r))
            using (Pen pen = new Pen(Color.FromArgb(14, c), r * 1.5f))
            {
                g.DrawPath(pen, ap);
            }
        }
    }

    private static void DrawRealisticHeart(Graphics g, float cx, float cy, float w, float h, Color highlight, Color shadow)
    {
        using (GraphicsPath hp = CreateHeartPath(cx, cy, w, h))
        using (PathGradientBrush brush = new PathGradientBrush(hp))
        {
            brush.CenterPoint = new PointF(cx - w * 0.28f, cy - h * 0.12f);
            brush.CenterColor = highlight;
            brush.SurroundColors = new Color[] { shadow };
            g.FillPath(brush, hp);
        }
    }

    private static void DrawVentricles(Graphics g, float cx, float cy, float w, float h)
    {
        using (GraphicsPath left = new GraphicsPath())
        {
            left.AddEllipse(cx - w * 0.78f, cy - h * 0.22f, w * 0.88f, h * 0.68f);
            using (PathGradientBrush pgb = new PathGradientBrush(left))
            {
                pgb.CenterPoint = new PointF(cx - w * 0.45f, cy - h * 0.05f);
                pgb.CenterColor = Color.FromArgb(130, 255, 60, 90);
                pgb.SurroundColors = new Color[] { Color.FromArgb(0, 120, 0, 20) };
                g.FillPath(pgb, left);
            }
        }

        using (GraphicsPath right = new GraphicsPath())
        {
            right.AddEllipse(cx - w * 0.1f, cy - h * 0.18f, w * 0.78f, h * 0.64f);
            using (PathGradientBrush pgb = new PathGradientBrush(right))
            {
                pgb.CenterPoint = new PointF(cx + w * 0.32f, cy);
                pgb.CenterColor = Color.FromArgb(110, 255, 50, 80);
                pgb.SurroundColors = new Color[] { Color.FromArgb(0, 100, 0, 15) };
                g.FillPath(pgb, right);
            }
        }

        using (GraphicsPath groove = new GraphicsPath())
        {
            groove.AddBezier(
                new PointF(cx, cy - h * 0.05f),
                new PointF(cx - w * 0.10f, cy + h * 0.15f),
                new PointF(cx - w * 0.06f, cy + h * 0.35f),
                new PointF(cx, cy + h * 0.54f)
            );
            using (Pen gp = new Pen(Color.FromArgb(140, 45, 0, 10), 18f))
            {
                gp.StartCap = LineCap.Round;
                gp.EndCap = LineCap.Round;
                g.DrawPath(gp, groove);
            }
        }
    }

    private static void DrawCoronaryArteries(Graphics g, float cx, float cy, float w, float h, Color c)
    {
        PointF[][] vessels = new PointF[][]
        {
            new PointF[] {
                new PointF(cx, cy - h * 0.05f),
                new PointF(cx - w * 0.15f, cy + h * 0.15f),
                new PointF(cx - w * 0.10f, cy + h * 0.32f),
                new PointF(cx - w * 0.02f, cy + h * 0.50f)
            },
            new PointF[] {
                new PointF(cx - w * 0.15f, cy + h * 0.15f),
                new PointF(cx - w * 0.40f, cy + h * 0.22f),
                new PointF(cx - w * 0.60f, cy + h * 0.26f)
            },
            new PointF[] {
                new PointF(cx - w * 0.10f, cy + h * 0.32f),
                new PointF(cx - w * 0.30f, cy + h * 0.38f)
            },
            new PointF[] {
                new PointF(cx + w * 0.12f, cy - h * 0.02f),
                new PointF(cx + w * 0.35f, cy + h * 0.15f),
                new PointF(cx + w * 0.52f, cy + h * 0.28f)
            },
            new PointF[] {
                new PointF(cx + w * 0.35f, cy + h * 0.15f),
                new PointF(cx + w * 0.24f, cy + h * 0.34f)
            }
        };

        foreach (var v in vessels)
        {
            using (Pen vpGlow = new Pen(Color.FromArgb(85, c), 8f))
            using (Pen vpCore = new Pen(Color.FromArgb(240, 255, 255, 255), 3f))
            {
                vpGlow.StartCap = LineCap.Round;
                vpGlow.EndCap = LineCap.Round;
                vpCore.StartCap = LineCap.Round;
                vpCore.EndCap = LineCap.Round;
                if (v.Length == 4)
                {
                    g.DrawBezier(vpGlow, v[0], v[1], v[2], v[3]);
                    g.DrawBezier(vpCore, v[0], v[1], v[2], v[3]);
                }
                else
                {
                    g.DrawLines(vpGlow, v);
                    g.DrawLines(vpCore, v);
                }
            }
        }
    }

    private static void DrawSpecular(Graphics g, float cx, float cy, float w, float h)
    {
        using (GraphicsPath glint = new GraphicsPath())
        {
            glint.AddBezier(
                new PointF(cx - w * 0.65f, cy - h * 0.10f),
                new PointF(cx - w * 0.50f, cy - h * 0.32f),
                new PointF(cx - w * 0.22f, cy - h * 0.28f),
                new PointF(cx - w * 0.42f, cy - h * 0.08f)
            );
            using (SolidBrush glintBrush = new SolidBrush(Color.FromArgb(190, 255, 255, 255)))
            {
                g.FillPath(glintBrush, glint);
            }
        }
        using (SolidBrush dot = new SolidBrush(Color.FromArgb(160, 255, 240, 245)))
        {
            g.FillEllipse(dot, cx + w * 0.28f, cy - h * 0.08f, 26f, 18f);
        }
    }

    private static void DrawRimGlow(Graphics g, float cx, float cy, float w, float h, Color c, float width)
    {
        using (GraphicsPath hp = CreateHeartPath(cx, cy, w, h))
        using (Pen p = new Pen(c, width))
        {
            g.DrawPath(p, hp);
        }
    }

    private static void DrawCrystalFacets(Graphics g, float cx, float cy, float w, float h)
    {
        using (Pen edge = new Pen(Color.FromArgb(170, 255, 255, 255), 2.5f))
        {
            PointF top = new PointF(cx, cy - h * 0.20f);
            PointF bot = new PointF(cx, cy + h * 0.54f);
            PointF left = new PointF(cx - w * 0.80f, cy + h * 0.08f);
            PointF right = new PointF(cx + w * 0.80f, cy + h * 0.08f);
            PointF mid = new PointF(cx, cy + h * 0.14f);

            g.DrawLine(edge, top, mid);
            g.DrawLine(edge, mid, bot);
            g.DrawLine(edge, top, left);
            g.DrawLine(edge, top, right);
            g.DrawLine(edge, mid, left);
            g.DrawLine(edge, mid, right);
            g.DrawLine(edge, left, bot);
            g.DrawLine(edge, right, bot);
        }
    }

    private static void DrawLavaVeins(Graphics g, float cx, float cy, float w, float h)
    {
        using (Pen lava = new Pen(Color.FromArgb(255, 255, 120, 0), 4.5f))
        {
            lava.StartCap = LineCap.Round;
            lava.EndCap = LineCap.Round;
            g.DrawLine(lava, cx - w * 0.25f, cy - h * 0.05f, cx + w * 0.12f, cy + h * 0.18f);
            g.DrawLine(lava, cx + w * 0.12f, cy + h * 0.18f, cx - w * 0.15f, cy + h * 0.38f);
            g.DrawLine(lava, cx - w * 0.45f, cy + h * 0.08f, cx - w * 0.25f, cy - h * 0.05f);
        }
    }

    private static void DrawGoldenRings(Graphics g, float cx, float cy, float r)
    {
        using (Pen gp = new Pen(Color.FromArgb(170, 255, 215, 0), 4f))
        {
            g.DrawEllipse(gp, cx - r, cy - r + 25, r * 2, r * 2);
        }
    }

    private static void BuildIco(Bitmap src, string outIco)
    {
        int[] sizes = new int[] { 256, 128, 64, 48, 32, 16 };
        byte[][] iconImages = new byte[sizes.Length][];

        for (int i = 0; i < sizes.Length; i++)
        {
            int s = sizes[i];
            using (Bitmap resized = new Bitmap(s, s, PixelFormat.Format32bppArgb))
            using (Graphics g = Graphics.FromImage(resized))
            {
                g.CompositingMode = CompositingMode.SourceCopy;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(src, new Rectangle(0, 0, s, s));

                if (s == 256)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        resized.Save(ms, ImageFormat.Png);
                        iconImages[i] = ms.ToArray();
                    }
                }
                else
                {
                    iconImages[i] = CreateDibData(resized);
                }
            }
        }

        using (FileStream fs = new FileStream(outIco, FileMode.Create))
        using (BinaryWriter bw = new BinaryWriter(fs))
        {
            bw.Write((ushort)0);
            bw.Write((ushort)1);
            bw.Write((ushort)sizes.Length);

            int offset = 6 + (16 * sizes.Length);
            for (int i = 0; i < sizes.Length; i++)
            {
                int s = sizes[i];
                bw.Write((byte)(s >= 256 ? 0 : s));
                bw.Write((byte)(s >= 256 ? 0 : s));
                bw.Write((byte)0);
                bw.Write((byte)0);
                bw.Write((ushort)1);
                bw.Write((ushort)32);
                bw.Write((uint)iconImages[i].Length);
                bw.Write((uint)offset);
                offset += iconImages[i].Length;
            }

            for (int i = 0; i < sizes.Length; i++)
            {
                bw.Write(iconImages[i]);
            }
        }
    }

    private static byte[] CreateDibData(Bitmap bmp)
    {
        int width = bmp.Width;
        int height = bmp.Height;
        int andMaskSize = ((width + 31) / 32) * 4 * height;
        int xorSize = width * height * 4;
        int dibHeaderSize = 40;
        int totalSize = dibHeaderSize + xorSize + andMaskSize;

        byte[] dib = new byte[totalSize];
        using (MemoryStream ms = new MemoryStream(dib))
        using (BinaryWriter bw = new BinaryWriter(ms))
        {
            bw.Write(40);
            bw.Write(width);
            bw.Write(height * 2);
            bw.Write((ushort)1);
            bw.Write((ushort)32);
            bw.Write(0);
            bw.Write(xorSize + andMaskSize);
            bw.Write(0);
            bw.Write(0);
            bw.Write(0);
            bw.Write(0);

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

            byte[] andMask = new byte[andMaskSize];
            bw.Write(andMask);
        }

        return dib;
    }
}
