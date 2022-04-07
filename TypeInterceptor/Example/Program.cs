using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.TypeInterceptor;
using System;
using System.Threading.Tasks;

namespace Example
{
    public class NormalInterceptor : Interceptor
    {
        public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            Console.WriteLine("Normal Interceptor At Client");
            return base.BlockingUnaryCall(request, context, continuation);
        }

        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            Console.WriteLine("Normal Interceptor At Server");
            return base.UnaryServerHandler(request, context, continuation);
        }
    }

    public class TypeInterceptor : TypeInterceptor<Message1,Message2>
    {
        public override Message2 BlockingUnaryCall(Message1 request, ClientInterceptorContext<Message1, Message2> context, Interceptor.BlockingUnaryCallContinuation<Message1, Message2> continuation)
        {
            Console.WriteLine("Type Interceptor At Client");
            return base.BlockingUnaryCall(request, context, continuation);
        }

        public override Task<Message2> UnaryServerHandler(Message1 request, ServerCallContext context, UnaryServerMethod<Message1, Message2> continuation)
        {
            Console.WriteLine("Type Interceptor At Server");
            return base.UnaryServerHandler(request, context, continuation);
        }
    }

    public class APIService : API.APIBase
    {
        public override Task<Message2> API1(Message1 request, ServerCallContext context)
        {
            Console.WriteLine($"API1 {request.Msg}");
            return Task.FromResult(new Message2() { Msg = $"Receive {request.Msg}" });
        }

        public override Task<Message4> API2(Message3 request, ServerCallContext context)
        {
            Console.WriteLine($"API2 {request.Msg}");
            return Task.FromResult(new Message4() { Msg = $"Receive {request.Msg}" });
        }
    }
    class Program
    {
        static async Task Main(string[] args)
        {
            var normal = new NormalInterceptor();
            var type = new TypeInterceptor();
            _ = Task.Run(() =>
            {
                var service = new APIService();
                var Server = new Server()
                {
                    Services = { API.BindService(service).TypeIntercept(normal, type) },
                    Ports = { new ServerPort("127.0.0.1", 5000, ServerCredentials.Insecure)}
                };
                Server.Start();
            });
            _ = Task.Run(async () =>
            {
                await Task.Delay(1000);
                var channel = new Channel("127.0.0.1", 5000, ChannelCredentials.Insecure);
                var client = new API.APIClient(channel.CreateCallInvoker().TypeIntercept(normal,type));
                var api1rp = client.API1(new Message1() { Msg = "Call API1" });
                Console.WriteLine(api1rp.Msg);
                Console.WriteLine("----------");
                var api2rp = client.API2(new Message3() { Msg = "Call API2" });
                Console.WriteLine(api2rp.Msg);
            });
            await Task.Delay(TimeSpan.FromDays(1));
        }
    }
}
