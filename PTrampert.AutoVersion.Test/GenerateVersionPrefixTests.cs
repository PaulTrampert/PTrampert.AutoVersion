using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using PTrampert.ExeInvoke;
using System.IO;

namespace PTrampert.AutoVersion.Test
{
    public class GenerateVersionPrefixTests
    {
        [Fact]
        public void GeneratesVersionPrefixWithIncrementedPatchWhenNotOnTag()
        {
            var invoker = new Mock<IExeInvoker>();
            invoker.Setup(i => i.Invoke("git describe --tags", It.IsAny<string[]>(), It.IsAny<ExeEnvironment>()))
                .Returns<string, string[], ExeEnvironment>(async (cmd, args, env) =>
                {
                    using (var stream = new MemoryStream(Encoding.UTF8.GetBytes("v1.2.3-22-abcde")))
                    using (var streamReader = new StreamReader(stream))
                    {
                        await env.StandardOutReader(streamReader);
                    }
                });
            var task = new GenerateVersionPrefix();
            task.Invoker = invoker.Object;
            Assert.True(task.Execute());
            Assert.Equal("1.2.4", task.VersionPrefix);
        }

        [Fact]
        public void GeneratesVersionPrefixWhenOnTag()
        {
            var invoker = new Mock<IExeInvoker>();
            invoker.Setup(i => i.Invoke("git describe --tags", It.IsAny<string[]>(), It.IsAny<ExeEnvironment>()))
                .Returns<string, string[], ExeEnvironment>(async (cmd, args, env) =>
                {
                    using (var stream = new MemoryStream(Encoding.UTF8.GetBytes("v1.2.3")))
                    using (var streamReader = new StreamReader(stream))
                    {
                        await env.StandardOutReader(streamReader);
                    }
                });
            var task = new GenerateVersionPrefix();
            task.Invoker = invoker.Object;
            Assert.True(task.Execute());
            Assert.Equal("1.2.3", task.VersionPrefix);
        }
    }
}
