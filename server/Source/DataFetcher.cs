using System.Threading;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using server.Data;

namespace server.Source
{
    public class DataFetcherHostedService : IHostedService, IDisposable
    {
        private Timer timer;
        private readonly ILogger<DataFetcherHostedService> _logger;
        private readonly IServiceProvider _provider;
        private readonly IHostEnvironment _environment;

        public DataFetcherHostedService(ILogger<DataFetcherHostedService> logger, IServiceProvider provider, IWebHostEnvironment environment)
        {
            _logger = logger;
            _provider = provider;
            _environment = environment;
        }

        public Task StartAsync(CancellationToken stopToken)
        {
            _logger.LogInformation("DataFetcherHostedService initialization started.");
            var span = 15 - DateTime.Now.Minute % 15;
            if (span < 0) span += 15;
            timer = new Timer(DoFetch, null, TimeSpan.FromMinutes(span), TimeSpan.FromMinutes(15));
            // timer = new Timer(DoFetch, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));
            _logger.LogInformation("DataFetcherHostedService initialized.");
            return Task.CompletedTask;
        }

        private void DoFetch(object state)
        {
            _logger.LogInformation("A new fetch process is started.");
            DateTime dt = DateTime.Now;
            string tarrt = $"{_environment.ContentRootPath}/wwwroot/data/{dt.ToString("yyyyMMdd_HHmmss")}/";
            string scrrt = $"{_environment.ContentRootPath}/Scripts/";
            Directory.CreateDirectory(tarrt);

            _logger.LogInformation("Fetch data from weibo...");
            Process p = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "python",
                    CreateNoWindow = true,
                    // RedirectStandardError = true,
                    // RedirectStandardOutput = true,
                }
            };
            _logger.LogInformation(scrrt + "WeiBoData.py");
            p.StartInfo.ArgumentList.Add(scrrt + "WeiBoData.py");
            p.StartInfo.ArgumentList.Add(tarrt);
            p.Start();
            p.WaitForExit(3 * 60 * 1000);
            if (!p.HasExited)
            {
                _logger.LogError("Process working too long, terminating...");
                p.Kill();
                _logger.LogInformation("Process terminated. Skip weibo.");
            }
            else
            {
                if (p.ExitCode != 0)
                {
                    _logger.LogError($"Run Error.{p.ExitCode}");
                    goto zhihu;
                }
                _logger.LogInformation("Weibo data visualizing...");
                p.StartInfo.ArgumentList[0] = scrrt + "WeiBo Data Visualization.py";
                p.Start();
                p.WaitForExit(3 * 60 * 1000);
                if (!p.HasExited)
                {
                    _logger.LogError("Process working too long, terminating...");
                    p.Kill();
                    _logger.LogInformation("Process terminated. Skip visualization.");
                }
                if (p.ExitCode != 0)
                {
                    _logger.LogError($"Run Error.{p.ExitCode}");
                }
                _logger.LogInformation("Parsing and insert data line to database...");
                string[][] data = CSVReader.ReadCSV(tarrt + "weibo.csv");
                List<TopicEntry> topics = new List<TopicEntry>();
                foreach (var line in data[2..])
                {
                    if (line.Length < 3) continue;// ignore invalid data
                    topics.Add(new TopicEntry()
                    {
                        Topic = line[1],
                        HotScore = int.Parse(line[2])
                    });
                }
                using (var prov = _provider.CreateScope())
                {
                    var ef = prov.ServiceProvider.GetService<FetchedDataContext>();
                    ef.ChronicleRecords.AddAsync(new Models.ChronicleRecord()
                    {
                        Type = Models.ChronicleRecordType.Weibo,
                        Topics = topics,
                        RecordedTime = dt
                    });
                    
                    ef.SaveChanges();
                }
            }

        //fetch zhihu
        zhihu: _logger.LogInformation("Fetching data from zhihu...");
            p.StartInfo.ArgumentList[0] = scrrt + "getData.py";
            p.Start();
            p.WaitForExit(3 * 60 * 1000);
            if (!p.HasExited)
            {
                _logger.LogError("Process working too long, terminating...");
                p.Kill();
                _logger.LogInformation("Process terminated.");
                _logger.LogInformation("Skip zhihu.");
            }
            else
            {
                if (p.ExitCode != 0)
                {
                    _logger.LogError($"Run Error.{p.ExitCode}");
                    goto end;
                }
                _logger.LogInformation("Data visualization of zhihu started.");
                _logger.LogInformation("Visulizing answer ratio...");
                p.StartInfo.ArgumentList[0] = scrrt + "AnswerCmp.py";
                p.Start();
                p.WaitForExit(10000);
                if (!p.HasExited)
                {
                    _logger.LogError("Process takes too long, killing...");
                    p.Kill();
                    _logger.LogInformation("Skip");
                }
                if (p.ExitCode != 0)
                {
                    _logger.LogError($"Run Error.{p.ExitCode}");
                }
                _logger.LogInformation("Visulizing hot value...");
                p.StartInfo.ArgumentList[0] = scrrt + "HotCmp.py";
                p.Start();
                p.WaitForExit(10000);
                if (!p.HasExited)
                {
                    _logger.LogError("Process takes too long, killing...");
                    p.Kill();
                    _logger.LogInformation("Skip");
                }
                if (p.ExitCode != 0)
                {
                    _logger.LogError($"Run Error.{p.ExitCode}");
                }
                _logger.LogInformation("Visulizing words...");
                p.StartInfo.ArgumentList[0] = scrrt + "WordCloud.py";
                p.Start();
                p.WaitForExit(60000);
                if (!p.HasExited)
                {
                    _logger.LogError("Process takes too long, killing...");
                    p.Kill();
                    _logger.LogInformation("Skip");
                }
                if (p.ExitCode != 0)
                {
                    _logger.LogError($"Run Error.{p.ExitCode}");
                }
                _logger.LogInformation("Parsing and insert data line to database...");
                string[][] data = CSVReader.ReadCSV(tarrt + "zhihu.csv");
                List<TopicEntry> topics = new List<TopicEntry>();
                foreach (var line in data[1..])
                {
                    if (line.Length < 4) continue;
                    topics.Add(new TopicEntry()
                    {
                        Topic = line[3],
                        HotScore = int.Parse(line[2].Remove(line[2].IndexOf("万热度"))) * 10000,
                        Description = line?[4]
                    });
                }
                using (var prov = _provider.CreateScope())
                {
                    var ef = prov.ServiceProvider.GetService<FetchedDataContext>();
                    ef.ChronicleRecords.AddAsync(new Models.ChronicleRecord()
                    {
                        Type = Models.ChronicleRecordType.Zhihu,
                        Topics = topics,
                        RecordedTime = dt
                    });
                    ef.SaveChanges();
                }
            }
        end: _logger.LogInformation("Timed Task Finished.");
        }

        public Task StopAsync(CancellationToken stoptoken)
        {
            _logger.LogInformation("Finializing...");
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}