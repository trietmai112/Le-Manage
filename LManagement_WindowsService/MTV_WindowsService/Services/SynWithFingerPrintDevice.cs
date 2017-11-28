using MTV_WindowsService.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MTV_WindowsService.Services
{
    partial class SynWithFingerPrintDevice : ServiceBase
    {
        PipleConnection _pipleConnection = new PipleConnection();
        NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public SynWithFingerPrintDevice()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif
            _logger.Debug("SynWithFingerPrintDevice starting...");
            _pipleConnection.Start();
            _logger.Debug("SynWithFingerPrintDevice start completed");
            
        }
        
        protected override void OnStop()
        {
            _logger.Debug("SynWithFingerPrintDevice stoping...");
            _pipleConnection.Stop();
            _logger.Debug("SynWithFingerPrintDevice stoped");
        }
    }
}
