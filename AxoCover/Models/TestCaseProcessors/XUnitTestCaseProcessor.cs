﻿using AxoCover.Models.Data;
using AxoCover.Models.Extensions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Text.RegularExpressions;

namespace AxoCover.Models.TestCaseProcessors
{
  public class XUnitTestCaseProcessor : ITestCaseProcessor
  {
    private Regex _displayNameRegex = new Regex(@"(?>(?<path>[\w\.]*))(?>(?<arguments>.+))");
    private Regex _fullyQualifiedNameRegex = new Regex(@"(?>(?<path>[\w\.]*)) \((?>(?<id>\w+))\)");

    public bool CanProcessCase(TestCase testCase)
    {
      return testCase.ExecutorUri.ToString().Contains("xunit", StringComparison.OrdinalIgnoreCase);
    }

    public void ProcessCase(TestCase testCase, ref CodeItemKind testItemKind, ref string testItemPath, ref string displayName)
    {
      var fullyQualifiedNameMatch = _fullyQualifiedNameRegex.Match(testCase.FullyQualifiedName);
      if (fullyQualifiedNameMatch.Success)
      {
        var displayNameMatch = _displayNameRegex.Match(testCase.DisplayName);
        if (displayNameMatch.Success)
        {
          testItemKind = CodeItemKind.Data;
          displayName = displayNameMatch.Groups["arguments"].Value;
          testItemPath = fullyQualifiedNameMatch.Groups["path"].Value + "." + fullyQualifiedNameMatch.Groups["id"].Value;
        }
        else
        {
          testItemPath = fullyQualifiedNameMatch.Groups["path"].Value;
        }
      }
    }
  }
}
