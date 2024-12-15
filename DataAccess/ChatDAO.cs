using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using BusinessObject.Dto.MessageChatDetail;
using AutoMapper.Execution;
using BusinessObject.Dto.Trainer;
using BusinessObject.Dto.Chat;

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

        //private ChatDAO() { }

        public async Task CreateChatAsync(int memberId, string createNewChat)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var newChat = new MessageChat
                    {
                        MemberId = memberId,

                        CreateAt = DateTime.UtcNow,
                        ContentStart=createNewChat,

                    };

                    context.MessageChats.Add(newChat);
                    await context.SaveChangesAsync();
                    var getChat = context.MessageChats.Where(c => c.MemberId == memberId && c.Status == true).FirstOrDefault();

                    
                    // return newChat;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating chat: {ex.Message}", ex);
            }
        }

        public async Task<List<GetMessageChatDetailDTO>> GetMemberChatDetailsAsync(int memberId, int chatId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var messageChatDetails = context.MessageChatDetails.Where(d => d.MessageChatId == chatId)
                        .Select(detail => new GetMessageChatDetailDTO
                        {
                            MessageChatDetailsId = detail.MessageChatDetailsId,
                            MessageContent = detail.MessageContent,
                            SentAt = detail.SentAt,
                            SenderType = detail.SenderType,

                        })
                        .ToList();

                    return messageChatDetails;
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
                        .Where(c => c.MemberId == memberId && c.Status == true)
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

        public async Task RateChatAsync(int memberId, int chatId, int rating)
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
                        SenderType = "2", // Loại người gửi (Staff)
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

        public async Task SendMessageMemberAsync(int memberId, int chatId, string messageContent)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var chat = await context.MessageChats
                        .FirstOrDefaultAsync(c => c.MessageChatId == chatId && c.MemberId == memberId);

                    if (chat == null)
                    {
                        throw new Exception("Chat not found or unauthorized.");
                    }


                    var message = new MessageChatDetail
                    {
                        MessageChatId = chatId,
                        SenderType = "1",

                        MessageContent = messageContent,
                        SentAt = DateTime.UtcNow
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

        public async Task<List<GetMessageChatDetailDTO>> GetAllMessageForTrainerToAsign(int chatId, int staffId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var messageChatDetails = context.MessageChatDetails.Where(d => d.MessageChatId == chatId)
                        .Select(detail => new GetMessageChatDetailDTO
                        {
                            MessageChatDetailsId = detail.MessageChatDetailsId,
                            MessageContent = detail.MessageContent,
                            SentAt = detail.SentAt,
                            SenderType = detail.SenderType,


                        })
                        .ToList();

                    return messageChatDetails;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving chat details: {ex.Message}", ex);
            }
        }

        public async Task<PagedResult<AllMessageChatDTO>> GetAllMessageChatForTrainerNeedAsign(int pageNumber, int pageSize)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var totalRecords = await context.MessageChats
                        .OrderBy(d => d.MessageChatId)
                        .Where(c => c.StaffId == null)
                        .CountAsync();

                    var chats = await context.MessageChats
                        .Where(c => c.StaffId == null)
                        .OrderBy(c => c.CreateAt)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .Select(c => new AllMessageChatDTO
                        {
                            MessageChatId = c.MessageChatId,
                            MemberId = c.MemberId,
                            CreateAt = c.CreateAt,
                            ContentStart=c.ContentStart,
                        })
                        .ToListAsync();

                    var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                    return new PagedResult<AllMessageChatDTO>
                    {
                        TotalRecords = totalRecords,
                        TotalPages = totalPages,
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        Data = chats
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving chats: {ex.Message}", ex);
            }
        }

        public async Task<bool> EndChatsAsync(int memberId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var getChat = context.MessageChats.Where(c => c.MemberId == memberId && c.Status == true).FirstOrDefault();
                    if (getChat == null) return false;


                    getChat.Status = false;
                   await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving chats: {ex.Message}", ex);
            }
        }

        public async Task<List<GetAllAccountTrainer>> GetAllTrainerToAssign()
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var getAllTrainer = context.staffs.Where(c => c.Role >1 && c.Status == true).
                        Select(c => new GetAllAccountTrainer
                        {
                            Value=c.StaffId,
                            Label=c.FullName,
                        })
                        .ToList();
                    if (getAllTrainer == null) return null;


                   
                    return getAllTrainer;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving chats: {ex.Message}", ex);
            }
        }

        public async Task<List<OverViewMessageDTO>> OverviewAllMessageOfTrainer(int staffId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                   
                    var messages = await context.MessageChats
                        .Where(chat => chat.StaffId == staffId) 
                        .Join(
                            context.Members,
                            chat => chat.MemberId,
                            member => member.MemberId,
                            (chat, member) => new OverViewMessageDTO
                            {
                                ChatId = chat.MessageChatId,
                                MemberId = chat.Member.MemberId,
                                ImageMember = member.ImageMember, 
                                ContentStart = chat.ContentStart, 
                                MessageContent = chat.ContentStart, 
                                
                            }
                        )
                        .ToListAsync();

                    return messages;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving chats: {ex.Message}", ex);
            }
        }


    }
}