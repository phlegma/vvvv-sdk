using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Phidgets;
using Phidgets.Events;

namespace VVVV.Nodes
{
    public class WrapperManager
    {

        #region Field Declaration

        public Manager FPhidget;
        public DeviceInfo FInfo;
        public List<PhidgetException> FPhidgetErrors = new List<PhidgetException>();
        private bool FChanged;
        private bool disposed = false;


        public struct DeviceInfo
        {
            public string Name;
            public double SerialNumber;
            public double Version;
            public Phidget.PhidgetID PhidgetId;
        }

        #endregion Field Declaration


        public bool Changed
        {
            get 
            {
                bool temp = FChanged;
                FChanged = false;
                return FChanged; 
            }
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

        public WrapperManager()
        {
            FPhidget = new Manager();
            AddEventHandler();
            FPhidget.open();
        }

        public WrapperManager(string IP, int Port, String Password)
        {
            
            FPhidget = new Manager();
            AddEventHandler();
            if (String.IsNullOrEmpty(Password))
                FPhidget.open(IP,Port);
            else
                FPhidget.open(IP, Port,Password);
           
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
            FPhidget.Detach += new DetachEventHandler(DetachHandler);
            FPhidget.Error += new ErrorEventHandler(ErrorHandler);

        }

        public void RemoveEventHandler()
        {
            FPhidget.Attach -= new AttachEventHandler(AttachHandler);
            FPhidget.Detach -= new DetachEventHandler(DetachHandler);
            FPhidget.Error -= new ErrorEventHandler(ErrorHandler);
        }

        void AttachHandler(object sender, AttachEventArgs e)
        {
            Manager attached = (Manager)sender;
            FChanged = true;

        }

        void DetachHandler(object sender, DetachEventArgs e)
        {
            Manager attached = (Manager)sender;
            FChanged = true;
        }

        void ErrorHandler(object sender, ErrorEventArgs e)
        {
            FPhidgetErrors.Add(e.exception);
        }

        #endregion Attach Detach Event Handler

      
    }
}
