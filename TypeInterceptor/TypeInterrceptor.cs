using Grpc.Core;
using Grpc.Core.Interceptors;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using static Grpc.Core.Interceptors.Interceptor;

namespace Grpc.TypeInterceptor
{
    public enum BWList
    {
        Black,
        White,
    }
    internal sealed class InterceptorBinder : ServiceBinder
    {
        public InterceptorBinder(Interceptor interceptor)
        {
            InterceptorInstance = interceptor;
        }
    }
    public class TypeInterceptor : ServiceBinder
    {
        public TypeInterceptor()
        {
            InterceptorInstance = this;
        }
    }
    public interface ICallInvoker
    {
        public BWList BlackWhiteList { get; set; }

        public HashSet<string> IgnoreInterceptor { get; set; }

        public object InterceptorInstance { get; set; }

        public CallInvoker Invoker { get; set; }

        public bool CheckIgnore(string fullName);
        public AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options)
            where TRequest : class
            where TResponse : class
            => CheckIgnore(method.FullName) switch
            {
                true => Invoker.AsyncClientStreamingCall(method, host, options),
                false => InterceptorInstance switch
                {
                    TypeInterceptor<TRequest, TResponse> typeInterceptor => typeInterceptor.AsyncClientStreamingCall(
                        new ClientInterceptorContext<TRequest, TResponse>(method, host, options),
                         ctx => Invoker.AsyncClientStreamingCall(ctx.Method, ctx.Host, ctx.Options)),
                    Interceptor interceptor => interceptor.AsyncClientStreamingCall(
                        new ClientInterceptorContext<TRequest, TResponse>(method, host, options),
                         ctx => Invoker.AsyncClientStreamingCall(ctx.Method, ctx.Host, ctx.Options)),
                    _ => Invoker.AsyncClientStreamingCall(method, host, options)
                }
            };

        public AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options)
            where TRequest : class
            where TResponse : class
            => CheckIgnore(method.FullName) switch
            {
                true => Invoker.AsyncDuplexStreamingCall(method, host, options),
                false => InterceptorInstance switch
                {
                    TypeInterceptor<TRequest, TResponse> typeInterceptor => typeInterceptor.AsyncDuplexStreamingCall(
                        new ClientInterceptorContext<TRequest, TResponse>(method, host, options),
                         ctx => Invoker.AsyncDuplexStreamingCall(ctx.Method, ctx.Host, ctx.Options)),
                    Interceptor interceptor => interceptor.AsyncDuplexStreamingCall(
                        new ClientInterceptorContext<TRequest, TResponse>(method, host, options),
                         ctx => Invoker.AsyncDuplexStreamingCall(ctx.Method, ctx.Host, ctx.Options)),
                    _ => Invoker.AsyncDuplexStreamingCall(method, host, options)
                }
            };

        public AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options, TRequest request)
            where TRequest : class
            where TResponse : class
            => CheckIgnore(method.FullName) switch
            {
                true => Invoker.AsyncServerStreamingCall(method, host, options, request),
                false => InterceptorInstance switch
                {
                    TypeInterceptor<TRequest, TResponse> typeInterceptor => typeInterceptor.AsyncServerStreamingCall(
                        request,
                        new ClientInterceptorContext<TRequest, TResponse>(method, host, options),
                        (req, ctx) => Invoker.AsyncServerStreamingCall(ctx.Method, ctx.Host, ctx.Options, req)),
                    Interceptor interceptor => interceptor.AsyncServerStreamingCall(
                        request,
                        new ClientInterceptorContext<TRequest, TResponse>(method, host, options),
                        (req, ctx) => Invoker.AsyncServerStreamingCall(ctx.Method, ctx.Host, ctx.Options, req)),
                    _ => Invoker.AsyncServerStreamingCall(method, host, options, request)
                }
            };

        public AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options, TRequest request)
            where TRequest : class
            where TResponse : class
            => CheckIgnore(method.FullName) switch
            {
                true => Invoker.AsyncUnaryCall(method, host, options, request),
                false => InterceptorInstance switch
                {
                    TypeInterceptor<TRequest, TResponse> typeInterceptor => typeInterceptor.AsyncUnaryCall(
                        request,
                        new ClientInterceptorContext<TRequest, TResponse>(method, host, options),
                        (req, ctx) => Invoker.AsyncUnaryCall(ctx.Method, ctx.Host, ctx.Options, req)),
                    Interceptor interceptor => interceptor.AsyncUnaryCall(
                        request,
                        new ClientInterceptorContext<TRequest, TResponse>(method, host, options),
                        (req, ctx) => Invoker.AsyncUnaryCall(ctx.Method, ctx.Host, ctx.Options, req)),
                    _ => Invoker.AsyncUnaryCall(method, host, options, request)
                }
            };

        public TResponse BlockingUnaryCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options, TRequest request)
            where TRequest : class
            where TResponse : class
            => CheckIgnore(method.FullName) switch
            {
                true => Invoker.BlockingUnaryCall(method, host, options, request),
                false => InterceptorInstance switch
                {
                    TypeInterceptor<TRequest, TResponse> typeInterceptor => typeInterceptor.BlockingUnaryCall(
                        request,
                        new ClientInterceptorContext<TRequest, TResponse>(method, host, options),
                        (req, ctx) => Invoker.BlockingUnaryCall(ctx.Method, ctx.Host, ctx.Options, req)),
                    Interceptor interceptor => interceptor.BlockingUnaryCall(
                        request,
                        new ClientInterceptorContext<TRequest, TResponse>(method, host, options),
                        (req, ctx) => Invoker.BlockingUnaryCall(ctx.Method, ctx.Host, ctx.Options, req)),
                    _ => Invoker.BlockingUnaryCall(method, host, options, request)
                }
            };
    }
    public class TypeInterceptor<TRequest, TResponse> : TypeInterceptor
        where TRequest : class
        where TResponse : class
    {
        #region CallerInvoker
        public virtual AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall(ClientInterceptorContext<TRequest, TResponse> context, AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            return continuation(context);
        }
        public virtual AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall(ClientInterceptorContext<TRequest, TResponse> context, AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            return continuation(context);
        }
        public virtual AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            return continuation(request, context);
        }
        public virtual AsyncUnaryCall<TResponse> AsyncUnaryCall(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            return continuation(request, context);
        }
        public virtual TResponse BlockingUnaryCall(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            return continuation(request, context);
        }
        #endregion

        #region BindService
        public virtual Task<TResponse> ClientStreamingServerHandler(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context, ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            return continuation(requestStream, context);
        }
        public virtual Task DuplexStreamingServerHandler(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            return continuation(requestStream, responseStream, context);
        }
        public virtual Task ServerStreamingServerHandler(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            return continuation(request, responseStream, context);
        }
        public virtual Task<TResponse> UnaryServerHandler(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            return continuation(request, context);
        }
        #endregion
    }
    public class ServiceBinder : ServiceBinderBase, ICallInvoker
    {
        public BWList BlackWhiteList { get; set; }

        public HashSet<string> IgnoreInterceptor { get; set; } = new HashSet<string>();

        public ServerServiceDefinition.Builder Builder { get; set; } = new ServerServiceDefinition.Builder();

        public object InterceptorInstance { get; set; }

        public CallInvoker Invoker { get; set; }

        public ServerServiceDefinition ServiceBuild() => Builder.Build();

        public bool CheckIgnore(string fullName) => BlackWhiteList switch
        {
            BWList.Black => IgnoreInterceptor.Contains(fullName),
            BWList.White => !IgnoreInterceptor.Contains(fullName),
            _ => true
        };

        public override void AddMethod<TRequest, TResponse>(Method<TRequest, TResponse> method, UnaryServerMethod<TRequest, TResponse> handler)
        {
            _ = CheckIgnore(method.FullName) switch
            {
                true => Builder.AddMethod(method, handler),
                false => InterceptorInstance switch
                {
                    TypeInterceptor<TRequest, TResponse> typeInterceptor => Builder.AddMethod(method, (request, context) => typeInterceptor.UnaryServerHandler(request, context, handler)),
                    Interceptor interceptor => Builder.AddMethod(method, (request, context) => interceptor.UnaryServerHandler(request, context, handler)),
                    _ => Builder.AddMethod(method, handler)
                }
            };
        }
        public override void AddMethod<TRequest, TResponse>(Method<TRequest, TResponse> method, ClientStreamingServerMethod<TRequest, TResponse> handler)
        {
            _ = CheckIgnore(method.FullName) switch
            {
                true => Builder.AddMethod(method, handler),
                false => InterceptorInstance switch
                {
                    TypeInterceptor<TRequest, TResponse> typeInterceptor => Builder.AddMethod(method, (request, context) => typeInterceptor.ClientStreamingServerHandler(request, context, handler)),
                    Interceptor interceptor => Builder.AddMethod(method, (request, context) => interceptor.ClientStreamingServerHandler(request, context, handler)),
                    _ => Builder.AddMethod(method, handler)
                }
            };
        }
        public override void AddMethod<TRequest, TResponse>(Method<TRequest, TResponse> method, DuplexStreamingServerMethod<TRequest, TResponse> handler)
        {
            _ = CheckIgnore(method.FullName) switch
            {
                true => Builder.AddMethod(method, handler),
                false => InterceptorInstance switch
                {
                    TypeInterceptor<TRequest, TResponse> typeInterceptor => Builder.AddMethod(method, (requestStream, responseStream, context) => typeInterceptor.DuplexStreamingServerHandler(requestStream, responseStream, context, handler)),
                    Interceptor interceptor => Builder.AddMethod(method, (requestStream, responseStream, context) => interceptor.DuplexStreamingServerHandler(requestStream, responseStream, context, handler)),
                    _ => Builder.AddMethod(method, handler)
                }
            };
        }

        public override void AddMethod<TRequest, TResponse>(Method<TRequest, TResponse> method, ServerStreamingServerMethod<TRequest, TResponse> handler)
        {
            _ = CheckIgnore(method.FullName) switch
            {
                true => Builder.AddMethod(method, handler),
                false => InterceptorInstance switch
                {
                    TypeInterceptor<TRequest, TResponse> typeInterceptor => Builder.AddMethod(method, (request, responseStream, context) => typeInterceptor.ServerStreamingServerHandler(request, responseStream, context, handler)),
                    Interceptor interceptor => Builder.AddMethod(method, (request, responseStream, context) => interceptor.ServerStreamingServerHandler(request, responseStream, context, handler)),
                    _ => Builder.AddMethod(method, handler)
                }
            };
        }
    }

    public static class Extensions
    {
        internal readonly static MethodInfo BindServiceMethodd = typeof(ServerServiceDefinition).GetMethod("BindService", BindingFlags.Instance | BindingFlags.NonPublic);

        public static ServerServiceDefinition TypeIntercept(this ServerServiceDefinition serverServiceDefinition, params object[] interceptors)
        {
            foreach (var interceptor in interceptors.Reverse())
                serverServiceDefinition = serverServiceDefinition.TypeIntercept(interceptor);
            return serverServiceDefinition;
        }

        public static ServerServiceDefinition TypeIntercept(this ServerServiceDefinition serverServiceDefinition, object interceptor)
        {
            return interceptor switch
            {
                ServiceBinder binder => build(binder),
                Interceptor obj => build(new InterceptorBinder(obj)),
                _ => serverServiceDefinition
            };

            ServerServiceDefinition build(ServiceBinder o)
            {
                BindServiceMethodd.Invoke(serverServiceDefinition, new object[] { o });
                return o.ServiceBuild();
            }
        }
        public static CallInvoker TypeIntercept(this CallInvoker callInvoker, params object[] interceptors)
        {
            foreach (var interceptor in interceptors.Reverse())
                callInvoker = callInvoker.TypeIntercept(interceptor);
            return callInvoker;
        }

        public static CallInvoker TypeIntercept(this CallInvoker callInvoker, object interceptor)
        {
            return interceptor switch
            {
                ServiceBinder binder => new TypeInterceptorCallInvoker(callInvoker, binder),
                Interceptor obj => new TypeInterceptorCallInvoker(callInvoker, new InterceptorBinder(obj)),
                _ => callInvoker
            };
        }
        private class TypeInterceptorCallInvoker : CallInvoker
        {
            private readonly ICallInvoker Invoker;
            public TypeInterceptorCallInvoker(CallInvoker callInvoker, ICallInvoker invoker)
            {
                Invoker = invoker;
                Invoker.Invoker = callInvoker;
            }
            public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options)
            => Invoker.AsyncClientStreamingCall(method, host, options);

            public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options)
            => Invoker.AsyncDuplexStreamingCall(method, host, options);

            public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options, TRequest request)
            => Invoker.AsyncServerStreamingCall(method, host, options, request);

            public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options, TRequest request)
            => Invoker.AsyncUnaryCall(method, host, options, request);

            public override TResponse BlockingUnaryCall<TRequest, TResponse>(Method<TRequest, TResponse> method, string host, CallOptions options, TRequest request)
            => Invoker.BlockingUnaryCall(method, host, options, request);
        }
    }
}
