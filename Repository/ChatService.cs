using Chat_With_Polling_SignalR_DotNetCore_MySQL.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Chat_With_Polling_SignalR_DotNetCore_MySQL.Repository
{
    public class ChatService : IChatService
    {
        private readonly IConfiguration _configuration;
        private static string connectionString;
        public ChatService(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetSection("ConnectionString").GetSection("MySqlConnString").Value;
        }

        public Task<bool> SaveMessage(ChatMessage msg)
        {
            string sqlInsert = null;
            try
            {
                sqlInsert = "INSERT INTO CHAT_MESSAGES(USERID, MESSAGE, CONNECTIONID) VALUES('"+msg.UserId+"', '"+msg.Message+"', '"+msg.Connectionid+"')";

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    SqlMapper.Query(conn, sqlInsert, commandType: CommandType.Text);

                }
                return Task.FromResult(true); ;
            }
            catch (Exception ex)
            {
                //log.Error("Error with SaveMessage, CONNECTIONID: " + msg.Connectionid + "\n\r" + ex);
                return Task.FromResult(false);
            }
        }

        public Task<string> GenerateConnectionId(string senderId, string receiverId)
        {
            string sqlInsert;
            string connectionId = null;
            try
            {
                connectionId = senderId.Substring(0, 5).ToUpper() + "" + receiverId.Substring(0,5).ToUpper() + DateTime.Now.ToString("yyyyMMddHHmmss");
                sqlInsert = "INSERT INTO CHAT_CONNECTION_ID(USERID_1, USERID_2, CONNECTIONID) VALUES('" + senderId + "', '" + receiverId + "', '"+ connectionId + "')";
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    SqlMapper.Query(conn, sqlInsert, commandType: CommandType.Text);

                }
                return Task.FromResult(connectionId); ;
            }
            catch (Exception ex)
            {
                //log.Error("Error with GenerateConnectionId, CONNECTIONID: " + connectionId + "\n\r" + "sqlInsert: " + sqlInsert + "\n\r" + ex);
                return Task.FromResult(connectionId);
            }
        }

        public Task<List<ChatMessage>> QueryChatMessages(string connectionId)
        {
            var responses = new List<ChatMessage>();
            string sqlSelect = "SELECT * FROM CHAT_MESSAGES WHERE CONNECTIONID = '"+ connectionId + "' AND DEL_FLG = 'N' ORDER BY ID ASC";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    responses = SqlMapper.Query<ChatMessage>(conn, sqlSelect, commandType: CommandType.Text).ToList();
                }
                return Task.FromResult(responses);
            }
            catch (Exception ex)
            {
                //log.Error("Error with QueryChatMessages, CONNECTIONID: " + connectionId + "\n\r" + ex);
                responses = new List<ChatMessage>();
            }
            return Task.FromResult(responses);
        }

        public Task<string> QueryConnectionId(string senderId, string receiverId)
        {
            string responses;
            string sqlSelect = "SELECT CONNECTIONID FROM CHAT_CONNECTION_ID WHERE (USERID_1 = '" + senderId + "' AND USERID_2 = '" + receiverId + "') " +
                                                                   "OR (USERID_1 = '" + receiverId + "' AND USERID_2 = '" + senderId + "') AND DEL_FLG = 'N'";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    responses = SqlMapper.Query<string>(conn, sqlSelect, commandType: CommandType.Text).FirstOrDefault();
                }
                return Task.FromResult(responses);
            }
            catch (Exception ex)
            {
                //log.Error("Error with QueryChatMessages, CONNECTIONID: " + connectionId + "\n\r" + ex);
                responses = null;
            }
            return Task.FromResult(responses);
        }
    }
}
