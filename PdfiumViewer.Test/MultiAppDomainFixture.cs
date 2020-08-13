using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using NUnit.Framework;

namespace PdfiumViewer.Test
{
  [TestFixture]
  public class MultiAppDomainFixture
  {
    private void RunThreads()
    {
      const int scripts = 10;
      const int iterations = 20;
      List<Thread> threads = new List<Thread>();

      for (int i = 0; i < scripts; i++)
      {
        Thread thread = new Thread(() =>
        {
          try
          {
            for (int j = 0; j < iterations; j++)
              using (Runner runner = new Runner())
              {
                runner.Run();
              }
          }
          catch (Exception ex)
          {
            Console.WriteLine(ex);
            Console.WriteLine(ex.StackTrace);
          }
        });

        threads.Add(thread);
        thread.Start();
      }

      foreach (Thread thread in threads) thread.Join();
    }

    private class Script : MarshalByRefObject
    {
      public void Run()
      {
        using (Stream stream = typeof(MultiAppDomainFixture).Assembly.GetManifestResourceStream(
          typeof(MultiAppDomainFixture).Namespace + ".Example" + (new Random().Next(2) + 1) + ".pdf"
        ))
        {
          PdfDocument document = PdfDocument.Load(stream);

          for (int i = 0; i < document.PageCount; i++)
            using (document.Render(i, 96, 96, false))
            {
            }
        }
      }
    }

    private class Runner : IDisposable
    {
      private AppDomain _appDomain;
      private bool _disposed;

      public Runner()
      {
        _appDomain = AppDomain.CreateDomain(
          "Unit test",
          AppDomain.CurrentDomain.Evidence,
          new AppDomainSetup
          {
            ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
            ApplicationName = "Unit test"
          }
        );
      }

      public void Dispose()
      {
        if (!_disposed)
        {
          if (_appDomain != null)
          {
            AppDomain.Unload(_appDomain);
            _appDomain = null;
          }

          _disposed = true;
        }
      }

      public void Run()
      {
        Script script = (Script) _appDomain.CreateInstanceAndUnwrap(
          typeof(Script).Assembly.FullName,
          typeof(Script).FullName
        );

        script.Run();
      }
    }

    [Test]
    public void MultipleAppDomains()
    {
      RunThreads();
    }

    [Test]
    public void MultipleAppDomainsAndCurrent()
    {
      using (Runner runner = new Runner())
      {
        runner.Run();

        RunThreads();
      }
    }
  }
}
