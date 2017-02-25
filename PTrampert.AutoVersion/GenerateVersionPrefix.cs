﻿using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using PTrampert.ExeInvoke;
using System.Text.RegularExpressions;

namespace PTrampert.AutoVersion
{
    public class GenerateVersionPrefix : Task
    {
        public string VersionPrefix { get; set; }

        public IExeInvoker Invoker = new ExeInvoker();

        public override bool Execute()
        {
            var env = new ExeEnvironment
            {
                StandardOutReader = async (stream) =>
                {
                    var describeString = await stream.ReadToEndAsync();
                    var semver = SemVer.Parse(describeString);
                    if (semver.PreReleaseVersion)
                    {
                        semver.Patch++;
                        semver.Suffix = null;
                    }
                    VersionPrefix = semver.ToString();
                }
            };

            Invoker.Invoke("git describe --tags", env: env).Wait();
            return true;
        }
    }
}
