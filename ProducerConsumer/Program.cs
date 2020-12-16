using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    class Program
    {
        // old product queue
        static Queue<string> OldProductQueue = new Queue<string>();
        // new product list
        static List<string> FreshProductList = new List<string>();

        // method to dequeue product from queue
        static void GetProduct()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                lock (OldProductQueue)
                {
                    while (OldProductQueue.Count == 0)
                    {
                        Console.WriteLine($"Consumer is waiting there is {OldProductQueue.Count} left");
                        Monitor.PulseAll(OldProductQueue);
                        Monitor.Wait(OldProductQueue);
                    }

                    Console.WriteLine("Consumer is consuming " + OldProductQueue.Dequeue());
                }

                Thread.Sleep(2000);
            }
        }

        // method to refillProductqueue
        static void RefillProductQueue()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                int count = 0;
                int maxProductToFill = 10;
                if (OldProductQueue.Count == 0)
                {
                    lock (OldProductQueue)
                    {
                        if (FreshProductList.Count != 0)
                        {
                            foreach (string item in FreshProductList)
                            {
                                Console.WriteLine("Producer is producing " + item);
                                OldProductQueue.Enqueue(item);
                            }
                            Monitor.PulseAll(OldProductQueue);
                        }

                        else if (FreshProductList.Count == 0)
                        {
                            while (FreshProductList.Count < 5)
                            {
                                count++;
                                string newProduct = "Product count : " + count.ToString();
                                FreshProductList.Add(newProduct);
                            }
                        }
                        else if (OldProductQueue.Count > 1)
                        {
                            Monitor.Wait(OldProductQueue);
                        }
                    }

                }
                Thread.Sleep(2000);
            }
        }
        static void Main(string[] args)
        {
            Thread t1 = new Thread(RefillProductQueue);
            Thread t2 = new Thread(GetProduct);

            t1.Start();
            t2.Start();
        }
    }
}
