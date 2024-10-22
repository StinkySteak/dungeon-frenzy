using Netick;
using System;
using UnityEngine;

namespace StinkySteak.N2D
{
    public unsafe class WebRTCTransportConnection : TransportConnection
    {
        public NetickWebRTCTransport Transport;
        public int MaxPayloadSize;

        public override IEndPoint EndPoint => throw new NotImplementedException();

        public override int Mtu => MaxPayloadSize;

        public override void Send(IntPtr ptr, int length)
        {

        }

        public WebRTCTransportConnection(NetickWebRTCTransport transport)
        {
            Transport = transport;
        }
    }
    public unsafe class NetickWebRTCTransport : NetworkTransport
    {
        public override void Connect(string address, int port, byte[] connectionData, int connectionDataLength)
        {
            throw new NotImplementedException();
        }

        public override void Disconnect(TransportConnection connection)
        {
            throw new NotImplementedException();
        }

        public override void PollEvents()
        {
            throw new NotImplementedException();
        }

        public override void Run(RunMode mode, int port)
        {
            throw new NotImplementedException();
        }

        public override void Shutdown()
        {
            throw new NotImplementedException();
        }

        public struct NetickUnityTransportEndPoint : IEndPoint
        {
            public string IPAddress => throw new NotImplementedException();

            public int Port => throw new NotImplementedException();
        }
    }
}
