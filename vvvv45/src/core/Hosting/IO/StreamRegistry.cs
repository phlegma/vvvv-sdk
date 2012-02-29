﻿using System;
using System.Linq;
using SlimDX;
using SlimDX.Direct3D9;
using VVVV.Hosting.IO.Streams;
using VVVV.Hosting.Pins.Input;
using VVVV.Hosting.Pins.Output;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.PluginInterfaces.V2.EX9;
using VVVV.Utils.Streams;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

namespace VVVV.Hosting.IO
{
    unsafe class StreamRegistry : IORegistryBase
    {
        public StreamRegistry()
        {
            int* pLength;
            double** ppDoubleData;
            float** ppFloatData;
            
            RegisterInput(typeof(IInStream<double>), (factory, attribute, t) => {
                              var host = factory.PluginHost;
                              if (attribute.CheckIfChanged)
                              {
                                  var valueIn = host.CreateValueInput(attribute, t);
                                  valueIn.GetValuePointer(out pLength, out ppDoubleData);
                                  var stream = new DoubleInStream(pLength, ppDoubleData, GetValidateFunc(valueIn));
                                  return IOContainer.Create(factory, stream, valueIn);
                              }
                              else
                              {
                                  var valueFastIn = host.CreateValueFastInput(attribute, t);
                                  valueFastIn.GetValuePointer(out pLength, out ppDoubleData);
                                  var stream = new DoubleInStream(pLength, ppDoubleData, GetValidateFunc(valueFastIn, attribute));
                                  return IOContainer.Create(factory, stream, valueFastIn);
                              }
                          });
            
            RegisterInput(typeof(IInStream<float>), (factory, attribute, t) => {
                              var host = factory.PluginHost;
                              if (attribute.CheckIfChanged)
                              {
                                  var valueIn = host.CreateValueInput(attribute, t);
                                  valueIn.GetValuePointer(out pLength, out ppDoubleData);
                                  var stream = new FloatInStream(pLength, ppDoubleData, GetValidateFunc(valueIn));
                                  return IOContainer.Create(factory, stream, valueIn);
                              }
                              else
                              {
                                  var valueFastIn = host.CreateValueFastInput(attribute, t);
                                  valueFastIn.GetValuePointer(out pLength, out ppDoubleData);
                                  var stream = new FloatInStream(pLength, ppDoubleData, GetValidateFunc(valueFastIn, attribute));
                                  return IOContainer.Create(factory, stream, valueFastIn);
                              }
                          });
            
            RegisterInput(typeof(IInStream<int>), (factory, attribute, t) => {
                              var host = factory.PluginHost;
                              if (attribute.CheckIfChanged)
                              {
                                  var valueIn = host.CreateValueInput(attribute, t);
                                  valueIn.GetValuePointer(out pLength, out ppDoubleData);
                                  var stream = new IntInStream(pLength, ppDoubleData, GetValidateFunc(valueIn));
                                  return IOContainer.Create(factory, stream, valueIn);
                              }
                              else
                              {
                                  var valueFastIn = host.CreateValueFastInput(attribute, t);
                                  valueFastIn.GetValuePointer(out pLength, out ppDoubleData);
                                  var stream = new IntInStream(pLength, ppDoubleData, GetValidateFunc(valueFastIn, attribute));
                                  return IOContainer.Create(factory, stream, valueFastIn);
                              }
                          });
            
            RegisterInput(typeof(IInStream<uint>), (factory, attribute, t) => {
                              var host = factory.PluginHost;
                              if (attribute.CheckIfChanged)
                              {
                                  var valueIn = host.CreateValueInput(attribute, t);
                                  valueIn.GetValuePointer(out pLength, out ppDoubleData);
                                  var stream = new UIntInStream(pLength, ppDoubleData, GetValidateFunc(valueIn));
                                  return IOContainer.Create(factory, stream, valueIn);
                              }
                              else
                              {
                                  var valueFastIn = host.CreateValueFastInput(attribute, t);
                                  valueFastIn.GetValuePointer(out pLength, out ppDoubleData);
                                  var stream = new UIntInStream(pLength, ppDoubleData, GetValidateFunc(valueFastIn, attribute));
                                  return IOContainer.Create(factory, stream, valueFastIn);
                              }
                          });
            
            RegisterInput(typeof(IInStream<bool>), (factory, attribute, t) => {
                              var host = factory.PluginHost;
                              if (attribute.CheckIfChanged)
                              {
                                  var valueIn = host.CreateValueInput(attribute, t);
                                  valueIn.GetValuePointer(out pLength, out ppDoubleData);
                                  var stream = new BoolInStream(pLength, ppDoubleData, GetValidateFunc(valueIn));
                                  return IOContainer.Create(factory, stream, valueIn);
                              }
                              else
                              {
                                  var valueFastIn = host.CreateValueFastInput(attribute, t);
                                  valueFastIn.GetValuePointer(out pLength, out ppDoubleData);
                                  var stream = new BoolInStream(pLength, ppDoubleData, GetValidateFunc(valueFastIn, attribute));
                                  return IOContainer.Create(factory, stream, valueFastIn);
                              }
                          });

            RegisterInput(typeof(IInStream<Matrix4x4>), (factory, attribute, t) => {
                              var host = factory.PluginHost;
                              var transformIn = host.CreateTransformInput(attribute, t);
                              transformIn.GetMatrixPointer(out pLength, out ppFloatData);
                              var stream = new Matrix4x4InStream(pLength, (Matrix**) ppFloatData, GetValidateFunc(transformIn));
                              return IOContainer.Create(factory, stream, transformIn);
                          });
            
            RegisterInput(typeof(IInStream<Matrix>), (factory, attribute, t) => {
                              var host = factory.PluginHost;
                              var transformIn = host.CreateTransformInput(attribute, t);
                              transformIn.GetMatrixPointer(out pLength, out ppFloatData);
                              var stream = new MatrixInStream(pLength, (Matrix**) ppFloatData, GetValidateFunc(transformIn));
                              return IOContainer.Create(factory, stream, transformIn);
                          });

            RegisterInput(typeof(IInStream<Vector2D>), (factory, attribute, t) => {
                              var host = factory.PluginHost;
                              var valueFastIn = host.CreateValueFastInput(attribute, t);
                              valueFastIn.GetValuePointer(out pLength, out ppDoubleData);
                              var stream = new Vector2DInStream(pLength, ppDoubleData, GetValidateFunc(valueFastIn, attribute));
                              return IOContainer.Create(factory, stream, valueFastIn);
                          });
            RegisterInput(typeof(IInStream<Vector3D>),(factory, attribute, t) => {
                              var host = factory.PluginHost;
                              var valueFastIn = host.CreateValueFastInput(attribute, t);
                              valueFastIn.GetValuePointer(out pLength, out ppDoubleData);
                              var stream = new Vector3DInStream(pLength, ppDoubleData, GetValidateFunc(valueFastIn, attribute));
                              return IOContainer.Create(factory, stream, valueFastIn);
                          });
            RegisterInput(typeof(IInStream<Vector4D>),(factory, attribute, t) => {
                              var host = factory.PluginHost;
                              var valueFastIn = host.CreateValueFastInput(attribute, t);
                              valueFastIn.GetValuePointer(out pLength, out ppDoubleData);
                              var stream = new Vector4DInStream(pLength, ppDoubleData, GetValidateFunc(valueFastIn, attribute));
                              return IOContainer.Create(factory, stream, valueFastIn);
                          });

            RegisterInput(typeof(IInStream<Vector2>), (factory, attribute, t) => {
                              var host = factory.PluginHost;
                              var valueFastIn = host.CreateValueFastInput(attribute, t);
                              valueFastIn.GetValuePointer(out pLength, out ppDoubleData);
                              var stream = new Vector2InStream(pLength, ppDoubleData, GetValidateFunc(valueFastIn, attribute));
                              return IOContainer.Create(factory, stream, valueFastIn);
                          });
            RegisterInput(typeof(IInStream<Vector3>), (factory, attribute, t) => {
                              var host = factory.PluginHost;
                              var valueFastIn = host.CreateValueFastInput(attribute, t);
                              valueFastIn.GetValuePointer(out pLength, out ppDoubleData);
                              var stream = new Vector3InStream(pLength, ppDoubleData, GetValidateFunc(valueFastIn, attribute));
                              return IOContainer.Create(factory, stream, valueFastIn);
                          });
            RegisterInput(typeof(IInStream<Vector4>), (factory, attribute, t) => {
                              var host = factory.PluginHost;
                              var valueFastIn = host.CreateValueFastInput(attribute, t);
                              valueFastIn.GetValuePointer(out pLength, out ppDoubleData);
                              var stream = new Vector4InStream(pLength, ppDoubleData, GetValidateFunc(valueFastIn, attribute));
                              return IOContainer.Create(factory, stream, valueFastIn);
                          });
            
            RegisterInput(typeof(IInStream<RGBAColor>), (factory, attribute, t) => {
                              var host = factory.PluginHost;
                              var colorIn = host.CreateColorInput(attribute, t);
                              colorIn.GetColorPointer(out pLength, out ppDoubleData);
                              var stream = new ColorInStream(pLength, (RGBAColor**) ppDoubleData, GetValidateFunc(colorIn));
                              return IOContainer.Create(factory, stream, colorIn);
                          });
            
            RegisterInput(typeof(IInStream<Color4>), (factory, attribute, t) => {
                              var host = factory.PluginHost;
                              var colorIn = host.CreateColorInput(attribute, t);
                              colorIn.GetColorPointer(out pLength, out ppDoubleData);
                              var stream = new SlimDXColorInStream(pLength, (RGBAColor**) ppDoubleData, GetValidateFunc(colorIn));
                              return IOContainer.Create(factory, stream, colorIn);
                          });
            
            RegisterInput<BufferedIOStream<string>>(
                (factory, attribute, t) =>
                {
                    var host = factory.PluginHost;
                    var stringIn = host.CreateStringInput(attribute, t);
                    var stream = new StringInStream(stringIn);
                    // Using ManagedIOStream -> needs to be synced on managed side.
                    if (attribute.AutoValidate)
                        return IOContainer.Create(factory, stream, stringIn, s => s.Sync());
                    else
                        return IOContainer.Create(factory, stream, stringIn);
                });

            RegisterInput<BufferedIOStream<EnumEntry>>(
                (factory, attribute, t) =>
                {
                    var host = factory.PluginHost;
                    var enumIn = host.CreateEnumInput(attribute, t);
                    var stream = new DynamicEnumInStream(enumIn, attribute.EnumName);
                    // Using ManagedIOStream -> needs to be synced on managed side.
                    if (attribute.AutoValidate)
                        return IOContainer.Create(factory, stream, enumIn, s => s.Sync());
                    else
                        return IOContainer.Create(factory, stream, enumIn);
                });
            
            // InputIOStream can fullfill this contract a little more memory efficient than BufferedIOStream
            RegisterInput(typeof(IIOStream<>),
                          (factory, attribute, t) =>
                          {
                              var inStreamType = typeof(IInStream<>).MakeGenericType(t);
                              var ioStreamType = typeof(InputIOStream<>).MakeGenericType(t);
                              var inStream = factory.CreateIO(inStreamType, attribute, false);
                              var ioStream = (IIOStream) Activator.CreateInstance(ioStreamType, inStream);
                              if (attribute.AutoValidate)
                                  return IOContainer.Create(factory, ioStream, null, s => s.Sync(), s => s.Flush());
                              else
                                  return IOContainer.Create(factory, ioStream, null, null, s => s.Flush());
                          },
                          false);
            
            RegisterInput(typeof(IInStream<>), (factory, attribute, t) => {
                              var host = factory.PluginHost;
                              if (t.IsGenericType && t.GetGenericArguments().Length == 1)
                              {
                                  if (typeof(IInStream<>).MakeGenericType(t.GetGenericArguments().First()).IsAssignableFrom(t))
                                  {
                                      var multiDimStreamType = typeof(MultiDimInStream<>).MakeGenericType(t.GetGenericArguments().First());
                                      if (attribute.IsPinGroup)
                                      {
                                          multiDimStreamType = typeof(GroupInStream<>).MakeGenericType(t.GetGenericArguments().First());
                                      }
                                      
                                      var stream = Activator.CreateInstance(multiDimStreamType, factory, attribute.Clone()) as IInStream;
                                      
                                      // PinGroup implementation doesn't need to get synced on managed side.
                                      if (!attribute.IsPinGroup && attribute.AutoValidate)
                                          return IOContainer.Create(factory, stream, null, s => s.Sync());
                                      else
                                          return IOContainer.Create(factory, stream, null);
                                  }
                              }
                              
                              {
                                  var nodeIn = host.CreateNodeInput(attribute, t);
                                  var stream = Activator.CreateInstance(typeof(NodeInStream<>).MakeGenericType(t), new object[] { nodeIn }) as IInStream;
                                  return IOContainer.Create(factory, stream, nodeIn);
                              }
                          });
            
            RegisterOutput(typeof(IOutStream<double>), (factory, attribute, t) => {
                               var host = factory.PluginHost;
                               var valueOut = host.CreateValueOutput(attribute, t);
                               valueOut.GetValuePointer(out ppDoubleData);
                               return IOContainer.Create(factory, new DoubleOutStream(ppDoubleData, GetSetValueLengthAction(valueOut)), valueOut);
                           });
            
            RegisterOutput(typeof(IOutStream<float>), (factory, attribute, t) => {
                               var host = factory.PluginHost;
                               var valueOut = host.CreateValueOutput(attribute, t);
                               valueOut.GetValuePointer(out ppDoubleData);
                               return IOContainer.Create(factory, new FloatOutStream(ppDoubleData, GetSetValueLengthAction(valueOut)), valueOut);
                           });
            
            RegisterOutput(typeof(IOutStream<int>), (factory, attribute, t) => {
                               var host = factory.PluginHost;
                               var valueOut = host.CreateValueOutput(attribute, t);
                               valueOut.GetValuePointer(out ppDoubleData);
                               return IOContainer.Create(factory, new IntOutStream(ppDoubleData, GetSetValueLengthAction(valueOut)), valueOut);
                           });
            
            RegisterOutput(typeof(IOutStream<uint>), (factory, attribute, t) => {
                               var host = factory.PluginHost;
                               var valueOut = host.CreateValueOutput(attribute, t);
                               valueOut.GetValuePointer(out ppDoubleData);
                               return IOContainer.Create(factory, new UIntOutStream(ppDoubleData, GetSetValueLengthAction(valueOut)), valueOut);
                           });
            
            RegisterOutput(typeof(IOutStream<bool>), (factory, attribute, t) => {
                               var host = factory.PluginHost;
                               var valueOut = host.CreateValueOutput(attribute, t);
                               valueOut.GetValuePointer(out ppDoubleData);
                               return IOContainer.Create(factory, new BoolOutStream(ppDoubleData, GetSetValueLengthAction(valueOut)), valueOut);
                           });

            RegisterOutput(typeof(IOutStream<Matrix4x4>), (factory, attribute, t) => {
                               var host = factory.PluginHost;
                               var transformOut = host.CreateTransformOutput(attribute, t);
                               transformOut.GetMatrixPointer(out ppFloatData);
                               return IOContainer.Create(factory, new Matrix4x4OutStream((Matrix**) ppFloatData, GetSetMatrixLengthAction(transformOut)), transformOut);
                           });
            
            RegisterOutput(typeof(IOutStream<Matrix>), (factory, attribute, t) => {
                               var host = factory.PluginHost;
                               var transformOut = host.CreateTransformOutput(attribute, t);
                               transformOut.GetMatrixPointer(out ppFloatData);
                               return IOContainer.Create(factory, new MatrixOutStream((Matrix**) ppFloatData, GetSetMatrixLengthAction(transformOut)), transformOut);
                           });

            RegisterOutput(typeof(IOutStream<Vector2D>), (factory, attribute, t) => {
                               var host = factory.PluginHost;
                               var valueOut = host.CreateValueOutput(attribute, t);
                               valueOut.GetValuePointer(out ppDoubleData);
                               return IOContainer.Create(factory, new Vector2DOutStream(ppDoubleData, GetSetValueLengthAction(valueOut)), valueOut);
                           });
            RegisterOutput(typeof(IOutStream<Vector3D>),(factory, attribute, t) => {
                               var host = factory.PluginHost;
                               var valueOut = host.CreateValueOutput(attribute, t);
                               valueOut.GetValuePointer(out ppDoubleData);
                               return IOContainer.Create(factory, new Vector3DOutStream(ppDoubleData, GetSetValueLengthAction(valueOut)), valueOut);
                           });
            RegisterOutput(typeof(IOutStream<Vector4D>),(factory, attribute, t) => {
                               var host = factory.PluginHost;
                               var valueOut = host.CreateValueOutput(attribute, t);
                               valueOut.GetValuePointer(out ppDoubleData);
                               return IOContainer.Create(factory, new Vector4DOutStream(ppDoubleData, GetSetValueLengthAction(valueOut)), valueOut);
                           });

            RegisterOutput(typeof(IOutStream<Vector2>), (factory, attribute, t) => {
                               var host = factory.PluginHost;
                               var valueOut = host.CreateValueOutput(attribute, t);
                               valueOut.GetValuePointer(out ppDoubleData);
                               return IOContainer.Create(factory, new Vector2OutStream(ppDoubleData, GetSetValueLengthAction(valueOut)), valueOut);
                           });
            RegisterOutput(typeof(IOutStream<Vector3>), (factory, attribute, t) => {
                               var host = factory.PluginHost;
                               var valueOut = host.CreateValueOutput(attribute, t);
                               valueOut.GetValuePointer(out ppDoubleData);
                               return IOContainer.Create(factory, new Vector3OutStream(ppDoubleData, GetSetValueLengthAction(valueOut)), valueOut);
                           });
            RegisterOutput(typeof(IOutStream<Vector4>), (factory, attribute, t) => {
                               var host = factory.PluginHost;
                               var valueOut = host.CreateValueOutput(attribute, t);
                               valueOut.GetValuePointer(out ppDoubleData);
                               return IOContainer.Create(factory, new Vector4OutStream(ppDoubleData, GetSetValueLengthAction(valueOut)), valueOut);
                           });

            RegisterOutput(typeof(IOutStream<RGBAColor>), (factory, attribute, t) => {
                               var host = factory.PluginHost;
                               var colorOut = host.CreateColorOutput(attribute, t);
                               colorOut.GetColorPointer(out ppDoubleData);
                               return IOContainer.Create(factory, new ColorOutStream((RGBAColor**) ppDoubleData, GetSetColorLengthAction(colorOut)), colorOut);
                           });
            
            RegisterOutput(typeof(IOutStream<Color4>), (factory, attribute, t) => {
                               var host = factory.PluginHost;
                               var colorOut = host.CreateColorOutput(attribute, t);
                               colorOut.GetColorPointer(out ppDoubleData);
                               return IOContainer.Create(factory, new SlimDXColorOutStream((RGBAColor**) ppDoubleData, GetSetColorLengthAction(colorOut)), colorOut);
                           });

            RegisterOutput(typeof(IOutStream<EnumEntry>), (factory, attribute, t) => {
                               var host = factory.PluginHost;
                               var enumOut = host.CreateEnumOutput(attribute, t);
                               return IOContainer.Create(factory, new DynamicEnumOutStream(enumOut), enumOut, null, s => s.Flush());
                           });
            
            RegisterOutput<BufferedIOStream<string>>(
                (factory, attribute, t) =>
                {
                    var host = factory.PluginHost;
                    var stringOut = host.CreateStringOutput(attribute, t);
                    return IOContainer.Create(factory, new StringOutStream(stringOut), stringOut, null, s => s.Flush());
                });
            
            RegisterOutput(typeof(IOutStream<>), (factory, attribute, t) => {
                               var host = factory.PluginHost;
                               
                               var genericArguments = t.GetGenericArguments();
                               if (t.IsGenericType)
                               {
                                   switch (genericArguments.Length) {
                                       case 1:
                                           if (typeof(IInStream<>).MakeGenericType(genericArguments).IsAssignableFrom(t))
                                           {
                                               var multiDimStreamType = typeof(MultiDimOutStream<>).MakeGenericType(t.GetGenericArguments().First());
                                               var stream = Activator.CreateInstance(multiDimStreamType, factory, attribute.Clone()) as IOutStream;
                                               return IOContainer.Create(factory, stream, null, null, s => s.Flush());
                                           }
                                           break;
                                       case 2:
                                           if (typeof(DXResource<,>).MakeGenericType(genericArguments).IsAssignableFrom(t))
                                           {
                                               var resourceType = genericArguments[0];
                                               var metadataType = genericArguments[1];
                                               if (resourceType == typeof(Texture))
                                               {
                                                   var textureOutStreamType = typeof(TextureOutStream<,>);
                                                   textureOutStreamType = textureOutStreamType.MakeGenericType(t, metadataType);
                                                   var stream = Activator.CreateInstance(textureOutStreamType, host, attribute) as IOutStream;
                                                   return IOContainer.Create(factory, stream, null, null, s => s.Flush());
                                               }
                                               else
                                               {
                                                   throw new NotImplementedException();
                                               }
                                           }
                                           break;
                                   }
                               }
                               
                               {
                                   var nodeOut = host.CreateNodeOutput(attribute, t);
                                   var stream = Activator.CreateInstance(typeof(NodeOutStream<>).MakeGenericType(t), new object[] { nodeOut }) as IOutStream;
                                   return IOContainer.Create(factory, stream, nodeOut, null, s => s.Flush());
                               }
                           });
            
            RegisterOutput(typeof(IInStream<>), (factory, attribute, t) => {
                               var host = factory.PluginHost;
                               if (t.IsGenericType)
                               {
                                   if (typeof(IOutStream<>).MakeGenericType(t.GetGenericArguments()).IsAssignableFrom(t))
                                   {
                                       var multiDimStreamType = typeof(GroupOutStream<>).MakeGenericType(t.GetGenericArguments().First());
                                       if (!attribute.IsPinGroup)
                                       {
                                           throw new NotSupportedException("IInStream<IOutStream<T>> can only be used as a pin group.");
                                       }
                                       
                                       var stream = Activator.CreateInstance(multiDimStreamType, factory, attribute.Clone()) as IFlushable;
                                       return IOContainer.Create(factory, stream, null, null, s => s.Flush());
                                   }
                               }
                               
                               return null; // IOFactory will throw a NotSupportedException with a few more details.
                           });
            
            RegisterOutput(typeof(IIOStream<>),
                           (factory, attribute, t) =>
                           {
                               var outStreamType = typeof(IOutStream<>).MakeGenericType(t);
                               var ioStreamType = typeof(BufferedOutputIOStream<>).MakeGenericType(t);
                               var outStream = factory.CreateIO(outStreamType, attribute, false);
                               var ioStream = (IIOStream) Activator.CreateInstance(ioStreamType, outStream);
                               return IOContainer.Create(factory, ioStream, null, null, s => s.Flush());
                           },
                           false);
            
            RegisterConfig(typeof(BufferedIOStream<string>), (factory, attribute, t) => {
                               var host = factory.PluginHost;
                               var stringConfig = host.CreateStringConfig(attribute, t);
                               return IOContainer.Create(factory, new StringConfigStream(stringConfig), stringConfig, null, s => s.Flush(), s => s.Sync());
                           });
            
            RegisterConfig(typeof(BufferedIOStream<RGBAColor>), (factory, attribute, t) => {
                               var host = factory.PluginHost;
                               var colorConfig = host.CreateColorConfig(attribute, t);
                               var stream = new ColorConfigStream(colorConfig);
                               return IOContainer.Create(factory, stream, colorConfig, null, s => s.Flush(), s => s.Sync());
                           });
            
            RegisterConfig(typeof(BufferedIOStream<Color4>), (factory, attribute, t) => {
                               var host = factory.PluginHost;
                               var colorConfig = host.CreateColorConfig(attribute, t);
                               var stream = new SlimDXColorConfigStream(colorConfig);
                               return IOContainer.Create(factory, stream, colorConfig, null, s => s.Flush(), s => s.Sync());
                           });

            RegisterConfig(typeof(BufferedIOStream<EnumEntry>), (factory, attribute, t) => {
                               var host = factory.PluginHost;
                               var enumConfig = host.CreateEnumConfig(attribute, t);
                               return IOContainer.Create(factory, new DynamicEnumConfigStream(enumConfig), enumConfig, null, s => s.Flush(), s => s.Sync());
                           });
            
            RegisterConfig(typeof(BufferedIOStream<>), (factory, attribute, t) => {
                               var host = factory.PluginHost;
                               if (t.IsPrimitive)
                               {
                                   var valueConfig = host.CreateValueConfig(attribute, t);
                                   var streamType = typeof(ValueConfigStream<>).MakeGenericType(t);
                                   var stream = Activator.CreateInstance(streamType, new object[] { valueConfig }) as IIOStream;
                                   return IOContainer.Create(factory, stream, valueConfig, null, s => s.Flush(), s => s.Sync());
                               }
                               throw new NotSupportedException(string.Format("Config pin of type '{0}' is not supported.", t));
                           });
        }
        
        static private Func<bool> GetValidateFunc(IPluginIn pluginIn)
        {
            return () => { return pluginIn.Validate(); };
        }
        
        static private Func<bool> GetValidateFunc(IPluginFastIn pluginFastIn, InputAttribute attribute)
        {
            if (attribute.AutoValidate)
            {
                return () => { return true; };
            }
            return () => { return pluginFastIn.Validate(); };
        }
        
        static private Action<int> GetSetValueLengthAction(IValueOut valueOut)
        {
            return (newLength) =>
            {
                valueOut.SliceCount = newLength;
            };
        }
        
        static private Action<int> GetSetColorLengthAction(IColorOut colorOut)
        {
            return (newLength) =>
            {
                colorOut.SliceCount = newLength;
            };
        }
        
        static private Action<int> GetSetMatrixLengthAction(ITransformOut transformOut)
        {
            return (newLength) =>
            {
                transformOut.SliceCount = newLength;
            };
        }
        
        static private Func<bool> GetValidateFunc(IValueConfig valueConfig)
        {
            return () => { return valueConfig.PinIsChanged; };
        }
        
        static private Func<bool> GetValidateFunc(IColorConfig colorConfig)
        {
            return () => { return colorConfig.PinIsChanged; };
        }
        
        static private Action<int> GetSetValueLengthAction(IValueConfig valueConfig)
        {
            return (int newLength) => {
                valueConfig.SliceCount = newLength;
            };
        }
        
        static private Action<int> GetSetColorLengthAction(IColorConfig colorConfig)
        {
            return (int newLength) => {
                colorConfig.SliceCount = newLength;
            };
        }
    }
}
