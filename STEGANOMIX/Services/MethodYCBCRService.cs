using STEGANOMIX.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Printing.IndexedProperties;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace STEGANOMIX.Services
{
    public class MethodYCBCRService : IMethodService
    {
        private string image_path { get; }

        private bool _file { get; }

        private string secret_data { get; }

        private string output_path { get; }

        private string stego_image_path { get; }

        private string script_path { get; }


        public MethodYCBCRService(string? image_path = "", string? secret_data = "", string? output_path = "", string? stego_image_path = "", bool? _file = false)
        {
            script_path = string.Format($@"{System.AppDomain.CurrentDomain.BaseDirectory}Scripts\rgb.py").Replace("\\","/");
            this.image_path = image_path;
            this.secret_data = secret_data;
            this.output_path = output_path;
            this.stego_image_path = stego_image_path;
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
            var command = $"\"{script_path}\" extract \"{stego_image_path}\"";
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
            var command = $"\"{script_path}\" embed \"{image_path}\" \"{secret_data}\" \"{output_path.Replace("\\","/")}\"";
            command += _file ? " --file" : "";
            start.Arguments = string.Format("{0}", command);
            //start.Verb = "runas";
            start.CreateNoWindow = false;
            start.UseShellExecute = false;
            start.RedirectStandardError = true;
            start.RedirectStandardOutput = true;
            using(Process process = Process.Start(start))
            {
                using(StreamReader reader = process.StandardOutput)
                {
                    result = reader.ReadToEnd();
                }
                if(string.IsNullOrEmpty(result))
                {
                    using(StreamReader reader = process.StandardError)
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }

            return result;
        }
    }
}
