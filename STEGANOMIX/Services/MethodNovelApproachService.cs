using STEGANOMIX.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEGANOMIX.Services
{
    public class MethodNovelApproachService : IMethodService
    {
        private string input_path { get; }

        private bool _file { get; }

        private string message { get; }

        private string output_file { get; }

        private string script_path { get; }

        public MethodNovelApproachService(string? input_path = "", string? message = "", string? output_file = "", bool? _file = false)
        {
            script_path = string.Format($@"{System.AppDomain.CurrentDomain.BaseDirectory}Scripts\date.py").Replace("\\", "/");
            this.message = message;
            this.input_path = input_path;
            this.output_file = output_file;
            this._file = _file.HasValue ? _file.Value : false;
        }

        public string DecodeToString()
        {
            string python = string.Empty;
            if (!string.IsNullOrEmpty(SettingsViewModel.PythonURL))
                python = SettingsViewModel.PythonURL;
            if (string.IsNullOrEmpty(python))
                python = "python";

            var result = string.Empty;

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = python;
            var command = $"\"{script_path}\" extract ";
            command += _file ? "--file " : "";
            command += $"\"{input_path}\"";
            start.Arguments = string.Format("{0}", command);
            //start.Verb = "runas";
            start.CreateNoWindow = false;
            start.UseShellExecute = false;
            start.RedirectStandardError = true;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    result = reader.ReadToEnd();
                }
                if (string.IsNullOrEmpty(result))
                {
                    using (StreamReader reader = process.StandardError)
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }

            return result;
        }

        public byte[] DecoteToByte()
        {
            byte[] result = null;
            return result;
        }

        public byte[] EncodeToByte()
        {
            byte[] result = null;
            return result;
        }

        public string EncodeToString()
        {
            string python = string.Empty;
            if (!string.IsNullOrEmpty(SettingsViewModel.PythonURL))
                python = SettingsViewModel.PythonURL;
            if (string.IsNullOrEmpty(python))
                python = "python";

            var result = string.Empty;

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = python;
            var command = $"\"{script_path}\" hide ";
            command += _file ? "--template " : "";
            command += $"\"{input_path.Replace("\\","/")}\" \"{message}\" \"{output_file.Replace("\\","/")}\"";
            start.Arguments = string.Format("{0}", command);
            //start.Verb = "runas";
            start.CreateNoWindow = false;
            start.UseShellExecute = false;
            start.RedirectStandardError = true;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    result = reader.ReadToEnd();
                }
                if (string.IsNullOrEmpty(result))
                {
                    using (StreamReader reader = process.StandardError)
                    {
                        result = reader.ReadToEnd();
                    }
                    result = "error";
                }
            }

            return result;
        }
    }
}
