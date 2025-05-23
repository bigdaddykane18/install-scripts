﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using Xunit;
using System.Collections.Generic;
using Microsoft.NET.TestFramework.Assertions;
using FluentAssertions;
using System.Text.RegularExpressions;

namespace Microsoft.DotNet.InstallationScript.Tests
{
   public class AkaMsLinksTests
   {
       /// <summary>
       /// Test verifies E2E the aka.ms resolution for SDK
       /// </summary>
       [Theory]
       // aka.ms links don't work for versions before 6.0 yet
       //[InlineData("1.0", null, @"https://aka.ms/dotnet/1.0/dotnet-sdk-")]
       //[InlineData("1.1", null, @"https://aka.ms/dotnet/1.1/dotnet-sdk-")]
       //[InlineData("2.0", null, @"https://aka.ms/dotnet/2.0/dotnet-sdk-")]
       //[InlineData("2.1", null, @"https://aka.ms/dotnet/2.1/dotnet-sdk-")]
       //[InlineData("3.0", null, @"https://aka.ms/dotnet/3.0/dotnet-sdk-")]
       //[InlineData("3.1", null, @"https://aka.ms/dotnet/3.1/dotnet-sdk-")]
       //[InlineData("5.0", null, @"https://aka.ms/dotnet/5.0/dotnet-sdk-")]
       //[InlineData("5.0.1xx", null, @"https://aka.ms/dotnet/5.0.1xx/dotnet-sdk-")]
       //[InlineData("5.0.2xx", null, @"https://aka.ms/dotnet/5.0.2xx/dotnet-sdk-")]
       //[InlineData("STS", null, @"https://aka.ms/dotnet/STS/dotnet-sdk-")]
       //[InlineData("LTS", null, @"https://aka.ms/dotnet/LTS/dotnet-sdk-")]
       //[InlineData("5.0.2xx", "daily", @"https://aka.ms/dotnet/5.0.2xx/daily/dotnet-sdk-")]
       //[InlineData("5.0.2xx", "preview", @"https://aka.ms/dotnet/5.0.2xx/preview/dotnet-sdk-")]
       //[InlineData("5.0.2xx", "ga", @"https://aka.ms/dotnet/5.0.2xx/dotnet-sdk-")]
        
       //[InlineData("6.0", "preview", @"https://aka.ms/dotnet/6.0/preview/dotnet-sdk-")]
       //[InlineData("6.0", "ga", @"https://aka.ms/dotnet/6.0/dotnet-sdk-")]

       [InlineData("6.0", "daily", @"https://aka.ms/dotnet/6.0/daily/dotnet-sdk-")]
       [InlineData("7.0", "daily", @"https://aka.ms/dotnet/7.0/daily/dotnet-sdk-")]
       [InlineData("8.0", "ga", @"https://aka.ms/dotnet/8.0/dotnet-sdk-")]
       [InlineData("9.0", "ga", @"https://aka.ms/dotnet/9.0/dotnet-sdk-")]
       public void SDK_IntegrationTest(string channel, string quality, string expectedLink)
       {
           string expectedLinkPattern = Regex.Escape(expectedLink);
           if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
           {
               expectedLinkPattern += "win";
           }
           else
           {
               expectedLinkPattern += "(linux|linux-musl|osx)";
           }

           expectedLinkPattern += "-(x86|x64|arm|arm64)\\.(zip|tar\\.gz)";

           var args = new List<string> { "-dryrun", "-channel", channel, "-verbose" };
           if (!string.IsNullOrWhiteSpace(quality))
           {
               args.Add("-quality");
               args.Add(quality);
           }

           var commandResult = TestUtils.CreateInstallCommand(args).ExecuteInstallation();

           commandResult.Should().Pass();
           commandResult.Should().NotHaveStdErr();
           commandResult.Should().HaveStdOutContaining(output => Regex.IsMatch(output, expectedLinkPattern));
           commandResult.Should().HaveStdOutContaining("The redirect location retrieved:");
           commandResult.Should().HaveStdOutContaining("Downloading using legacy url will not be attempted.");
           commandResult.Should().HaveStdOutContaining("Repeatable invocation:");
           commandResult.Should().HaveStdOutContainingIgnoreCase("-version");
           commandResult.Should().NotHaveStdOutContaining("Falling back to latest.version file approach.");
           commandResult.Should().NotHaveStdOutContaining("Legacy named payload URL: ");
       }

       /// <summary>
       /// Test verifies E2E the aka.ms resolution for runtimes
       /// </summary>
       [Theory]
       // aka.ms links don't work for versions before 6.0 yet
       //[InlineData("1.0", "dotnet", null, @"https://aka.ms/dotnet/1.0/dotnet-runtime-")]
       //[InlineData("1.1", "dotnet", null, @"https://aka.ms/dotnet/1.1/dotnet-runtime-")]
       //[InlineData("2.0", "dotnet", null, @"https://aka.ms/dotnet/2.0/dotnet-runtime-")]
       //[InlineData("2.1", "dotnet", null, @"https://aka.ms/dotnet/2.1/dotnet-runtime-")]
       //[InlineData("3.0", "dotnet", null, @"https://aka.ms/dotnet/3.0/dotnet-runtime-")]
       //[InlineData("3.1", "dotnet", null, @"https://aka.ms/dotnet/3.1/dotnet-runtime-")]
       //[InlineData("5.0", "dotnet", null, @"https://aka.ms/dotnet/5.0/dotnet-runtime-")]
       //[InlineData("STS", "dotnet", null, @"https://aka.ms/dotnet/STS/dotnet-runtime-")]
       //[InlineData("LTS", "dotnet", null, @"https://aka.ms/dotnet/LTS/dotnet-runtime-")]
       //[InlineData("5.0", "dotnet", "daily", @"https://aka.ms/dotnet/5.0/daily/dotnet-runtime-")]
       //[InlineData("5.0", "dotnet", "preview", @"https://aka.ms/dotnet/5.0/preview/dotnet-runtime-")]
       //[InlineData("5.0", "dotnet", "ga", @"https://aka.ms/dotnet/5.0/dotnet-runtime-")]
       //[InlineData("2.0", "aspnetcore", null, @"https://aka.ms/dotnet/2.0/aspnetcore-runtime-")]
       //[InlineData("2.1", "aspnetcore", null, @"https://aka.ms/dotnet/2.1/aspnetcore-runtime-")]
       //[InlineData("3.0", "aspnetcore", null, @"https://aka.ms/dotnet/3.0/aspnetcore-runtime-")]
       //[InlineData("3.1", "aspnetcore", null, @"https://aka.ms/dotnet/3.1/aspnetcore-runtime-")]
       //[InlineData("5.0", "aspnetcore", null, @"https://aka.ms/dotnet/5.0/aspnetcore-runtime-")]
       //[InlineData("STS", "aspnetcore", null, @"https://aka.ms/dotnet/STS/aspnetcore-runtime-")]
       //[InlineData("LTS", "aspnetcore", null, @"https://aka.ms/dotnet/LTS/aspnetcore-runtime-")]
       //[InlineData("5.0", "aspnetcore", "daily", @"https://aka.ms/dotnet/5.0/daily/aspnetcore-runtime-")]
       //[InlineData("5.0", "aspnetcore", "preview", @"https://aka.ms/dotnet/5.0/preview/aspnetcore-runtime-")]
       //[InlineData("5.0", "aspnetcore", "ga", @"https://aka.ms/dotnet/5.0/aspnetcore-runtime-")]
       //[InlineData("3.0", "windowsdesktop", null, @"https://aka.ms/dotnet/3.0/windowsdesktop-runtime-")]
       //[InlineData("3.1", "windowsdesktop", null, @"https://aka.ms/dotnet/3.1/windowsdesktop-runtime-")]
       //[InlineData("5.0", "windowsdesktop", null, @"https://aka.ms/dotnet/5.0/windowsdesktop-runtime-")]
       //[InlineData("STS", "windowsdesktop", null, @"https://aka.ms/dotnet/STS/windowsdesktop-runtime-")]
       //[InlineData("LTS", "windowsdesktop", null, @"https://aka.ms/dotnet/LTS/windowsdesktop-runtime-")]
       //[InlineData("5.0", "windowsdesktop", "daily", @"https://aka.ms/dotnet/5.0/daily/windowsdesktop-runtime-")]
       //[InlineData("5.0", "windowsdesktop", "preview", @"https://aka.ms/dotnet/5.0/preview/windowsdesktop-runtime-")]
       //[InlineData("5.0", "windowsdesktop", "ga", @"https://aka.ms/dotnet/5.0/windowsdesktop-runtime-")]

       // [InlineData("6.0", "windowsdesktop", "preview", @"https://aka.ms/dotnet/6.0/preview/windowsdesktop-runtime-")]
       // [InlineData("6.0", "windowsdesktop", "ga", @"https://aka.ms/dotnet/6.0/windowsdesktop-runtime-")]
        
       [InlineData("6.0", "windowsdesktop", "daily", @"https://aka.ms/dotnet/6.0/daily/windowsdesktop-runtime-")]
       [InlineData("7.0", "windowsdesktop", "daily", @"https://aka.ms/dotnet/7.0/daily/windowsdesktop-runtime-")]
       public void Runtime_IntegrationTest(string channel, string runtime, string quality, string expectedLink)
       {
           if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && runtime == "windowsdesktop")
           {
               // Do not run windowsdesktop tests on Linux environment.
               return;
           }

           string expectedLinkPattern = Regex.Escape(expectedLink);
           if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
           {
               expectedLinkPattern += "win";
           }
           else
           {
               expectedLinkPattern += "(linux|linux-musl|osx)";
           }

           expectedLinkPattern += "-(x86|x64|arm|arm64)\\.(zip|tar\\.gz)";

           var args = new List<string> { "-dryrun", "-channel", channel, "-verbose", "-runtime", runtime };
           if (!string.IsNullOrWhiteSpace(quality))
           {
               args.Add("-quality");
               args.Add(quality);
           }

           var commandResult = TestUtils.CreateInstallCommand(args).ExecuteInstallation();

           commandResult.Should().Pass();
           commandResult.Should().NotHaveStdErr();
           commandResult.Should().HaveStdOutContaining(output => Regex.IsMatch(output, expectedLinkPattern));
           commandResult.Should().HaveStdOutContaining("The redirect location retrieved:");
           commandResult.Should().HaveStdOutContaining("Downloading using legacy url will not be attempted.");
           commandResult.Should().HaveStdOutContaining("Repeatable invocation:");
           commandResult.Should().HaveStdOutContainingIgnoreCase("-version");
           commandResult.Should().NotHaveStdOutContaining("Falling back to latest.version file approach.");
           commandResult.Should().NotHaveStdOutContaining("Legacy named payload URL: ");
       }

       [Theory]
       [InlineData("2.1", null, false, @"https://aka.ms/dotnet/2.1/dotnet-sdk-")]
       [InlineData("3.1", null, false, @"https://aka.ms/dotnet/3.1/dotnet-sdk-")]
       [InlineData("5.0", null, false, @"https://aka.ms/dotnet/5.0/dotnet-sdk-")]
       [InlineData("5.0.1xx", null, false, @"https://aka.ms/dotnet/5.0.1xx/dotnet-sdk-")]
       [InlineData("5.0.2xx", null, false, @"https://aka.ms/dotnet/5.0.2xx/dotnet-sdk-")]
       [InlineData("STS", null, false, @"https://aka.ms/dotnet/STS/dotnet-sdk-")]
       [InlineData("LTS", null, false, @"https://aka.ms/dotnet/LTS/dotnet-sdk-")]
       [InlineData("5.0.2xx", "daily", false, @"https://aka.ms/dotnet/5.0.2xx/daily/dotnet-sdk-")]
       [InlineData("5.0.2xx", "preview", false, @"https://aka.ms/dotnet/5.0.2xx/preview/dotnet-sdk-")]
       [InlineData("5.0.2xx", "ga", false, @"https://aka.ms/dotnet/5.0.2xx/dotnet-sdk-")]
       [InlineData("3.1", null, true, @"https://aka.ms/dotnet/internal/3.1/dotnet-sdk-")]
       [InlineData("5.0.2xx", null, true, @"https://aka.ms/dotnet/internal/5.0.2xx/dotnet-sdk-")]
       [InlineData("STS", null, true, @"https://aka.ms/dotnet/internal/STS/dotnet-sdk-")]
       [InlineData("LTS", null, true, @"https://aka.ms/dotnet/internal/LTS/dotnet-sdk-")]
       public void LinkCanBeCreatedForSdk(string channel, string quality, bool isInternal, string expectedLink)
       {
           string expectedLinkPattern = Regex.Escape(expectedLink);
           if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
           {
               expectedLinkPattern += "win";
           }
           else
           {
               expectedLinkPattern += "(linux|linux-musl|osx)";
           }

           expectedLinkPattern += "-(x86|x64|arm|arm64)\\.(zip|tar\\.gz)";

           var args = new List<string> { "-dryrun", "-channel", channel, "-verbose" };

           if (!string.IsNullOrWhiteSpace (quality))
           {
               args.Add("-quality");
               args.Add(quality);
           }

           string? feedCredentials = default;
           if (isInternal)
           {
               feedCredentials = Guid.NewGuid().ToString();
               args.Add("-internal");
               args.Add("-feedcredential");
               args.Add(feedCredentials);
           }

           var commandResult = TestUtils.CreateInstallCommand(args).ExecuteInstallation();

           commandResult.Should().HaveStdOutContaining(output => Regex.IsMatch(output, expectedLinkPattern));

           if(isInternal)
           {
               commandResult.Should().NotHaveStdOutContainingIgnoreCase(feedCredentials ?? string.Empty);
           }
       }

       [Theory]
       [InlineData("2.1", "dotnet", null, false, @"https://aka.ms/dotnet/2.1/dotnet-runtime-")]
       [InlineData("3.1", "dotnet", null, false, @"https://aka.ms/dotnet/3.1/dotnet-runtime-")]
       [InlineData("5.0", "dotnet", null, false, @"https://aka.ms/dotnet/5.0/dotnet-runtime-")]
       [InlineData("STS", "dotnet", null, false, @"https://aka.ms/dotnet/STS/dotnet-runtime-")]
       [InlineData("LTS", "dotnet", null, false, @"https://aka.ms/dotnet/LTS/dotnet-runtime-")]
       [InlineData("5.0", "dotnet", "daily", false, @"https://aka.ms/dotnet/5.0/daily/dotnet-runtime-")]
       [InlineData("5.0", "dotnet", "preview", false, @"https://aka.ms/dotnet/5.0/preview/dotnet-runtime-")]
       [InlineData("5.0", "dotnet", "ga", false, @"https://aka.ms/dotnet/5.0/dotnet-runtime-")]
       [InlineData("2.1", "aspnetcore", null, false, @"https://aka.ms/dotnet/2.1/aspnetcore-runtime-")]
       [InlineData("3.1", "aspnetcore", null, false, @"https://aka.ms/dotnet/3.1/aspnetcore-runtime-")]
       [InlineData("5.0", "aspnetcore", null, false, @"https://aka.ms/dotnet/5.0/aspnetcore-runtime-")]
       [InlineData("STS", "aspnetcore", null, false, @"https://aka.ms/dotnet/STS/aspnetcore-runtime-")]
       [InlineData("LTS", "aspnetcore", null, false, @"https://aka.ms/dotnet/LTS/aspnetcore-runtime-")]
       [InlineData("5.0", "aspnetcore", "daily", false, @"https://aka.ms/dotnet/5.0/daily/aspnetcore-runtime-")]
       [InlineData("5.0", "aspnetcore", "preview", false, @"https://aka.ms/dotnet/5.0/preview/aspnetcore-runtime-")]
       [InlineData("5.0", "aspnetcore", "ga", false, @"https://aka.ms/dotnet/5.0/aspnetcore-runtime-")]
       [InlineData("3.1", "windowsdesktop", null, false, @"https://aka.ms/dotnet/3.1/windowsdesktop-runtime-")]
       [InlineData("5.0", "windowsdesktop", null, false, @"https://aka.ms/dotnet/5.0/windowsdesktop-runtime-")]
       [InlineData("STS", "windowsdesktop", null, false, @"https://aka.ms/dotnet/STS/windowsdesktop-runtime-")]
       [InlineData("LTS", "windowsdesktop", null, false, @"https://aka.ms/dotnet/LTS/windowsdesktop-runtime-")]
       [InlineData("5.0", "windowsdesktop", "daily", false, @"https://aka.ms/dotnet/5.0/daily/windowsdesktop-runtime-")]
       [InlineData("5.0", "windowsdesktop", "preview", false,  @"https://aka.ms/dotnet/5.0/preview/windowsdesktop-runtime-")]
       [InlineData("5.0", "windowsdesktop", "ga", false, @"https://aka.ms/dotnet/5.0/windowsdesktop-runtime-")]
       [InlineData("LTS", "dotnet", null, true, @"https://aka.ms/dotnet/internal/LTS/dotnet-runtime-")]
       [InlineData("5.0", "dotnet", "daily", true, @"https://aka.ms/dotnet/internal/5.0/daily/dotnet-runtime-")]
       [InlineData("STS", "aspnetcore", null, true, @"https://aka.ms/dotnet/internal/STS/aspnetcore-runtime-")]
       [InlineData("5.0", "aspnetcore", "ga", true, @"https://aka.ms/dotnet/internal/5.0/aspnetcore-runtime-")]
       [InlineData("LTS", "windowsdesktop", null, true, @"https://aka.ms/dotnet/internal/LTS/windowsdesktop-runtime-")]
       [InlineData("5.0", "windowsdesktop", "ga", true, @"https://aka.ms/dotnet/internal/5.0/windowsdesktop-runtime-")]
       [InlineData("6.0", "windowsdesktop", "ga", true, @"https://aka.ms/dotnet/internal/6.0/windowsdesktop-runtime-")]
       [InlineData("7.0", "windowsdesktop", "ga", true, @"https://aka.ms/dotnet/internal/7.0/windowsdesktop-runtime-")]
       [InlineData("8.0", "windowsdesktop", "ga", true, @"https://aka.ms/dotnet/internal/8.0/windowsdesktop-runtime-")]
       [InlineData("9.0", "windowsdesktop", "ga", true, @"https://aka.ms/dotnet/internal/9.0/windowsdesktop-runtime-")]
       public void LinkCanBeCreatedForGivenRuntime(string channel, string runtime, string quality, bool isInternal, string expectedLink)
       {
           if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && runtime == "windowsdesktop")
           {
               // Do not run windowsdesktop tests on Linux environment.
               return;
           }

           string expectedLinkPattern = Regex.Escape(expectedLink);
           if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
           {
               expectedLinkPattern += "win";
           }
           else
           {
               expectedLinkPattern += "(linux|linux-musl|osx)";
           }

           expectedLinkPattern += "-(x86|x64|arm|arm64)\\.(zip|tar\\.gz)";

           var args = new List<string> { "-dryrun", "-channel", channel, "-verbose", "-runtime", runtime };

           if (!string.IsNullOrWhiteSpace(quality))
           {
               args.Add("-quality");
               args.Add(quality);
           }

           string? feedCredentials = default;
           if (isInternal)
           {
               feedCredentials = Guid.NewGuid().ToString();
               args.Add("-internal");
               args.Add("-feedcredential");
               args.Add(feedCredentials);
           }

           var commandResult = TestUtils.CreateInstallCommand(args).ExecuteInstallation();

           commandResult.Should().HaveStdOutContaining(output => Regex.IsMatch(output, expectedLinkPattern));

           if(isInternal)
           {
               commandResult.Should().NotHaveStdOutContainingIgnoreCase(feedCredentials ?? string.Empty);
           }
       }

       [Theory]
       [InlineData("STS", null, "daily", @"https://aka.ms/dotnet/STS/dotnet-sdk-")]
       [InlineData("LTS", "dotnet", "preview", @"https://aka.ms/dotnet/LTS/dotnet-runtime-")]
       [InlineData("STS", "aspnetcore", "daily", @"https://aka.ms/dotnet/STS/aspnetcore-runtime-")]
       [InlineData("LTS", "windowsdesktop", "preview", @"https://aka.ms/dotnet/LTS/windowsdesktop-runtime-")]
       public void QualityIsSkippedForLTSAndCurrentChannel(string channel, string runtime, string quality, string expectedLink)
       {
           if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && runtime == "windowsdesktop")
           {
               // Do not run windowsdesktop tests on Linux environment.
               return;
           }

           string expectedLinkPattern = Regex.Escape(expectedLink);
           if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
           {
               expectedLinkPattern += "win";
           }
           else
           {
               expectedLinkPattern += "(linux|linux-musl|osx)";
           }

           expectedLinkPattern += "-(x86|x64|arm|arm64)\\.(zip|tar\\.gz)";

           var args = new List<string> { "-dryrun", "-channel", channel, "-verbose", "-quality", quality };

           if (!string.IsNullOrWhiteSpace(runtime))
           {
               args.Add("-runtime");
               args.Add(runtime);
           }

           var commandResult = TestUtils.CreateInstallCommand(args).ExecuteInstallation();

           commandResult.Should().HaveStdOutContaining("Specifying quality for STS or LTS channel is not supported, the quality will be ignored.");
           commandResult.Should().HaveStdOutContaining(output => Regex.IsMatch(output, expectedLinkPattern));
       }

       [Theory]
       [InlineData("Fake", null, "daily")]
       [InlineData("Fake", "dotnet", "daily")]
       [InlineData("Fake", "aspnetcore", "daily")]
       [InlineData("Fake", "windowsdesktop", "daily")]
       public void NoFallbackIfQualityIsGiven(string channel, string runtime, string quality)
       {
           if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && runtime == "windowsdesktop")
           {
               // Do not run windowsdesktop tests on Linux environment.
               return;
           }

           var args = new List<string> { "-dryrun", "-channel", channel, "-verbose", "-quality", quality };

           if (!string.IsNullOrWhiteSpace(runtime))
           {
               args.Add("-runtime");
               args.Add(runtime);
           }

           var commandResult = TestUtils.CreateInstallCommand(args).ExecuteInstallation();

           commandResult.Should().Fail();
           commandResult.Should().HaveStdErrContaining("Failed to locate the latest version in the channel");
       }

       [Fact]
       public void AkaMsLinkIsNotUsedWhenExactVersionIsSpecified()
       {
           var args = new string[] { "-dryrun", "-version", "3.1.100", "-verbose" };

           var commandResult = TestUtils.CreateInstallCommand(args).ExecuteInstallation();

           commandResult.Should().Pass();
           commandResult.Should().NotHaveStdErr();
           commandResult.Should().NotHaveStdOutContaining("Retrieving primary payload URL from aka.ms link for channel");
           commandResult.Should().HaveStdOutContaining("Repeatable invocation:");
       }

       [Fact]
       public void AkaMsLinkIsNotUsedWhenExactVersionInJsonFileIsSpecified()
       {
           var installationScriptTestsJsonFile = Path.Combine(Environment.CurrentDirectory,
             "Assets", "InstallationScriptTests.json");
           var args = new string[] { "-dryrun", "-jsonfile", installationScriptTestsJsonFile, "-verbose" };

           var commandResult = TestUtils.CreateInstallCommand(args).ExecuteInstallation();

           commandResult.Should().Pass();
           commandResult.Should().NotHaveStdErr();
           commandResult.Should().NotHaveStdOutContaining("Retrieving primary payload URL from aka.ms link for channel");
           commandResult.Should().HaveStdOutContaining("Repeatable invocation:");
       }
   }
}
