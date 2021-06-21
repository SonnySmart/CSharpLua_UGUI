using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LuaFramework;

public class SocketCommand : SimpleCommand {

    public override void Execute(IMessage message) {
        object data = message.Body;
        if (data == null) return;
        KeyValuePair<int, ByteBuffer> buffer = (KeyValuePair<int, ByteBuffer>)data;
        switch (buffer.Key) {
            case Protocal.Connect: { OnConnect(buffer.Value); } break;
            case Protocal.Exception: { OnException(buffer.Value); } break;
            case Protocal.Disconnect: { OnDisconnect(buffer.Value); } break;
            //default: Util.CallMethod("Network", "OnSocket", buffer.Key, buffer.Value); break;
            default: { OnMessage(buffer.Key, buffer.Value); } break;
        }
	}

    void OnConnect(ByteBuffer buffer)
    {
        int state = buffer.ReadInt();
        string message = buffer.ReadString();
        if (state != 0)
        {
            Debug.LogError(message);
            return;
        }

        Debug.Log(message);
    }

    void OnException(ByteBuffer buffer)
    {
        int state = buffer.ReadInt();
        string message = buffer.ReadString();
        Debug.LogError(message);

        // 重新链接
        LuaHelper.GetNetManager().SendConnect();
    }

    void OnDisconnect(ByteBuffer buffer)
    {
        int state = buffer.ReadInt();
        string message = buffer.ReadString();
        Debug.LogError(message);
    }

    void OnMessage(int protocal, ByteBuffer buffer)
    {
        // todo:消息分发
    }
}
