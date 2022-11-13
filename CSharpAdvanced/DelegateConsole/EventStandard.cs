using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegateConsole
{
    public class EventStandard
    {
        // 发布者的实例
        private static PublicNumber publicNumber = new PublicNumber()
        {
            Id = 1,
            Name = ".net社区"
        };

        public static void Show()
        {
            publicNumber.PublishShow();
        }

        /// <summary>
        /// 订阅：关联发布者与订阅者之间的关系
        /// </summary>
        public static void Init()
        {
            ///订阅者的实例
            User user = new User()
            {
                Id = 234,
                Name = "1"
            };

            //订阅者的实例
            Neter neter = new Neter()
            {
                Id = 345,
                Name = "摸鱼人"
            };
            //建立发布者和订阅者之间的关系
            publicNumber.Push += user.Read;
            publicNumber.Push += neter.Learn;
        }
    }

    /// <summary>
    /// 公众号
    /// 发布者：当触发一个动作后，触发这个事件
    /// </summary>
    public class PublicNumber
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public void PublishShow()
        {
            Console.WriteLine("发布了一篇文章...");

            Push?.Invoke(this, new MessageInfo()
            {
                Id =123,
                Title ="net",
                Description = "DescriptionDescriptionDescriptionDescriptionDescription",
            });
        }

        public event EventHandler? Push;
    }

    /// <summary>
    /// 用户
    /// 订阅者：对发布者发布的事件关注
    /// 关注公众号
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public void Read(object? sender,EventArgs e)
        {
            MessageInfo info = (MessageInfo)e;
            Console.WriteLine(info.Id);
            Console.WriteLine(Id);
            Console.WriteLine("收到了....");
        }
    }

    /// <summary>
    /// .net开发者
    /// 订阅者：对发布者发布的事件关注
    /// 关注公众号
    /// </summary>
    public class Neter
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public void Learn(object? sender, EventArgs e)
        {
            MessageInfo info = (MessageInfo)e;
            Console.WriteLine(info);
            Console.WriteLine("学废了...");
        }
    }

    public class MessageInfo : EventArgs
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    } 
}
