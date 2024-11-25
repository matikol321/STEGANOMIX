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
    public class MethodPolishService : IMethodService
    {
        private string input_path { get; }

        private string secret_message { get; }

        private string script_path { get; }

        public MethodPolishService(string? input_path = null, string? secret_message = null)
        {
            script_path = System.AppDomain.CurrentDomain.BaseDirectory + "Scripts\\polish_characters.py";
            this.input_path = input_path;
            this.secret_message = secret_message;
        }

        public string DecodeToString()
        {
            if (string.IsNullOrEmpty(SettingsViewModel.PythonURL))
                return string.Empty;

            var result = string.Empty;
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
            if (string.IsNullOrEmpty(SettingsViewModel.PythonURL))
                return string.Empty;

            var result = string.Empty;

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = SettingsViewModel.PythonURL;
            start.Arguments = string.Format("'{0}' embed '{1}' '{2}'", script_path, input_path, secret_message);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    result = reader.ReadToEnd();
                }
            }

            return result;
        }
    }
}
