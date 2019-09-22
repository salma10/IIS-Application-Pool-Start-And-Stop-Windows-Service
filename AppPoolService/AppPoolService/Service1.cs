using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AppPoolService
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteToFile("Service is started at " + DateTime.Now);
            RestartApplicationPoolCollection();
            WriteToFile("Service is RestartApplicationPoolCollection at " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 60000; //number in milisecinds  
            timer.Enabled = true;  
        }

        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);  
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            RestartApplicationPoolCollection();
            WriteToFile("Service is recall at " + DateTime.Now);
        }
        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\AppPoolLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }

        [ModuleServiceMethod(PassThrough = true)]
        public void RestartApplicationPoolCollection()
        {
            // Use an ArrayList to transfer objects to the client.
           // ArrayList arrayOfApplicationBags = new ArrayList();

            ServerManager serverManager = new ServerManager();
            ApplicationPoolCollection applicationPoolCollection = serverManager.ApplicationPools;
            foreach (ApplicationPool applicationPool in applicationPoolCollection)
            {
                //PropertyBag applicationPoolBag = new PropertyBag();
                //applicationPoolBag[ServerManagerDemoGlobals.ApplicationPoolArray] = applicationPool;
                //arrayOfApplicationBags.Add(applicationPoolBag);
                // If the applicationPool is stopped, restart it.
                if (applicationPool.State == ObjectState.Stopped)
                {
                    WriteToFile("Service is Stopped at " + DateTime.Now);
                    try
                    {
                        WriteToFile("Service is Starting ready at " + DateTime.Now);
                        applicationPool.Start();
                        //applicationPool.Recycle();
                        WriteToFile("Service is Starting end at " + DateTime.Now);
                    }
                    catch(Exception ex)
                    {
                        WriteToFile("Service Exception " + DateTime.Now + " " + ex.Message);
                    }
                   
                }

                //if (applicationPool.State == ObjectState.Started)
                //{
                //    applicationPool.Stop();
                //}

            }

            // CommitChanges to persist the changes to the ApplicationHost.config.
            serverManager.CommitChanges();
            //return arrayOfApplicationBags;
        }
    }
}
