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
            byte[] buffer = new byte[1024 * 1024];  // 1MB
            ArraySegment<byte> segmentBuffer = new ArraySegment<byte>(buffer);
            long lUserNo = 0L;
            string strUserName = string.Empty;
            do
            {
                string json = string.Empty;
                Packet packet = null;
                WebSocketState webSocketState = webSocket.State;
                if (webSocketState == WebSocketState.Open)
                {
                    try
                    {
                        result = await webSocket.ReceiveAsync(segmentBuffer, CancellationToken.None);
                        json = Encoding.UTF8.GetString(segmentBuffer.Array, 0, result.Count);
                        packet = JsonConvert.DeserializeObject<Packet>(json);
                    }
                    catch (Exception ex)
                    {
                        Log.Write("#### Exception!!! THROWN ####");
                        Log.Write(ex.Message);
                        Log.Write(ex.StackTrace);
                        Log.Write("#### Exception!!! THROWN ####");

                        continue;
                    }

                    switch (packet.hd.iRmiID)
                    {
                        case (int)E_RMIID.E_RMIID_REQ_LOGIN:
                            {
                                Req_Login req = JsonConvert.DeserializeObject<Req_Login>(packet.strJson);

                                lUserNo = req.lUserNo;
                                ChatUser chatUser = getUser(lUserNo);
                                if (chatUser == null)
                                {
                                    strUserName = req.strUserName;

                                    chatUser = createUser(lUserNo, strUserName, webSocket);
                                    await chatUser.Do_Req_Login(req);
                                }
                                else
                                {

                                }
                            }
                            break;
                        case (int)E_RMIID.E_RMIID_REQ_CHAT:
                            {
                                Req_Chat req = JsonConvert.DeserializeObject<Req_Chat>(packet.strJson);

                                Log.Write("[{0}/{1}] 채팅 브로드 캐스팅 시작...", lUserNo, strUserName);
                                int cnt = m_lstUsers.Count;
                                for (int i = 0; i < cnt; ++i)
                                {
                                    Log.Write("[{0}/{1}]({2}/{3}) 채팅 브로드 캐스팅 중...", lUserNo, strUserName, i, cnt);
                                    await m_lstUsers[i].Do_Req_Chat(req);
                                }
                                Log.Write("[{0}/{1}] 채팅 브로드 캐스팅 끝...", lUserNo, strUserName);
                            }
                            break;
                        default:
                            {

                            }
                            break;
                    }
                }
                else if(webSocketState == WebSocketState.Aborted)
                {
                    break;
                }
            } while (!result.CloseStatus.HasValue);

            disposeUser(lUserNo);

            if (result.CloseStatus.HasValue)
                await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            
            webSocket.Dispose();
        }

        ChatUser getUser(long lUserNo)
        {
            lock (m_objLock)
            {
                int cnt = m_lstUsers.Count;
                for (int i = 0; i < cnt; ++i)
                {
                    if (m_lstUsers[i].UserNo == lUserNo)
                        return m_lstUsers[i];
                }
                return null;
            }
        }

        ChatUser createUser(long lUserNo, string strUserName, WebSocket webSocket)
        {
            lock (m_objLock)
            {
                ChatUser chatUser = new ChatUser(lUserNo, strUserName, webSocket);
                m_lstUsers.Add(chatUser);

                Log.Write("[{0}/{1}] 유저 객체 생성 완료", lUserNo, strUserName);

                return chatUser;
            }
        }

        void disposeUser(long lUserNo)
        {
            lock (m_objLock)
            {
                ChatUser user = getUser(lUserNo);
                if (m_lstUsers.Contains(user))
                {
                    m_lstUsers.Remove(user);

                    Log.Write("[{0}/{1}] 유저 객체 제거 완료", lUserNo, user.UserName);
                }
            }
        }
    }
}
