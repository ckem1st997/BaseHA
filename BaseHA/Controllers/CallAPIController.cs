using BaseHA.Domain.Entity;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Share.BaseCore.Base;
using Share.BaseCore.Extensions;
using Share.BaseCore.IRepositories;
using System.Net.Http.Headers;

namespace BaseHA.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class CallAPIController : ControllerBase
    {
        private readonly IRepositoryEF<Domain.Entity.Chat> _generic;
        private readonly IRepositoryEF<QnA> _genericq;

        public CallAPIController()
        {
            _generic = _generic = EngineContext.Current.Resolve<IRepositoryEF<Domain.Entity.Chat>>(DataConnectionHelper.ConnectionStringNames.Warehouse);
            _genericq = EngineContext.Current.Resolve<IRepositoryEF<QnA>>(DataConnectionHelper.ConnectionStringNames.Warehouse);
        }

        [HttpGet("call-api1")]
        public async Task<IActionResult> IndexAsync1111()
        {
            var list = _generic.GetList(x => x.Id != null && x.MessageIndex !=null && !x.Content.Contains("thoát hộp thoại")).OrderBy(x => x.ConversationId).ThenBy(x => x.MessageIndex);
            //foreach (var item in list.)
            //{
                
            //}
            return Ok(list);
        }


            [HttpGet("call-api")]
        public async Task<IActionResult> IndexAsync()
        {
            var date = new DateTime(2021, 3, 1);
            string dateTo = "";
            string dateFrom = "";
            var listChat = new List<Domain.Entity.Chat>();

            while (date <= DateTime.Now)
            {
                dateTo = date.ToString("yyyy-MM-dd");
                dateFrom = date.AddDays(29).ToString("yyyy-MM-dd");
                Console.Write(dateTo);

                string apiUrl = "https://api.caresoft.vn/hanoicomputer/api/v1/chats/messages?start_time_since=" + dateTo + "T00:00:00Z&conserstation_type=3&start_time_to=" + dateFrom + "T00:00:00Z"; // Điền URL của API vào đây
                string accessToken = "1FRvLEuRQgf5gdE"; // Điền access token của bạn vào đây

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    Console.WriteLine(response.StatusCode);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(responseData);
                        if (myDeserializedClass != null && myDeserializedClass.code.Equals("ok"))
                        {
                            foreach (var item in myDeserializedClass.chats)
                            {
                                await _generic.AddAsync(new Domain.Entity.Chat()
                                {
                                    Id=item.id.ToString(),
                                    Content = item.content,
                                    ConversationId = item.conversation_id,
                                    ConversationType = item.conversation_type,
                                    LastAgentUserId = item.last_agent_user_id,
                                    MessageIndex = item.message_index,
                                    RequesterId = item.requester_id,
                                    SenderAgentId = item.sender_agent_id,
                                    SenderAgentName = item.sender_agent_name,
                                    SenderVisitorId = item.sender_visitor_id,
                                    SenderVisitorName = item.sender_visitor_name,
                                    MsgId = item.msg_id,
                                    ServiceId = item.service_id,
                                    StartTime = item.start_time,
                                    TicketId = item.ticket_id,
                                    Time = item.time,
                                    Type = item.type
                                });
                            }
                        }


                    }
                }
                date = date.AddDays(29);
            }
            var res = await _generic.SaveChangesConfigureAwaitAsync();
            return Ok(res);
        }


        public class Chatha
        {
            public string? conversation_id { get; set; }
            public int? id { get; set; }
            public string? msg_id { get; set; }
            public int? message_index { get; set; }
            public string? content { get; set; }
            public string? time { get; set; }
            public int? service_id { get; set; }
            public string? start_time { get; set; }
            public string? sender_agent_name { get; set; }
            public int? sender_agent_id { get; set; }
            public string? sender_visitor_name { get; set; }
            public int? sender_visitor_id { get; set; }
            public int? last_agent_user_id { get; set; }
            public int? ticket_id { get; set; }
            public int? type { get; set; }
            public int? conversation_type { get; set; }
            public int? requester_id { get; set; }
        }

        public class Root
        {
            public string? code { get; set; }
            public int? numFound { get; set; }
            public List<Chatha> chats { get; set; }
        }
    }
}
