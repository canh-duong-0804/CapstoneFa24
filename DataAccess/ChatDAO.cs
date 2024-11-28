using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;

namespace DataAccess
{
    public class ChatDAO
    {
        private static ChatDAO instance = null;
        private static readonly object instanceLock = new object();

        public static ChatDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ChatDAO();
                    }
                    return instance;
                }
            }
        }

        private ChatDAO() { }

        public async Task CreateChatAsync(int memberId, string initialMessage)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var newChat = new MessageChat
                    {
                        MemberId = memberId,
                        
                        CreateAt = DateTime.UtcNow,
                      
                    };

                    context.MessageChats.Add(newChat);
                    await context.SaveChangesAsync();

                   // return newChat;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating chat: {ex.Message}", ex);
            }
        }

        public async Task<MessageChat> GetMemberChatDetailsAsync(int memberId, int chatId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var chat = await context.MessageChats
                        .FirstOrDefaultAsync(c => c.MemberId == memberId && c.MessageChatId == chatId);

                    if (chat == null)
                        throw new Exception("Chat not found or unauthorized access.");

                    return chat;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving chat details: {ex.Message}", ex);
            }
        }

        public async Task<List<MessageChat>> GetMemberChatsAsync(int memberId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var chats = await context.MessageChats
                        .Where(c => c.MemberId == memberId)
                        .OrderByDescending(c => c.CreateAt)
                        .ToListAsync();

                    return chats;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving chats: {ex.Message}", ex);
            }
        }

        public async Task RateChatAsync(int memberId, int chatId, double rating)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var chat = await context.MessageChats
                        .FirstOrDefaultAsync(c => c.MemberId == memberId && c.MessageChatId == chatId);

                    if (chat == null)
                        throw new Exception("Chat not found or unauthorized access.");

                    chat.RateStar = rating;
                    //chat.Status = "Closed";

                    context.MessageChats.Update(chat);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error rating chat: {ex.Message}", ex);
            }
        }

        public async Task AssignStaffToChatAsync(int chatId, int staffId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                   
                    var chat = await context.MessageChats.FirstOrDefaultAsync(c => c.MessageChatId == chatId);
                    if (chat == null)
                    {
                        throw new Exception("Chat not found.");
                    }

                   
                    chat.StaffId = staffId;

                    
                    context.MessageChats.Update(chat);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error assigning staff to chat: {ex.Message}", ex);
            }
        }

        public async Task SendMessageAsync(int chatId, int staffId, string messageContent)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                   
                    var chat = await context.MessageChats.FirstOrDefaultAsync(c => c.MessageChatId == chatId);
                    if (chat == null)
                    {
                        throw new Exception("Chat not found.");
                    }

                   
                    if (chat.StaffId != staffId)
                    {
                        throw new Exception("You are not assigned to this chat.");
                    }

                    // Tạo một tin nhắn mới
                    var message = new MessageChatDetail
                    {
                        MessageChatId = chatId, // Gắn ID của cuộc trò chuyện
                        SenderType = "Staff", // Loại người gửi (Staff)
                        MessageContent = messageContent, // Nội dung tin nhắn
                        SentAt = DateTime.UtcNow // Thời gian gửi
                    };


                    context.MessageChatDetails.Add(message);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error sending message: {ex.Message}", ex);
            }
        }

    }
}
