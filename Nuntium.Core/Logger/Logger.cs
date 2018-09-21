using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Nuntium.Core
{
    /// <summary>
    /// Basic logger
    /// </summary>
    public static class Logger
    {
        #region Private memebers

        /// <summary>
        /// Path leading to log direvtory
        /// </summary>
        private static string mPathToLogDirectory = "./Log/";

        /// <summary>
        /// Name of the log file
        /// </summary>
        private static string mFileName = "log.txt";

        /// <summary>
        /// Date format used when logging messages
        /// </summary>
        private static string mDateFormat = "dd.MM.yyyy H:mm:ss";

        /// <summary>
        /// Full path directly to log file
        /// </summary>
        private static string mFullPath = mPathToLogDirectory + mFileName;

        private static ReaderWriterLock locker = new ReaderWriterLock();

        /// <summary>
        /// Time limit for logging to file 300000 = 5min
        /// </summary>
        private static int mTimeOut = 300000;
        #endregion

        /// <summary>
        /// Deletes log file if one exist
        /// </summary>
        public static void DeleteLogFile()
        {
            if (File.Exists(mFullPath))
            {
                File.Delete(mFullPath);
                Log(LogStatus.DEBUG, "Deleting log file");
            }
        }

        /// <summary>
        /// Writes message to log file
        /// </summary>
        /// <param name="status">Log status</param>
        /// <param name="message">Message to log</param>
        public static void Log(LogStatus status, string message = null, Exception exception = null, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int linenumber = 0)
        {
            var date = DateTime.Now.ToString(mDateFormat);
            var textToLog = "";

            switch (status)
            {
                default:
                    textToLog = $"[{status}] ({date}) [{linenumber}] {filePath} {message}";
                    break;

                case LogStatus.ERROR:
                    textToLog = $"[{status}] ({date}) [{linenumber}] Caller's name: {callerName} {exception?.ToString()} ";
                    break;

            }

            textToLog += Environment.NewLine;

            WriteToFile(textToLog);
        }

        /// <summary>
        /// Writes message to log file
        /// </summary>
        /// <param name="status">Log status</param>
        /// <param name="message">Message to log</param>
        /// <param name="obj">object to log as json</param>
        public static void Log(LogStatus status, object obj, string message = null, Exception exception = null, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int linenumber = 0)
        {
            var date = DateTime.Now.ToString(mDateFormat);
            var serializedObject = JsonConvert.SerializeObject(obj);
            var textToLog = "";

            switch (status)
            {
                default:
                    textToLog = $"[{status}] ({date}) [{linenumber}] {filePath} {message} {serializedObject}";
                    break;

                case LogStatus.ERROR:
                    textToLog = $"[{status}] ({date}) [{linenumber}] Caller's name: {callerName} {message} {exception?.ToString()} {serializedObject}";
                    break;

            }

            textToLog += Environment.NewLine;

            WriteToFile(textToLog);
        }

        /// <summary>
        /// Writes message to log file
        /// </summary>
        /// <param name="status">Log status</param>
        /// <param name="message">Message to log</param>
        /// <param name="objArray">array of object to log as json</param>
        public static void Log(LogStatus status, object[] objArray, string message = null, Exception exception = null, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int linenumber = 0)
        {
            var date = DateTime.Now.ToString(mDateFormat);

            var serializedObjectsList = new List<string>();

            for (int i = 0; i < objArray.Length; i++)
            {
                serializedObjectsList.Add(JsonConvert.SerializeObject(objArray[i]));
            }

            var textToLog = "";

            switch (status)
            {
                default:
                    textToLog = $"[{status}] ({date}) [{linenumber}] {filePath} {message} {Environment.NewLine}";
                    serializedObjectsList.ForEach(x => textToLog += x + Environment.NewLine);
                    break;

                case LogStatus.ERROR:
                    textToLog = $"[{status}] ({date}) [{linenumber}] Caller's name: {callerName} {message} {exception?.ToString()} {Environment.NewLine} ";
                    serializedObjectsList.ForEach(x => textToLog += x + Environment.NewLine);
                    break;

            }


            textToLog += Environment.NewLine;

            WriteToFile(textToLog);
        }

        /// <summary>
        /// Writes text to log file in a thread save manner
        /// </summary>
        /// <param name="textToLog">Text that will be append to log file</param>
        private static void WriteToFile(string textToLog)
        {
            try
            {
                locker.AcquireWriterLock(mTimeOut);
                File.AppendAllText(mFullPath, textToLog);
            }
            catch(Exception ex)
            {
                Log(LogStatus.ERROR, "Error while writing log to file", ex);
            }
            finally
            {
                locker.ReleaseWriterLock();
            }
        }
    }
}
