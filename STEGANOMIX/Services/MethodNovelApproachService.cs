﻿using STEGANOMIX.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEGANOMIX.Services
{
    public class MethodNovelApproachService : IMethodService
    {
        private string script_path { get; }

        public MethodNovelApproachService()
        {
            script_path = System.AppDomain.CurrentDomain.BaseDirectory + "Scripts\\date.py";
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
            return result;
        }
    }
}
