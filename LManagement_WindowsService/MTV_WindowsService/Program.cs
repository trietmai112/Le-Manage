using MTV_WindowsService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MTV_WindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new SynWithFingerPrintDevice()
            };
            
#if DEBUG
            ServiceProcess.Helpers.ServiceRunner.LoadServices(ServicesToRun);
#else
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
