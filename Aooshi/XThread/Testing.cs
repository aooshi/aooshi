using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Aooshi.XThread
{
    //class Testing
    //{
    //    static void Main(string[] args)
    //    {
    //        var thread = new Aooshi.XThread.XThreadPool(10);

    //        int i = 0;
    //        while (i++ < 100000000)
    //        {
    //            Thread.Sleep(50);
    //            thread.Start(i, new WaitCallback(Start));
    //        }
    //    }


     //static void Main(string[] args)
     //   {
     //       var thread = new Aooshi.XThread.XThreadPool(10);


     //       var ts = new Thread[]{
     //           new Thread(()=>{
     //               int i = 0;
     //               while (i++ < 100000000)
     //               {
     //                   //Thread.Sleep(10);
     //                   thread.Start("a" + i,  new WaitCallback(Start));
     //               }     
     //           }),

     //           new Thread(()=>{
     //               int i = 0;
     //               while (i++ < 100000000)
     //               {
     //                   //Thread.Sleep(10);
     //                   thread.Start("b" + i,  new WaitCallback(Start));
     //               }
     //           }),

     //           new Thread(()=>{
     //               int i = 0;
     //               while (i++ < 100000000)
     //               {
     //                   //Thread.Sleep(10);
     //                   thread.Start("c" + i,  new WaitCallback(Start));
     //               }
     //           }),

     //           new Thread(()=>{
     //               int i = 0;
     //               while (i++ < 100000000)
     //               {
     //                   //Thread.Sleep(10);
     //                   thread.Start("d" + i,  new WaitCallback(Start));
     //               }
     //           }),

     //           new Thread(()=>{
     //               int i = 0;
     //               while (i++ < 100000000)
     //               {
     //                   //Thread.Sleep(10);
     //                   thread.Start("e" + i,  new WaitCallback(Start));
     //               }
     //           }),

     //           new Thread(()=>{
     //               int i = 0;
     //               while (i++ < 100000000)
     //               {
     //                   //Thread.Sleep(10);
     //                   thread.Start("f" + i,  new WaitCallback(Start));
     //               }
     //           }),

     //           new Thread(()=>{
     //               int i = 0;
     //               while (i++ < 100000000)
     //               {
     //                   //Thread.Sleep(10);
     //                   thread.Start("g" + i,  new WaitCallback(Start));
     //               }
     //           }),

     //           new Thread(()=>{
     //               int i = 0;
     //               while (i++ < 100000000)
     //               {
     //                   //Thread.Sleep(10);
     //                   thread.Start("h" + i,  new WaitCallback(Start));
     //               }
     //           }),

     //           new Thread(()=>{
     //               int i = 0;
     //               while (i++ < 100000000)
     //               {
     //                   //Thread.Sleep(10);
     //                   thread.Start(i,  new WaitCallback(Start));
     //               }
     //           }),

     //           new Thread(()=>{
     //               int i = 0;
     //               while (i++ < 100000000)
     //               {
     //                   //Thread.Sleep(10);
     //                   thread.Start(i,  new WaitCallback(Start));
     //               }
     //           }),

     //           new Thread(()=>{
     //               int i = 0;
     //               while (i++ < 100000000)
     //               {
     //                   //Thread.Sleep(10);
     //                   thread.Start(i,  new WaitCallback(Start));
     //               }
     //           })
           
     //       };

     //       foreach (var t in ts)
     //       {
     //           t.Start();
     //       }


            //Thread.Sleep(5000);
            //thread.Dispose();

     //       Console.Read();
     //   }

    //    static void Start(object userdate)
    //    {
    //        Console.WriteLine("{0}:{1} ", Thread.CurrentThread.ManagedThreadId, userdate);
    //        Thread.Sleep(1000);
    //    }
    //}




}
