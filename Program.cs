using System;
using VkNet;
using VkNet.Model;

namespace Vk
{
    class Program
    {
        static void Main(string[] args)
        {
            ulong id = 1; // id group
            string token = "1"; // access token

            var vkApi = new VkApi();
            vkApi.Authorize(new ApiAuthParams()
            {
                AccessToken = token
            });

            var bot = new Api(vkApi, id);
            bot.Start();

            Console.ReadLine();
        }
    }
}
