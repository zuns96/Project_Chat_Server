using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using static ASPDotNetCore.WSPacket;

namespace ASPDotNetCore
{
    public class ChatManager
    {
        static ChatManager s_instance = null;

        WebSocketOptions m_webSocketOptions = null;

        List<ChatUser> m_lstUsers = null;

        object m_objLock = new object();

        static public void Create(IApplicationBuilder app)
        {
            if (s_instance == null)
                s_instance = new ChatManager(app);
        }

        static public void Release()
        {
            if(s_instance != null)
            {
                s_instance.release();
                s_instance = null;
            }
        }

        static public WebSocketOptions GetWebSocketOption()
        {
            if (s_instance != null)
                return s_instance.m_webSocketOptions;
            return null;
        }

        static public void DisposeUser(ChatUser chatUser)
        {
            if (s_instance != null)
                s_instance.disposeUser(chatUser);
        }

        ChatManager(IApplicationBuilder app)
        {
            m_lstUsers = new List<ChatUser>();

            m_webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
            };

            app.UseWebSockets(m_webSocketOptions);
            
            app.Use(ProcessWebSocket);
        }

        void release()
        {
            m_lstUsers.Clear();
            m_lstUsers = null;

            m_webSocketOptions = null;
        }

        private async Task ProcessWebSocket(HttpContext context, Func<Task> next)
        {
            if (context.Request.Path.Equals("/ws"))
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    using (WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync())
                    {
                        await ProcessPacket(webSocket);
                    }
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else
            {
                await next();
            }
        }

        private async Task ProcessPacket(WebSocket webSocket)
        {
            WebSocketReceiveResult result = null;
            do
            {
                byte[] buffer = new byte[1024 * 1024 * 10];  // 1MB
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                string json = Encoding.UTF8.GetString(buffer);
                Packet packet = JsonConvert.DeserializeObject<Packet>(json);
                switch (packet.hd.iRmiID)
                {
                    case (int)E_RMIID.E_RMIID_REQ_LOGIN:
                        {
                            Req_Login req = JsonConvert.DeserializeObject<Req_Login>(packet.strJson);

                            long lUserNo = req.lUserNo;
                            ChatUser chatUser = getUser(lUserNo);
                            if (chatUser == null)
                            {
                                string strUserName = req.strUserName;

                                chatUser = createUser(lUserNo, strUserName, webSocket);
                                await chatUser.Do_Req_Login(req, result);
                            }
                            else
                            {

                            }
                        }
                        break;
                    case (int)E_RMIID.E_RMIID_REQ_CHAT:
                        {
                            Req_Chat req = JsonConvert.DeserializeObject<Req_Chat>(packet.strJson);

                            int cnt = m_lstUsers.Count;
                            for (int i = 0; i < cnt; ++i)
                            {
                                await m_lstUsers[i].Do_Req_Chat(req, result);
                            }
                        }
                        break;
                    default:
                        {

                        }
                        break;
                }
            } while(!result.CloseStatus.HasValue);
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        ChatUser getUser(long userNo)
        {
            lock (m_objLock)
            {
                int cnt = m_lstUsers.Count;
                for (int i = 0; i < cnt; ++i)
                {
                    if (m_lstUsers[i].UserNo == userNo)
                        return m_lstUsers[i];
                }
                return null;
            }
        }

        ChatUser createUser(long userNo, string strUserName, WebSocket webSocket)
        {
            lock (m_objLock)
            {
                ChatUser chatUser = new ChatUser(userNo, strUserName, webSocket);
                m_lstUsers.Add(chatUser);
                return chatUser;
            }
        }

        void disposeUser(ChatUser chatUser)
        {
            if (m_lstUsers.Contains(chatUser))
            {
                m_lstUsers.Remove(chatUser);
            }
        }
    }
}
