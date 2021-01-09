﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace AltoMultiThreadDownloadManager.Helpers
{
    /// <summary>
    /// Provides method to get the corrent filename that server sent
    /// </summary>
    public static class FileNameHelper
    {
        /// <summary>
        /// Gets the filename from response
        /// </summary>
        /// <param name="resp">Response received from server</param>
        /// <returns></returns>
        public static string GetFileName(HttpWebResponse resp)
        {
            var cdHeader = resp.Headers["Content-Disposition"];
            var location = resp.Headers["Location"];
            if (!string.IsNullOrEmpty(cdHeader))
            {
                var pattern = string.Format("filename[^;=\n]*=((['\"]).*?{0}|[^;\n]*)", Regex.Escape("2"));
                var omitPattern = "filename[^;\n=]*=(([^'\"])*'')?";
                var properFormat = Encoding.UTF8.GetString(Encoding.GetEncoding("ISO-8859-1").GetBytes(cdHeader));
                properFormat = HttpUtility.UrlDecode(properFormat);
                properFormat = properFormat.Replace("\"", "");
                var matches = Regex.Matches(properFormat, pattern);
                if (matches.Count > 0)
                {
                    var filename = matches.Cast<Match>().Last().Value;
                    filename = Regex.Replace(filename, omitPattern, "");
                    return filename.ReplaceInvalidChars();
                }
            }
            if (!string.IsNullOrEmpty(location))
            {
                var locUri = new Uri(location);
                var first = Path.GetFileName(locUri.LocalPath);
                if (first.IsCorrectFilename())
                {
                    return HttpUtility.UrlDecode(first);
                }
                else
                {
                    first = Path.GetFileName(locUri.AbsolutePath);
                    return HttpUtility.UrlDecode(first.ReplaceInvalidChars());
                }
            }
            else
            {
                var first = Path.GetFileName(resp.ResponseUri.LocalPath);
                if (first.IsCorrectFilename())
                {
                    return HttpUtility.UrlDecode(first);
                }
                else
                {
                    first = Path.GetFileName(resp.ResponseUri.AbsolutePath);
                    if (first.IsCorrectFilename())
                        return HttpUtility.UrlDecode(first.ReplaceInvalidChars());
                }

				return "index.html";
            }
        }

        private static bool HasExtension(this string filename)
        {
            return !string.IsNullOrEmpty(Path.GetExtension(filename));
        }

        private static bool HasInvalidChar(this string filename)
        {
            return Path.GetInvalidFileNameChars().Any(filename.Contains);
        }

        private static string ReplaceInvalidChars(this string filename)
        {
            return Path.GetInvalidFileNameChars().Aggregate(filename, (current, c) => current.Replace(c.ToString(), ""));
        }

        private static bool IsCorrectFilename(this string filename)
        {
            return filename.HasExtension() && !filename.HasInvalidChar();
        }

    }

}
