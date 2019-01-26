using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Jontacos
{
    public class BitmapLoader
    {
        public struct BitmapFileHeader
        {
            public ushort fType;
            public uint fSize;
            public ushort fReserved1;
            public ushort fReserved2;
            public uint fOffset;
        }
        public struct BitmapInfoHeader
        {
            public uint biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
        }

        public static Texture2D Load(string path)
        {
            BitmapFileHeader bfh;
            BitmapInfoHeader bih;
            Color[] colorPal;
            byte[] bitData;
            Load(path, out bfh, out bih, out colorPal, out bitData);
            int stride = ((bih.biWidth * bih.biBitCount + 31) / 32) * 4;
            int ch = bih.biBitCount / 8;
            var tex = new Texture2D(bih.biWidth, bih.biHeight, TextureFormat.RGBA32, false);
            tex.LoadRawTextureData(bitData);
            tex.Apply();
            return tex;
        }

        private static bool Load(string path, out BitmapFileHeader bfh, out BitmapInfoHeader bih, out Color[] colorPal, out byte[] bitData)
        {
            var ext = Path.GetExtension(path).ToLower();
            if (ext != ".bmp")
                goto ErrorHandler;

            var readData = new byte[4];
            FileStream fs;
            try
            {
                fs = File.Open(path, FileMode.Open, FileAccess.Read);
                if (fs == null)
                    goto ErrorHandler;
            }
            catch
            {
                goto ErrorHandler;
            }

            fs.Read(readData, 0, 2);
            bfh.fType = BitConverter.ToUInt16(readData, 0);
            fs.Read(readData, 0, 4);
            bfh.fSize = BitConverter.ToUInt32(readData, 0);
            fs.Read(readData, 0, 2);
            bfh.fReserved1 = BitConverter.ToUInt16(readData, 0);
            fs.Read(readData, 0, 2);
            bfh.fReserved2 = BitConverter.ToUInt16(readData, 0);
            fs.Read(readData, 0, 4);
            bfh.fOffset = BitConverter.ToUInt32(readData, 0);


            fs.Read(readData, 0, 4);
            bih.biSize = BitConverter.ToUInt32(readData, 0);
            fs.Read(readData, 0, 4);
            bih.biWidth = BitConverter.ToInt32(readData, 0);
            fs.Read(readData, 0, 4);
            bih.biHeight = BitConverter.ToInt32(readData, 0);
            fs.Read(readData, 0, 2);
            bih.biPlanes = BitConverter.ToUInt16(readData, 0);
            fs.Read(readData, 0, 2);
            bih.biBitCount = BitConverter.ToUInt16(readData, 0);
            fs.Read(readData, 0, 4);
            bih.biCompression = BitConverter.ToUInt32(readData, 0);
            fs.Read(readData, 0, 4);
            bih.biSizeImage = BitConverter.ToUInt32(readData, 0);
            fs.Read(readData, 0, 4);
            bih.biXPelsPerMeter = BitConverter.ToInt32(readData, 0);
            fs.Read(readData, 0, 4);
            bih.biYPelsPerMeter = BitConverter.ToInt32(readData, 0);
            fs.Read(readData, 0, 4);
            bih.biClrUsed = BitConverter.ToUInt32(readData, 0);
            fs.Read(readData, 0, 4);
            bih.biClrImportant = BitConverter.ToUInt32(readData, 0);

            long palSize = (bfh.fOffset - 14 - 40) / 4;
            if(palSize != 0)
            {
                colorPal = new Color[palSize];
                for (int i = 0; i < palSize; ++i)
                {
                    fs.Read(readData, 0, 4);
                    colorPal[i] = new Color(readData[2], readData[1], readData[0], readData[3]);
                }
            }
            else
            {
                colorPal = null;
            }

            int stride = ((bih.biWidth * bih.biBitCount + 31) / 32) * 4;
            bitData = new byte[stride * bih.biHeight];

            for (int i = 0; i < bih.biHeight; ++i)
            {
                fs.Read(bitData, i * stride, stride);
            }

            fs.Close();
            fs.Dispose();

            return true;

            ErrorHandler:
                Debug.LogError("-----BitmapLoaderError-----");
                bfh.fType = 0;
                bfh.fSize = 0;
                bfh.fReserved1 = 0;
                bfh.fReserved2 = 0;
                bfh.fOffset = 0;

                bih.biSize = 0;
                bih.biWidth = 0;
                bih.biHeight = 0;
                bih.biPlanes = 0;
                bih.biBitCount = 0;
                bih.biCompression = 0;
                bih.biSizeImage = 0;
                bih.biXPelsPerMeter = 0;
                bih.biYPelsPerMeter = 0;
                bih.biClrUsed = 0;
                bih.biClrImportant = 0;

                colorPal = null;
                bitData = null;
                return false;
        }

    }
}
