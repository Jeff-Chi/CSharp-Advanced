using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegateConsole
{
    /// <summary>
    /// 实例2： 商家--平台--顾客
    /// </summary>
    public class EventStandard2
    {
        public static void Show()
        {
            Seller frameworkClass = new Seller()
            {
                Id = 1,
                Name = "笔记本电脑",
                Price = 2100
            };
            //订阅：订阅者和事件发布者关联
            frameworkClass.PriceIncrease += new Customer().Buy;
            frameworkClass.PriceIncrease += new Taobao().PushMessage;
            frameworkClass.Price = 6299;
        }
    }

    /// <summary>
    /// 发布者： 满足特定条件发布事件
    /// </summary>
    public class Seller
    {
        public event EventHandler? PriceIncrease;
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        private int _price = 5299;
        public int Price
        {
            get { return _price; }
            set
            {
                if (value > _price)
                {
                    Console.WriteLine("价格变化事件触发");
                    PriceIncrease?.Invoke(this, new SellerEventArge
                    {
                        OldPrice = _price,
                        NewPrice = value
                    });
                }

                _price = value;
            }
        }
    }

    /// <summary>
    /// 订阅者: 关注事件，事件触发后，自己做出相应的动作
    /// </summary>
    public class Customer
    {
        public void Buy(object? sender, EventArgs e)
        {
            Seller seller = (Seller)sender!;
            SellerEventArge sellerEventArge = (SellerEventArge)e;
            Console.WriteLine($"this is {seller.Name}");
            Console.WriteLine($"{seller.Name}之前的价格是{sellerEventArge.OldPrice}");
            Console.WriteLine($"{seller.Name}现在要马上更新的价格是{sellerEventArge.NewPrice}");
            Console.WriteLine(".........买不买....................");
            Console.WriteLine($"{seller.Name}现在要马上更新的价格是{sellerEventArge.NewPrice}");
        }
    }

    /// <summary>
    /// 订阅者
    /// </summary>
    public class Taobao
    {
        public void PushMessage(object? sender,EventArgs eventArgs)
        {
            Seller seller = (Seller)sender!;
            SellerEventArge sellerEventArge = (SellerEventArge)eventArgs;
            Console.WriteLine($"this is {seller.Name}");
            Console.WriteLine($"{seller.Name}之前的价格是{sellerEventArge.OldPrice}");
            Console.WriteLine($"{seller.Name}现在要马上更新的价格是{sellerEventArge.NewPrice}");
            Console.WriteLine("消息推送....来一波广告");
        }
    }

    /// <summary>
    /// 事件传递的参数
    /// </summary>
    public class SellerEventArge : EventArgs
    {
        public int OldPrice { get; set; }
        public int NewPrice { get; set; }
    }
}
