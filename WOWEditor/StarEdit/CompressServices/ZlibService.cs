using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace StarEdit.CompressServices
{
    class ZlibService
    {
        public static void DecompressFileToStream(string inFile, Stream outStream)
        {
            zlib.ZOutputStream outZStream = new zlib.ZOutputStream(outStream);
            System.IO.FileStream inFileStream = new System.IO.FileStream(inFile, System.IO.FileMode.Open);
            try
            {
                CopyStream(inFileStream, outZStream);
            }
            finally
            {
                outZStream.Close();
                inFileStream.Close();
            }
        }
        public static void DecompressStreamToStream(Stream inStream, Stream outStream)
        {
            zlib.ZOutputStream outZStream = new zlib.ZOutputStream(outStream);
            try
            {
                CopyStream(inStream, outZStream);
            }
            finally
            {
                outZStream.Close();
            }
        }
        public static void CompressFileToStream(string inFile, Stream outStream)
        {
            zlib.ZOutputStream outZStream = new zlib.ZOutputStream(outStream, zlib.zlibConst.Z_DEFAULT_COMPRESSION);
            System.IO.FileStream inFileStream = new System.IO.FileStream(inFile, System.IO.FileMode.Open);
            try
            {
                CopyStream(inFileStream, outZStream);
            }
            finally
            {
                outZStream.Close();
                inFileStream.Close();
            }
        }

        public static void CompressStreamToFile(Stream inStream, string outFile)
        {
            System.IO.FileStream outFileStream = new System.IO.FileStream(outFile, System.IO.FileMode.OpenOrCreate);
            zlib.ZOutputStream outZStream = new zlib.ZOutputStream(outFileStream, zlib.zlibConst.Z_BEST_COMPRESSION);
            try
            {
                CopyStream(inStream, outZStream);
            }
            finally
            {
                inStream.Close();
                outZStream.Close();
            }
        }

        private static void CopyStream(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] buffer = new byte[1000];
            int len;
            while ((len = input.Read(buffer, 0, 1000)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Flush();
        }

    }
}
