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
    public class MethodHomografyService : IMethodService
    {
        private string input_path { get; }

        private string secret_message { get; }

        private string key_path { get; }

        private string script_path { get; }

        private string output_path { get; }

        public MethodHomografyService(string? input_path = "", string? secret_message = "", string? key_path = "", string? output_path = "")
        {
            script_path = System.AppDomain.CurrentDomain.BaseDirectory + "Scripts\\paragraphs.py";
            this.input_path = input_path;
            this.secret_message = secret_message;
            this.key_path = key_path;
            this.output_path = output_path;
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
            var command = $"\"{script_path}\" extract \"{input_path}\" \"{key_path}\"";
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
                    result = result.Replace("\0", "").Replace(Environment.NewLine, "");
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
            var command = $"\"{script_path}\" embed \"{input_path}\" \"{secret_message}\" \"{output_path}\"";
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
