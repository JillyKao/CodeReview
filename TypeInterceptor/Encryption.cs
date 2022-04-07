using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grpc.Encryption
{
    public interface IEncryption
    {
        byte[] Encryption(byte[] bin);
        byte[] Decrypt(byte[] bin);
    }

    public static class Extensisons
    {
        internal static Method<TRequest, TResponse> Encryption<TRequest, TResponse>(this Method<TRequest, TResponse> method, IEncryption encryption)
            where TRequest : class
            where TResponse : class
        {
            var rq = Marshallers.Create(obj => encryption.Encryption(method.RequestMarshaller.Serializer(obj)), bin => method.RequestMarshaller.Deserializer(encryption.Decrypt(bin)));
            var rp = Marshallers.Create(obj => encryption.Encryption(method.ResponseMarshaller.Serializer(obj)), bin => method.ResponseMarshaller.Deserializer(encryption.Decrypt(bin)));
            return new Method<TRequest, TResponse>(method.Type, method.ServiceName, method.Name, rq, rp);
        }

        public static ServerServiceDefinition Encryption(this ServerServiceDefinition serverServiceDefinition, IEncryption encryption)
        {
            var binder = new EncryptionServiceBinder(encryption);
            TypeInterceptor.Extensions.BindServiceMethodd.Invoke(serverServiceDefinition, new object[] { binder });
            return binder.Build();
        }

        internal class EncryptionCallInvoker : CallInvoker
        {
            private IEncryption Encryption;
            private CallInvoker CallInvoker;
            public EncryptionCallInvoker(CallInvoker callInvoker, IEncryption encryption)
            {
                Encryption = encryption;
                CallInvoker = callInvoker;
            }

            public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options)
                => CallInvoker.AsyncClientStreamingCall(method.Encryption(Encryption), host, options);

            public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options)
                => CallInvoker.AsyncDuplexStreamingCall(method.Encryption(Encryption), host, options);

            public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options, TRequest request)
                => CallInvoker.AsyncServerStreamingCall(method.Encryption(Encryption), host, options, request);

            public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options, TRequest request)
                => CallInvoker.AsyncUnaryCall(method.Encryption(Encryption), host, options, request);

            public override TResponse BlockingUnaryCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options, TRequest request)
                => CallInvoker.BlockingUnaryCall(method.Encryption(Encryption), host, options, request);
        }

        internal class EncryptionServiceBinder : ServiceBinderBase
        {
            private ServerServiceDefinition.Builder Builder = new ServerServiceDefinition.Builder();
            private IEncryption Encryption;
            public EncryptionServiceBinder(IEncryption encryption)
            {
                Encryption = encryption;
            }
            public ServerServiceDefinition Build() => Builder.Build();

            public override void AddMethod<TRequest, TResponse>(Method<TRequest, TResponse> method, ClientStreamingServerMethod<TRequest, TResponse> handler)
                => Builder.AddMethod(method.Encryption(Encryption), handler);
            public override void AddMethod<TRequest, TResponse>(Method<TRequest, TResponse> method, DuplexStreamingServerMethod<TRequest, TResponse> handler)
                => Builder.AddMethod(method.Encryption(Encryption), handler);
            public override void AddMethod<TRequest, TResponse>(Method<TRequest, TResponse> method, ServerStreamingServerMethod<TRequest, TResponse> handler)
                => Builder.AddMethod(method.Encryption(Encryption), handler);
            public override void AddMethod<TRequest, TResponse>(Method<TRequest, TResponse> method, UnaryServerMethod<TRequest, TResponse> handler)
                => Builder.AddMethod(method.Encryption(Encryption), handler);
        }
    }
}
