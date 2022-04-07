// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: proto/API.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

public static partial class API
{
  static readonly string __ServiceName = "API";

  static readonly grpc::Marshaller<global::Message1> __Marshaller_Message1 = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Message1.Parser.ParseFrom);
  static readonly grpc::Marshaller<global::Message2> __Marshaller_Message2 = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Message2.Parser.ParseFrom);
  static readonly grpc::Marshaller<global::Message3> __Marshaller_Message3 = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Message3.Parser.ParseFrom);
  static readonly grpc::Marshaller<global::Message4> __Marshaller_Message4 = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Message4.Parser.ParseFrom);

  static readonly grpc::Method<global::Message1, global::Message2> __Method_API1 = new grpc::Method<global::Message1, global::Message2>(
      grpc::MethodType.Unary,
      __ServiceName,
      "API1",
      __Marshaller_Message1,
      __Marshaller_Message2);

  static readonly grpc::Method<global::Message3, global::Message4> __Method_API2 = new grpc::Method<global::Message3, global::Message4>(
      grpc::MethodType.Unary,
      __ServiceName,
      "API2",
      __Marshaller_Message3,
      __Marshaller_Message4);

  /// <summary>Service descriptor</summary>
  public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
  {
    get { return global::APIReflection.Descriptor.Services[0]; }
  }

  /// <summary>Base class for server-side implementations of API</summary>
  [grpc::BindServiceMethod(typeof(API), "BindService")]
  public abstract partial class APIBase
  {
    public virtual global::System.Threading.Tasks.Task<global::Message2> API1(global::Message1 request, grpc::ServerCallContext context)
    {
      throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
    }

    public virtual global::System.Threading.Tasks.Task<global::Message4> API2(global::Message3 request, grpc::ServerCallContext context)
    {
      throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
    }

  }

  /// <summary>Client for API</summary>
  public partial class APIClient : grpc::ClientBase<APIClient>
  {
    /// <summary>Creates a new client for API</summary>
    /// <param name="channel">The channel to use to make remote calls.</param>
    public APIClient(grpc::ChannelBase channel) : base(channel)
    {
    }
    /// <summary>Creates a new client for API that uses a custom <c>CallInvoker</c>.</summary>
    /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
    public APIClient(grpc::CallInvoker callInvoker) : base(callInvoker)
    {
    }
    /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
    protected APIClient() : base()
    {
    }
    /// <summary>Protected constructor to allow creation of configured clients.</summary>
    /// <param name="configuration">The client configuration.</param>
    protected APIClient(ClientBaseConfiguration configuration) : base(configuration)
    {
    }

    public virtual global::Message2 API1(global::Message1 request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
    {
      return API1(request, new grpc::CallOptions(headers, deadline, cancellationToken));
    }
    public virtual global::Message2 API1(global::Message1 request, grpc::CallOptions options)
    {
      return CallInvoker.BlockingUnaryCall(__Method_API1, null, options, request);
    }
    public virtual grpc::AsyncUnaryCall<global::Message2> API1Async(global::Message1 request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
    {
      return API1Async(request, new grpc::CallOptions(headers, deadline, cancellationToken));
    }
    public virtual grpc::AsyncUnaryCall<global::Message2> API1Async(global::Message1 request, grpc::CallOptions options)
    {
      return CallInvoker.AsyncUnaryCall(__Method_API1, null, options, request);
    }
    public virtual global::Message4 API2(global::Message3 request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
    {
      return API2(request, new grpc::CallOptions(headers, deadline, cancellationToken));
    }
    public virtual global::Message4 API2(global::Message3 request, grpc::CallOptions options)
    {
      return CallInvoker.BlockingUnaryCall(__Method_API2, null, options, request);
    }
    public virtual grpc::AsyncUnaryCall<global::Message4> API2Async(global::Message3 request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
    {
      return API2Async(request, new grpc::CallOptions(headers, deadline, cancellationToken));
    }
    public virtual grpc::AsyncUnaryCall<global::Message4> API2Async(global::Message3 request, grpc::CallOptions options)
    {
      return CallInvoker.AsyncUnaryCall(__Method_API2, null, options, request);
    }
    /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
    protected override APIClient NewInstance(ClientBaseConfiguration configuration)
    {
      return new APIClient(configuration);
    }
  }

  /// <summary>Creates service definition that can be registered with a server</summary>
  /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
  public static grpc::ServerServiceDefinition BindService(APIBase serviceImpl)
  {
    return grpc::ServerServiceDefinition.CreateBuilder()
        .AddMethod(__Method_API1, serviceImpl.API1)
        .AddMethod(__Method_API2, serviceImpl.API2).Build();
  }

  /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
  /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
  /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
  /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
  public static void BindService(grpc::ServiceBinderBase serviceBinder, APIBase serviceImpl)
  {
    serviceBinder.AddMethod(__Method_API1, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Message1, global::Message2>(serviceImpl.API1));
    serviceBinder.AddMethod(__Method_API2, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Message3, global::Message4>(serviceImpl.API2));
  }

}
#endregion