using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.GroupUpdate;

namespace Vk
{
    class Api
    {
        private VkApi ApiProvider;
        private int LastTS;
        private ulong GroupId;

        public Api(VkApi api, ulong id)
        {
            this.GroupId = id;
            this.ApiProvider = api;
        }

        public void Start()
        {
            var group = ApiProvider.Groups.GetLongPollServer(GroupId);
            LastTS = int.Parse(group.Ts);

            while (true)
            {
                var actions = ApiProvider.Groups.GetBotsLongPollHistory(new VkNet.Model.RequestParams.BotsLongPollHistoryParams()
                {
                    Key = group.Key,
                    Server = group.Server,
                    Ts = LastTS.ToString()
                });

                foreach (var action in actions.Updates) new Thread(() => Handler(action)).Start();

                LastTS++;
            }
        }

        private void Handler(GroupUpdate update)
        {
            if (update.Type != GroupUpdateType.MessageNew) return;

            var message = update.MessageNew.Message.Text;
            var id = update.MessageNew.Message.PeerId;

            Console.WriteLine("У вас новое сообщение, ебать его в сраку. Епт, звучит оно так: " + message);
            switch (message)
            {
                case "хуй":
                    Send("пизда", id);
                    break;
            }
        }

        private void Send(string text, long? id)
        {
            if (text == null) return;
            ApiProvider.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams()
            {
                Message = text,
                PeerId = id,
                RandomId = new Random().Next(1, 50)
            });
        }
    }
}
