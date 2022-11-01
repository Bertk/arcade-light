// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Text;
using System.Diagnostics;

namespace DotNet.ArcadeLight.Common
{
    public struct CommandResult
    {
        public static readonly CommandResult Empty;

        public ProcessStartInfo StartInfo { get; }
        public int ExitCode { get; }
        public string StdOut { get; }
        public string StdErr { get; }

        public CommandResult(ProcessStartInfo startInfo, int exitCode, string stdOut, string stdErr)
        {
            StartInfo = startInfo;
            ExitCode = exitCode;
            StdOut = stdOut;
            StdErr = stdErr;
        }

        public void EnsureSuccessful()
        {
            EnsureSuccessful(false);
        }

            public void EnsureSuccessful(bool suppressOutput)
        {
            if (ExitCode != 0)
            {
                StringBuilder message = new StringBuilder($"Command failed with exit code {ExitCode}: {StartInfo.FileName} {StartInfo.Arguments}");

                if (!suppressOutput)
                {
                    if (!string.IsNullOrEmpty(StdOut))
                    {
                        message.AppendLine($"{Environment.NewLine}Standard Output:{Environment.NewLine}{StdOut}");
                    }

                    if (!string.IsNullOrEmpty(StdErr))
                    {
                        message.AppendLine($"{Environment.NewLine}Standard Error:{Environment.NewLine}{StdErr}");
                    }
                }

                throw new Exception(message.ToString());
            }
        }
    }
}
