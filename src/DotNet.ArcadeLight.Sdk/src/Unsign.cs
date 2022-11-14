// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading;

namespace DotNet.ArcadeLight.Sdk
{

    public class Unsign : Microsoft.Build.Utilities.Task
    {

        [Required]
        public string FilePath { get; set; }

        public override bool Execute()
        {

            try
            {
                ExecuteImpl();
                return !Log.HasLoggedErrors;
            }
            finally
            {
                // empty block
            }
        }

        private void ExecuteImpl()
        {
            using (var stream = File.Open(FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
            using (var peReader = new PEReader(stream))
            {
                var headers = peReader.PEHeaders;
                var entry = headers.PEHeader.CertificateTableDirectory;
                if (entry.Size == 0)
                {
                    return;
                }

                using (var writer = new BinaryWriter(stream))
                {
                    int certificateTableDirectoryOffset = (headers.PEHeader.Magic == PEMagic.PE32Plus) ? 144 : 128;
                    stream.Position = peReader.PEHeaders.PEHeaderStartOffset + certificateTableDirectoryOffset;

                    writer.Write((long)0);
                    writer.Flush();
                }
            }
        }
    }
}
