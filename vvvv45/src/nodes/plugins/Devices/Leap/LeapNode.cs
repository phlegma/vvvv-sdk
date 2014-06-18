#region usings
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.Algorithm;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using Leap;

#endregion usings

namespace VVVV.Nodes.Devices
{
	#region PluginInfo
	[PluginInfo(Name = "Leap",
	Category = "Devices", 
	Help = "Returns the tracking data of the Leap device",
	Tags = "tracking, hand, finger",
	AutoEvaluate = true)]
	#endregion PluginInfo
	public class LeapNode : IPluginEvaluate, IDisposable
	{
		#region fields & pins
		#pragma warning disable 0649
		
		//circle
		[Input("Enable Circle Gesture")]
		IDiffSpread<bool> FEnableCircleGesture;
		
		[Input("Circle Min Radius (mm)", DefaultValue = 5.0, Visibility = PinVisibility.OnlyInspector)]
		IDiffSpread<float> FCircleMinRadius;
		
		[Input("Circle Min Arc (radians)", DefaultValue = 1.5*Math.PI, Visibility = PinVisibility.OnlyInspector)]
		IDiffSpread<float> FCircleMinArc;
		
		//swipe
		[Input("Enable Swipe Gesture")]
		IDiffSpread<bool> FEnableSwipeGesture;
		
		[Input("Swipe Min Length (mm)", DefaultValue = 150.0, Visibility = PinVisibility.OnlyInspector)]
		IDiffSpread<float> FSwipeMinLength;
		
		[Input("Swipe Min Velocity (mm/s)", DefaultValue = 1000.0, Visibility = PinVisibility.OnlyInspector)]
		IDiffSpread<float> FSwipeMinVelocity;
		
		//key tab
		[Input("Enable KeyTab Gesture")]
		IDiffSpread<bool> FEnableKeyTabGesture;
		
		[Input("KeyTab Min Down Velocity (mm/s)", DefaultValue = 50.0, Visibility = PinVisibility.OnlyInspector)]
		IDiffSpread<float> FKeyTabMindDownVelo;
		
		[Input("KeyTab History Seconds (s)", DefaultValue = 0.1, Visibility = PinVisibility.OnlyInspector)]
		IDiffSpread<float> FKeyTabHistorySeconds;
		
		[Input("KeyTab Min Distance (mm)", DefaultValue = 5.0, Visibility = PinVisibility.OnlyInspector)]
		IDiffSpread<float> FKeyTabMindDistance;
		
		//screen tab
		[Input("Enable ScreenTab Gesture")]
		IDiffSpread<bool> FEnableScreenTabGesture;
		
		[Input("ScreenTab Min Forward Velocity (mm/s)", DefaultValue = 50.0, Visibility = PinVisibility.OnlyInspector)]
		IDiffSpread<float> FScreenTabMindForwardVelo;
		
		[Input("ScreenTab History Seconds (s)", DefaultValue = 0.1, Visibility = PinVisibility.OnlyInspector)]
		IDiffSpread<float> FScreenTabHistorySeconds;
		
		[Input("ScreenTab Min Distance (mm)", DefaultValue = 3.0, Visibility = PinVisibility.OnlyInspector)]
		IDiffSpread<float> FScreenTabMindDistance;
		
		//settings
		[Input("Reset", IsBang = true)]
		IDiffSpread<bool> FResetIn;

        [Input("Enable")]
        IDiffSpread<bool> FEnabelIn;

        [Output("Frame")]
        ISpread<Frame> FFrameOut;

        [Output("Gestures")]
        ISpread<GestureList> FGestureOut;


        
		#pragma warning restore
		
		Controller FLeapController;
		Frame FLastFrame;
		
		#endregion fields & pins
		
		public LeapNode()
		{
			FLeapController = new Controller();
			FLeapController.SetPolicyFlags(Controller.PolicyFlag.POLICYBACKGROUNDFRAMES);
		}

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
//			if(FEnableGestures.IsChanged)
//			{
//				foreach (var gestureType in (Gesture.GestureType[])Enum.GetValues(typeof(Gesture.GestureType)))
//				{
//					if(gestureType != Gesture.GestureType.TYPEINVALID)
//						FLeapController.EnableGesture(gestureType, FEnableGestures[0]);
//				}
//			}
			
			if(FResetIn[0])
			{
				FLeapController.Dispose();
				FLeapController = new Controller();
				FLeapController.SetPolicyFlags(Controller.PolicyFlag.POLICYBACKGROUNDFRAMES);
			}
			
			ConfigureGestures();
			
			var frame = FLeapController.Frame();
			
			if(FLeapController.IsConnected && frame.IsValid)
			{
				//gestures
				FGestureOut[0] = FLastFrame != null ? frame.Gestures(FLastFrame) : frame.Gestures();               
                FFrameOut[0] = frame;
				FLastFrame = frame;
			}
		}
		
		void ConfigureGestures()
		{
			var cfg = FLeapController.Config;
			
			//circle
			HandleEnableGesture(FEnableCircleGesture, Gesture.GestureType.TYPECIRCLE);
			HandleGestureConfig(new string[]{"Circle.MinRadius", "Circle.MinArc"}, FCircleMinRadius, FCircleMinArc);
			
			//swipe
			HandleEnableGesture(FEnableSwipeGesture, Gesture.GestureType.TYPESWIPE);
			HandleGestureConfig(new string[]{"Swipe.MinLength", "Swipe.MinVelocity"}, FSwipeMinLength, FSwipeMinVelocity);
			
			//key tab
			HandleEnableGesture(FEnableKeyTabGesture, Gesture.GestureType.TYPEKEYTAP);
			HandleGestureConfig(new string[]{"KeyTap.MinDownVelocity", "KeyTap.HistorySeconds", "KeyTap.MinDistance"}, 
			                    FKeyTabMindDownVelo, FKeyTabHistorySeconds, FKeyTabMindDistance);
			
			//screen tab
			HandleEnableGesture(FEnableScreenTabGesture, Gesture.GestureType.TYPESCREENTAP);
			HandleGestureConfig(new string[]{"ScreenTap.MinForwardVelocity", "ScreenTap.HistorySeconds", "ScreenTap.MinDistance"}, 
			                    FScreenTabMindForwardVelo, FScreenTabHistorySeconds, FScreenTabMindDistance);
		}
		
		//generic enable
		void HandleEnableGesture(IDiffSpread<bool> pin, Gesture.GestureType gestureType)
		{
			if(pin.IsChanged)
				FLeapController.EnableGesture(gestureType, pin[0]);
		}
		
		//generic gesture config
		void HandleGestureConfig(string[] keys, params IDiffSpread<float>[] pins)
		{
			for (int i = 0; i < keys.Length; i++) 
			{
				if(pins[i].IsChanged)
					FLeapController.Config.SetFloat("Gesture." + keys[i], pins[i][0]);
			}	
		}

		public void Dispose()
		{
			FLeapController.Dispose();
		}
	}

	public static class LeapExtensions
	{
		public static Vector3D ToVector3DPos(this Vector v)
		{
			return new Vector3D(v.x * 0.001, v.y * 0.001, v.z * -0.001);
		}
		
		public static Vector3D ToVector3DDir(this Vector v)
		{
			return new Vector3D(v.x, v.y, -v.z);
		}
	}
}




