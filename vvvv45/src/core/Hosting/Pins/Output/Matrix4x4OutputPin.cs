﻿
using System;
using System.Runtime.InteropServices;
using SlimDX;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VMath;

namespace VVVV.Hosting.Pins.Output
{
	public class Matrix4x4OutputPin : Pin<Matrix4x4>, IPinUpdater
	{
		protected ITransformOut FTransformOut;
		protected float[] FData;
		protected int FSliceCount;
		
		public Matrix4x4OutputPin(IPluginHost host, OutputAttribute attribute)
			: base(host, attribute)
		{
			host.CreateTransformOutput(attribute.Name, (TSliceMode)attribute.SliceMode, (TPinVisibility)attribute.Visibility, out FTransformOut);
			
			FTransformOut.SetPinUpdater(this);
			
			SliceCount = 1;
		}
		
		public override IPluginIO PluginIO 
		{
			get
			{
				return FTransformOut;
			}
		}
		
		public override int SliceCount 
		{
			get 
			{
				return FSliceCount;
			}
			set 
			{
				if (FSliceCount != value)
					FData = new float[value * 16];
				
				FSliceCount = value;
				
				if (FAttribute.SliceMode != SliceMode.Single)
					FTransformOut.SliceCount = value;
			}
		}
		
		unsafe public override Matrix4x4 this[int index] 
		{
			get 
			{
				throw new NotImplementedException();
			}
			set 
			{
				fixed (float* ptr = FData)
				{
					((Matrix*)ptr)[index % FSliceCount] = value.ToSlimDXMatrix();
				}
			}
		}
		
		unsafe public override void Update()
		{
			base.Update();
			
			float* destination;
			FTransformOut.GetMatrixPointer(out destination);
			
			if (FSliceCount > 0)
				Marshal.Copy(FData, 0, new IntPtr(destination), FData.Length);
		}
	}
}