// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.IO;
using System.Text;
using WcfSoapLogger.Exceptions;

namespace WcfSoapLogger.FileWriting
{
    internal static class FileSystem
    {
        internal static void WriteAllText(string filePath, string contents)
        {
            try
            {
                File.WriteAllText(filePath, contents, Encoding.UTF8);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new FileSystemAcccesDeniedException(ex);
            }
            catch (DirectoryNotFoundException)
            {
                throw;
            }
        }


        internal static void WriteAllBytes(string filePath, byte[] bytes)
        {
            try
            {
                File.WriteAllBytes(filePath, bytes);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new FileSystemAcccesDeniedException(ex);
            }
            catch (DirectoryNotFoundException)
            {
                throw;
            }
        }


        internal static void CreateDirectory(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new FileSystemAcccesDeniedException(ex);
            }
        }

    }
}
