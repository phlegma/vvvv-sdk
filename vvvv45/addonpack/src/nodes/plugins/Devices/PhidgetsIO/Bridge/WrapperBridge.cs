using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Phidgets;
using Phidgets.Events;

namespace VVVV.Nodes
{
    class WrapperBridge: Phidgets<Bridge>, IPhidgetsWrapper
    {

        bool FChanged = false;
		bool FSensorChanged = false;
		bool FDigitalInputChanged = false;

        #region constructor

        public WrapperBridge()
            : base()
        {

        }

        public WrapperBridge(int SerialNumber)
            : base(SerialNumber)
        {

        }

        #endregion constructor


        #region setter fuctions


        public void SetBridgeEnable(int Index, bool value)
        {
            FPhidget.bridges[Index].Enabled = value;
        }

        public void SetBridgeGain(int Index, BridgeInput.Gains value)
        {
            FPhidget.bridges[Index].Gain = value;
        }


        #endregion setter function


        #region getter functions

        public bool GetBridgeEnable(int Index)
        {
            return FPhidget.bridges[Index].Enabled;
        }


        public double GetSensorValue(int Index)
        {
            return FPhidget.bridges[Index].BridgeValue;
        }

        public double GetBridgeMax(int Index)
        {
            return FPhidget.bridges[Index].BridgeMax;
        }

        public double GetBridgeMin(int Index)
        {
            return FPhidget.bridges[Index].BridgeMin;
        }

        public string GetGain(int Index)
        {
            return FPhidget.bridges[Index].Gain.ToString();
        }


        #endregion getter functions




        public override void AddChangedHandler()
        {
            FPhidget.BridgeData += new BridgeDataEventHandler(SensorChange);
        }

        void SensorChange(object sender, BridgeDataEventArgs e)
        {
            FChanged = true;
			FSensorChanged = true;
        }

        public override void RemoveChangedHandler()
        {
            FPhidget.BridgeData -= new BridgeDataEventHandler(SensorChange);
        }


        public int Count
        {
            get { return FPhidget.bridges.Count; }
        }


        public bool Changed
        {
            get
            {
                bool temp = FChanged;
                FChanged = false;
                return temp;
            }
        }
    }
}
