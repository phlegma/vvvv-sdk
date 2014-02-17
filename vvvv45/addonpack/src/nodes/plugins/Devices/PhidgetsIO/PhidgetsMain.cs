﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Phidgets;
using Phidgets.Events;

namespace VVVV.Nodes
{
    public class PhidgetsMain<T> where T : Phidget, new()
    {

        #region Field Declaration

        public T FPhidget;
        public DeviceInfo FInfo;
        public List<PhidgetException> FPhidgetErrors = new List<PhidgetException>();
        private bool FAttached = false;
        private bool FConnected = false;

        public struct DeviceInfo
        {
            public string Name;
            public double SerialNumber;
            public double Version;
            public Phidget.PhidgetID PhidgetId;
        }

        #endregion Field Declaration


        public bool Attached
        {
            get { return FAttached; }
        }

        public bool Connected
        {
            get { return FConnected; }
        }

        public List<PhidgetException> Errors
        {
            get
            {
                if (FPhidgetErrors.Count > 0)
                {
                    List<PhidgetException> Temp = new List<PhidgetException>(FPhidgetErrors);
                    FPhidgetErrors.Clear();
                    return Temp;
                }
                else
                    return null;
            }
        }

        #region constructor + Close

        public PhidgetsMain()
        {
            FPhidget = new T();
            AddEventHandler();
        }

        public PhidgetsMain(int SerialNumber)
        {
            FPhidget = new T();
            AddEventHandler();
            try
            {
                if (SerialNumber > 0)
                {
                    FPhidget.open(SerialNumber);
                }
                else
                {
                    FPhidget.open();
                }
            }
            catch (PhidgetException ex)
            {
                FPhidgetErrors.Add(ex);
            }
        }

        public PhidgetsMain(int SerialNumber, string IP, int Port, string Password)
        {
            FPhidget = new T();
            AddEventHandler();
            try
            {
                if (SerialNumber > 0)
                {
                    if (!String.IsNullOrEmpty(Password))
                    {
                        FPhidget.open(SerialNumber, IP, Port, Password);
                    }
                    else
                    {
                        FPhidget.open(SerialNumber, IP, Port);
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(Password))
                    {
                        FPhidget.open(IP, Port, Password); 
                    }
                    else
                    {
                        FPhidget.open(IP, Port); 
                    }
                }
            }
            catch (PhidgetException ex)
            {
                FPhidgetErrors.Add(ex);
            }
        }

        public void Close()
        {
            RemoveEventHandler();
            FPhidget.close();
            Thread.Sleep(30);
        }

        #endregion constructor + Close


        #region Attach Detach Error Handling

        public void AddEventHandler()
        {
            FPhidget.Attach += new AttachEventHandler(AttachHandler);
            FPhidget.ServerConnect += new ServerConnectEventHandler(ServerConnect);
            FPhidget.ServerDisconnect += new ServerDisconnectEventHandler(ServerDisconnect);
            FPhidget.Detach += new DetachEventHandler(DetachHandler);
            FPhidget.Error += new ErrorEventHandler(ErrorHandler);
        }

        public void RemoveEventHandler()
        {
            FPhidget.Attach -= new AttachEventHandler(AttachHandler);
            FPhidget.Detach -= new DetachEventHandler(DetachHandler);
            FPhidget.Error -= new ErrorEventHandler(ErrorHandler);
            FPhidget.ServerConnect -= new ServerConnectEventHandler(ServerConnect);
            FPhidget.ServerDisconnect -= new ServerDisconnectEventHandler(ServerDisconnect);
        }

        void AttachHandler(object sender, AttachEventArgs e)
        {
            T attached = (T)sender;
            if (attached.Attached)
            {
                FInfo = new DeviceInfo();
                FInfo.Name = attached.Name;
                FInfo.SerialNumber = attached.SerialNumber;
                FInfo.Version = attached.Version;
                FInfo.PhidgetId = attached.ID;
            }

            FAttached = true;
        }

        void DetachHandler(object sender, DetachEventArgs e)
        {
            T attached = (T)sender;
            FAttached = false;
        }

        void ServerConnect(object sender, ServerConnectEventArgs e)
        {
            FConnected = true;
        }

        void ServerDisconnect(object sender, ServerDisconnectEventArgs e)
        {
            FConnected = false;
        }



        void ErrorHandler(object sender, ErrorEventArgs e)
        {
            FPhidgetErrors.Add(e.exception);
        }

        #endregion Attach Detach Event Handler

    }
}
