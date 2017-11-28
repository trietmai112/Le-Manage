using MTV_WindowsService.CallerMethods;
using MTV_WindowsService.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MTV_WindowsService.Functions
{
    public class PipleConnection: IFunction
    {
        NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        NamedPipeServerStream pipeServer = null;
        
        StreamReader sr = null;// new StreamReader(pipeServer);
        StreamWriter sw = null;// new StreamWriter(pipeServer);
        Thread _mainThread = null;
        private void Doing()
        {
            do
            {
                try
                {
                    pipeServer.WaitForConnection();                    
                    sw.WriteLine("Waiting");
                    sw.Flush();
                    pipeServer.WaitForPipeDrain();
                    var messageFromClient = sr.ReadLine();
                    _logger.Debug($"PipeConnection recive a message '{messageFromClient}' from client");
                    switch (messageFromClient.ToLower())
                    {
                        case "getdata":
                            {
                                _logger.Debug($"PipleConnection begin get data from device");
                                FingerPrintDevice fingerPrintDevice = new FingerPrintDevice();
                                fingerPrintDevice.GetTimeTrackerFromDevice();
                                _logger.Debug($"PipleConnection end get data from device");
                                break;
                            }
                    }
                    pipeServer.WaitForPipeDrain();
                    sw.WriteLine("Complete");
                    sw.Flush();                    
                }

                catch (Exception ex)
                {
                    _logger.Error("Waiting connect/read message from client occur error");
                    _logger.Error($"-> Error message: {ex.Message}");
                    _logger.Error($"-> Error stack trace: {ex.StackTrace}");
                }

                finally
                {
                    if (pipeServer.IsConnected)
                    {
                        pipeServer.Disconnect();
                    }
                }
            } while (true);


        }

        public void Start()
        {
            try
            {
                _logger.Debug("PipleConnection starting...");
                PipeSecurity ps = new PipeSecurity();
                System.Security.Principal.SecurityIdentifier sid = 
                    new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid , null);
                PipeAccessRule par = new PipeAccessRule(sid, PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow);
                ps.AddAccessRule(par);

                pipeServer = new NamedPipeServerStream(ConfigurationManager.AppSettings["PipleName"], PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, int.MaxValue , int.MaxValue, ps);
                sr = new StreamReader(pipeServer);
                sw = new StreamWriter(pipeServer);
                _mainThread = new Thread(new ThreadStart(Doing));
                _mainThread.IsBackground = true;
                _mainThread.Start();
                _logger.Debug("PipleConnection start completed");
            }
            catch(Exception ex)
            {
                _logger.Error("Can not initilize piple server");
                _logger.Error($"-> Error message: {ex.Message}");
                _logger.Error($"-> Error stack trace: {ex.StackTrace}");
            }
        }

        public void Stop()
        {
            _logger.Debug("PipleConnection stoping...");
            try
            {
                pipeServer?.Disconnect();
            }catch(Exception ex)
            {
                _logger.Debug($"----- piple sever cannot disconnect because: {ex.Message}");
            }
            _logger.Debug("----- piple sever disconnect");
            pipeServer?.Close();
            _logger.Debug("----- piple sever close");
            pipeServer?.Dispose();
            _logger.Debug("----- piple sever dispose");
            try
            {
                _mainThread?.Abort();
                _logger.Debug("----- main sever disconnect");
            }
            catch(Exception ex)
            {
                _logger.Error("PipleConnection occur an error when stop");
                _logger.Error($"-> Error message: {ex.Message}");
                _logger.Error($"-> Error stack trace: {ex.StackTrace}");
            }
            _logger.Debug("PipleConnection stoped");
        }


    
    }
}
